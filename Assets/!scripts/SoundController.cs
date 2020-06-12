#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class SoundController : Singleton<SoundController>
{
    [SerializeField]
    private AudioSource   sound_click            = null;
    [SerializeField]
    private AudioSource   sound_click_denail     = null;
    [SerializeField]
    private AudioSource   sound_building_install = null;
    [SerializeField]
    private AudioSource   sound_building_sell    = null;
    [SerializeField]
    private AudioSource   sound_main_theme       = null;
    [SerializeField]
    private AudioSource[] sound_battle_themes    = null;
    [SerializeField]
    private AudioSource[] sound_map_selector     = null;
    [SerializeField]
    private AudioSource   sound_win_theme        = null;
    [SerializeField]
    private AudioSource   sound_lose_theme       = null;
    [SerializeField]
    private AudioSource   sound_explosion_1      = null;
    [SerializeField]
    private AudioSource   sound_explosion_2      = null;
    [SerializeField]
    private AudioSource   sound_explosion_3      = null;
    [SerializeField]
    private AudioSource   sound_shoot_1          = null;

    private bool          sound_enabled          = true;
    private bool          music_enabled          = true;
    private AudioSource   current_music          = null;

    //****************************************************************
    public bool SoundEnabled
    {
        get{ return sound_enabled;  }
        set{ sound_enabled = value; }
    }
    public bool MusicEnabled
    {
        get{ return music_enabled;  }
        set
        {
            music_enabled = value;

            if( !music_enabled )
                this.StopAllMusic();
        }
    }

    //****************************************************************
    public void Play_SoundClick()
    {
        if( sound_enabled )
            this._Play( sound_click, true );
    }

    //****************************************************************
    public void Play_SoundClickDenail()
    {
        if( sound_enabled )
            this._Play( sound_click_denail, true );
    }

    //****************************************************************
    public void Play_SoundBuildingInstall()
    {
        if( sound_enabled )
            this._Play( sound_building_install, true );
    }

    //****************************************************************
    public void Play_SoundBuildingSell()
    {
        if( sound_enabled )
            this._Play( sound_building_sell, true );
    }

    //****************************************************************
    public void Play_SoundMainTheme()
    {
        if( music_enabled )
            this._Play( sound_main_theme, true );
    }

    //****************************************************************
    public void Play_SoundBattleThemeRnd()
    {
        if( music_enabled )
        {
            this.StopAllMusic();
            int index = Utils.Random( 0, sound_battle_themes.Length );
            this._Play( sound_battle_themes[ index ], true );
        }
    }

    //****************************************************************
    public void Play_SoundMapSelectorRnd()
    {
        if( music_enabled && sound_map_selector.Length > 0 )
        {
            this.StopAllMusic();
            int index = Utils.Random( 0, sound_map_selector.Length );
            this._Play( sound_map_selector[ index ], true );
        }
    }

    //****************************************************************
    public void Play_SoundWinTheme()
    {
        if( music_enabled )
            this._Play( sound_win_theme, true );
    }

    //****************************************************************
    public void Play_SoundLoseTheme()
    {
        if( music_enabled )
            this._Play( sound_lose_theme, true );
    }

    //****************************************************************
    public void Play_SoundExplosion1()
    {
        if( sound_enabled )
            this._Play( sound_explosion_1, true );
    }

    //****************************************************************
    public void Play_SoundExplosion2()
    {
        if( sound_enabled )
            this._Play( sound_explosion_2, true );
    }

    //****************************************************************
    public void Play_SoundExplosion3()
    {
        if( sound_enabled )
            this._Play( sound_explosion_3, true );
    }

    //****************************************************************
    public void Play_SoundShoot1()
    {
        if( sound_enabled )
            this._Play( sound_shoot_1, true );
    }

    //****************************************************************
    public string Play_Sound
	{
		set
		{
			switch( value )
			{
			case "sound-click":
				this.Play_SoundClick();
				break;

            case "sound-click-denail":
				this.Play_SoundClickDenail();
				break;

            case "sound-building-install":
				this.Play_SoundBuildingInstall();
				break;

            case "sound-building-sell":
				this.Play_SoundBuildingSell();
				break;

            case "main-theme":
				this.Play_SoundMainTheme();
				break;

            case "main-map-selector-rnd":
				this.Play_SoundMapSelectorRnd();
				break;

            case "main-battle-rnd":
				this.Play_SoundBattleThemeRnd();
				break;

            case "sound-win-theme":
				this.Play_SoundWinTheme();
				break;

            case "sound-lose-theme":
				this.Play_SoundLoseTheme();
				break;

            case "sound-explosion-1":
				this.Play_SoundExplosion1();
				break;

            case "sound-explosion-2":
				this.Play_SoundExplosion2();
				break;

            case "sound-explosion-3":
				this.Play_SoundExplosion3();
				break;

            case "sound-shoot-1":
				this.Play_SoundShoot1();
				break;
			}
		}
	}

    //****************************************************************
	public void StopAll()
	{
		this.StopAllSounds();
        this.StopAllMusic();
	}

	//****************************************************************
	public void StopAllSounds()
	{
		sound_click           .Stop();
        sound_click_denail    .Stop();
        sound_building_install.Stop();
        sound_building_sell   .Stop();
        sound_explosion_1     .Stop();
        sound_explosion_2     .Stop();
        sound_explosion_3     .Stop();
        sound_shoot_1         .Stop();
	}

    //****************************************************************
	public void StopAllMusic()
	{
		sound_main_theme.Stop();
        sound_win_theme .Stop();
        sound_lose_theme.Stop();

        foreach( AudioSource a in sound_battle_themes )
        {
            if( a.isPlaying )
                a.Stop();
        }

        foreach( AudioSource a in sound_map_selector )
        {
            if( a.isPlaying )
                a.Stop();
        }
	}

    //****************************************************************
	private void _Play( AudioSource asource, bool play )
	{
		if( play )
		{
			if( asource != null ) // prevent exception on start
				asource.Play();
		}
		else
		{
			if( asource != null ) // prevent exception on start
				if( asource.isPlaying )
					asource.Stop();
		}
	}

    //****************************************************************
    private void FixedUpdate()
    {
        if( music_enabled && current_music != null )
        {
            if( !current_music.isPlaying )
            {
                // ...
            }
        }
    }
}
