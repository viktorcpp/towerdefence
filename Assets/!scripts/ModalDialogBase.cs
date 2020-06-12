using UnityEngine;

public abstract class ModalDialogBase : MonoBehaviour
{
	public abstract void _Start();
	public abstract void Show();
	public abstract void Hide();
	public abstract void OnBtnClose( IUIObject btn );
}
