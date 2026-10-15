package ru.pragmatix.wormix.model.user
{
   import §_-Y1O§.§_-42h§;
   import §_-b1F§.§_-G4§;
   import §_-rM§.§_-61j§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.server.RefundPvpBattleLossResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.GameObjectUtils;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class §_-r1d§ extends EventDispatcher
   {
      
      private var §_-z2g§:UserProfile;
      
      private var §_-8T§:Boolean;
      
      public function §_-r1d§()
      {
         super();
      }
      
      public function §_-yJ§(param1:UserProfile) : void
      {
         var _loc2_:Boolean = this.§_-82v§(param1);
         if(_loc2_)
         {
            dispatchEventWith(Event.CHANGE);
         }
      }
      
      public function §_-82K§(param1:UserProfile) : void
      {
         this.§_-82v§(param1);
         dispatchEventWith(Event.UPDATE);
      }
      
      private function §_-82v§(param1:UserProfile) : Boolean
      {
         if(this.§_-z2g§ == null || this.§_-z2g§ != param1)
         {
            if(this.§_-z2g§)
            {
               this.§_-z2g§.removeEventListener(Event.CHANGE,this.§_-M1j§);
            }
            this.§_-z2g§ = param1;
            if(this.§_-z2g§)
            {
               this.§_-z2g§.addEventListener(Event.CHANGE,this.§_-M1j§);
            }
            this.§_-8T§ = §_-33r§.§_-ym§(this.§_-z2g§);
            return true;
         }
         return false;
      }
      
      public function get userProfile() : UserProfile
      {
         return this.§_-z2g§;
      }
      
      public function get §_-x2k§() : Boolean
      {
         return this.§_-8T§;
      }
      
      private function §_-M1j§(param1:Event) : void
      {
         dispatchEventWith(Event.UPDATE);
      }
   }
}

