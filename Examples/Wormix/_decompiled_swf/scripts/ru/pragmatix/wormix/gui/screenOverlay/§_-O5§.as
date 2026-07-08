package ru.pragmatix.wormix.gui.screenOverlay
{
   import §_-5Y§.§_-h2c§;
   import §_-62§.*;
   import §_-G1h§.MobileMainOverlayScreen;
   import §_-G2H§.*;
   import §_-K3r§.§_-E3O§;
   import §_-TF§.§_-q1j§;
   import §_-Z1K§.§_-q24§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-vO§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-H3D§;
   import §_-w2W§.§_-21w§;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import feathers.controls.Screen;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.events.FeathersEventType;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.response.PromoteRankInClanResponse;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-O5§ extends ScreenNavigator
   {
      
      private static const MAIN_OVERLAY:String = "MAIN_OVERLAY";
      
      private static const EMPTY_OVERLAY:String = "EMPTY_OVERLAY";
      
      private var §_-E1k§:§_-E3O§;
      
      public function §_-O5§(param1:§_-E3O§)
      {
         super();
         this.§_-E1k§ = param1;
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-E1k§.addEventListener(FeathersEventType.CLEAR,this.§_-Y1A§);
         this.§_-E1k§.addEventListener(Event.CHANGE,this.§_-e2P§);
         var _loc1_:ScreenNavigatorItem = new ScreenNavigatorItem(MobileMainOverlayScreen);
         this.addScreen(MAIN_OVERLAY,_loc1_);
         var _loc2_:ScreenNavigatorItem = new ScreenNavigatorItem(Screen);
         this.addScreen(EMPTY_OVERLAY,_loc2_);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         if(this.§_-E1k§)
         {
            this.§_-E1k§.addEventListener(FeathersEventType.CLEAR,this.§_-Y1A§);
            this.§_-E1k§.addEventListener(Event.CHANGE,this.§_-e2P§);
            this.§_-E1k§ = null;
         }
      }
      
      private function §_-e2P§(param1:Event) : void
      {
         switch(this.§_-E1k§.activeScreenID)
         {
            case §_-21w§.§_-G31§:
               Application.§_-SH§.§_-yJ§(Application.userProfile);
               showScreen(MAIN_OVERLAY,this.§_-E1k§.transition);
               break;
            case §_-21w§.§_-F3d§:
            case §_-21w§.§_-H2E§:
            case §_-21w§.§_-G2M§:
            case §_-21w§.§_-jc§:
            case §_-21w§.§_-l1e§:
            case §_-21w§.§_-43d§:
               showScreen(MAIN_OVERLAY,this.§_-E1k§.transition);
               break;
            case §_-21w§.BATTLE_CLEANING:
            case §_-21w§.BATTLE_LOADING:
               showScreen(EMPTY_OVERLAY,this.§_-E1k§.transition);
               break;
            default:
               clearScreen();
         }
      }
      
      private function §_-Y1A§(param1:Event) : void
      {
         clearScreen();
      }
   }
}

