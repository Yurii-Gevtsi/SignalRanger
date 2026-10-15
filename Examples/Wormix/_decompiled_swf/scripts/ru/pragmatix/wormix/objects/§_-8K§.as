package ru.pragmatix.wormix.objects
{
   import §_-12a§.§_-6p§;
   import §_-21p§.§_-4w§;
   import §_-23P§.§_-u1z§;
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-52L§.§_-u2x§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.*;
   import §_-73I§.§_-K2s§;
   import §_-73I§.§_-Z2e§;
   import §_-73d§.§_-V19§;
   import §_-73d§.§_-v1o§;
   import §_-79§.§_-ps§;
   import §_-82l§.§_-A2Y§;
   import §_-91A§.§_-N2O§;
   import §_-91A§.§_-sH§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-Z1Y§;
   import §_-CR§.§_-72p§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-F39§.*;
   import §_-G2H§.*;
   import §_-GQ§.§_-31v§;
   import §_-J3L§.§_-K2i§;
   import §_-L1H§.§_-C3e§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-P1B§.§_-EB§;
   import §_-P1B§.§_-F1q§;
   import §_-P1B§.§_-b21§;
   import §_-P2i§.§_-G3J§;
   import §_-P2i§.§_-b7§;
   import §_-TF§.§_-y2n§;
   import §_-Ta§.*;
   import §_-Tm§.§_-K1I§;
   import §_-U1i§.§_-I3t§;
   import §_-Vg§.*;
   import §_-Vu§.MD5;
   import §_-Y1O§.*;
   import §_-Y1b§.*;
   import §_-aM§.§_-P1N§;
   import §_-b1F§.§_-v2Q§;
   import §_-bI§.*;
   import §_-d26§.§_-c2E§;
   import §_-d26§.§_-d1H§;
   import §_-g2C§.GlobalChatTextMessageItemRenderer;
   import §_-g2C§.§_-62U§;
   import §_-g2C§.§_-I1R§;
   import §_-g2C§.§_-K2A§;
   import §_-g2C§.§_-K2u§;
   import §_-g2C§.§_-p8§;
   import §_-g2C§.§_-w1K§;
   import §_-g2e§.§_-q2Z§;
   import §_-gG§.§_-8u§;
   import §_-hO§.SupplyPickedUpEvent;
   import §_-hO§.§_-BY§;
   import §_-j2R§.§_-v1T§;
   import §_-jF§.ClanInviteRenderer;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-d2j§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-m0§.§_-X1F§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-sQ§.§_-H3D§;
   import §_-u1P§.ShopResultEvent;
   import §_-u2J§.§_-T1K§;
   import §_-w2W§.§_-21w§;
   import §_-w2i§.§_-Ia§;
   import §_-wD§.*;
   import §_-x22§.§_-xY§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-522§;
   import §_-zI§.§_-X1q§;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.greensock.TweenMax;
   import com.hurlant.crypto.tls.§_-21v§;
   import feathers.controls.Button;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.FontColor;
   import feathers.themes.styles.AlternateButtonsStyles;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.utils.textures.TextureCache;
   import flash.desktop.NativeApplication;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-41c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.fx.trail.ParticleTrail;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.rules.§_-Q2u§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.clans.chat.*;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.IncomingPayment;
   import ru.pragmatix.wormix.messages.server.SteamTransactionInitResponse;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.utils.Result;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.bonus.§_-z17§;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Antimagnet;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-8K§ extends §_-F3o§ implements §_-sH§, §_-N2O§, §_-01J§
   {
      
      public static const LEFT:int = -1;
      
      public static const RIGHT:int = 1;
      
      private static const §_-Kv§:uint = 30;
      
      private var _weapon:Weapon = null;
      
      private var §_-83d§:Weapon = null;
      
      private var §_-m2x§:String = null;
      
      private var §_-bA§:§_-V23§;
      
      private var §_-si§:UserProfile;
      
      private var §_-wm§:Number = 0;
      
      private var §_-23F§:§_-7u§ = new §_-7u§(false);
      
      private var §_-811§:Boolean = false;
      
      private var _direction:int = -1;
      
      private var §_-K1r§:Boolean = false;
      
      private var §_-63r§:§_-Z2e§;
      
      private var §_-La§:DisplayObject;
      
      private var trail:ParticleTrail;
      
      private var §_-uq§:§_-v1T§;
      
      private var §_-k3§:§_-z17§;
      
      private var hat:§_-zF§;
      
      private var artifact:§_-T1y§;
      
      private var §_-l1Z§:Boolean = this.§_-l1Z§;
      
      private var §_-B1z§:Boolean = false;
      
      private var §_-K§:Boolean = false;
      
      private var §_-M17§:Boolean = true;
      
      private var §_-L4§:Boolean = true;
      
      private var §_-ic§:Boolean = false;
      
      private var §_-w28§:§_-A1l§ = null;
      
      private var §_-t9§:int = -1;
      
      private var §_-31h§:int = 1;
      
      private var §_-020§:Boolean = false;
      
      private var §_-c2A§:UnitPhysicModel;
      
      private var §_-t2G§:RopePhysicModel;
      
      private var §_-03e§:Boolean = true;
      
      private var §_-b2V§:Boolean = false;
      
      private var §_-C2K§:Number = 0;
      
      private var §_-e2S§:Number = 0;
      
      private var _isActive:Boolean = false;
      
      private var §_-Pm§:§_-I3t§ = new §_-I3t§();
      
      private var §_-4q§:§_-G3J§;
      
      public function §_-8K§(param1:UserProfile, param2:int, param3:int, param4:Boolean = false)
      {
         this.§_-si§ = param1;
         super(param4);
         this.§_-w28§ = param1.aiArsenal;
         this.§_-63r§ = new §_-Z2e§(param1.§_-P2e§());
         this.§_-63r§.§_-N1h§(false);
         getPhysicModel().x = param2;
         getPhysicModel().y = param3;
         §_-z1H§ = this.§_-63r§.getContainer();
         this.§_-83d§ = §_-73v§.§_-z1F§(param1);
         §_-33R§ = new §_-EB§(this,param1.stats.§_-g2I§.value,param1.stats.§_-A3Z§.value,param1.stats.§_-Z2U§.value);
         §_-33R§.§_-kN§ = new §_-F1q§(this);
         §_-33R§.jump = new §_-q2Z§(this);
         this.§_-uq§ = new §_-v1T§(this);
         this.§_-k3§ = new §_-z17§(this);
         this.§_-k3§.§_-U1Z§(param1.§_-P2e§().§_-V1v§());
         this.§_-L§(param1.getHat());
         this.§_-aV§(param1.§_-32q§());
         §_-Nt§ = true;
         tags = new <§_-D3B§>[§_-D3B§.§_-J1i§];
         §_-TZ§.addListener(this,§_-L3Q§.§_-Qw§,this.defaultOnDamaged);
         if(!param4)
         {
            §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         }
         §_-92S§ = true;
         this.§_-La§ = this.§_-Y1r§();
         this.§_-4q§ = new §_-G3J§();
      }
      
      private function §_-Y1r§() : DisplayObject
      {
         var _loc1_:Quad = new Quad(Physics.UNIT_WIDTH,Physics.UNIT_HEIGHT);
         _loc1_.x = -_loc1_.width * 0.5;
         _loc1_.y = -_loc1_.height * 0.5;
         _loc1_.alpha = 0;
         this.§_-63r§.getContainer().addChild(_loc1_);
         return _loc1_;
      }
      
      public function §_-o1z§(param1:Boolean = true) : void
      {
         this.§_-l1Z§ = true;
         if(param1)
         {
            §_-MN§.§_-D9§(this,ZombieGrave.§_-d19§);
         }
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.UNIT);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         physParams.optimizedMovement = false;
         physParams.sinkable = true;
         this.§_-t2G§ = new RopePhysicModel(physParams);
         this.§_-t2G§.addEventListener(§_-L3Q§.§_-kl§,this.defaultOnSink);
         this.§_-c2A§ = new UnitPhysicModel(physParams);
         this.§_-c2A§.addEventListener(§_-42G§.§_-K2J§,this.onStable);
         this.§_-c2A§.addEventListener(§_-42G§.§_-o1a§,this.onUnstable);
         this.§_-c2A§.addEventListener(§_-42G§.§_-Lz§,this.onFalling);
         §_-Y2I§ = this.§_-c2A§;
      }
      
      public function §_-gU§(param1:IPhysicModel) : void
      {
         var _loc2_:IPhysicModel = §_-Y2I§;
         §_-Y2I§ = param1;
         §_-Y2I§.x = _loc2_.x;
         §_-Y2I§.y = _loc2_.y;
      }
      
      public function §_-dz§() : void
      {
         var _loc1_:uint = 0;
         var _loc2_:Point = null;
         this.§_-gU§(this.§_-c2A§);
         §_-Vw§(0,0);
         if(HitTest.unit(x,y))
         {
            _loc1_ = 30;
            _loc2_ = Pool.getPoint();
            if(Physics.nearestFreePoint(x,y,HitTest.unit,_loc1_,_loc2_))
            {
               x = _loc2_.x;
               y = _loc2_.y;
            }
            Pool.putPoint(_loc2_);
         }
      }
      
      public function §_-41F§(param1:Point, param2:Number) : void
      {
         this.§_-gU§(this.§_-t2G§);
         this.§_-t2G§.reset(param1);
         this.§_-t2G§.setLength(param2);
      }
      
      protected function onStable(param1:starling.events.Event) : void
      {
         var _loc2_:UnitPhysicModel = UnitPhysicModel(getPhysicModel());
         _loc2_.stop();
         this.§_-l2A§();
         §_-q1Q§();
         this.§_-K1r§ = false;
         _loc2_.rollable = false;
         this.§_-K1O§();
         if(this.trail != null)
         {
            this.§_-B1C§();
         }
         dispatchEventWith(§_-L3Q§.§_-a1V§);
      }
      
      public function onUnstable(param1:starling.events.Event) : void
      {
         if(this.crawling != 0)
         {
            this.§_-01D§();
         }
         getPhysicModel().stop();
         this.§_-f2T§();
         dispatchEventWith(§_-L3Q§.§_-S2B§);
      }
      
      public function onFalling(param1:starling.events.Event) : void
      {
         this.§_-01D§();
         getPhysicModel().addSpeedXY(this._direction == LEFT ? -1 : 1,0);
         addEventListener(§_-L3Q§.§_-a1V§,attributes.jump.stabilize);
      }
      
      protected function §_-K1O§() : void
      {
         this.§_-63r§.§_-x29§();
         if(Boolean(this._weapon) && !this._weapon.§_-h1N§())
         {
            if(!§_-X1j§.§_-12n§.gameMaster.§_-Rk§())
            {
               this._weapon.activate();
            }
            else
            {
               this._weapon.rotateWeaponClip(this.direction == §_-8K§.LEFT ? Math.PI : 0);
            }
         }
      }
      
      public function §_-f2T§() : void
      {
         this.§_-63r§.§_-62C§();
         if(Boolean(this._weapon) && this._weapon.isActive())
         {
            this._weapon.deactivate();
         }
      }
      
      public function §_-K1A§(param1:§_-i14§) : void
      {
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§() && param1.§_-l2Z§().squad.getTeamId() != this.§_-bA§.getTeamId())
         {
            §_-X1j§.§_-12n§.battleStats.§_-L1k§(param1.getId());
         }
         this.§_-G15§(this._direction * -3,-6);
         this.§_-r1V§(param1.hatId);
         param1.disposed = true;
      }
      
      public function §_-r1V§(param1:int) : void
      {
         var _loc2_:§_-zF§ = §_-G1p§.§_-12J§(param1,this.§_-si§.§_-P2e§());
         this.§_-L§(_loc2_);
      }
      
      public function §_-L§(param1:§_-zF§) : void
      {
         var _loc2_:Boolean = §_-71v§.§_-Q19§(this.§_-si§.§_-t2I§());
         if(this.hat != null)
         {
            this.§_-k3§.§_-Y2t§(this.hat.§_-V1v§(_loc2_));
            this.hat = null;
         }
         if(param1 != null)
         {
            this.hat = param1;
            this.§_-k3§.§_-U1Z§(param1.§_-V1v§(_loc2_));
         }
         this.§_-63r§.§_-L§(param1);
      }
      
      public function §_-P1t§(param1:int) : void
      {
         var _loc2_:§_-T1y§ = §_-G1p§.§_-93G§(param1);
         this.§_-aV§(_loc2_);
      }
      
      public function §_-aV§(param1:§_-T1y§) : void
      {
         var _loc2_:Boolean = §_-71v§.§_-Q19§(this.§_-si§.§_-t2I§());
         if(this.artifact != null)
         {
            this.§_-k3§.§_-Y2t§(this.artifact.§_-V1v§(_loc2_));
            this.artifact = null;
         }
         if(param1 != null)
         {
            this.artifact = param1;
            this.§_-k3§.§_-U1Z§(param1.§_-V1v§(_loc2_));
         }
         this.§_-63r§.§_-aV§(param1);
      }
      
      public function §_-G15§(param1:int, param2:int) : void
      {
         var _loc3_:int = 0;
         var _loc4_:§_-i14§ = null;
         if(Boolean(this.hat) && this.§_-M17§)
         {
            _loc3_ = HitTest.unit(x,y - Physics.UNIT_HALF_HEIGHT) ? y : int(y - Physics.UNIT_HALF_HEIGHT);
            _loc4_ = new §_-i14§(this.hat.§_-G3a§(),x,_loc3_,this,this.hat);
            _loc4_.setSpeedXY(param1,param2);
            this.§_-L§(null);
         }
      }
      
      override public function nextState() : void
      {
         var _loc2_:Number = NaN;
         var _loc3_:Number = NaN;
         super.nextState();
         var _loc1_:Number = Number.NaN;
         if(this.§_-K1r§)
         {
            if(§_-Y2I§ is UnitPhysicModel && this.rolling != 0)
            {
               _loc1_ = 0;
            }
            else
            {
               _loc2_ = §_-Y2I§.speed;
               _loc3_ = §_-Y2I§.speedX;
               if(Boolean(_loc2_) && Boolean(_loc3_))
               {
                  if(_loc2_ < 10 || Math.abs(_loc3_) < 1)
                  {
                     _loc1_ = §_-Y2I§.angle + MathUtil.PI_HALF;
                  }
                  else
                  {
                     _loc1_ = this.§_-63r§.getContainer().rotation + §_-H3D§.sign(_loc3_) * _loc2_;
                  }
               }
            }
         }
         if(§_-Y2I§.stable && this.crawling != 0)
         {
            this.§_-y2K§();
         }
         if(!isNaN(_loc1_))
         {
            this.§_-63r§.getContainer().rotation = _loc1_;
         }
         this.§_-63r§.§_-u1s§();
      }
      
      public function §_-ab§(param1:Number) : void
      {
         this.§_-63r§.getContainer().rotation = param1;
         this.§_-63r§.§_-u1s§();
      }
      
      public function activate() : void
      {
         this._isActive = true;
      }
      
      public function §_-F27§(param1:Boolean) : void
      {
         this.§_-K1r§ = param1;
      }
      
      public function deactivate() : void
      {
         this._isActive = false;
         this.§_-01D§();
         this.§_-Ck§();
      }
      
      public function §_-f1r§() : Boolean
      {
         return !disposed;
      }
      
      public function §_-l2A§(param1:Boolean = false) : void
      {
         this.§_-ab§(0);
         var _loc2_:Boolean = this.§_-uq§.hasBuff(§_-8u§.NAME) || this.hp < §_-Kv§;
         this.§_-6v§(_loc2_,param1);
      }
      
      protected function §_-6v§(param1:Boolean, param2:Boolean = false) : void
      {
         if(this.§_-m2x§ != null)
         {
            this.§_-63r§[this.§_-m2x§]();
         }
         else
         {
            this.§_-63r§.§_-N1h§(param1,param2);
         }
      }
      
      protected function §_-y2K§() : void
      {
         this.§_-63r§.go();
      }
      
      public function §_-oK§(param1:DisplayObject, param2:int = 0, param3:int = 0, param4:Boolean = true) : void
      {
         var _loc5_:IPhysicModel = getPhysicModel();
         if(!disposed)
         {
            this.§_-63r§.§_-oK§(§_-K2s§.§_-J1o§,param1,param2,param3,_loc5_.stable,param4);
         }
      }
      
      public function §_-d2T§(param1:DisplayObject) : void
      {
         this.§_-63r§.§_-d2T§(§_-K2s§.§_-J1o§,param1);
      }
      
      public function §_-U8§(param1:DisplayObject, param2:int = 0, param3:int = 0, param4:Boolean = true, param5:Boolean = false, param6:Boolean = true) : void
      {
         if(!disposed)
         {
            if(param5)
            {
               this.§_-63r§.§_-O1i§(§_-K2s§.§_-r2F§,param1,param2,param3,true,param4);
            }
            else
            {
               this.§_-63r§.§_-oK§(§_-K2s§.§_-r2F§,param1,param2,param3,true,param4);
            }
         }
      }
      
      public function §_-n28§(param1:DisplayObject) : void
      {
         this.§_-63r§.§_-d2T§(§_-K2s§.§_-r2F§,param1);
      }
      
      public function get crawling() : int
      {
         var _loc1_:IPhysicModel = getPhysicModel();
         return _loc1_ is UnitPhysicModel ? UnitPhysicModel(_loc1_).crawling : 0;
      }
      
      public function set crawling(param1:int) : void
      {
         UnitPhysicModel(getPhysicModel()).crawling = param1;
      }
      
      public function get rolling() : int
      {
         return UnitPhysicModel(§_-Y2I§).rolling;
      }
      
      public function set rolling(param1:int) : void
      {
         UnitPhysicModel(§_-Y2I§).rolling = param1;
      }
      
      public function get keepUnstable() : Boolean
      {
         return getPhysicModel().keepUnstable;
      }
      
      public function set keepUnstable(param1:Boolean) : void
      {
         getPhysicModel().keepUnstable = param1;
      }
      
      override protected function defaultOnSink(param1:starling.events.Event) : void
      {
         this.§_-B1C§();
         super.defaultOnSink(param1);
      }
      
      override public function defaultOnDamaged(param1:starling.events.Event, param2:§_-v2Q§) : void
      {
         if(this.§_-bA§ == null)
         {
            return;
         }
         super.defaultOnDamaged(param1,param2);
         var _loc3_:IPhysicModel = getPhysicModel();
         if(_loc3_ is UnitPhysicModel)
         {
            if(param2 ? Boolean(param2.power) : attributes.hp.§_-oJ§() > 0)
            {
               this.attachTrail();
            }
            if(this.crawling)
            {
               this.§_-01D§();
            }
            if(this.rolling != 0)
            {
               this.rolling = 0;
            }
            if(_loc3_.speedY > -2 + Physics.EPSILON)
            {
               _loc3_.setSpeedXY(_loc3_.speedX,-2);
            }
            if(Math.abs(_loc3_.speedX) < 0.2 + Physics.EPSILON)
            {
               _loc3_.setSpeedXY(§_-H3D§.sign(_loc3_.speedX) * 0.2,_loc3_.speedY);
            }
            if(!this.§_-K1r§)
            {
               this.§_-ab§(0);
               this.§_-63r§.damaged();
               if(Math.abs(_loc3_.speed) > 3)
               {
                  this.rotateUntilStable();
               }
            }
         }
         §_-ps§.§_-wC§(this);
      }
      
      public function §_-f15§() : void
      {
         this.§_-B1z§ = true;
         this.§_-K§ = true;
      }
      
      public function §_-n2t§() : void
      {
         this.§_-K§ = true;
      }
      
      public function §_-q2T§() : void
      {
         this.§_-B1z§ = true;
      }
      
      override public function defaultOnDisposed(param1:starling.events.Event = null) : void
      {
         var _loc3_:§_-A16§ = null;
         var _loc4_:IPhysicModel = null;
         if(this.trail)
         {
            this.§_-B1C§();
            this.trail.removeFromParent(true);
         }
         if(attributes.hp.value > 0)
         {
            attributes.hp.change(-attributes.hp.value);
         }
         if(this._weapon != null)
         {
            this._weapon.detach();
         }
         this.§_-uq§.clearAll();
         this.§_-63r§.§_-62C§();
         this.§_-63r§.dispose();
         var _loc2_:IPhysicModel = getPhysicModel();
         if(!§_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-C6§())
         {
            §_-X1j§.§_-12n§.battleStats.§_-7m§(this);
            if(!this.§_-K§)
            {
               if(!this.§_-l1Z§)
               {
                  _loc3_ = new §_-A16§(this.§_-si§.§_-P2e§().graveType,_loc2_.x,_loc2_.y,this,0.5);
                  _loc4_ = _loc3_.getPhysicModel();
                  _loc4_.setSpeedXY(_loc2_.speedX,_loc2_.speedY);
               }
               else
               {
                  Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,_loc2_.x,_loc2_.y,50,§_-d27§.§_-S3§);
               }
               this.§_-ak§(this.§_-si§,_loc2_.x,_loc2_.y);
            }
         }
         if(§_-X1j§.§_-12n§.gameMaster)
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-b2W§(this).length == 0 && !this.§_-020§)
            {
               if(this._weapon != null)
               {
                  this._weapon.detach();
               }
               §_-X1j§.§_-12n§.gameMaster.nextTurnOnStable();
            }
            if(!this.§_-B1z§)
            {
               this.§_-G15§(-_loc2_.speedX,1.5 * _loc2_.speedY);
            }
         }
         §_-TZ§.removeListener(this,§_-L3Q§.§_-Qw§,this.defaultOnDamaged);
         this.§_-La§ = null;
         super.defaultOnDisposed();
      }
      
      public function §_-r8§() : void
      {
         this.§_-020§ = disposed = true;
      }
      
      public function §_-Sk§(param1:DamageType) : void
      {
         §_-KX§.damageType = param1;
      }
      
      public function §_-ak§(param1:UserProfile, param2:int, param3:int) : void
      {
         if(Boolean(!this.§_-020§ && !§_-91i§) && Boolean(§_-KX§) && §_-KX§.damageType == DamageType.FIRE)
         {
            Effects.createSoul("Burning",param2,param3);
         }
         else
         {
            Effects.createSoul("soul_full",param2,param3);
         }
      }
      
      public function turnRight() : void
      {
         this.turn(RIGHT);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-WM§(false);
      }
      
      public function turnLeft() : void
      {
         this.turn(LEFT);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-WM§(true);
      }
      
      public function §_-o1k§(param1:Boolean = false) : void
      {
         this.§_-rN§(RIGHT,param1);
      }
      
      public function §_-u2K§(param1:Boolean = false) : void
      {
         this.§_-rN§(LEFT,param1);
      }
      
      public function turn(param1:int) : void
      {
         if(this._direction != param1)
         {
            this.§_-ab§(0);
            if(param1 == LEFT)
            {
               this.§_-63r§.turnLeft();
            }
            else
            {
               this.§_-63r§.turnRight();
            }
            this._direction = param1;
            this.§_-63r§.§_-62C§();
            this.§_-63r§.§_-x29§();
         }
      }
      
      public function §_-rN§(param1:int, param2:Boolean = false) : void
      {
         var _loc3_:IPhysicModel = getPhysicModel();
         if(!_loc3_)
         {
            return;
         }
         if(Boolean(_loc3_ && _loc3_.stable) && Boolean(!this.crawling) && !this.§_-23F§.value)
         {
            this.§_-63r§.go();
         }
         if(_loc3_.stable && (param1 == LEFT && this.crawling >= 0 || param1 == RIGHT && this.crawling <= 0))
         {
            this.turn(param1);
            this.§_-f2T§();
            if(!this.§_-23F§.value)
            {
               this.crawling = §_-H3D§.sign(param1) * (param2 ? §_-F1q§.§_-l19§.value : attributes.§_-kN§.value);
               if(this.§_-63r§)
               {
                  this.§_-63r§.§_-62C§();
               }
            }
            else
            {
               this.§_-63r§.§_-x29§();
               this.§_-l2A§();
               if(this._weapon)
               {
                  this._weapon.activate();
               }
            }
         }
      }
      
      public function §_-Ck§(param1:Boolean = false) : void
      {
         if(this._weapon)
         {
            if(this._weapon is Jetpack)
            {
               if(param1 || !Jetpack(this._weapon).isUsed())
               {
                  this._weapon.detach();
               }
            }
            else
            {
               this._weapon.detach();
            }
         }
      }
      
      public function §_-01D§() : void
      {
         if(!disposed && this.crawling != 0)
         {
            this.crawling = 0;
            this.§_-l2A§();
            this.§_-K1O§();
         }
      }
      
      public function rotateUntilStable(param1:Boolean = true) : void
      {
         this.§_-K1r§ = true;
         var _loc2_:UnitPhysicModel = UnitPhysicModel(getPhysicModel());
         _loc2_.rollable = param1;
         addEventListener(§_-L3Q§.§_-a1V§,this.§_-22t§);
      }
      
      public function §_-H2R§() : void
      {
         this.§_-ab§(0);
         this.§_-K1r§ = false;
      }
      
      public function §_-22t§(param1:starling.events.Event) : void
      {
         removeEventListener(§_-L3Q§.§_-a1V§,this.§_-22t§);
         this.§_-K1r§ = false;
         UnitPhysicModel(getPhysicModel()).rollable = false;
         this.§_-l2A§();
      }
      
      public function §_-P1W§(param1:uint) : void
      {
         var _loc2_:IPhysicModel = getPhysicModel();
         var _loc3_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc2_) && Boolean(!disposed) && _loc3_.§_-42C§())
         {
            if(this._weapon != null)
            {
               this._weapon.detach();
            }
            this._weapon = §_-73v§.getWeapon(param1);
            this._weapon.attach(this);
            if(!_loc2_.stable)
            {
               this.§_-63r§.§_-m14§(true);
               this.§_-63r§.§_-i§();
            }
         }
      }
      
      public function reattachWeapon() : void
      {
         if(§_-X1j§.§_-12n§.isLearning)
         {
            return;
         }
         if(this.§_-83d§ == null)
         {
            return;
         }
         if(this.§_-83d§.§_-h1N§())
         {
            return;
         }
         var _loc1_:uint = uint(Weapon(this.§_-83d§).recordId.value);
         if(§_-X1j§.§_-12n§.gameMaster.§_-91D§(_loc1_))
         {
            §_-Q2v§.§_-kd§(_loc1_);
            this.§_-P1W§(_loc1_);
         }
         else
         {
            this.§_-83d§ = null;
            §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
            §_-b7§.§_-13Z§(this);
         }
      }
      
      public function attachTrail() : void
      {
         if(Boolean(this.trail) && !this.trail.isEmitting)
         {
            this.trail.removeFromParent(true);
            this.trail = null;
         }
         if(this.trail == null)
         {
            this.trail = Effects.attachParticleTrail(ParticleEffectType.AMMO_TRAIL,this);
         }
      }
      
      public function §_-B1C§() : void
      {
         if(this.trail != null)
         {
            this.trail.stop();
         }
      }
      
      public function get hp() : int
      {
         return attributes.hp.value;
      }
      
      public function §_-OQ§() : Boolean
      {
         return this.§_-l1Z§;
      }
      
      public function get bonuses() : §_-z17§
      {
         return this.§_-k3§;
      }
      
      public function get buffs() : §_-v1T§
      {
         return this.§_-uq§;
      }
      
      public function §_-z1Q§() : void
      {
         dispatchEventWith(§_-T1K§.UNIT_INFO_VISIBILITY_CHANGED,false,false);
      }
      
      public function §_-M2z§() : void
      {
         dispatchEventWith(§_-T1K§.UNIT_INFO_VISIBILITY_CHANGED,false,true);
      }
      
      public function getNameHintInfoPanel() : String
      {
         return this.§_-si§.getCharName();
      }
      
      public function getSquadNumber() : uint
      {
         return this.§_-bA§.getNumber();
      }
      
      override public function get height() : int
      {
         return Physics.UNIT_HEIGHT;
      }
      
      override public function get width() : int
      {
         return Physics.UNIT_WIDTH;
      }
      
      public function isImmobile() : Boolean
      {
         return this.§_-23F§.value;
      }
      
      public function §_-e14§(param1:Boolean) : void
      {
         if(param1)
         {
            this.§_-01D§();
         }
         this.§_-23F§.value = param1;
      }
      
      public function getHat() : §_-zF§
      {
         return this.hat;
      }
      
      public function §_-32q§() : §_-T1y§
      {
         return this.artifact;
      }
      
      public function get §_-vp§() : Boolean
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         var _loc2_:Boolean = this.§_-03e§ && this.§_-b2V§ && !_loc1_.§_-v1V§() && !_loc1_.§_-Rk§();
         var _loc3_:§_-Q2v§ = §_-X1j§.§_-12n§.scene.gameInterface;
         return _loc2_ && !_loc3_.§_-rD§();
      }
      
      public function set §_-vp§(param1:Boolean) : void
      {
         this.§_-03e§ = param1;
         this.§_-K13§();
      }
      
      public function get §_-9u§() : Boolean
      {
         return this.§_-b2V§;
      }
      
      public function set §_-9u§(param1:Boolean) : void
      {
         this.§_-b2V§ = param1;
         this.§_-K13§();
      }
      
      public function §_-K13§() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(_loc1_.§_-E3b§() && !_loc1_.§_-Rk§())
         {
            §_-X1j§.§_-12n§.scene.gameInterface.§_-z26§(!this.§_-b2V§ || !this.§_-03e§);
         }
      }
      
      public function §_-Cs§(param1:§_-x2M§) : int
      {
         var _loc2_:int = 0;
         if(this.hat)
         {
            _loc2_ += this.hat.§_-V1v§().§_-F1e§(param1);
         }
         if(this.artifact)
         {
            _loc2_ += this.artifact.§_-V1v§().§_-F1e§(param1);
         }
         return _loc2_;
      }
      
      public function §_-s2C§() : Boolean
      {
         return this.§_-020§;
      }
      
      public function get weapon() : Weapon
      {
         return this._weapon;
      }
      
      public function set weapon(param1:Weapon) : void
      {
         this._weapon = param1;
      }
      
      public function get §_-81o§() : Weapon
      {
         return this.§_-83d§;
      }
      
      public function set §_-81o§(param1:Weapon) : void
      {
         this.§_-83d§ = param1;
      }
      
      public function get squad() : §_-V23§
      {
         return this.§_-bA§;
      }
      
      public function set squad(param1:§_-V23§) : void
      {
         this.§_-bA§ = param1;
         this.§_-k3§.§_-pi§(this.§_-bA§);
      }
      
      public function get record() : UserProfile
      {
         return this.§_-si§;
      }
      
      public function set record(param1:UserProfile) : void
      {
         this.§_-si§ = param1;
      }
      
      public function get §_-m1X§() : Number
      {
         return this.§_-wm§;
      }
      
      public function set §_-m1X§(param1:Number) : void
      {
         this.§_-wm§ = param1;
      }
      
      public function get direction() : int
      {
         return this._direction;
      }
      
      public function set direction(param1:int) : void
      {
         this._direction = param1;
      }
      
      public function get graphics() : §_-Z2e§
      {
         return this.§_-63r§;
      }
      
      public function set graphics(param1:§_-Z2e§) : void
      {
         this.§_-63r§ = param1;
      }
      
      public function get §_-vt§() : Boolean
      {
         return this.§_-M17§;
      }
      
      public function set §_-vt§(param1:Boolean) : void
      {
         this.§_-M17§ = param1;
      }
      
      public function get aiArsenal() : §_-A1l§
      {
         return this.§_-w28§;
      }
      
      public function set aiArsenal(param1:§_-A1l§) : void
      {
         this.§_-w28§ = param1;
      }
      
      public function get aiMissFactor() : int
      {
         return this.§_-t9§;
      }
      
      public function set aiMissFactor(param1:int) : void
      {
         this.§_-t9§ = param1;
      }
      
      public function get aiShotPower() : int
      {
         return this.§_-31h§;
      }
      
      public function set aiShotPower(param1:int) : void
      {
         this.§_-31h§ = param1;
      }
      
      public function get §_-s1l§() : String
      {
         return this.§_-m2x§;
      }
      
      public function set §_-s1l§(param1:String) : void
      {
         this.§_-m2x§ = param1;
      }
      
      public function get §_-m13§() : DisplayObject
      {
         return this.§_-63r§.§_-63t§();
      }
      
      public function get doesntCountTurns() : Boolean
      {
         return true;
      }
      
      public function get §_-9K§() : Boolean
      {
         return this.§_-811§;
      }
      
      public function set §_-9K§(param1:Boolean) : void
      {
         this.§_-811§ = param1;
      }
      
      public function get §_-G1e§() : Boolean
      {
         return this.§_-L4§;
      }
      
      public function set §_-G1e§(param1:Boolean) : void
      {
         this.§_-L4§ = param1;
      }
      
      public function §_-c1F§() : void
      {
         if(this.§_-03e§)
         {
            this.§_-Ck§();
            this.graphics.§_-i§();
            ControlSets.UNIT_CONTROL_SET.deactivate();
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-X1j§.§_-12n§.scene.gameInterface.§_-K1N§();
            }
         }
      }
      
      public function §_-W1u§(param1:int) : void
      {
         if(Boolean(this.§_-81o§) && !this.§_-81o§.§_-23R§)
         {
            this.§_-81o§.§_-23R§ = true;
         }
         ControlSets.UNIT_CONTROL_SET.activate();
         this.§_-P1W§(param1);
         dispatchEvent(new starling.events.Event(§_-L3Q§.WEAPON_SELECT));
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-X1j§.§_-12n§.scene.gameInterface.§_-a25§();
         }
      }
      
      public function §_-U1S§() : void
      {
         ControlSets.UNIT_CONTROL_SET.activate();
         this.reattachWeapon();
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-X1j§.§_-12n§.scene.gameInterface.§_-a25§();
         }
      }
      
      public function get §_-F2x§() : §_-I3t§
      {
         return this.§_-Pm§;
      }
      
      public function set §_-F2x§(param1:§_-I3t§) : void
      {
         this.§_-Pm§ = param1;
      }
      
      public function §_-Xo§() : DisplayObject
      {
         if(!disposed)
         {
            return §_-y2n§.§_-i1N§.§_-63N§(this);
         }
         return null;
      }
      
      public function §_-b2T§() : void
      {
         dispatchEventWith(§_-BY§.HIDE);
      }
      
      public function §_-Xb§() : DisplayObject
      {
         var _loc1_:Sprite = §_-z1H§ as Sprite;
         var _loc2_:LayoutGroup = new LayoutGroup();
         _loc2_.width = 40;
         _loc2_.height = 40;
         _loc2_.alignPivot();
         _loc2_.name = "battle_hint_origin";
         _loc1_.addChild(_loc2_);
         return _loc2_;
      }
      
      public function §_-k1K§() : void
      {
         var _loc2_:LayoutGroup = null;
         var _loc1_:Sprite = §_-z1H§ as Sprite;
         if(_loc1_)
         {
            _loc2_ = _loc1_.getChildByName("battle_hint_origin") as LayoutGroup;
            if(_loc2_)
            {
               _loc2_.removeFromParent(true);
            }
         }
      }
      
      public function get controlsMap() : §_-G3J§
      {
         return this.§_-4q§;
      }
      
      public function get §_-m9§() : Boolean
      {
         return this.§_-ic§;
      }
      
      public function set §_-m9§(param1:Boolean) : void
      {
         this.§_-ic§ = param1;
      }
   }
}

