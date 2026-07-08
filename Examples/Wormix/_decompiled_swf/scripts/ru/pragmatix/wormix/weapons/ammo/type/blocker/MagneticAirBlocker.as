package ru.pragmatix.wormix.weapons.ammo.type.blocker
{
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-su§.§_-F1j§;
   import §_-su§.§_-Vz§;
   import dragonBones.animation.WorldClock;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class MagneticAirBlocker extends AirBlocker
   {
      
      private static const ELECTRIC_CHARGE:String = "ElectricCharge";
      
      private var capturedAmmos:Dictionary;
      
      private var chargeClips:Dictionary;
      
      public function MagneticAirBlocker(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         super(param1,param2,param3,param4);
      }
      
      override protected function checkTargets() : void
      {
         var _loc1_:Vector.<§_-F3o§> = null;
         var _loc3_:§_-F3o§ = null;
         var _loc4_:* = undefined;
         super.checkTargets();
         _loc1_ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([§_-F1j§]);
         var _loc2_:Vector.<§_-F3o§> = Physics.getGameObjectsInCircle(x,y,radius,_loc1_);
         for(_loc4_ in this.capturedAmmos)
         {
            _loc3_ = §_-F3o§(_loc4_);
            if(_loc3_.disposed || !Physics.isGobjInRadius(_loc4_,x,y,radius,false))
            {
               delete this.capturedAmmos[_loc4_];
               if(this.chargeClips[_loc4_])
               {
                  this.detachElectricCharge(this.chargeClips[_loc4_]);
                  delete this.chargeClips[_loc4_];
               }
            }
         }
         for each(_loc3_ in _loc2_)
         {
            if(!this.capturedAmmos[_loc3_])
            {
               this.capturedAmmos[_loc3_] = frames + MAX_UNGROUNDED_TIME;
               this.chargeClips[_loc3_] = this.playElectricCharge(_loc3_);
            }
            else if(this.capturedAmmos[_loc3_] <= frames)
            {
               Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,_loc3_.x,_loc3_.y,30);
               Effects.lightningOnObjects(this,_loc3_,12,10,6068463,8);
               §_-h2B§.play(§_-d27§.§_-Z1d§);
               this.detachElectricCharge(this.chargeClips[_loc3_]);
               delete this.chargeClips[_loc3_];
               delete this.capturedAmmos[_loc3_];
               if(_loc3_ is §_-Vz§)
               {
                  §_-Vz§(_loc3_).explode();
               }
               else
               {
                  _loc3_.disposed = true;
               }
            }
         }
      }
      
      private function playElectricCharge(param1:§_-F3o§) : §_-di§
      {
         var _loc2_:§_-di§ = §_-K1t§.buildArmature(ELECTRIC_CHARGE,§_-K1t§.§_-G1E§,§_-K1t§.§_-B1s§);
         WorldClock.clock.add(_loc2_);
         (param1.clip as DisplayObjectContainer).addChild(_loc2_.display);
         return _loc2_;
      }
      
      override protected function onSot(param1:Event) : void
      {
         super.onSot(param1);
         if(active)
         {
            this.capturedAmmos = new Dictionary();
            this.chargeClips = new Dictionary();
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         var _loc2_:* = undefined;
         super.defaultOnDisposed(param1);
         for(_loc2_ in this.chargeClips)
         {
            if(this.chargeClips[_loc2_])
            {
               this.detachElectricCharge(this.chargeClips[_loc2_]);
               delete this.chargeClips[_loc2_];
            }
         }
      }
      
      private function detachElectricCharge(param1:§_-di§) : void
      {
         if(param1)
         {
            if(param1.display)
            {
               param1.display.removeFromParent();
            }
            WorldClock.clock.remove(param1);
            param1.dispose();
         }
      }
   }
}

