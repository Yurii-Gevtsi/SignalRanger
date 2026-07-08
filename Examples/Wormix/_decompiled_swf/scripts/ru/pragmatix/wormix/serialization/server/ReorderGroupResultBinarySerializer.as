package ru.pragmatix.wormix.serialization.server
{
   import §_-l1K§.§_-W2f§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.messages.server.ReorderGroupResult;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.MolotovProjectile;
   
   public class ReorderGroupResultBinarySerializer implements §_-D2L§
   {
      
      public function ReorderGroupResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1022;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ReorderGroupResult = new ReorderGroupResult();
         _loc3_.result = param1.readShort();
         return _loc3_;
      }
   }
}

