package ru.pragmatix.wormix.messages.server
{
   import §_-21p§.§_-4w§;
   import §_-C2t§.§_-Ui§;
   import §_-DJ§.§_-J2l§;
   import §_-E1Z§.§_-E2i§;
   import §_-Y1O§.*;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import com.distriqt.extension.inappbilling.events.AvailabilityEvent;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import feathers.controls.List;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.ZombieGrave;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class GetFriendsForBossResult
   {
      
      public static const STATE_SUCCESS:uint = 0;
      
      public static const STATE_EXCEED_BATTLES:uint = 1;
      
      public static const STATE_MISSION_LOCKED:uint = 2;
      
      public var profiles:Array;
      
      public var states:Vector.<int>;
      
      public function GetFriendsForBossResult()
      {
         super();
      }
   }
}

