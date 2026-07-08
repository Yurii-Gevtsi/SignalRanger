package ru.pragmatix.wormix.utils
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-51p§.§_-W2e§;
   import §_-62§.*;
   import §_-63U§.§_-21K§;
   import §_-73I§.§_-Z2e§;
   import §_-73I§.§_-q15§;
   import §_-73d§.§_-x2C§;
   import §_-82a§.*;
   import §_-9e§.§_-73e§;
   import §_-A1K§.§_-TA§;
   import §_-B2z§.§_-63i§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-Nq§.*;
   import §_-P1z§.*;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-O2i§;
   import §_-W§.§_-ix§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-ZP§.§_-YG§;
   import §_-ZP§.§_-w1J§;
   import §_-d26§.§_-d1H§;
   import §_-g2r§.§_-03A§;
   import §_-hO§.§_-v24§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-s1s§.*;
   import §_-w2i§.*;
   import §_-y1s§.§_-P9§;
   import §_-z1g§.§_-M2v§;
   import §_-z1g§.§_-lp§;
   import feathers.core.ITextRenderer;
   import feathers.data.ArrayCollection;
   import feathers.layout.RelativePosition;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.fx.CustomColorMatrixFilter;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.DistributePointsResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-m1x§;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreated;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.scene.tile.TileData;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.extensions.PDParticleSystem;
   import starling.filters.ColorMatrixFilter;
   import starling.textures.TextureSmoothing;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class §_-MN§
   {
      
      private static const §_-tB§:Vector.<Number> = Vector.<Number>([1 / 3,1 / 3,1 / 3,0,0,1 / 3,1 / 3,1 / 3,0,0,1 / 3,1 / 3,1 / 3,0,0,0,0,0,1,0]);
      
      private static var §_-H5§:Array = [];
      
      private static var §_-m1Y§:Array = [];
      
      private static var §_-915§:Array = [];
      
      private static var §_-UI§:Array = [];
      
      private static var §_-82E§:Array = [];
      
      private static const §_-62k§:uint = 16777215;
      
      public function §_-MN§()
      {
         super();
      }
      
      public static function §_-lM§(param1:DisplayObject, param2:Boolean = true) : void
      {
         §_-6G§(param1);
         §_-U29§(param1,§_-tB§,param2);
      }
      
      public static function tint(param1:DisplayObject, param2:uint, param3:Number, param4:Boolean = false) : void
      {
         §_-6G§(param1);
         var _loc5_:ColorMatrixFilter = §_-U29§(param1,null,param4);
         _loc5_.tint(param2);
         _loc5_.adjustContrast(param3);
         param1.filter = _loc5_;
      }
      
      public static function §_-6G§(param1:DisplayObject) : void
      {
         if(param1.filter)
         {
            param1.filter.dispose();
            param1.filter = null;
         }
      }
      
      private static function §_-U29§(param1:DisplayObject, param2:Vector.<Number>, param3:Boolean = false) : ColorMatrixFilter
      {
         var _loc4_:ColorMatrixFilter = new CustomColorMatrixFilter();
         _loc4_.matrix = param2;
         _loc4_.textureSmoothing = TextureSmoothing.TRILINEAR;
         param1.filter = _loc4_;
         if(param3)
         {
            _loc4_.cache();
         }
         return _loc4_;
      }
      
      private static function §_-H2o§(param1:Vector.<Number>) : ColorTransform
      {
         if(param1.length < 20)
         {
            return new ColorTransform();
         }
         var _loc2_:Number = §_-j1P§((param1[0] - 1) * 255);
         var _loc3_:Number = §_-j1P§((param1[6] - 1) * 255);
         var _loc4_:Number = §_-j1P§((param1[12] - 1) * 255);
         var _loc5_:Number = §_-j1P§((param1[18] - 1) * 255);
         return new ColorTransform(1,1,1,1,_loc2_,_loc3_,_loc4_,_loc5_);
      }
      
      private static function §_-43X§(param1:ColorTransform) : Vector.<Number>
      {
         return new <Number>[1 + param1.redOffset / 255,0,0,0,0,0,1 + param1.greenOffset / 255,0,0,0,0,0,1 + param1.blueOffset / 255,0,0,0,0,0,1 + param1.alphaOffset / 255,0];
      }
      
      private static function §_-5Q§(param1:ColorTransform) : Boolean
      {
         return param1.redOffset == 0 && param1.greenOffset == 0 && param1.blueOffset == 0 && param1.alphaOffset == 0;
      }
      
      public static function §_-z1b§(param1:DisplayObject) : ColorTransform
      {
         var _loc2_:ColorMatrixFilter = param1.filter as ColorMatrixFilter;
         if(!_loc2_)
         {
            return new ColorTransform();
         }
         return §_-H2o§(_loc2_.matrix);
      }
      
      public static function §_-H1e§(param1:DisplayObject, param2:ColorTransform, param3:Boolean = false) : void
      {
         var _loc6_:Vector.<Number> = null;
         var _loc11_:ColorTransform = null;
         var _loc4_:ColorMatrixFilter = param1.filter as ColorMatrixFilter;
         var _loc5_:ColorTransform = new ColorTransform(param2.redMultiplier,param2.greenMultiplier,param2.blueMultiplier,param2.alphaMultiplier,§_-j1P§(param2.redOffset),§_-j1P§(param2.greenOffset),§_-j1P§(param2.blueOffset),param2.alphaOffset);
         if(_loc4_)
         {
            _loc11_ = §_-H2o§(_loc4_.matrix);
            _loc5_.concat(_loc11_);
            _loc4_.dispose();
         }
         if(§_-5Q§(_loc5_))
         {
            §_-6G§(param1);
            return;
         }
         var _loc7_:int = §_-82E§.indexOf(_loc5_);
         if(_loc7_ < 0)
         {
            _loc6_ = §_-43X§(_loc5_);
            §_-U29§(param1,_loc6_,param3);
            §_-82E§.push(_loc5_);
            §_-UI§.push(param1.filter);
         }
         else
         {
            param1.filter = §_-UI§[_loc7_];
         }
         var _loc8_:ColorMatrixFilter = param1.filter as ColorMatrixFilter;
         _loc6_ = _loc8_.matrix;
         var _loc9_:String = _loc6_[0].toFixed(2);
         var _loc10_:int = 1;
         while(_loc10_ < _loc6_.length)
         {
            _loc9_ += " " + _loc6_[_loc10_].toFixed(2);
            _loc10_++;
         }
      }
      
      public static function §_-u1S§(param1:DisplayObject, param2:Vector.<ColorTransform>, param3:Boolean = true) : void
      {
         if(param3)
         {
            §_-6G§(param1);
         }
         var _loc4_:ColorTransform = new ColorTransform();
         var _loc5_:* = int(param2.length);
         while(_loc5_--)
         {
            _loc4_.concat(param2[_loc5_]);
         }
         §_-H1e§(param1,_loc4_);
      }
      
      public static function §_-j1P§(param1:int) : int
      {
         return Math.max(Math.min(param1,255),-255);
      }
      
      public static function §_-F3D§(param1:DisplayObjectContainer, param2:uint) : void
      {
         var _loc4_:DisplayObject = null;
         if(!param1)
         {
            return;
         }
         var _loc3_:* = param1.numChildren;
         while(_loc3_--)
         {
            _loc4_ = param1.getChildAt(_loc3_);
            if(_loc4_ is DisplayObjectContainer)
            {
               §_-F3D§(_loc4_ as DisplayObjectContainer,param2);
            }
            else if(_loc4_ is Image)
            {
               (_loc4_ as Image).color = param2;
            }
         }
      }
      
      public static function §_-D9§(param1:§_-01J§, param2:uint) : void
      {
         §_-V1k§(param1.§_-m13§,param2);
      }
      
      public static function §_-ZM§(param1:§_-01J§, param2:uint) : void
      {
         §_-D1a§(param1.§_-m13§,param2);
      }
      
      public static function §_-V1k§(param1:DisplayObject, param2:uint) : void
      {
         var _loc4_:int = 0;
         var _loc3_:int = §_-H5§.indexOf(param1);
         if(_loc3_ < 0)
         {
            §_-H5§.push(param1);
            §_-m1Y§.push(param2);
            §_-915§.push([param2]);
            §_-F3D§(param1 as DisplayObjectContainer,param2);
         }
         else
         {
            if(!§_-915§[_loc3_])
            {
               §_-915§[_loc3_] = [];
            }
            _loc4_ = int(§_-915§[_loc3_].indexOf(param2));
            if(_loc4_ > 0)
            {
               return;
            }
            §_-915§[_loc3_].push(param2);
            §_-m1Y§[_loc3_] = §_-M2C§(_loc3_);
            §_-F3D§(param1 as DisplayObjectContainer,§_-m1Y§[_loc3_]);
         }
      }
      
      public static function §_-D1a§(param1:DisplayObject, param2:uint) : void
      {
         var _loc4_:int = 0;
         var _loc3_:int = §_-H5§.indexOf(param1);
         if(_loc3_ >= 0 && Boolean(§_-915§[_loc3_]))
         {
            _loc4_ = int(§_-915§[_loc3_].indexOf(param2));
            if(_loc4_ < 0)
            {
               return;
            }
            §_-915§[_loc3_].splice(_loc4_,1);
            §_-m1Y§[_loc3_] = §_-M2C§(_loc3_);
            §_-F3D§(param1 as DisplayObjectContainer,§_-m1Y§[_loc3_]);
         }
      }
      
      private static function §_-M2C§(param1:int) : uint
      {
         var _loc2_:Array = §_-915§[param1];
         if(!_loc2_ || _loc2_.length == 0)
         {
            return §_-62k§;
         }
         var _loc3_:int = int(_loc2_.length);
         var _loc4_:uint = uint(_loc2_[_loc3_ - 1]);
         var _loc5_:* = int(_loc3_ - 1);
         while(_loc5_--)
         {
            _loc4_ = §_-K1y§(_loc4_,_loc2_[_loc5_],0.6,true);
         }
         if(_loc4_ <= 0)
         {
            _loc4_ = §_-62k§;
         }
         return _loc4_;
      }
      
      public static function §_-K1y§(param1:uint, param2:uint = 4294967295, param3:Number = 0.5, param4:Boolean = false) : uint
      {
         if(param3 < 0 || param3 > 1)
         {
            param3 = 0.5;
         }
         var _loc5_:uint = uint(param1 >> 24 & 0xFF);
         var _loc6_:uint = uint(param1 >> 16 & 0xFF);
         var _loc7_:uint = uint(param1 >> 8 & 0xFF);
         var _loc8_:uint = uint(param1 >> 0 & 0xFF);
         var _loc9_:uint = uint(param2 >> 24 & 0xFF);
         var _loc10_:uint = uint(param2 >> 16 & 0xFF);
         var _loc11_:uint = uint(param2 >> 8 & 0xFF);
         var _loc12_:uint = uint(param2 >> 0 & 0xFF);
         var _loc13_:uint = uint(_loc5_ * param3 + _loc9_ * (1 - param3) & 0xFF);
         var _loc14_:uint = uint(_loc6_ * param3 + _loc10_ * (1 - param3) & 0xFF);
         var _loc15_:uint = uint(_loc7_ * param3 + _loc11_ * (1 - param3) & 0xFF);
         var _loc16_:uint = uint(_loc8_ * param3 + _loc12_ * (1 - param3) & 0xFF);
         return (param4 ? _loc13_ << 24 : 0) | _loc14_ << 16 | _loc15_ << 8 | _loc16_;
      }
      
      public static function §_-PN§(param1:DisplayObject) : void
      {
         var _loc2_:int = §_-H5§.indexOf(param1);
         if(_loc2_ >= 0)
         {
            §_-H5§.splice(_loc2_,1);
            §_-m1Y§.splice(_loc2_,1);
            §_-915§.splice(_loc2_,1);
         }
      }
      
      public static function §_-sN§() : void
      {
         var _loc1_:* = int(§_-H5§.length);
         while(_loc1_--)
         {
            §_-PN§(§_-H5§[_loc1_]);
         }
         §_-H5§ = [];
         §_-915§ = [];
         §_-m1Y§ = [];
      }
      
      public static function §_-73M§() : void
      {
         var _loc1_:* = int(§_-H5§.length);
         while(_loc1_--)
         {
            §_-F3D§(§_-H5§[_loc1_],§_-m1Y§[_loc1_]);
         }
      }
   }
}

