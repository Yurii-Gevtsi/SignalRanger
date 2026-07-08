package ru.pragmatix.wormix.serialization.structures
{
   import §_-91B§.§_-z2l§;
   import §_-dS§.§_-D3a§;
   import §_-j1w§.§_-B2i§;
   import §_-rM§.§_-02j§;
   import §_-u1T§.*;
   import §_-w2i§.§_-Zd§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.wormix.game.ai.§_-12u§;
   import ru.pragmatix.wormix.game.ai.§_-H3h§;
   import ru.pragmatix.wormix.game.ai.§_-dG§;
   import ru.pragmatix.wormix.game.ai.§_-n2q§;
   import ru.pragmatix.wormix.messages.structures.PumpReactionRateStructure;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   
   public class PumpReactionRateStructureBinarySerializer implements §_-02j§
   {
      
      public function PumpReactionRateStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:PumpReactionRateStructure = PumpReactionRateStructure(param1);
         param2.writeUnsignedInt(_loc4_.friendId);
         param2.writeByte(_loc4_.result);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:PumpReactionRateStructure = new PumpReactionRateStructure();
         _loc3_.friendId = param1.readUnsignedInt();
         _loc3_.result = param1.readByte();
         return _loc3_;
      }
   }
}

