using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Player : Singleton<Player>
{
	private string       player_name      = string.Empty;
    private string       player_id        = "id";
    private float        player_dbirth    = 0;
    private bool         initialized      = false;
    private bool         is_music_enabled = true;
    private bool         is_sound_enabled = true;
    private List<string> map_done_list    = new List<string>();

	//****************************************************************
    public bool IsPlayerInitilized
    {
        get{ return initialized; }
    }
	public string PlayerName
	{
		get{ return player_name; }
		set{ player_name = value; }
	}
    public string PlayerId
	{
		get{ return player_id; }
		set{ player_id = value; }
	}
    public float PlayerDateBirth
	{
		get{ return player_dbirth; }
		set{ player_dbirth = value; }
	}
    public List<string> GetPlayerMapDoneList
    {
        get{ return map_done_list; }
    }
    public bool OptMusicEnabled
    {
        get{ return is_music_enabled;  }
        set{ is_music_enabled = value; }
    }
    public bool OptSoundEnabled
    {
        get{ return is_sound_enabled;  }
        set{ is_sound_enabled = value; }
    }

    //****************************************************************
    public void PlayerMapDoneAdd( string map_id )
    {
        string map = map_done_list.Find( delegate( string s ){ return s == map_id; } );
        if( map == null || map == string.Empty )
        {
            map_done_list.Add( map_id );
        }
    }
    
	//****************************************************************
	public void _Start()
	{
        PlayerSave.Instance._Start();
        initialized = true;

        SoundController.Instance.MusicEnabled = this.is_music_enabled;
        SoundController.Instance.SoundEnabled = this.is_sound_enabled;
	}
}
