package ru.pragmatix.wormix.scene.camera
{
   import §_-B1y§.Cookie;
   import config.§_-vR§;
   import flash.geom.Rectangle;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.scene.camera.controls.CameraKeyboardControls;
   import ru.pragmatix.wormix.scene.camera.controls.CameraMouseControls;
   import ru.pragmatix.wormix.scene.camera.controls.CameraScaleSliderControls;
   import ru.pragmatix.wormix.scene.camera.controls.CameraTouchControls;
   import ru.pragmatix.wormix.scene.camera.controls.ICameraControl;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.animation.Transitions;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Stage;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class SceneCamera extends EventDispatcher
   {
      
      public static const SCALE_CHANGED:String = "scale-changed";
      
      private static const ZOOM_VALUE:String = "ZOOM_VALUE";
      
      private var _scene:Scene;
      
      private var stageRect:Rectangle;
      
      private var _cameraRect:CameraRect;
      
      private var cameraControls:Array;
      
      private var miniMap:CameraMiniMap;
      
      private var parallaxLayers:Vector.<ParallaxLayer>;
      
      private var _updateTween:Tween;
      
      public function SceneCamera(param1:Scene, param2:§_-Q2v§)
      {
         super();
         this._scene = param1;
         this.parallaxLayers = new Vector.<ParallaxLayer>(0);
         var _loc3_:Stage = Starling.current.stage;
         this.stageRect = Pool.getRectangle(0,0,_loc3_.stageWidth,_loc3_.stageHeight);
         this._cameraRect = new CameraRect(_loc3_.stageWidth,_loc3_.stageHeight,param1.width,param1.height);
         this._cameraRect.x = 0;
         this._cameraRect.y = 0;
         this.cameraControls = [];
         this.cameraControls.push(new CameraTouchControls(this));
         if(§_-vR§.§_-pD§ || SystemUtil.isDesktop)
         {
            this.cameraControls.push(new CameraKeyboardControls(this));
         }
         if(SystemUtil.isDesktop)
         {
            this.cameraControls.push(new CameraMouseControls(this));
            this.cameraControls.push(new CameraScaleSliderControls(this,param2.§_-N2u§.§_-ki§));
         }
         if(§_-vR§.§_-pD§)
         {
            this.miniMap = new CameraMiniMap(this);
         }
      }
      
      public function init() : void
      {
         var _loc1_:Number = Cookie.getValue(ZOOM_VALUE,1) as Number;
         this._cameraRect.scale = _loc1_;
         this.setToCenter();
      }
      
      public function setToCenter() : void
      {
         this.moveBy((this._scene.width - Starling.current.stage.stageWidth) * 0.5,(this._scene.height - Starling.current.stage.stageHeight) * 0.5);
      }
      
      public function dispose() : void
      {
         var _loc1_:ICameraControl = null;
         Pool.putRectangle(this.stageRect);
         for each(_loc1_ in this.cameraControls)
         {
            _loc1_.dispose();
         }
         this.cameraControls = null;
         if(this._updateTween)
         {
            Starling.juggler.remove(this._updateTween);
         }
         if(this.miniMap)
         {
            this.miniMap.dispose();
         }
         this._cameraRect.dispose();
         Cookie.setValue(ZOOM_VALUE,this._cameraRect.scale);
      }
      
      public function activateControls() : void
      {
         var _loc1_:ICameraControl = null;
         for each(_loc1_ in this.cameraControls)
         {
            _loc1_.activate();
         }
      }
      
      public function deactivateControls() : void
      {
         var _loc1_:ICameraControl = null;
         this.stopTween();
         for each(_loc1_ in this.cameraControls)
         {
            _loc1_.deactivate();
         }
      }
      
      public function addParallaxLayer(param1:DisplayObject, param2:Number, param3:Number, param4:Number) : void
      {
         var _loc5_:Rectangle = Pool.getRectangle(0,0,param3,param4);
         var _loc6_:Rectangle = Pool.getRectangle(0,0,Starling.current.stage.stageWidth,Starling.current.stage.stageHeight);
         var _loc7_:ParallaxLayer = new ParallaxLayer();
         _loc7_.display = param1;
         _loc7_.parallax = param2;
         _loc7_.minScale = Math.max(_loc6_.width / param3,_loc6_.height / param4);
         RectangleUtil.fit(_loc6_,_loc5_,ScaleMode.SHOW_ALL,false,_loc6_);
         _loc7_.offsetX = param2 == 1 ? 0 : _loc6_.x;
         _loc7_.offsetY = param2 == 1 ? 0 : _loc6_.y;
         this.parallaxLayers.push(_loc7_);
         Pool.putRectangle(_loc5_);
         Pool.putRectangle(_loc6_);
      }
      
      private function updateLayers() : void
      {
         var _loc2_:ParallaxLayer = null;
         var _loc1_:* = int(this.parallaxLayers.length);
         while(_loc1_--)
         {
            _loc2_ = this.parallaxLayers[_loc1_];
            _loc2_.display.scale = _loc2_.minScale + (1 / this._cameraRect.scale - _loc2_.minScale) * _loc2_.parallax;
            _loc2_.display.x = (-this._cameraRect.x * _loc2_.parallax - _loc2_.offsetX) * _loc2_.display.scale;
            _loc2_.display.y = (-this._cameraRect.y * _loc2_.parallax - _loc2_.offsetY) * _loc2_.display.scale;
         }
      }
      
      public function moveBy(param1:Number, param2:Number) : void
      {
         this.stopTween();
         this._cameraRect.x += param1;
         this._cameraRect.y += param2;
         this.updateLayers();
      }
      
      public function throwDrag(param1:Number, param2:Number) : void
      {
         var tween:Tween = null;
         var dragX:Number = param1;
         var dragY:Number = param2;
         if(Boolean(dragX) || Boolean(dragY))
         {
            tween = this.getTween(1);
            tween.onUpdateArgs = [{
               "x":dragX,
               "y":dragY
            }];
            tween.onUpdate = function(param1:Object):void
            {
               updateLayers();
               _cameraRect.x += param1.x;
               _cameraRect.y += param1.y;
               param1.x *= 0.95;
               param1.y *= 0.95;
            };
            Starling.juggler.add(tween);
         }
      }
      
      public function moveTo(param1:Number, param2:Number, param3:Boolean = true, param4:Boolean = true, param5:Number = NaN) : void
      {
         var _loc9_:Number = NaN;
         var _loc10_:Number = NaN;
         var _loc11_:Number = NaN;
         var _loc12_:Number = NaN;
         var _loc13_:Tween = null;
         if(isNaN(param5))
         {
            param5 = this._cameraRect.scale;
         }
         var _loc6_:Number = param5 / this._cameraRect.scale;
         var _loc7_:Number = this._cameraRect.width * _loc6_;
         var _loc8_:Number = this._cameraRect.height * _loc6_;
         if(param3)
         {
            _loc9_ = param1 - _loc7_ * 0.5;
            _loc10_ = param2 - _loc8_ * 0.5;
         }
         else
         {
            _loc11_ = _loc7_ * 0.25;
            _loc12_ = _loc8_ * 0.25;
            if(param1 < this._cameraRect.x + _loc11_)
            {
               _loc9_ = param1 - _loc11_;
            }
            else if(param1 > this._cameraRect.x + _loc7_ - _loc11_)
            {
               _loc9_ = param1 - _loc7_ + _loc11_;
            }
            else
            {
               _loc9_ = this._cameraRect.x;
            }
            if(param2 < this._cameraRect.y + _loc12_)
            {
               _loc10_ = param2 - _loc12_;
            }
            else if(param2 > this._cameraRect.y + _loc8_ - _loc12_)
            {
               _loc10_ = param2 - _loc8_ + _loc12_;
            }
            else
            {
               _loc10_ = this._cameraRect.y;
            }
         }
         if(param4)
         {
            _loc13_ = this.getTween(1,Transitions.EASE_OUT);
            _loc13_.animate("scale",param5);
            _loc13_.moveTo(_loc9_,_loc10_);
            _loc13_.onUpdate = this.updateLayers;
            Starling.juggler.add(_loc13_);
         }
         else
         {
            this.stopTween();
            this._cameraRect.scale = param5;
            this._cameraRect.x = _loc9_;
            this._cameraRect.y = _loc10_;
            this.updateLayers();
         }
      }
      
      public function moveToObject(param1:§_-F3o§, param2:Boolean = true, param3:Boolean = true, param4:Number = NaN) : void
      {
         var _loc5_:IPhysicModel = param1.getPhysicModel();
         if(_loc5_)
         {
            this.moveTo(_loc5_.x,_loc5_.y,param2,param3);
         }
      }
      
      public function scaleBy(param1:Number) : void
      {
         var _loc2_:Number = param1 == 1 ? 0 : param1;
         if(_loc2_ != 0)
         {
            this._cameraRect.scale *= _loc2_;
            dispatchEventWith(SCALE_CHANGED,false,this._cameraRect.scale);
            this.updateLayers();
         }
      }
      
      public function scaleTo(param1:Number) : void
      {
         if(param1 != this._cameraRect.scale)
         {
            this._cameraRect.scale = param1;
            dispatchEventWith(SCALE_CHANGED,false,this._cameraRect.scale);
            this.updateLayers();
         }
      }
      
      public function followObject(param1:§_-F3o§) : void
      {
         var _loc2_:Tween = this.getTween(5);
         _loc2_.onUpdateArgs = [param1];
         _loc2_.onUpdate = this.updateFollowObjects;
         _loc2_.repeatCount = 0;
         Starling.juggler.add(_loc2_);
      }
      
      private function updateFollowObjects(param1:§_-F3o§) : void
      {
         var _loc3_:Number = NaN;
         var _loc4_:Number = NaN;
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         var _loc7_:Number = NaN;
         var _loc8_:Number = NaN;
         var _loc2_:IPhysicModel = param1.getPhysicModel();
         if(_loc2_)
         {
            _loc5_ = _loc2_.x;
            _loc6_ = _loc2_.y;
            _loc7_ = this._cameraRect.width * 0.4;
            _loc8_ = this._cameraRect.height * 0.4;
            if(_loc5_ < this._cameraRect.x + _loc7_)
            {
               _loc3_ = _loc5_ - _loc7_;
            }
            else if(_loc5_ > this._cameraRect.x + this._cameraRect.width - _loc7_)
            {
               _loc3_ = _loc5_ - this._cameraRect.width + _loc7_;
            }
            else
            {
               _loc3_ = this._cameraRect.x;
            }
            if(_loc6_ < this._cameraRect.y + _loc8_)
            {
               _loc4_ = _loc6_ - _loc8_;
            }
            else if(_loc6_ > this._cameraRect.y + this._cameraRect.height - _loc8_)
            {
               _loc4_ = _loc6_ - this._cameraRect.height + _loc8_;
            }
            else
            {
               _loc4_ = this._cameraRect.y;
            }
            this._cameraRect.x = MathUtil.§_-Z2Y§(this._cameraRect.x,_loc3_,0.05);
            this._cameraRect.y = MathUtil.§_-Z2Y§(this._cameraRect.y,_loc4_,0.05);
            this.updateLayers();
         }
         else
         {
            this.stopTween();
         }
      }
      
      public function get scene() : Scene
      {
         return this._scene;
      }
      
      private function getTween(param1:Number, param2:Object = "linear") : Tween
      {
         this.stopTween();
         this._updateTween = new Tween(this._cameraRect,param1,param2);
         return this._updateTween;
      }
      
      private function stopTween() : void
      {
         if(this._updateTween)
         {
            Starling.juggler.remove(this._updateTween);
         }
      }
      
      public function get cameraRect() : CameraRect
      {
         return this._cameraRect;
      }
      
      public function lock() : void
      {
         this.deactivateControls();
      }
      
      public function unlock() : void
      {
         this.activateControls();
      }
   }
}

