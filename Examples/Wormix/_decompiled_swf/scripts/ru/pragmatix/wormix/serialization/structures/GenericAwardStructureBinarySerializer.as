package ru.pragmatix.wormix.serialization.structures
{
   import §_-A1K§.§_-TA§;
   import §_-C2t§.*;
   import §_-CR§.§_-S2o§;
   import §_-Y1O§.*;
   import §_-YF§.§_-q2v§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-kP§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-02j§;
   import §_-z1g§.§_-y1S§;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class GenericAwardStructureBinarySerializer implements §_-02j§
   {
      
      public function GenericAwardStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:GenericAwardStructure = new GenericAwardStructure();
         _loc3_.awardKind = param1.readShort();
         _loc3_.count = param1.readInt();
         _loc3_.itemId = param1.readInt();
         return _loc3_;
      }
   }
}

