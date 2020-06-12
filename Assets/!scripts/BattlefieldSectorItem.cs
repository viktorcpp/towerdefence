using UnityEngine;

public class BattlefieldSectorItem : MonoBehaviour
{
    [SerializeField]
	private BattlefieldSectorItem left_top  = null;
    [SerializeField]
    private BattlefieldSectorItem top       = null;
    [SerializeField]
    private BattlefieldSectorItem top_right = null;
    [SerializeField]
    private BattlefieldSectorItem right     = null;
    [SerializeField]
    private BattlefieldSectorItem bot_right = null;
    [SerializeField]
    private BattlefieldSectorItem bot       = null;
    [SerializeField]
    private BattlefieldSectorItem bot_left  = null;
    [SerializeField]
    private BattlefieldSectorItem left      = null;

    private Building              building  = null;

    public Building BuildingBusy
    {
        get{ return building;  }
        set
        {
            building = value;

            if( building != null )
            {
                Transform sbt     = BattlefieldSectors.Instance.GetSectorBusyTmplt();
                sbt.parent        = transform;
                sbt.localPosition = new Vector3( 0, 0, -0.2f );
            }
            else
            {
                Utils.ClearChilds( transform );
            }
        }
    }

    public BattlefieldSectorItem NodeLeftTop
    {
        get{ return left_top;  }
        set{ left_top = value; }
    }
    public BattlefieldSectorItem NodeTop
    {
        get{ return top;  }
        set{ top = value; }
    }
    public BattlefieldSectorItem NodeTopRight
    {
        get{ return top_right;  }
        set{ top_right = value; }
    }
    public BattlefieldSectorItem NodeRight
    {
        get{ return right;  }
        set{ right = value; }
    }
    public BattlefieldSectorItem NodeBotRight
    {
        get{ return bot_right;  }
        set{ bot_right = value; }
    }
    public BattlefieldSectorItem NodeBot
    {
        get{ return bot;  }
        set{ bot = value; }
    }
    public BattlefieldSectorItem NodeBotLeft
    {
        get{ return bot_left;  }
        set{ bot_left = value; }
    }
    public BattlefieldSectorItem NodeLeft
    {
        get{ return left;  }
        set{ left = value; }
    }
}
