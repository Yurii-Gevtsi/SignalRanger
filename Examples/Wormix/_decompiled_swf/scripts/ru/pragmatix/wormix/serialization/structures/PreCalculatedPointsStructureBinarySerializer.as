package ru.pragmatix.wormix.serialization.structures
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-D3A§.§_-TZ§;
   import §_-F39§.*;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.§_-Z25§;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import §_-z1g§.§_-M2v§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.messages.server.ChooseBonusItemResult;
   import ru.pragmatix.wormix.messages.structures.PreCalculatedPoints;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   
   public class PreCalculatedPointsStructureBinarySerializer implements §_-02j§
   {
      
      public function PreCalculatedPointsStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:PreCalculatedPoints = PreCalculatedPoints(param1);
         param2.writeInt(_loc4_.ratingPoints);
         param2.writeInt(_loc4_.rankPoints);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:PreCalculatedPoints = null;
         _loc3_ = new PreCalculatedPoints();
         _loc3_.ratingPoints = param1.readInt();
         _loc3_.rankPoints = param1.readInt();
         return _loc3_;
      }
   }
}

