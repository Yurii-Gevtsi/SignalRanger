package ru.pragmatix.wormix.weapons
{
   import §_-31N§.§_-42G§;
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.BouncingClusterGrenade;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.events.Event;
   
   public class §_-E3o§ extends BouncingClusterGrenade
   {
      
      public function §_-E3o§(param1:§_-01J§, param2:§_-C3e§, param3:ShellBurst, param4:uint, param5:Number, param6:uint, param7:Number, param8:String)
      {
         super(param1,param2,param3,param4,param5,param6,param7,param8);
         counter.detach();
         counter = null;
      }
      
      override protected function onStopMoving(param1:Event) : void
      {
         explode();
      }
      
      override protected function grenadeTick() : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc1_:Vector.<§_-01J§> = Physics.getUnitsInCircle(x,y,paramSet.damageRadius.value);
         for each(_loc2_ in _loc1_)
         {
            if(!_loc2_.disposed && _loc2_ != owner && _loc2_.squad.getTeamId() != owner.squad.getTeamId())
            {
               explode();
               break;
            }
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-Y2I§.removeEventListener(§_-42G§.§_-X1C§,this.onStopMoving);
         super.defaultOnDisposed(param1);
      }
   }
}

