#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using UnitData = defines.UnitData;

public class WindowUnitInfo : Singleton<WindowUnitInfo>
{
    [SerializeField]
    private SpriteText          lbl_unit_name           = null;
    [SerializeField]
    private SpriteText          lbl_unit_reward         = null;
    [SerializeField]
    private SpriteText          lbl_unit_health_percent = null;
    [SerializeField]
    private SpriteText          lbl_unit_health_value   = null;
    [SerializeField]
    private UIScrollList        sc_desc                 = null;
    [SerializeField]
    private UIListItemContainer t_desc_tmplt            = null;
    [SerializeField]
    private Transform           t_ico_cont              = null;
    [SerializeField]
    private UIButton            btn_close               = null;
    [SerializeField]
    private UIPanel             panel                   = null;

    private UnitData            unit_data               = null;
    private bool                is_visible              = false;
    private Enemy               unit_enemy              = null;

    //****************************************************************
    public UnitData UnitData
    {
        get{ return unit_data;  }
        set{ unit_data = value; }
    }
    public Enemy Enemy
    {
        get{ return unit_enemy;  }
        set{ unit_enemy = value; }
    }
    public bool IsVisible
    {
        get{ return is_visible; }
    }

    //****************************************************************
    public void _Start()
    {
        // ...
    }

    //****************************************************************
    public void Show( UnitData udata, Enemy enemy )
    {
        unit_data = udata != null ? udata : unit_data;
        if( unit_data == null )
        {
            Core.Log = "EXCEPTION: UnitData is NULL";
            return;
        }

        UIManager.instance.blockInput = true;

        unit_enemy = enemy;

        this._InitView( unit_data, unit_enemy );

        panel.StartTransition( UIPanelManager.SHOW_MODE.BringInForward );
        panel.AddTempTransitionDelegate
        (
            ( a, b ) =>
            {
                is_visible = true;
                UIManager.instance.blockInput = false;
            }
        );

        Utils.Translate( transform );
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
    private void _InitView( UnitData udata, Enemy enemy )
    {
        lbl_unit_name           .Text = LangController.String_( unit_data.UnitName );
        lbl_unit_reward         .Text = unit_data.UnitReward + "";
        lbl_unit_health_percent .Text = (int)( ( (float)enemy.UnitHealth / (float)unit_data.UnitHealthMax )*100 ) + "";
        lbl_unit_health_value   .Text = string.Format( "{0}/{1}", enemy.UnitHealth, (float)unit_data.UnitHealthMax );

        sc_desc.ClearList( true );
        UIListItemContainer desc_item = sc_desc.CreateItem( t_desc_tmplt.gameObject ).gameObject.GetComponent<UIListItemContainer>();
        desc_item.Text = LangController.String_( unit_data.UnitDesc );
        desc_item.ScanChildren();

        Utils.ClearChilds( t_ico_cont );
        ItemsController.Instance.GetIcon( unit_data.UnitIco, t_ico_cont );
    }

    //****************************************************************
    private void Start()
    {
        btn_close.SetValueChangedDelegate( this._OnBtnClose );
    }

    //****************************************************************
    private void _OnBtnClose( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        this.Hide();
    }
}
