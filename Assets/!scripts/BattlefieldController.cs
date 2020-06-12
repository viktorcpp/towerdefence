#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using WeaponData    = defines.WeaponData;
using MapData       = defines.MapData;
using MapDataPlayer = defines.MapDataPlayer;
using UnitData      = defines.UnitData;
using TimeSpan      = System.TimeSpan;

public class BattlefieldController : Singleton<BattlefieldController>
{
    private Transform             weapon_res          = null;
    private MapData               map_data            = null;
    private MapDataPlayer         player_map_data     = new MapDataPlayer();
    private BattlefieldSectors    sectors             = null;
    private List<Building>        building_list       = new List<Building>();
    private Building              building_to_install = null;
    private bool                  is_battle_paused    = false;
    private Transform             enemy_unit_res      = null;
    private BattlefieldSectorItem node_start          = null;
    private BattlefieldSectorItem node_end            = null;
    private List<Enemy>           enemyes_on_map      = new List<Enemy>();
    private Timer                 timeout_timer       = null;
    private TimeSpan              ts_next;
    private int                   seconds_to_next     = 0;
    private bool                  is_spawn_done       = false;

    //****************************************************************
    public Building HasBuildingToInstall
    {
        get{ return building_to_install;  }
        set{ building_to_install = value; }
    }
    public MapDataPlayer MapDataPlayer
    {
        get{ return player_map_data; }
    }
    public bool BattlePaused
    {
        get{ return is_battle_paused;  }
        set
        {
            is_battle_paused = value;

            if( is_battle_paused && timeout_timer != null )
            {
                seconds_to_next = timeout_timer.GetTimeDiff( ts_next ).Seconds;
            }
            else
            {
                if( timeout_timer != null )
                {
                    ts_next = timeout_timer.GetTimeNext( seconds_to_next );
                }
            }
        }
    }
    public List<Enemy> GetEnemyesOnMap
    {
        get{ return enemyes_on_map; }
    }
    public Timer GetMapTimer
    {
        get{ return timeout_timer; }
    }

    //****************************************************************
    public string GetNodeStartName()
    {
        return map_data.MapSpawn;
    }

    //****************************************************************
    public BattlefieldSectorItem GetNodeStart()
    {
        return node_start;
    }

    //****************************************************************
    public string GetNodeEndName()
    {
        return map_data.MapTarget;
    }

    //****************************************************************
    public BattlefieldSectorItem GetNodeEnd()
    {
        return node_end;
    }

    //****************************************************************
    public Building GetBuildingSelected()
    {
        return building_list.Find( delegate( Building b ){ return b.IsSelected; } );
    }
    public void AddBuilding( Building b )
    {
        building_list.Add( b );
    }

    //****************************************************************
    public void RemBuilding( Building b )
    {
        building_list.Remove( b );
    }

	//****************************************************************
	public void _Start()
	{
        BattlefieldUI.Instance._Start();

		weapon_res                    = ((GameObject)Resources.Load( "prefabs/weapon-building" )).transform;
        map_data                      = MapController.Instance.GetMapData( Application.loadedLevelName );
        player_map_data.PlayerMapData = map_data;
        player_map_data.PlayerBalance = map_data.MapBalance;
        node_start                    = BattlefieldSectors.Instance.transform.FindChild( map_data.MapSpawn  ).GetComponent<BattlefieldSectorItem>();
        node_end                      = BattlefieldSectors.Instance.transform.FindChild( map_data.MapTarget ).GetComponent<BattlefieldSectorItem>();

        this._LoadSectors();
	}

    //****************************************************************
    public void OnBattleBegin()
    {
        is_spawn_done = false;

        this.Spawn();

        timeout_timer = new Timer();
        ts_next       = timeout_timer.GetTimeNext( map_data.MapTimer );
    }

    //****************************************************************
    private void FixedUpdate()
    {
        if( timeout_timer == null || is_battle_paused ) return;

        TimeSpan time_diff = timeout_timer.GetTimeDiff( ts_next );
        
        if( time_diff.Seconds <= 0 )
        {
            this.OnBattleBegin();
        }
        else
        {
            BattlefieldUI.Instance.UpdateTimer( timeout_timer.FormatStringA( time_diff ) );
        }
    }
    
    //****************************************************************
    public void Spawn()
    {
        player_map_data.PlayerWaves++;

        BattlefieldUI.Instance.SetBtnNextWaveEnabled = false;

        StartCoroutine( _CorSpawn() );
    } private IEnumerator _CorSpawn()
    {
        Vector3 pos_base = node_start.transform.position;
        Enemy   enemy    = null;
        Vector3 pos      = pos_base;

        pos_base.z = 0;
        pos.z      = 0;

        string unit_id = map_data.MapWaves[ player_map_data.PlayerWaves-1 ];

        UnitData unit_data = UnitsController.Instance.GetUnitDataList().Find( delegate( UnitData ud ){ return ud.UnitId == unit_id; } );

        for( int x = 0; x < map_data.MapEnemyesInWave; x++ )
        {
            yield return new WaitForSeconds( 0.25f );

            if( is_battle_paused )
            {
                x--;
                continue;
            }

            pos.x += Utils.Random( -64.5f, 21.5f );
            pos.y += Utils.Random( -64.5f, 21.5f );

            enemy                    = ((GameObject)Instantiate( enemy_unit_res.gameObject )).GetComponent<Enemy>();
            enemy.UnitData           = unit_data;
            enemy.transform.position = pos;
            enemy._Start();
            enemy.InitMove( node_start, node_end );

            enemyes_on_map.Add( enemy );
            pos = pos_base;
        }

        is_spawn_done = true;
    }

    //****************************************************************
    public void OnEnemyPassed( Enemy enemy )
    {
        StartCoroutine( _CorOnEnemyPassed( enemy ) );
    } private IEnumerator _CorOnEnemyPassed( Enemy enemy )
    {
        yield return new WaitForEndOfFrame();

        player_map_data.PlayerWavesPassed++;
        BattlefieldUI.Instance.UpdateLoseLimit( map_data.MapLoseWavesLimit - player_map_data.PlayerWavesPassed );
        enemyes_on_map.Remove( enemy );
        enemy.Remove();

        if( player_map_data.PlayerWavesPassed == map_data.MapLoseWavesLimit )
        {
            this.OnLose();
        }
    }

    //****************************************************************
    public void OnEnemyDestroyed( Enemy enemy )
    {
        StartCoroutine( this._CorOnEnemyDestroyed( enemy ) );
    } private IEnumerator _CorOnEnemyDestroyed( Enemy enemy )
    {
        yield return new WaitForEndOfFrame();

        player_map_data.PlayerBalance += enemy.UnitData.UnitReward;
        enemyes_on_map.Remove( enemy );
        enemy.Remove();

        if( enemyes_on_map.Count < 1 && is_spawn_done )
        {
            if( player_map_data.PlayerWaves == player_map_data.PlayerMapData.MapWavesCount )
            {
                this.OnWin();
            }
            else
            {
                BattlefieldUI.Instance.SetBtnNextWaveEnabled = true;
            }
        }
    }

    //****************************************************************
    public void OnLose()
    {
        is_battle_paused = true;

        BattlefieldUI.Instance.ShowWindowLose();
    }

    //****************************************************************
    public void OnWin()
    {
        is_battle_paused = true;

        BattlefieldUI.Instance.ShowWindowWin();

        Game.Player.PlayerMapDoneAdd( map_data.MapId );
        PlayerSave.Save();
    }
	
    //****************************************************************
    public void OnBuildingBuy( WeaponData wd )
    {
        this._CreateWeaponView( wd );
        BattlefieldSectors.Instance.Show();
    }

    //****************************************************************
    public void OnBuildingPlaced( WeaponData wd )
    {
        player_map_data.PlayerBalance -= wd.WpnPrice;
        BattlefieldSectors.Instance.Hide();
        StartCoroutine( _UpdatePathNode() );
    }
    private IEnumerator _UpdatePathNode()
    {
        yield return new WaitForEndOfFrame();

        foreach( var e in enemyes_on_map )
        {
            e.UpdatePathNode();
        }
    }

    //****************************************************************
    public void OnBuildingSelled()
    {
        foreach( var e in enemyes_on_map )
        {
            e.UpdatePathNode();
        }
    }

    //****************************************************************
	private void Start()
	{
		enemy_unit_res = ((GameObject)Resources.Load( "prefabs/enemy-unit" )).transform;
	}

    //****************************************************************
    private void _LoadSectors()
    {
        if( sectors == null )
        {
            sectors = Utils.FindComp<BattlefieldSectors>();
            sectors._Start();
        }
    }

    //****************************************************************
    private Building _CreateWeaponView( WeaponData wd )
    {
        Building b = ((GameObject)Instantiate( weapon_res.gameObject )).GetComponent<Building>();

        b.WeaponData = wd;
        b._Start();

        return b;
    }
}
