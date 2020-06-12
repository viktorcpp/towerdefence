using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class TranslatableToggleBtn : Translatable
{
	[SerializeField]
	private string id = string.Empty;

	//****************************************************************
	public override void Translate()
	{
		UIStateToggleBtn lbl = GetComponent<UIStateToggleBtn>();
		lbl.Text             = LangController.Instance.String( id );
	}
}
