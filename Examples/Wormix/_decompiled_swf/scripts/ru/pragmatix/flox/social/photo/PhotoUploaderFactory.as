package ru.pragmatix.flox.social.photo
{
   import §_-23P§.§_-u1z§;
   import §_-82l§.§_-1I§;
   import §_-9§.§_-52A§;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-J2l§;
   import §_-E1I§.§_-b2n§;
   import §_-H3§.*;
   import §_-d2p§.*;
   import §_-p18§.§_-vd§;
   import §_-sQ§.§_-737§;
   import feathers.controls.List;
   import flash.display.DisplayObject;
   import flash.system.Capabilities;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.display.Quad;
   
   public class PhotoUploaderFactory
   {
      
      public function PhotoUploaderFactory()
      {
         super();
      }
      
      public static function getPhotoUploader(param1:String, param2:String, param3:DisplayObject, param4:String, param5:ISocialController) : IPhotoUploader
      {
         var _loc6_:String = Capabilities.version.substr(0,3);
         var _loc7_:Boolean = _loc6_ == "AND" || _loc6_ == "IOS";
         switch(param5.getType())
         {
            case SocialType.VK:
               if(_loc7_)
               {
                  return new VkMobilePhotoUploader(param5);
               }
            case SocialType.FACEBOOK:
               if(_loc7_)
               {
                  return new FbMobilePhotoUploader(param5);
               }
         }
         return new EmptyMobilePhotoUploader();
      }
      
      public static function uploadAvailable(param1:ISocialController) : Boolean
      {
         return getPhotoUploader("test","test",null,"test",param1) != null;
      }
   }
}

