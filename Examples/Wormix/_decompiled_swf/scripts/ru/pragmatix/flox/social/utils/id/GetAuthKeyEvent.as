package ru.pragmatix.flox.social.utils.id
{
   import flash.events.Event;
   
   public class GetAuthKeyEvent extends Event
   {
      
      public static const GET_AUTH_KEY:String = "get_auth_key";
      
      private var _result:Boolean;
      
      public function GetAuthKeyEvent(param1:Boolean)
      {
         super(GET_AUTH_KEY);
         this._result = param1;
      }
      
      public function get result() : Boolean
      {
         return this._result;
      }
   }
}

