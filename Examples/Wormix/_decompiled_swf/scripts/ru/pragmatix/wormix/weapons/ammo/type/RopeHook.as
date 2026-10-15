package ru.pragmatix.wormix.weapons.ammo.type
{
   import §_-A1K§.§_-TA§;
   import §_-J31§.§_-R1h§;
   import §_-L1H§.§_-C3e§;
   import §_-Z1w§.§_-s1x§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.Rope;
   import starling.events.Event;
   
   public class RopeHook extends Rope
   {
      
      private static const MIN_DISTANCE:int = 70;
      
      private static const MAX_COLLISIONS:§_-TA§ = new §_-TA§(5);
      
      private var paramSet:§_-C3e§;
      
      private var power:Number;
      
      private var target:§_-01J§;
      
      private var curCollisions:§_-TA§ = new §_-TA§(0);
      
      private var returning:Boolean = false;
      
      public function RopeHook(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:int)
      {
         super(param1,param2,param3,param4);
         this.paramSet = param2;
         this.power = param4;
         this.maxLength = new §_-TA§(param5);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND + PhysParams.COLLIDES_WITH_UNIT;
      }
      
      override protected function onCollide(param1:Event) : void
      {
         if(param1.data.other)
         {
            if(param1.data.other is §_-01J§)
            {
               if(§_-01J§(param1.data.other).squad.getTeamId() == owner.squad.getTeamId())
               {
                  disposed = true;
               }
               else if(this.target)
               {
                  disposed = true;
               }
               else
               {
                  dispatchEvent(new §_-R1h§(§_-s1x§.§_-n1k§));
                  this.target = §_-01J§(param1.data.other);
                  this.turnBack();
               }
            }
            else
            {
               if(param1.data.other is §_-Qi§ && §_-Qi§(param1.data.other).owner.squad.getTeamId() != owner.squad.getTeamId())
               {
                  param1.data.other.disposed = true;
               }
               this.turnBack();
            }
         }
         else
         {
            inflictDamage(param1.data.other,0,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value,false);
            Effects.explodeParticles(x,y,4);
            if(++this.curCollisions.value >= MAX_COLLISIONS.value)
            {
               this.turnBack();
            }
         }
      }
      
      override protected function onFrame(param1:Event) : void
      {
         super.onFrame(param1);
         if(Boolean(this.target && this.target.disposed) || Boolean(!owner) || owner.disposed)
         {
            disposed = true;
            if(Boolean(this.target) && !this.target.disposed)
            {
               this.target.stable = false;
            }
         }
         else if(Boolean(this.target) && !disposed)
         {
            this.target.x = x;
            this.target.y = y;
            if(MathUtil.distance(owner.x,owner.y,this.target.x,this.target.y) <= MIN_DISTANCE)
            {
               disposed = true;
            }
         }
         else if(this.returning)
         {
            if(MathUtil.distance(owner.x,owner.y,x,y) <= MIN_DISTANCE)
            {
               disposed = true;
            }
         }
      }
      
      private function turnBack() : void
      {
         if(!owner.disposed && !this.disposed)
         {
            setSpeedVector(Math.atan2(owner.y - y,owner.x - x),this.power);
            this.returning = true;
            §_-V2m§ = Math.PI;
         }
         else
         {
            disposed = true;
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(Boolean(this.target) && Boolean(!this.target.disposed) && !disposed)
         {
            this.target.setSpeedVector(Math.PI,5);
         }
         super.defaultOnDisposed(param1);
      }
   }
}

