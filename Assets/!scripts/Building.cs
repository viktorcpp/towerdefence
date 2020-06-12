#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using MapData      = defines.MapData;
using WeaponData   = defines.WeaponData;
using Upgrade      = defines.WeaponData.Upgrade;
using BuildingData = defines.BuildingData;

public class Building : MonoBehaviour
{
    [SerializeField]
    private Button       btn                = null;
    [SerializeField]
    private Transform    t_ico_cont         = null;
    [SerializeField]
    private Transform    t_ico              = null;
    [SerializeField]
    private Transform    t_sec_green_4_cont = null;
    [SerializeField]
    private Transform    t_sec_red_4_cont   = null;
    [SerializeField]
    private Transform[]  t_sec_green_4_list = null;
    [SerializeField]
    private Transform[]  t_sec_red_4_list   = null;
    [SerializeField]
    private Transform    t_distance_view    = null;
    [SerializeField]
    private Transform    t_dist_on_select   = null;
    [SerializeField]
    private PackedSprite ani_shoot_view     = null;

    private WeaponData              weapon_data        = null;
    private BuildingData            building_data      = null;
    private bool                    is_initialized     = false;
    private bool                    is_placed          = false;
    private bool                    is_can_build       = false;
    private Transform               turret             = null;
    private Vector3                 pos_to_build       = Vector3.zero;
    private Vector3                 turret_dir_up      = new Vector3( 0, 1, 0 );
    private Vector3                 turret_dir_axis    = new Vector3( 0, 0, 1 );
    private bool                    is_selected        = false;
    private BattlefieldSectorItem[] busy_places        = new BattlefieldSectorItem[4]{ null, null, null, null };
    
    //****************************************************************
    public bool IsSelected
    {
        get{ return is_selected; }
    }
    public WeaponData WeaponData
    {
        get{ return weapon_data;  }
        set
        {
            weapon_data              = value;
            building_data            = new BuildingData();
            building_data.WeaponData = weapon_data;
        }
    }

    //****************************************************************
    public BuildingData GetBuildingData()
    {
        return building_data;
    }

	//****************************************************************
	public void _Start()
	{
        Utils.ClearChilds( t_ico );
		ItemsController.Instance.GetIcon( weapon_data.WpnTexture, t_ico );
        turret = t_ico_cont;

        this.HideDistance();

        is_initialized = true;

        BattlefieldController.Instance.HasBuildingToInstall = this;
	}

    //****************************************************************
    public void ShowDistanceView()
    {
        t_distance_view .gameObject.SetActive( true );
        t_dist_on_select.gameObject.SetActive( true );

        float scale = ( 512.0f * weapon_data.WpnRange / 100.0f ) / 512.0f;
        t_distance_view.localScale = new Vector3( scale, scale, 0 );

        is_selected = true;
    }

    //****************************************************************
    public void HideDistance()
    {
        t_distance_view .gameObject.SetActive( false );
        t_dist_on_select.gameObject.SetActive( false );

        is_selected = false;
    }

    //****************************************************************
    public void UpdateDirection( Vector3 target_in_world )
    {
        Vector3 target_world = target_in_world;
        target_world.z = 0;

        float angle = Vector3.Angle( turret_dir_up, target_world - transform.position );
        angle = target_world.x > transform.position.x ? angle : -angle;
        Quaternion rota = Quaternion.AngleAxis( -angle, turret_dir_axis );
        rota.x = 0;
        rota.y = 0;
        turret.localRotation = rota;
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

		if( !is_placed )
        {
            // cursor
            Vector3 pos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            pos.z = -20;
            transform.position = pos;

            // check if can install
            Ray          ray;
            RaycastHit[] hits;
            int          sector_count      = 0;
            Vector3      pos_for_build_tmp = Vector3.zero;
            for( int x = 0; x < t_sec_green_4_list.Length; x++ )
            {
                Transform t_curr_sector_green = t_sec_green_4_list[x];
                Transform t_curr_sector_red   = t_sec_red_4_list  [x];

                ray  = Camera.main.ScreenPointToRay( Camera.main.WorldToScreenPoint( t_curr_sector_green.position ) );
                hits = Physics.RaycastAll( ray );

                for( int h = 0; h < hits.Length; h++ )
                {
                    BattlefieldSectorItem bsi_found = hits[h].collider.GetComponent<BattlefieldSectorItem>();

                    t_curr_sector_green.gameObject.SetActive( bsi_found && bsi_found.BuildingBusy == null  );
                    t_curr_sector_red  .gameObject.SetActive( !bsi_found );

                    if( bsi_found && bsi_found.BuildingBusy == null )
                    {
                        if( x == 0 )
                        {
                            pos_for_build_tmp    = bsi_found.transform.position;
                            pos_for_build_tmp.x += 21.5f;
                            pos_for_build_tmp.y -= 21.5f;
                            pos_for_build_tmp.z  = 0;

                            busy_places[0] = bsi_found;
                            busy_places[1] = bsi_found.NodeRight;
                            busy_places[2] = bsi_found.NodeBot;
                            busy_places[3] = bsi_found.NodeBot == null ? null : bsi_found.NodeBot.NodeRight;
                        }
                        break;
                    }
                }

                sector_count = t_curr_sector_green.gameObject.activeInHierarchy ? sector_count+1 : sector_count-1;
            }

            is_can_build = sector_count == t_sec_green_4_list.Length;

            // test for pathnode lock
            if( is_can_build )
            {
                busy_places[0].BuildingBusy = this;
                busy_places[1].BuildingBusy = this;
                busy_places[2].BuildingBusy = this;
                busy_places[3].BuildingBusy = this;

                MapData map_data = MapController.Instance.GetMapData( Application.loadedLevelName );

                is_can_build = BattlefieldSectors.Instance.GetPath( map_data.MapSpawn, map_data.MapTarget ).Count > 0;

                busy_places[0].BuildingBusy = null;
                busy_places[1].BuildingBusy = null;
                busy_places[2].BuildingBusy = null;
                busy_places[3].BuildingBusy = null;
            }

            if( !is_can_build )
            {
                busy_places[0] = null;
                busy_places[1] = null;
                busy_places[2] = null;
                busy_places[3] = null;
            }

            pos_for_build_tmp.z = 0;
            pos_to_build        = pos_for_build_tmp;
        }
        else
        {
            this.FindTarget();

            if( enemy_target != null )
            {
                this.UpdateDirection( enemy_target.transform.position );
            }

            this.Attack();
        }
	}
    
    private Enemy enemy_target = null;

    //****************************************************************
    private void FindTarget()
    {
        if( enemy_target != null ) return;

        List<Enemy> enemy_list = BattlefieldController.Instance.GetEnemyesOnMap;
        
        enemy_target = enemy_list.Find
        (
            delegate( Enemy e ){ return Vector3.Distance( transform.position, e.transform.position ) <= weapon_data.WpnRange*2.60; }
        );
    }

    float last_shoot_time = -1;
    //****************************************************************
    private void Attack()
    {
        if( enemy_target == null ) return;

        if( last_shoot_time < 0 || ( Time.realtimeSinceStartup - last_shoot_time >= weapon_data.WpnFirerate ) )
        {
            SoundController.Instance.Play_SoundShoot1();
            ani_shoot_view.PlayAnim( 0 );

            enemy_target.AddDamage( weapon_data.WpnDamage );
            last_shoot_time = Time.realtimeSinceStartup;
        }
    }

    //****************************************************************
    private void _OnBtn( IUIObject btn )
    {
        if( !is_placed )
        {
            if( is_can_build && !BattlefieldController.Instance.BattlePaused )
            {
                SoundController.Instance.Play_SoundBuildingInstall();
                is_placed = true;
                BattlefieldController.Instance.OnBuildingPlaced( weapon_data );

                t_sec_red_4_cont  .gameObject.SetActive( false );
                t_sec_green_4_cont.gameObject.SetActive( false );

                transform.position = pos_to_build;

                busy_places[0].BuildingBusy = this;
                busy_places[1].BuildingBusy = this;
                busy_places[2].BuildingBusy = this;
                busy_places[3].BuildingBusy = this;

                BattlefieldController.Instance.AddBuilding( this );
                BattlefieldController.Instance.HasBuildingToInstall = null;
            }
            else
            {
                SoundController.Instance.Play_SoundClickDenail();
            }
        }
        else
        {
            if( !BattlefieldController.Instance.HasBuildingToInstall )
            {
                SoundController.Instance.Play_SoundClick();
                
                Building b_selected = BattlefieldController.Instance.GetBuildingSelected();
                if( b_selected != null )
                    b_selected.HideDistance();

                this.ShowDistanceView();
                BuildingController.Instance.ShowDialogBuildingInfo( this );
            }
        }
    }

    //****************************************************************
    public void Remove()
    {
        if( is_placed )
        {
            busy_places[0].BuildingBusy = null;
            busy_places[1].BuildingBusy = null;
            busy_places[2].BuildingBusy = null;
            busy_places[3].BuildingBusy = null;

            try
            {
                BattlefieldController.Instance.RemBuilding( this );
            }
            catch( System.Exception ){} // on game exit
        }

        DestroyImmediate( gameObject );
    }
}
