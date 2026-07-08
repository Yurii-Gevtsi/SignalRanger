package ru.pragmatix.wormix.controller.analytics
{
   import §_-ZB§.§_-W2Q§;
   import ru.pragmatix.flox.social.controller.SocialType;
   
   public class AnalyticsControllerFactory
   {
      
      public function AnalyticsControllerFactory()
      {
         super();
      }
      
      public static function create(param1:SocialType) : §_-W2Q§
      {
         var _loc2_:§_-W2Q§ = null;
         switch(param1)
         {
            case SocialType.MOBILE_IOS:
            case SocialType.MOBILE_ANDROID:
               _loc2_ = new AppsFlyerController();
         }
         if(_loc2_ == null)
         {
            _loc2_ = new DummyAnalyticsController();
         }
         return _loc2_;
      }
   }
}

