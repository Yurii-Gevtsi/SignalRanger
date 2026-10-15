package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.IncreaseAchievementResult;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.DisplayObject;
   
   public class IncreaseAchievementResultBinarySerializer implements §_-D2L§
   {
      
      public function IncreaseAchievementResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 13004;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:IncreaseAchievementResult = IncreaseAchievementResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         SerializationUtils.writeIntArray(_loc5_,_loc3_.achievementsIndex);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.achievementsValues);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.thresholdAchievmensIndex);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.thresholdAchievmensOldValues);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.boolAchievements);
         _loc5_.writeUnsignedInt(_loc3_.timeSequence);
         _loc5_.writeByte(_loc3_.investedAwardPoints);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:IncreaseAchievementResult = new IncreaseAchievementResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.achievementsIndex = SerializationUtils.readIntArray(_loc4_);
         _loc3_.achievementsValues = SerializationUtils.readIntArray(_loc4_);
         _loc3_.thresholdAchievmensIndex = SerializationUtils.readIntArray(_loc4_);
         _loc3_.thresholdAchievmensOldValues = SerializationUtils.readIntArray(_loc4_);
         _loc3_.boolAchievements = SerializationUtils.readIntArray(_loc4_);
         _loc3_.timeSequence = _loc4_.readUnsignedInt();
         _loc3_.investedAwardPoints = _loc4_.readByte();
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

