package ru.pragmatix.wormix.serialization.client
{
   import §_-12W§.*;
   import §_-73I§.*;
   import §_-Cl§.*;
   import §_-Vg§.§_-M11§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import feathers.events.FeathersEventType;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.client.NeedMoney;
   import ru.pragmatix.wormix.messages.server.BuyRaceResult;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.DisplayObject;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   public class NeedMoneyBinarySerializer implements §_-D2L§
   {
      
      public function NeedMoneyBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 2;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:NeedMoney = NeedMoney(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.value);
         _loc5_.writeInt(_loc3_.moneyType);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:NeedMoney = new NeedMoney();
         _loc3_.value = param1.readInt();
         _loc3_.moneyType = param1.readInt();
         return _loc3_;
      }
   }
}

