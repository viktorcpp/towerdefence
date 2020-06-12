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

public class UnitsController : Singleton<UnitsController>
{
    private List<UnitData> unit_data_list = new List<UnitData>();

    //****************************************************************
    public UnitData GetUnitData( string unit_id )
    {
        return unit_data_list.Find( delegate( UnitData ud ){ return ud.UnitId == unit_id; } );
    }

    //****************************************************************
    public List<UnitData> GetUnitDataList()
    {
        return unit_data_list;
    }

	//****************************************************************
	public void _Start()
	{
		this._LoadXml();
	}

    //****************************************************************
    private void _LoadXml()
    {
        XmlNodeList node_list = Utils.LoadXml( defines.XML_PATH_UNITS ).GetElementsByTagName( "i" );
        foreach( XmlNode node in node_list )
        {
            UnitData udata = new UnitData();

            udata.UnitId        = node.Attributes[ "id"   ].Value;
            udata.UnitName      = node.Attributes[ "name" ].Value;
            udata.UnitDesc      = node.Attributes[ "desc" ].Value;
            udata.UnitIco       = node.Attributes[ "ico"  ].Value;
            udata.UnitTex       = node.Attributes[ "tex"  ].Value;
            udata.UnitReward    = int.Parse( node.Attributes[ "reward" ].Value );
            udata.UnitHealthMax = int.Parse( node.Attributes[ "health" ].Value );

            unit_data_list.Add( udata );
        }
    }
}
