#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using MapData                = defines.MapData;
using MessageBoxType         = defines.MessageBoxType;
using MessageBoxRet          = defines.MessageBoxRet;
using GameMusicChangeHandler = defines.GameMusicChangeHandler;
using GameSoundChangeHandler = defines.GameSoundChangeHandler;

public class MapSelector : Singleton<MapSelector>
{
    [SerializeField]
    private MapSelectorPreviewBig map_preview_big            = null;
    [SerializeField]
    private MapItemViewContainer  map_item_view_tmplt        = null;
    [SerializeField]
    private UIScrollList          map_items_view             = null;
    [SerializeField]
    private UIButton              btn_scroll_prev            = null;
    [SerializeField]
    private UIButton              btn_scroll_next            = null;
    [SerializeField]
    private SpriteText            lbl_pager                  = null;
    [SerializeField]
    private UIButton              btn_back_main_menu         = null;
    [SerializeField]
    private UIButton              btn_options                = null;
    [SerializeField]
    private UIButton              btn_play                   = null;
    [SerializeField]
    private string                lbl_confirm_back_main_menu = "lbl-confirm-back-main-menu";
    [SerializeField]
    private string                lbl_confirm_start_game     = "lbl-confirm-start-game";
    [SerializeField]
    private string                lbl_confirm_cant_attack    = "lbl-confirm-cant-attack";
    private int                   curr_scroll_index          = 0;
    private WindowOptions         window_options             = null;
    private MapData               selected_map_data          = null;
    private List<MapData>         map_data_list              = new List<MapData>();

	//****************************************************************
	public void _Start()
	{
        map_data_list = MapController.Instance.GetMapDataList;
        // Init
        MapItemViewContainer map_item_cont = null;
        for( int x = 0; x < map_data_list.Count; x++ )
        {
            if( x == 0 || x % 4 == 0 )
            {
                map_item_cont = map_items_view.CreateItem( map_item_view_tmplt.gameObject ).gameObject.GetComponent<MapItemViewContainer>();
                map_item_cont.gameObject.SetActive(true);
            }
            map_item_cont.MapData = map_data_list[ x ];
            if( x == 0 )
            {
                map_item_cont.CheckFirstItem();
            }
        }
        map_item_cont.Clear();
        map_items_view.ScrollToItem( 0, 0 );

        this._UpdatePager();
        this._UpdateScrollButtons();
	}

    //****************************************************************
    public void OnItemSelected( MapItemView item )
    {
        selected_map_data                      = item.MapData;
        MapController.Instance.MapDataSelected = selected_map_data;
        map_preview_big.MapData                = selected_map_data;

        map_preview_big.Init();
    }

    //****************************************************************
    private void Start()
    {
        btn_scroll_prev   .SetValueChangedDelegate( this._OnBtnScrollPrev   );
        btn_scroll_next   .SetValueChangedDelegate( this._OnBtnScrollNext   );
        btn_back_main_menu.SetValueChangedDelegate( this._OnBtnBackMainMenu );
        btn_options       .SetValueChangedDelegate( this._OnBtnOptions      );
        btn_play          .SetValueChangedDelegate( this._OnBtnPlay         );

        map_items_view.AddItemSnappedDelegate( _OnScrollSnaped );
    }

    //****************************************************************
    private void _OnScrollSnaped( IUIListObject item )
    {
        curr_scroll_index = item.Index < 0 ? 0 : item.Index;
        curr_scroll_index = item.Index > map_items_view.Count-1 ? map_items_view.Count-1 : item.Index;

        this._UpdatePager();
        this._UpdateScrollButtons();
    }

    //****************************************************************
    private void _OnBtnScrollPrev( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        int prev_item_index = curr_scroll_index-1;
        prev_item_index = prev_item_index < 0 ? 0 : prev_item_index;
        map_items_view.ScrollToItem( prev_item_index, 0.15f );

        this._UpdatePager();
        this._UpdateScrollButtons();
    }

    //****************************************************************
    private void _OnBtnScrollNext( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        int next_item_index = curr_scroll_index+1;
        next_item_index = next_item_index > map_items_view.Count-1 ? map_items_view.Count-1 : next_item_index;
        map_items_view.ScrollToItem( next_item_index, 0.15f );

        this._UpdatePager();
        this._UpdateScrollButtons();
    }

    //****************************************************************
    private void _OnBtnBackMainMenu( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();
        
        MessageBox.Instance.Bind( this._OnConfirmBackMainMenu );
        MessageBox.Instance.Init( LangController.String_( lbl_confirm_back_main_menu ), MessageBoxType.OKCANCEL );
    }

    //****************************************************************
    private void _OnBtnOptions( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        this._LoadDialogOptions();
        window_options.SetTempMusicChangeHandler( this._OptionsMusicChange );
        window_options.Show();
    }

    //****************************************************************
    private void _OptionsMusicChange( bool play )
    {
        if( play )
            SoundController.Instance.Play_SoundMapSelectorRnd();
        else
            SoundController.Instance.StopAllMusic();
    }

    //****************************************************************
    private void _OnBtnPlay( IUIObject btn )
    {
        if( _CheckIfCanAttack( selected_map_data ) )
        {
            SoundController.Instance.Play_SoundClick();

            MessageBox.Instance.Bind( this._OnConfirmStartGame );
            MessageBox.Instance.Init( LangController.String_( lbl_confirm_start_game ), MessageBoxType.OKCANCEL );
        }
        else
        {
            SoundController.Instance.Play_SoundClickDenail();

            MessageBox.Instance.Bind( this._OnConfirmCantAttack );
            MessageBox.Instance.Init( LangController.String_( lbl_confirm_cant_attack ), MessageBoxType.OK );
        }
    }

    //****************************************************************
    private void _OnConfirmStartGame( MessageBoxRet ret )
    {
        if( ret == MessageBoxRet.OK )
        {
            Core.Instance.LoadScene( selected_map_data.MapId );
        }
    }

    //****************************************************************
    private void _OnConfirmCantAttack( MessageBoxRet ret )
    {
        // ...
    }

    //****************************************************************
    private bool _CheckIfCanAttack( MapData map_data )
    {
        if( map_data == null )
        {
            print( "EXCEPTION: MapData is NULL" );
            return false;
        }

        bool is_req_map_done  = Utils.IsPlayerMapDone( map_data.MapRequired ) || map_data.MapRequired == string.Empty;
        bool is_curr_map_done = Utils.IsPlayerMapDone( map_data.MapId       );

        if( is_req_map_done )
        {
            return true;
        }
        else
        {
            if( is_curr_map_done )
            {
                return true;
            }

            return false;
        }
    }

    //****************************************************************
    private void _OnConfirmBackMainMenu( MessageBoxRet ret )
    {
        if( ret == MessageBoxRet.OK )
        {
            SoundController.Instance.StopAll();
            Core.Instance.LoadScene( "main-menu" );
        }
    }

    //****************************************************************
    private void _LoadDialogOptions()
    {
        if( window_options == null )
        {
            window_options = Utils.FindComp<WindowOptions>();
            if( window_options == null )
            {
                window_options = Utils.LoadPrefab( "window-options" ).GetComponent<WindowOptions>();
            }
            window_options._Start();
        }
    }

    //****************************************************************
    private void _UpdatePager()
    {
        lbl_pager.Text = string.Format( "{0}/{1}", curr_scroll_index+1, map_items_view.Count );
    }

    //****************************************************************
    private void _UpdateScrollButtons()
    {
        if( curr_scroll_index < 1 )
        {
            btn_scroll_prev.controlIsEnabled = false;
            btn_scroll_next.controlIsEnabled = true;
        }
        else if( curr_scroll_index == map_items_view.Count-1 )
        {
            btn_scroll_prev.controlIsEnabled = true;
            btn_scroll_next.controlIsEnabled = false;
        }

        if( map_items_view.Count == 1 )
        {
            btn_scroll_prev.controlIsEnabled = false;
            btn_scroll_next.controlIsEnabled = false;
        }

        if( curr_scroll_index > 0 && map_items_view.Count > 1 && curr_scroll_index < map_items_view.Count-1 )
        {
            btn_scroll_prev.controlIsEnabled = true;
            btn_scroll_next.controlIsEnabled = true;
        }
    }
}
