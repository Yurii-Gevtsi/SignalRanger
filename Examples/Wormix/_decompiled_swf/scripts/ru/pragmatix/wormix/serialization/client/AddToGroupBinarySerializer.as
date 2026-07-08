package ru.pragmatix.wormix.serialization.client
{
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-Tm§.§_-RF§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.client.AddToGroup;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.*;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class AddToGroupBinarySerializer implements §_-D2L§
   {
      
      public function AddToGroupBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 12;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:AddToGroup = AddToGroup(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.teamMemberId);
         _loc5_.writeShort(_loc3_.moneyType);
         _loc5_.writeShort(_loc3_.teamMemberType);
         _loc5_.writeInt(_loc3_.prevTeamMemberId);
         _loc5_.writeByte(_loc3_.active ? 1 : 0);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:AddToGroup = new AddToGroup();
         _loc3_.teamMemberId = param1.readInt();
         _loc3_.moneyType = param1.readShort();
         _loc3_.teamMemberType = param1.readShort();
         _loc3_.prevTeamMemberId = param1.readInt();
         _loc3_.active = param1.readByte() != 0;
         return _loc3_;
      }
   }
}

