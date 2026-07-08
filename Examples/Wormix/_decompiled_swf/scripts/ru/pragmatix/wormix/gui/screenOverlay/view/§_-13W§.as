package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-L1H§.§_-C3e§;
   import com.distriqt.extension.inappbilling.Purchase;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.model.user.§_-O1f§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class §_-13W§ implements §_-O1f§
   {
      
      private var _elements:Array;
      
      private var §_-O24§:§_-r1d§;
      
      public function §_-13W§()
      {
         super();
         this._elements = [];
      }
      
      public function dispose() : void
      {
         this.§_-Y2v§();
         this._elements = null;
      }
      
      public function §_-E2V§(param1:DisplayObject, param2:Boolean = true) : void
      {
         this._elements.push({
            "element":param1,
            "hideWhenGuestProfile":param2
         });
      }
      
      public function §_-32X§(param1:§_-r1d§) : void
      {
         this.§_-Y2v§();
         this.§_-O24§ = param1;
         if(this.§_-O24§)
         {
            this.§_-O24§.addEventListener(Event.CHANGE,this.refresh);
            this.§_-O24§.addEventListener(Event.UPDATE,this.refresh);
         }
         this.refresh();
      }
      
      private function §_-Y2v§() : void
      {
         if(this.§_-O24§)
         {
            this.§_-O24§.removeEventListener(Event.CHANGE,this.refresh);
            this.§_-O24§.removeEventListener(Event.UPDATE,this.refresh);
            this.§_-O24§ = null;
         }
      }
      
      private function refresh(param1:Event = null) : void
      {
         var _loc2_:DisplayObject = null;
         var _loc3_:Boolean = false;
         var _loc4_:Object = null;
         if(this.§_-O24§)
         {
            _loc3_ = this.§_-O24§.§_-x2k§;
            for each(_loc4_ in this._elements)
            {
               _loc2_ = _loc4_.element as DisplayObject;
               if(_loc2_)
               {
                  _loc2_.visible = _loc3_ ? true : !_loc4_.hideWhenGuestProfile;
               }
            }
         }
      }
   }
}

