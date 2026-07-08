package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-x2C§;
   import §_-W§.§_-ix§;
   import §_-l1K§.§_-d2j§;
   import ru.pragmatix.flox.social.utils.id.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class Bazooka extends Weapon
   {
      
      public static const §_-Yg§:uint = 45;
      
      public static const §_-4A§:uint = 15;
      
      private var dontSpendTurn:Boolean;
      
      private var §_-L0§:Weapon;
      
      public function Bazooka()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-Y2U§ = new §_-x2C§(50,true);
         §_-z1W§(new §_-d2j§());
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         if(getProperty("dontSpendTurn"))
         {
            §_-a1R§.value = false;
            §_-N1N§ = false;
            this.dontSpendTurn = true;
         }
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
         if(this.dontSpendTurn)
         {
            this.§_-L0§ = owner.§_-81o§ == null || Boolean(owner.§_-81o§ is Bazooka) && Boolean(owner.§_-81o§.getProperty("dontSpendTurn")) ? §_-73v§.§_-z1F§(owner.record) : owner.§_-81o§;
         }
      }
      
      override public function detach() : void
      {
         var _loc1_:§_-01J§ = owner;
         super.detach();
         if(this.§_-L0§)
         {
            _loc1_.§_-81o§ = this.§_-L0§;
            this.§_-L0§ = null;
         }
      }
      
      override protected function shoot() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(this.dontSpendTurn) && Boolean(_loc1_) && _loc1_.§_-42C§())
         {
            _loc1_.activeUnit.§_-vp§ = true;
         }
         var _loc2_:Class = getProperty("ammoClass") as Class;
         new _loc2_(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power));
         this.detach();
         if(this.dontSpendTurn)
         {
            if(Boolean(_loc1_) && _loc1_.§_-42C§())
            {
               if(_loc1_.§_-E3b§())
               {
                  ControlSets.UNIT_CONTROL_SET.activate();
               }
            }
         }
         else
         {
            nextTurnOnStable();
         }
      }
   }
}

