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

public class MapSelectorPreviewBig : MonoBehaviour
{
    [SerializeField]
    private SpriteText lbl_level_name            = null;
    [SerializeField]
    private SpriteText lbl_balance_money_value   = null;
    [SerializeField]
    private SpriteText lbl_waves_count_value     = null;
    [SerializeField]
    private SpriteText lbl_enemyes_in_wave_value = null;
    [SerializeField]
    private SpriteText lbl_waves_timer           = null;
    [SerializeField]
    private Transform  t_map_preview             = null;
    private MapData    map_data                  = null;

    //****************************************************************
    public MapData MapData
    {
        get{ return map_data;  }
        set{ map_data = value; }
    }

    //****************************************************************
    public void Init( MapData mdata = null )
    {
        map_data = mdata != null ? mdata : map_data;

        if( map_data == null ) return;

        lbl_level_name             .Text = LangController.String_( map_data.MapName );
        lbl_balance_money_value    .Text = map_data.MapBalance + "";
        lbl_waves_count_value      .Text = map_data.MapWavesCount + "";
        lbl_enemyes_in_wave_value  .Text = map_data.MapEnemyesInWave + "";
        lbl_waves_timer            .Text = Utils.FormatTimeA( map_data.MapTimer ) + "";

        Utils.ClearChilds( t_map_preview );
        ItemsController.Instance.GetIcon( map_data.MapIcoBig, t_map_preview );
    }
}
