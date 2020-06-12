using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class LoadingScreen : Singleton<LoadingScreen>
{
	[SerializeField]
	private Transform     lscreen  = null;
	[SerializeField]
	private UIProgressBar pbar     = null;
    [SerializeField]
	private Vector3       pos_show = new Vector3( 0, 0, -998 );
    [SerializeField]
	private Vector3       pos_hide = new Vector3( 0, 0, 3000 );

	//****************************************************************
	public void _Start()
	{
		lscreen.transform.localPosition = pos_hide;
		lscreen.gameObject.SetActive( false );
	}
	
	//****************************************************************
	public void Progress( float val )
	{
		pbar.Value = val;
	}

	//****************************************************************
	public void Show()
	{
		lscreen.gameObject.SetActive( true );
		lscreen.transform.localPosition = pos_show;
	}

	//****************************************************************
	public void Hide()
	{
		lscreen.transform.localPosition = pos_hide;
		lscreen.gameObject.SetActive( false );
	}

    //****************************************************************
	//float p = 0;
	//private void OnGUI()
	//{
	//    if( GUI.Button( new Rect( 20, 20, 200, 30 ), "Progress" ) )
	//    {
	//        this.Progress( p );
	//        p += 0.1f;
	//    }
	//}
}
