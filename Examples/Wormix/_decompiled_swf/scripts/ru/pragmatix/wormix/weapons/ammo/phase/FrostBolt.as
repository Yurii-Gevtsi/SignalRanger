package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-Z2z§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class FrostBolt extends §_-Qi§
   {
      
      private var paramSet:§_-C3e§;
      
      private var dirAngle:Number;
      
      private var initialAngle:Number;
      
      private var power:uint;
      
      private var fluct:Number = 0.12566370614359174;
      
      public function FrostBolt(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:Boolean, param6:uint = 90)
      {
         super(param1,param1.x,param1.y,false,param6,DamageType.COLD);
         this.paramSet = param2;
         this.initialAngle = param3;
         this.power = param4;
         if(!param5)
         {
            this.fluct = 0;
         }
         this.dirAngle = param3 + §_-X1j§.randomSeed.§_-Fh§(-7,7) * this.fluct;
         if(§_-X1j§.randomSeed.getBoolean())
         {
            this.fluct *= -1;
         }
         var _loc7_:Point = §_-fm§(this.dirAngle,§_-X1j§.randomSeed.§_-Fh§(0,50));
         initialize(_loc7_,param2.ammoClassName);
         Pool.putPoint(_loc7_);
         Effects.attachTrail(this,12,6,11382255,16777215,1);
         §_-32§ = §_-uN§;
         setSpeedVector(this.dirAngle,param4);
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.NONE);
         physParams.ignoresTerrain = true;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_UNIT;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
      
      protected function onFrame(param1:Event) : void
      {
         if(!disposed)
         {
            if(Math.abs(this.initialAngle - this.dirAngle) > MathUtil.PI_QUARTER)
            {
               this.fluct *= -1;
            }
            this.dirAngle += this.fluct;
            setSpeedVector(this.dirAngle,this.power);
         }
      }
      
      protected function onCollide(param1:Event) : void
      {
         if(param1.data.other != null && param1.data.other is §_-01J§ && (§_-62F§ || §_-01J§(param1.data.other).squad.getTeamId() != owner.squad.getTeamId()))
         {
            §_-h2B§.play(§_-d27§.§_-Y24§);
            inflictDamage(param1.data.other,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
            §_-01J§(param1.data.other).buffs.place(new §_-Z2z§(2));
            disposed = true;
            removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
            §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
         }
      }
   }
}

