using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class SetSizeSpriteList : EditorWindow
{
    private static SetSizeSpriteList curr_component = null;

    private string str_width  = string.Empty;
    private string str_height = string.Empty;

	//****************************************************************
	[MenuItem("TowerDefence Utils/Items modifications/Set Sprites Size", false, 6)]
	public static void init()
	{
		curr_component          = EditorWindow.GetWindow<SetSizeSpriteList>();
		curr_component.position = new Rect( Screen.width/2-300, 50, 300, 200 );
	}

    //****************************************************************
    private void OnGUI()
    {
        str_width  = EditorGUI.TextField( new Rect( 20, 20, 100, 20 ), str_width  ).Trim();
        str_height = EditorGUI.TextField( new Rect( 20, 50, 100, 20 ), str_height ).Trim();

        EditorGUI.LabelField( new Rect( 130, 24, 100, 20 ), "Width"  );
        EditorGUI.LabelField( new Rect( 130, 54, 100, 20 ), "Height" );

        if( GUI.Button( new Rect( 20, 80, 150, 20 ), "Apply" ) )
        {
            if( str_width == string.Empty || str_height == string.Empty ) return;

            List<GameObject> objects = new List<GameObject>( Selection.gameObjects );
            
            foreach( GameObject go in objects )
            {
                SpriteBase sprite = go.GetComponent<SpriteBase>();
                
                if( sprite == null ) continue;

                sprite.width  = float.Parse( str_width  );
                sprite.height = float.Parse( str_height );
            }
        }
    }

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}
