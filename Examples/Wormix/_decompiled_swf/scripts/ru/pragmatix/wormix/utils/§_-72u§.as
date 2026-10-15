package ru.pragmatix.wormix.utils
{
   import §_-02q§.§_-4s§;
   import §_-62§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-L2F§.§_-D2p§;
   import §_-P1B§.§_-F1q§;
   import §_-RE§.§_-r2q§;
   import §_-T2V§.§_-13F§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-Z2S§.§_-c2q§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-F3m§;
   import §_-l1K§.§_-I7§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.X509CertificateCollection;
   import §_-q26§.*;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-H3D§;
   import §_-z1g§.§_-lp§;
   import com.hurlant.crypto.tls.*;
   import dragonBones.animation.WorldClock;
   import feathers.layout.VerticalLayout;
   import flash.display.DisplayObject;
   import flash.geom.Point;
   import flash.globalization.DateTimeFormatter;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.ClanSummaryRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.ClanSummaryResponse;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.server.GetFriendListPageResult;
   import ru.pragmatix.wormix.messages.server.ProfilesResult;
   import ru.pragmatix.wormix.messages.server.SteamTransactionCommitResponse;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-72u§
   {
      
      public static const §_-E1O§:int = 1000;
      
      public static const §_-71e§:int = 1000 * 60;
      
      public static const §_-x1R§:int = 1000 * 60 * 60;
      
      public static const §_-w2h§:int = 60;
      
      public static const §_-03d§:int = 60 * 60;
      
      public static const §_-M0§:int = 60;
      
      public static const §_-E3g§:int = 24;
      
      public static const §_-Ei§:int = 24 * 60 * 60;
      
      private static const §_-J3C§:Array = [];
      
      private static const §_-o2Y§:Array = [];
      
      public function §_-72u§()
      {
         super();
      }
      
      public static function §_-74§(param1:int) : int
      {
         return Math.floor(param1 / (§_-w2h§ * §_-M0§ * §_-E3g§));
      }
      
      public static function §_-M16§(param1:int) : int
      {
         return §_-5L§() - param1;
      }
      
      public static function §_-5L§() : uint
      {
         return §_-X1j§.serverTime + uint(getTimer() / §_-E1O§);
      }
      
      public static function §_-rO§() : uint
      {
         return §_-X1j§.serverTime * §_-E1O§ + getTimer();
      }
      
      public static function §_-X1O§() : Number
      {
         return new Date().getTime() / §_-E1O§;
      }
      
      public static function §_-m1w§() : Number
      {
         return new Date().getTime();
      }
      
      public static function §_-T1Y§(param1:Number) : Number
      {
         var _loc2_:Number = §_-X1O§() / §_-Ei§;
         return _loc2_ - param1;
      }
      
      public static function §_-q1c§(param1:Number) : Number
      {
         var _loc2_:Number = §_-X1O§() / §_-03d§;
         return _loc2_ - param1;
      }
      
      public static function §_-p2H§(param1:int) : Date
      {
         var _loc2_:Date = new Date("yyyy-MM-dd HH:mm:ss");
         _loc2_.setTime(param1 * §_-E1O§);
         return _loc2_;
      }
      
      public static function §_-i2g§(param1:Number) : Date
      {
         var _loc2_:Date = new Date("yyyy-MM-dd HH:mm:ss.SSS");
         _loc2_.setTime(param1);
         return _loc2_;
      }
      
      public static function §_-v1s§() : String
      {
         var _loc1_:Number = §_-X1O§();
         return §_-9Q§(_loc1_,"yyyy-MM-dd HH:mm:ss");
      }
      
      public static function §_-I1A§() : String
      {
         var _loc1_:Number = §_-m1w§();
         var _loc2_:int = _loc1_ - int(_loc1_ / §_-E1O§) * §_-E1O§;
         return §_-K2t§(_loc1_) + "." + (_loc2_ < 10 ? "00" : (_loc2_ < 100 ? "0" : "")) + _loc2_;
      }
      
      public static function §_-9Q§(param1:int, param2:String = "yyyy-MM-dd HH:mm") : String
      {
         var _loc3_:Date = null;
         var _loc4_:DateTimeFormatter = null;
         _loc3_ = §_-p2H§(param1);
         _loc4_ = new DateTimeFormatter("eu-EU");
         _loc4_.setDateTimePattern(param2);
         return _loc4_.format(_loc3_);
      }
      
      public static function §_-K2t§(param1:Number, param2:String = "yyyy-MM-dd HH:mm:ss") : String
      {
         var _loc3_:Date = null;
         var _loc4_:DateTimeFormatter = null;
         _loc3_ = §_-i2g§(param1);
         _loc4_ = new DateTimeFormatter("eu-EU");
         _loc4_.setDateTimePattern(param2);
         return _loc4_.format(_loc3_);
      }
      
      private static function §_-jK§() : void
      {
         §_-J3C§.push("jan_short","feb_short","mar_short","apr_short","may_short","jun_short","jul_short","aug_short","sep_short","oct_short","nov_short","dec_short");
      }
      
      private static function §_-C1g§() : void
      {
         §_-o2Y§.push("jan_full","feb_full","mar_full","apr_full","may_full","jun_full","jul_full","aug_full","sep_full","oct_full","nov_full","dec_full");
      }
      
      public static function §_-9i§(param1:int) : String
      {
         if(§_-J3C§.length == 0)
         {
            §_-jK§();
         }
         return 0 <= param1 && param1 < §_-J3C§.length ? §_-s2a§.§_-y2o§(§_-J3C§[param1]) : "";
      }
      
      public static function §_-G2v§(param1:int) : String
      {
         if(§_-o2Y§.length == 0)
         {
            §_-C1g§();
         }
         return 0 <= param1 && param1 < §_-o2Y§.length ? §_-s2a§.§_-y2o§(§_-o2Y§[param1]) : "";
      }
      
      public static function §_-91§(param1:int) : Boolean
      {
         var _loc2_:Date = §_-p2H§(param1);
         var _loc3_:Date = §_-p2H§(§_-5L§());
         return _loc2_.fullYear == _loc3_.fullYear && _loc2_.month == _loc3_.month && _loc2_.date == _loc3_.date;
      }
      
      public static function §_-826§(param1:int) : String
      {
         var _loc2_:String = "";
         if(param1 <= 0)
         {
            return "";
         }
         _loc2_ = §_-s2a§.§_-K3t§(§_-s2a§.HAT_DISAPPEAR) + " ";
         return _loc2_ + §_-I1I§(param1);
      }
      
      public static function §_-I1I§(param1:int, param2:Boolean = false) : String
      {
         var _loc6_:int = 0;
         var _loc3_:String = "";
         if(param1 <= 0)
         {
            return "";
         }
         var _loc4_:int = param1 / (§_-E1O§ * §_-w2h§);
         var _loc5_:int = _loc4_ / §_-M0§;
         if(_loc5_ > 0)
         {
            _loc4_ -= _loc5_ * §_-M0§;
         }
         if(!param2)
         {
            _loc6_ = _loc5_ / §_-E3g§;
            if(_loc6_ > 0)
            {
               _loc5_ -= _loc6_ * §_-E3g§;
            }
            if(_loc6_ > 0)
            {
               _loc3_ += §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.DAY_TEXT),{"value":_loc6_}) + " ";
            }
         }
         if(_loc5_ > 0)
         {
            _loc3_ += §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.HOURS_TEXT),{"value":_loc5_}) + " ";
         }
         if(_loc4_ > 0)
         {
            _loc3_ += §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.MINUTE_TEXT),{"value":_loc4_}) + " ";
         }
         else if(_loc6_ == 0 && _loc5_ == 0)
         {
            _loc3_ += §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.MINUTE_TEXT),{"value":1}) + " ";
         }
         return _loc3_;
      }
   }
}

