#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class BattlefieldBackgroundReceiver : Singleton<BattlefieldBackgroundReceiver>
{
    [SerializeField]
    private Button btn = null;

	//****************************************************************
	private void Start()
	{
		btn.SetValueChangedDelegate( this.OnBtn );
	}

    //****************************************************************
    private void OnBtn( IUIObject btn )
    {
        if( BattlefieldController.Instance.HasBuildingToInstall )
        {
            BattlefieldController.Instance.HasBuildingToInstall.HideDistance();
        }

        Building b_selected = BattlefieldController.Instance.GetBuildingSelected();
        if( b_selected != null )
        {
            WindowBuildingInfo.Instance.Hide();
        }
    }
}
