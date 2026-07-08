package ru.pragmatix.wormix.gui.menu.shop.mobile
{
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-Tm§.§_-RF§;
   import §_-Y1O§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import config.§_-B1K§;
   import feathers.controls.List;
   import feathers.controls.Screen;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-IC§ extends Screen
   {
      
      private var §_-J37§:List;
      
      public function §_-IC§()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         layout = new AnchorLayout();
         this.§_-J37§ = new List();
         this.§_-J37§.layoutData = new AnchorLayoutData(0,0,0,0);
         addChild(this.§_-J37§);
         this.§_-J37§.itemRendererType = MobileShopCatecoryItemRenderer;
         var _loc1_:TiledRowsLayout = new TiledRowsLayout();
         _loc1_.useSquareTiles = false;
         _loc1_.requestedRowCount = 2;
         _loc1_.distributeHeights = false;
         _loc1_.distributeWidths = false;
         _loc1_.horizontalGap = 10;
         _loc1_.verticalGap = 10;
         _loc1_.horizontalAlign = HorizontalAlign.CENTER;
         _loc1_.verticalAlign = VerticalAlign.MIDDLE;
         this.§_-J37§.layout = _loc1_;
         this.§_-J37§.horizontalScrollPolicy = ScrollPolicy.OFF;
         this.§_-J37§.verticalScrollPolicy = ScrollPolicy.OFF;
         var _loc2_:Array = [{
            "title":§_-s2a§.LONG_RANGE_WEAPONS,
            "category":§_-B1K§.§_-c1c§,
            "icon":"ShopMenuBase_shopPanel_tab1_icon",
            "screenId":§_-QR§.§_-I22§
         },{
            "title":§_-s2a§.CLOSE_RANGE_WEAPONS,
            "category":§_-B1K§.§_-C23§,
            "icon":"ShopMenuBase_shopPanel_tab2_icon",
            "screenId":§_-QR§.§_-H3F§
         },{
            "title":§_-s2a§.SPECIAL_WEAPONS,
            "category":§_-B1K§.§_-A2L§,
            "icon":"ShopMenuBase_shopPanel_tab3_icon",
            "screenId":§_-QR§.§_-G2q§
         },{
            "title":§_-s2a§.GADGETS,
            "category":§_-B1K§.§_-x1w§,
            "icon":"ShopMenuBase_shopPanel_tab4_icon",
            "screenId":§_-QR§.GADGETS
         },{
            "title":§_-s2a§.HATS,
            "category":§_-B1K§.HAT,
            "icon":"ShopMenuBase_shopPanel_tab5_icon",
            "screenId":§_-QR§.HATS
         },{
            "title":§_-s2a§.ARTIFACTS,
            "category":§_-B1K§.ARTIFACT,
            "icon":"ShopMenuBase_shopPanel_tab6_icon",
            "screenId":§_-QR§.ARTIFACTS
         }];
         this.§_-J37§.dataProvider = new ArrayCollection(_loc2_);
         this.§_-J37§.addEventListener(Event.CHANGE,this.§_-b5§);
      }
      
      private function §_-b5§(param1:Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         if(this.§_-J37§.selectedItem)
         {
            dispatchEventWith(this.§_-J37§.selectedItem.screenId);
         }
      }
   }
}

