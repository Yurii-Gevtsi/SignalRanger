package ru.pragmatix.wormix.scene.camera.controls
{
   import flash.ui.Keyboard;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.scene.camera.*;
   import starling.animation.IAnimatable;
   import starling.core.Starling;
   import starling.events.KeyboardEvent;
   
   public class CameraKeyboardControls implements IAnimatable, ICameraControl
   {
      
      private static const CAMERA_MOVE_SPEED:int = 400;
      
      private static const CAMERA_SCALE_SPEED:Number = 1;
      
      private var camera:SceneCamera;
      
      private var pressedKeys:Dictionary;
      
      private var posChangeX:Number = 0;
      
      private var posChangeY:Number = 0;
      
      private var scaleChange:Number = 0;
      
      public function CameraKeyboardControls(param1:SceneCamera)
      {
         super();
         this.camera = param1;
         this.pressedKeys = new Dictionary();
         this.pressedKeys[Keyboard.NUMPAD_8] = 0;
         this.pressedKeys[Keyboard.NUMPAD_2] = 0;
         this.pressedKeys[Keyboard.NUMPAD_4] = 0;
         this.pressedKeys[Keyboard.NUMPAD_6] = 0;
         this.pressedKeys[Keyboard.NUMPAD_ADD] = 0;
         this.pressedKeys[Keyboard.NUMPAD_SUBTRACT] = 0;
      }
      
      public function dispose() : void
      {
         this.pressedKeys = null;
         this.deactivate();
         Starling.current.juggler.remove(this);
      }
      
      public function activate() : void
      {
         Starling.current.stage.addEventListener(KeyboardEvent.KEY_DOWN,this.onKeyDown);
         Starling.current.stage.addEventListener(KeyboardEvent.KEY_UP,this.onKeyUp);
         this.resetChanges();
      }
      
      public function deactivate() : void
      {
         Starling.current.stage.removeEventListener(KeyboardEvent.KEY_DOWN,this.onKeyDown);
         Starling.current.stage.removeEventListener(KeyboardEvent.KEY_UP,this.onKeyUp);
         this.resetChanges();
      }
      
      private function resetChanges() : void
      {
         this.posChangeX = 0;
         this.posChangeY = 0;
         this.scaleChange = 0;
      }
      
      public function advanceTime(param1:Number) : void
      {
         if(Boolean(this.posChangeX) || Boolean(this.posChangeY))
         {
            this.camera.moveBy(this.posChangeX * param1,this.posChangeY * param1);
         }
         if(this.scaleChange)
         {
            this.camera.scaleBy(1 + this.scaleChange * param1);
         }
      }
      
      private function onKeyDown(param1:KeyboardEvent) : void
      {
         this.pressedKeys[param1.keyCode] = 1;
         this.updatePressedButtons();
      }
      
      private function onKeyUp(param1:KeyboardEvent) : void
      {
         this.pressedKeys[param1.keyCode] = 0;
         this.updatePressedButtons();
      }
      
      private function updatePressedButtons() : void
      {
         this.posChangeX = (this.pressedKeys[Keyboard.NUMPAD_6] - this.pressedKeys[Keyboard.NUMPAD_4]) * CAMERA_MOVE_SPEED;
         this.posChangeY = (this.pressedKeys[Keyboard.NUMPAD_2] - this.pressedKeys[Keyboard.NUMPAD_8]) * CAMERA_MOVE_SPEED;
         this.scaleChange = (this.pressedKeys[Keyboard.NUMPAD_SUBTRACT] - this.pressedKeys[Keyboard.NUMPAD_ADD]) * CAMERA_SCALE_SPEED;
         if(Boolean(this.posChangeX) || Boolean(this.posChangeY) || Boolean(this.scaleChange))
         {
            if(!Starling.juggler.contains(this))
            {
               Starling.current.juggler.add(this);
            }
         }
         else
         {
            Starling.juggler.remove(this);
         }
      }
   }
}

