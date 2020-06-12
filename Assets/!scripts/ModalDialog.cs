using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class ModalDialog : ModalDialogBase
{
	[SerializeField]
	protected UIPanel  panel        = null;
	[SerializeField]
	protected UIButton button_close = null;
	protected bool     visible      = false;

	//****************************************************************
	public override void _Start()
	{
		if( button_close != null )
		{
			button_close.SetValueChangedDelegate( OnBtnClose );
		}
	}

	//****************************************************************
	public override void Show()
	{
		if( visible || Game.ActiveDialog != null ) return;

        Overlay.Instance.PosShowZ = transform.localPosition.z + 1;
		Overlay.Show();

		Game.ActiveDialog = this;

		panel.AddTempTransitionDelegate
		(
			( p, t ) =>
			{
				visible = true;
			}
		);

		panel.StartTransition( UIPanelManager.SHOW_MODE.BringInForward );
	}

	//****************************************************************
	public override void Hide()
	{
		if( !visible ) return;

		panel.AddTempTransitionDelegate
		(
			( p, t ) =>
			{
				visible           = false;
				Game.ActiveDialog = null;
				Overlay.Hide();
			}
		);

		panel.StartTransition( UIPanelManager.SHOW_MODE.DismissForward );
	}

	//****************************************************************
	public override void OnBtnClose( IUIObject btn )
	{
        SoundController.Instance.Play_SoundClick();
		this.Hide();
	}

#region TEST
#if UNITY_EDITOR
	//****************************************************************
	//private void OnGUI()
	//{
	//    if( GUI.Button( new Rect( 20, 20, 200, 50 ), "Show" ) )
	//    {
	//        this.Show();
	//    }
	//}
#endif
#endregion
}
