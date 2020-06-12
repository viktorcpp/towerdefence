using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class TranslatableTab : Translatable
{
	[SerializeField]
	private string id = string.Empty;

	//****************************************************************
	public override void Translate()
	{
		UIPanelTab lbl = GetComponent<UIPanelTab>();
		lbl.Text       = LangController.Instance.String( id );
	}
}
