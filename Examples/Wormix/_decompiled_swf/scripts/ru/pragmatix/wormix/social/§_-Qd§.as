package ru.pragmatix.wormix.social
{
   import §_-B1y§.*;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-TF§.§_-q1j§;
   import §_-b1F§.§_-v2Q§;
   import §_-hO§.§_-03k§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillMissile;
   import ru.pragmatix.wormix.weapons.ammo.VerticalDrillStanding;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-Qd§
   {
      
      public function §_-Qd§()
      {
         super();
      }
      
      public static function §_-920§(param1:SocialType) : String
      {
         switch(param1)
         {
            case SocialType.NONE:
               return §_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_NONE_STATUS);
            case SocialType.STEAM:
               return §_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_STEAM_NAME);
            case SocialType.VK:
               return §_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_VK_NAME);
            case SocialType.FACEBOOK:
               return §_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_FB_NAME);
            default:
               return §_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_NONE_STATUS);
         }
      }
      
      public static function §_-72t§(param1:SocialType) : String
      {
         switch(param1)
         {
            case SocialType.NONE:
               return §_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_NONE_ACTION);
            case SocialType.VK:
               return §_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_VK_NAME);
            case SocialType.FACEBOOK:
               return §_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_FB_NAME);
            default:
               return §_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_NONE_ACTION);
         }
      }
      
      public static function §_-31Q§(param1:SocialType) : String
      {
         var _loc2_:String = "";
         switch(param1)
         {
            case SocialType.VK:
               _loc2_ = §_-s2a§.SOCIAL_VK_SHORT_NAME;
               break;
            case SocialType.FACEBOOK:
               _loc2_ = §_-s2a§.SOCIAL_FB_SHORT_NAME;
         }
         return _loc2_ ? §_-s2a§.§_-K3t§(_loc2_) : "";
      }
   }
}

