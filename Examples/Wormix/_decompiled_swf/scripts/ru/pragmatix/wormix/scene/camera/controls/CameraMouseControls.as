package ru.pragmatix.wormix.scene.camera.controls
{
   import flash.events.MouseEvent;
   import ru.pragmatix.wormix.scene.camera.SceneCamera;
   import starling.core.Starling;
   
   public class CameraMouseControls implements ICameraControl
   {
      
      private static const CAMERA_SCALE_SPEED:Number = 0.005;
      
      private var camera:SceneCamera;
      
      public function CameraMouseControls(param1:SceneCamera)
      {
         super();
         this.camera = param1;
      }
      
      public function dispose() : void
      {
         this.deactivate();
      }
      
      public function activate() : void
      {
         Starling.current.nativeStage.addEventListener(MouseEvent.MOUSE_WHEEL,this.zoomOnWheel);
      }
      
      public function deactivate() : void
      {
         Starling.current.nativeStage.removeEventListener(MouseEvent.MOUSE_WHEEL,this.zoomOnWheel);
      }
      
      private function zoomOnWheel(param1:MouseEvent) : void
      {
         this.camera.scaleBy(1 - param1.delta * CAMERA_SCALE_SPEED);
      }
   }
}

