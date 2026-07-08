package ru.pragmatix.wormix.gui.menu.shop.mobile
{
   import §_-62§.*;
   import §_-73d§.§_-pT§;
   import §_-A1K§.§_-7u§;
   import §_-B1y§.§_-ts§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-lD§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-d2j§;
   import §_-u1P§.§_-Po§;
   import feathers.controls.Screen;
   import feathers.data.ListCollection;
   import feathers.layout.VerticalLayout;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class §_-p1d§ extends Screen
   {
      
      protected var §_-T1b§:int;
      
      protected var §_-213§:Number;
      
      protected var §_-E1v§:Number;
      
      protected var §_-vU§:§_-VD§;
      
      public function §_-p1d§()
      {
         super();
         var _loc1_:VerticalLayout = new VerticalLayout();
         _loc1_.padding = 0;
         this.layout = _loc1_;
         if(Application.userProfile.§_-33n§())
         {
            Application.userProfile.§_-pk§().addEventListener(Event.COMPLETE,this.§_-yL§);
         }
      }
      
      private function §_-yL§(param1:Event) : void
      {
         invalidate(INVALIDATION_FLAG_DATA);
      }
      
      public function get panelId() : int
      {
         return this.§_-T1b§;
      }
      
      public function set panelId(param1:int) : void
      {
         if(this.§_-T1b§ == param1)
         {
            return;
         }
         this.§_-T1b§ = param1;
         this.invalidate(INVALIDATION_FLAG_DATA);
      }
      
      override protected function draw() : void
      {
         if(!this.§_-vU§)
         {
            this.§_-vU§ = this.createList();
            this.§_-vU§.addEventListener(Event.CHANGE,this.§_-4M§);
            this.§_-zO§();
         }
         var _loc1_:Array = §_-G1p§.§_-O1b§(this.§_-T1b§);
         §_-ou§.§_-au§(_loc1_);
         this.§_-vU§.dataProvider = new ListCollection(_loc1_);
      }
      
      private function §_-4M§(param1:Event) : void
      {
         if(this.§_-vU§.selectedItem)
         {
            dispatchEvent(new §_-Po§(this.§_-vU§.selectedItem as §_-W1r§,(this.§_-vU§.dataProvider as ListCollection).data as Array,this.§_-vU§.selectedIndex));
            this.§_-vU§.selectedItem = null;
         }
      }
      
      public function update(param1:int) : void
      {
         this.§_-vU§.invalidate(INVALIDATION_FLAG_DATA);
         this.§_-vU§.dataProvider.updateItemAt(this.getItemIndex(param1));
      }
      
      private function getItemIndex(param1:int) : int
      {
         var _loc2_:* = this.§_-vU§.dataProvider.length;
         while(_loc2_--)
         {
            if(this.§_-vU§.dataProvider.getItemAt(_loc2_).getId() == param1)
            {
               return _loc2_;
            }
         }
         return -1;
      }
      
      private function createList() : §_-VD§
      {
         var _loc1_:§_-VD§ = new §_-VD§();
         addChild(_loc1_);
         return _loc1_;
      }
      
      public function get listWidth() : Number
      {
         return this.§_-213§;
      }
      
      public function set listWidth(param1:Number) : void
      {
         if(this.§_-213§ == param1)
         {
            return;
         }
         this.§_-213§ = param1;
         if(this.§_-vU§)
         {
            this.§_-zO§();
         }
      }
      
      public function get listHeight() : Number
      {
         return this.§_-E1v§;
      }
      
      public function set listHeight(param1:Number) : void
      {
         if(this.§_-E1v§ == param1)
         {
            return;
         }
         this.§_-E1v§ = param1;
         if(this.§_-vU§)
         {
            this.§_-zO§();
         }
      }
      
      private function §_-zO§() : void
      {
         this.§_-vU§.width = this.§_-213§;
         this.§_-vU§.height = this.§_-E1v§;
      }
      
      public function get §_-83t§() : Number
      {
         return this.§_-vU§.horizontalScrollPosition;
      }
      
      public function set §_-83t§(param1:Number) : void
      {
         if(isNaN(param1))
         {
            param1 = 0;
         }
         param1 = Math.min(param1,this.§_-vU§.maxHorizontalScrollPosition);
         this.§_-vU§.scrollToPosition(param1,0,0);
      }
      
      override public function dispose() : void
      {
         if(this.§_-vU§)
         {
            this.§_-vU§.removeEventListener(Event.CHANGE,this.§_-4M§);
            this.§_-vU§.dispose();
            this.§_-vU§ = null;
         }
         if(Application.userProfile.§_-33n§())
         {
            Application.userProfile.§_-pk§().removeEventListener(Event.COMPLETE,this.§_-yL§);
         }
         super.dispose();
      }
   }
}

