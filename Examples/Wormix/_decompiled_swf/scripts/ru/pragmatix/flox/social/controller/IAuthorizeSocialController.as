package ru.pragmatix.flox.social.controller
{
   public interface IAuthorizeSocialController extends ISocialController
   {
      
      function login(param1:Function) : void;
      
      function logout() : void;
      
      function isLoggedIn() : Boolean;
      
      function dispose() : void;
   }
}

