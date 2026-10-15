package ru.pragmatix.wormix.utils
{
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-gr§;
   import §_-73I§.§_-Y1l§;
   import §_-73I§.§_-q15§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.*;
   import §_-CF§.§_-x2t§;
   import §_-GQ§.§_-31v§;
   import §_-H16§.*;
   import §_-Nq§.*;
   import §_-SP§.§_-M4§;
   import §_-TF§.§_-q1j§;
   import §_-TF§.§_-y2n§;
   import §_-Ta§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-V1n§;
   import §_-u1F§.§_-SE§;
   import §_-v2t§.§_-73F§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.*;
   import §_-zI§.*;
   import com.hurlant.math.BigInteger;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.core.ITextRenderer;
   import feathers.data.ArrayCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import flash.display.BitmapData;
   import flash.display.MovieClip;
   import flash.display.Shape;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.setInterval;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.MobileShopPropertiesListItem;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.textures.TextureSmoothing;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class §_-Py§
   {
      
      private static var PI:Number = Math.PI;
      
      protected static const randomSeed:§_-V1n§ = §_-X1j§.randomSeed;
      
      public function §_-Py§()
      {
         super();
      }
      
      public static function §_-rR§(param1:int) : Boolean
      {
         return !(param1 & 1);
      }
      
      public static function §_-UW§(param1:Number, param2:Number) : Point
      {
         var _loc3_:Number = §_-Py§.abs(param1);
         return Pool.getPoint(_loc3_ * Math.cos(param2),_loc3_ * Math.sin(param2));
      }
      
      public static function random(param1:Number, param2:Number) : Number
      {
         return Math.round(Math.random() * (param2 - param1)) + param1;
      }
      
      public static function distance(param1:Number, param2:Number, param3:Number, param4:Number) : Number
      {
         var _loc5_:Number = param3 - param1;
         var _loc6_:Number = param4 - param2;
         return Math.sqrt(_loc5_ * _loc5_ + _loc6_ * _loc6_);
      }
      
      public static function §_-02X§(param1:Number, param2:Number, param3:Number, param4:Number, param5:Boolean = true) : Number
      {
         var _loc6_:Number = param3 - param1;
         var _loc7_:Number = param4 - param2;
         var _loc8_:Number = Math.atan2(_loc7_,_loc6_);
         if(param5)
         {
            if(_loc8_ < 0)
            {
               _loc8_ = PI * 2 + _loc8_;
            }
            else if(_loc8_ >= PI * 2)
            {
               _loc8_ -= PI * 2;
            }
         }
         return _loc8_;
      }
      
      public static function §_-M5§(param1:Number, param2:Number, param3:Number, param4:Number, param5:Boolean = true) : Number
      {
         var _loc6_:Number = param3 - param1;
         var _loc7_:Number = param4 - param2;
         var _loc8_:Number = Math.atan2(_loc7_,_loc6_) / PI * 180;
         if(param5)
         {
            if(_loc8_ < 0)
            {
               _loc8_ = 360 + _loc8_;
            }
            else if(_loc8_ >= 360)
            {
               _loc8_ -= 360;
            }
         }
         return _loc8_;
      }
      
      public static function §_-oL§(param1:Number, param2:Number) : Number
      {
         return param1 / param2 * 100;
      }
      
      public static function §_-na§(param1:Number, param2:Number) : Number
      {
         return param1 * param2 / 100;
      }
      
      public static function equal(param1:Number, param2:Number, param3:Number = 0.00001) : Boolean
      {
         return abs(param1 - param2) <= param3;
      }
      
      public static function §_-f1V§(param1:Number) : Number
      {
         return param1 * 180 / PI;
      }
      
      public static function §_-Z1G§(param1:Number) : Number
      {
         return param1 * PI / 180;
      }
      
      public static function abs(param1:Number) : Number
      {
         return (param1 ^ param1 >> 31) - (param1 >> 31);
      }
      
      public static function §_-G1o§(param1:Point, param2:Point, param3:Point, param4:Number) : Boolean
      {
         var _loc11_:Number = NaN;
         var _loc12_:Number = NaN;
         var _loc13_:Number = NaN;
         var _loc14_:Number = NaN;
         var _loc15_:Number = NaN;
         var _loc16_:Number = NaN;
         var _loc17_:Number = NaN;
         var _loc5_:Number = param2.x - param1.x;
         var _loc6_:Number = param2.y - param1.y;
         var _loc7_:Number = _loc5_ * _loc5_ + _loc6_ * _loc6_;
         var _loc8_:Number = 2 * (_loc5_ * (param1.x - param3.x) + _loc6_ * (param1.y - param3.y));
         var _loc9_:Number = param3.x * param3.x + param3.y * param3.y;
         _loc9_ = _loc9_ + (param1.x * param1.x + param1.y * param1.y);
         _loc9_ = _loc9_ - 2 * (param3.x * param1.x + param3.y * param1.y);
         _loc9_ = _loc9_ - param4 * param4;
         var _loc10_:Number = _loc8_ * _loc8_ - 4 * _loc7_ * _loc9_;
         if(_loc10_ < 0)
         {
            return false;
         }
         _loc11_ = (-_loc8_ + Math.sqrt(_loc8_ * _loc8_ - 4 * _loc7_ * _loc9_)) / (2 * _loc7_);
         _loc12_ = param1.x + _loc11_ * _loc5_;
         _loc13_ = param1.y + _loc11_ * _loc6_;
         _loc11_ = (-_loc8_ - Math.sqrt(_loc8_ * _loc8_ - 4 * _loc7_ * _loc9_)) / (2 * _loc7_);
         _loc14_ = param1.x + _loc11_ * _loc5_;
         _loc15_ = param1.y + _loc11_ * _loc6_;
         if(§_-Py§.distance(param1.x,param1.y,param3.x,param3.y) < §_-Py§.distance(param2.x,param2.y,param3.x,param3.y))
         {
            _loc16_ = param2.x;
            _loc17_ = param2.y;
         }
         else
         {
            _loc16_ = param1.x;
            _loc17_ = param1.y;
         }
         return §_-Py§.distance(_loc16_,_loc17_,_loc12_,_loc13_) < §_-Py§.distance(param1.x,param1.y,param2.x,param2.y) || §_-Py§.distance(_loc16_,_loc17_,_loc14_,_loc15_) < §_-Py§.distance(param1.x,param1.y,param2.x,param2.y);
      }
      
      public static function §_-41E§(param1:Number, param2:Number = 0, param3:Number = 0) : Point
      {
         var _loc4_:Number = randomSeed.§_-y2j§() * ru.pragmatix.wormix.utils.MathUtil.§_-M2N§;
         var _loc5_:Number = randomSeed.§_-y2j§() * param1;
         return Pool.getPoint(_loc5_ * Math.cos(_loc4_) + param2,_loc5_ * Math.sin(_loc4_) + param3);
      }
      
      public static function §_-b28§(param1:Number, param2:int) : Number
      {
         var _loc3_:int = Math.pow(10,param2);
         return Math.round(param1 * _loc3_) / _loc3_;
      }
   }
}

