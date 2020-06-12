using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class CoreHelper : MonoBehaviour
{
    private bool started = false;

	//****************************************************************
	public void Start()
	{
        if( started )  return;

        Core c = (Core)GameObject.FindObjectOfType( typeof(Core) );

        if( c != null )
        {
            Core.Instance.Start();
        }
        else
        {
            GameObject o = null;

            o = (GameObject)Resources.Load( "prefabs/!core", typeof( GameObject ) );
            o = (GameObject)Instantiate( o );
            o.name = "!core";
        }

        started = true;
	}

#region Context Menu
#if UNITY_EDITOR
	//****************************************************************
	[ContextMenu( "Load Core" )]
	private void Editor_CoreLoad()
	{
		GameObject go = GameObject.Find( "!core" );
		if( go == null )
		{
			go = (GameObject)Resources.Load( "prefabs/!core" );
			go = (GameObject)PrefabUtility.InstantiatePrefab( go );
		}
		go.name = "!core";
	}
#endif
#endregion
}
