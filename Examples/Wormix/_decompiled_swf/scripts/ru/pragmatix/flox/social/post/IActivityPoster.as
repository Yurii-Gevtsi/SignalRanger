package ru.pragmatix.flox.social.post
{
   public interface IActivityPoster extends IPoster
   {
      
      function activity(param1:String, param2:String = null, param3:String = null, param4:String = null) : void;
   }
}

