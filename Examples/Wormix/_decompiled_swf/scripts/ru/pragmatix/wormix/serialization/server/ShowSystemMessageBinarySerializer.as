package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-7u§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-YF§.§_-Zi§;
   import §_-gG§.§_-M2S§;
   import §_-j22§.§_-5g§;
   import §_-m1g§.§_-N1y§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-Ia§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.server.GetRatingResult;
   import ru.pragmatix.wormix.messages.server.ShowSystemMessage;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.StaticCharge;
   
   public class ShowSystemMessageBinarySerializer implements §_-D2L§
   {
      
      public function ShowSystemMessageBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10011;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ShowSystemMessage = ShowSystemMessage(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.msg);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ShowSystemMessage = new ShowSystemMessage();
         _loc3_.msg = param1.readUTF();
         return _loc3_;
      }
   }
}

