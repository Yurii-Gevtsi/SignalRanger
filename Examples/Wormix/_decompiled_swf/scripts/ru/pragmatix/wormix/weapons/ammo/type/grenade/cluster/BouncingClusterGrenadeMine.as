package ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.ai.§_-Q2U§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   
   public class BouncingClusterGrenadeMine extends BouncingClusterGrenade implements §_-Q2U§
   {
      
      private static const MOVE_LIFE_TIME_LIMIT:int = 10 * 30;
      
      private var setup:Boolean = false;
      
      public function BouncingClusterGrenadeMine(param1:§_-01J§, param2:§_-C3e§, param3:ShellBurst, param4:uint, param5:Number, param6:uint, param7:Number, param8:String)
      {
         super(param1,param2,param3,param4,param5,param6,param7,param8);
         counter.detach();
         counter = null;
         §_-iN§ = -1;
      }
      
      override protected function onStopMoving(param1:Event) : void
      {
         stable = true;
         if(!this.setup)
         {
            this.setup = true;
            tags.push(§_-D3B§.§_-f1U§);
            §_-X1j§.§_-12n§.gameMaster.nextTurnOnStable();
         }
      }
      
      override protected function grenadeTick() : void
      {
         var _loc2_:§_-01J§ = null;
         if(disposed)
         {
            return;
         }
         var _loc1_:Vector.<§_-01J§> = Physics.getUnitsInCircle(x,y,paramSet.damageRadius.value);
         for each(_loc2_ in _loc1_)
         {
            if(!_loc2_.disposed && _loc2_ != owner && _loc2_.squad.getTeamId() != owner.squad.getTeamId())
            {
               explode();
               return;
            }
         }
         if(frames > MOVE_LIFE_TIME_LIMIT && !this.setup && !stable)
         {
            explode();
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-Y2I§.removeEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
         super.defaultOnDisposed(param1);
      }
      
      public function getDangerRadius() : int
      {
         return paramSet.damageRadius.value + 5;
      }
   }
}

