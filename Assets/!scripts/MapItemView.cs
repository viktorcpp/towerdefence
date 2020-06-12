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

public class MapItemView : MonoBehaviour
{
    [SerializeField]
    private SpriteText lbl_level_name      = null;
    [SerializeField]
    private SpriteText lbl_balance         = null;
    [SerializeField]
    private SpriteText lbl_waves_count     = null;
    [SerializeField]
    private Transform  t_ico_preview       = null;
    [SerializeField]
    private Transform  t_state_locked      = null;
    [SerializeField]
    private Transform  t_state_can_attack  = null;
    [SerializeField]
    private Transform  t_state_done        = null;
    [SerializeField]
    private string     lbl_map_balance_xx  = "lbl-map-balance-xx";
    [SerializeField]
    private string     lbl_map_waves_count = "lbl-map-waves-count-xx";
    [SerializeField]
    private UIRadioBtn btn                 = null;
    private MapData    map_data            = null;

    //****************************************************************
    public MapData MapData
    {
        get{ return map_data;  }
        set{ map_data = value; }
    }

    //****************************************************************
    public void CheckItem()
    {
        btn.Value = true;
        MapSelector.Instance.OnItemSelected( this );
    }

    //****************************************************************
    public void UnCheckItem()
    {
        btn.Value = false;
        this._OnBtn( btn );
    }

    //****************************************************************
    public void Init( MapData mdata = null )
    {
        map_data = mdata != null ? mdata : map_data;

        lbl_level_name. Text = LangController.String_( map_data.MapName );
        lbl_balance    .Text = string.Format( LangController.String_( lbl_map_balance_xx  ), map_data.MapBalance );
        lbl_waves_count.Text = string.Format( LangController.String_( lbl_map_waves_count ), map_data.MapWavesCount   );

        Utils.ClearChilds( t_ico_preview );
        ItemsController.Instance.GetIcon( map_data.MapIcoSmall, t_ico_preview );

        bool is_req_map_done  = Utils.IsPlayerMapDone( map_data.MapRequired ) || map_data.MapRequired == string.Empty;
        bool is_curr_map_done = Utils.IsPlayerMapDone( map_data.MapId       );

        if( is_curr_map_done )
        {
            DestroyImmediate( t_state_locked.gameObject     );
            DestroyImmediate( t_state_can_attack.gameObject );
        }
        else
        {
            if( is_req_map_done )
            {
                DestroyImmediate( t_state_locked.gameObject );
                DestroyImmediate( t_state_done.gameObject   );
            }
            else
            {
                DestroyImmediate( t_state_can_attack.gameObject );
                DestroyImmediate( t_state_done.gameObject       );
            }
        }
    }
	
	//****************************************************************
	private void Start()
	{
		btn.SetValueChangedDelegate( _OnBtn );
	}

    //****************************************************************
    private void _OnBtn( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        UIRadioBtn rbtn = (UIRadioBtn)btn;
        if( rbtn.Value )
        {
            MapSelector.Instance.OnItemSelected( this );
        }
    }
}
