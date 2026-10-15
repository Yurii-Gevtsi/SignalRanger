package ru.pragmatix.flox.social.photo
{
   import §_-31W§.*;
   import §_-A2U§.§_-A23§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import flash.display.BitmapData;
   import flash.events.TimerEvent;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.response.photo.PhotoSaveResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class VkMobilePhotoUploader implements IPhotoUploader
   {
      
      private var _socialController:ISocialController;
      
      public function VkMobilePhotoUploader(param1:ISocialController)
      {
         super();
         this._socialController = param1;
      }
      
      public function savePhoto(param1:BitmapData, param2:String, param3:Function) : void
      {
         this.executeCallbackWithResult(param3,false);
      }
      
      private function executeCallbackWithResult(param1:Function, param2:Boolean) : void
      {
         if(param1 != null)
         {
            param1(new PhotoSaveResponse(param2));
         }
      }
   }
}

