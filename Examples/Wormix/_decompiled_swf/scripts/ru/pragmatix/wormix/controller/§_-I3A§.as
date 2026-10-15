package ru.pragmatix.wormix.controller
{
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-zF§;
   import §_-79§.§_-ps§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-Af§.§_-k1I§;
   import §_-B1y§.Cookie;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-c1G§;
   import §_-Bv§.§_-G3P§;
   import §_-C2M§.DesktopTutorialPanel;
   import §_-C2M§.MobileTutorialPanel;
   import §_-C2M§.§_-o2f§;
   import §_-C2t§.§_-92W§;
   import §_-C2t§.§_-i1V§;
   import §_-CF§.§_-p2U§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-K3O§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.§_-Ey§;
   import §_-H2g§.§_-di§;
   import §_-H3§.*;
   import §_-J31§.§_-R1h§;
   import §_-J31§.§_-e2J§;
   import §_-Jd§.§_-g2F§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.§_-52p§;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-U16§;
   import §_-Vg§.*;
   import §_-W§.§_-Ap§;
   import §_-X14§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-Y1b§.§_-81l§;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-eO§.*;
   import §_-f1G§.§_-kf§;
   import §_-fo§.§_-A29§;
   import §_-hO§.§_-03k§;
   import §_-j1w§.§_-L1x§;
   import §_-j1w§.§_-T0§;
   import §_-j1y§.§_-i1b§;
   import §_-j22§.§_-5g§;
   import §_-k13§.§_-j23§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Vr§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-o14§.§_-Dd§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.*;
   import §_-s1Q§.Artifacts;
   import §_-s20§.§_-6a§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-C32§;
   import §_-z1g§.§_-lp§;
   import §_-zI§.§_-F3q§;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import com.hurlant.util.der.*;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.List;
   import feathers.controls.Panel;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ToggleButton;
   import feathers.core.FeathersControl;
   import feathers.core.PopUpManager;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.system.ApplicationDomain;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import org.as3commons.zip.§_-G22§;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.controls.sets.AbstractControlSet;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-fT§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.InviteToClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanInviteTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.GetFriendListPage;
   import ru.pragmatix.wormix.messages.client.PvpChatMessage;
   import ru.pragmatix.wormix.messages.client.ReorderGroup;
   import ru.pragmatix.wormix.messages.client.RepeatLevelUpAwardRequest;
   import ru.pragmatix.wormix.messages.server.GetFriendListPageResult;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.Girder;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.Meteor;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.type.RopeHook;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.text.TextField;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   use namespace gestouch_internal;
   use namespace bi_internal;
   
   public class §_-I3A§
   {
      
      private static var §_-i1N§:§_-I3A§ = new §_-I3A§();
      
      private static const §_-f1D§:uint = 9;
      
      private static const §_-B3J§:uint = 27;
      
      private var loadedProfiles:Array = [];
      
      private var §_-YT§:Array = [];
      
      private var §_-X10§:uint;
      
      private var §_-j2s§:uint;
      
      public function §_-I3A§()
      {
         super();
      }
      
      public static function §_-F1k§() : uint
      {
         return §_-i1N§.§_-X10§;
      }
      
      public static function §_-31X§(param1:Array, param2:Array, param3:uint, param4:uint, param5:uint) : Array
      {
         return §_-i1N§.§_-31X§(param1,param2,param3,param4,param5);
      }
      
      public static function §_-w1S§(param1:uint, param2:uint, param3:Function) : void
      {
         §_-i1N§.§_-w1S§(param1,param2,param3);
      }
      
      public static function §_-m1q§() : uint
      {
         return §_-i1N§.§_-m1q§();
      }
      
      public static function §_-bz§() : uint
      {
         return §_-i1N§.§_-j2s§;
      }
      
      public static function getAppFriends() : Array
      {
         return §_-i1N§.§_-YT§;
      }
      
      public static function §_-M1d§(param1:String) : Boolean
      {
         return §_-i1N§.§_-M1d§(param1);
      }
      
      public static function §_-E14§(param1:SocialType) : String
      {
         return §_-i1N§.§_-E14§(param1);
      }
      
      public static function §_-52T§(param1:int) : Boolean
      {
         var _loc2_:int = int(§_-I3A§.getAppFriends().length);
         var _loc3_:Array = §_-I3A§.getAppFriends();
         var _loc4_:int = 0;
         while(_loc4_ < _loc2_)
         {
            if(_loc3_[_loc4_] == param1)
            {
               return true;
            }
            _loc4_++;
         }
         return false;
      }
      
      public static function clear() : void
      {
         §_-i1N§ = new §_-I3A§();
      }
      
      private function §_-31X§(param1:Array, param2:Array, param3:uint, param4:uint, param5:uint) : Array
      {
         var _loc7_:UserProfile = null;
         var _loc8_:int = 0;
         this.§_-X10§ = param4;
         this.§_-j2s§ = Math.min(param3,param5);
         this.§_-YT§ = param2;
         var _loc6_:Array = §_-33r§.§_-i1§(param1);
         if(_loc6_.length == 0 && this.loadedProfiles.length > 0)
         {
            this.loadedProfiles = [];
         }
         else
         {
            _loc8_ = 0;
            while(_loc8_ < param4)
            {
               _loc7_ = _loc6_[_loc8_] as UserProfile;
               if(_loc7_)
               {
                  _loc7_.§_-x2F§(true);
               }
               _loc8_++;
            }
            this.§_-v1R§(0,_loc6_);
         }
         return this.loadedProfiles;
      }
      
      public function §_-v1R§(param1:uint, param2:Array) : void
      {
         var _loc3_:uint = 0;
         while(_loc3_ < param2.length)
         {
            if(this.loadedProfiles.indexOf(param2[_loc3_]) == -1)
            {
               this.loadedProfiles[param1 + _loc3_] = param2[_loc3_];
            }
            _loc3_++;
         }
      }
      
      private function §_-w1S§(param1:uint, param2:uint, param3:Function) : void
      {
         var _loc6_:uint = 0;
         var _loc7_:Object = null;
         var _loc4_:Object = {
            "firstIndex":param1,
            "pageSize":param2,
            "callback":param3
         };
         var _loc5_:uint = Math.min(param1 + param2,this.§_-j2s§ - 1);
         if(!this.§_-f2l§(param1,_loc5_))
         {
            _loc6_ = this.§_-G3o§(param1,param2);
            _loc4_.serverPage = _loc6_;
            _loc7_ = new GetFriendListPage(_loc6_);
            §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc7_,GetFriendListPageResult,this.§_-I24§,this.§_-w19§,_loc4_);
         }
         else
         {
            param3(this.§_-91s§(_loc4_));
         }
      }
      
      private function §_-G3o§(param1:uint, param2:uint) : uint
      {
         return uint(1 + Math.ceil(Math.max(param1 + param2 - §_-f1D§,0) / §_-B3J§));
      }
      
      private function §_-E2l§(param1:uint) : uint
      {
         if(param1 == 1)
         {
            return 0;
         }
         return §_-f1D§ + (param1 - 2) * §_-B3J§;
      }
      
      private function §_-f2l§(param1:uint, param2:uint) : Boolean
      {
         return this.loadedProfiles[param1] != null && this.loadedProfiles[param2] != null;
      }
      
      private function §_-91s§(param1:Object) : Array
      {
         return this.loadedProfiles.slice(param1.firstIndex,Math.min(Math.min(param1.firstIndex + param1.pageSize,this.loadedProfiles.length),this.§_-j2s§));
      }
      
      private function §_-I24§(param1:§_-63i§, param2:Object) : void
      {
         var _loc7_:UserProfileStructure = null;
         var _loc3_:GetFriendListPageResult = GetFriendListPageResult(param1.message);
         var _loc4_:Array = _loc3_.profileStructures;
         var _loc5_:String = "";
         var _loc6_:uint = 0;
         while(_loc6_ < _loc4_.length)
         {
            _loc7_ = _loc4_[_loc6_];
            _loc5_ += " " + _loc7_.id;
            _loc6_++;
         }
         this.§_-I3W§(_loc4_,param2);
      }
      
      private function §_-w19§(param1:Object, param2:Object) : void
      {
         param2.callback(this.§_-91s§(param2));
      }
      
      private function §_-I3W§(param1:Array, param2:Object) : void
      {
         new §_-Zi§().§_-Zb§(param1,§_-bs§.§_-d1Z§(this.§_-M13§,this,[param2],true));
      }
      
      private function §_-M13§(param1:Array, param2:Boolean, param3:Object) : void
      {
         this.§_-v1R§(this.§_-E2l§(param3.serverPage),param1);
         param3.callback(this.§_-91s§(param3));
      }
      
      public function §_-m1q§() : uint
      {
         return this.§_-j2s§;
      }
      
      public function §_-M1d§(param1:String) : Boolean
      {
         var _loc2_:String = null;
         for each(_loc2_ in §_-I3A§.getAppFriends())
         {
            if(_loc2_ == param1)
            {
               return true;
            }
         }
         return false;
      }
      
      public function §_-E14§(param1:SocialType) : String
      {
         var _loc2_:String = null;
         var _loc3_:String = null;
         var _loc4_:UserProfile = null;
         if(param1.equals(SocialType.FACEBOOK))
         {
            _loc2_ = "";
            for each(_loc4_ in this.loadedProfiles)
            {
               _loc3_ = _loc4_.getSocialIds()[param1];
               if(_loc3_)
               {
                  _loc2_ += (_loc2_ != "" ? "," : "") + _loc3_;
               }
            }
         }
         return _loc2_;
      }
   }
}

