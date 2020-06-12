#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class WindowWinLose : Singleton<WindowWinLose>
{
    [SerializeField]
    private SpriteText lbl_title      = null;
    [SerializeField]
    private UIButton   btn_ok         = null;
    [SerializeField]
    private string     lbl_win_title  = null;
    [SerializeField]
    private string     lbl_lose_title = null;
    [SerializeField]
    private Vector3    pos_visible    = new Vector3( 0, 0, -995 );
    [SerializeField]
    private Vector3    pos_hidden     = new Vector3( 0, 0, 2001 );

    //****************************************************************
    public void ShowWin()
    {
        SoundController.Instance.StopAllMusic();
        SoundController.Instance.Play_SoundWinTheme();

        this._OnShow();

        lbl_title.Text = LangController.String_( lbl_win_title );
    }

    //****************************************************************
    public void ShowLose()
    {
        SoundController.Instance.StopAllMusic();
        SoundController.Instance.Play_SoundLoseTheme();

        this._OnShow();

        lbl_title.Text = LangController.String_( lbl_lose_title );
    }

    //****************************************************************
    private IEnumerator _CorOnShow()
    {
        if( SoundController.Instance.MusicEnabled )
            yield return new WaitForSeconds( 5 );
        else
            yield return new WaitForSeconds( 0 );

        btn_ok.controlIsEnabled = true;
    }

    //****************************************************************
    private void _OnShow()
    {
        gameObject.SetActive( true );
        transform.position = pos_visible;
        Utils.Translate( transform );

        StartCoroutine( this._CorOnShow() );
    }
	
	//****************************************************************
	private void Start()
	{
		btn_ok.SetValueChangedDelegate( this._OnBtnOk );
        btn_ok.controlIsEnabled = false;
	}

    //****************************************************************
    private void _OnBtnOk( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        Core.Instance.LoadScene( "map-selector" );
    }
}
