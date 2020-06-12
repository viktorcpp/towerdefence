#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using SceneType                = defines.SceneType;
using GameMusicChangeHandler   = defines.GameMusicChangeHandler;
using GameSoundChangeHandler   = defines.GameSoundChangeHandler;
using OptionsVisibilityHandler = defines.OptionsVisibilityHandler;

public class WindowOptions : ModalDialog
{
    [SerializeField]
    private UIStateToggleBtn         btn_switch_music      = null;
    [SerializeField]
    private UIStateToggleBtn         btn_switch_sound      = null;
    private bool                     is_initialized_view   = false;
    private GameMusicChangeHandler   del_music_change      = null;
    private GameSoundChangeHandler   del_sound_change      = null;
    private OptionsVisibilityHandler del_visibility_change = null;

    //****************************************************************
    public void SetTempVisibilityChangeHandler( OptionsVisibilityHandler vc )
    {
        del_visibility_change = vc;
    }

    //****************************************************************
    public void SetTempMusicChangeHandler( GameMusicChangeHandler mc )
    {
        del_music_change = mc;
    }

    //****************************************************************
    public void SetTempSoundChangeHandler( GameSoundChangeHandler mc )
    {
        del_sound_change = mc;
    }

	//****************************************************************
	public override void _Start()
	{
		base._Start();

        Utils.Translate( transform );
	}
	
	//****************************************************************
	public override void Show()
	{
		base.Show();

        btn_switch_music.SetToggleState( Game.Player.OptMusicEnabled ? "on" : "off", false );
        btn_switch_sound.SetToggleState( Game.Player.OptSoundEnabled ? "on" : "off", false );

        is_initialized_view = true;

        if( del_visibility_change != null )
            del_visibility_change( true );
	}
	
	//****************************************************************
	public override void Hide()
	{
		base.Hide();

        del_music_change    = null;
        del_sound_change    = null;

        is_initialized_view = false;

        if( del_visibility_change != null )
            del_visibility_change( false );

        del_visibility_change = null;
	}

    //****************************************************************
	public override void OnBtnClose( IUIObject btn )
	{
        SoundController.Instance.Play_SoundClick();
		this.Hide();
        PlayerSave.Save();
	}

    //****************************************************************
	private void Start()
	{
		btn_switch_music.SetValueChangedDelegate( this._OnBtnMusic );
        btn_switch_sound.SetValueChangedDelegate( this._OnBtnSound );
	}

    //****************************************************************
    private void _OnBtnMusic( IUIObject btn )
    {
        if( !is_initialized_view ) return;

        SoundController.Instance.Play_SoundClick();

        UIStateToggleBtn btn_state            = (UIStateToggleBtn)btn;
        SoundController.Instance.MusicEnabled = btn_state.StateName == "on" ? true : false;

        if( del_music_change != null )
        {
            del_music_change( SoundController.Instance.MusicEnabled );
        }

        if( del_sound_change != null )
        {
            del_sound_change( SoundController.Instance.SoundEnabled );
        }
        
        Game.Player.OptMusicEnabled = SoundController.Instance.MusicEnabled;
    }

    //****************************************************************
    private void _OnBtnSound( IUIObject btn )
    {
        if( !is_initialized_view ) return;

        SoundController.Instance.Play_SoundClick();

        UIStateToggleBtn btn_state            = (UIStateToggleBtn)btn;
        SoundController.Instance.SoundEnabled = btn_state.StateName == "on" ? true : false;

        Game.Player.OptSoundEnabled = SoundController.Instance.SoundEnabled;
    }

    //****************************************************************
    //public void OnGUI()
    //{
    //    if( GUI.Button( new Rect( 10, 20, 100, 30 ), "Show" ) )
    //    {
    //        this._Start();
    //        this.Show();
    //    }
    //}
}
