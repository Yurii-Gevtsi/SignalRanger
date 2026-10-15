package ru.pragmatix.wormix.serialization.clans.structures
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-g1P§.§_-L38§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-02j§;
   import §_-s20§.§_-J2g§;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   
   public class ClanTOBinarySerializer implements §_-02j§
   {
      
      public function ClanTOBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ClanTO = new ClanTO();
         _loc3_.id = param1.readInt();
         _loc3_.name = param1.readUTF();
         _loc3_.level = param1.readInt();
         _loc3_.size = param1.readInt();
         _loc3_.rating = param1.readInt();
         _loc3_.seasonRating = param1.readInt();
         _loc3_.joinRating = param1.readInt();
         _loc3_.createDate = param1.readInt();
         var _loc4_:int = param1.readShort();
         var _loc5_:ByteArray = new ByteArray();
         param1.readBytes(_loc5_,0,_loc4_);
         SerializationUtils.readUintVector(_loc5_,_loc3_.emblem);
         _loc3_.clanDescription = param1.readUTF();
         _loc3_.reviewState = param1.readShort();
         _loc3_.topPlace = param1.readInt();
         _loc3_.prevSeasonTopPlace = param1.readInt();
         _loc3_.members = Vector.<ClanMemberTO>(SerializationUtils.readStructureVector(param1 as ByteArray,ClanMemberTO));
         _loc3_.isExitClosed = param1.readBoolean();
         _loc3_.treasury = param1.readInt();
         _loc3_.medalPrice = param1.readByte();
         _loc3_.cashedMedals = param1.readInt();
         return _loc3_;
      }
   }
}

