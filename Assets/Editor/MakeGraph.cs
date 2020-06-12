#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using DateTime = System.DateTime;
using TimeSpan = System.TimeSpan;

public class MakeGraph : EditorWindow
{
    private static MakeGraph curr_component = null;

    private int INDEX_A_MIN = 0;
    private int INDEX_A_MAX = 14;
    private int INDEX_B_MIN = 0;
    private int INDEX_B_MAX = 28;

    //****************************************************************
	[MenuItem("TowerDefence Utils/Make Battlefield Graph", false, 5)]
	public static void init()
	{
		curr_component          = EditorWindow.GetWindow<MakeGraph>();
		curr_component.position = new Rect( Screen.width/2-300, 50, 600, 300 );
	}

    //****************************************************************
	private void OnGUI()
	{
        if( GUI.Button( new Rect( 20, 20, 100, 30 ), "Build Graph" ) )
        {
            this._Make();
        }
	}

    //****************************************************************
    private void _Make()
    {
        Transform root = Selection.activeTransform;

        if( root == null ) return;
        
        List<BattlefieldSectorItem> items = root.GetComponentsInChildren<BattlefieldSectorItem>().ToList<BattlefieldSectorItem>();
        
        List<BattlefieldSectorItem> items_list   = null;
        BattlefieldSectorItem       item_current = null;
        for( int a = INDEX_A_MIN; a < INDEX_A_MAX; a++ )
        {
            for( int b = INDEX_B_MIN; b < INDEX_B_MAX; b++ )
            {
                List<BattlefieldSectorItem> item_curr_list = items.Where( bsi => bsi.name == string.Format( "sector-view-[{0},{1}]", a, b ) ).ToList();
                if( item_curr_list.Count < 1 ) continue;
                item_current         = item_curr_list[0];

                // top
                items_list           = items.Where( bsi => bsi.name == string.Format( "sector-view-[{0},{1}]", a-1, b ) ).ToList();
                item_current.NodeTop = items_list.Count > 0 ? items_list[0] : null;
                
                // top-right
                items_list                = items.Where( bsi => bsi.name == string.Format( "sector-view-[{0},{1}]", a-1, b+1 ) ).ToList();
                item_current.NodeTopRight = items_list.Count > 0 ? items_list[0] : null;

                // right
                items_list                = items.Where( bsi => bsi.name == string.Format( "sector-view-[{0},{1}]", a, b+1 ) ).ToList();
                item_current.NodeRight = items_list.Count > 0 ? items_list[0] : null;

                // right bottom
                items_list                = items.Where( bsi => bsi.name == string.Format( "sector-view-[{0},{1}]", a+1, b+1 ) ).ToList();
                item_current.NodeBotRight = items_list.Count > 0 ? items_list[0] : null;

                // bottom
                items_list           = items.Where( bsi => bsi.name == string.Format( "sector-view-[{0},{1}]", a+1, b ) ).ToList();
                item_current.NodeBot = items_list.Count > 0 ? items_list[0] : null;

                // bottom left
                items_list               = items.Where( bsi => bsi.name == string.Format( "sector-view-[{0},{1}]", a+1, b-1 ) ).ToList();
                item_current.NodeBotLeft = items_list.Count > 0 ? items_list[0] : null;

                // left
                items_list            = items.Where( bsi => bsi.name == string.Format( "sector-view-[{0},{1}]", a, b-1 ) ).ToList();
                item_current.NodeLeft = items_list.Count > 0 ? items_list[0] : null;

                // left top
                items_list               = items.Where( bsi => bsi.name == string.Format( "sector-view-[{0},{1}]", a+1, b-1 ) ).ToList();
                item_current.NodeLeftTop = items_list.Count > 0 ? items_list[0] : null;
            }
        }
    }

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}
