package ru.pragmatix.wormix.objects
{
   import §_-31N§.§_-42G§;
   import §_-31N§.§_-Yy§;
   import §_-31Y§.§_-D3B§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-63U§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-F39§.*;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-P1B§.§_-EB§;
   import §_-Tm§.§_-RF§;
   import §_-Ty§.§_-1R§;
   import §_-Ty§.§_-71n§;
   import §_-Ty§.§_-Jb§;
   import §_-Ty§.§_-M1h§;
   import §_-Ty§.§_-Z2E§;
   import §_-Y1O§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-b1F§.§_-H3Z§;
   import §_-b1F§.§_-v2Q§;
   import §_-j1w§.§_-B2i§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-l2r§.§_-K1t§;
   import §_-nZ§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-sQ§.§_-H3D§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-5c§;
   import config.§_-V2w§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.LayoutGroup;
   import feathers.events.FeathersEventType;
   import flash.display.DisplayObjectContainer;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.ChooseBonusItemResult;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.core.Starling;
   import starling.display.Canvas;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-F3o§ extends EventDispatcher
   {
      
      public static const §_-A3X§:§_-TA§ = new §_-TA§(6);
      
      protected var §_-D2j§:§_-TA§ = new §_-TA§(§_-A3X§.value);
      
      protected var id:uint;
      
      protected var _type:ObjectType;
      
      protected var §_-j2P§:SortingLayer;
      
      private var §_-d28§:DisplayObjectContainer;
      
      protected var §_-z1H§:DisplayObject;
      
      protected var §_-53z§:Point;
      
      protected var §_-V2m§:Number = 0;
      
      protected var _width:uint = 1;
      
      protected var _height:uint = 1;
      
      public var §_-Y2I§:IPhysicModel;
      
      private var §_-G2s§:PhysParams;
      
      protected var §_-33R§:§_-EB§;
      
      private var §_-f2P§:Boolean = false;
      
      protected var §_-KX§:§_-H3Z§;
      
      public var §_-Nt§:Boolean = false;
      
      protected var tags:Vector.<§_-D3B§> = new Vector.<§_-D3B§>(0);
      
      private var §_-T2c§:Boolean = false;
      
      protected var frames:int = 0;
      
      protected var §_-O27§:Boolean = true;
      
      protected var §_-53T§:Boolean = false;
      
      public function §_-F3o§(param1:Boolean = false, param2:ObjectType = null, param3:SortingLayer = null)
      {
         super();
         this.id = Boolean(!param1) && Boolean(§_-X1j§.§_-12n§) && Boolean(§_-X1j§.§_-12n§.scene) ? §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-qj§() : 0;
         this._type = param2 || ObjectType.GAME_OBJECT;
         this.§_-j2P§ = param3 ? param3 : SortingLayer.UNIT_LAYER;
         this.§_-53z§ = Pool.getPoint(0,0);
         this.§_-93A§();
         this.initPhysicModel();
         this.§_-Y2I§.addEventListener(§_-L3Q§.§_-kl§,this.defaultOnSink);
         this.§_-Y2I§.addEventListener(§_-L3Q§.§_-o13§,this.defaultOnCollide);
         this.§_-Y2I§.addEventListener(§_-42G§.§_-K2J§,this.§_-635§);
         this.§_-Y2I§.addEventListener(§_-L3Q§.§_-U2j§,this.defaultOnBeforeHit);
         this.§_-KX§ = new §_-H3Z§();
      }
      
      public function §_-93A§() : void
      {
      }
      
      public function getId() : uint
      {
         return this.id;
      }
      
      public function getPhysicModel() : IPhysicModel
      {
         return this.§_-Y2I§;
      }
      
      protected function initPhysicModel() : void
      {
         this.§_-G2s§ = new PhysParams(CollideType.NONE);
         this.§_-Y2I§ = new GenericPhysicModel(this.§_-G2s§);
      }
      
      public function get x() : int
      {
         return this.§_-Y2I§.x;
      }
      
      public function set x(param1:int) : void
      {
         this.§_-Y2I§.x = param1;
      }
      
      public function get y() : int
      {
         return this.§_-Y2I§.y;
      }
      
      public function set y(param1:int) : void
      {
         this.§_-Y2I§.y = param1;
      }
      
      public function get height() : int
      {
         return this._height;
      }
      
      public function §_-22o§() : void
      {
         if(this.clip)
         {
            this._width = Math.max(1,this.clip.width);
            this._height = Math.max(1,this.clip.height);
         }
      }
      
      public function get width() : int
      {
         return this._width;
      }
      
      public function get angle() : Number
      {
         return this.§_-Y2I§.angle;
      }
      
      public function set disabled(param1:Boolean) : void
      {
         this.§_-53T§ = param1;
      }
      
      public function get disabled() : Boolean
      {
         return this.§_-53T§;
      }
      
      public function get disposed() : Boolean
      {
         return this.§_-Y2I§ ? this.§_-G2s§.disposed : true;
      }
      
      public function set disposed(param1:Boolean) : void
      {
         this.§_-G2s§.disposed = param1;
         if(param1 && !this.physParams.disposed)
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"disposal hacked"));
         }
      }
      
      public function get stable() : Boolean
      {
         return Boolean(this.§_-Y2I§) && this.§_-Y2I§.stable;
      }
      
      public function set stable(param1:Boolean) : void
      {
         this.§_-Y2I§.stable = param1;
      }
      
      public function setSpeedVector(param1:Number, param2:Number) : void
      {
         this.§_-Y2I§.setSpeedVector(param1,param2);
      }
      
      public function addSpeedVector(param1:Number, param2:Number) : void
      {
         this.§_-Y2I§.addSpeedVector(param1,param2);
      }
      
      public function setSpeedXY(param1:Number, param2:Number) : void
      {
         this.§_-Y2I§.setSpeedXY(param1,param2);
      }
      
      public function addSpeedXY(param1:Number, param2:Number) : void
      {
         this.§_-Y2I§.addSpeedXY(param1,param2);
      }
      
      public function get speedX() : Number
      {
         return this.§_-Y2I§.speedX;
      }
      
      public function get speedY() : Number
      {
         return this.§_-Y2I§.speedY;
      }
      
      public function get speed() : Number
      {
         return this.§_-Y2I§.speed;
      }
      
      protected function §_-Vw§(param1:int, param2:int) : void
      {
         this.§_-53z§.x = param1;
         this.§_-53z§.y = param2;
      }
      
      public function nextState() : void
      {
         this.increaseFrames();
         this.§_-Y2I§.nextState();
      }
      
      protected function increaseFrames() : void
      {
         ++this.frames;
      }
      
      public function §_-q1Q§() : void
      {
         if(this.§_-z1H§ != null)
         {
            this.§_-z1H§.x = this.§_-Y2I§.x + this.§_-53z§.x;
            this.§_-z1H§.y = this.§_-Y2I§.y + this.§_-53z§.y;
         }
      }
      
      public function updateClipPos() : void
      {
         if(this.§_-z1H§ != null)
         {
            this.§_-z1H§.x = MathUtil.§_-Z2Y§(this.§_-z1H§.x,this.§_-Y2I§.x + this.§_-53z§.x,0.5);
            this.§_-z1H§.y = MathUtil.§_-Z2Y§(this.§_-z1H§.y,this.§_-Y2I§.y + this.§_-53z§.y,0.5);
         }
      }
      
      public function get clip() : DisplayObject
      {
         return this.§_-z1H§;
      }
      
      public function set clip(param1:DisplayObject) : void
      {
         this.§_-z1H§ = param1;
         this.§_-22o§();
      }
      
      public function set scale(param1:Number) : void
      {
         if(this.§_-d28§)
         {
            this.§_-d28§.scaleX = this.§_-d28§.scaleY = param1;
         }
         if(this.§_-z1H§)
         {
            this.§_-z1H§.scaleX = this.§_-z1H§.scaleY = param1;
         }
      }
      
      public function §_-F1S§() : void
      {
         var _loc1_:Rectangle = null;
         if(this.§_-z1H§)
         {
            _loc1_ = this.§_-z1H§.getBounds(this.§_-z1H§);
            this.§_-G2s§.setGroundShift(Math.round((_loc1_.left + _loc1_.right) / 2),Math.round(_loc1_.bottom));
            Pool.putRectangle(_loc1_);
         }
      }
      
      public function get type() : ObjectType
      {
         return this._type;
      }
      
      public function get §_-V2p§() : SortingLayer
      {
         return this.§_-j2P§;
      }
      
      public function get lastDamager() : §_-H3Z§
      {
         return this.§_-KX§;
      }
      
      public function get attributes() : §_-EB§
      {
         return this.§_-33R§;
      }
      
      public function get §_-92S§() : Boolean
      {
         return this.§_-T2c§;
      }
      
      public function set §_-92S§(param1:Boolean) : void
      {
         this.§_-T2c§ = param1;
      }
      
      public function §_-l28§(param1:§_-D3B§) : Boolean
      {
         return this.tags.indexOf(param1) != -1;
      }
      
      public function defaultOnDamaged(param1:Event, param2:§_-v2Q§) : void
      {
         var _loc3_:int = param2.victim.x - param2.source.x;
         var _loc4_:int = param2.victim.y - param2.source.y;
         var _loc5_:Number = _loc3_ != 0 ? Math.atan2(_loc4_,_loc3_) : -MathUtil.PI_HALF;
         this.§_-KX§.§_-Ku§(param2.source);
         var _loc6_:Number = Math.max(2,param2.power / this.§_-D2j§.value);
         this.addSpeedVector(_loc5_,_loc6_);
         this.§_-Y2I§.stable = false;
      }
      
      public function §_-E17§(param1:§_-TA§) : void
      {
         this.§_-D2j§.value = param1.value;
      }
      
      public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.§_-Y2I§)
         {
            this.§_-Y2I§.removeEventListener(§_-L3Q§.§_-kl§,this.defaultOnSink);
            this.§_-Y2I§.removeEventListener(§_-L3Q§.§_-o13§,this.defaultOnCollide);
            this.§_-Y2I§.removeEventListener(§_-42G§.§_-K2J§,this.§_-635§);
            this.§_-Y2I§.removeEventListener(§_-L3Q§.§_-U2j§,this.defaultOnBeforeHit);
            this.§_-Y2I§.clear();
         }
         this.§_-d28§ = null;
         if(this.§_-z1H§)
         {
            if(this.§_-z1H§.parent)
            {
               this.§_-z1H§.parent.removeChild(this.§_-z1H§);
            }
            this.§_-z1H§.dispose();
         }
         this.§_-z1H§ = null;
         this._type = null;
         this.§_-G2s§ = null;
         this.§_-Y2I§ = null;
         this.§_-KX§ = null;
         Pool.putPoint(this.§_-53z§);
         this.§_-53z§ = null;
      }
      
      protected function defaultOnSink(param1:Event) : void
      {
         if(this.y > 0)
         {
            §_-h2B§.play(§_-d27§.§_-k4§([§_-d27§.WATER_SPLASH1,§_-d27§.WATER_SPLASH2,§_-d27§.WATER_SPLASH3]));
         }
         this.§_-f2P§ = true;
         this.disposed = true;
         dispatchEvent(param1);
      }
      
      protected function defaultOnCollide(param1:Event) : void
      {
         dispatchEvent(param1);
      }
      
      private function §_-635§(param1:Event) : void
      {
         this.§_-q1Q§();
      }
      
      protected function defaultOnBeforeHit(param1:§_-Yy§) : void
      {
         param1.§_-33i§ = this;
         var _loc2_:§_-Yy§ = param1.clone() as §_-Yy§;
         if(param1.subject)
         {
            param1.subject.dispatchEvent(_loc2_);
         }
         if(_loc2_.§_-K1l§)
         {
            param1.§_-K1l§ = true;
         }
      }
      
      public function get §_-23f§() : Boolean
      {
         return this.§_-O27§;
      }
      
      public function set §_-23f§(param1:Boolean) : void
      {
         this.§_-O27§ = param1;
      }
      
      public function get physParams() : PhysParams
      {
         return this.§_-G2s§;
      }
      
      public function set physParams(param1:PhysParams) : void
      {
         this.§_-G2s§ = param1;
      }
      
      public function get §_-91i§() : Boolean
      {
         return this.§_-f2P§;
      }
      
      public function set §_-91i§(param1:Boolean) : void
      {
         this.§_-f2P§ = param1;
      }
   }
}

