package ru.pragmatix.wormix.serialization.clans.structures
{
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-G2H§.*;
   import §_-rM§.§_-02j§;
   import §_-v2t§.§_-73F§;
   import com.distriqt.extension.inappbilling.Purchase;
   import flash.net.SharedObject;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.request.treasury.CashMedalsClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.Image;
   
   public class ClanNewsTOBinarySerializer implements §_-02j§
   {
      
      public function ClanNewsTOBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ClanNewsTO = new ClanNewsTO();
         _loc3_.logDate = param1.readInt();
         _loc3_.publisherSocialId = param1.readShort();
         _loc3_.publisherProfileId = param1.readInt();
         _loc3_.publisherName = param1.readUTF();
         _loc3_.text = param1.readUTF();
         return _loc3_;
      }
   }
}

