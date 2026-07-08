package ru.pragmatix.wormix.weapons.ammo.type.grenade.sticky
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-Q2U§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.GrenadePhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.weapons.ammo.Shard;
   import starling.events.Event;
   
   public class ShardMine extends Shard implements §_-Q2U§
   {
      
      private var setup:Boolean = false;
      
      private var duration:§_-TA§;
      
      private var curTurn:§_-TA§ = new §_-TA§(0);
      
      public function ShardMine(param1:§_-01J§, param2:int, param3:int, param4:Number, param5:uint, param6:§_-C3e§, param7:Number = 1, param8:Boolean = false, param9:uint = 5)
      {
         super(param1,param2,param3,param4,param5,param6,param7,false);
         tags = new <§_-D3B§>[§_-D3B§.§_-f1U§];
         §_-iN§ = -1;
         §_-Nt§ = true;
         §_-32§ = §_-h2D§;
         this.duration = new §_-TA§(param9);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
         if(§_-Y2I§.hitTestGround(x,y))
         {
            explode(null);
         }
      }
      
      private function nextTurn(param1:Event) : void
      {
         if(++this.curTurn.value >= this.duration.value)
         {
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,36,null);
            disposed = true;
         }
      }
      
      override protected function onCollision(param1:Event) : void
      {
         if(!this.setup && param1.data.other != null)
         {
            explode(param1.data.other);
         }
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-Y2I§ = new GrenadePhysicModel(physParams,0.3,0);
         §_-Y2I§.addEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
      }
      
      protected function onStopMoving(param1:Event) : void
      {
         stable = true;
         if(!this.setup)
         {
            this.setup = true;
            §_-X1j§.§_-12n§.gameMaster.nextTurnOnStable();
         }
      }
      
      override public function nextState() : void
      {
         if(!§_-53T§)
         {
            this.checkTargets();
         }
         super.nextState();
      }
      
      private function checkTargets() : void
      {
         var _loc1_:§_-01J§ = null;
         if(this.setup)
         {
            for each(_loc1_ in Physics.getUnitsInCircle(x,y,paramSet.damageRadius.value))
            {
               if(!_loc1_.disposed)
               {
                  explode(_loc1_ as §_-F3o§);
                  break;
               }
            }
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
         §_-Y2I§.removeEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
         super.defaultOnDisposed(param1);
      }
      
      public function getDangerRadius() : int
      {
         return paramSet.damageRadius.value + 5;
      }
   }
}

