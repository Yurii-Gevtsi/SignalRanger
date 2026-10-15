package ru.pragmatix.wormix.weapons
{
   import §_-03T§.§_-2p§;
   import §_-73d§.§_-h2X§;
   import §_-b1F§.§_-G4§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   
   public class ArchbotSacrifice extends Weapon
   {
      
      public function ArchbotSacrifice()
      {
         super();
         MIN_POWER = 45;
         MAX_POWER = 45;
         §_-z1W§(new §_-W2f§());
         §_-Y2U§ = new §_-h2X§(300);
      }
      
      override protected function shoot() : void
      {
         var _loc1_:§_-01J§ = §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner)[0];
         var _loc2_:§_-01J§ = this.§_-cd§(_loc1_);
         if(_loc2_ == null)
         {
            _loc2_ = owner;
         }
         if(_loc2_)
         {
            §_-G4§.§_-Ee§(owner,_loc1_.x,_loc1_.y,null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value,false);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,_loc1_.x,_loc1_.y,30,§_-d27§.§_-S14§);
            Physics.teleportUnit(_loc2_ as §_-F3o§,_loc1_.x,_loc1_.y);
            §_-h2B§.play(§_-d27§.TELEPORT);
            Effects.putSimpleUncontrolledDisappearingCrossMark(_loc1_.x,_loc1_.y,20,true);
         }
         detach();
         nextTurnOnStable();
      }
      
      private function §_-cd§(param1:§_-01J§) : §_-01J§
      {
         return Physics.nearestObject(param1 as §_-F3o§,§_-X1j§.§_-12n§.gameMaster.§_-b2W§(owner,false,false)) as §_-01J§;
      }
   }
}

