package ru.pragmatix.wormix.scene.interaction
{
   import §_-G16§.§_-O2q§;
   import config.§_-vR§;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.utils.Timer;
   import ru.pragmatix.wormix.Scene;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class SceneInteractionsHandler
   {
      
      private var scene:Scene;
      
      private var _location:Point;
      
      private var _isActive:Boolean;
      
      private var _isPaused:Boolean;
      
      private var _isTouched:Boolean;
      
      private var _interactionData:SceneInteractionData;
      
      private var _singleTapInteractionType:int;
      
      private var _doubleTapInteractionType:int;
      
      private var _touchTimer:Timer;
      
      private var _tapTouch:Touch;
      
      public function SceneInteractionsHandler(param1:Scene, param2:Number = 350)
      {
         super();
         this.scene = param1;
         this._location = Pool.getPoint(NaN,NaN);
         this._isActive = false;
         this._isPaused = false;
         this._isTouched = false;
         this._interactionData = new SceneInteractionData();
         this._touchTimer = new Timer(param2,1);
         this._touchTimer.addEventListener(TimerEvent.TIMER,this.onTouchTimerComplete);
      }
      
      public function dispose() : void
      {
         this.scene.removeEventListener(TouchEvent.TOUCH,this.onSceneTouch);
         this.scene = null;
         §_-O2q§.§_-x2z§(this._location);
         this._location = null;
         this._interactionData.dispose();
         this._interactionData = null;
         this._touchTimer.stop();
         this._touchTimer.removeEventListener(TimerEvent.TIMER,this.onTouchTimerComplete);
         this._touchTimer = null;
      }
      
      public function activate() : void
      {
         if(!this._isActive)
         {
            this._isActive = true;
            this._isPaused = false;
            this.scene.addEventListener(TouchEvent.TOUCH,this.onSceneTouch);
            this._singleTapInteractionType = SceneInteractionType.SINGLE_TAP;
            this._doubleTapInteractionType = SystemUtil.isDesktop ? SceneInteractionType.DOUBLE_TAP_DESKTOP : SceneInteractionType.DOUBLE_TAP_MOBILE;
         }
      }
      
      public function deactivate() : void
      {
         if(this._isActive)
         {
            this._isActive = false;
            this._isPaused = false;
            this.scene.removeEventListener(TouchEvent.TOUCH,this.onSceneTouch);
            this._touchTimer.stop();
         }
      }
      
      public function get isActive() : Boolean
      {
         return this._isActive;
      }
      
      public function pause() : void
      {
         this._isPaused = true;
         if(this._isActive)
         {
            this._isTouched = false;
            this.dispatchEvent(SceneInteractionType.SCENE_TOUCH,this._location,this._isTouched);
         }
      }
      
      public function resume() : void
      {
         this._isPaused = false;
         if(this._isActive)
         {
            this._isTouched = true;
            this.dispatchEvent(SceneInteractionType.SCENE_TOUCH,this._location,this._isTouched);
         }
      }
      
      private function onSceneTouch(param1:TouchEvent) : void
      {
         var _loc3_:Point = null;
         var _loc4_:Point = null;
         if(this._isPaused)
         {
            return;
         }
         var _loc2_:Touch = param1.getTouch(this.scene);
         this.changeIsTouched(_loc2_ != null);
         if(this._isTouched)
         {
            if(SystemUtil.isDesktop && _loc2_.phase == TouchPhase.HOVER)
            {
               _loc2_.getLocation(this.scene.gameParallaxLayer,this._location);
               this.dispatchEvent(SceneInteractionType.MOVE_POINT,this._location);
            }
            else if(_loc2_.phase == TouchPhase.MOVED)
            {
               _loc2_.getLocation(this.scene.gameParallaxLayer,this._location);
               _loc3_ = Pool.getPoint(this._location.x,this._location.y);
               _loc2_.getPreviousLocation(this.scene.gameParallaxLayer,this._location);
               _loc4_ = Pool.getPoint(this._location.x,this._location.y);
               if(Point.distance(_loc3_,_loc4_) > §_-vR§.§_-52v§)
               {
                  this._touchTimer.stop();
                  this._tapTouch = null;
               }
               Pool.putPoint(_loc3_);
               Pool.putPoint(_loc4_);
            }
            else if(_loc2_.phase == TouchPhase.BEGAN)
            {
               if(!this._touchTimer.running)
               {
                  this._touchTimer.reset();
                  this._touchTimer.start();
               }
               this._tapTouch = null;
            }
            else if(_loc2_.phase == TouchPhase.ENDED)
            {
               if(this._touchTimer.running)
               {
                  if(_loc2_.tapCount >= 2)
                  {
                     this._touchTimer.stop();
                     this.onDoubleTap(_loc2_);
                  }
                  else
                  {
                     this._tapTouch = _loc2_;
                  }
               }
            }
         }
      }
      
      private function onTouchTimerComplete(param1:TimerEvent) : void
      {
         if(this._tapTouch)
         {
            this.onSingleTap(this._tapTouch);
            this._tapTouch = null;
         }
      }
      
      [Inline]
      private function changeIsTouched(param1:Boolean) : void
      {
         if(param1 != this._isTouched)
         {
            this._isTouched = param1;
            this.dispatchEvent(SceneInteractionType.SCENE_TOUCH,this._location,this._isTouched);
         }
      }
      
      private function onSingleTap(param1:Touch) : void
      {
         param1.getLocation(this.scene.gameParallaxLayer,this._location);
         this.dispatchEvent(this._singleTapInteractionType,this._location);
      }
      
      private function onDoubleTap(param1:Touch) : void
      {
         param1.getLocation(this.scene.gameParallaxLayer,this._location);
         this.dispatchEvent(this._doubleTapInteractionType,this._location);
      }
      
      private function dispatchEvent(param1:int, param2:Point, param3:Boolean = true) : void
      {
         this._interactionData.setData(param1,Pool.getPoint(param2.x,param2.y),param3);
         this.scene.dispatchEventWith(SceneInteractionEvent.INTERACT,false,this._interactionData);
      }
   }
}

