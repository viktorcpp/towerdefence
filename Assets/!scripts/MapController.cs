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

public class MapController : Singleton<MapController>
{
    private List<MapData> map_data_list     = new List<MapData>();
    private MapData       map_data_selected = null;

    //****************************************************************
    public List<MapData> GetMapDataList
    {
        get{ return map_data_list; }
    }
    public MapData MapDataSelected
    {
        get{ return map_data_selected;  }
        set{ map_data_selected = value; }
    }

	//****************************************************************
	public void _Start()
	{
		this._LoadXml();
	}

    //****************************************************************
    public MapData GetMapData( string map_id )
    {
        return map_data_list.Find( delegate( MapData md ){ return md.MapId == map_id; } );
    }

    //****************************************************************
    private void _LoadXml()
    {
        XmlNodeList node_list = Utils.LoadXml( defines.XML_PATH_MAPS ).GetElementsByTagName( "i" );
        foreach( XmlNode node in node_list )
        {
            MapData md = new MapData();

            md.MapId             = node.Attributes[ "id"   ].Value;
            md.MapName           = node.Attributes[ "name" ].Value;
            md.MapBalance        = int.Parse( node.Attributes[ "balance"         ].Value );
            md.MapWavesCount     = int.Parse( node.Attributes[ "waves"           ].Value );
            md.MapEnemyesInWave  = int.Parse( node.Attributes[ "enemyes-in-wave" ].Value );
            md.MapLoseWavesLimit = int.Parse( node.Attributes[ "lose-limit" ].Value );
            md.MapTimer          = int.Parse( node.Attributes[ "timer"           ].Value );
            md.MapIcoBig         = node.Attributes[ "ico-big"   ].Value;
            md.MapIcoSmall       = node.Attributes[ "ico-small" ].Value;
            md.MapRequired       = node.Attributes[ "map-req"   ].Value;
            md.MapSpawn          = node.Attributes[ "spawn"     ].Value;
            md.MapTarget         = node.Attributes[ "target"    ].Value;

            string[] arcenal = node[ "arcenal" ].Attributes[ "wpn" ].Value.Split( ',' );
            md.MapArcenal = new string[ arcenal.Length ];
            for( int x = 0; x < arcenal.Length; x++ )
            {
                md.MapArcenal[ x ] = arcenal[ x ].Trim();
            }

            string[] waves = node[ "w" ].Attributes[ "list" ].Value.Split( ',' );
            md.MapWaves = new string[ waves.Length ];
            for( int x = 0; x < waves.Length; x++ )
            {
                md.MapWaves[ x ] = waves[ x ].Trim();
            }

            map_data_list.Add( md );
        }
    }
}
