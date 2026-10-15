package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-12a§.§_-6p§;
   import §_-5Y§.§_-h2c§;
   import §_-L1H§.§_-b2K§;
   import §_-P1B§.§_-b21§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2W§.§_-21w§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import feathers.controls.ScreenNavigator;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   
   public class §_-I1W§
   {
      
      private var §_-z1K§:Array;
      
      private var §_-D§:ScreenNavigator;
      
      public function §_-I1W§(param1:ScreenNavigator)
      {
         super();
         this.§_-z1K§ = [];
         this.§_-D§ = param1;
         if(this.§_-D§)
         {
            this.§_-W2u§(this.§_-D§);
            this.§_-D§.addEventListener(Event.CHANGE,this.§_-c1k§);
         }
      }
      
      public function dispose() : void
      {
         if(this.§_-D§)
         {
            this.§_-D§.removeEventListener(Event.CHANGE,this.§_-c1k§);
         }
      }
      
      public function §_-3Q§(param1:DisplayObjectContainer) : void
      {
         var _loc2_:String = null;
         var _loc3_:uint = 0;
         this.§_-z1K§.push(param1);
         if(this.§_-D§)
         {
            _loc2_ = this.§_-D§.activeScreenID;
            _loc3_ = this.§_-o4§(_loc2_);
            this.§_-o2l§(param1,_loc3_);
         }
      }
      
      private function §_-c1k§(param1:Event) : void
      {
         this.§_-W2u§(param1.target as ScreenNavigator);
      }
      
      private function §_-W2u§(param1:ScreenNavigator) : void
      {
         var _loc2_:String = null;
         var _loc3_:uint = 0;
         if(param1)
         {
            _loc2_ = param1.activeScreenID;
            _loc3_ = this.§_-o4§(_loc2_);
            this.changeColor(this.§_-z1K§,_loc3_);
         }
      }
      
      private function §_-o4§(param1:String) : uint
      {
         var _loc2_:uint = 0;
         switch(param1)
         {
            case §_-21w§.§_-G31§:
               _loc2_ = 10975081;
               break;
            case §_-21w§.§_-F3d§:
               _loc2_ = 4806787;
               break;
            case §_-21w§.§_-H2E§:
               _loc2_ = 7702598;
               break;
            case §_-21w§.§_-G2M§:
               _loc2_ = 10850398;
               break;
            case §_-21w§.§_-jc§:
               _loc2_ = 4032410;
               break;
            case §_-21w§.§_-l1e§:
               _loc2_ = 4806787;
               break;
            case §_-21w§.§_-43d§:
               _loc2_ = 10125402;
               break;
            default:
               _loc2_ = 0;
         }
         return _loc2_;
      }
      
      private function changeColor(param1:Array, param2:uint) : void
      {
         var _loc3_:DisplayObject = null;
         if(param1)
         {
            for each(_loc3_ in param1)
            {
               this.§_-o2l§(_loc3_,param2);
            }
         }
      }
      
      private function §_-o2l§(param1:DisplayObject, param2:uint) : void
      {
         var _loc3_:DisplayObjectContainer = null;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:Image = null;
         if(param1 is DisplayObjectContainer)
         {
            _loc3_ = param1 as DisplayObjectContainer;
            _loc4_ = _loc3_.numChildren;
            _loc5_ = 0;
            while(_loc5_ < _loc4_)
            {
               this.§_-o2l§(_loc3_.getChildAt(_loc5_),param2);
               _loc5_++;
            }
         }
         else if(Boolean(param1 is Image) && Boolean(param1.name) && param1.name.indexOf("blick") != -1)
         {
            _loc6_ = param1 as Image;
            _loc6_.color = param2;
            _loc6_.touchable = false;
         }
      }
   }
}

