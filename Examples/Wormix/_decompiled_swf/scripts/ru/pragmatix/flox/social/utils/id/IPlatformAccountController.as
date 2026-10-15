package ru.pragmatix.flox.social.utils.id
{
   import ru.pragmatix.flox.social.controller.SocialType;
   
   public interface IPlatformAccountController
   {
      
      function init() : void;
      
      function getUserId() : String;
      
      function getUserIdByType(param1:String) : String;
      
      function setUserIdToStore(param1:String, param2:String) : void;
      
      function getAuthKey() : String;
      
      function refreshAuthKey() : void;
      
      function clearUserId(param1:String) : void;
      
      function getPlatformType() : SocialType;
      
      function getSupportedSocialNetworkTypes() : Array;
      
      function get isCreatedId() : Boolean;
      
      function get isUserIdEquals() : Boolean;
      
      function get localStoreType() : String;
      
      function set localStoreType(param1:String) : void;
   }
}

