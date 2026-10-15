package ru.pragmatix.wormix.utils
{
   import §_-62§.*;
   import §_-73d§.§_-pT§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.*;
   import §_-B1y§.§_-ts§;
   import §_-B2z§.§_-63i§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H3§.*;
   import §_-Ly§.§_-81L§;
   import §_-hO§.§_-03k§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-lD§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-t2x§.§_-B3y§;
   import §_-t2x§.§_-H12§;
   import §_-w2W§.§_-v27§;
   import com.distriqt.extension.cloudstorage.CloudStorage;
   import com.hurlant.math.BigInteger;
   import config.§_-vR§;
   import feathers.controls.ScrollPolicy;
   import feathers.core.FeathersControl;
   import flash.desktop.NativeApplication;
   import flash.display.Stage;
   import flash.events.Event;
   import flash.events.KeyboardEvent;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.ui.Keyboard;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreated;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillMissile;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillStanding;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-o19§
   {
      
      private var §_-vh§:Boolean = false;
      
      private var §_-12X§:§_-H12§;
      
      public function §_-o19§()
      {
         super();
         this.§_-vh§ = true;
         this.§_-12X§ = §_-B3y§.§_-H1W§();
         §_-g1p§(Starling.current.nativeStage,this.onStageActivated,this.onStageDeactivated,this.§_-q22§);
      }
      
      public static function §_-g1p§(param1:Stage, param2:Function, param3:Function, param4:Function) : void
      {
         if(SystemUtil.isAIR)
         {
            NativeApplication.nativeApplication.addEventListener(flash.events.Event.ACTIVATE,param2);
            NativeApplication.nativeApplication.addEventListener(flash.events.Event.DEACTIVATE,param3);
            NativeApplication.nativeApplication.addEventListener(flash.events.Event.SUSPEND,param4);
            NativeApplication.nativeApplication.addEventListener(KeyboardEvent.KEY_DOWN,§_-1M§);
         }
         else
         {
            param1.addEventListener(flash.events.Event.ACTIVATE,param2);
            param1.addEventListener(flash.events.Event.DEACTIVATE,param3);
         }
      }
      
      private static function §_-1M§(param1:KeyboardEvent) : void
      {
         if(param1.keyCode == Keyboard.S && param1.ctrlKey)
         {
            param1.preventDefault();
         }
      }
      
      public function §_-F1x§() : void
      {
         this.§_-12X§.§_-F1x§();
      }
      
      private function onStageActivated(param1:flash.events.Event) : void
      {
         if(§_-YD§.§_-t1O§ && §_-vR§.§_-pD§)
         {
            return;
         }
         if(!this.§_-vh§)
         {
            this.§_-vh§ = true;
            if(§_-X1j§.§_-dr§)
            {
               §_-X1j§.§_-dr§.§_-214§();
            }
            this.§_-12X§.onStageActivated();
         }
         Application.§_-i1N§.dispatchEventWith(flash.events.Event.ACTIVATE);
      }
      
      private function onStageDeactivated(param1:flash.events.Event) : void
      {
         if(§_-YD§.§_-t1O§ && §_-vR§.§_-pD§)
         {
            return;
         }
         if(this.§_-vh§)
         {
            this.§_-vh§ = false;
            this.§_-12X§.onStageDeactivated();
         }
         Application.§_-i1N§.dispatchEventWith(flash.events.Event.DEACTIVATE);
      }
      
      private function §_-q22§(param1:flash.events.Event) : void
      {
         this.§_-12X§.§_-q22§();
         Application.§_-i1N§.dispatchEventWith(flash.events.Event.SUSPEND);
      }
      
      public function get §_-b2l§() : Boolean
      {
         return this.§_-vh§;
      }
   }
}

