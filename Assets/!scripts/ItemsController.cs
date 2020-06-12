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
using WeaponArea = defines.WeaponArea;

public class ItemsController : Singleton<ItemsController>
{
    private Transform icons_res = null;

    //****************************************************************
    public Transform GetIcon( string id, Transform parent )
    {
        Transform ret = null;

        ret = _LoadIcoFromRes( id, icons_res, parent );

        return ret;
    }

	//****************************************************************
	public void _Start()
	{
		icons_res = (( GameObject )Resources.Load( "prefabs/icons", typeof( GameObject ) )).transform;
	}

    //****************************************************************
	private Transform _LoadIcoFromRes( string ico_name, Transform resource, Transform parent = null )
	{
		Transform ret    = null;
        Transform t_ico  = resource.FindChild( ico_name );
        if( t_ico == null )
        {
            print( "EXCEPTION: ItemsController :: _LoadIcoFromRes( string, Transform, Transform ) ico not found - " + ico_name );

            ret      = new GameObject().transform;
            ret.name = "!!!- " + ico_name + " -404-not-found";
            if( parent != null )
            {
                ret.parent        = parent;
                ret.localPosition = Vector3.zero;
                ret.localScale    = Vector3.one;
            }
            return ret;
        }

        GameObject go_ico = t_ico.gameObject;
        ret               = ( ( GameObject )Instantiate( go_ico ) ).transform;
		ret.name          = ico_name;
        
        if( parent != null )
        {
            ret.parent = parent;
        }

        ret.localPosition = Vector3.zero;
        ret.localScale    = Vector3.one;

        return ret;
	}
}
