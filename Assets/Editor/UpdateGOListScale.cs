using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UpdateGOListScale : EditorWindow
{
	private static UpdateGOListScale curr_component = null;

	string str_x = string.Empty;
	string str_y = string.Empty;
	string str_z = string.Empty;

	//****************************************************************
	[MenuItem("TowerDefence Utils/Items modifications/Set items list Scale", false, 2)]
	public static void init()
	{
		curr_component          = EditorWindow.GetWindow<UpdateGOListScale>();
		curr_component.position = new Rect( Screen.width/2-300, 50, 300, 200 );
	}

	//****************************************************************
	void OnGUI()
	{
		str_x = EditorGUI.TextField( new Rect( 20, 20, 100, 20 ), str_x ).Trim();
		str_y = EditorGUI.TextField( new Rect( 20, 50, 100, 20 ), str_y ).Trim();
		str_z = EditorGUI.TextField( new Rect( 20, 80, 100, 20 ), str_z ).Trim();

		EditorGUI.LabelField( new Rect( 130, 20, 100, 20 ), "x", "x" );
		EditorGUI.LabelField( new Rect( 130, 50, 100, 20 ), "y", "y" );
		EditorGUI.LabelField( new Rect( 130, 80, 100, 20 ), "z", "z" );

		// add textures from selection
		if( GUI.Button( new Rect( 20, 110, 150, 20 ), "Apply" ) )
		{
            GameObject[] ids = Selection.gameObjects;
            for( int x = 0; x < ids.Length; x++ )
			{
                GameObject go = ids[ x ];

				Vector3 new_pos = Vector3.zero;
				new_pos.x = str_x != string.Empty ? float.Parse( str_x ) : go.transform.localScale.x;
				new_pos.y = str_y != string.Empty ? float.Parse( str_y ) : go.transform.localScale.y;
				new_pos.z = str_z != string.Empty ? float.Parse( str_z ) : go.transform.localScale.z;

                go.transform.localScale = new Vector3( new_pos.x, new_pos.y, new_pos.z );
            }
        }
	}

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}
