package ru.pragmatix.wormix.serialization.client
{
   import §_-21p§.§_-N1H§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.Cookie;
   import §_-DJ§.§_-T2G§;
   import §_-DJ§.§_-fH§;
   import §_-DJ§.§_-zb§;
   import §_-SP§.§_-M4§;
   import §_-Tm§.§_-RF§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y1Q§.§_-aG§;
   import §_-ZP§.§_-b23§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Oi§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-w2W§.§_-21w§;
   import com.greensock.*;
   import com.greensock.core.*;
   import feathers.events.FeathersEventType;
   import flash.events.*;
   import flash.system.LoaderContext;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpRetryCommandRequestClient;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   
   public class PvpRetryCommandRequestClientBinarySerializer implements §_-D2L§
   {
      
      public function PvpRetryCommandRequestClientBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1018;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:PvpRetryCommandRequestClient = PvpRetryCommandRequestClient(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.commandNum);
         _loc5_.writeShort(_loc3_.turnNum);
         _loc5_.writeInt(_loc3_.battleId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:PvpRetryCommandRequestClient = new PvpRetryCommandRequestClient();
         _loc4_.commandNum = _loc3_.readShort();
         _loc4_.turnNum = _loc3_.readShort();
         _loc4_.battleId = _loc3_.readInt();
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

