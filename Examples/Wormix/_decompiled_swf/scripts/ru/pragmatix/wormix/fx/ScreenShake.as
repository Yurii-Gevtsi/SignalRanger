package ru.pragmatix.wormix.fx
{
   import flash.events.TimerEvent;
   import flash.utils.Timer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.camera.CameraRect;
   
   public class ScreenShake
   {
      
      private static var timer:Timer;
      
      private static var originalX:int;
      
      private static var originalY:int;
      
      private static var intensityOffset:int;
      
      private static const FRAME_RATE:int = 100;
      
      public function ScreenShake()
      {
         super();
      }
      
      public static function go(param1:Number = 10, param2:Boolean = false, param3:Number = 0.1) : void
      {
         var _loc7_:Vector.<§_-01J§> = null;
         var _loc8_:int = 0;
         var _loc9_:§_-01J§ = null;
         var _loc10_:int = 0;
         if(timer)
         {
            cleanup();
         }
         var _loc4_:CameraRect = §_-X1j§.§_-12n§.scene.sceneCamera.cameraRect;
         originalX = _loc4_.centerX;
         originalY = _loc4_.centerY;
         intensityOffset = param1 / 2;
         var _loc5_:int = int(1000 / FRAME_RATE);
         var _loc6_:int = int(param3 * 1000 / _loc5_);
         timer = new Timer(_loc5_,_loc6_);
         timer.addEventListener(TimerEvent.TIMER,quake);
         timer.addEventListener(TimerEvent.TIMER_COMPLETE,resetImage,false,0,true);
         timer.start();
         if(param2)
         {
            _loc7_ = §_-X1j§.§_-12n§.gameMaster.§_-D3n§();
            _loc8_ = int(_loc7_.length);
            _loc10_ = 0;
            while(_loc10_ < _loc8_)
            {
               _loc9_ = _loc7_[_loc10_];
               if(_loc9_.stable)
               {
                  _loc9_.rotateUntilStable();
                  _loc9_.addSpeedXY(§_-X1j§.randomSeed.§_-Fh§(-3,3),-param1 / 10);
               }
               _loc10_++;
            }
         }
      }
      
      private static function quake(param1:TimerEvent) : void
      {
         var _loc2_:int = Math.random() * intensityOffset * 4 - intensityOffset * 2;
         var _loc3_:int = Math.random() * intensityOffset * 4 - intensityOffset * 2;
         §_-X1j§.§_-12n§.scene.sceneCamera.moveBy(_loc2_,_loc3_);
      }
      
      private static function resetImage(param1:TimerEvent = null) : void
      {
         §_-X1j§.§_-12n§.scene.sceneCamera.moveTo(originalX,originalY,true,false);
         cleanup();
      }
      
      private static function cleanup() : void
      {
         if(timer)
         {
            timer.removeEventListener(TimerEvent.TIMER,quake);
            timer.removeEventListener(TimerEvent.TIMER_COMPLETE,resetImage);
            timer.stop();
            timer = null;
         }
      }
   }
}

