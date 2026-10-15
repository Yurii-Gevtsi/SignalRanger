package ru.pragmatix.wormix.weapons
{
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-Z1K§.§_-q24§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-T2N§;
   import §_-u1P§.ShopResultEvent;
   import flash.errors.IllegalOperationError;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   
   public class §_-73v§
   {
      
      public static const §_-Ix§:Class = Mortar;
      
      public static const §_-Y27§:Class = FireThrower;
      
      public static const §_-I3R§:Class = Minigun;
      
      public static const §_-Z1E§:Class = HomingMinigun;
      
      public static const §_-7F§:Class = Bazooka;
      
      public static const §_-jy§:Class = Grenade;
      
      public static const §_-S1x§:Class = ClusterGrenade;
      
      public static const §_-e19§:Class = ClusterGrenadeExplodable;
      
      public static const §_-T12§:Class = Rifle;
      
      public static const §_-I2n§:Class = Uzi;
      
      public static const §_-X2S§:Class = DrillGun;
      
      public static const §_-d1u§:Class = CatBazooka;
      
      public static const §_-y2m§:Class = Shotgun;
      
      public static const §_-C2o§:Class = WatermelonBomb;
      
      public static const §_-O8§:Class = StickyGrenade;
      
      public static const §_-W1N§:Class = SniperRifle;
      
      public static const §_-m2M§:Class = Firework;
      
      public static const §_-S1C§:Class = Molotov;
      
      public static const §_-i1U§:Class = MolotovExplodable;
      
      public static const §_-n2e§:Class = HomingBazooka;
      
      public static const §_-vY§:Class = SwineWeapon;
      
      public static const §_-t2S§:Class = SuperBoarWeapon;
      
      public static const §_-F3e§:Class = ShurikenToss;
      
      public static const §_-Q16§:Class = Cannon;
      
      public static const §_-i1r§:Class = ElectricHarpoon;
      
      public static const §_-G1C§:Class = Kalash;
      
      public static const §_-y1e§:Class = FreezingBazooka;
      
      public static const §_-W2p§:Class = AlterableFreezingBazooka;
      
      public static const §_-e1B§:Class = Crossbow;
      
      public static const §_-O1s§:Class = GroundBreaker;
      
      public static const §_-m2c§:Class = Helipad;
      
      public static const §_-c19§:Class = UfoPad;
      
      public static const §_-W8§:Class = DiscLauncher;
      
      public static const §_-M2s§:Class = HoppingWeapon;
      
      public static const §_-mn§:Class = CircusCannon;
      
      public static const §_-c26§:Class = BoomerangWeapon;
      
      public static const §_-23i§:Class = Shocker;
      
      public static const §_-4P§:Class = Sledgehammer;
      
      public static const §_-G3f§:Class = SledgehammerDownGradable;
      
      public static const §_-w1i§:Class = Bat;
      
      public static const §_-SZ§:Class = BatDownGradable;
      
      public static const §_-v1h§:Class = Dynamite;
      
      public static const §_-O1E§:Class = MineWeapon;
      
      public static const §_-929§:Class = SpiderWeapon;
      
      public static const §_-j20§:Class = FreezingMineWeapon;
      
      public static const §_-a2p§:Class = GuardingBotWeapon;
      
      public static const AIR_STRIKE:Class = AirStrike;
      
      public static const §_-F29§:Class = MeteorStrike;
      
      public static const CAT_STRIKE:Class = CatStrike;
      
      public static const NAPALM_STRIKE:Class = NapalmStrike;
      
      public static const BUNKER_BUSTER:Class = BunkerBuster;
      
      public static const §_-Y17§:Class = Drill;
      
      public static const §_-x1s§:Class = Auger;
      
      public static const §_-tb§:Class = Teleport;
      
      public static const §_-818§:Class = Jetpack;
      
      public static const §_-g1C§:Class = TeleThrower;
      
      public static const §_-71L§:Class = RopeGun;
      
      public static const §_-E1s§:Class = SupplyRopeGun;
      
      public static const §_-51b§:Class = Girder;
      
      public static const §_-D2d§:Class = §_-E3w§;
      
      public static const SWITCH_TURN:Class = SwitchTurn;
      
      public static const FORCE_FIELD:Class = ForceField;
      
      public static const §_-L2u§:Class = AntifreezePotion;
      
      public static const SUPER_BUNKER_BUSTER:Class = SuperBunkerBuster;
      
      public static const EMERGENCY_TELEPORT:Class = EmergencyTeleport;
      
      public static const BANDAGE:Class = Bandage;
      
      public static const GRAVITY_GUN:Class = GravityGun;
      
      public static const THUNDERBOLT:Class = Thunderbolt;
      
      public static const BLIZZARD:Class = Blizzard;
      
      public static const LASER:Class = LaserBarrier;
      
      public static const FROZEN_ORB:Class = FrozenOrbWeapon;
      
      public static const §_-E2U§:Class = AtomBombWeapon;
      
      public static const §_-F3C§:Class = §_-d13§;
      
      public static const ANTITOXIN:Class = Antitoxin;
      
      public static const §_-03L§:Class = ElectroGun;
      
      public static const §_-71b§:Class = §_-K3V§;
      
      public static const §_-qb§:Class = GasGrenadeWeapon;
      
      public static const §_-53X§:Class = GasGrenadeExplodable;
      
      public static const §_-S1Y§:Class = HealBlockerWeapon;
      
      public static const §_-t1f§:Class = PumpkinWeapon;
      
      public static const ZOMBIE_MARCH:Class = ZombieMarch;
      
      public static const §_-j1S§:Class = AssassinHeavyKnifeWeapon;
      
      public static const §_-mO§:Class = AssassinFanOfKnives;
      
      public static const §_-i2§:Class = ChainGun;
      
      public static const §_-E3k§:Class = FireMinigun;
      
      public static const §_-60§:Class = ArchbotSacrifice;
      
      public static const PARASITE:Class = ParasiteWeapon;
      
      public static const §_-B2F§:Class = KamikazeDynamite;
      
      public static const §_-L13§:Class = ControlledSwineWeapon;
      
      public static const LASER_GUN:Class = LaserGun;
      
      public static const §_-v22§:Class = JumpingShocker;
      
      public static const §_-L2G§:Class = ThrowableMineWeapon;
      
      public static const §_-s2§:Class = GrabberGun;
      
      public static const §_-OE§:Class = MedEmergencyTeleport;
      
      public static const PUSHING_MINE:Class = PushingMineWeapon;
      
      public static const ANVIL:Class = AnvilWeapon;
      
      public static const §_-22S§:Class = Blizzard;
      
      public static const §_-41a§:Class = FrozenOrbWeapon;
      
      public static const §_-Eo§:Class = SoulMinigun;
      
      public static const §_-Lv§:Class = DoctorBag;
      
      public function §_-73v§()
      {
         super();
         throw new IllegalOperationError("Arsenal cannot be instantiated");
      }
      
      public static function getWeapon(param1:int) : Weapon
      {
         return §_-G1p§.§_-I4§(param1).getWeapon();
      }
      
      public static function §_-z1F§(param1:UserProfile) : Weapon
      {
         var _loc2_:§_-Oi§ = §_-Cq§.§_-J2a§(§_-q24§.BAZOOKA,param1.§_-pd§());
         return _loc2_ ? §_-O2H§(_loc2_.getRecord()).getWeapon() : getWeapon(§_-q24§.BAZOOKA);
      }
      
      public static function §_-a2h§() : void
      {
         §_-G1p§.§_-a2h§();
      }
   }
}

