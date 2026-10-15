package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class VerticalDrillMissile extends §_-Qi§
   {
      
      private static const ANGLE:Number = -MathUtil.PI_HALF;
      
      private static const POWER:§_-TA§ = new §_-TA§(20);
      
      private static const PIERCE_DAMAGE:§_-TA§ = new §_-TA§(30);
      
      private static const MAX_COUNTER:uint = 65;
      
      private var counter:uint = 0;
      
      private var paramSet:§_-C3e§;
      
      private var wormsHit:Array;
      
      public function VerticalDrillMissile(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§)
      {
         var self:VerticalDrillMissile = null;
         var owner:§_-01J§ = param1;
         var x:int = param2;
         var y:int = param3;
         var paramSet:§_-C3e§ = param4;
         this.wormsHit = [];
         super(owner,x,y,true);
         initialize(Pool.getPoint(x,y),paramSet.ammoClassName);
         this.§_-z1H§.rotation = ANGLE;
         this.paramSet = paramSet;
         physParams.addForce(new Force("UP",ANGLE,2));
         physParams.optimizedMovement = false;
         physParams.ignoresTerrain = true;
         §_-Y2I§.stop();
         setSpeedVector(ANGLE,POWER.value);
         self = this;
         addEventListener(§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            var otherGobj:§_-F3o§ = null;
            var unit:§_-01J§ = null;
            var owner:§_-01J§ = null;
            var f:Function = null;
            var e:Event = param1;
            otherGobj = e.data.other;
            if(otherGobj is §_-01J§ && otherGobj.y < self.y)
            {
               unit = §_-01J§(otherGobj);
               otherGobj.stable = false;
               otherGobj.setSpeedXY(0,Math.max(-100,self.speedY));
               if(wormsHit.indexOf(otherGobj) == -1)
               {
                  unit.§_-G1e§ = false;
                  self.inflictDamage(otherGobj,PIERCE_DAMAGE.value,0,0);
                  wormsHit.push(otherGobj);
                  owner = self.owner;
                  f = function(param1:Event):void
                  {
                     var _loc2_:Number = NaN;
                     if(param1.data.other == null)
                     {
                        unit.§_-G1e§ = true;
                        if(otherGobj.speedY > 30)
                        {
                           _loc2_ = otherGobj.speed;
                           §_-G4§.§_-Ee§(owner,otherGobj.x,otherGobj.y,otherGobj,_loc2_,0,0,true,true);
                        }
                        otherGobj.removeEventListener(§_-L3Q§.§_-o13§,f);
                     }
                  };
                  otherGobj.addEventListener(§_-L3Q§.§_-o13§,f);
               }
            }
            else if(otherGobj is §_-F3o§ && !(otherGobj is §_-01J§))
            {
               self.inflictDamage(otherGobj,0,0,0);
            }
            if(otherGobj == null)
            {
               ++counter;
               self.inflictDamage(otherGobj,0,20,6);
               Effects.explodeParticles(self.x,self.y,4);
               if(§_-X1j§.§_-12n§.scene.hitTestPermanent(e.data.x,e.data.y))
               {
                  explode();
               }
            }
         });
         §_-32§ = §_-uN§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(this.y < 150)
         {
            this.explode();
         }
      }
      
      private function explode() : void
      {
         if(!disposed)
         {
            inflictDamage(null,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
            disposed = true;
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_LARGE,x,y,this.paramSet.explosionRadius.value * 2);
            §_-h2B§.play(§_-d27§.§_-J1x§);
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         this.wormsHit.length = 0;
         super.defaultOnDisposed(param1);
      }
   }
}

