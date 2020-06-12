using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GroupRenameGO : EditorWindow
{
	private static GroupRenameGO curr_component = null;

	private string str_tmp       = string.Empty;
    private string int_begin_str = "1";
    private string int_step_str  = "1";
    private string int_fill_str  = "1";

    private string find_what     = string.Empty;
    private string replace_for   = string.Empty;

	//****************************************************************
	[MenuItem("TowerDefence Utils/Group rename GO", false, 2)]
	public static void init()
	{
		curr_component          = EditorWindow.GetWindow<GroupRenameGO>();
		curr_component.position = new Rect( Screen.width/2-300, 50, 600, 300 );
	}

	//****************************************************************
	void OnGUI()
	{
        this._DrawTemplateRename();
        this._DrawFindAndReplace();
	}

    //****************************************************************
    private void _DrawTemplateRename()
    {
        // set name by template
        EditorGUI.LabelField( new Rect( 20, 10, 300, 20 ), "Шаблон", "" );
		str_tmp       = EditorGUI.TextField( new Rect( 20, 30, 300, 20 ), str_tmp       ).Trim();

        EditorGUI.LabelField( new Rect( 330, 10, 70, 20 ), "Начать с", "" );
        int_begin_str = EditorGUI.TextField( new Rect( 330, 30, 70, 20 ), int_begin_str ).Trim();

        EditorGUI.LabelField( new Rect( 410, 10, 70, 20 ), "Шаг", "" );
        int_step_str = EditorGUI.TextField( new Rect( 410, 30, 70, 20 ), int_step_str ).Trim();

        EditorGUI.LabelField( new Rect( 490, 10, 70, 20 ), "Цифр", "" );
        int_fill_str = EditorGUI.TextField( new Rect( 490, 30, 70, 20 ), int_fill_str ).Trim();

		// add textures from selection
		if( GUI.Button( new Rect( 20, 60, 150, 20 ), "Применить" ) )
		{
            GameObject[] ids = Selection.gameObjects;
            
            int int_begin   = int.Parse( int_begin_str.Trim() );
            int int_step    = int.Parse( int_step_str );
            int int_fill    = int.Parse( int_fill_str );
            string fill_str = string.Empty;
            if( int_fill <= 1 )
            {
                fill_str = "{0}";
            }
            else
            {
                fill_str = "{0:";
                for( int x = 0; x < int_fill; x++ )
                {
                    fill_str += "0";
                }
                fill_str += "}";
            }

            for( int x = 0; x < ids.Length; x++ )
			{
                GameObject go = ids[ x ];
                string str_tmplt_2 = str_tmp;
                str_tmplt_2 = str_tmplt_2.Replace( "[C]", string.Format( fill_str, int_begin ) );
                str_tmplt_2 = str_tmplt_2.Replace( "[N]", go.name );

				go.name = str_tmplt_2;

                int_begin += int_step;
            }
        }
    }

    //****************************************************************
    private void _DrawFindAndReplace()
    {
        EditorGUI.LabelField( new Rect( 20, 120, 300, 20 ), "Найти:", "" );
		find_what = EditorGUI.TextField( new Rect( 20, 140, 300, 20 ), find_what ).Trim();

        EditorGUI.LabelField( new Rect( 20, 170, 300, 20 ), "Заменить на:", "" );
        replace_for = EditorGUI.TextField( new Rect( 20, 190, 300, 20 ), replace_for ).Trim();

        if( GUI.Button( new Rect( 20, 220, 150, 20 ), "Применить" ) )
        {
            
            MonoBehaviour.print( this.position.xMax - this.position.xMin );

            if( find_what == string.Empty ) return;

            GameObject[] ids = Selection.gameObjects;

            foreach( GameObject go in ids )
            {
                go.name = go.name.Replace( find_what, replace_for );
            }
        }
    }

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}
