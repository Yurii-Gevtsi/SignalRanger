package ru.pragmatix.wormix.controls.system
{
   import flash.ui.GameInputDevice;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controller.§_-Z2p§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.CommandState;
   import ru.pragmatix.wormix.controls.GamepadControlsHandler;
   import ru.pragmatix.wormix.controls.GamepadControlsMap;
   import ru.pragmatix.wormix.controls.GamepadControlsMapData;
   import starling.events.Event;
   
   public class GamepadControlSystem extends AbstractControlSystem
   {
      
      public static const NAME:String = "Joystick";
      
      private var deviceId:String;
      
      private var device:GameInputDevice;
      
      private var controlsMap:GamepadControlsMap;
      
      private var handler:GamepadControlsHandler;
      
      public function GamepadControlSystem(param1:String)
      {
         super(NAME);
         this.deviceId = param1;
      }
      
      override protected function activate() : void
      {
         this.activateDevice();
         this.controlsMap = §_-X1j§.§_-dr§.§_-N2z§(this.deviceId);
         §_-X1j§.§_-dr§.addEventListener(§_-Z2p§.§_-eM§,this.onDeviceAdded);
         §_-X1j§.§_-dr§.addEventListener(§_-Z2p§.§_-m3§,this.onDeviceRemoved);
      }
      
      override protected function deactivate() : void
      {
         §_-X1j§.§_-dr§.removeEventListener(§_-Z2p§.§_-eM§,this.onDeviceAdded);
         §_-X1j§.§_-dr§.removeEventListener(§_-Z2p§.§_-m3§,this.onDeviceRemoved);
         this.deactivateDevice();
      }
      
      private function onDeviceAdded(param1:Event) : void
      {
         if(param1.data.id == this.deviceId)
         {
            if(!this.device)
            {
               this.activateDevice();
            }
            else
            {
               this.device.enabled = true;
            }
         }
      }
      
      private function onDeviceRemoved(param1:Event) : void
      {
         if(param1.data.id == this.deviceId)
         {
            this.deactivateDevice();
         }
      }
      
      private function activateDevice() : void
      {
         this.device = §_-X1j§.§_-dr§.§_-sl§(this.deviceId);
         if(!this.device)
         {
            return;
         }
         this.handler = new GamepadControlsHandler(this.device);
         this.listenToChangeEvent(false);
         this.handler.activate();
      }
      
      private function deactivateDevice() : void
      {
         if(!this.device)
         {
            return;
         }
         this.listenToChangeEvent(true);
         this.handler.dispose();
         this.handler = null;
         this.device = null;
         this.endExecutingCommand();
      }
      
      override public function pause(param1:Boolean) : void
      {
         super.pause(param1);
         if(!this.handler)
         {
            return;
         }
         this.listenToChangeEvent(param1);
      }
      
      private function listenToChangeEvent(param1:Boolean) : void
      {
         if(param1)
         {
            this.handler.removeEventListener(GamepadControlsHandler.EVENT,this.onControlChange);
         }
         else
         {
            this.handler.addEventListener(GamepadControlsHandler.EVENT,this.onControlChange);
         }
      }
      
      private function endExecutingCommand() : void
      {
      }
      
      private function onControlChange(param1:Event) : void
      {
         var _loc3_:Command = null;
         var _loc2_:String = GamepadControlsMapData.getKey(param1.data.id,param1.data.sign);
         if(_loc2_)
         {
            _loc3_ = this.controlsMap.matchByKey(_loc2_);
            if(_loc3_)
            {
               dispatchCommand(_loc3_,param1.data.begin ? CommandState.BEGIN : CommandState.END);
            }
         }
      }
   }
}

