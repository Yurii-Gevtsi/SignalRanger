package ru.pragmatix.wormix.controller
{
   import §_-21p§.§_-4w§;
   import §_-5P§.*;
   import §_-73I§.§_-K2s§;
   import §_-73d§.§_-pT§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.Cookie;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-92W§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-OJ§.§_-L3B§;
   import §_-P1B§.§_-b21§;
   import §_-P1B§.§_-bb§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-RF§;
   import §_-Vu§.MD5;
   import §_-W§.§_-Ap§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1b§.§_-81l§;
   import §_-aM§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-p14§.§_-U1D§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-v2t§.§_-73F§;
   import §_-w2i§.§_-Zd§;
   import §_-wD§.*;
   import §_-zI§.*;
   import com.hurlant.util.der.§_-p1B§;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Panel;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.events.FeathersEventType;
   import feathers.themes.FontSize;
   import flash.display.Shape;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.events.GameInputEvent;
   import flash.events.TimerEvent;
   import flash.geom.Rectangle;
   import flash.system.LoaderContext;
   import flash.text.TextFormatAlign;
   import flash.ui.GameInput;
   import flash.ui.GameInputDevice;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.ICustomInviter;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controls.GamepadControlsMap;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.BossTypesTab;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.QuitClanResponse;
   import ru.pragmatix.wormix.messages.client.MobileLogin;
   import ru.pragmatix.wormix.messages.client.PvpChatMessage;
   import ru.pragmatix.wormix.messages.client.SteamProductsInfoRequest;
   import ru.pragmatix.wormix.messages.server.AchieveLoginError;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class §_-Z2p§ extends EventDispatcher
   {
      
      public static const §_-eM§:String = "onDeviceAdded";
      
      public static const §_-m3§:String = "onDeviceRemoved";
      
      private static const GAMEPADS_CONTROLS_MAPS:String = "GAMEPADS_CONTROLS_MAPS";
      
      private var §_-22l§:GameInput;
      
      private var §_-H19§:Object = {};
      
      private var §_-c29§:Dictionary = new Dictionary();
      
      private var §_-Sj§:Object = {};
      
      private var §_-71T§:int = 0;
      
      public function §_-Z2p§()
      {
         super();
         this.§_-22l§ = new GameInput();
         this.§_-22l§.addEventListener(GameInputEvent.DEVICE_ADDED,this.onDeviceAdded);
         this.§_-22l§.addEventListener(GameInputEvent.DEVICE_REMOVED,this.onDeviceRemoved);
         this.§_-M2h§();
         this.§_-O1l§();
      }
      
      private function onDeviceAdded(param1:GameInputEvent) : void
      {
         this.§_-jQ§(param1.device);
      }
      
      private function onDeviceRemoved(param1:GameInputEvent) : void
      {
         this.§_-52E§(param1.device);
      }
      
      private function §_-jQ§(param1:GameInputDevice) : void
      {
         if(!param1)
         {
            return;
         }
         if(!this.§_-H19§[param1.id])
         {
            ++this.§_-71T§;
         }
         §_-vm§.initialize(param1);
         this.§_-c29§[param1] = param1.id;
         this.§_-H19§[param1.id] = param1;
         if(!this.§_-Sj§[param1.name])
         {
            this.§_-Sj§[param1.name] = GamepadControlsMap.getDefaultMapForDevice(param1);
         }
         dispatchEventWith(§_-eM§,false,{"id":param1.id});
      }
      
      private function §_-52E§(param1:GameInputDevice) : void
      {
         var _loc2_:String = this.§_-c29§[param1] as String;
         if(Boolean(_loc2_) && Boolean(this.§_-H19§[_loc2_]))
         {
            delete this.§_-H19§[_loc2_];
            --this.§_-71T§;
            dispatchEventWith(§_-m3§,false,{"id":_loc2_});
         }
      }
      
      private function §_-O1l§() : void
      {
         var _loc1_:int = 0;
         while(_loc1_ < GameInput.numDevices)
         {
            this.§_-jQ§(GameInput.getDeviceAt(_loc1_));
            _loc1_++;
         }
      }
      
      public function §_-214§() : void
      {
         this.§_-O1l§();
      }
      
      public function §_-sl§(param1:String) : GameInputDevice
      {
         return this.§_-H19§[param1] as GameInputDevice;
      }
      
      public function §_-73Z§() : Vector.<GameInputDevice>
      {
         var _loc2_:GameInputDevice = null;
         var _loc1_:Vector.<GameInputDevice> = new Vector.<GameInputDevice>();
         for each(_loc2_ in this.§_-H19§)
         {
            _loc1_.push(_loc2_);
         }
         return _loc1_;
      }
      
      public function get numDevices() : int
      {
         return this.§_-71T§;
      }
      
      public function §_-L1j§(param1:GameInputDevice, param2:GamepadControlsMap) : void
      {
         this.§_-Sj§[param1.name] = param2;
         this.§_-Bg§();
      }
      
      public function §_-N2z§(param1:String) : GamepadControlsMap
      {
         var _loc2_:GameInputDevice = this.§_-sl§(param1);
         return _loc2_ ? this.§_-Sj§[_loc2_.name] as GamepadControlsMap : null;
      }
      
      private function §_-Bg§() : void
      {
         var _loc2_:GamepadControlsMap = null;
         var _loc1_:Array = [];
         for each(_loc2_ in this.§_-Sj§)
         {
            _loc1_.push(_loc2_.serialize());
         }
         Cookie.setValue(GAMEPADS_CONTROLS_MAPS,_loc1_);
      }
      
      private function §_-M2h§() : void
      {
         var _loc4_:GamepadControlsMap = null;
         var _loc1_:Array = Cookie.getValue(GAMEPADS_CONTROLS_MAPS,[]) as Array;
         var _loc2_:int = 0;
         var _loc3_:int = int(_loc1_.length);
         while(_loc2_ < _loc3_)
         {
            _loc4_ = GamepadControlsMap.deserialize(_loc1_[_loc2_]);
            if(_loc4_)
            {
               this.§_-Sj§[_loc4_.deviceName] = _loc4_;
            }
            _loc2_++;
         }
      }
      
      public function dispose() : void
      {
         this.§_-22l§.removeEventListener(GameInputEvent.DEVICE_ADDED,this.onDeviceAdded);
         this.§_-22l§.removeEventListener(GameInputEvent.DEVICE_REMOVED,this.onDeviceRemoved);
         this.§_-22l§ = null;
         this.§_-H19§ = null;
      }
   }
}

