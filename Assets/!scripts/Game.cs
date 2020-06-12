#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class Game : Singleton<Game>
{
    private static bool            is_modal_dialog_opened = false;
    private static ModalDialogBase active_dialog          = null;

    //****************************************************************
    public static ModalDialogBase ActiveDialog
    {
        get{ return active_dialog;  }
        set{ active_dialog = value; }
    }
    public static bool IsModalDialogOpened
    {
        get{ return is_modal_dialog_opened;  }
        set{ is_modal_dialog_opened = value; }
    }
    public static Player Player
    {
        get{ return Player.Instance; }
    }
}
