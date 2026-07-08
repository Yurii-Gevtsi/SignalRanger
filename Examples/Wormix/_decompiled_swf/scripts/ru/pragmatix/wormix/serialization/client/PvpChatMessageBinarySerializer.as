package ru.pragmatix.wormix.serialization.client
{
   import §_-21p§.§_-4w§;
   import §_-62§.§_-O2H§;
   import §_-73d§.§_-pT§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-CF§.§_-p2U§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-P1z§.*;
   import §_-Tm§.Action;
   import §_-Tm§.§_-RF§;
   import §_-Y1Q§.§_-aG§;
   import §_-YF§.§_-Zi§;
   import §_-f1G§.§_-kf§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-p14§.§_-U1D§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.PvpChatMessage;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.ammo.feats.IFrameFeat;
   import starling.events.Event;
   
   use namespace gestouch_internal;
   
   public class PvpChatMessageBinarySerializer implements §_-D2L§
   {
      
      public function PvpChatMessageBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return PvpChatMessage.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:PvpChatMessage = PvpChatMessage(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeByte(_loc3_.playerNum);
         _loc5_.writeShort(_loc3_.action);
         _loc5_.writeUTF(_loc3_.message);
         _loc5_.writeUnsignedInt(_loc3_.battleId);
         _loc5_.writeBoolean(_loc3_.isTeamMsg);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:PvpChatMessage = new PvpChatMessage();
         _loc4_.playerNum = _loc3_.readByte();
         _loc4_.action = _loc3_.readShort();
         _loc4_.message = _loc3_.readUTF();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.isTeamMsg = _loc3_.readByte() != 0;
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
            return null;
         }
         return _loc4_;
      }
   }
}

