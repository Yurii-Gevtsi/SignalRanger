package ru.pragmatix.wormix.weapons
{
   import §_-31N§.§_-42G§;
   import §_-l1K§.§_-W2f§;
   import §_-w2i§.§_-Zd§;
   import §_-zI§.§_-X1q§;
   import feathers.data.ArrayCollection;
   import flash.events.Event;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.menu.clans.chat.§_-D2t§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import starling.events.EventDispatcher;
   
   public class §_-E3w§ extends Weapon
   {
      
      public function §_-E3w§(param1:uint = 0)
      {
         super();
         MIN_POWER = MAX_POWER = 30;
         §_-z1W§(new §_-W2f§());
         §_-a1R§.value = false;
         §_-N1N§ = false;
      }
      
      override protected function shoot() : void
      {
         var tempOwner:§_-01J§ = null;
         owner.setSpeedVector(shotParams.angle,MAX_POWER);
         owner.attachTrail();
         UnitPhysicModel(owner.getPhysicModel()).rollable = true;
         tempOwner = owner;
         §_-Zd§.once(owner as EventDispatcher,§_-42G§.§_-K2J§,function(param1:Event):void
         {
            if(!§_-X1j§.§_-12n§.gameMaster.§_-Rk§())
            {
               tempOwner.reattachWeapon();
               §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = true;
               if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
               {
                  ControlSets.UNIT_CONTROL_SET.activate();
               }
            }
         });
         ControlSets.UNIT_CONTROL_SET.deactivate();
         detach();
      }
   }
}

