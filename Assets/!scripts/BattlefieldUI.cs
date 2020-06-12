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
using MapDataPlayer          = defines.MapDataPlayer;
using WeaponData             = defines.WeaponData;
using MessageBoxType         = defines.MessageBoxType;
using MessageBoxRet          = defines.MessageBoxRet;
using GameMusicChangeHandler = defines.GameMusicChangeHandler;
using GameSoundChangeHandler = defines.GameSoundChangeHandler;
using PauseOverlayHandler    = defines.PauseOverlayHandler;
using UnitData               = defines.UnitData;


public class BattlefieldUI : Singleton<BattlefieldUI>
{
    [SerializeField]
    private SpriteText                 lbl_map_name                   = null;
    [SerializeField]
    private SpriteText                 lbl_waves_counter              = null;
    [SerializeField]
    private SpriteText                 lbl_balance                    = null;
    [SerializeField]
    private SpriteText                 lbl_wave_limit                 = null;
    [SerializeField]
    private SpriteText                 lbl_timer                      = null;
    [SerializeField]
    private SpriteText                 lbl_enemyes_destroyed          = null;
    [SerializeField]
    private UIButton                   btn_start_wave                 = null;
    [SerializeField]
    private UIButton                   btn_pause                      = null;
    [SerializeField]
    private UIButton                   btn_back                       = null;
    [SerializeField]
    private UIButton                   btn_options                    = null;
    [SerializeField]
    private UIButton                   btn_scroll_prev                = null;
    [SerializeField]
    private UIButton                   btn_scroll_next                = null;
    [SerializeField]
    private UIScrollList               sc_arcenal_view                = null;
    [SerializeField]
    private BattlefieldArcenalViewCont arcenal_item_tmplt             = null;
    [SerializeField]
    private string                     lbl_start_wave_first           = "lbl-start-wave-first";
    [SerializeField]
    private string                     lbl_start_wave_next            = "lbl-start-wave-next";
    [SerializeField]
    private string                     lbl_exit_battle_confirm        = "lbl-exit-battle-confirm";
    [SerializeField]
    private string                     dialog_unit_info_prefab_name   = "window-unit-info";
    [SerializeField]
    private string                     dialog_weapon_info_prefab_name = "window-weapon-info";
    [SerializeField]
    private string                     dialog_options_prefab_name     = "window-options";
    [SerializeField]
    private string                     dialog_winlose_prefab_name     = "window-winlose";

    private WindowUnitInfo             dialog_unit_info               = null;
    private WindowWeaponInfo           dialog_wpn_info                = null;
    private WindowWinLose              dialog_win_lose                = null;
    private int                        sc_curr_index                  = 0;
    private MapData                    map_data                       = null;
    private WindowOptions              window_options                 = null;
    private bool                       is_initialized                 = false;

	//****************************************************************
	public void _Start()
	{
		map_data = MapController.Instance.MapDataSelected;
        map_data = map_data == null ? MapController.Instance.GetMapDataList[0] : map_data;

        lbl_map_name.Text = LangController.String_( map_data.MapName );

        btn_start_wave.Text = LangController.String_( lbl_start_wave_first );

        this._InitArcenal();

        this.UpdateBalance();
        this.UpdateLoseLimit( map_data.MapLoseWavesLimit );
        this.UpdateTimer();

        is_initialized = true;
	}

    //****************************************************************
    public void OnBtnWeapon( WeaponData wd )
    {
        if( wd == null ) return;
        
        this.ShowDialogWeaponInfo( wd );
    }

    //****************************************************************
    public void UpdateWavesCounter()
    {
        if( !is_initialized ) return;

        MapDataPlayer mdp = BattlefieldController.Instance.MapDataPlayer;
        lbl_waves_counter.Text = string.Format( "{0:00}/{1:00}", mdp.PlayerWaves, mdp.PlayerMapData.MapWavesCount );
    }

    //****************************************************************
    public void UpdateBalance()
    {
        lbl_balance.Text = string.Format( "{0:000000}", BattlefieldController.Instance.MapDataPlayer.PlayerBalance );
    }

    //****************************************************************
    public void UpdateEnemyesDestroyed()
    {
        lbl_enemyes_destroyed.Text = "20";
    }

    //****************************************************************
    public void ShowDialogWeaponInfo( WeaponData wd )
    {
        this._LoadWindowWpnInfo();
        dialog_wpn_info.Show( wd );
    }

    //****************************************************************
    public void ShowWindowUnitInfo( UnitData udata, Enemy enemy )
    {
        if( WindowBuildingInfo.Instance != null )
        {
            if( WindowBuildingInfo.Instance.IsVisible )
            {
                WindowBuildingInfo.Instance.Hide();
            }
        }

        this._LoadWindowUnitInfo();
        dialog_unit_info.Show( udata, enemy );
    }

    //****************************************************************
    public void HideWindowUnitInfo( Enemy enemy )
    {
        if( WindowUnitInfo.Instance != null )
        {
            if( WindowUnitInfo.Instance.IsVisible )
            {
                WindowUnitInfo.Instance.Hide();
            }
        }
    }

    //****************************************************************
    public void ShowWindowWin()
    {
        this._LoadWindowWinLose();
        dialog_win_lose.ShowWin();
    }

    //****************************************************************
    public void ShowWindowLose()
    {
        this._LoadWindowWinLose();
        dialog_win_lose.ShowLose();
    }

    //****************************************************************
    public bool SetBtnNextWaveEnabled
    {
        set{ btn_start_wave.controlIsEnabled = value; }
    }

    //****************************************************************
    private void FixedUpdate()
    {
        if( !is_initialized ) return;

        this.UpdateBalance();
        this.UpdateWavesCounter();
    }

    //****************************************************************
    private void _InitArcenal()
    {
        BattlefieldArcenalViewCont item_view = null;
        WeaponData                 wd        = null;
        for( int x = 0; x < map_data.MapArcenal.Length; x++ )
        {
            wd = BuildingController.Instance.GetWeaponData( map_data.MapArcenal[x] );

            if( x == 0 || x % 5 == 0 )
            {
                item_view = sc_arcenal_view.CreateItem( arcenal_item_tmplt.gameObject ).gameObject.GetComponent<BattlefieldArcenalViewCont>();
            }

            item_view.WeaponData = wd;
        }
        sc_arcenal_view.ScrollToItem( 0, 0 );
    }

    //****************************************************************
    public void UpdateLoseLimit( int waves )
    {
        lbl_wave_limit.Text = waves + "";
    }

    //****************************************************************
    public void UpdateTimer( string str = "" )
    {
        lbl_timer.Text = str == "" ? "00:00" : str;
    }
	
	//****************************************************************
	private void Start()
	{
		btn_start_wave .SetValueChangedDelegate( this._OnBtnStartWave  );
        btn_pause      .SetValueChangedDelegate( this._OnBtnPause      );
        btn_back       .SetValueChangedDelegate( this._OnBtnBack       );
        btn_options    .SetValueChangedDelegate( this._OnBtnOptions    );
        btn_scroll_prev.SetValueChangedDelegate( this._OnBtnScrollPrev );
        btn_scroll_next.SetValueChangedDelegate( this._OnBtnScrollNext );

        sc_arcenal_view.AddItemSnappedDelegate( this._OnArcenalItemSnapped );
	}

    //****************************************************************
    private void _OnBtnStartWave( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        BattlefieldController.Instance.OnBattleBegin();

        btn_start_wave.Text = LangController.String_( lbl_start_wave_next );
    }

    //****************************************************************
    private void _OnBtnPause( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        Pause.Instance.SetHandler( _OnPause );
        Pause.Instance.Show();

        BattlefieldController.Instance.BattlePaused = true;
    }

    //****************************************************************
    private void _OnPause( bool is_show )
    {
        if( !is_show )
        {
            BattlefieldController.Instance.BattlePaused = false;
        }
    }

    //****************************************************************
    private void _OnBtnBack( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        MessageBox.Instance.Bind( _OnConfirmExit );
        MessageBox.Instance.Init( LangController.String_( lbl_exit_battle_confirm ), MessageBoxType.OKCANCEL );
    }

    //****************************************************************
    private void _OnConfirmExit( MessageBoxRet ret )
    {
        if( ret == MessageBoxRet.OK )
        {
            SoundController.Instance.StopAll();
            Core.Instance.LoadScene( "map-selector" );
        }
    }

    //****************************************************************
    private void _OnBtnOptions( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        this._LoadDialogOptions();
        window_options.SetTempMusicChangeHandler( this._OptionsMusicChange );
        window_options.SetTempVisibilityChangeHandler( this._OptionsVisibilityChange );
        window_options.Show();
    }

    //****************************************************************
    private void _OptionsVisibilityChange( bool is_show )
    {
        BattlefieldController.Instance.BattlePaused = is_show;
    }

    //****************************************************************
    private void _OptionsMusicChange( bool play )
    {
        if( play )
            SoundController.Instance.Play_SoundBattleThemeRnd();
        else
            SoundController.Instance.StopAllMusic();
    }

    //****************************************************************
    private void _OnArcenalItemSnapped( IUIListObject item )
    {
        sc_curr_index = item.Index < 0 ? 0 : item.Index;
        sc_curr_index = item.Index > sc_arcenal_view.Count-1 ? sc_arcenal_view.Count-1 : item.Index;
        sc_arcenal_view.PositionItems();
        sc_arcenal_view.RepositionItems();
    }

    //****************************************************************
    private void _OnBtnScrollPrev( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        int next_index = sc_curr_index-1;
        next_index = next_index < 0 ? 0 : next_index;
        sc_arcenal_view.ScrollToItem( next_index, 0.15f );
    }

    //****************************************************************
    private void _OnBtnScrollNext( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        int next_index = sc_curr_index+1;
        next_index = next_index > sc_arcenal_view.Count-1 ? sc_arcenal_view.Count-1 : next_index;
        sc_arcenal_view.ScrollToItem( next_index, 0.15f );
    }

    //****************************************************************
    private void _LoadDialogOptions()
    {
        if( window_options == null )
        {
            window_options = Utils.FindComp<WindowOptions>();
            if( window_options == null )
            {
                window_options = Utils.LoadPrefab( dialog_options_prefab_name ).GetComponent<WindowOptions>();
            }
            window_options._Start();
        }
    }

    //****************************************************************
    private void _LoadWindowWpnInfo()
    {
        if( dialog_wpn_info == null )
        {
            dialog_wpn_info = Utils.FindComp<WindowWeaponInfo>();
            if( dialog_wpn_info == null )
            {
                dialog_wpn_info = Utils.LoadPrefab( dialog_weapon_info_prefab_name ).GetComponent<WindowWeaponInfo>();
            }
        }
    }

    //****************************************************************
    private void _LoadWindowUnitInfo()
    {
        if( dialog_unit_info == null )
        {
            dialog_unit_info = Utils.FindComp<WindowUnitInfo>();
            if( dialog_unit_info == null )
            {
                dialog_unit_info = Utils.LoadPrefab( dialog_unit_info_prefab_name ).GetComponent<WindowUnitInfo>();
            }
            dialog_unit_info._Start();
        }
    }

    //****************************************************************
    private void _LoadWindowWinLose()
    {
        if( dialog_win_lose == null )
        {
            dialog_win_lose = Utils.FindComp<WindowWinLose>();
            if( dialog_win_lose == null )
            {
                dialog_win_lose = Utils.LoadPrefab( dialog_winlose_prefab_name ).GetComponent<WindowWinLose>();
            }
        }
    }

    //****************************************************************
    //private void OnGUI()
    //{
    //    if( GUI.Button( new Rect( 20, 60, 100, 30 ), "Show Unit Info" ) )
    //    {
    //        UnitData udata = UnitsController.Instance.GetUnitDataList()[ Utils.Random( 0, UnitsController.Instance.GetUnitDataList().Count ) ];
    //        this.ShowWindowUnitInfo( udata );
    //    }
    //}
}
