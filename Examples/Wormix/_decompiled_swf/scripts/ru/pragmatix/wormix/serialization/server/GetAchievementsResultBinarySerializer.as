package ru.pragmatix.wormix.serialization.server
{
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-x2Q§.§_-dU§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.GetAchievementsResult;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.events.Event;
   
   public class GetAchievementsResultBinarySerializer implements §_-D2L§
   {
      
      public function GetAchievementsResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 13003;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetAchievementsResult = GetAchievementsResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.profileId);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.achievementsIndex);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.achievements);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.boolAchievements);
         _loc5_.writeByte(_loc3_.investedAwardPoints);
         _loc5_.writeInt(_loc3_.timeSequence);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetAchievementsResult = new GetAchievementsResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.profileId = _loc4_.readUTF();
         _loc3_.achievementsIndex = SerializationUtils.readIntArray(_loc4_);
         _loc3_.achievements = SerializationUtils.readIntArray(_loc4_);
         _loc3_.boolAchievements = SerializationUtils.readIntArray(_loc4_);
         _loc3_.investedAwardPoints = _loc4_.readByte();
         _loc3_.timeSequence = _loc4_.readInt();
         var _loc5_:Boolean = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
         return _loc3_;
      }
   }
}

