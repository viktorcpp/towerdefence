#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using WeaponData = defines.WeaponData;

public class BattlefieldArcenalViewCont : MonoBehaviour
{
    [SerializeField]
    private UIListItemContainer          cont    = null;
    [SerializeField]
    private BattlefieldArcenalItemView[] items   = null;
    private int                          counter = 0;

    public WeaponData WeaponData
    {
        set
        {
            bool is_act = gameObject.activeInHierarchy;
            gameObject.SetActive( true );

            if( counter >= items.Length ) return;

            items[ counter ].WeaponData = value;

            cont.ScanChildren();

            counter++;

            gameObject.SetActive( is_act );
        }
    }
}
