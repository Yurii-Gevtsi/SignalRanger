package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-7u§;
   import §_-gG§.§_-M2S§;
   import §_-l1K§.§_-cZ§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.StaticCharge;
   
   public class Shocker extends §_-72x§
   {
      
      private static const §_-p2S§:uint = 30;
      
      private static const SHOT_DELAY:uint = 15;
      
      public function Shocker()
      {
         super();
         §_-z1W§(new §_-cZ§());
      }
      
      override public function detach() : void
      {
         shooting = false;
         super.detach();
      }
      
      override protected function shoot() : void
      {
         var dmgX:int;
         var dmgY:int;
         var jump:Boolean;
         var slow:Boolean;
         var units:Vector.<§_-01J§>;
         var unit:§_-01J§ = null;
         shooting = true;
         dmgX = owner.x + owner.direction * §_-p2S§;
         dmgY = owner.y;
         jump = Boolean(getProperty("jump")) && !shotParams.§_-R18§ ? §_-7u§(getProperty("jump")).value : false;
         slow = getProperty("isSlowdown") ? §_-7u§(getProperty("isSlowdown")).value : false;
         units = Physics.getUnitsInCircle(dmgX,dmgY,paramSet.damageRadius.value);
         for each(unit in units)
         {
            if(unit != owner)
            {
               Effects.lightningOnObject(unit as §_-F3o§,owner.x + owner.direction * 10,owner.y,SHOT_DELAY,5,13948116,6);
               if(slow)
               {
                  if(unit.buffs.hasBuff(§_-M2S§.NAME))
                  {
                     unit.buffs.§_-e1z§(§_-M2S§.NAME);
                  }
                  unit.buffs.place(new §_-M2S§());
               }
            }
         }
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            ControlSets.UNIT_CONTROL_SET.deactivate();
         }
         new StaticCharge(owner,paramSet,dmgX,dmgY,jump);
         §_-Ia§.§_-33o§(SHOT_DELAY,function():void
         {
            if(owner != null)
            {
               if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
               {
                  ControlSets.UNIT_CONTROL_SET.activate();
               }
               detach();
            }
            nextTurnOnStable();
         });
         §_-h2B§.play(§_-d27§.§_-Z1d§);
      }
   }
}

