#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using MapData    = defines.MapData;
using WeaponData = defines.WeaponData;

public class BattlefieldArcenalItemView : MonoBehaviour
{
    [SerializeField]
    private UIButton   btn         = null;
    [SerializeField]
    private Transform  ico_cont    = null;
    private WeaponData weapon_data = null;

    //****************************************************************
    public WeaponData WeaponData
    {
        get{ return weapon_data;  }
        set
        {
            weapon_data = value;

            Utils.ClearChilds( ico_cont );

            if( weapon_data != null )
                ItemsController.Instance.GetIcon( weapon_data.WpnIco, ico_cont );
            else
                ItemsController.Instance.GetIcon( "wpn-null", ico_cont );
        }
    }

    //****************************************************************
    private void Start()
    {
        btn.SetValueChangedDelegate( this._OnBtn );
    }

    //****************************************************************
    private void _OnBtn( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        if( BattlefieldController.Instance.HasBuildingToInstall == null )
        {
            BattlefieldUI.Instance.OnBtnWeapon( weapon_data );
        }
        else
        {
            Destroy( BattlefieldController.Instance.HasBuildingToInstall.gameObject );
            BattlefieldController.Instance.HasBuildingToInstall = null;
            BattlefieldSectors.Instance.Hide();
        }

        Building b_selected = BattlefieldController.Instance.GetBuildingSelected();
        if( b_selected != null )
        {
            WindowBuildingInfo.Instance.Hide();
        }
    }
}
