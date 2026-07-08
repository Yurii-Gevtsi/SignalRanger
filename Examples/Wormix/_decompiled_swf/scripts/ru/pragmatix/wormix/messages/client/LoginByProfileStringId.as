package ru.pragmatix.wormix.messages.client
{
   import §_-21p§.§_-4w§;
   import §_-31N§.§_-42G§;
   import §_-51p§.§_-W2e§;
   import §_-62§.§_-W1r§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-J31§.§_-R1h§;
   import §_-SP§.§_-M4§;
   import §_-Tm§.§_-RF§;
   import §_-Z1w§.§_-s1x§;
   import §_-hO§.SupplyPickedUpEvent;
   import §_-hO§.§_-v24§;
   import §_-r1C§.*;
   import §_-w2i§.§_-Zd§;
   import §_-ws§.§_-B3I§;
   import com.distriqt.extension.application.Application;
   import com.hurlant.crypto.tls.*;
   import feathers.controls.Button;
   import flash.events.MouseEvent;
   import flash.events.ProgressEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-fT§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class LoginByProfileStringId
   {
      
      public var id:String;
      
      public var referrerId:String = "";
      
      public var authKey:String;
      
      public var version:int;
      
      public var ids:Array;
      
      public var socialCode:uint;
      
      public var params:Array = [];
      
      public var offlineBattles:Array = [];
      
      public function LoginByProfileStringId()
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
   }
}

