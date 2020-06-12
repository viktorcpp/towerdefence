using UnityEngine;
using UnityEditor;
using System;
using System.IO;

using DateTime = System.DateTime;
using TimeSpan = System.TimeSpan;

public class BuildCustom : EditorWindow
{
	// MyGang.YY.MM.DD    - AM
	// MyGang.YY.MM.DD.HH - PM
	private static BuildCustom curr_component    = null;
	private string             path              = "Assets/Resources/xml/!build-version.xml";
	private string             build_version_str = string.Empty;
	private long               build_version_d   = 0;
	private string             error             = string.Empty;
	private string             str_save          = string.Empty;
	private string             str_file_name     = string.Empty;

	//****************************************************************
	[MenuItem("TowerDefence Utils/===> BUILD APK <===", false, 20)]
	public static void init()
	{
		curr_component = EditorWindow.GetWindow<BuildCustom>();
        curr_component._Build();
	}

	//****************************************************************
	private void _Build()
	{
		string[] levels = new string[ EditorBuildSettings.scenes.Length ];
		for( int x = 0; x < EditorBuildSettings.scenes.Length; x++ )
		{
			levels[ x ] = EditorBuildSettings.scenes[ x ].path;
		}
		curr_component.Close();

		this._UpdateBuildTimestamp();

		EditorApplication.OpenScene( levels[ 0 ] );
		error = BuildPipeline.BuildPlayer( levels , "!builds/" + str_file_name + ".apk", BuildTarget.Android, BuildOptions.None );
		if( error != string.Empty )
		{
		    MonoBehaviour.print( error );
		    error = string.Empty;
		}
		//EditorApplication.OpenScene( levels[ 1 ] );
		if( error != string.Empty )
		{
		    MonoBehaviour.print( "Build success!" );
		    error = string.Empty;
		}

		MonoBehaviour.print( "Build finished. Version: " + build_version_d  );
	}

	//****************************************************************
	private void _UpdateBuildTimestamp()
	{
		// load xml
		string[] lines = File.ReadAllLines( path );
		build_version_d = long.Parse( lines[ 1 ] );
		++build_version_d;

		int year  = DateTime.Now.Year - 2000;
		int month = DateTime.Now.Month;
		int day   = DateTime.Now.Day;
		int hour  = DateTime.Now.Hour;
		
		build_version_str = string.Format( "{0:0000}", build_version_d );

		str_save      = "Sophi Adventures." + build_version_str + "." + year + "." + month + "." + day;
		str_save     += hour > 12 ? "." + hour : "";
		str_file_name = str_save;
		str_save     += "\n" + build_version_d;
		File.WriteAllText( path, str_save );

		TextAsset ta = (TextAsset)Resources.Load( "xml/!build-version" );
		AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( ta.GetInstanceID() ), ImportAssetOptions.ForceUpdate );
		
		MonoBehaviour.print( "Build timestamp updated: " + str_save + " Build version: " + build_version_d );
	}

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}
