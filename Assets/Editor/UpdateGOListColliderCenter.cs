using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UpdateGOListColliderCenter : EditorWindow
{
	private static UpdateGOListColliderCenter curr_component = null;

	string str_x = string.Empty;
	string str_y = string.Empty;
    string str_z = string.Empty;

	//****************************************************************
	[MenuItem("TowerDefence Utils/Items modifications/Set items list Collider center", false, 4)]
	public static void init()
	{
		curr_component          = EditorWindow.GetWindow<UpdateGOListColliderCenter>();
		curr_component.position = new Rect( Screen.width/2-300, 50, 300, 200 );
	}

	//****************************************************************
	void OnGUI()
	{
		str_x = EditorGUI.TextField( new Rect( 20, 20, 100, 20 ), str_x ).Trim();
		str_y = EditorGUI.TextField( new Rect( 20, 50, 100, 20 ), str_y ).Trim();
        str_z = EditorGUI.TextField( new Rect( 20, 80, 100, 20 ), str_z ).Trim();

		EditorGUI.LabelField( new Rect( 130, 20, 100, 20 ), "center-X", "center-X" );
		EditorGUI.LabelField( new Rect( 130, 50, 100, 20 ), "center-Y", "center-Y" );
        EditorGUI.LabelField( new Rect( 130, 80, 100, 20 ), "center-Z", "center-Z" );

		// add textures from selection
		if( GUI.Button( new Rect( 20, 110, 150, 20 ), "Apply" ) )
		{
            GameObject[] ids = Selection.gameObjects;
            for( int x = 0; x < ids.Length; x++ )
			{
                GameObject go = ids[ x ];
				BoxCollider bc = go.GetComponent<BoxCollider>();

                if( bc )
                {
                    Vector3 bc_old_center = bc.center;
                    bc_old_center.x = str_x != string.Empty ? float.Parse( str_x ) : bc.size.x;
                    bc_old_center.y = str_y != string.Empty ? float.Parse( str_y ) : bc.size.y;
                    bc_old_center.z = str_z != string.Empty ? float.Parse( str_z ) : bc.size.z;
                    bc.center = bc_old_center;
                }
            }
        }
	}

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}
