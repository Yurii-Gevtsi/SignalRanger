package ru.pragmatix.flox.social.email
{
   import §_-5Y§.§_-t2O§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-G2H§.*;
   import §_-Ly§.§_-81L§;
   import §_-d2p§.EquipStatCell;
   import §_-fo§.§_-r5§;
   import §_-j22§.§_-5g§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.*;
   import com.greensock.loading.core.LoaderCore;
   import flash.display.MovieClip;
   import flash.events.Event;
   import flash.events.TimerEvent;
   import flash.system.Capabilities;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.§_-P1h§;
   import ru.pragmatix.wormix.messages.clans.request.AuditActionsRequest;
   import ru.pragmatix.wormix.messages.clans.response.AuditActionsResponse;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.messages.server.GetFriendListPageResult;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class EmailSenderFactory
   {
      
      public function EmailSenderFactory()
      {
         super();
      }
      
      public static function create() : IEmailSender
      {
         var _loc1_:String = Capabilities.version.substr(0,3);
         switch(_loc1_)
         {
            case "AND":
            case "IOS":
               return new MobileEmailSender();
            default:
               return new EmptyEmailSender();
         }
      }
   }
}

