package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-V19§;
   import §_-SP§.§_-M4§;
   import §_-Vg§.*;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import feathers.controls.List;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.data.ListCollection;
   import feathers.events.CollectionEventType;
   import feathers.layout.AnchorLayoutData;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.events.KeyboardEvent;
   
   public class Rifle extends Weapon
   {
      
      public function Rifle()
      {
         super();
         MIN_POWER = MAX_POWER = 100;
         §_-Y2U§ = new §_-V19§(65,MIN_POWER);
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         if(param1["trajectoryCrosshair"].value)
         {
            §_-z1W§(new §_-N1Z§(MIN_POWER,new <Force>[Force.GRAVITY],§_-N1Z§.§_-q17§));
         }
         else
         {
            §_-z1W§(new §_-W2f§());
         }
         super.§_-z2Y§(param1);
      }
      
      override protected function shoot() : void
      {
         §_-h2B§.play(§_-d27§.§_-c2j§);
         var _loc1_:IPooledAmmo = AmmosFactory.getInstance().createObject(AmmosFactory.OBJECT_BULLET);
         _loc1_.initAmmo(AmmosFactory.BULLET_SIMPLE,owner,getBarrelPoint(shotParams.angle),paramSet,shotParams.angle,MIN_POWER,2,false,true);
         _loc1_.addTrail();
         if(shotsLeft.value == 0)
         {
            detach();
            nextTurnOnStable();
         }
      }
   }
}

