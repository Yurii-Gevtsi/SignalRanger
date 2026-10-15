package ru.pragmatix.wormix.utils
{
   import §_-21p§.§_-4w§;
   import §_-22z§.§_-f1b§;
   import §_-43V§.*;
   import §_-51M§.Cat;
   import §_-51j§.*;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-T1y§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B2z§.Server;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-H16§.*;
   import §_-HD§.*;
   import §_-TF§.§_-P19§;
   import §_-Tm§.§_-RF§;
   import §_-Ty§.§_-SC§;
   import §_-Vg§.ClanEditView;
   import §_-Y1O§.§_-V23§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-YF§.§_-H2F§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-G4§;
   import §_-hO§.§_-03k§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-6a§;
   import §_-s20§.§_-J2g§;
   import §_-u1P§.ShopResultEvent;
   import §_-w§.§_-63m§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import §_-zI§.§_-F3q§;
   import §_-zI§.§_-X1q§;
   import com.greensock.loading.core.*;
   import feathers.controls.NumericStepper;
   import feathers.core.PopUpManager;
   import feathers.data.ListCollection;
   import feathers.layout.RelativePosition;
   import flash.display.Shape;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.URLVariables;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getDefinitionByName;
   import flash.utils.getQualifiedClassName;
   import org.swiftsuspenders.Injector;
   import org.swiftsuspenders.§_-71M§;
   import org.swiftsuspenders.§_-T1p§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-s1V§;
   import ru.pragmatix.wormix.messages.server.ConnectResult;
   import ru.pragmatix.wormix.messages.server.FastBattleResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreated;
   import ru.pragmatix.wormix.pvp.messages.ex.JoinToBattle;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.AirCatAmmo;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class MathUtil
   {
      
      public static const §_-M2N§:Number = Math.PI * 2;
      
      public static const PI_HALF:Number = Math.PI * 0.5;
      
      public static const PI_QUARTER:Number = Math.PI * 0.25;
      
      public static const §_-y2w§:Number = Math.PI / 3;
      
      public static const §_-CZ§:Number = Math.PI / 5;
      
      public static const PI_ONE_SIX:Number = Math.PI / 6;
      
      public static const §_-4S§:Number = Math.PI / 8;
      
      public static const §_-J3v§:Number = Math.PI / 10;
      
      public static const PI_ONE_12:Number = Math.PI / 12;
      
      public static const PI_ONE_16:Number = Math.PI / 16;
      
      public static const PI_ONE_18:Number = Math.PI / 18;
      
      public static const PI_ONE_20:Number = Math.PI / 20;
      
      public static const PI_ONE_24:Number = Math.PI / 24;
      
      public static const PI_ONE_30:Number = Math.PI / 30;
      
      public static const PI_ONE_36:Number = Math.PI / 36;
      
      public static const PI_ONE_40:Number = Math.PI / 40;
      
      public static const PI_ONE_48:Number = Math.PI / 48;
      
      public static const PI_ONE_60:Number = Math.PI / 60;
      
      public static const PI_ONE_64:Number = Math.PI / 64;
      
      public static const PI_ONE_80:Number = Math.PI / 80;
      
      public static const PI_ONE_90:Number = Math.PI / 90;
      
      public static const PI_ONE_100:Number = Math.PI / 100;
      
      public static const PI_ONE_144:Number = Math.PI / 144;
      
      public static const PI_ONE_180:Number = Math.PI / 180;
      
      public static const PI_ONE_288:Number = Math.PI / 288;
      
      public static const PI_ONE_360:Number = Math.PI / 360;
      
      public static const PI_ONE_512:Number = Math.PI / 512;
      
      public static const §_-s1S§:Number = 180 / Math.PI;
      
      public static const §_-k1q§:Number = Math.PI / 180;
      
      private static const §_-p15§:Number = 10000000;
      
      private static const §_-58§:Array = [-1,1];
      
      public function MathUtil()
      {
         super();
      }
      
      public static function §_-pw§(param1:Number, param2:Number, param3:Number, param4:Boolean) : Boolean
      {
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         if(param2 >= param3)
         {
            _loc5_ = param3;
            _loc6_ = param2;
         }
         else
         {
            _loc5_ = param2;
            _loc6_ = param3;
         }
         if(param4)
         {
            return param1 >= _loc5_ && param1 <= _loc6_;
         }
         return param1 > _loc5_ && param1 < _loc6_;
      }
      
      public static function §_-UW§(param1:uint, param2:Number, param3:Point = null) : Point
      {
         if(!param3)
         {
            param3 = Pool.getPoint();
         }
         param3.setTo(param1 * Math.cos(param2),param1 * Math.sin(param2));
         return param3;
      }
      
      public static function §_-m2X§(param1:Point, param2:Number) : void
      {
         var _loc3_:Number = Math.sin(param2);
         var _loc4_:Number = Math.cos(param2);
         rotateVector2(param1,_loc3_,_loc4_);
      }
      
      public static function rotateVector2(param1:Point, param2:Number, param3:Number) : void
      {
         var _loc4_:Number = param1.x;
         var _loc5_:Number = param1.y;
         param1.x = _loc4_ * param3 - _loc5_ * param2;
         param1.y = _loc4_ * param2 + _loc5_ * param3;
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
      
      public static function §_-Ih§(param1:Number, param2:Number, param3:Number, param4:Number) : Number
      {
         var _loc5_:Number = param3 - param1;
         var _loc6_:Number = param4 - param2;
         return _loc5_ * _loc5_ + _loc6_ * _loc6_;
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
               _loc8_ = §_-M2N§ + _loc8_;
            }
            else if(_loc8_ >= §_-M2N§)
            {
               _loc8_ -= §_-M2N§;
            }
         }
         return _loc8_;
      }
      
      public static function §_-M5§(param1:Number, param2:Number, param3:Number, param4:Number, param5:Boolean = true) : Number
      {
         var _loc6_:Number = param3 - param1;
         var _loc7_:Number = param4 - param2;
         var _loc8_:Number = Math.atan2(_loc7_,_loc6_) / PI_ONE_180;
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
      
      public static function §_-m5§(param1:§_-F3o§, param2:§_-F3o§) : Number
      {
         return Math.atan2(param2.y - param1.y,param2.x - param1.x);
      }
      
      public static function §_-oL§(param1:Number, param2:Number) : Number
      {
         return param1 / param2 * 100;
      }
      
      public static function §_-na§(param1:Number, param2:Number) : Number
      {
         return param1 * param2 / 100;
      }
      
      public static function §_-W1p§(param1:Number, param2:Number) : Number
      {
         return param2 + param1 * param2 / 100;
      }
      
      public static function equal(param1:Number, param2:Number) : Boolean
      {
         return Math.abs(param1 - param2) <= Physics.EPSILON;
      }
      
      public static function §_-f1V§(param1:Number) : Number
      {
         return param1 * §_-s1S§;
      }
      
      public static function §_-Z1G§(param1:Number) : Number
      {
         return param1 * §_-k1q§;
      }
      
      public static function §_-G1o§(param1:int, param2:int, param3:int, param4:int, param5:int, param6:int, param7:int) : Boolean
      {
         var _loc8_:Number = param1 - param5;
         var _loc9_:Number = param2 - param6;
         var _loc10_:Number = param3 - param5;
         var _loc11_:Number = param4 - param6;
         var _loc12_:Number = _loc10_ - _loc8_;
         var _loc13_:Number = _loc11_ - _loc9_;
         var _loc14_:Number = _loc12_ * _loc12_ + _loc13_ * _loc13_;
         var _loc15_:Number = 2 * (_loc8_ * _loc12_ + _loc9_ * _loc13_);
         var _loc16_:Number = _loc8_ * _loc8_ + _loc9_ * _loc9_ - param7 * param7;
         if(-_loc15_ < 0)
         {
            return _loc16_ < 0;
         }
         if(-_loc15_ < 2 * _loc14_)
         {
            return 4 * _loc14_ * _loc16_ - _loc15_ * _loc15_ < 0;
         }
         return _loc14_ + _loc15_ + _loc16_ < 0;
      }
      
      public static function §_-7h§(param1:int, param2:int, param3:int, param4:int, param5:int, param6:int, param7:Point) : void
      {
         var _loc8_:int = param3 - param1;
         var _loc9_:int = param4 - param2;
         var _loc10_:int = _loc8_ * _loc8_ + _loc9_ * _loc9_;
         var _loc11_:Number = ((param5 - param1) * _loc8_ + (param6 - param2) * _loc9_) / _loc10_;
         var _loc12_:int = param1 + _loc11_ * _loc8_;
         var _loc13_:int = param2 + _loc11_ * _loc9_;
         param7.setTo(_loc12_,_loc13_);
      }
      
      public static function §_-O1r§(param1:int, param2:int, param3:int, param4:int, param5:int, param6:int, param7:int, param8:int) : Boolean
      {
         var _loc9_:Number = param1 - param3;
         var _loc10_:Number = param5 - param7;
         var _loc11_:Number = param2 - param4;
         var _loc12_:Number = param6 - param8;
         var _loc13_:Number = _loc9_ * _loc12_ - _loc11_ * _loc10_;
         if(_loc13_ == 0)
         {
            return false;
         }
         _loc13_ = 1 / _loc13_;
         var _loc14_:Number = param1 * param4 - param2 * param3;
         var _loc15_:Number = param5 * param8 - param6 * param7;
         var _loc16_:Number = (_loc14_ * _loc10_ - _loc9_ * _loc15_) * _loc13_;
         var _loc17_:Number = (_loc14_ * _loc12_ - _loc11_ * _loc15_) * _loc13_;
         if(_loc16_ < Math.min(param1,param3) || _loc16_ > Math.max(param1,param3) || _loc16_ < Math.min(param5,param7) || _loc16_ > Math.max(param5,param7))
         {
            return false;
         }
         if(_loc17_ < Math.min(param2,param4) || _loc17_ > Math.max(param2,param4) || _loc17_ < Math.min(param6,param8) || _loc17_ > Math.max(param6,param8))
         {
            return false;
         }
         return true;
      }
      
      public static function §_-41E§(param1:Number, param2:Number = 0, param3:Number = 0) : Point
      {
         var _loc4_:Number = §_-X1j§.randomSeed.§_-y2j§() * §_-M2N§;
         var _loc5_:Number = §_-X1j§.randomSeed.§_-y2j§() * param1;
         return Pool.getPoint(_loc5_ * Math.cos(_loc4_) + param2,_loc5_ * Math.sin(_loc4_) + param3);
      }
      
      public static function §_-b28§(param1:Number, param2:int) : Number
      {
         var _loc3_:int = Math.pow(10,param2);
         return Math.round(param1 * _loc3_) / _loc3_;
      }
      
      public static function §_-T1l§(param1:Number, param2:int) : Number
      {
         return Math.round(param1 * 10 * param2) / (10 * param2);
      }
      
      public static function §_-F1V§(param1:Number) : Number
      {
         return Math.round(param1 * §_-p15§) / §_-p15§;
      }
      
      public static function §_-l2u§(param1:Number, param2:Number, param3:Number) : Number
      {
         return MathUtil.§_-J2q§(param1,param2) < MathUtil.§_-J2q§(param1,param3) ? param2 : param3;
      }
      
      public static function §_-J2q§(param1:Number, param2:Number) : Number
      {
         param1 = normalizeAngle(param1);
         param2 = normalizeAngle(param2);
         var _loc3_:Number = param2 - param1;
         _loc3_ += _loc3_ > Math.PI ? -§_-M2N§ : (_loc3_ < -Math.PI ? §_-M2N§ : 0);
         return Math.abs(_loc3_);
      }
      
      public static function normalizeAngle(param1:Number) : Number
      {
         while(param1 < -Math.PI)
         {
            param1 += §_-M2N§;
         }
         while(param1 > Math.PI)
         {
            param1 -= §_-M2N§;
         }
         return param1;
      }
      
      public static function §_-v1x§() : int
      {
         return §_-58§[§_-X1j§.randomSeed.§_-Fh§(0,1)];
      }
      
      [Inline]
      public static function §_-Z2Y§(param1:Number, param2:Number, param3:Number) : Number
      {
         return param1 + (param2 - param1) * param3;
      }
   }
}

