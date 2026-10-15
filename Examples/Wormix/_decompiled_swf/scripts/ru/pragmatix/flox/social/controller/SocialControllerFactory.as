package ru.pragmatix.flox.social.controller
{
   import §_-62§.§_-zF§;
   import §_-T2h§.*;
   import flash.display.DisplayObjectContainer;
   import flash.system.Capabilities;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.mobile.EmptyMobileSocialController;
   import ru.pragmatix.flox.social.controller.mobile.FbMobileSocialController;
   import ru.pragmatix.flox.social.controller.mobile.VkMobileSocialController;
   import ru.pragmatix.flox.social.controller.steam.SteamSocialController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import starling.events.Event;
   
   public class SocialControllerFactory
   {
      
      public function SocialControllerFactory()
      {
         super();
      }
      
      public static function create(param1:SocialAppContext, param2:DisplayObjectContainer = null) : ISocialController
      {
         var _loc3_:String = Capabilities.version.substr(0,3);
         var _loc4_:Boolean = _loc3_ == "AND" || _loc3_ == "IOS";
         switch(param1.getSocialType())
         {
            case SocialType.VK:
               if(_loc4_)
               {
                  return new VkMobileSocialController(param1);
               }
               return null;
               break;
            case SocialType.FACEBOOK:
               if(_loc4_)
               {
                  return new FbMobileSocialController(param1);
               }
               return null;
               break;
            case SocialType.STEAM:
               if(!_loc4_)
               {
                  return new SteamSocialController(param1);
               }
               return null;
               break;
            case SocialType.NONE:
               return new EmptyMobileSocialController(param1);
            default:
               return null;
         }
      }
   }
}

