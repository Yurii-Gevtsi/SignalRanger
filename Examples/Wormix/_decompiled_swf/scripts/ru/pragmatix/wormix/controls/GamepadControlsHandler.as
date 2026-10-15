package ru.pragmatix.wormix.controls
{
   import flash.events.Event;
   import flash.ui.GameInputControl;
   import flash.ui.GameInputDevice;
   import flash.utils.Dictionary;
   import starling.core.Starling;
   import starling.events.EventDispatcher;
   import starling.events.KeyboardEvent;
   
   public class GamepadControlsHandler extends EventDispatcher
   {
      
      public static const EVENT:String = "GAMEPAD_CONTROL_EVENT";
      
      private static const CONTROLS_SENSITIVITY:Number = 0.8;
      
      private var device:GameInputDevice;
      
      private var controlIsPressed:Object = {};
      
      private var controlsAreListening:Dictionary = new Dictionary();
      
      private var helper:Object = {};
      
      public function GamepadControlsHandler(param1:GameInputDevice)
      {
         super();
         this.device = param1;
      }
      
      public function activate() : void
      {
         var _loc3_:GameInputControl = null;
         var _loc1_:int = 0;
         var _loc2_:int = this.device.numControls;
         while(_loc1_ < _loc2_)
         {
            _loc3_ = this.device.getControlAt(_loc1_);
            _loc3_.addEventListener(Event.CHANGE,this.onControlChange);
            this.controlsAreListening[_loc3_] = 1;
            _loc1_++;
         }
         Starling.current.stage.addEventListener(KeyboardEvent.KEY_DOWN,this.onKeyControlDown);
         Starling.current.stage.addEventListener(KeyboardEvent.KEY_UP,this.onKeyControlUp);
         this.device.enabled = true;
      }
      
      public function deactivate() : void
      {
         var _loc1_:Object = null;
         var _loc2_:GameInputControl = null;
         for(_loc1_ in this.controlsAreListening)
         {
            _loc2_ = _loc1_ as GameInputControl;
            if(_loc2_)
            {
               _loc2_.removeEventListener(Event.CHANGE,this.onControlChange);
            }
         }
         Starling.current.stage.removeEventListener(KeyboardEvent.KEY_DOWN,this.onKeyControlDown);
         Starling.current.stage.removeEventListener(KeyboardEvent.KEY_UP,this.onKeyControlUp);
         this.device.enabled = false;
         this.controlsAreListening = new Dictionary();
         this.controlIsPressed = {};
      }
      
      private function onControlChange(param1:Event) : void
      {
         var _loc3_:Number = NaN;
         var _loc4_:String = null;
         var _loc5_:Vector.<String> = null;
         var _loc2_:GameInputControl = param1.target as GameInputControl;
         if(_loc2_)
         {
            _loc3_ = Math.abs(_loc2_.value);
            if(_loc3_ > CONTROLS_SENSITIVITY)
            {
               _loc4_ = GamepadControlsMapData.getKey(_loc2_.id,_loc2_.value);
               if(!this.controlIsPressed[_loc4_])
               {
                  this.parseAndDispatchCommand(_loc4_,true);
                  this.controlIsPressed[_loc4_] = 1;
               }
            }
            else
            {
               _loc5_ = GamepadControlsMapData.getKeysByControlId(_loc2_.id);
               for each(_loc4_ in _loc5_)
               {
                  if(this.controlIsPressed[_loc4_])
                  {
                     this.parseAndDispatchCommand(_loc4_,false);
                     this.controlIsPressed[_loc4_] = 0;
                  }
               }
            }
         }
      }
      
      private function onKeyControlDown(param1:KeyboardEvent) : void
      {
         this.saveAndDispatchKeyCommand(param1,true);
      }
      
      private function onKeyControlUp(param1:KeyboardEvent) : void
      {
         this.saveAndDispatchKeyCommand(param1,false);
      }
      
      private function saveAndDispatchKeyCommand(param1:KeyboardEvent, param2:Boolean) : void
      {
         var _loc3_:int = param2 ? 1 : 0;
         var _loc4_:String = this.createIdFromKeyData(param1);
         if(this.controlIsPressed[_loc4_] != _loc3_)
         {
            this.helper.id = _loc4_;
            this.helper.sign = 1;
            this.dispatchParsedCommand(param2);
            this.controlIsPressed[_loc4_] = _loc3_;
         }
      }
      
      private function createIdFromKeyData(param1:KeyboardEvent) : String
      {
         return (param1.altKey ? "a" : "").concat(param1.charCode).concat(param1.ctrlKey ? "ctrl" : "").concat(param1.keyCode).concat(param1.keyLocation).concat(param1.shiftKey ? "shift" : "");
      }
      
      private function parseAndDispatchCommand(param1:String, param2:Boolean) : void
      {
         GamepadControlsMapData.parseKey(param1,this.helper);
         this.dispatchParsedCommand(param2);
      }
      
      private function dispatchParsedCommand(param1:Boolean) : void
      {
         dispatchEventWith(EVENT,false,{
            "id":this.helper.id,
            "sign":this.helper.sign,
            "begin":param1
         });
      }
      
      public function dispose() : void
      {
         this.deactivate();
         this.device = null;
      }
   }
}

