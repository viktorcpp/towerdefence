using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class BuildVersion : Singleton<BuildVersion>
{
    [SerializeField]
	private SpriteText label    = null;
    private bool       locked   = false;
    private Vector3    pos_show = Vector3.zero;
    private Vector3    pos_hide = Vector3.zero;

    //****************************************************************
    public bool  Lock
    {
        set
        {
            locked = value;
            if( locked )
            {
                label.transform.localPosition = pos_hide;
            }
            else
            {
                label.transform.localPosition = pos_show;
            }
        }
    }

	//****************************************************************
	public void _Start()
	{
        pos_show   = label.transform.localPosition;
        pos_hide   = label.transform.localPosition;
        pos_hide.z = 3000.0f;

        if( !Options.Instance.ValueB( "is-dev-showversion" ) )
        {
            this.Lock = true;
            return;
        }

		TextAsset text = ( TextAsset )Resources.Load( "xml/!build-version" );
		string[] str = text.text.Split( '\n' );
		label.Text = str[ 0 ];
	}
}
