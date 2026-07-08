package ru.pragmatix.wormix.objects
{
   import §_-21p§.§_-N1H§;
   import §_-73d§.§_-V19§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-G2H§.*;
   import §_-H2g§.§_-di§;
   import §_-P1z§.*;
   import §_-Y1b§.§_-OP§;
   import §_-Z1K§.§_-A2n§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-b1F§.§_-G4§;
   import §_-j1w§.§_-L1x§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-sQ§.§_-H3D§;
   import §_-u1T§.LocalizationService;
   import §_-x22§.§_-xY§;
   import dragonBones.animation.WorldClock;
   import dragonBones.objects.DragonBonesData;
   import dragonBones.starling.StarlingFactory;
   import feathers.controls.List;
   import flash.desktop.NativeApplication;
   import flash.geom.Point;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.GuardingBot;
   import ru.pragmatix.wormix.weapons.ammo.feats.IFrameFeat;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-Qi§ extends §_-F3o§
   {
      
      private static var §_-AB§:StarlingFactory;
      
      private static var §_-d2k§:DragonBonesData;
      
      public static const §_-n18§:uint = 300;
      
      protected static const AMMO_ARMATURE_PREFIX:String = "resurces/_ammo/";
      
      private static const §_-x2J§:String = "resurces/_ammo/";
      
      private var §_-m1K§:int = 0;
      
      private var §_-233§:int;
      
      private var §_-s1M§:int;
      
      private var §_-d2§:Boolean = false;
      
      private var §_-O2t§:Boolean = false;
      
      protected var §_-22h§:int;
      
      protected var §_-iN§:int;
      
      protected const §_-8G§:int = 0;
      
      protected const §_-uN§:int = 1;
      
      protected const §_-h2D§:int = 2;
      
      protected const §_-t3§:int = 3;
      
      protected var §_-32§:int = 0;
      
      protected var §_-In§:DamageType;
      
      protected var §_-ni§:Boolean = true;
      
      protected var armature:§_-di§;
      
      protected var §_-433§:Boolean = this.§_-62F§ = true;
      
      protected var §_-62F§:Boolean;
      
      protected var §_-P1P§:Vector.<IFrameFeat> = new Vector.<IFrameFeat>(0);
      
      public var owner:§_-01J§;
      
      public var §_-22Y§:Boolean = false;
      
      public function §_-Qi§(param1:§_-01J§, param2:int, param3:int, param4:Boolean = false, param5:int = 300, param6:DamageType = null, param7:Boolean = true)
      {
         this.owner = param1;
         this.§_-92S§ = param7;
         this.§_-m1K§ = param1 != null ? param1.direction : 0;
         this.§_-233§ = param2;
         this.§_-s1M§ = param3;
         this.§_-In§ = param6 == null ? DamageType.PHYSICAL : param6;
         super(param4,ObjectType.GAME_OBJECT,SortingLayer.AMMO_LAYER);
         §_-Y2I§.keepUnstable = true;
         §_-TZ§.addListener(this,§_-L3Q§.§_-Qw§,defaultOnDamaged);
         this.§_-22h§ = param5;
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND + PhysParams.COLLIDES_WITH_UNIT;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
      
      protected function initialize(param1:Point, param2:String = null, param3:Boolean = false, param4:String = "resurces/_ammo/") : void
      {
         var ammoTextureName:String = null;
         var pivotPoint:Point = null;
         var base:Point = param1;
         var clipClassName:String = param2;
         var fromSpriteSheet:Boolean = param3;
         var classPrefix:String = param4;
         try
         {
            this.x = base.x;
            this.y = base.y;
            this.§_-iN§ = this.§_-22h§;
            if(clipClassName == null || clipClassName == "")
            {
               return;
            }
            if(§_-d2k§ == null)
            {
               §_-AB§ = Application.factory;
               §_-d2k§ = §_-AB§.getDragonBonesData(§_-K1t§.§_-t1V§);
            }
            this.armature = §_-K1t§.buildArmature(classPrefix + clipClassName,null,§_-K1t§.§_-t1V§);
            if(this.armature)
            {
               §_-z1H§ = this.armature.display;
               if(this.armature.animation.hasAnimation(§_-K1t§.§_-G1E§))
               {
                  this.armature.playAnimation(§_-K1t§.§_-G1E§);
               }
               else if(this.armature.animation.hasAnimation(§_-K1t§.§_-Y1c§))
               {
                  this.armature.playAnimation(§_-K1t§.§_-Y1c§);
               }
               WorldClock.clock.add(this.armature);
            }
            else
            {
               ammoTextureName = §_-x2J§ + clipClassName;
               pivotPoint = §_-A2n§.§_-l1§(clipClassName);
               §_-z1H§ = §_-AB§.getTextureDisplay(ammoTextureName,§_-K1t§.§_-t1V§) as Image;
               §_-z1H§.pivotX = -pivotPoint.x / §_-K1t§.§_-31c§;
               §_-z1H§.pivotY = -pivotPoint.y / §_-K1t§.§_-31c§;
               Pool.putPoint(pivotPoint);
            }
            §_-z1H§.scaleX = §_-z1H§.scaleY = §_-K1t§.§_-31c§;
         }
         catch(e:ArgumentError)
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A3T§,"ammo init hack"));
         }
      }
      
      protected function §_-fm§(param1:Number, param2:uint = 25, param3:Point = null) : Point
      {
         var _loc4_:Point = param3 == null ? Pool.getPoint(this.owner.x,this.owner.y) : param3;
         var _loc5_:Point = Pool.getPoint(_loc4_.x + Math.cos(param1) * param2,_loc4_.y + Math.sin(param1) * param2);
         if(param3 == null)
         {
            Pool.putPoint(_loc4_);
         }
         return _loc5_;
      }
      
      public function §_-S2S§(param1:Boolean, param2:Boolean) : void
      {
         this.§_-433§ = param1;
         this.§_-62F§ = param2;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-TZ§.removeListener(this,§_-L3Q§.§_-Qw§,defaultOnDamaged);
         super.defaultOnDisposed(param1);
         if(this.armature)
         {
            if(WorldClock.clock.contains(this.armature))
            {
               WorldClock.clock.remove(this.armature);
            }
            this.armature.dispose();
            this.armature = null;
         }
      }
      
      public function inflictDamage(param1:§_-F3o§, param2:Number, param3:Number, param4:Number, param5:Boolean = true, param6:Boolean = false) : void
      {
         var victim:§_-F3o§ = param1;
         var damage:Number = param2;
         var damageRadius:Number = param3;
         var expRadius:Number = param4;
         var selfHit:Boolean = param5;
         var pure:Boolean = param6;
         try
         {
            §_-G4§.inflictDamage(this,victim,damage,damageRadius,expRadius,selfHit,pure);
         }
         catch(e:ArgumentError)
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A3T§,"explosion hack 2"));
         }
      }
      
      protected function mainOnFrame() : void
      {
         this.rotate();
         this.§_-f2K§();
      }
      
      private function rotate() : void
      {
         var _loc1_:Number = NaN;
         if(clip == null)
         {
            return;
         }
         if(this.§_-32§ == this.§_-uN§)
         {
            this.clip.rotation = this.angle + this.§_-V2m§;
         }
         else if(this.§_-32§ == this.§_-h2D§)
         {
            if((Math.abs(speedX) > 0.1 || Math.abs(speedY) > 0.1) && §_-Y2I§.lastTravelledDistance > 1)
            {
               this.clip.rotation += Math.max(Math.abs(speed),7) * §_-H3D§.sign(speedX) * MathUtil.PI_ONE_180;
            }
         }
         else if(this.§_-32§ == this.§_-t3§)
         {
            _loc1_ = angle + §_-V2m§;
            if(Math.abs(_loc1_) > MathUtil.PI_HALF && clip.scaleY > 0)
            {
               clip.scaleY *= -1;
            }
            else if(Math.abs(_loc1_) < MathUtil.PI_HALF && clip.scaleY < 0)
            {
               clip.scaleY *= -1;
            }
            clip.rotation = _loc1_;
         }
      }
      
      private function §_-f2K§() : void
      {
         if(this.§_-iN§ > 0)
         {
            if(!disposed && --this.§_-iN§ == 0)
            {
               if(clip != null)
               {
                  Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,clip.width * 2,§_-d27§.§_-J1x§);
               }
               disposed = true;
            }
         }
      }
      
      override public function nextState() : void
      {
         var _loc1_:IFrameFeat = null;
         this.mainOnFrame();
         for each(_loc1_ in this.§_-P1P§)
         {
            _loc1_.nextState(frames);
         }
         super.nextState();
      }
      
      public function get §_-53a§() : int
      {
         return this.§_-m1K§;
      }
      
      public function get §_-w1U§() : int
      {
         return this.§_-233§;
      }
      
      public function get §_-i2D§() : int
      {
         return this.§_-s1M§;
      }
      
      public function get §_-U1o§() : Boolean
      {
         return this.§_-d2§;
      }
      
      public function set §_-U1o§(param1:Boolean) : void
      {
         this.§_-d2§ = param1;
      }
      
      public function get §_-A3d§() : Boolean
      {
         return this.§_-O2t§;
      }
      
      public function set §_-A3d§(param1:Boolean) : void
      {
         this.§_-O2t§ = param1;
      }
      
      public function get damageType() : DamageType
      {
         return this.§_-In§;
      }
      
      public function get §_-C2P§() : Boolean
      {
         return this.§_-ni§;
      }
      
      public function set §_-C2P§(param1:Boolean) : void
      {
         this.§_-ni§ = param1;
      }
   }
}

