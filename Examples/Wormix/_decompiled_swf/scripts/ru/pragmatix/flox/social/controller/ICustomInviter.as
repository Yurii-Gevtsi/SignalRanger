package ru.pragmatix.flox.social.controller
{
   public interface ICustomInviter
   {
      
      function getFriendsToInvite(param1:Function) : void;
      
      function inviteFriend(param1:String, param2:String, param3:Object, param4:Function) : void;
   }
}

