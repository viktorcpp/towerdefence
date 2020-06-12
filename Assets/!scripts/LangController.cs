using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class LangController : Singleton<LangController>
{
	private Dictionary<string, DictData> strings = new Dictionary<string, DictData>();
	private string                       log     = string.Empty;
	private SystemLanguage               lang    = SystemLanguage.English;

	//****************************************************************
	public string Log
	{
		get{ return log; }
	}

	//****************************************************************
	public void _Start()
	{
        lang = Application.systemLanguage;
		this._Load( defines.XML_PATH_LANGUAGE );
	}

    //****************************************************************
    public static string String_( string string_id )
    {
        return LangController.Instance.String( string_id );
    }

	//****************************************************************
	public string String( string string_id )
	{
		DictData dict_str = null;
		string   str      = string_id;

		strings.TryGetValue( string_id, out dict_str );

		if( dict_str != null )
		{
			switch( lang )
			{
			case SystemLanguage.Russian:
				str = dict_str.StringRU;
				break;

			case SystemLanguage.English:
				str = dict_str.StringEN;
				break;

			case SystemLanguage.Dutch:
				str = dict_str.StringDE;
				break;

            default:
				str = dict_str.StringEN;
				break;
			}
		}
		else
		{
			log += "requested string \"" + string_id + "\" not found\n";
		}

		return str;
	}

	//****************************************************************
	private void _Load( string path )
	{
		XmlDocument doc  = Utils.LoadXml( path );
		XmlNodeList list = doc.GetElementsByTagName( "label" );

		foreach( XmlNode i in list )
		{
			DictData dd = new DictData();

            dd.StringRU = i[ "ru" ].InnerText.Trim();
			dd.StringEN = i[ "en" ].InnerText.Trim();
			dd.StringDE = i[ "de" ].InnerText.Trim();

			string id = i[ "id" ].InnerText.Trim();

			strings.Add( id, dd );
		}
	}
}



public class DictData
{
	private string ru = string.Empty;
	private string de = string.Empty;
	private string en = string.Empty;

	public string StringRU
	{
		get{ return ru; }
		set{ ru = value; }
	}
	public string StringDE
	{
		get { return de; }
		set { de = value; }
	}
	public string StringEN
	{
		get { return en; }
		set { en = value; }
	}
}
