package ru.pragmatix.wormix.pvp.messages.server
{
   import §_-62§.*;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-C3f§.§_-Z1Y§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-J3L§.§_-h1D§;
   import §_-L2F§.§_-D2p§;
   import §_-N8§.§_-o2w§;
   import §_-RE§.§_-r2q§;
   import §_-TF§.§_-y2n§;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-qH§.§_-S25§;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.GetRatingResult;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.utils.§_-L2i§;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class PvpSystemMessage
   {
      
      public static const SYSTEM_MESSAGE_COLOR:String = "#919191";
      
      public static const MESSAGE_FONT_BEGIN:String = "<font color=\"";
      
      public static const MESSAGE_FONT_END:String = "\">";
      
      public static const SYSTEM_MESSAGE_END:String = "</font>";
      
      public static const UNDEFINED:int = -1;
      
      public static const DROPPED_BY_RECONNECTION_TIMEOUT:int = 1;
      
      public static const DROPPED_BY_COMMAND_TIMEOUT:int = 2;
      
      public static const DROPPED_BY_RESPONSE_TIMEOUT:int = 3;
      
      public static const SURRENDERED:int = 4;
      
      public static const CHEATER:int = 5;
      
      public static const DISCONNECTED:int = 6;
      
      public static const RECONNECTED:int = 7;
      
      public static const PLAYER_LONG_TIME_TURN:int = 8;
      
      public static const BATTLE_NOT_EXIST:int = 9;
      
      public static const PLAYER_NOT_FOUND_IN_BATTLE:int = 10;
      
      public static const PLAYER_JOINED_TO_BATTLE:int = 11;
      
      public static const PLAYER_REPEATED_COMMAND:int = 12;
      
      public static const PLAYER_SKIPPED_COMMAND:int = 14;
      
      public var type:int;
      
      public var playerNum:uint;
      
      public function PvpSystemMessage()
      {
         super();
      }
      
      public static function wrapSystemText(param1:String, param2:String = "#919191") : String
      {
         var _loc6_:int = 0;
         var _loc3_:String = param1;
         var _loc4_:String = "";
         var _loc5_:int = _loc3_.indexOf("<font");
         while(_loc5_ >= 0)
         {
            _loc6_ = _loc3_.indexOf(SYSTEM_MESSAGE_END) + SYSTEM_MESSAGE_END.length;
            _loc4_ = _loc3_.substring(0,_loc5_) + SYSTEM_MESSAGE_END + _loc3_.substring(_loc5_,_loc6_) + (_loc6_ == _loc3_.length ? "" : §_-V1t§(param2));
            _loc3_ = _loc3_.substr(_loc6_);
            _loc5_ = _loc3_.indexOf("<font");
         }
         _loc4_ += _loc3_;
         return §_-V1t§(param2) + _loc4_ + SYSTEM_MESSAGE_END;
      }
      
      private static function §_-V1t§(param1:String) : String
      {
         return MESSAGE_FONT_BEGIN + param1 + MESSAGE_FONT_END;
      }
      
      public function toChatString() : String
      {
         var _loc1_:String = "";
         switch(this.type)
         {
            case 1:
               _loc1_ = §_-s2a§.SYSPVP_OPPONENT_OUT;
               break;
            case 2:
               _loc1_ = §_-s2a§.SYSPVP_INACTIVITY_DROP;
               break;
            case 3:
               _loc1_ = §_-s2a§.SYSPVP_UNCONFIRMED_EOT;
               break;
            case 4:
               _loc1_ = §_-s2a§.SYSPVP_OPPONENT_SURRENDER;
               break;
            case 5:
               _loc1_ = §_-s2a§.SYSPVP_OPPONENT_CHEATER;
               break;
            case 6:
               _loc1_ = §_-s2a§.SYSPVP_OPPONENT_LOST_CONNECTION;
               break;
            case 7:
               _loc1_ = §_-s2a§.SYSPVP_OPPONENT_RETURNED;
               break;
            case 8:
               _loc1_ = §_-s2a§.SYSPVP_LONG_TURN;
               break;
            case 9:
               _loc1_ = "";
               break;
            case 10:
               _loc1_ = "";
               break;
            case 11:
               _loc1_ = "";
               break;
            case 12:
               _loc1_ = "";
               break;
            case 14:
               _loc1_ = "";
               break;
            default:
               _loc1_ = §_-s2a§.SYSPVP_UNDEFINED;
         }
         return _loc1_ == "" ? "" : wrapSystemText(§_-s2a§.§_-K3t§(_loc1_));
      }
      
      public function isPlayerDropped() : Boolean
      {
         var _loc1_:Boolean = false;
         switch(this.type)
         {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
               _loc1_ = true;
         }
         return _loc1_;
      }
   }
}

