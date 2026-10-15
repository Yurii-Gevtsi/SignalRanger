package ru.pragmatix.wormix.model.user
{
   import §_-21p§.§_-4w§;
   import §_-23P§.§_-V2T§;
   import §_-23P§.§_-u1z§;
   import §_-51p§.§_-W2e§;
   import §_-533§.§_-c2b§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.*;
   import §_-73d§.§_-v1o§;
   import §_-79§.§_-11E§;
   import §_-82l§.§_-A2Y§;
   import §_-937§.§_-n1b§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-F39§.§_-a2W§;
   import §_-GQ§.§_-31v§;
   import §_-L1H§.§_-C3e§;
   import §_-L1w§.§_-CD§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-P1z§.*;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-Vg§.§_-M11§;
   import §_-Y1O§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-Z1w§.*;
   import §_-ZP§.§_-b23§;
   import §_-hO§.§_-v24§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-52D§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import §_-k1u§.§_-Vr§;
   import §_-k1u§.§_-by§;
   import §_-k2s§.§_-Go§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-lh§.§_-z2A§;
   import §_-o14§.*;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-M2v§;
   import §_-z1g§.§_-lp§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.PurchaseRequest;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderStatus;
   import com.hurlant.util.der.§_-UO§;
   import config.§_-V2w§;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ToggleButton;
   import feathers.core.FeathersControl;
   import feathers.core.ToggleGroup;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import flash.display.InteractiveObject;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.EventPhase;
   import flash.events.MouseEvent;
   import flash.events.ProgressEvent;
   import flash.geom.Point;
   import flash.net.Socket;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.IUserProfile;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanInviteTO;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.*;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DynamiteStick;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.core.Starling;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Align;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   use namespace gestouch_internal;
   
   public class UserProfile extends EventDispatcher implements IUserProfile
   {
      
      internal var id:int;
      
      internal var platformId:int;
      
      internal var socialIds:Dictionary = new Dictionary();
      
      internal var §_-Dw§:Dictionary = new Dictionary();
      
      internal var ingameMoney:§_-TA§ = new §_-TA§(0);
      
      internal var realMoney:§_-TA§ = new §_-TA§(0);
      
      internal var rating:§_-TA§ = new §_-TA§(0);
      
      internal var dailyRating:§_-TA§ = new §_-TA§(0);
      
      private var oldPlace:§_-TA§ = new §_-TA§(0);
      
      internal var reactionRate:§_-TA§ = new §_-TA§(0);
      
      private var level:§_-TA§ = new §_-TA§(0);
      
      internal var experience:§_-TA§ = new §_-TA§(0);
      
      internal var charName:String;
      
      internal var §_-i2W§:String;
      
      internal var online:Boolean;
      
      internal var §_-U2G§:Number;
      
      private var §_-B2d§:§_-7u§;
      
      internal var §_-A2I§:§_-C2c§;
      
      internal var team:Array = new Array();
      
      internal var teamMemberType:int = §_-yA§.§_-F1v§;
      
      internal var §_-53P§:§_-m1x§ = new §_-m1x§();
      
      private var races:Vector.<§_-B2i§> = new Vector.<§_-B2i§>(0);
      
      private var §_-h2G§:§_-11E§;
      
      internal var §_-31d§:§_-2m§ = new §_-2m§();
      
      internal var §_-R1X§:§_-Vr§ = new §_-Vr§();
      
      internal var §_-V2K§:§_-F3H§ = new §_-F3H§();
      
      internal var §_-h2M§:§_-52D§ = new §_-52D§();
      
      internal var wardrobe:§_-by§ = new §_-by§();
      
      internal var §_-91o§:§_-H2M§ = new §_-H2M§();
      
      internal var §_-Q28§:§_-H2M§ = new §_-H2M§();
      
      public var aiArsenal:§_-A1l§;
      
      internal var §_-23h§:ClanMemberStructure;
      
      internal var §_-2N§:Vector.<TemporalStuffStructure>;
      
      internal var §_-v6§:§_-TA§ = new §_-TA§();
      
      internal var §_-D3N§:int;
      
      internal var §_-7P§:int;
      
      private var §_-t2J§:Boolean = true;
      
      private var builder:§_-w1H§;
      
      public function UserProfile()
      {
         super();
         this.builder = new §_-w1H§(this);
         this.§_-53P§.§_-BU§(this);
      }
      
      public static function custom(param1:String, param2:uint, param3:§_-L1x§, param4:uint, param5:uint, param6:uint, param7:uint = 0, param8:uint = 0, param9:§_-11E§ = null) : UserProfile
      {
         var _loc11_:§_-zF§ = null;
         var _loc10_:UserProfile = new UserProfile();
         _loc10_.charName = param1;
         _loc10_.§_-i2W§ = param1;
         _loc10_.§_-53P§.§_-g2I§ = new §_-TA§(param4);
         _loc10_.§_-53P§.§_-Z2U§ = new §_-TA§(param5);
         _loc10_.§_-53P§.§_-A3Z§ = new §_-TA§(param6);
         _loc10_.level = new §_-TA§(param2);
         _loc10_.§_-s1I§(param3.§_-wz§());
         if(param7 != 0)
         {
            _loc11_ = §_-G1p§.§_-12J§(param7,_loc10_.§_-53P§.raceRecord);
            _loc10_.§_-L§(_loc11_);
         }
         if(param8 != 0)
         {
            _loc10_.§_-aV§(§_-G1p§.§_-93G§(param8));
         }
         _loc10_.§_-h2G§ = param9;
         return _loc10_;
      }
      
      public static function §_-bR§(param1:Number, param2:String, param3:uint, param4:§_-L1x§, param5:uint, param6:uint, param7:uint, param8:uint = 0, param9:uint = 0, param10:§_-11E§ = null) : UserProfile
      {
         return custom(param2,Math.max(param3 * param1,1),param4,Math.max(param5 * param1,1),Math.max(param6 * param1,1),Math.max(param7 * param1,1),param8,param9,param10);
      }
      
      public function §_-dP§() : void
      {
         var _loc1_:Number = this.§_-u1a§();
         if(_loc1_ > 0)
         {
            this.§_-A2I§.run(_loc1_ / §_-72u§.§_-E1O§);
            this.§_-A2I§.addEventListener(starling.events.Event.COMPLETE,this.§_-C3y§);
            this.§_-A2I§.addEventListener(§_-C2c§.CHEAT,this.§_-C3y§);
         }
         else
         {
            this.§_-C3y§();
         }
      }
      
      public function §_-C3y§(param1:starling.events.Event = null) : void
      {
         var _loc2_:Number = this.§_-u1a§();
         if(_loc2_ > 0)
         {
            this.§_-A2I§.run(_loc2_ / §_-72u§.§_-E1O§);
         }
         else
         {
            this.§_-g2M§();
            if(!this.§_-B2d§)
            {
               this.§_-B2d§ = new §_-7u§();
            }
            if(Boolean(§_-X1j§.§_-12n§) && !§_-X1j§.§_-12n§.inBattle)
            {
               this.§_-72S§(new §_-Vr§());
               this.§_-h2M§.§_-b2q§();
               this.§_-B2d§.value = false;
               §_-N1V§.§_-42k§.updateSubscribe();
            }
            else
            {
               this.§_-B2d§.value = true;
            }
         }
      }
      
      public function §_-g2M§() : void
      {
         this.§_-A2I§.stop();
         this.§_-A2I§.removeEventListeners(starling.events.Event.COMPLETE);
         this.§_-A2I§.removeEventListeners(§_-C2c§.CHEAT);
      }
      
      public function §_-Q2S§() : void
      {
         var _loc1_:UserProfile = null;
         var _loc2_:§_-u1z§ = null;
         for each(_loc1_ in this.team)
         {
            if(_loc1_.teamMemberType == §_-yA§.§_-e2e§)
            {
               _loc2_ = §_-V2T§.§_-H2p§(_loc1_.id);
               if(_loc2_)
               {
                  _loc1_.§_-t0§(_loc2_.getName());
                  _loc1_.§_-I1N§(_loc2_.getName());
               }
            }
         }
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      public function §_-c7§(param1:SocialType) : void
      {
         this.§_-Dw§[param1] = null;
      }
      
      public function §_-s1I§(param1:§_-B2i§) : void
      {
         var _loc2_:§_-Oi§ = null;
         var _loc3_:int = 0;
         if(this.§_-53P§.raceRecord != param1)
         {
            for each(_loc2_ in this.wardrobe.§_-O2Z§())
            {
               _loc3_ = _loc2_.getRecord().getId();
               if(§_-V2w§.§_-q1F§(_loc3_))
               {
                  this.wardrobe.replaceItem(_loc2_.getId(),§_-G1p§.§_-12J§(_loc3_,param1));
               }
               else if(§_-V2w§.§_-Q2a§(_loc3_))
               {
                  this.wardrobe.replaceItem(_loc2_.getId(),§_-G1p§.§_-93G§(_loc3_));
               }
            }
            this.§_-53P§.§_-2S§(param1);
            if(this.races.indexOf(param1) == -1)
            {
               this.races.push(param1);
            }
         }
      }
      
      public function §_-L§(param1:§_-zF§) : void
      {
         if(this.§_-53P§.hat != param1)
         {
            this.§_-53P§.§_-Yd§(param1);
         }
      }
      
      public function §_-aV§(param1:§_-T1y§) : void
      {
         if(this.§_-53P§.artifact != param1)
         {
            this.§_-53P§.§_-W1V§(param1);
         }
      }
      
      public function §_-A37§() : Vector.<§_-B2i§>
      {
         return this.races;
      }
      
      public function getHat() : §_-zF§
      {
         return this.§_-53P§.hat;
      }
      
      public function §_-32q§() : §_-T1y§
      {
         return this.§_-53P§.artifact;
      }
      
      public function §_-n2k§(param1:int) : int
      {
         var _loc2_:TemporalStuffStructure = null;
         for each(_loc2_ in this.§_-2N§)
         {
            if(_loc2_.stuffId == param1)
            {
               return _loc2_.expireDate;
            }
         }
         return 0;
      }
      
      public function §_-ew§(param1:int) : Boolean
      {
         return this.§_-q2W§(param1) >= 0;
      }
      
      public function §_-l2e§(param1:§_-W1r§, param2:int) : void
      {
         var _loc3_:TemporalStuffStructure = new TemporalStuffStructure();
         _loc3_.stuffId = param1.getId();
         _loc3_.expireDate = §_-X1j§.serverTime + (getTimer() - §_-X1j§.§_-Q9§) / §_-72u§.§_-E1O§ + param2;
         var _loc4_:int = this.§_-q2W§(param1.getId());
         if(_loc4_ < 0)
         {
            this.§_-2N§.push(_loc3_);
         }
         else
         {
            this.§_-2N§[_loc4_].expireDate = _loc3_.expireDate;
         }
      }
      
      private function §_-q2W§(param1:int) : int
      {
         var _loc2_:* = int(this.§_-2N§.length);
         while(_loc2_--)
         {
            if(this.§_-2N§[_loc2_].stuffId == param1)
            {
               return _loc2_;
            }
         }
         return -1;
      }
      
      public function getSocialData() : SocialUserData
      {
         return this.§_-Dw§[§_-X1j§.socialController.getType()];
      }
      
      public function §_-b1b§() : Dictionary
      {
         return this.§_-Dw§;
      }
      
      public function §_-521§(param1:uint, param2:Dictionary) : void
      {
         this.id = param1;
         if(UserProfileStructure.getSocialIdsAsString(param2) != "")
         {
            this.socialIds = param2;
         }
      }
      
      public function getId() : int
      {
         return this.id;
      }
      
      public function setId(param1:uint) : void
      {
         this.id = param1;
      }
      
      public function §_-V1K§() : int
      {
         return this.platformId;
      }
      
      public function §_-bo§(param1:int) : void
      {
         this.platformId = param1;
      }
      
      public function get §_-d1C§() : Boolean
      {
         return this.platformId > 0 && this.platformId != §_-X1j§.§_-It§.getPlatformType().servercode;
      }
      
      public function getSocialIds() : Dictionary
      {
         return this.socialIds;
      }
      
      public function §_-620§(param1:Dictionary) : void
      {
         this.socialIds = param1;
      }
      
      public function §_-51e§() : uint
      {
         return Math.max(this.ingameMoney.value,0);
      }
      
      public function §_-12l§(param1:int, param2:Boolean = true) : void
      {
         this.ingameMoney.value = param1;
         if(param2)
         {
            this.§_-W2K§();
         }
      }
      
      public function §_-oA§(param1:int, param2:Boolean = true) : void
      {
         this.ingameMoney.value += param1;
         if(param2)
         {
            this.§_-W2K§();
         }
      }
      
      public function §_-o1N§() : uint
      {
         return Math.max(this.realMoney.value,0);
      }
      
      public function §_-Y2k§(param1:int, param2:Boolean = true) : void
      {
         this.realMoney.value = param1;
         if(param2)
         {
            this.§_-W2K§();
         }
      }
      
      public function §_-H3i§(param1:int, param2:Boolean = true) : void
      {
         this.realMoney.value += param1;
         if(param2)
         {
            this.§_-W2K§();
         }
      }
      
      public function §_-11§(param1:§_-W1r§, param2:int = -1, param3:Boolean = true) : void
      {
         this.§_-E3H§().§_-Um§(param1,param2);
         if(param3)
         {
            this.§_-W2K§();
         }
      }
      
      public function §_-W2K§() : void
      {
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      public function §_-Zp§() : uint
      {
         return this.rating.value;
      }
      
      public function §_-G2§(param1:uint) : void
      {
         this.rating.value = param1;
      }
      
      public function §_-E3L§(param1:int) : void
      {
         this.rating.value += param1;
      }
      
      public function §_-C2Z§() : uint
      {
         return this.dailyRating.value;
      }
      
      public function §_-q2E§() : int
      {
         return this.oldPlace.value;
      }
      
      public function §_-O1Y§(param1:§_-TA§) : void
      {
         this.oldPlace = param1;
      }
      
      public function §_-A1U§() : Array
      {
         return this.team;
      }
      
      public function §_-n26§() : Array
      {
         var _loc2_:UserProfile = null;
         var _loc1_:Array = [];
         for each(_loc2_ in this.team)
         {
            if(_loc2_.getId() == this.getId() && _loc2_.getId() != 0)
            {
               _loc2_.§_-Ox§(true);
            }
            if(_loc2_.§_-H38§() && !§_-71v§.§_-Hd§(_loc2_.teamMemberType))
            {
               _loc1_.push(_loc2_);
            }
         }
         return _loc1_;
      }
      
      public function §_-fR§(param1:Array) : void
      {
         this.team = param1;
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      public function §_-hz§() : uint
      {
         var _loc2_:UserProfile = null;
         var _loc1_:uint = 0;
         for each(_loc2_ in this.§_-n26§())
         {
            _loc1_ += _loc2_.getLevel();
         }
         return _loc1_;
      }
      
      public function getMaxLevel() : uint
      {
         var _loc2_:UserProfile = null;
         var _loc1_:uint = 0;
         for each(_loc2_ in this.team)
         {
            if(_loc1_ < _loc2_.getLevel())
            {
               _loc1_ = uint(_loc2_.getLevel());
            }
         }
         return _loc1_;
      }
      
      public function §_-N23§() : int
      {
         return this.§_-53P§.§_-g2I§.value;
      }
      
      public function §_-h2m§() : int
      {
         return this.§_-53P§.§_-Z2U§.value;
      }
      
      public function §_-q1H§(param1:int) : void
      {
         this.§_-53P§.§_-Z2U§.value = Math.max(0,param1);
      }
      
      public function §_-w1p§() : int
      {
         return this.§_-53P§.§_-A3Z§.value;
      }
      
      public function §_-eQ§(param1:int) : void
      {
         this.§_-53P§.§_-A3Z§.value = param1;
      }
      
      public function getLevel() : int
      {
         return this.level.value;
      }
      
      public function §_-y1E§() : int
      {
         return this.§_-v6§.value;
      }
      
      public function §_-t5§(param1:int) : void
      {
         this.experience.value = param1;
      }
      
      public function §_-K1U§(param1:int) : void
      {
         this.experience.value += param1;
      }
      
      public function §_-p25§() : int
      {
         return this.experience.value;
      }
      
      public function §_-q28§(param1:int) : void
      {
         this.§_-53P§.§_-g2I§.value = param1;
      }
      
      public function §_-u2l§() : uint
      {
         return this.reactionRate.value;
      }
      
      public function §_-j1u§(param1:uint) : void
      {
         this.reactionRate.value = param1;
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      public function §_-P2P§(param1:uint) : void
      {
         this.§_-j1u§(this.reactionRate.value + param1);
      }
      
      public function getOnline() : Boolean
      {
         return this.online;
      }
      
      public function §_-x2F§(param1:Boolean) : void
      {
         this.online = param1;
      }
      
      public function get stats() : §_-m1x§
      {
         return this.§_-53P§;
      }
      
      public function §_-N1n§(param1:int) : void
      {
         this.level.value = param1;
      }
      
      public function getCharName() : String
      {
         return §_-33r§.§_-KQ§(this);
      }
      
      public function §_-K3u§() : String
      {
         return §_-33r§.§_-c2x§(this);
      }
      
      public function §_-I1N§(param1:String) : void
      {
         this.§_-i2W§ = param1;
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      public function §_-62T§(param1:int) : void
      {
         this.teamMemberType = param1;
      }
      
      public function §_-t2I§() : int
      {
         return this.teamMemberType;
      }
      
      public function §_-P2e§() : §_-B2i§
      {
         return this.§_-53P§.raceRecord;
      }
      
      public function §_-s1o§(param1:§_-B2i§) : void
      {
         var _loc2_:int = 0;
         var _loc3_:§_-Oi§ = null;
         if(!param1)
         {
            return;
         }
         this.§_-53P§.§_-2S§(param1);
         if(this.§_-53P§.raceRecord != param1)
         {
            for each(_loc3_ in this.wardrobe.§_-O2Z§())
            {
               _loc2_ = _loc3_.getRecord().getId();
               if(§_-V2w§.§_-q1F§(_loc2_))
               {
                  this.wardrobe.replaceItem(_loc3_.getId(),§_-G1p§.§_-12J§(_loc2_,param1));
               }
               else if(§_-V2w§.§_-Q2a§(_loc2_))
               {
                  this.wardrobe.replaceItem(_loc3_.getId(),§_-G1p§.§_-93G§(_loc2_));
               }
            }
         }
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      public function §_-t0§(param1:String) : void
      {
         this.charName = param1;
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      public function §_-v2z§(param1:§_-B2i§) : String
      {
         return param1.graveType;
      }
      
      public function §_-uV§(param1:§_-B2i§) : String
      {
         return param1.soulType;
      }
      
      public function §_-pd§() : §_-2m§
      {
         return this.§_-31d§;
      }
      
      public function §_-E39§() : §_-Vr§
      {
         return this.§_-R1X§;
      }
      
      public function §_-NG§(param1:§_-2m§) : void
      {
         this.§_-31d§ = param1;
      }
      
      public function §_-72S§(param1:§_-Vr§) : void
      {
         this.§_-R1X§ = param1;
      }
      
      public function §_-V2J§() : §_-F3H§
      {
         return this.§_-V2K§;
      }
      
      public function §_-Y20§(param1:§_-F3H§) : void
      {
         this.§_-V2K§ = param1;
      }
      
      public function §_-h1v§() : §_-H2M§
      {
         return this.§_-Q28§;
      }
      
      public function §_-p2c§() : §_-52D§
      {
         return this.§_-h2M§;
      }
      
      public function §_-u1K§(param1:UserProfile) : Boolean
      {
         var _loc2_:UserProfile = null;
         for each(_loc2_ in this.team)
         {
            if(_loc2_.getId() == param1.getId())
            {
               return true;
            }
         }
         return false;
      }
      
      public function §_-S2A§() : Boolean
      {
         return this.team.length < §_-lp§.§_-H§(this.getLevel());
      }
      
      public function §_-dE§(param1:§_-x2t§, param2:§_-E19§) : void
      {
         switch(param2)
         {
            case §_-E19§.§_-51D§:
               this.§_-H3i§(-param1.realPrice);
               break;
            case §_-E19§.§_-i2d§:
               this.§_-oA§(-param1.§_-R1T§);
         }
      }
      
      public function §_-m1O§(param1:int, param2:§_-E19§) : void
      {
         switch(param2)
         {
            case §_-E19§.§_-51D§:
               this.§_-H3i§(param1);
               break;
            case §_-E19§.§_-i2d§:
               this.§_-oA§(param1);
         }
      }
      
      public function §_-e2A§() : uint
      {
         var _loc3_:UserProfile = null;
         var _loc1_:uint = 0;
         var _loc2_:Array = this.§_-n26§();
         for each(_loc3_ in _loc2_)
         {
            _loc1_ += _loc3_.§_-53P§.§_-g2I§.value + _loc3_.§_-53P§.§_-q1G§.value;
         }
         return _loc1_;
      }
      
      public function equals(param1:UserProfile) : Boolean
      {
         return param1 != null && this.id == param1.id;
      }
      
      public function get build() : §_-w1H§
      {
         return this.builder;
      }
      
      public function §_-XT§() : §_-by§
      {
         return this.wardrobe;
      }
      
      public function §_-E3H§() : §_-H2M§
      {
         return this.§_-91o§;
      }
      
      public function §_-zC§() : §_-11E§
      {
         return this.§_-h2G§;
      }
      
      public function getSocialId() : String
      {
         return this.socialIds[§_-X1j§.§_-It§.getPlatformType()];
      }
      
      public function §_-61H§() : String
      {
         var _loc2_:Object = null;
         var _loc1_:String = this.getSocialId();
         if(_loc1_ != null)
         {
            return _loc1_;
         }
         for(_loc2_ in this.socialIds)
         {
            if(_loc2_ != SocialType.FACEBOOK)
            {
               _loc1_ = this.socialIds[_loc2_];
               if(_loc1_ != null)
               {
                  return _loc1_;
               }
            }
         }
         return null;
      }
      
      public function §_-F1b§(param1:int = -1) : UserProfile
      {
         var _loc2_:UserProfile = UserProfile.custom(this.getCharName(),this.getLevel(),§_-L1x§.§_-V10§(this.§_-53P§.raceRecord),this.§_-53P§.§_-g2I§.value,this.§_-53P§.§_-Z2U§.value,this.§_-53P§.§_-A3Z§.value);
         if(param1 != -1)
         {
            _loc2_.§_-62T§(param1);
         }
         _loc2_.§_-Ox§(this.§_-t2J§);
         _loc2_.§_-L§(this.getHat());
         _loc2_.§_-aV§(this.§_-32q§());
         return _loc2_;
      }
      
      public function §_-517§(param1:int) : UserProfile
      {
         var _loc2_:UserProfile = this.§_-F1b§(param1);
         _loc2_.§_-521§(this.id,this.socialIds);
         return _loc2_;
      }
      
      public function §_-Ox§(param1:Boolean) : void
      {
         this.§_-t2J§ = param1;
      }
      
      public function §_-H38§() : Boolean
      {
         return this.§_-t2J§;
      }
      
      public function §_-33n§() : Boolean
      {
         return this.§_-u1a§() > 0 || Boolean(this.§_-B2d§) && this.§_-B2d§.value;
      }
      
      public function §_-u1a§() : Number
      {
         return (this.§_-U2G§ - §_-X1j§.serverTime) * §_-72u§.§_-E1O§ - (getTimer() - §_-X1j§.§_-Q9§);
      }
      
      public function §_-pk§() : §_-C2c§
      {
         return this.§_-A2I§;
      }
      
      public function §_-ia§() : Boolean
      {
         return this.§_-23h§ ? this.§_-23h§.clanId != 0 : false;
      }
      
      public function get clanMember() : ClanMemberStructure
      {
         return this.§_-23h§;
      }
   }
}

