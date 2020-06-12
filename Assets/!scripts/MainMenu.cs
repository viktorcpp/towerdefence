#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using MessageBoxType         = defines.MessageBoxType;
using MessageBoxRet          = defines.MessageBoxRet;
using GameMusicChangeHandler = defines.GameMusicChangeHandler;
using GameSoundChangeHandler = defines.GameSoundChangeHandler;

public class MainMenu : Singleton<MainMenu>
{
    [SerializeField]
    private UIButton      btn_play       = null;
    [SerializeField]
    private UIButton      btn_options    = null;
    [SerializeField]
    private UIButton      btn_test       = null;
    [SerializeField]
    private UIButton      btn_exit       = null;
    [SerializeField]
    private UIButton      btn_about      = null;
    private WindowOptions dialog_options = null;

	//****************************************************************
	private void Start()
	{
		btn_play   .SetValueChangedDelegate( this._OnBtnPlay    );
        btn_options.SetValueChangedDelegate( this._OnBtnOptions );
        btn_test   .SetValueChangedDelegate( this._OnBtnTest    );
        btn_exit   .SetValueChangedDelegate( this._OnBtnExit    );
        btn_about  .SetValueChangedDelegate( this._OnBtnAbout   );

        btn_test.controlIsEnabled = false;
	}

    //****************************************************************
    private void _OnBtnPlay( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        Core.Instance.LoadScene( "map-selector" );
    }

    //****************************************************************
    private void _OnBtnOptions( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        this._LoadDialogOptions();
        dialog_options.SetTempMusicChangeHandler( this._OptionsMusicChange );
        dialog_options.Show();
    }

    //****************************************************************
    private void _OptionsMusicChange( bool play )
    {
        if( play )
            SoundController.Instance.Play_SoundMainTheme();
        else
            SoundController.Instance.StopAllMusic();
    }

    //****************************************************************

    //****************************************************************
    private void _LoadDialogOptions()
    {
        if( dialog_options == null )
        {
            dialog_options = Utils.FindComp<WindowOptions>();
            if( dialog_options == null )
            {
                dialog_options = Utils.LoadPrefab( "window-options" ).GetComponent<WindowOptions>();
            }
            dialog_options._Start();
        }
    }

    //****************************************************************
    private void _OnBtnTest( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();
    }

    //****************************************************************
    private void _OnBtnExit( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        MessageBox.Instance.Bind( this._OnConfirmExit );
        MessageBox.Instance.Init( LangController.String_( "lbl-confirm-exit" ), MessageBoxType.OKCANCEL, ItemsController.Instance.GetIcon( "ico-exit-confirm", null ) );
    }

    //****************************************************************
    private void _OnBtnAbout( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        MessageBox.Instance.Bind( this._OnConfirmAbout );
        MessageBox.Instance.Init( LangController.String_( "lbl-about" ), MessageBoxType.OK );
    }

    //****************************************************************
    private void _OnConfirmExit( MessageBoxRet ret )
    {
        if( ret == MessageBoxRet.OK )
        {
            Application.Quit();
        }
    }

    //****************************************************************
    private void _OnConfirmAbout( MessageBoxRet ret )
    {
        // ...
    }
}
