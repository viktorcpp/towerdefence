using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using MessageBoxType       = defines.MessageBoxType;
using MessageBoxRet        = defines.MessageBoxRet;
using MessageBoxRetHandler = defines.MessageBoxRetHandler;

public class MessageBox : Singleton<MessageBox>
{
    [SerializeField]
    private UIButton             btn_ok           = null;
    [SerializeField]
    private UIButton             btn_retry        = null;
    [SerializeField]
    private UIButton             btn_cancel       = null;
    [SerializeField]
    private UIScrollList         desc_scroll      = null;
    [SerializeField]
    private UIListItemContainer  desc_lbl_tmplt   = null;
    [SerializeField]
    private Transform            t_ico_cont       = null;
    [SerializeField]
    private string               lbl_ok           = "lbl-ok";
    [SerializeField]
    private string               lbl_cancel       = "lbl-cancel";
    [SerializeField]
    private string               lbl_retry        = "lbl-retry";
    private MessageBoxType       type             = MessageBoxType.NULL;
    private Vector3              old_overlay_pos  = Vector3.one;
    private Transform            overlay          = null;
    private MessageBoxRetHandler del              = null;
    private bool                 is_open_as_modal = false;

    //****************************************************************
    public string MsgText
    {
        set{ desc_lbl_tmplt.Text = value; }
    }
    public Transform MsgIco
    {
        set
        {
            Utils.ClearChilds( t_ico_cont );

            if( value == null ) return;

            value.parent        = t_ico_cont;
            value.localPosition = Vector3.zero;
            value.localScale    = Vector3.one;
        }
    }
    public MessageBoxType MsgType
    {
        set
        {
            type = value;

            btn_ok    .gameObject.SetActive( false );
            btn_retry .gameObject.SetActive( false );
            btn_cancel.gameObject.SetActive( false );

            switch( type )
            {
            case MessageBoxType.OK:
                btn_ok.gameObject.SetActive( true );
                btn_ok.Text = LangController.String_( lbl_ok );
                btn_ok.SetValueChangedDelegate( this.OnBtnOk );
                break;

            case MessageBoxType.OKCANCEL:
                btn_retry .gameObject.SetActive( true );
                btn_cancel.gameObject.SetActive( true );
                btn_retry.Text  = LangController.String_( lbl_ok );
                btn_cancel.Text = LangController.String_( lbl_cancel );
                btn_retry .SetValueChangedDelegate( this.OnBtnOk     );
                btn_cancel.SetValueChangedDelegate( this.OnBtnCancel );
                break;

            case MessageBoxType.RETRYCANCEL:
                btn_retry .gameObject.SetActive( true );
                btn_cancel.gameObject.SetActive( true );
                btn_retry.Text  = LangController.String_( lbl_retry );
                btn_cancel.Text = LangController.String_( lbl_cancel );
                btn_retry .SetValueChangedDelegate( this.OnBtnRetry  );
                btn_cancel.SetValueChangedDelegate( this.OnBtnCancel );
                break;
            }
        }
    }

    //****************************************************************
    public void Init( string msg = "?", MessageBoxType type = MessageBoxType.OK, Transform ico = null )
    {
        if( Game.IsModalDialogOpened )
        {
            is_open_as_modal = false;
        }
        else
        {
            is_open_as_modal = true;
        }

        this.MsgText = msg;
        this.MsgType = type;
        this.MsgIco  = ico;

        this.Show();
    }

    //****************************************************************
    public void Bind( MessageBoxRetHandler new_del )
    {
        del = new_del;
    }

    //****************************************************************
    public void Show()
    {
        desc_scroll.CreateItem( desc_lbl_tmplt.gameObject );
        overlay                 = Overlay.Instance.GetSceneObject;
        old_overlay_pos         = overlay.localPosition;
        transform.localPosition = new Vector3( 0, 0, -999.0f );
        overlay.localPosition   = new Vector3( 0, 0, -998.0f );
        overlay.gameObject.SetActive( true );
    }

    //****************************************************************
    public void Hide()
    {
        Utils.ClearChilds( t_ico_cont );
        desc_scroll.ClearList( true );

        transform.localPosition = new Vector3( 0, 1280.0f, 3000.0f );
        overlay.localPosition   = old_overlay_pos;
        
        if( is_open_as_modal )
        {
            Game.IsModalDialogOpened = false;
        }
    }

    //****************************************************************
    private void OnBtnOk( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        if( del != null )
            del( MessageBoxRet.OK );

        del = null;

        this.Hide();
    }

    //****************************************************************
    private void OnBtnCancel( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        if( del != null )
            del( MessageBoxRet.CANCEL );

        del = null;

        this.Hide();
    }

    //****************************************************************
    private void OnBtnRetry( IUIObject btn )
    {
        SoundController.Instance.Play_SoundClick();

        if( del != null )
            del( MessageBoxRet.RETRY );

        del = null;

        this.Hide();
    }

    //****************************************************************
    private void Start()
    {
        //
    }

    //****************************************************************
    //private void OnGUI()
    //{
    //    if( GUI.Button( new Rect( 10, 10, 200, 20 ), "Show btn OK" ) )
    //    {
    //        this.Bind( Test );
    //        this.Init( "Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message ", MessageBoxType.OK );
    //    }

    //    if( GUI.Button( new Rect( 10, 35, 200, 20 ), "Show btn OKCANCEL" ) )
    //    {
    //        this.Bind( Test );
    //        this.Init( "Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message Test Message ", MessageBoxType.OKCANCEL );
    //    }

    //    if( GUI.Button( new Rect( 10, 60, 200, 20 ), "Show btn RETRYCANCEL" ) )
    //    {
    //        this.Bind( Test );
    //        this.Init( "Test Message", MessageBoxType.RETRYCANCEL );
    //    }
    //}
    //private void Test( MessageBoxRet ret )
    //{
    //    print( ret );
    //}
}
