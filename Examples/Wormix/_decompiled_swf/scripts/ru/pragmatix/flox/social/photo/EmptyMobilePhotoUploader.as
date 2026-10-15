package ru.pragmatix.flox.social.photo
{
   import §_-5Y§.§_-t2O§;
   import §_-91A§.§_-sH§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import com.greensock.loading.LoaderStatus;
   import flash.display.BitmapData;
   import flash.events.Event;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.structures.*;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.cloud.BasicCloud;
   
   public class EmptyMobilePhotoUploader implements IPhotoUploader
   {
      
      public function EmptyMobilePhotoUploader()
      {
         super();
      }
      
      public function savePhoto(param1:BitmapData, param2:String, param3:Function) : void
      {
      }
   }
}

