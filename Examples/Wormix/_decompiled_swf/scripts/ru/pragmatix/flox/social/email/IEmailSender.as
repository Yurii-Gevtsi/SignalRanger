package ru.pragmatix.flox.social.email
{
   public interface IEmailSender
   {
      
      function isAvailable() : Boolean;
      
      function sendEmail(param1:String, param2:String, param3:String, param4:Function) : void;
   }
}

