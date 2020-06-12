#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using UnitData = defines.UnitData;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform                   ico_cont        = null;
    [SerializeField]
    private Transform                   unit            = null;
    [SerializeField]
    private Button                      btn             = null;
    [SerializeField]
    private UIProgressBar               health_bar      = null;

    private Vector3                     enemy_dir_right = new Vector3( 0, 1, 0 );
    private Vector3                     enemy_dir_axis  = new Vector3( 0, 0, 1 );
    private bool                        is_initialized  = false;
    private UnitData                    unit_data       = null;
    private BattlefieldSectorItem       sector_start    = null;
    private BattlefieldSectorItem       sector_end      = null;

    Vector3 pos_next;
    private bool                        is_move         = false;
    private List<BattlefieldSectorItem> path_node       = new List<BattlefieldSectorItem>();
    private BattlefieldSectorItem       curr_node       = null;
    private BattlefieldSectorItem       node_next       = null;
    private int                         health          = 0;
    private bool                        is_destroyed    = false;
    

    //****************************************************************
    public UnitData UnitData
    {
        get{ return unit_data;  }
        set{ unit_data = value; }
    }
    public int UnitHealth
    {
        get{ return health;  }
        set{ health = value; }
    }

    //****************************************************************
    public void AddDamage( int d )
    {
        if( is_destroyed ) return;

        health -= d;

        if( health <= 0 )
        {
            SoundController.Instance.Play_SoundExplosion3();
            is_initialized = is_move = false;
            BattlefieldController.Instance.OnEnemyDestroyed( this );
        }
    }

	//****************************************************************
	public void _Start()
	{
        Utils.ClearChilds( ico_cont );
        unit = ItemsController.Instance.GetIcon( unit_data.UnitTex, ico_cont );

        health = unit_data.UnitHealthMax;
        
        is_initialized = true;
	}

    //****************************************************************
    public void InitMove( string sector_start_id, string sector_end_id )
    {
        this.InitMove( BattlefieldSectors.Instance.GetSectorItem( sector_start_id ), BattlefieldSectors.Instance.GetSectorItem( sector_end_id   ) );
    }

    //****************************************************************
    public void InitMove( BattlefieldSectorItem new_sector_start, BattlefieldSectorItem new_sector_end )
    {
        sector_start = new_sector_start;
        sector_end   = new_sector_end;
        curr_node    = sector_start;

        UpdatePathNode();

        is_move = true;
    }

    //****************************************************************
    public void UpdateDirection( Vector3 target_in_world )
    {
        Vector3 target_world = target_in_world;
        target_world.z = 0;

        float angle = Vector3.Angle( enemy_dir_right, target_world - transform.position );
        angle = target_world.x > transform.position.x ? angle : -angle;
        Quaternion rota = Quaternion.AngleAxis( -angle, enemy_dir_axis );
        rota.x = 0;
        rota.y = 0;
        unit.localRotation = rota;
    }

    //****************************************************************
    public void UpdatePathNode()
    {
        path_node  = BattlefieldSectors.Instance.GetPath( curr_node, sector_end );
        if( path_node.Count > 0 )
        {
            node_next  = path_node[0];
            pos_next   = node_next.transform.position;
            pos_next.z = 0;
        }
    }

    //****************************************************************
    public void Remove()
    {
        if( is_destroyed ) return;

        BattlefieldUI.Instance.HideWindowUnitInfo( this );

        is_destroyed = true;
        is_initialized = is_move = false;
        DestroyImmediate( gameObject );
    }
	
	//****************************************************************
	private void Start()
	{
		btn.SetValueChangedDelegate( this._OnBtn );
	}
	
	//****************************************************************
	private void FixedUpdate()
	{
        if( !is_initialized || BattlefieldController.Instance.BattlePaused ) return;

        if( unit_data != null )
        {
            health_bar.Value = (float)health / (float)unit_data.UnitHealthMax;
        }

        if( is_move )
        {
            float speed = 20 * Time.deltaTime;

            if( Vector3.Distance( transform.position, pos_next ) <= speed )
            {
                if( node_next == sector_end )
                {
                    BattlefieldController.Instance.OnEnemyPassed( this );
                    is_initialized = false;
                }
                if( path_node.Count == 2 )
                {
                    curr_node  = path_node[0];
                    node_next  = path_node[1];
                    pos_next   = node_next.transform.position;
                    pos_next.z = 0;
                }
                else
                {
                    curr_node = path_node[0];
                    this.UpdatePathNode();
                }
            }

            transform.position = Vector3.MoveTowards( transform.position, pos_next, speed );
            UpdateDirection( pos_next );
        }
	}

    //****************************************************************
    private void _OnBtn( IUIObject btn )
    {
        if( !is_initialized ) return;

        SoundController.Instance.Play_SoundClick();

        BattlefieldUI.Instance.ShowWindowUnitInfo( unit_data, this );
    }
}
