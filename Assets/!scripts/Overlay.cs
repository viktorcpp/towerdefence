#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class Overlay : Singleton<Overlay>
{
	[SerializeField]
	private Transform scene_object = null;
    [SerializeField]
	private Vector3   pos_show     = new Vector3( 0, 0, -500 );
    [SerializeField]
	private Vector3   pos_hide     = new Vector3( 0, 0, 3000 );

    //****************************************************************
    public Transform GetSceneObject
    {
        get{ return scene_object; }
    }
    public float PosShowZ
    {
        set{ pos_show.z = value; }
    }

	//****************************************************************
	public static void Show()
	{
		Overlay o                    = Overlay.Instance;
        o.scene_object.gameObject.SetActive( true );
		o.scene_object.localPosition = o.pos_show;
	}
	
	//****************************************************************
	public static void Hide()
	{
		Overlay o                    = Overlay.Instance;
		o.scene_object.localPosition = o.pos_hide;
        o.scene_object.gameObject.SetActive( false );
	}

	//****************************************************************
	public void Start()
	{
		scene_object.localPosition = pos_hide;
        scene_object.gameObject.SetActive( false );
	}
}
