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
using Upgrade      = defines.WeaponData.Upgrade;
using WeaponArea   = defines.WeaponArea;
using BuildingData = defines.BuildingData;

public class BuildingController : Singleton<BuildingController>
{
    [SerializeField]
    private string                dialog_building_info_prefab_name    = "window-building-info";
    private string                dialog_building_details_prefab_name = "window-building-details";
    private WindowBuildingInfo    dialog_building_info                = null;
    private WindowBuildingDetails window_building_details             = null;
    private List<WeaponData>      weapon_data_list                    = new List<WeaponData>();
    private Building              building                            = null;

    //****************************************************************
    public List<WeaponData> GetWeaponDataAll()
    {
        return weapon_data_list;
    }

    //****************************************************************
    public WeaponData GetWeaponData( string wpn_id )
    {
        if( wpn_id == string.Empty || wpn_id == "NULL" ) return null;

        return weapon_data_list.Find( delegate( WeaponData wd ){ return wd.WpnId == wpn_id; } );
    }

	//****************************************************************
	public void _Start()
	{
		this._LoadXmlWeapons();
	}

    //****************************************************************
    public void ShowDialogBuildingInfo( BuildingData bd )
    {
        this._LoadDialogBuildingInfo();
        dialog_building_info.BuildingData = bd;
        dialog_building_info.Show();
    }

    //****************************************************************
    public void ShowDialogBuildingInfo( Building b )
    {
        if( WindowUnitInfo.Instance != null )
        {
            if( WindowUnitInfo.Instance.IsVisible )
            {
                WindowUnitInfo.Instance.Hide();
            }
        }

        building = b;

        this._LoadDialogBuildingInfo();

        dialog_building_info.Building     = b;
        dialog_building_info.BuildingData = building.GetBuildingData();
        dialog_building_info.Show();
    }

    //****************************************************************
    public void ShowDialogBuildingDetails()
    {
        this._LoadDialogBuildingDetails();
        window_building_details.Show();
    }

    //****************************************************************
    public void OnBuildingSell()
    {
        if( building == null )
        {
            Core.Log = "EXCEPTION: Building is NULL";
            return;
        }

        BattlefieldController.Instance.MapDataPlayer.PlayerBalance += building.GetBuildingData().WeaponSellPrice;

        building.Remove();
        building = null;

        BattlefieldController.Instance.OnBuildingSelled();
    }

    //****************************************************************
    private void _LoadDialogBuildingInfo()
    {
        if( dialog_building_info == null )
        {
            dialog_building_info = Utils.FindComp<WindowBuildingInfo>();
            if( dialog_building_info == null )
            {
                dialog_building_info = Utils.LoadPrefab( dialog_building_info_prefab_name ).GetComponent<WindowBuildingInfo>();
            }
            dialog_building_info._Start();
        }
    }

    //****************************************************************
    private void _LoadDialogBuildingDetails()
    {
        if( window_building_details == null )
        {
            window_building_details = Utils.FindComp<WindowBuildingDetails>();
            if( window_building_details == null )
            {
                window_building_details = Utils.LoadPrefab( dialog_building_details_prefab_name ).GetComponent<WindowBuildingDetails>();
            }
            window_building_details._Start();
        }
    }
    
    //****************************************************************
    private void _LoadXmlWeapons()
    {
        XmlNodeList node_list = Utils.LoadXml( defines.XML_PATH_WEAPONS ).GetElementsByTagName( "i" );
        foreach( XmlNode node in node_list )
        {
            WeaponData wpndata = new WeaponData();

            wpndata.WpnId           = node.Attributes[ "id"   ].Value;
            wpndata.WpnName         = node.Attributes[ "name" ].Value;
            wpndata.WpnDesc         = node.Attributes[ "desc" ].Value;
            wpndata.WpnIco          = node.Attributes[ "ico"  ].Value;
            wpndata.WpnTexture      = node.Attributes[ "tex"  ].Value;
            wpndata.WpnPrice        = int.Parse( node.Attributes[ "price"  ].Value );
            wpndata.WpnRange        = int.Parse( node.Attributes[ "range"  ].Value );
            wpndata.WpnDamage       = int.Parse( node.Attributes[ "damage" ].Value );
            wpndata.WpnFirerate     = float.Parse( node.Attributes[ "fire-rate" ].Value );
            wpndata.WeaponArea      = Utils.GetWeaponArea( node.Attributes[ "area" ].Value );
            wpndata.WeaponSellPrice = int.Parse( node.Attributes[ "sell-price" ].Value );

            XmlNodeList upgrade_list = node.ChildNodes;
            wpndata.WeaponUpgrades = new Upgrade[ upgrade_list.Count ];
            foreach( XmlNode node_upgrade in upgrade_list )
            {
                int upgrade_id = int.Parse( node_upgrade.Attributes[ "id" ].Value );
                wpndata.WeaponUpgrades[ upgrade_id ] = new Upgrade();
                
                wpndata.WeaponUpgrades[ upgrade_id ].UPrice          = int.Parse( node_upgrade.Attributes[ "price"      ].Value );
                wpndata.WeaponUpgrades[ upgrade_id ].UBuildTime      = int.Parse( node_upgrade.Attributes[ "time"       ].Value );
                wpndata.WeaponUpgrades[ upgrade_id ].URangeBonus     = int.Parse( node_upgrade.Attributes[ "range"      ].Value );
                wpndata.WeaponUpgrades[ upgrade_id ].UFirerateBonus  = float.Parse( node_upgrade.Attributes[ "firerate" ].Value );
                wpndata.WeaponUpgrades[ upgrade_id ].UDamageBonus    = int.Parse( node_upgrade.Attributes[ "damage"     ].Value );
                wpndata.WeaponUpgrades[ upgrade_id ].USellPriceBonus = int.Parse( node_upgrade.Attributes[ "sell-price" ].Value );
            }

            weapon_data_list.Add( wpndata );
        }
    }

    //****************************************************************
    //private void OnGUI()
    //{
    //    if( GUI.Button( new Rect( 10, 60, 100, 30 ), "Show Building Info" ) )
    //    {
    //        BuildingData bd = new BuildingData();
    //        bd.WeaponData = weapon_data_list[ Utils.Random( 0, weapon_data_list.Count ) ];

    //        this.ShowDialogBuildingInfo( bd );
    //    }
    //}
}
