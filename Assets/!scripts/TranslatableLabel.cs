using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class TranslatableLabel : Translatable
{
	[SerializeField]
	private string id = string.Empty;

	//****************************************************************
	public override void Translate()
	{
		SpriteText lbl = GetComponent<SpriteText>();
		lbl.Text       = LangController.Instance.String( id );
	}
}
