package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-W1r§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-YF§.§_-q2v§;
   import §_-fo§.§_-r5§;
   import §_-j1w§.§_-B2i§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-Oi§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.distriqt.extension.inappbilling.Purchase;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalLayout;
   import flash.display.MovieClip;
   import flash.events.TimerEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.request.AuditActionsRequest;
   import ru.pragmatix.wormix.messages.clans.response.AuditActionsResponse;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpDropPlayer;
   import ru.pragmatix.wormix.pvp.messages.ex.*;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class PvpDropPlayerBinarySerializer implements §_-D2L§
   {
      
      public function PvpDropPlayerBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1006;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:PvpDropPlayer = PvpDropPlayer(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeByte(_loc3_.playerNum);
         _loc5_.writeUnsignedInt(_loc3_.battleId);
         _loc5_.writeShort(_loc3_.type);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:PvpDropPlayer = new PvpDropPlayer();
         _loc4_.playerNum = _loc3_.readByte();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.type = _loc3_.readShort();
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
         return _loc4_;
      }
   }
}

