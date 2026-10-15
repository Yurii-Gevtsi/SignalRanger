package ru.pragmatix.wormix.messages.server
{
   import §_-02q§.§_-4s§;
   import §_-23P§.§_-u1z§;
   import §_-62§.§_-zF§;
   import §_-F2Q§.GestureEvent;
   import §_-YF§.§_-G2U§;
   import §_-p18§.§_-83X§;
   import §_-p18§.§_-V14§;
   import §_-r1C§.§_-U2p§;
   import com.greensock.loading.core.*;
   import feathers.events.FeathersEventType;
   import flash.events.Event;
   import flash.text.TextField;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.wormix.controller.*;
   import ru.pragmatix.wormix.fx.UfoTargetPointer;
   import ru.pragmatix.wormix.messages.SecureMessage;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class FastBattleResult extends SecureMessage
   {
      
      public var result:int;
      
      public var adEventId:String;
      
      public var awards:Array;
      
      public var advPurchases:Array;
      
      public var sessionKey:String;
      
      public function FastBattleResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "FastBattleResult{" + "result=" + this.result + ",adEventId=" + this.adEventId + ",award=" + this.awards + ",ads=" + this.advPurchases + ",sessionKey=" + this.sessionKey + ",isSecure=" + isSecure + "}";
      }
   }
}

