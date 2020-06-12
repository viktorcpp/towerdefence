#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using DateTime   = System.DateTime;
using TimeSpan   = System.TimeSpan;
using SceneType  = defines.SceneType;
using WeaponArea = defines.WeaponArea;

public class Core : Singleton<Core>
{
	[SerializeField]
	private SpriteText   debug       = null;
	private bool         initialized = false;
    private List<string> scenes      = new List<string>();

	//****************************************************************
	public void Start()
	{
		if( initialized ) return;

        scenes.Add( "loader"       );
        scenes.Add( "main-menu"    );
        scenes.Add( "map-selector" );
        scenes.Add( "map-atol"     );

        Core.DebugClear();

        Options            .Instance._Start();
		LoadingScreen      .Instance._Start();
        BuildVersion       .Instance._Start();
		LangController     .Instance._Start();
		FPSOutput          .Instance._Start();
        ItemsController    .Instance._Start();
        MapController      .Instance._Start();
        UnitsController    .Instance._Start();
        BuildingController .Instance._Start();

		Player             .Instance._Start();

		initialized = true;

		UIManager.instance.blockInput = false;

        this.OnLevelWasLoaded();
	}

    //****************************************************************
    public static List<string> GetScenes
    {
        get{ return Core.Instance.scenes; }
    }

    //****************************************************************
    public void LoadScene( string scene_name = "main-menu" )
    {
        StartCoroutine( _CorLoadScene( scene_name ) );
    } private IEnumerator _CorLoadScene( string scene_name = "main-menu" )
    {
        UIManager.instance.blockInput = true;
        LoadingScreen.Instance.Show();

        AsyncOperation ao = null;

        ao = Application.LoadLevelAsync( scene_name );

        while( !ao.isDone )
        {
            yield return new WaitForSeconds( 0.05f );
            LoadingScreen.Instance.Progress( ao.progress );
        }

        yield return new WaitForSeconds( 0.5f );

        LoadingScreen.Instance.Hide();
        UIManager.instance.blockInput = false;
    }

	//****************************************************************
	private void OnLevelWasLoaded()
	{
        Utils.TranslateStaticAll();
		this.InitScene();
	}

	//****************************************************************
    private void Awake()
    {
		Screen.sleepTimeout = 0;

		DontDestroyOnLoad( gameObject );
    }

	//****************************************************************
	public static object Log
	{
		set
		{
			Core.Instance.debug.Text += value + "\n";

			UnityEngine.Debug.Log( value );
		}
	}

	//****************************************************************
	public static void DebugClear()
	{
		Core.Instance.debug.Text = string.Empty;
	}

	//****************************************************************
	private void InitScene()
	{
		switch( SceneInfo.SceneType )
        {
        case SceneType.LOADER:
            this.LoadScene();
            break;

        case SceneType.MAIN_MENU:
            SoundController.Instance.Play_SoundMainTheme();
            break;

        case SceneType.MAP_SELECTOR:
            MapSelector.Instance._Start();
            SoundController.Instance.Play_SoundMapSelectorRnd();
            break;

        case SceneType.BATTLE:
            BattlefieldController.Instance._Start();
            SoundController.Instance.Play_SoundBattleThemeRnd();
            break;
        }
	}

    //****************************************************************
    private void OnGUI()
    {
        if( GUI.Button( new Rect( 10, Screen.height - 120, 80, 20 ), "Clear Log" ) )
        {
            Core.DebugClear();
        }
    }
}

public class Utils
{
    //****************************************************************
    public static string GetPathSaveFolderEditorMode()
    {
        return Application.dataPath.Substring( 0, Application.dataPath.IndexOf( "Assets" ) ) + "save";
    }

    //****************************************************************
    public static void TranslateStaticAll()
    {
        Translatable[] tlist = Utils.FindCompList<Translatable>();
        foreach( Translatable t in tlist )
        {
            t.Translate();
        }
    }

    //****************************************************************
    public static void Translate( Transform t_cont )
    {
        Translatable[] tr = t_cont.GetComponentsInChildren<Translatable>();
        foreach( Translatable t in tr )
        {
            t.Translate();
        }
    }

    //****************************************************************
    public static T FindComp<T>()
    {
        return (T)( (object) GameObject.FindObjectOfType( typeof(T) ) );
    }

    //****************************************************************
    public static T[] FindCompList<T>()
    {
        return (T[])( (object) GameObject.FindObjectsOfType( typeof(T) ) );
    }

	//****************************************************************
	public static GameObject FindGO( string name )
	{
		return GameObject.Find( name );
	}

    //****************************************************************
	public static XmlDocument LoadXml( string path )
	{
		TextAsset   text = null;
		XmlDocument doc  = null;

		text = ( TextAsset )Resources.Load( path, typeof( TextAsset ) );
		doc  = new XmlDocument();

		doc.LoadXml( text.text );

		return doc;
	}

    //****************************************************************
	public static XmlDocument LoadXmlStr( string xml_str )
	{
		XmlDocument doc  = new XmlDocument();

		doc.LoadXml( xml_str );

		return doc;
	}

	//****************************************************************
	public static Transform LoadPrefab( string prefab_name, string prefab_prefix = "", Transform parent = null )
	{
		GameObject o = null;
		o      = ( GameObject )Resources.Load( "prefabs/" + prefab_prefix + prefab_name, typeof( GameObject ) );
		o      = ( GameObject )MonoBehaviour.Instantiate( o );
		o.name = prefab_name;

		if( parent != null )
		{
			o.transform.parent        = parent;
			o.transform.localPosition = Vector3.zero;
			o.transform.localScale    = Vector3.one;
		}

		return o.transform;
	}

	//****************************************************************
	public static Transform LoadPrefabSafe( string prefab_name, string prefab_prefix = "", Transform parent = null )
	{
		GameObject o   = Utils.FindGO( prefab_name );
		Transform  ret = null;

        ret = o == null ? Utils.LoadPrefab( prefab_name, prefab_prefix, parent ) : o.transform;

		return ret;
	}

    //****************************************************************
    public static T CloneGO<T>( GameObject go_tmplt )
    {
        return (T)(object)( (GameObject)MonoBehaviour.Instantiate( go_tmplt ) ).GetComponent( typeof( T ).Name ).GetType();
    }

    //****************************************************************
    public static void ClearChilds( Transform t )
    {
        while( t.childCount > 0 )
        {
            MonoBehaviour.DestroyImmediate( t.GetChild( 0 ).gameObject );
        }
    }

    //****************************************************************
    public static float Random( float min, float max )
    {
        return UnityEngine.Random.Range( min, max );
    }

    //****************************************************************
    public static int Random( int min, int max )
    {
        return UnityEngine.Random.Range( min, max );
    }

    //****************************************************************
    /// <summary>
    /// random array shuffle
    /// </summary>
    /// <param name="array">array for shuffle</param>
    /// <returns>shuffled array</returns>
    public static T[] ArrayShuffle<T>( T[] array )
    {
        return (T[]) array.OrderBy( i => Utils.Random( 0, array.Length ) ).ToArray();
    }

    //****************************************************************
    public static string FormatTimeA( int seconds )
    {
        string ret = string.Empty;

        TimeSpan ts = new TimeSpan( 0, 0, seconds );
        ret += string.Format( "{0:00}:{1:00}", ts.Minutes, ts.Seconds );

        return ret;
    }

    //****************************************************************
    public static bool IsPlayerMapDone( string map_id )
    {
        return Game.Player.GetPlayerMapDoneList.Find( delegate( string s ){ return s == map_id; } ) != null ? true : false;
    }

    //****************************************************************
    public static string FormatTimeString( int seconds )
    {
        TimeSpan ts = new TimeSpan( 0, 0, seconds );
        return string.Format( "{0:00}:{1:00}", ts.Minutes, ts.Seconds );
    }

    //****************************************************************
    public static WeaponArea GetWeaponArea( string wpn_area )
    {
        WeaponArea ret = WeaponArea.NULL;

        wpn_area = wpn_area.ToUpper();

        if( wpn_area == WeaponArea.AREA_1.ToString() )
        {
            ret = WeaponArea.AREA_1;
        }
        else if( wpn_area == WeaponArea.AREA_2_H.ToString() )
        {
            ret = WeaponArea.AREA_2_H;
        }
        else if( wpn_area == WeaponArea.AREA_2_V.ToString() )
        {
            ret = WeaponArea.AREA_2_V;
        }
        else if( wpn_area == WeaponArea.AREA_4.ToString() )
        {
            ret = WeaponArea.AREA_4;
        }
        else if( wpn_area == WeaponArea.AREA_6_H.ToString() )
        {
            ret = WeaponArea.AREA_6_H;
        }
        else if( wpn_area == WeaponArea.AREA_6_V.ToString() )
        {
            ret = WeaponArea.AREA_6_V;
        }
        else if( wpn_area == WeaponArea.AREA_9.ToString() )
        {
            ret = WeaponArea.AREA_9;
        }

        return ret;
    }
}

public class Timer
{
	//****************************************************************
	public string FormatStringA( TimeSpan tspan )
	{
		string ret = string.Format( "{0:00}:{1:00}", tspan.Minutes, tspan.Seconds );

		return ret;
	}

	//****************************************************************
	// returns current time
	public TimeSpan GetTimeCurr
	{
		get
		{
			return new TimeSpan( DateTime.Now.Ticks );
		}
	}

	//****************************************************************
	// return (current time + add seconds time)
	public TimeSpan GetTimeNext( float add_seconds )
	{
		return new TimeSpan( DateTime.Now.AddSeconds( add_seconds ).Ticks );
	}

	//****************************************************************
	// returns time before next
	public TimeSpan GetTimeDiff( TimeSpan next )
	{
		return new TimeSpan( next.Ticks - DateTime.Now.Ticks );
	}
}
