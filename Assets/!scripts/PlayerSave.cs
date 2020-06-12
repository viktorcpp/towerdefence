using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class PlayerSave : Singleton<PlayerSave>
{
    //****************************************************************
    public static void Save()
    {
        if( Application.isEditor )
        {
            PlayerSave.Instance.SaveEditor();
        }
        else
        {
            PlayerSave.Instance.SaveReal();
        }
    }

    //****************************************************************
    public static void InitSave()
    {
        if( Application.isEditor )
        {
            PlayerSave.Instance.InitSaveEditor();
        }
        else
        {
            PlayerSave.Instance.InitSaveReal();
        }
    }

	//****************************************************************
	public void _Start()
	{
		PlayerSave.InitSave();
	}

    //****************************************************************
    private void InitSaveEditor()
    {
        string path_folder = Utils.GetPathSaveFolderEditorMode();
        string path_save   = path_folder + "/save.xml";
            
        if( !Directory.Exists( path_folder ) )
        {
            Directory.CreateDirectory( path_folder );
            File.Create( path_save ).Close();

            this.SaveEditor();
        }

        if( !File.Exists( path_save ) )
        {
            File.Create( path_save ).Close();

            this.SaveEditor();
        }

        this.LoadEditor();
    }

    //****************************************************************
    private void SaveEditor()
    {
        string path_save = Utils.GetPathSaveFolderEditorMode() + "/save.xml";
        string str_out   = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + '\n' + '\n';
        str_out         += "<save version=\"1.0\">" + '\n';

        //* MAIN *\\
        str_out += string.Format
        (
            "    <player name=\"{0}\" id=\"{1}\" datebirth=\"{2}\" />" + '\n',
            Game.Player.PlayerName,
            Game.Player.PlayerId,
            Game.Player.PlayerDateBirth
        );

        //* OPTIONS *\\
        str_out += string.Format
        (
            "    <opt music=\"{0}\" sound=\"{1}\" />" + '\n',
            Game.Player.OptMusicEnabled ? 1 : 0,
            Game.Player.OptSoundEnabled ? 1 : 0
        );

        //* MAPS DONE *\\
        str_out += "    <maps>" + '\n';
        foreach( string s in Game.Player.GetPlayerMapDoneList )
        {
            str_out += string.Format
            (
                "        <i id=\"{0}\" cnt=\"0\" />" + '\n',
                s
            );
        }
        str_out += "    </maps>" + '\n';

        str_out += "</save>" + '\n' + '\n';

        File.WriteAllText( path_save, str_out );
    }

    //****************************************************************
    private void LoadEditor()
    {
        string xml_text = string.Empty;
        try
        {
            xml_text = File.ReadAllText( Utils.GetPathSaveFolderEditorMode() + "/save.xml" );
        }
        catch( System.Exception exc )
        {
            Core.Log = exc.Message;
            return;
        }

        XmlDocument doc = new XmlDocument();
        doc.LoadXml( xml_text );

        //* MAIN *\\
        XmlNode player_node = doc.GetElementsByTagName("player")[ 0 ];
        Game.Player.PlayerName      = player_node.Attributes[ "name" ].Value;
        Game.Player.PlayerId        = player_node.Attributes[ "id"   ].Value;
        Game.Player.PlayerDateBirth = float.Parse( player_node.Attributes[ "datebirth" ].Value );

        //* OPTIONS *\\
        XmlNode options_node = doc.GetElementsByTagName( "opt" )[0];
        Game.Player.OptMusicEnabled = int.Parse( options_node.Attributes[ "music" ].Value ) == 1;
        Game.Player.OptSoundEnabled = int.Parse( options_node.Attributes[ "sound" ].Value ) == 1;

        //* MAPS DONE *\\
        XmlNodeList node_maps_list = doc.GetElementsByTagName( "maps" )[0].ChildNodes;
        foreach( XmlNode node_map in node_maps_list )
        {
            Game.Player.GetPlayerMapDoneList.Add( node_map.Attributes[ "id" ].Value );
        }

        PlayerSave.Save();
    }

    //****************************************************************
    private void InitSaveReal()
    {
        bool save_exists = PlayerPrefs.GetString( "id" ) == string.Empty ? false : true;

        if( !save_exists )
        {
            this.SaveReal();
        }

        this.LoadReal();
    }

    //****************************************************************
    private void SaveReal()
    {
        //* MAIN *\\
        PlayerPrefs.SetString( "name",      Player.Instance.PlayerName      );
        PlayerPrefs.SetString( "id",        Player.Instance.PlayerId        );
        PlayerPrefs.SetFloat ( "datebirth", Player.Instance.PlayerDateBirth );

        //* OPTIONS *\\
        string str_out = string.Format
        (
            "<opt music=\"{0}\" sound=\"{1}\" />",
            Game.Player.OptMusicEnabled ? 1 : 0,
            Game.Player.OptSoundEnabled ? 1 : 0
        );
        PlayerPrefs.SetString( "opt", str_out );

        //* MAPS DONE *\\
        str_out = "<maps>";
        foreach( string s in Game.Player.GetPlayerMapDoneList )
        {
            str_out += string.Format
            (
                "<i id=\"{0}\" cnt=\"0\" />",
                s
            );
        }
        str_out += "</maps>";
        PlayerPrefs.SetString( "maps", str_out );
    }

    //****************************************************************
    private void LoadReal()
    {
        try
        {
            //* MAIN *\\
            Game.Player.PlayerName      = PlayerPrefs.GetString( "name"      );
            Game.Player.PlayerId        = PlayerPrefs.GetString( "id"        );
            Game.Player.PlayerDateBirth = PlayerPrefs.GetFloat ( "datebirth" );

            //* OPTIONS *\\
            XmlNode node_options = Utils.LoadXmlStr( PlayerPrefs.GetString( "opt" ) ).GetElementsByTagName( "opt" )[0];
            Game.Player.OptMusicEnabled = int.Parse( node_options.Attributes[ "music" ].Value ) == 1;
            Game.Player.OptSoundEnabled = int.Parse( node_options.Attributes[ "sound" ].Value ) == 1;

            //* MAPS DONE *\\
            XmlNodeList node_maps_list = Utils.LoadXmlStr( PlayerPrefs.GetString( "maps" ) ).GetElementsByTagName( "i" );
            foreach( XmlNode node_map in node_maps_list )
            {
                Game.Player.GetPlayerMapDoneList.Add( node_map.Attributes[ "id" ].Value );
            }
        }
        catch( System.Exception exc )
        {
            Core.Log = exc.Message;
            Core.Log = exc.StackTrace;
        }
    }
}
