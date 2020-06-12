using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class Options : Singleton<Options>
{
    [SerializeField]
    private bool     is_dev_enabled          = false;
    [SerializeField]
    private bool     is_dev_showfps          = false;
    [SerializeField]
	private bool     is_dev_showversion      = false;

    private Dictionary<string, string> options = new Dictionary<string, string>();

    //****************************************************************
    public float ValueF( string key )
    {
        return float.Parse( this._Value( key ) );
    }

    //****************************************************************
    public int ValueI( string key )
    {
        return int.Parse( this._Value( key ) );
    }

    //****************************************************************
    public string ValueS( string key )
    {
        return this._Value( key );
    }

    //****************************************************************
    public bool ValueB( string key )
    {
        return int.Parse( this._Value( key ) ) == 1 ? true : false;
    }

	//****************************************************************
	public void _Start()
	{
        options.Add( "is-dev-showfps",     is_dev_showfps     ? 1 + "" : 0 + "" );
        options.Add( "is-dev-showversion", is_dev_showversion ? 1 + "" : 0 + "" );
        options.Add( "is-dev-enabled",     is_dev_enabled         + "" );
	}

    //****************************************************************
    private string _Value( string key )
    {
        string val = string.Empty;
        options.TryGetValue( key, out val );

        return val;
    }
}
