package ru.pragmatix.wormix.utils
{
   import §_-31N§.§_-42G§;
   import §_-62§.§_-W1r§;
   import §_-B1y§.§_-bs§;
   import §_-Y1Q§.§_-61Z§;
   import §_-hO§.§_-v24§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import com.hurlant.crypto.tls.*;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.EventDispatcher;
   
   public class §_-TT§
   {
      
      public function §_-TT§()
      {
         super();
      }
      
      public static function §_-92P§(param1:§_-W1r§) : Image
      {
         var _loc2_:Image = null;
         var _loc3_:String = param1.§_-92P§() || param1.§_-G3a§();
         if(_loc3_)
         {
            _loc2_ = new Image(Application.assetManager.getTexture(_loc3_));
         }
         return _loc2_;
      }
   }
}

