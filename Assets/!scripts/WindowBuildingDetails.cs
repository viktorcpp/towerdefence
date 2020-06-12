#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class WindowBuildingDetails : ModalDialog
{
	//****************************************************************
	public override void _Start()
	{
		base._Start();
	}

    //****************************************************************
	public override void Show()
	{
		base.Show();

        Utils.Translate( transform );
	}

    //****************************************************************
	public override void Hide()
	{
		base.Hide();
	}
}
