using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Button : MonoBehaviour, IUIObject
{
	private bool is_enabled = true;
    private bool pressed    = false;

	//****************************************************************
	public bool Enabled
	{
		get{ return is_enabled; }
		set
		{
			is_enabled = value;
		}
	}
    public bool Pressed
    {
        get{ return pressed;  }
        set{ pressed = value; }
    }

	//****************************************************************
	public void OnInput( POINTER_INFO ptr )
	{
        if( (ptr.evt == POINTER_INFO.INPUT_EVENT.PRESS) && !pressed )
        {
            pressed = true;
        }
		else if( (ptr.evt == POINTER_INFO.INPUT_EVENT.RELEASE || ptr.evt == POINTER_INFO.INPUT_EVENT.TAP) && pressed )
		{
            if( change_delegate != null )
                change_delegate( this );

            pressed = false;
		}
	}

    //****************************************************************
    public override string ToString()
    {
        string str = this.GetType().Name + '\n';
        str += "is_enabled = " + is_enabled + '\n';
        str += "pressed    = " + pressed    + '\n';
        return str;
    }



	/*****************************************************************\
	|                                                                 |
	|                          BASE OVERRIDES                         |
	|                                                                 |
	\*****************************************************************/

	private EZInputDelegate        input_delegate  = null;
	private EZValueChangedDelegate change_delegate = null;
	
	public virtual void SetInputDelegate( EZInputDelegate del )
	{
		input_delegate = del;
	}
	public virtual void AddInputDelegate( EZInputDelegate del )
	{
		input_delegate += del;
	}
	public virtual void RemoveInputDelegate( EZInputDelegate del )
	{
		input_delegate -= del;
	}
	public virtual void SetValueChangedDelegate( EZValueChangedDelegate del )
	{
		change_delegate = del;
	}
	public virtual void AddValueChangedDelegate( EZValueChangedDelegate del )
	{
		change_delegate += del;
	}
	public virtual void RemoveValueChangedDelegate( EZValueChangedDelegate del )
	{
		change_delegate += del;
	}



	/*****************************************************************\
	|                                                                 |
	|                         BASE OVERRIDES ETC                      |
	|                                                                 |
	\*****************************************************************/



	public virtual bool controlIsEnabled
	{
		get{ return true; }
		set{  }
	}
	public virtual bool DetargetOnDisable
	{
		get{ return true; }
		set{  }
	}
	public IUIObject GetControl( ref POINTER_INFO ptr )
	{
		return this;
	}
	public virtual IUIContainer Container
	{
		get{ return null; }
		set{  }
	}
	public virtual bool GotFocus() { return false; }
	public bool RequestContainership( IUIContainer cont )
	{
		return false;
	}
	[HideInInspector]
	public object data;
	public object Data
	{
		get{ return data;  }
		set{ data = value; }
	}
	public bool IsDraggable
	{
		get{ return false; }
		set{  }
	}
	[HideInInspector]
	public LayerMask drop_mask = -1;
	public LayerMask DropMask
	{
		get{ return drop_mask;  }
		set{ drop_mask = value; }
	}
	public bool IsDragging
	{
		get{ return false; }
		set{  }
	}
	public GameObject DropTarget
	{
		get{ return null; }
		set{  }
	}
	public bool DropHandled
	{
		get{ return false; }
		set{  }
	}
	public void DragUpdatePosition       ( POINTER_INFO ptr ){}
	public void DefaultDragUpdatePosition( POINTER_INFO ptr ){}
	public void SetDragPosUpdater        ( EZDragDropHelper.UpdateDragPositionDelegate del ){}
	public void CancelDrag               (){}
	public float DragOffset
	{
		get{ return 50.0f; }
		set{  }
	}
	public EZAnimation.EASING_TYPE CancelDragEasing
	{
		get{ return EZAnimation.EASING_TYPE.BackIn; }
		set{  }
	}
	public float CancelDragDuration
	{
		get{ return 0; }
		set{  }
	}
	public void                                       OnEZDragDrop_Internal         ( EZDragDropParams parms ){}
	public void                                       AddDragDropDelegate           ( EZDragDropDelegate del ){}
	public void                                       RemoveDragDropDelegate        ( EZDragDropDelegate del ){}
	public void                                       SetDragDropDelegate           ( EZDragDropDelegate del ){}
	public void                                       SetDragDropInternalDelegate   ( EZDragDropHelper.DragDrop_InternalDelegate del ){}
	public void                                       AddDragDropInternalDelegate   ( EZDragDropHelper.DragDrop_InternalDelegate del ){}
	public void                                       RemoveDragDropInternalDelegate( EZDragDropHelper.DragDrop_InternalDelegate del ){}
	public EZDragDropHelper.DragDrop_InternalDelegate GetDragDropInternalDelegate   (){ return null; }
}
