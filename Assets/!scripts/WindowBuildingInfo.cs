#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using WeaponData   = defines.WeaponData;
using WeaponArea   = defines.WeaponArea;
using BuildingData = defines.BuildingData;

public class WindowBuildingInfo : Singleton<WindowBuildingInfo>
{
    [SerializeField]
    private SpriteText   lbl_weapon_name          = null;
    [SerializeField]
    private SpriteText   lbl_weapon_range         = null;
    [SerializeField]
    private SpriteText   lbl_weapon_dps           = null;
    [SerializeField]
    private SpriteText   lbl_weapon_damage        = null;
    [SerializeField]
    private SpriteText   lbl_weapon_upgrade_price = null;
    [SerializeField]
    private SpriteText   lbl_weapon_upgrade_time  = null;
    [SerializeField]
    private SpriteText   lbl_weapon_sell_price    = null;
    [SerializeField]
    private UIButton     btn_upgrade              = null;
    [SerializeField]
    private UIButton     btn_details              = null;
    [SerializeField]
    private UIButton     btn_sell                 = null;
    [SerializeField]
    private UIButton     btn_close                = null;
    [SerializeField]
    private Transform    t_ico_cont               = null;
    [SerializeField]
    private UIPanel      panel                    = null;

    private BuildingData building_data            = null;
    private Building     building                 = null;
    private bool         is_visible               = false;

    //****************************************************************
    public bool IsVisible
    {
        get{ return is_visible; }
    }
    
    //****************************************************************
    public BuildingData BuildingData
    {
        get{ return building_data;  }
        set{ building_data = value; }
    }
    public Building Building
    {
        get{ return building;  }
        set{ building = value; }
    }

	//****************************************************************
	public void _Start()
	{
		// ...
	}

    //****************************************************************
    public void Show( BuildingData bdata = null )
    {
        building_data = bdata != null ? bdata : building_data;
        if( building_data == null )
        {
            Core.Log = "EXCEPTION: BuildingData is NULL";
            return;
        }

        UIManager.instance.blockInput = true;

        this._InitView( building_data );

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

        if( building != null )
            building.HideDistance();

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
		btn_close  .SetValueChangedDelegate( _OnBtnClose   );
        btn_upgrade.SetValueChangedDelegate( _OnBtnUpgrade );
        btn_details.SetValueChangedDelegate( _OnBtnDetails );
        btn_sell   .SetValueChangedDelegate( _OnBtnSell    );
	}

    //****************************************************************
    private void _InitView( BuildingData bdata = null )
    {
        lbl_weapon_name          .Text = LangController.String_( building_data.WeaponData.WpnName );
        lbl_weapon_range         .Text = building_data.WeaponRange + "";
        lbl_weapon_dps           .Text = building_data.WeaponFirerate * building_data.WeaponData.WpnDamage + "";
        lbl_weapon_damage        .Text = building_data.WeaponDamage + "";
        lbl_weapon_upgrade_price .Text = building_data.WeaponUpgradePrice + "";
        lbl_weapon_upgrade_time  .Text = Utils.FormatTimeA( building_data.WeaponUpgradeTime );
        lbl_weapon_sell_price    .Text = "$" + building_data.WeaponSellPrice;

        Utils.ClearChilds( t_ico_cont );
        ItemsController.Instance.GetIcon( building_data.WeaponData.WpnIco, t_ico_cont );

        // TODO: upgrade doesnt implemented yet
        btn_upgrade.controlIsEnabled = false;
        if( building_data.WeaponUpgradePrice > BattlefieldController.Instance.MapDataPlayer.PlayerBalance )
        {
            btn_upgrade.controlIsEnabled = false;
        }

        Utils.Translate( transform );
    }

    //****************************************************************
    private void _OnBtnSell( IUIObject btn )
    {
        SoundController.Instance.Play_SoundBuildingSell();

        BuildingController.Instance.OnBuildingSell();

        this.Hide();
    }

    //****************************************************************
    private void _OnBtnDetails( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        BuildingController.Instance.ShowDialogBuildingDetails();
    }

    //****************************************************************
    private void _OnBtnUpgrade( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();
    }

    //****************************************************************
    private void _OnBtnClose( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        this.Hide();
    }
}
