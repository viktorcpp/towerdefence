#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using PauseOverlayHandler = defines.PauseOverlayHandler;

public class Pause : Singleton<Pause>
{
    [SerializeField]
    private Transform           t_scene_object    = null;
    [SerializeField]
    private Vector3             pos_visible       = new Vector3( 0, 0, -990 );
    [SerializeField]
    private Vector3             pos_hidden        = new Vector3( 0, 0, 3000 );
    [SerializeField]
    private UIButton            btn_continue      = null;
    private PauseOverlayHandler del_on_hide       = null;
    private PauseOverlayHandler del_on_show       = null;

    //****************************************************************
    public void Show()
    {
        t_scene_object.gameObject.SetActive( true );

        Utils.Translate( transform );

        t_scene_object.localPosition = pos_visible;

        if( del_on_show != null )
            del_on_show( true );

        del_on_show = null;
    }

    //****************************************************************
    public void Hide()
    {
        SoundController.Instance.Play_SoundClick();

        t_scene_object.localPosition = pos_hidden;
        
        if( del_on_hide != null )
            del_on_hide( false );

        del_on_hide = null;

        t_scene_object.gameObject.SetActive( false );
    }

    //****************************************************************
    public void SetHandler( PauseOverlayHandler new_handler )
    {
        del_on_hide = new_handler;
    }

    //****************************************************************
    public void AddHandler( PauseOverlayHandler new_handler )
    {
        del_on_hide += new_handler;
    }

    //****************************************************************
    public void RemHandler( PauseOverlayHandler new_handler )
    {
        del_on_hide -= new_handler;
    }

    //****************************************************************
    private void Start()
    {
        btn_continue.SetValueChangedDelegate( _OnBtnContinue );
    }

    //****************************************************************
    private void _OnBtnContinue( IUIObject btn )
    {
        this.Hide();
    }
}
