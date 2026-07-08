package ru.pragmatix.wormix.gui.screenOverlay
{
   import §_-03T§.§_-2p§;
   import §_-21p§.§_-N1H§;
   import §_-62§.§_-O2H§;
   import §_-G1h§.DesktopMainOverlayScreen;
   import §_-K3r§.§_-E3O§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2W§.§_-21w§;
   import §_-z1g§.§_-lp§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import feathers.controls.Screen;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.events.FeathersEventType;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.MathUtil;
   
   public class §_-W5§ extends ScreenNavigator
   {
      
      private static const MAIN_OVERLAY:String = "MAIN_OVERLAY";
      
      private static const EMPTY_OVERLAY:String = "EMPTY_OVERLAY";
      
      private var §_-E1k§:§_-E3O§;
      
      public function §_-W5§(param1:§_-E3O§)
      {
         super();
         this.§_-E1k§ = param1;
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-E1k§.addEventListener(FeathersEventType.CLEAR,this.§_-Y1A§);
         this.§_-E1k§.addEventListener(Event.CHANGE,this.§_-e2P§);
         var _loc1_:ScreenNavigatorItem = new ScreenNavigatorItem(DesktopMainOverlayScreen);
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

