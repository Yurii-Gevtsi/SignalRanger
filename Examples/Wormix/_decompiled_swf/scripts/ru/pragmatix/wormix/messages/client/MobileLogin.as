package ru.pragmatix.wormix.messages.client
{
   import §_-21p§.§_-4w§;
   import §_-51p§.§_-W2e§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-Y1Q§.§_-61Z§;
   import §_-Z1K§.*;
   import §_-hO§.SupplyPickedUpEvent;
   import §_-j1w§.*;
   import §_-j22§.§_-5g§;
   import §_-j2R§.§_-73G§;
   import §_-l1g§.§_-L3Q§;
   import §_-sK§.*;
   import §_-z20§.§_-V7§;
   import §_-z20§.§_-p4§;
   import com.hurlant.util.der.§_-p1B§;
   import config.§_-vR§;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-I3A§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.Canvas;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class MobileLogin
   {
      
      public var deviceId:String;
      
      public var authKey:String;
      
      public var version:int;
      
      public var platformId:uint;
      
      public var params:Array = [];
      
      public var offlineBattles:Array = [];
      
      public function MobileLogin()
      {
         super();
      }
      
      public static function versionFromString(param1:String) : int
      {
         var _loc2_:Array = param1.split(".");
         var _loc3_:int = parseInt(_loc2_[3]);
         _loc3_ |= parseInt(_loc2_[2]) << 8;
         _loc3_ |= parseInt(_loc2_[1]) << 16;
         return _loc3_ | parseInt(_loc2_[0]) << 24;
      }
      
      public static function versionToString(param1:int) : String
      {
         var _loc2_:String = "" + (param1 & 0xFF);
         _loc2_ = (param1 >>> 8 & 0xFF) + "." + _loc2_;
         _loc2_ = (param1 >>> 16 & 0xFF) + "." + _loc2_;
         return (param1 >>> 24 & 0xFF) + "." + _loc2_;
      }
      
      public function toString() : String
      {
         return "MobileLogin{" + "deviceId=" + this.deviceId + ",platformId=" + this.platformId + ",version=" + this.version + ",authKey=" + this.authKey + ",params=" + this.params + ",offlineBattles=" + this.offlineBattles + "}";
      }
   }
}

