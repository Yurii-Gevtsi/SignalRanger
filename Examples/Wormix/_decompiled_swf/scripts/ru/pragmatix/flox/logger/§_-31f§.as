package ru.pragmatix.flox.logger
{
   import §_-31N§.§_-42G§;
   import §_-533§.§_-c2b§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-zF§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-x2t§;
   import §_-L1H§.§_-C3e§;
   import §_-Z1K§.§_-q24§;
   import §_-l1K§.§_-d2j§;
   import §_-w2i§.§_-Zd§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.display.Display;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import com.distriqt.extension.inappbilling.events.AvailabilityEvent;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-z1x§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.events.EventDispatcher;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-31f§
   {
      
      private static var §_-Qh§:Array = [];
      
      private var §_-C3o§:String;
      
      private var §_-8L§:Array;
      
      private var §_-Th§:Array;
      
      private var _level:§_-93v§;
      
      public function §_-31f§(param1:String)
      {
         super();
         if(Boolean(param1) && param1.length > 0)
         {
            this.§_-C3o§ = param1;
         }
         this._level = §_-93v§.LOG;
         this.§_-8L§ = new Array();
         this.§_-Th§ = new Array();
      }
      
      public static function §_-43B§(param1:§_-a1K§) : void
      {
         if(!§_-Qh§[param1.toString()])
         {
            §_-Qh§[param1.toString()] = param1;
         }
      }
      
      public static function §_-Df§(param1:§_-a1K§) : void
      {
         delete §_-Qh§[param1.toString()];
      }
      
      public function §_-N1n§(param1:§_-93v§) : void
      {
         this._level = param1;
      }
      
      public function addPublisher(param1:§_-a1K§) : void
      {
         if(!this.§_-8L§[param1.toString()])
         {
            this.§_-8L§[param1.toString()] = param1;
         }
      }
      
      public function §_-22p§(param1:§_-a1K§) : void
      {
         delete this.§_-8L§[param1.toString()];
      }
      
      public function §_-e1E§() : Array
      {
         return this.§_-8L§;
      }
      
      private function publish(param1:Object, param2:§_-93v§, param3:String) : void
      {
         var _loc4_:§_-V2x§ = null;
         var _loc5_:String = null;
         if(param2.getValue() >= this._level.getValue())
         {
            _loc4_ = new §_-V2x§(this.§_-C3o§,param1,param2,param3);
            if(this.§_-8L§)
            {
               for(_loc5_ in this.§_-8L§)
               {
                  §_-a1K§(this.§_-8L§[_loc5_]).publish(_loc4_);
               }
            }
            if(§_-Qh§)
            {
               for(_loc5_ in §_-Qh§)
               {
                  §_-a1K§(§_-Qh§[_loc5_]).publish(_loc4_);
               }
            }
         }
      }
      
      public function console(param1:Object) : void
      {
         this.publish(param1,§_-93v§.LOG,"console");
      }
      
      public function log(param1:Object, param2:String = null) : void
      {
         this.publish(param1,§_-93v§.LOG,param2);
      }
      
      public function debug(param1:Object, param2:String = null) : void
      {
         this.publish(param1,§_-93v§.DEBUG,param2);
      }
      
      public function info(param1:Object, param2:String = null) : void
      {
         this.publish(param1,§_-93v§.INFO,param2);
      }
      
      public function §_-E2X§(param1:Object, param2:String = null) : void
      {
         this.publish(param1,§_-93v§.WARNING,param2);
      }
      
      public function error(param1:Object, param2:String = null) : void
      {
         this.publish(param1,§_-93v§.ERROR,param2);
      }
      
      public function §_-M2b§(param1:Object, param2:String = null) : void
      {
         this.publish(param1,§_-93v§.FATAL,param2);
      }
   }
}

