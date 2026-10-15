package ru.pragmatix.wormix.scene.camera.controls
{
   import ru.pragmatix.wormix.gui.§_-F3s§;
   import ru.pragmatix.wormix.scene.camera.SceneCamera;
   import starling.events.Event;
   
   public class CameraScaleSliderControls implements ICameraControl
   {
      
      private static const MULTIPLIER:Number = 100;
      
      private var camera:SceneCamera;
      
      private var slider:§_-F3s§;
      
      public function CameraScaleSliderControls(param1:SceneCamera, param2:§_-F3s§)
      {
         super();
         this.camera = param1;
         this.slider = param2;
      }
      
      public function dispose() : void
      {
         this.deactivate();
      }
      
      public function activate() : void
      {
         this.slider.§_-E2N§(this.camera.cameraRect.scaleMax * MULTIPLIER);
         this.slider.§_-i2y§(this.camera.cameraRect.scaleMin * MULTIPLIER);
         this.slider.setValue(this.camera.cameraRect.scale * MULTIPLIER);
         this.slider.addEventListener(Event.CHANGE,this.onSliderChanged);
         this.camera.addEventListener(SceneCamera.SCALE_CHANGED,this.onCameraScaleChanged);
      }
      
      public function deactivate() : void
      {
         this.slider.removeEventListener(Event.CHANGE,this.onSliderChanged);
         this.camera.removeEventListener(SceneCamera.SCALE_CHANGED,this.onCameraScaleChanged);
      }
      
      private function onSliderChanged(param1:Event) : void
      {
         this.camera.scaleTo(this.slider.getValue() / MULTIPLIER);
      }
      
      private function onCameraScaleChanged(param1:Event) : void
      {
         this.slider.setValue((param1.data as Number) * MULTIPLIER,false);
      }
   }
}

