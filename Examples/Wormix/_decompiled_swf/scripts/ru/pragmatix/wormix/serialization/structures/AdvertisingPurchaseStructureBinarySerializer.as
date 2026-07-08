package ru.pragmatix.wormix.serialization.structures
{
   import §_-21p§.§_-4w§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-02j§;
   import dragonBones.animation.WorldClock;
   import flash.net.Socket;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.response.LowerRankInClanResponse;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import starling.events.Event;
   
   public class AdvertisingPurchaseStructureBinarySerializer implements §_-02j§
   {
      
      public function AdvertisingPurchaseStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:AdvertisingPurchaseStructure = AdvertisingPurchaseStructure(param1);
         param2.writeUTF(_loc4_.name);
         param2.writeInt(_loc4_.nextTime);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ByteArray = null;
         _loc3_ = param1 as ByteArray;
         var _loc4_:String = _loc3_.readUTF();
         var _loc5_:int = _loc3_.readInt();
         return new AdvertisingPurchaseStructure(_loc4_,_loc5_);
      }
   }
}

