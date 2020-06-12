#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using WeaponData    = defines.WeaponData;
using WeaponArea    = defines.WeaponArea;
using MapData       = defines.MapData;
using MapDataPlayer = defines.MapDataPlayer;

public class WindowWeaponInfo : Singleton<WindowWeaponInfo>
{
    [SerializeField]
    private SpriteText          lbl_wpn_name       = null;
    [SerializeField]
    private SpriteText          lbl_wpn_tech_class = null;
    [SerializeField]
    private SpriteText          lbl_wpn_price      = null;
    [SerializeField]
    private SpriteText          lbl_wpn_range      = null;
    [SerializeField]
    private SpriteText          lbl_wpn_damage     = null;
    [SerializeField]
    private SpriteText          lbl_wpn_upgrades   = null;
    [SerializeField]
    private SpriteText          lbl_wpn_firerate   = null;
    [SerializeField]
    private SpriteText          lbl_wpn_dps        = null;
    [SerializeField]
    private Transform           t_wpn_ico          = null;
    [SerializeField]
    private UIListItemContainer lbl_desc_tmplt     = null;
    [SerializeField]
    private UIScrollList        sc_desc            = null;
    [SerializeField]
    private Transform[]         t_wpn_area_list    = null;
    [SerializeField]
    private UIPanel             panel              = null;
    [SerializeField]
    private UIButton            btn_close          = null;
    [SerializeField]
    private UIButton            btn_buy            = null;

    private WeaponData          weapon_data        = null;
    private bool                is_visible         = false;

    //****************************************************************
    public WeaponData WeaponData
    {
        get{ return weapon_data;  }
        set{ weapon_data = value; }
    }
    public bool IsVisible
    {
        get{ return is_visible;  }
        set{ is_visible = value; }
    }

    //****************************************************************
    public void Show( WeaponData wpdata = null )
    {
        weapon_data = wpdata != null ? wpdata : weapon_data;
        if( weapon_data == null )
        {
            Core.Log = "EXCEPTION: WeaponData is NULL";
            return;
        }

        UIManager.instance.blockInput = true;

        this._InitView();

        panel.StartTransition( UIPanelManager.SHOW_MODE.BringInForward );
        panel.AddTempTransitionDelegate
        (
            ( a, b ) =>
            {
                is_visible = true;
                UIManager.instance.blockInput = false;
            }
        );
    }

    //****************************************************************
    public void Hide()
    {
        if( !is_visible ) return;

        UIManager.instance.blockInput = true;

        panel.StartTransition( UIPanelManager.SHOW_MODE.DismissForward );
        panel.AddTempTransitionDelegate
        (
            ( a, b ) =>
            {
                is_visible = false;
                UIManager.instance.blockInput = false;
            }
        );
    }

    //****************************************************************
    private void Start()
    {
        btn_close.SetValueChangedDelegate( this._OnBtnClose );
        btn_buy  .SetValueChangedDelegate( this._OnBtnBuy   );
    }

    //****************************************************************
    private void _OnBtnClose( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        this.Hide();
    }

    //****************************************************************
    private void _OnBtnBuy( IUIObject btn )
    {
        SoundController.Instance.Play_SoundBuildingSell();

        BattlefieldController.Instance.OnBuildingBuy( weapon_data );
        
        this.Hide();
    }

    //****************************************************************
    private void _InitView()
    {
        lbl_wpn_name       .Text = LangController.String_( weapon_data.WpnName );
        lbl_wpn_tech_class .Text = "";
        lbl_wpn_price      .Text = weapon_data.WpnPrice + "";
        lbl_wpn_range      .Text = weapon_data.WpnRange + "";
        lbl_wpn_damage     .Text = weapon_data.WpnDamage + "";
        lbl_wpn_upgrades   .Text = 0 + "";
        lbl_wpn_firerate   .Text = weapon_data.WpnFirerate + "";
        lbl_wpn_dps        .Text = weapon_data.WpnDamage*weapon_data.WpnFirerate + "";

        Utils.ClearChilds( t_wpn_ico );
        ItemsController.Instance.GetIcon( weapon_data.WpnIco, t_wpn_ico );

        sc_desc.ClearList( true );
        IUIListObject       lo     = sc_desc.CreateItem( lbl_desc_tmplt.gameObject );
        UIListItemContainer licont = lo.gameObject.GetComponent<UIListItemContainer>();
        licont.Text = LangController.String_( weapon_data.WpnDesc );

        for( int x = 1; x < t_wpn_area_list.Length; x++ )
        {
            t_wpn_area_list[x].gameObject.SetActive( false );
        }

        t_wpn_area_list[ (int)weapon_data.WeaponArea ].gameObject.SetActive( true );

        licont.ScanChildren();

        // buy confirm
        MapDataPlayer player_md = BattlefieldController.Instance.MapDataPlayer;
        btn_buy.controlIsEnabled = player_md.PlayerBalance >= weapon_data.WpnPrice;

        Utils.Translate( transform );
    }
}
