package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-8u§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Bolt extends §_-Qi§
   {
      
      protected const MAX_ROLLING:uint = 20;
      
      protected var paramSet:§_-C3e§;
      
      protected var distance:uint = 0;
      
      protected const groundHitSounds:Array = [§_-d27§.SHURIKEN_HITGROUND_01,§_-d27§.SHURIKEN_HITGROUND_02,§_-d27§.SHURIKEN_HITGROUND_03,§_-d27§.SHURIKEN_HITGROUND_04];
      
      protected const wormHitSounds:Array = [§_-d27§.§_-w2d§];
      
      public function Bolt(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:Boolean = true)
      {
         super(param1,param1.x,param1.y,false);
         §_-92S§ = false;
         §_-U1o§ = true;
         §_-A3d§ = true;
         this.paramSet = param2;
         var _loc6_:Point = §_-fm§(param3,5);
         initialize(_loc6_,param2.ammoClassName);
         Pool.putPoint(_loc6_);
         if(param5)
         {
            physParams.addForce(Force.WIND);
         }
         Effects.attachTrail(this,6,2,14606046,14606046);
         setSpeedVector(param3,param4);
         §_-z1H§.rotation = param3;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
      }
      
      protected function onCollide(param1:Event) : void
      {
         var _loc2_:Shell = null;
         if(param1.data.other is §_-01J§)
         {
            inflictDamage(param1.data.other,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value,false);
            if(!§_-01J§(param1.data.other).isImmobile())
            {
               UnitPhysicModel(§_-01J§(param1.data.other).getPhysicModel()).rolling = §_-H3D§.sign(speedX) * Math.min(this.MAX_ROLLING,8 + this.distance * 4);
            }
            if(!§_-01J§(param1.data.other).buffs.hasBuff(§_-8u§.NAME))
            {
               §_-h2B§.play(§_-d27§.§_-k4§(this.wormHitSounds));
            }
            §_-01J§(param1.data.other).buffs.place(new §_-8u§(owner));
         }
         else
         {
            _loc2_ = new Shell(x,y,this.paramSet.ammoClassName,1);
            _loc2_.setSpeedXY(-§_-H3D§.sign(speedX) * 5,-7 - §_-X1j§.randomSeed.§_-Fh§(1,6));
            _loc2_.clip.rotation = Math.random() * MathUtil.§_-M2N§;
            §_-h2B§.play(§_-d27§.§_-k4§(this.groundHitSounds));
         }
         disposed = true;
      }
      
      protected function onFrame(param1:Event) : void
      {
         ++this.distance;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
         super.defaultOnDisposed(param1);
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

