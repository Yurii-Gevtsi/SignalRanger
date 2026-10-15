package ru.pragmatix.wormix.serialization.structures
{
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-02j§;
   import §_-u13§.MAC;
   import §_-z1g§.§_-F4§;
   import com.hurlant.crypto.§_-G37§;
   import com.hurlant.crypto.tls.*;
   import flash.geom.Point;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.structures.PositionStructure;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.display.DisplayObject;
   import starling.utils.ScaleMode;
   
   public class PositionStructureBinarySerializer implements §_-02j§
   {
      
      public function PositionStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:PositionStructure = PositionStructure(param1);
         param2.writeInt(_loc4_.x);
         param2.writeInt(_loc4_.y);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:PositionStructure = new PositionStructure();
         _loc3_.x = param1.readInt();
         _loc3_.y = param1.readInt();
         return _loc3_;
      }
   }
}

