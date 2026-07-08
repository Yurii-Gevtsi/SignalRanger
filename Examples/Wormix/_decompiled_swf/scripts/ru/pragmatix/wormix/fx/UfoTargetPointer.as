package ru.pragmatix.wormix.fx
{
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.textures.Texture;
   
   public class UfoTargetPointer extends Sprite
   {
      
      private static const SPRITE_NAME:String = "ufo_target_indicator";
      
      public static const ATTACK_COLOR:uint = 16203069;
      
      public static const CARRY_COLOR:uint = 11789614;
      
      private var _target:§_-F3o§;
      
      public function UfoTargetPointer()
      {
         var _loc2_:Image = null;
         var _loc3_:Tween = null;
         var _loc6_:Number = NaN;
         var _loc7_:Number = NaN;
         super();
         var _loc1_:Texture = Application.assetManager.getTexture(SPRITE_NAME);
         var _loc4_:Number = -MathUtil.§_-Z1G§(45);
         var _loc5_:Number = MathUtil.§_-Z1G§(90);
         _loc6_ = 35;
         _loc7_ = 0.5;
         var _loc8_:int = 0;
         while(_loc8_ < 4)
         {
            _loc2_ = new Image(_loc1_);
            _loc2_.scale = 0.8;
            _loc2_.alignPivot();
            _loc2_.rotation = _loc4_ + _loc5_ * _loc8_;
            _loc2_.x = -Math.sin(_loc2_.rotation) * _loc6_;
            _loc2_.y = Math.cos(_loc2_.rotation) * _loc6_;
            addChild(_loc2_);
            _loc3_ = new Tween(_loc2_,_loc7_);
            _loc3_.moveTo(_loc2_.x * 1.25,_loc2_.y * 1.25);
            _loc3_.repeatCount = 0;
            _loc3_.reverse = true;
            Starling.juggler.add(_loc3_);
            _loc8_++;
         }
      }
      
      public function setColor(param1:uint) : void
      {
         var _loc3_:Image = null;
         var _loc2_:int = 0;
         while(_loc2_ < numChildren)
         {
            _loc3_ = getChildAt(_loc2_) as Image;
            if(_loc3_)
            {
               _loc3_.color = param1;
            }
            _loc2_++;
         }
      }
      
      override public function dispose() : void
      {
         var _loc2_:Image = null;
         var _loc1_:int = 0;
         while(_loc1_ < numChildren)
         {
            _loc2_ = getChildAt(_loc1_) as Image;
            if(_loc2_)
            {
               Starling.juggler.removeTweens(_loc2_);
            }
            _loc1_++;
         }
         super.dispose();
      }
      
      public function get target() : §_-F3o§
      {
         return this._target;
      }
      
      public function set target(param1:§_-F3o§) : void
      {
         this._target = param1;
      }
   }
}

