using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class TranslatableButton : Translatable
{
	[SerializeField]
	private string id = string.Empty;

	//****************************************************************
	public override void Translate()
	{
		UIButton btn = GetComponent<UIButton>();
		btn.Text     = LangController.Instance.String( id );
	}
}
