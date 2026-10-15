package ru.pragmatix.wormix.serialization.structures
{
   import §_-12W§.*;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-DJ§.§_-J2l§;
   import §_-K3r§.§_-E3O§;
   import §_-K3r§.§_-U2b§;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import feathers.controls.ScreenNavigator;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.screenOverlay.§_-O5§;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.messages.server.RefundPvpBattleLossResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.SocialLinkInfoStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.events.Event;
   
   public class SocialLinkInfoStructureBinarySerializer implements §_-02j§
   {
      
      public function SocialLinkInfoStructureBinarySerializer()
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
         var _loc4_:int = _loc3_.readShort();
         var _loc5_:String = _loc3_.readUTF();
         return new SocialLinkInfoStructure(_loc4_,_loc5_);
      }
   }
}

