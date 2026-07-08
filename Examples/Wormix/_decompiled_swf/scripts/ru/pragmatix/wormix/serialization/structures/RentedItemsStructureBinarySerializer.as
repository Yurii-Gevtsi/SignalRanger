package ru.pragmatix.wormix.serialization.structures
{
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-H16§.*;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-rM§.§_-02j§;
   import §_-v2t§.§_-73F§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getQualifiedClassName;
   import org.swiftsuspenders.Injector;
   import org.swiftsuspenders.§_-71M§;
   import org.swiftsuspenders.§_-T1p§;
   import ru.pragmatix.flox.social.model.IUserProfile;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class RentedItemsStructureBinarySerializer implements §_-02j§
   {
      
      public function RentedItemsStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:RentedItemsStructure = new RentedItemsStructure();
         _loc3_.activeUntil = param1.readInt();
         _loc3_.weapons = SerializationUtils.readShortArray(param1 as ByteArray);
         _loc3_.stuff = SerializationUtils.readShortArray(param1 as ByteArray);
         return _loc3_;
      }
   }
}

