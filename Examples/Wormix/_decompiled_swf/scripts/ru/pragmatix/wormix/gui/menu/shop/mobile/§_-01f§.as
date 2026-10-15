package ru.pragmatix.wormix.gui.menu.shop.mobile
{
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-zI§.*;
   import config.§_-B1K§;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.events.FeathersEventType;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-01f§ extends AbstractShopMenu implements §_-X1Y§
   {
      
      private var §_-q2u§:ScreenNavigator;
      
      private var §_-H27§:Array;
      
      public function §_-01f§()
      {
         super();
         this.§_-H27§ = [];
      }
      
      override protected function initialize() : void
      {
         var _loc1_:LayoutGroup = null;
         var _loc2_:ScreenNavigatorItem = null;
         super.initialize();
         _loc1_ = §_-ie§();
         this.§_-q2u§ = new ScreenNavigator();
         _loc2_ = new ScreenNavigatorItem(§_-IC§);
         this.§_-q2u§.addScreen(§_-QR§.§_-G31§,_loc2_);
         this.§_-q2u§.width = _loc1_.width;
         this.§_-q2u§.height = _loc1_.height;
         _loc1_.addChild(this.§_-q2u§);
         this.§_-92B§(this.§_-q2u§,_loc2_,§_-QR§.§_-I22§,§_-B1K§.§_-c1c§);
         this.§_-92B§(this.§_-q2u§,_loc2_,§_-QR§.§_-H3F§,§_-B1K§.§_-C23§);
         this.§_-92B§(this.§_-q2u§,_loc2_,§_-QR§.§_-G2q§,§_-B1K§.§_-A2L§);
         this.§_-92B§(this.§_-q2u§,_loc2_,§_-QR§.GADGETS,§_-B1K§.§_-x1w§);
         this.§_-92B§(this.§_-q2u§,_loc2_,§_-QR§.HATS,§_-B1K§.HAT);
         this.§_-92B§(this.§_-q2u§,_loc2_,§_-QR§.ARTIFACTS,§_-B1K§.ARTIFACT);
         this.§_-q2u§.addEventListener(FeathersEventType.TRANSITION_COMPLETE,this.§_-l2T§);
         this.§_-q2u§.showScreen(§_-QR§.§_-G31§);
         setTitle(§_-s2a§.§_-K3t§(§_-s2a§.SHOP));
      }
      
      private function §_-92B§(param1:ScreenNavigator, param2:ScreenNavigatorItem, param3:String, param4:int) : void
      {
         param2.setScreenIDForEvent(param3,param3);
         var _loc5_:LayoutGroup = §_-ie§();
         var _loc6_:ScreenNavigatorItem = new ScreenNavigatorItem(§_-p1d§);
         _loc6_.properties.panelId = param4;
         _loc6_.properties.listWidth = _loc5_.width;
         _loc6_.properties.listHeight = _loc5_.height;
         param1.addScreen(param3,_loc6_);
      }
      
      private function §_-l2T§(param1:Event) : void
      {
         §_-x16§(this.isSubpanelOpened);
         this.§_-A2N§();
         if(Boolean(this.§_-q2u§.activeScreen) && this.§_-q2u§.activeScreen is §_-p1d§)
         {
            (this.§_-q2u§.activeScreen as §_-p1d§).§_-83t§ = this.§_-H27§[this.§_-q2u§.activeScreenID];
         }
      }
      
      private function §_-A2N§() : void
      {
         var _loc1_:String = null;
         switch(this.§_-q2u§.activeScreenID)
         {
            case §_-QR§.§_-I22§:
               _loc1_ = §_-s2a§.LONG_RANGE_WEAPONS;
               break;
            case §_-QR§.§_-H3F§:
               _loc1_ = §_-s2a§.CLOSE_RANGE_WEAPONS;
               break;
            case §_-QR§.§_-G2q§:
               _loc1_ = §_-s2a§.SPECIAL_WEAPONS;
               break;
            case §_-QR§.GADGETS:
               _loc1_ = §_-s2a§.GADGETS;
               break;
            case §_-QR§.HATS:
               _loc1_ = §_-s2a§.HATS;
               break;
            case §_-QR§.ARTIFACTS:
               _loc1_ = §_-s2a§.ARTIFACTS;
               break;
            default:
               _loc1_ = §_-s2a§.SHOP;
         }
         setTitle(§_-s2a§.§_-K3t§(_loc1_));
      }
      
      override public function get isSubpanelOpened() : Boolean
      {
         return this.§_-q2u§.activeScreenID != §_-QR§.§_-G31§;
      }
      
      override public function §_-52u§(param1:Event = null) : void
      {
         if(Boolean(this.§_-q2u§.activeScreen) && this.§_-q2u§.activeScreen is §_-p1d§)
         {
            this.§_-H27§[this.§_-q2u§.activeScreenID] = (this.§_-q2u§.activeScreen as §_-p1d§).§_-83t§;
         }
         super.§_-52u§(param1);
         this.§_-q2u§.showScreen(§_-QR§.§_-G31§);
      }
      
      public function update(param1:int) : void
      {
         var _loc2_:§_-p1d§ = null;
         if(this.§_-q2u§.activeScreenID != §_-QR§.§_-G31§)
         {
            _loc2_ = this.§_-q2u§.activeScreen as §_-p1d§;
            _loc2_.update(param1);
         }
      }
      
      override public function dispose() : void
      {
         if(this.§_-q2u§)
         {
            this.§_-q2u§.removeEventListener(FeathersEventType.TRANSITION_COMPLETE,this.§_-l2T§);
         }
         this.§_-q2u§ = §_-c1S§.dispose(this.§_-q2u§) as ScreenNavigator;
         super.dispose();
      }
   }
}

