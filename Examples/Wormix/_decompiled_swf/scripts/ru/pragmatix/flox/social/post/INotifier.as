package ru.pragmatix.flox.social.post
{
   import ru.pragmatix.flox.social.model.IUserProfile;
   
   public interface INotifier extends IPoster
   {
      
      function notify(param1:IUserProfile, param2:String, param3:String, param4:Function = null, param5:String = null) : void;
      
      function returnFriendNotification(param1:IUserProfile, param2:String, param3:Function, param4:String = null) : void;
   }
}

