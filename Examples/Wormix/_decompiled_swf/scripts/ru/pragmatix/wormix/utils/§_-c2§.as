package ru.pragmatix.wormix.utils
{
   import §_-43V§.*;
   import §_-B1y§.*;
   import §_-B2z§.§_-82u§;
   import §_-RE§.§_-V1m§;
   import §_-RE§.§_-r2q§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1b§.*;
   import §_-aM§.§_-C1e§;
   import §_-aM§.§_-P1N§;
   import §_-aM§.§_-tX§;
   import §_-l1g§.§_-L3Q§;
   import §_-z1g§.§_-y1S§;
   import com.hurlant.util.§_-a2P§;
   import flash.errors.IllegalOperationError;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.social.common.post.*;
   
   public class §_-c2§
   {
      
      private static const §_-c2d§:Number = 0.7;
      
      public function §_-c2§()
      {
         super();
      }
      
      public static function §_-JT§(param1:PaymentOption) : Number
      {
         return §_-c2§.§_-c2d§ * param1.price;
      }
      
      public static function §_-X2h§(param1:PaymentOption) : String
      {
         var _loc2_:String = null;
         if(param1.name.indexOf("clan_donation") > -1)
         {
            _loc2_ = "clan_treasury_ruby";
         }
         else if(param1.name.indexOf("instant_ruby") > -1)
         {
            _loc2_ = "instant_ruby";
         }
         else if(param1.name.indexOf("instant_fuzy") > -1)
         {
            _loc2_ = "instant_fuzy";
         }
         else if(param1.name.indexOf("deposit_ruby") > -1)
         {
            _loc2_ = "deposit_ruby";
         }
         else if(param1.name.indexOf("deposit_fuzy") > -1)
         {
            _loc2_ = "deposit_fuzy";
         }
         else if(param1.name.indexOf("sticker_pack") > -1)
         {
            _loc2_ = "sticker_pack";
         }
         else if(param1.name.indexOf("craft_box") > -1)
         {
            _loc2_ = "craft_box";
         }
         else if(param1.name.indexOf("autorenew") > -1)
         {
            _loc2_ = "vip_autorenew";
         }
         else if(param1.name.indexOf("vip") > -1)
         {
            _loc2_ = "vip";
         }
         else
         {
            _loc2_ = param1.name;
         }
         return _loc2_;
      }
   }
}

