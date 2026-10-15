package ru.pragmatix.wormix.serialization.structures
{
   import §_-A1K§.§_-TA§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-02j§;
   import §_-w2i§.§_-Ia§;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import flash.filters.*;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.messages.server.GetRatingResult;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.RestrictionItemStructure;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.server.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.extensions.ParticleAura;
   
   public class RestrictionItemStructureBinarySerializer implements §_-02j§
   {
      
      public function RestrictionItemStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ByteArray = null;
         _loc3_ = param1 as ByteArray;
         var _loc4_:int = _loc3_.readInt();
         var _loc5_:int = _loc3_.readInt();
         var _loc6_:Vector.<int> = SerializationUtils.readByteVector(_loc3_);
         return new RestrictionItemStructure(_loc4_,_loc5_,_loc6_);
      }
   }
}

