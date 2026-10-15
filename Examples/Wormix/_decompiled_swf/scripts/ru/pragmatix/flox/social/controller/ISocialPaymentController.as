package ru.pragmatix.flox.social.controller
{
   import ru.pragmatix.flox.social.model.PaymentOption;
   
   public interface ISocialPaymentController
   {
      
      function pay(param1:PaymentOption, param2:Function) : void;
   }
}

