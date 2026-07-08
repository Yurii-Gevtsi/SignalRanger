package ru.pragmatix.flox.social.controller
{
   import §_-CF§.§_-p2U§;
   import flash.utils.ByteArray;
   
   public interface ISocialController
   {
      
      function init(param1:Function) : void;
      
      function getLanguageFromList(param1:Array) : §_-p2U§;
      
      function getMaxProfilesInRequest() : uint;
      
      function getUserId() : String;
      
      function getAppLink(param1:String = null) : String;
      
      function getDomain() : String;
      
      function getClubLink() : String;
      
      function getAuthKey() : String;
      
      function getAlbums(param1:Function) : void;
      
      function createAlbum(param1:String, param2:String, param3:Function) : void;
      
      function getAlbumUploadServer(param1:String, param2:Function) : void;
      
      function savePhotoInAlbum(param1:Array, param2:String, param3:Function) : void;
      
      function getAppFriends(param1:Function) : void;
      
      function getAllFriends(param1:Function) : void;
      
      function getUserProfiles(param1:Array, param2:Function) : void;
      
      function isAlbumsAvailable() : Boolean;
      
      function isSettingAvailable(param1:SocialApiSettings) : Boolean;
      
      function changeSettings(param1:SocialApiSettings, param2:Function, param3:Function = null) : void;
      
      function changeSettingsIfNeeded(param1:SocialApiSettings, param2:Function, param3:Function = null) : void;
      
      function getType() : SocialType;
      
      function showInviteFriendsDialog(param1:String = null) : void;
      
      function isVideoAvailable() : Boolean;
      
      function isYoutubeAvailable() : Boolean;
      
      function getVideoUploadServer(param1:String, param2:String, param3:Function) : void;
      
      function uploadVideo(param1:String, param2:ByteArray, param3:Function) : void;
      
      function getTypeIdSocial() : Class;
   }
}

