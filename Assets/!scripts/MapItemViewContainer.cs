#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using MapData = defines.MapData;

public class MapItemViewContainer : MonoBehaviour
{
    [SerializeField]
    private UIListItemContainer ui_list_item_cont      = null;
    [SerializeField]
    private MapItemView[]       item_view_list         = null;
    [SerializeField]
    private UIScrollList        ui_scroll_list         = null;
    private int                 item_view_list_counter = 0;

    //****************************************************************
    public MapData MapData
    {
        set
        {
            if( item_view_list_counter >= item_view_list.Length ) return;

            item_view_list[ item_view_list_counter ].MapData = value;
            item_view_list[ item_view_list_counter ].Init();
            item_view_list[ item_view_list_counter ].GetComponent<UIRadioBtn>().SetGroup( ui_scroll_list.gameObject );

            item_view_list_counter++;

            ui_list_item_cont.ScanChildren();
        }
    }

    //****************************************************************
    public void CheckFirstItem()
    {
        item_view_list[0].CheckItem();
    }

    //****************************************************************
    public void Clear()
    {
        if( item_view_list_counter >= item_view_list.Length ) return;

        for( int x = item_view_list_counter; x < item_view_list.Length; x++ )
        {
            item_view_list[ x ].GetComponent<UIRadioBtn>().SetGroup( item_view_list[ x ].gameObject );
            item_view_list[ x ].transform.localPosition = new Vector3( 0,0,3000 );
            item_view_list[ x ].gameObject.SetActive( false );
        }

        ui_list_item_cont.ScanChildren();
    }
}
