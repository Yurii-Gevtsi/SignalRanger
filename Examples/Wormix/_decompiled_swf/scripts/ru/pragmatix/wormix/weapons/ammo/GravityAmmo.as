package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class GravityAmmo extends §_-Qi§
   {
      
      private static const BOUNCE_REDUCE_FACTOR:uint = 2;
      
      private static const MIN_POWER:uint = 15;
      
      private static const MAX_POWER:uint = 23;
      
      private static const PLAYER_JUMP_RADIUS:§_-TA§ = new §_-TA§(40);
      
      protected var paramSet:§_-C3e§;
      
      public function GravityAmmo(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Number, param6:uint)
      {
         super(param1,param1.x,param1.y,false,90);
         §_-92S§ = false;
         var _loc7_:Point = §_-fm§(param5,5);
         initialize(_loc7_,param4.ammoClassName);
         Pool.putPoint(_loc7_);
         this.paramSet = param4;
         setSpeedVector(param5,param6);
         §_-32§ = §_-uN§;
         physParams.removeForce(Force.GRAVITY_NAME);
         physParams.optimizedMovement = true;
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,this.onCollide);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      protected function onCollide(param1:Event) : void
      {
         var _loc2_:Vector.<§_-F3o§> = null;
         var _loc4_:Vector.<§_-F3o§> = null;
         var _loc5_:§_-F3o§ = null;
         _loc2_ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-a1N§();
         var _loc3_:Vector.<§_-F3o§> = Physics.getGameObjectsInCircle(x,y,PLAYER_JUMP_RADIUS.value,_loc2_);
         if(_loc3_.indexOf(owner as §_-F3o§) != -1 && !owner.disposed)
         {
            owner.graphics.jump();
            this.bounce(owner as §_-F3o§);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,x,y,this.paramSet.damageRadius.value / 4,§_-d27§.§_-Z1d§);
         }
         else
         {
            _loc4_ = Physics.getGameObjectsInCircle(x,y,this.paramSet.damageRadius.value,_loc2_);
            for each(_loc5_ in _loc4_)
            {
               if(!_loc5_.disposed && _loc5_ != this && _loc5_ != owner)
               {
                  inflictDamage(_loc5_,this.paramSet.damage.value,0,0);
                  if(_loc5_ is §_-01J§)
                  {
                     §_-01J§(_loc5_).rotateUntilStable();
                  }
                  if(_loc5_.§_-Nt§)
                  {
                     this.bounce(_loc5_);
                  }
               }
            }
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,x,y,this.paramSet.damageRadius.value,§_-d27§.§_-Z1d§);
         }
         disposed = true;
         §_-h2B§.play(§_-d27§.§_-H2w§);
      }
      
      private function bounce(param1:§_-F3o§) : void
      {
         var _loc2_:Number = Math.atan2(param1.y - Physics.UNIT_HALF_HEIGHT / 2 - y,param1.x - x);
         var _loc3_:Number = §_-H3D§.distance(param1.x - x,param1.y - y);
         var _loc4_:int = Math.max(MIN_POWER,Math.round(Math.sqrt(this.paramSet.damageRadius.value * this.paramSet.damageRadius.value - _loc3_ * _loc3_)) / BOUNCE_REDUCE_FACTOR);
         _loc4_ = Math.min(MAX_POWER,_loc4_);
         if(_loc2_ >= 0 && param1 is §_-01J§)
         {
            §_-01J§(param1).rolling = §_-H3D§.sign(param1.x - x) * _loc4_ * 1.5;
            param1.stable = false;
         }
         else
         {
            §_-G4§.§_-H1E§(param1,_loc2_,_loc4_,false);
         }
      }
      
      override protected function defaultOnSink(param1:Event) : void
      {
         var _loc2_:Array = null;
         if(y > 0 && x > -200 && x < §_-X1j§.§_-12n§.scene.sceneW + 200)
         {
            _loc2_ = [§_-d27§.WATER_IMPACT1,§_-d27§.WATER_IMPACT2,§_-d27§.WATER_IMPACT3];
            §_-h2B§.play(§_-d27§.§_-k4§(_loc2_));
         }
         disposed = true;
      }
   }
}

