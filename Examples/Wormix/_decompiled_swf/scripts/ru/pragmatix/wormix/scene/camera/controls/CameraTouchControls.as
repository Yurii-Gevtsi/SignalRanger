package ru.pragmatix.wormix.scene.camera.controls
{
   import flash.geom.Point;
   import ru.pragmatix.wormix.scene.camera.*;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.core.Starling;
   import starling.display.Stage;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.utils.Pool;
   
   public class CameraTouchControls implements ICameraControl
   {
      
      private var camera:SceneCamera;
      
      private var checkingTouchPhases:Array;
      
      private var touches:Vector.<Touch>;
      
      private var prevTouchDelta:Point;
      
      private var prevTouchTimestamp:Number;
      
      public function CameraTouchControls(param1:SceneCamera)
      {
         super();
         this.camera = param1;
         this.checkingTouchPhases = [TouchPhase.BEGAN,TouchPhase.MOVED,TouchPhase.ENDED];
         this.touches = new Vector.<Touch>(0);
         this.prevTouchDelta = Pool.getPoint();
         this.prevTouchTimestamp = 0;
      }
      
      public function dispose() : void
      {
         this.deactivate();
         this.touches = null;
         Pool.putPoint(this.prevTouchDelta);
         this.prevTouchDelta = null;
      }
      
      public function activate() : void
      {
         this.camera.scene.addEventListener(TouchEvent.TOUCH,this.onTouch);
      }
      
      public function deactivate() : void
      {
         this.camera.scene.removeEventListener(TouchEvent.TOUCH,this.onTouch);
      }
      
      private function onTouch(param1:TouchEvent) : void
      {
         var _loc2_:String = null;
         for each(_loc2_ in this.checkingTouchPhases)
         {
            this.touches.length = 0;
            param1.getTouches(this.camera.scene,_loc2_,this.touches);
            if(this.touches.length > 0)
            {
               this.processTouches(this.touches,_loc2_);
               break;
            }
         }
      }
      
      private function processTouches(param1:Vector.<Touch>, param2:String) : void
      {
         var _loc4_:Touch = null;
         var _loc5_:Touch = null;
         var _loc6_:Touch = null;
         var _loc7_:Point = null;
         var _loc8_:Point = null;
         var _loc9_:Point = null;
         var _loc10_:Point = null;
         var _loc11_:Number = NaN;
         var _loc12_:Number = NaN;
         var _loc13_:Number = NaN;
         var _loc3_:Stage = Starling.current.stage;
         if(param1.length == 1)
         {
            _loc4_ = param1[0];
            if(param2 == TouchPhase.ENDED)
            {
               if(_loc4_.timestamp - this.prevTouchTimestamp < 0.05)
               {
                  this.camera.throwDrag(-this.prevTouchDelta.x,-this.prevTouchDelta.y);
               }
            }
            else
            {
               this.prevTouchDelta.x = _loc4_.globalX - _loc4_.previousGlobalX;
               this.prevTouchDelta.y = _loc4_.globalY - _loc4_.previousGlobalY;
               this.prevTouchTimestamp = _loc4_.timestamp;
               this.camera.moveBy(-this.prevTouchDelta.x,-this.prevTouchDelta.y);
            }
         }
         else if(param1.length == 2)
         {
            _loc5_ = param1[0];
            _loc6_ = param1[1];
            _loc7_ = Pool.getPoint();
            _loc8_ = Pool.getPoint();
            _loc9_ = Pool.getPoint();
            _loc10_ = Pool.getPoint();
            _loc5_.getLocation(_loc3_,_loc7_);
            _loc6_.getLocation(_loc3_,_loc9_);
            _loc5_.getPreviousLocation(_loc3_,_loc8_);
            _loc6_.getPreviousLocation(_loc3_,_loc10_);
            _loc11_ = MathUtil.§_-Ih§(_loc7_.x,_loc7_.y,_loc9_.x,_loc9_.y);
            _loc12_ = MathUtil.§_-Ih§(_loc8_.x,_loc8_.y,_loc10_.x,_loc10_.y);
            _loc13_ = Math.sqrt(_loc12_ / _loc11_);
            this.camera.scaleBy(_loc13_);
            Pool.putPoint(_loc7_);
            Pool.putPoint(_loc8_);
            Pool.putPoint(_loc9_);
            Pool.putPoint(_loc10_);
         }
      }
   }
}

