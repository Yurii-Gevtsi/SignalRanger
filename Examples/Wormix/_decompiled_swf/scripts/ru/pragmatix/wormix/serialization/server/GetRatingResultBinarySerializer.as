package ru.pragmatix.wormix.serialization.server
{
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.messages.server.GetRatingResult;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   
   public class GetRatingResultBinarySerializer implements §_-D2L§
   {
      
      public function GetRatingResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10019;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc8_:ByteArray = null;
         var _loc3_:GetRatingResult = GetRatingResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         var _loc6_:uint = 0;
         var _loc7_:uint = _loc3_.userProfileStructures.length;
         _loc5_.writeShort(_loc7_);
         _loc6_ = 0;
         while(_loc6_ < _loc7_)
         {
            _loc8_ = new ByteArray();
            §_-616§.§_-fk§(RatingProfileStructure).serializeStructure(_loc3_.userProfileStructures[_loc6_],_loc8_,null);
            _loc5_.writeShort(_loc8_.length);
            _loc5_.writeBytes(_loc8_);
            _loc6_++;
         }
         _loc5_.writeInt(_loc3_.rating);
         _loc5_.writeInt(_loc3_.oldPlace);
         _loc5_.writeInt(_loc3_.place);
         _loc5_.writeInt(_loc3_.ratingType);
         _loc5_.writeInt(_loc3_.battleWager);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetRatingResult = new GetRatingResult();
         var _loc4_:uint = 0;
         var _loc5_:uint = param1.readUnsignedShort();
         _loc3_.userProfileStructures = [];
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            param1.readShort();
            _loc3_.userProfileStructures[_loc4_] = §_-616§.§_-fk§(RatingProfileStructure).deserializeStructure(param1,null);
            _loc4_++;
         }
         _loc3_.rating = param1.readInt();
         _loc3_.oldPlace = param1.readInt();
         _loc3_.place = param1.readInt();
         _loc3_.ratingType = param1.readShort();
         _loc3_.battleWager = param1.readShort();
         return _loc3_;
      }
   }
}

