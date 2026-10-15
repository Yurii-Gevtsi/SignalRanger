package ru.pragmatix.wormix.weapons
{
   import §_-82l§.§_-A2Y§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-P2i§.§_-b7§;
   import §_-TF§.§_-y2n§;
   import §_-Vu§.MD5;
   import §_-Y1O§.*;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.ammo.Boomerang;
   import starling.events.Event;
   
   public class BoomerangWeapon extends Weapon
   {
      
      private var bouncing:Boolean = true;
      
      private var §_-514§:Boomerang;
      
      public function BoomerangWeapon()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-a1R§.value = false;
         §_-z1W§(new §_-d2j§());
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
         this.bouncing = true;
         §_-J1Q§ = false;
      }
      
      override protected function shoot() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         _loc1_.§_-c2w§();
         §_-J1Q§ = true;
         var _loc2_:int = 0;
         var _loc3_:Boolean = false;
         if(getProperty("secondHitBonus"))
         {
            _loc2_ = §_-TA§(getProperty("secondHitBonus")).value;
         }
         if(getProperty("knockHat"))
         {
            _loc3_ = §_-7u§(getProperty("knockHat")).value;
         }
         this.§_-514§ = new Boomerang(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power),this.bouncing,_loc2_,_loc3_);
         if(this.bouncing)
         {
            this.§_-514§.addEventListener(§_-L3Q§.DISPOSED,this.§_-4d§);
            this.§_-514§.addEventListener(§_-L3Q§.BOOMERANG_CAUGHT,this.§_-i6§);
         }
         else
         {
            this.§_-E0§();
            nextTurnOnStable();
            _loc1_.§_-vs§().stop(true);
         }
         detach();
      }
      
      private function §_-E0§() : void
      {
         var _loc1_:UserProfile = null;
         _loc1_ = owner.squad.§_-B2M§();
         var _loc2_:§_-Oi§ = _loc1_.§_-V2J§().§_-d0§(recordId.value);
         if(_loc2_)
         {
            _loc2_.§_-2a§();
         }
      }
      
      private function §_-I1Y§() : void
      {
         this.§_-514§.removeEventListener(§_-L3Q§.DISPOSED,this.§_-4d§);
         this.§_-514§.removeEventListener(§_-L3Q§.BOOMERANG_CAUGHT,this.§_-i6§);
      }
      
      private function §_-4d§(param1:Event) : void
      {
         §_-J1Q§ = false;
         this.§_-I1Y§();
         var _loc2_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(_loc2_)
         {
            _loc2_.§_-vs§().stop(true);
            nextTurnOnStable();
         }
      }
      
      private function §_-i6§(param1:Event) : void
      {
         var _loc3_:WeaponStructure = null;
         this.§_-I1Y§();
         var _loc2_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(!this.§_-514§.owner.disposed && !_loc2_.§_-U2O§())
         {
            if(_loc2_.§_-E3b§())
            {
               this.§_-514§.owner.§_-9u§ = false;
            }
            for each(_loc3_ in _loc2_.§_-4Y§())
            {
            }
            _loc2_.§_-36§(false);
            _loc2_.§_-c2w§(false);
            this.§_-514§.owner.weapon = this;
            this.attach(this.§_-514§.owner);
            §_-X1j§.§_-12n§.scene.dispatchEventWith(§_-L3Q§.WEAPON_RESTORE,false,recordId.value);
            this.bouncing = false;
            if(_loc2_.§_-E3b§())
            {
               §_-J1Q§ = true;
               §_-b7§.§_-z2s§(this.§_-514§.owner);
            }
         }
         else
         {
            §_-J1Q§ = false;
            _loc2_.§_-vs§().stop(true);
            nextTurnOnStable();
         }
      }
   }
}

