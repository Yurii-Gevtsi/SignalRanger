package ru.pragmatix.wormix.serialization.clans.structures
{
   import §_-31W§.§_-Kb§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-L1H§.§_-b2K§;
   import §_-N8§.*;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-02j§;
   import §_-s20§.§_-J2g§;
   import feathers.controls.Button;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.structures.ClanAuditActionTO;
   import ru.pragmatix.wormix.messages.server.IncomingPayment;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   
   public class ClanAuditActionTOBinarySerializer implements §_-02j§
   {
      
      public function ClanAuditActionTOBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ClanAuditActionTO = new ClanAuditActionTO();
         _loc3_.date = param1.readInt();
         _loc3_.action = param1.readShort();
         _loc3_.publisherId = param1.readInt();
         _loc3_.memberId = param1.readInt();
         _loc3_.param = param1.readInt();
         return _loc3_;
      }
   }
}

