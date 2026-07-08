package ru.pragmatix.wormix.utils
{
   import §_-A1K§.§_-7u§;
   import §_-DJ§.§_-J2l§;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-P1B§.§_-b21§;
   import §_-Vg§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-rM§.§_-61j§;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import feathers.core.FeathersControl;
   import flash.geom.Point;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.social.utils.id.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.PumpReactionTheHouseResult;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.weapons.ammo.AirMissile;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class StringUtil
   {
      
      public static const §_-43t§:String = "N/A";
      
      public static const §_-K3n§:String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
      
      public static const §_-n1E§:String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      
      public function StringUtil()
      {
         super();
      }
      
      public static function trim(param1:String) : String
      {
         return param1.replace(/^\s*|\s*$/g,"");
      }
      
      public static function §_-32B§(param1:String) : String
      {
         param1 = param1.replace(/&nbsp;/g," ");
         param1 = param1.replace(/&shy;/g," ");
         param1 = param1.replace(/&ensp;/g," ");
         param1 = param1.replace(/&emsp;/g," ");
         param1 = param1.replace(/&thinsp;/g," ");
         param1 = param1.replace(/&#00000013/g," ");
         return param1.replace(/\s{2,}/g," ");
      }
      
      public static function §_-a1p§(param1:int) : String
      {
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc8_:int = 0;
         var _loc2_:uint = 3;
         var _loc3_:String = param1 < 0 ? "-" : "";
         var _loc4_:String = Math.abs(param1).toString();
         var _loc5_:String = "";
         if(_loc4_.length > _loc2_)
         {
            _loc6_ = Math.floor((_loc4_.length - 1) / _loc2_);
            _loc7_ = _loc4_.length - _loc6_ * _loc2_;
            if(_loc7_ != 0)
            {
               _loc5_ = _loc4_.slice(0,_loc7_);
            }
            _loc8_ = 0;
            while(_loc8_ < _loc6_)
            {
               _loc5_ += " " + _loc4_.substr(_loc7_ + _loc8_ * _loc2_,_loc2_);
               _loc8_++;
            }
         }
         else
         {
            _loc5_ = _loc4_;
         }
         return _loc3_ + _loc5_;
      }
      
      public static function §_-jT§(param1:String, param2:uint) : String
      {
         if(Boolean(param1) && Boolean(param2 != -1) && param1.length > param2)
         {
            return param1.substring(0,param2) + "...";
         }
         return param1;
      }
      
      public static function §_-CN§(param1:String, param2:String) : String
      {
         return "<font color=\"" + param2 + "\">" + param1 + "</font>";
      }
      
      public static function format(param1:String, param2:Object = null) : String
      {
         var _loc3_:String = null;
         var _loc4_:String = null;
         if(Boolean(param1) && Boolean(param2))
         {
            param1 = §_-s2a§.§_-y2o§(param1);
            for(_loc3_ in param2)
            {
               _loc4_ = param2[_loc3_] != null && param2[_loc3_] != undefined ? param2[_loc3_].toString() : "";
               param1 = param1.replace("{" + _loc3_ + "}",_loc4_);
            }
         }
         return param1;
      }
      
      public static function §_-u12§(param1:uint, param2:String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=") : String
      {
         var _loc3_:String = "";
         var _loc4_:int = param2.length - 1;
         var _loc5_:int = 0;
         while(_loc5_ < param1)
         {
            _loc3_ += param2.charAt(Math.floor(_loc4_ * Math.random()));
            _loc5_++;
         }
         return _loc3_;
      }
   }
}

