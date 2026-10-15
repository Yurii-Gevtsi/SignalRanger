package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-W1r§;
   import §_-B1y§.§_-MQ§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.*;
   import §_-fo§.§_-r5§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.PurchaseRequest;
   import flash.display.MovieClip;
   import flash.events.TimerEvent;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.server.GetJoinClansMainRequest;
   import ru.pragmatix.wormix.messages.clans.request.AuditActionsRequest;
   import ru.pragmatix.wormix.messages.clans.response.AuditActionsResponse;
   import ru.pragmatix.wormix.messages.clans.response.GetJoinClansMainResponse;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.messages.client.SteamTransactionInitRequest;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.core.Starling;
   import starling.events.KeyboardEvent;
   
   public class SteamTransactionInitRequestBinarySerializer implements §_-D2L§
   {
      
      public function SteamTransactionInitRequestBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 6001;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SteamTransactionInitRequest = SteamTransactionInitRequest(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.productCode);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SteamTransactionInitRequest = new SteamTransactionInitRequest();
         _loc3_.productCode = param1.readUTF();
         return _loc3_;
      }
   }
}

