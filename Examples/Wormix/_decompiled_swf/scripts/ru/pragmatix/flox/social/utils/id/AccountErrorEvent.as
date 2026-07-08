package ru.pragmatix.flox.social.utils.id
{
   import flash.events.ErrorEvent;
   
   public class AccountErrorEvent extends ErrorEvent
   {
      
      public static const ACCOUNT_ERROR:String = "ACCOUNT_ERROR";
      
      private var _errorCode:int;
      
      public function AccountErrorEvent(param1:int)
      {
         super(ACCOUNT_ERROR);
         this._errorCode = param1;
      }
      
      public function get errorCode() : int
      {
         return this._errorCode;
      }
   }
}

