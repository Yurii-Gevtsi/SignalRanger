package ru.pragmatix.wormix.controls
{
   import §_-937§.§_-J28§;
   import §_-F2Q§.GestureEvent;
   import §_-f1G§.§_-425§;
   import §_-f1G§.§_-X1f§;
   import §_-f1G§.§_-kf§;
   import §_-f1G§.§_-ol§;
   import flash.utils.Dictionary;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   
   public class GesturesHandler
   {
      
      private var buttonsHandler:Object;
      
      private var longPressGesturesConfigs:Array;
      
      private var tapGesturesConfigs:Array;
      
      private var panGesturesConfigs:Array;
      
      private var longPressGestures:Dictionary;
      
      private var tapGestures:Dictionary;
      
      private var panGestures:Dictionary;
      
      private var inited:Boolean = false;
      
      private var panGesturesInited:Boolean = false;
      
      public function GesturesHandler(param1:Object, param2:Array, param3:Array, param4:Array)
      {
         super();
         this.buttonsHandler = param1;
         this.longPressGesturesConfigs = param2;
         this.tapGesturesConfigs = param3;
         this.panGesturesConfigs = param4;
      }
      
      public function initHandlers(param1:Boolean = false) : void
      {
         if(this.inited)
         {
            return;
         }
         this.inited = true;
         this.initLongPressGesturesHandlers();
         this.initTapGesturesHandlers();
         if(param1)
         {
            this.initPanGesturesHandlers();
         }
      }
      
      private function initLongPressGesturesHandlers() : void
      {
         var _loc1_:Object = null;
         var _loc2_:§_-kf§ = null;
         if(!this.longPressGesturesConfigs)
         {
            return;
         }
         this.longPressGestures = new Dictionary();
         for each(_loc1_ in this.longPressGesturesConfigs)
         {
            _loc2_ = this.longPressGestures[this.buttonsHandler[_loc1_.button]] = new §_-425§(this.buttonsHandler[_loc1_.button]);
            _loc2_.addEventListener(GestureEvent.§_-D1§,_loc1_.callback,false,0,true);
         }
         this.setMinLongPressDuration(1);
      }
      
      private function initTapGesturesHandlers() : void
      {
         var _loc1_:Object = null;
         var _loc2_:§_-kf§ = null;
         if(!this.tapGesturesConfigs)
         {
            return;
         }
         this.tapGestures = new Dictionary();
         for each(_loc1_ in this.tapGesturesConfigs)
         {
            _loc2_ = new §_-ol§(this.buttonsHandler[_loc1_.button]);
            _loc2_.addEventListener(GestureEvent.§_-m28§,this.tapGestureHandler,false,0,true);
            this.tapGestures[this.buttonsHandler[_loc1_.button]] = new §_-J28§(_loc2_,_loc1_.callback);
         }
      }
      
      public function initPanGesturesHandlers() : void
      {
         var _loc1_:§_-X1f§ = null;
         var _loc2_:Object = null;
         var _loc3_:Object = null;
         if(!this.inited || this.panGesturesInited || !this.panGesturesConfigs)
         {
            return;
         }
         this.panGestures = new Dictionary();
         for each(_loc2_ in this.panGesturesConfigs)
         {
            _loc3_ = this.buttonsHandler[_loc2_.button];
            _loc1_ = this.panGestures[_loc3_] = new §_-X1f§();
            _loc1_.target = _loc3_;
            _loc1_.§_-6h§ = 1;
            _loc1_.§_-G3C§ = 1;
            _loc1_.addEventListener(GestureEvent.§_-D1§,_loc2_.callback,false,0,true);
         }
         this.panGesturesInited = true;
      }
      
      public function removeHandlers() : void
      {
         if(!this.inited)
         {
            return;
         }
         this.removeLongPressGesturesHandlers();
         this.removeTapGesturesHandlers();
         this.removePanGesturesHandlers();
         this.inited = false;
      }
      
      private function removeLongPressGesturesHandlers() : void
      {
         var _loc1_:Object = null;
         var _loc2_:§_-kf§ = null;
         if(!this.longPressGesturesConfigs)
         {
            return;
         }
         for each(_loc1_ in this.longPressGesturesConfigs)
         {
            _loc2_ = this.longPressGestures[this.buttonsHandler[_loc1_.button]];
            if(_loc2_)
            {
               _loc2_.removeEventListener(GestureEvent.§_-D1§,_loc1_.callback);
               _loc2_.dispose();
               _loc2_ = null;
            }
         }
         this.longPressGestures = null;
      }
      
      private function removeTapGesturesHandlers() : void
      {
         var _loc1_:Object = null;
         var _loc2_:§_-J28§ = null;
         if(!this.tapGesturesConfigs)
         {
            return;
         }
         for each(_loc1_ in this.tapGesturesConfigs)
         {
            _loc2_ = this.tapGestures[this.buttonsHandler[_loc1_.button]];
            if(_loc2_)
            {
               _loc2_.gesture.removeEventListener(GestureEvent.§_-m28§,_loc1_.callback);
               _loc2_.gesture.dispose();
               _loc2_.gesture = null;
               _loc2_.handler = null;
            }
         }
         this.tapGestures = null;
      }
      
      public function removePanGesturesHandlers() : void
      {
         var _loc1_:Object = null;
         var _loc2_:§_-kf§ = null;
         if(!this.inited || !this.panGesturesInited || !this.panGesturesConfigs)
         {
            return;
         }
         for each(_loc1_ in this.panGesturesConfigs)
         {
            _loc2_ = this.panGestures[this.buttonsHandler[_loc1_.button]];
            if(_loc2_)
            {
               _loc2_.removeEventListener(GestureEvent.§_-D1§,_loc1_.callback);
               _loc2_.dispose();
               _loc2_ = null;
            }
         }
         this.panGesturesInited = false;
         this.panGestures = null;
      }
      
      private function tapGestureHandler(param1:GestureEvent) : void
      {
         var _loc2_:§_-ol§ = param1.target as §_-ol§;
         var _loc3_:§_-J28§ = this.tapGestures[_loc2_.target];
         if(Boolean(_loc3_) && Boolean(_loc2_.target.visible))
         {
            this.setTapGesturesActivity(this.tapGestures,false);
            _loc3_.handler();
            Starling.juggler.delayCall(this.setTapGesturesActivity,0.1,this.tapGestures,true);
         }
      }
      
      private function setTapGesturesActivity(param1:Dictionary, param2:Boolean) : void
      {
         var _loc3_:§_-J28§ = null;
         for each(_loc3_ in param1)
         {
            _loc3_.gesture.enabled = param2;
         }
      }
      
      public function setNumTapsRequired(param1:int) : void
      {
         var _loc2_:§_-J28§ = null;
         for each(_loc2_ in this.tapGestures)
         {
            _loc2_.gesture.§_-m2W§ = param1;
         }
      }
      
      public function setMinLongPressDuration(param1:int) : void
      {
         var _loc2_:§_-425§ = null;
         for each(_loc2_ in this.longPressGestures)
         {
            _loc2_.minPressDuration = param1;
         }
      }
      
      public function setNumTapsRequiredForButton(param1:DisplayObject, param2:int) : void
      {
         var _loc3_:§_-J28§ = this.tapGestures[param1] as §_-J28§;
         if(_loc3_)
         {
            _loc3_.gesture.§_-m2W§ = param2;
         }
      }
      
      public function dispose() : void
      {
         this.removeHandlers();
         this.buttonsHandler = null;
      }
   }
}

