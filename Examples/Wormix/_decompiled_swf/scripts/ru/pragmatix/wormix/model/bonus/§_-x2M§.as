package ru.pragmatix.wormix.model.bonus
{
   import §_-31W§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-zF§;
   import §_-79§.§_-sd§;
   import §_-A1K§.§_-TA§;
   import §_-Af§.§_-k1I§;
   import §_-B1y§.*;
   import §_-B2z§.Server;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-c1G§;
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-j15§;
   import §_-G2H§.*;
   import §_-GQ§.§_-31v§;
   import §_-GQ§.§_-HC§;
   import §_-GQ§.§_-I2C§;
   import §_-GQ§.§_-J3E§;
   import §_-GQ§.§_-V12§;
   import §_-GQ§.§_-Vh§;
   import §_-GQ§.§_-e2T§;
   import §_-GQ§.§_-yT§;
   import §_-H3§.*;
   import §_-Jd§.BonusStat;
   import §_-Jd§.BonusTrait;
   import §_-Ly§.§_-81L§;
   import §_-SP§.§_-M4§;
   import §_-Ta§.*;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-V1H§.§_-G2W§;
   import §_-V1H§.§_-Ph§;
   import §_-V1H§.§_-SI§;
   import §_-V1H§.§_-c1E§;
   import §_-V1H§.§_-m1d§;
   import §_-V1H§.§_-n20§;
   import §_-V1H§.§_-p2P§;
   import §_-V1H§.§_-s1h§;
   import §_-V1H§.§_-zw§;
   import §_-YF§.§_-q2v§;
   import §_-ZP§.§_-H1H§;
   import §_-ZP§.§_-w1J§;
   import §_-b1F§.§_-v2Q§;
   import §_-j1c§.§_-E3K§;
   import §_-j1c§.§_-F3t§;
   import §_-j1c§.§_-Nv§;
   import §_-j1c§.§_-x1A§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import §_-s1Q§.Artifacts;
   import §_-s20§.§_-6a§;
   import §_-x2Q§.§_-dU§;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import config.§_-vR§;
   import feathers.controls.Button;
   import feathers.controls.ProgressBar;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.layout.Direction;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.*;
   import ru.pragmatix.wormix.messages.client.ReorderGroup;
   import ru.pragmatix.wormix.messages.client.SelectStuff;
   import ru.pragmatix.wormix.messages.structures.SelectStuffStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.utils.§_-y1V§;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.filters.ColorMatrixFilter;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   
   public class §_-x2M§
   {
      
      public static var all:Vector.<§_-x2M§> = new Vector.<§_-x2M§>();
      
      public static const HP:§_-x2M§ = new §_-x2M§("hp",§_-zw§,§_-s2a§.HP,§_-X8§,167660,"HpIcon");
      
      public static const ATTACK:§_-x2M§ = new §_-x2M§("attack",§_-c1E§,§_-s2a§.ATTACK,§_-X8§,11730944,"AttackIcon");
      
      public static const ARMOR:§_-x2M§ = new §_-x2M§("armor",§_-s1h§,§_-s2a§.ARMOR,§_-X8§,6856467,"DefenceIcon");
      
      public static const §_-53e§:§_-x2M§ = new §_-x2M§("armorEachLevels",§_-m1d§,§_-s2a§.ARMOR,§_-X8§,6856467,"DefenceIcon");
      
      public static const SQUAD_ATTACK:§_-x2M§ = new §_-x2M§("squadAttack",§_-G2W§,"HAT.SQUAD_ATTACK",§_-X8§,11730944,"SquadAttackIcon");
      
      public static const SQUAD_ARMOR:§_-x2M§ = new §_-x2M§("squadArmor",§_-p2P§,"HAT.SQUAD_ARMOR",§_-X8§,6856467,"SquadDefenceIcon");
      
      public static const §_-93u§:§_-x2M§ = new §_-x2M§("hpPct",§_-SI§,"","",16777215);
      
      public static const SPEED:§_-x2M§ = new §_-x2M§("speed",§_-Ph§,§_-s2a§.§_-Rl§,§_-X8§,167660,"SpeedSmallIcon");
      
      public static const MIGHTY_JUMP:§_-x2M§ = new §_-x2M§("mightyJump",§_-31v§,§_-s2a§.§_-iY§);
      
      public static const DOUBLE_JUMP:§_-x2M§ = new §_-x2M§("doubleJump",§_-n20§,§_-s2a§.§_-Q2t§);
      
      public static const §_-Yn§:§_-x2M§ = new §_-x2M§("waterBreath",§_-HC§,§_-s2a§.§_-13h§);
      
      public static const §_-NL§:§_-x2M§ = new §_-x2M§("slowProtection",BonusTrait,§_-s2a§.§_-zj§);
      
      public static const REGENERATION:§_-x2M§ = new §_-x2M§("regeneration",§_-F3t§,§_-s2a§.REGENERATION,§_-X8§,16737894,"RegenerationSmallIcon");
      
      public static const LEECH:§_-x2M§ = new §_-x2M§("leech",§_-x1A§,§_-s2a§.LEECH,§_-01N§,10027212,"VampyricSmallIcon");
      
      public static const DODGE:§_-x2M§ = new §_-x2M§("dodge",§_-E3K§,§_-s2a§.DODGE,§_-01N§,10040064,"DodgeSmallIcon");
      
      public static const HEAL_BONUS:§_-x2M§ = new §_-x2M§("healBonus",BonusStat,§_-s2a§.HEAL_BONUS,§_-i1c§,16750848,"AidBonusSmallIcon");
      
      public static const §_-D2I§:§_-x2M§ = new §_-x2M§("poisonDamage",BonusStat,§_-s2a§.ATTACK_POISON,§_-i1c§,39321,"PoisonDamageSmallIcon");
      
      public static const §_-82B§:§_-x2M§ = new §_-x2M§("poisonDefense",BonusStat,§_-s2a§.§_-N1k§,§_-i1c§,39321,"PoisonResistSmallIcon");
      
      public static const §_-Cx§:§_-x2M§ = new §_-x2M§("frostDefense",BonusStat,§_-s2a§.§_-Yo§,§_-i1c§,823713,"FrostResistSmallIcon");
      
      public static const FROST_DAMAGE:§_-x2M§ = new §_-x2M§("frostDamage",BonusStat,§_-s2a§.§_-Q1m§,§_-i1c§,823713,"FrostDamageSmallIcon");
      
      public static const IMMOBILITY_REDUCTION:§_-x2M§ = new §_-x2M§("immobilityReduction",BonusStat,§_-s2a§.§_-N1G§,§_-X8§,823713,"FreezeReductionIcon");
      
      public static const §_-V§:§_-x2M§ = new §_-x2M§("fireDamage",BonusStat,§_-s2a§.ATTACK_FIRE,§_-i1c§,16711680,"FireDamageSmallIcon");
      
      public static const §_-Q26§:§_-x2M§ = new §_-x2M§("fireDefense",BonusStat,§_-s2a§.REDUCTION_FIRE,§_-i1c§,16750848,"FireResistSmallIcon");
      
      public static const §_-X1M§:§_-x2M§ = new §_-x2M§("shockDefense",BonusStat,§_-s2a§.REDUCTION_SHOCK,§_-i1c§,3789034,"LightningResistSmallIcon");
      
      public static const §_-V1x§:§_-x2M§ = new §_-x2M§("shockDamage",BonusStat,§_-s2a§.ATTACK_SHOCK,§_-i1c§,4845221,"LightningSmallIcon");
      
      public static const §_-y2F§:§_-x2M§ = new §_-x2M§("airDamage",BonusStat,§_-s2a§.INCREASE_AIR_DAMAGE,§_-i1c§,4008735,"icon_stat_air_damage");
      
      public static const §_-S1O§:§_-x2M§ = new §_-x2M§("airReduction",BonusStat,§_-s2a§.AIR_REDUCTION,§_-i1c§,4008735,"icon_stat_air_defence");
      
      public static const FORTITUDE:§_-x2M§ = new §_-x2M§("fortitude",§_-V12§,§_-s2a§.§_-A1T§);
      
      public static const EXPLOSION_DEATH:§_-x2M§ = new §_-x2M§("explosionDeath",§_-Nv§,§_-s2a§.§_-VA§);
      
      public static const PENETRATION:§_-x2M§ = new §_-x2M§("penetration",BonusStat,§_-s2a§.§_-K38§,§_-i1c§,15400981,"ArmorPenetrationSmallIcon");
      
      public static const WATER_PROTECTION:§_-x2M§ = new §_-x2M§("waterProtection",§_-Vh§,§_-s2a§.§_-J2H§);
      
      public static const §_-435§:§_-x2M§ = new §_-x2M§("legendaryHatFirstAct",§_-J3E§,§_-s2a§.§_-T25§,"",7829367,"HpIcon");
      
      public static const §_-iS§:§_-x2M§ = new §_-x2M§("legendaryArtifactFirstAct",§_-I2C§,§_-s2a§.§_-T25§,"",7829367,"HpIcon");
      
      public static const §_-x1I§:§_-x2M§ = new §_-x2M§("epicHatFirstAct",§_-e2T§,§_-s2a§.§_-w1R§,"",7829367,"HpIcon");
      
      public static const §_-S1g§:§_-x2M§ = new §_-x2M§("epicArtifactFirstAct",§_-yT§,§_-s2a§.§_-w1R§,"",7829367,"HpIcon");
      
      private static const §_-X8§:String = "{sign}{value}";
      
      private static const §_-01N§:String = "{value}%";
      
      private static const §_-i1c§:String = "{sign}{value}%";
      
      private static const §_-Sr§:Vector.<§_-x2M§> = new <§_-x2M§>[HP,DOUBLE_JUMP];
      
      §§push(§§findproperty(§_-9B§));
      
      private static const §_-9B§:Vector.<§_-x2M§> = new <§_-x2M§>[§_-435§,§_-iS§];
      
      private static const §_-I1s§:Vector.<§_-x2M§> = new <§_-x2M§>[§_-x1I§,§_-S1g§];
      
      private var name:String;
      
      private var §_-TV§:Class;
      
      private var title:String;
      
      private var description:String;
      
      private var §_-D2b§:uint;
      
      private var iconName:String;
      
      public function §_-x2M§(param1:String, param2:Class, param3:String, param4:String = "", param5:uint = 16777215, param6:String = null)
      {
         super();
         this.name = param1;
         this.§_-TV§ = param2;
         this.title = param3;
         this.description = param4;
         this.§_-D2b§ = param5;
         this.iconName = param6;
         all[all.length] = this;
      }
      
      public static function §_-N24§(param1:§_-x2M§) : Boolean
      {
         return §_-Sr§.indexOf(param1) == -1;
      }
      
      public static function §_-31B§(param1:§_-x2M§) : Boolean
      {
         return §_-9B§.indexOf(param1) > -1;
      }
      
      public static function §_-O2§(param1:§_-x2M§) : §_-x2M§
      {
         return §_-I1s§[§_-9B§.indexOf(param1)];
      }
      
      public function getName() : String
      {
         return this.name;
      }
      
      public function getClass() : Class
      {
         return this.§_-TV§;
      }
      
      public function §_-23r§() : String
      {
         return this.title;
      }
      
      public function §_-p1§(param1:Boolean = false) : String
      {
         if(param1 && this.description == §_-01N§)
         {
            return §_-i1c§;
         }
         return this.description;
      }
      
      public function §_-5B§() : uint
      {
         return this.§_-D2b§;
      }
      
      public function §_-M2c§() : String
      {
         return this.iconName;
      }
   }
}

