package ru.pragmatix.wormix.social.utils
{
   import config.§_-vR§;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-c2§;
   
   public class FacebookTracking
   {
      
      private static var _isEnabled:Boolean;
      
      public function FacebookTracking()
      {
         super();
      }
      
      public static function trackInitCheckout(param1:PaymentOption, param2:String) : void
      {
      }
      
      public static function trackPurchased(param1:PaymentOption, param2:String) : void
      {
      }
      
      private static function trackEventPayment(param1:String, param2:PaymentOption, param3:String) : void
      {
         var eventName:String = param1;
         var po:PaymentOption = param2;
         var platformProductId:String = param3;
         if(§_-vR§.§_-42H§ && _isEnabled)
         {
            try
            {
            }
            catch(err:Error)
            {
            }
         }
      }
      
      private static function fillEventPayment(param1:*, param2:PaymentOption, param3:String) : void
      {
         var _loc4_:PaymentOption = §_-N1V§.§_-42k§.§_-B12§(param2.name);
         var _loc5_:Number = §_-c2§.§_-JT§(_loc4_);
         var _loc6_:int = param2.count + param2.bonus;
         var _loc7_:String = "USD";
         param1.setParameter("net_income",_loc5_.toFixed(2));
      }
      
      public static function set isEnabled(param1:Boolean) : void
      {
         _isEnabled = param1;
      }
   }
}

