using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class defines
{
	public const string endl              = "\n";
	public const string XML_PATH_LANGUAGE = "xml/lang";
    public const string XML_PATH_MAPS     = "xml/maps";
    public const string XML_PATH_WEAPONS  = "xml/weapons";
    public const string XML_PATH_UNITS    = "xml/units";

    public delegate void MessageBoxRetHandler    ( MessageBoxRet ret_value );
    public delegate void PauseOverlayHandler     ( bool is_show );
    public delegate void GameMusicChangeHandler  ( bool play );
    public delegate void GameSoundChangeHandler  ( bool play );
    public delegate void OptionsVisibilityHandler( bool is_show );

    public enum MessageBoxType
    {
        NULL = 0,

        OK,
        OKCANCEL,
        RETRYCANCEL,

        COUNT
    }

    public enum MessageBoxRet
    {
        NULL = 0,

        OK,
        CANCEL,
        RETRY,

        COUNT
    }

	public enum MoneyType
	{
		NULL = 0,

		GOLD,
		SILVER,

		COUNT
	}

    public enum GenderType
    {
        NULL = 0,

        MALE,
        FEMALE,
        SHEMALE,

        COUNT
    }

    public enum SceneType
    {
        NULL = 0,
        
        LOADER,
        MAIN_MENU,
        MAP_SELECTOR,
        BATTLE,

        COUNT
    }

    public enum WeaponArea
    {
        NULL = 0,

        AREA_1,
        AREA_2_H,
        AREA_2_V,
        AREA_4,
        AREA_6_H,
        AREA_6_V,
        AREA_9,

        COUNT
    }

    public class MapData
    {
        public string MapId
        {
            get{ return map_id;  }
            set{ map_id = value; }
        }
        public string MapName
        {
            get{ return map_name;  }
            set{ map_name = value; }
        }
        public int MapBalance
        {
            get{ return map_balance;  }
            set{ map_balance = value; }
        }
        public int MapWavesCount
        {
            get{ return map_waves_count;  }
            set{ map_waves_count = value; }
        }
        public int MapEnemyesInWave
        {
            get{ return map_enemyes_in_wave;  }
            set{ map_enemyes_in_wave = value; }
        }
        public int MapLoseWavesLimit
        {
            get{ return map_lose_waves_limit;  }
            set{ map_lose_waves_limit = value; }
        }
        public int MapTimer
        {
            get{ return map_timer;  }
            set{ map_timer = value; }
        }
        public string MapIcoBig
        {
            get{ return map_ico_big;  }
            set{ map_ico_big = value; }
        }
        public string MapIcoSmall
        {
            get{ return map_ico_small;  }
            set{ map_ico_small = value; }
        }
        public string MapRequired
        {
            get{ return map_required;  }
            set{ map_required = value; }
        }
        public string MapSpawn
        {
            get{ return map_spawn;  }
            set{ map_spawn = value; }
        }
        public string MapTarget
        {
            get{ return map_target;  }
            set{ map_target = value; }
        }
        public string[] MapArcenal
        {
            get{ return map_arcenal;  }
            set{ map_arcenal = value; }
        }
        public string[] MapWaves
        {
            get{ return map_waves;  }
            set{ map_waves = value; }
        }

        private string   map_id               = string.Empty;
        private string   map_name             = string.Empty;
        private int      map_balance          = 0;
        private int      map_waves_count      = 999;
        private int      map_enemyes_in_wave  = 999;
        private int      map_lose_waves_limit = 20;
        private int      map_timer            = 1;
        private string   map_ico_big          = string.Empty;
        private string   map_ico_small        = string.Empty;
        private string   map_required         = string.Empty;
        private string   map_spawn            = string.Empty;
        private string   map_target           = string.Empty;
        private string[] map_arcenal          = null;
        private string[] map_waves            = null;

        public override string ToString()
        {
            string ret = this.GetType().Name + '\n';

            ret += "map_id               = " + map_id               + '\n';
            ret += "map_name             = " + map_name             + '\n';
            ret += "map_balance          = " + map_balance          + '\n';
            ret += "map_waves            = " + map_waves_count      + '\n';
            ret += "map_enemyes_in_wave  = " + map_enemyes_in_wave  + '\n';
            ret += "map_lose_waves_limit = " + map_lose_waves_limit + '\n';
            ret += "map_timer            = " + map_timer            + '\n';
            ret += "map_ico_big          = " + map_ico_big          + '\n';
            ret += "map_ico_small        = " + map_ico_small        + '\n';
            ret += "map_required         = " + map_required         + '\n';
            ret += "map_spawn            = " + map_spawn            + '\n';
            ret += "map_target           = " + map_target           + '\n';

            ret += "arcenal:" + '\n';
            foreach( string ma in map_arcenal )
            {
                ret += "    " + ma + '\n';
            }

            ret += "map_waves:" + '\n';
            foreach( string mw in map_waves )
            {
                ret += "    " + mw + '\n';
            }

            return ret;
        }
    }

    public class MapDataPlayer
    {
        public MapData PlayerMapData
        {
            get{ return player_map_data;  }
            set{ player_map_data = value; }
        }
        public int PlayerBalance
        {
            get{ return player_balance;  }
            set{ player_balance = value; }
        }
        public int PlayerWaves
        {
            get{ return player_waves;  }
            set{ player_waves = value; }
        }
        public int PlayerWavesPassed
        {
            get{ return player_waves_passed;  }
            set{ player_waves_passed = value; }
        }

        private MapData player_map_data     = null;
        private int     player_balance      = 0;
        private int     player_waves        = 0;
        private int     player_waves_passed = 0;

        public override string ToString()
        {
            string ret = this.GetType().Name + '\n';

            ret += "player_map_data     = " + player_map_data     + '\n';
            ret += "player_balance      = " + player_balance      + '\n';
            ret += "player_waves        = " + player_waves        + '\n';
            ret += "player_waves_passed = " + player_waves_passed + '\n';
            
            
            return ret;
        }
    }

    public class WeaponData
    {
        public string WpnId
        {
            get{ return wpn_id;  }
            set{ wpn_id = value; }
        }
        public string WpnName
        {
            get{ return wpn_name;  }
            set{ wpn_name = value; }
        }
        public string WpnDesc
        {
            get{ return wpn_desc;  }
            set{ wpn_desc = value; }
        }
        public string WpnIco
        {
            get{ return wpn_ico;  }
            set{ wpn_ico = value; }
        }
        public string WpnTexture
        {
            get{ return wpn_texture;  }
            set{ wpn_texture = value; }
        }
        public int WpnPrice
        {
            get{ return wpn_price;  }
            set{ wpn_price = value; }
        }
        public int WpnRange
        {
            get{ return wpn_range;  }
            set{ wpn_range = value; }
        }
        public int WpnDamage
        {
            get{ return wpn_damage;  }
            set{ wpn_damage = value; }
        }
        public float WpnFirerate
        {
            get{ return wpn_firerate;  }
            set{ wpn_firerate = value; }
        }
        public WeaponArea WeaponArea
        {
            get{ return wpn_area;  }
            set{ wpn_area = value; }
        }
        public int WeaponSellPrice
        {
            get{ return wpn_sell_price;  }
            set{ wpn_sell_price = value; }
        }
        public Upgrade[] WeaponUpgrades
        {
            get{ return wpn_upgrades;  }
            set{ wpn_upgrades = value; }
        }

        private string     wpn_id         = string.Empty;
        private string     wpn_name       = string.Empty;
        private string     wpn_desc       = string.Empty;
        private string     wpn_ico        = string.Empty;
        private string     wpn_texture    = string.Empty;
        private int        wpn_price      = 999;
        private int        wpn_range      = 0;
        private int        wpn_damage     = 0;
        private float      wpn_firerate   = 0;
        private WeaponArea wpn_area       = WeaponArea.NULL;
        private int        wpn_sell_price = 0;
        private Upgrade[]  wpn_upgrades   = null;

        public override string ToString()
        {
            string ret = this.GetType().Name + '\n';

            ret += "wpn_id         = " + wpn_id         + '\n';
            ret += "wpn_name       = " + wpn_name       + '\n';
            ret += "wpn_desc       = " + wpn_desc       + '\n';
            ret += "wpn_ico        = " + wpn_ico        + '\n';
            ret += "wpn_texture    = " + wpn_texture    + '\n';
            ret += "wpn_price      = " + wpn_price      + '\n';
            ret += "wpn_range      = " + wpn_range      + '\n';
            ret += "wpn_damage     = " + wpn_damage     + '\n';
            ret += "wpn_firerate   = " + wpn_firerate   + '\n';
            ret += "wpn_area       = " + wpn_area       + '\n';
            ret += "wpn_sell_price = " + wpn_sell_price + '\n';
            
            foreach( Upgrade u in wpn_upgrades )
            {
                ret += "wpn_upgrades:" + '\n';
                ret += u + "" + '\n';
            }

            return ret;
        }

        public class Upgrade
        {
            public int UPrice
            {
                get{ return upgrade_price;  }
                set{ upgrade_price = value; }
            }
            public int UBuildTime
            {
                get{ return upgrade_build_time;  }
                set{ upgrade_build_time = value; }
            }
            public int URangeBonus
            {
                get{ return upgrade_range_bonus;  }
                set{ upgrade_range_bonus = value; }
            }
            public float UFirerateBonus
            {
                get{ return upgrade_firerate_bonus;  }
                set{ upgrade_firerate_bonus = value; }
            }
            public int UDamageBonus
            {
                get{ return upgrade_damage_bonus;  }
                set{ upgrade_damage_bonus = value; }
            }
            public int USellPriceBonus
            {
                get{ return upgrade_sell_price_bonus;  }
                set{ upgrade_sell_price_bonus = value; }
            }

            private int   upgrade_price            = 0;
            private int   upgrade_build_time       = 0;
            private int   upgrade_range_bonus      = 0;
            private float upgrade_firerate_bonus   = 0;
            private int   upgrade_damage_bonus     = 0;
            private int   upgrade_sell_price_bonus = 0;

            public override string ToString()
            {
                string ret = this.GetType().Name + '\n';

                ret += "upgrade_price            = " + upgrade_price            + '\n';
                ret += "upgrade_build_time       = " + upgrade_build_time       + '\n';
                ret += "upgrade_range_bonus      = " + upgrade_range_bonus      + '\n';
                ret += "upgrade_firerate_bonus   = " + upgrade_firerate_bonus   + '\n';
                ret += "upgrade_sell_price_bonus = " + upgrade_sell_price_bonus + '\n';

                return ret;
            }
        }
    }

    public class UnitData
    {
        public string UnitId
        {
            get{ return unit_id;  }
            set{ unit_id = value; }
        }
        public string UnitName
        {
            get{ return unit_name;  }
            set{ unit_name = value; }
        }
        public string UnitDesc
        {
            get{ return unit_desc;  }
            set{ unit_desc = value; }
        }
        public string UnitIco
        {
            get{ return unit_ico;  }
            set{ unit_ico = value; }
        }
        public string UnitTex
        {
            get{ return unit_tex;  }
            set{ unit_tex = value; }
        }
        public int UnitReward
        {
            get{ return unit_reward;  }
            set{ unit_reward = value; }
        }
        public int UnitHealthMax
        {
            get{ return unit_health_max;  }
            set{ unit_health_max = value; }
        }

        private string unit_id         = string.Empty;
        private string unit_name       = string.Empty;
        private string unit_desc       = string.Empty;
        private string unit_ico        = string.Empty;
        private string unit_tex        = string.Empty;
        private int    unit_reward     = 0;
        private int    unit_health_max = 0;

        public override string ToString()
        {
            string ret = this.GetType().Name + '\n';

            ret += "unit_id         = " + unit_id         + '\n';
            ret += "unit_name       = " + unit_name       + '\n';
            ret += "unit_desc       = " + unit_desc       + '\n';
            ret += "unit_ico        = " + unit_ico        + '\n';
            ret += "unit_tex        = " + unit_tex        + '\n';
            ret += "unit_reward     = " + unit_reward     + '\n';
            ret += "unit_health_max = " + unit_health_max + '\n';

            return ret;
        }
    }

    public class BuildingData
    {
        public int WeaponUpgradePrice
        {
            get
            {
                if( building_upgrade_index < 0 )
                {
                    return building_weapon_data.WeaponUpgrades[0].UPrice;
                }
                else
                {
                    return building_weapon_data.WeaponUpgrades[ building_upgrade_index ].UPrice;
                }
            }
        }

        public int WeaponUpgradeTime
        {
            get
            {
                if( building_upgrade_index < 0 )
                {
                    return building_weapon_data.WeaponUpgrades[0].UBuildTime;
                }
                else
                {
                    return building_weapon_data.WeaponUpgrades[ building_upgrade_index ].UBuildTime;
                }
            }
        }

        public int WeaponRange
        {
            get
            {
                if( building_upgrade_index < 0 )
                {
                    return building_weapon_data.WpnRange;
                }
                else
                {
                    return building_weapon_data.WpnRange + building_weapon_data.WeaponUpgrades[ building_upgrade_index ].URangeBonus;
                }
            }
        }

        public float WeaponFirerate
        {
            get
            {
                if( building_upgrade_index < 0 )
                {
                    return building_weapon_data.WpnFirerate;
                }
                else
                {
                    return building_weapon_data.WpnFirerate + building_weapon_data.WeaponUpgrades[ building_upgrade_index ].UFirerateBonus;
                }
            }
        }

        public float WeaponDamage
        {
            get
            {
                if( building_upgrade_index < 0 )
                {
                    return building_weapon_data.WpnDamage;
                }
                else
                {
                    return building_weapon_data.WpnDamage + building_weapon_data.WeaponUpgrades[ building_upgrade_index ].UDamageBonus;
                }
            }
        }

        public int WeaponSellPrice
        {
            get
            {
                if( building_upgrade_index < 0 )
                {
                    return building_weapon_data.WeaponSellPrice;
                }
                else
                {
                    return building_weapon_data.WeaponSellPrice + building_weapon_data.WeaponUpgrades[ building_upgrade_index ].USellPriceBonus;
                }
            }
        }

        public WeaponData WeaponData
        {
            get{ return building_weapon_data;  }
            set{ building_weapon_data = value; }
        }

        public int WeaponUpgradeIndex
        {
            get{ return building_upgrade_index;  }
            set{ building_upgrade_index = value; }
        }

        private WeaponData building_weapon_data   = null;
        private int        building_upgrade_index = -1;
    }
}
