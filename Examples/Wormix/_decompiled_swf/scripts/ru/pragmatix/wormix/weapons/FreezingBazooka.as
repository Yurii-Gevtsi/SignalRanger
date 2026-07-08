package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-x2C§;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-l1K§.§_-N1Z§;
   import §_-z1w§.*;
   import ru.pragmatix.wormix.messages.server.GetRatingResult;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.weapons.ammo.FreezingMissile;
   import starling.events.Event;
   
   public class FreezingBazooka extends Bazooka
   {
      
      private static const §_-f§:uint = 30;
      
      public function FreezingBazooka()
      {
         super();
         §_-Y2U§ = new §_-x2C§(90,true,0);
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         if(param1["trajectoryCrosshair"])
         {
            §_-r2C§.length = 0;
            §_-z1W§(new §_-N1Z§(§_-f§,new <Force>[Force.GRAVITY,Force.WIND]));
         }
         super.§_-z2Y§(param1);
      }
      
      override protected function shoot() : void
      {
         var _loc1_:uint = property["trajectoryCrosshair"] ? §_-f§ : §_-Yz§(shotParams.power);
         var _loc2_:FreezingMissile = new FreezingMissile(owner,getBarrelPoint(shotParams.angle),paramSet,shotParams.angle,_loc1_);
         var _loc3_:Array = [];
         if(property["bonusOnFrozen"])
         {
            _loc3_.push(§_-Z2z§.NAME);
         }
         if(property["bonusOnSlowed"])
         {
            _loc3_.push(§_-M2S§.NAME);
         }
         if(_loc3_.length > 0)
         {
            _loc2_.applyBonusOnDebuffs(_loc3_);
         }
         if(property["freezeDuration"])
         {
            _loc2_.setFreezeDuration(property["freezeDuration"]);
         }
         if(property["bonusDamage"])
         {
            _loc2_.addBonus(property["bonusDamage"]);
         }
         if(property["bonusFreeze"])
         {
            _loc2_.setDirectHitDuration(property["bonusFreeze"]);
         }
         if(!shotParams.§_-R18§ && Boolean(property["targetSearch"]))
         {
            _loc2_.enableTargetSearch();
         }
         if(shotsLeft.value == 0)
         {
            detach();
            nextTurnOnStable();
         }
      }
   }
}

