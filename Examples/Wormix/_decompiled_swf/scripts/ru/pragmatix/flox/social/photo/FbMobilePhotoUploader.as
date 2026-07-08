package ru.pragmatix.flox.social.photo
{
   import §_-21p§.§_-4w§;
   import §_-C3f§.§_-1Y§;
   import §_-D3A§.§_-TZ§;
   import §_-E1I§.§_-b2n§;
   import §_-Y1O§.*;
   import §_-j1w§.§_-B2i§;
   import §_-l1g§.§_-L3Q§;
   import §_-p24§.*;
   import §_-rM§.§_-61j§;
   import flash.display.BitmapData;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.response.photo.PhotoSaveResponse;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.RepeatPvpBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.*;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   
   public class FbMobilePhotoUploader implements IPhotoUploader
   {
      
      private var _socialController:ISocialController;
      
      private var _callback:Function = null;
      
      public function FbMobilePhotoUploader(param1:ISocialController)
      {
         super();
         this._socialController = param1;
      }
      
      public function savePhoto(param1:BitmapData, param2:String, param3:Function) : void
      {
         this.executeCallback(param3,false);
      }
      
      private function onFacebookResponse(param1:*) : void
      {
         this.executeCallback(this._callback,false);
         this._callback = null;
      }
      
      private function executeCallback(param1:Function, param2:Boolean) : void
      {
         if(param1 != null)
         {
            param1(new PhotoSaveResponse(param2));
         }
      }
   }
}

