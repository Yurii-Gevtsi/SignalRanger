package ru.pragmatix.flox.social.utils.id
{
   import §_-Vg§.*;
   import §_-bI§.*;
   import feathers.layout.RelativePosition;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   
   public class BasePlatformAccountController extends EventDispatcher implements IPlatformAccountController
   {
      
      protected var flashVars:Object;
      
      protected var userId:String;
      
      protected var secretKey:String;
      
      public function BasePlatformAccountController(param1:Object)
      {
         super();
         this.flashVars = param1;
         this.secretKey = param1["secretKey"];
      }
      
      public function init() : void
      {
         dispatchEvent(new Event(Event.COMPLETE));
      }
      
      public function getUserId() : String
      {
         if(!this.userId)
         {
            if(String(this.flashVars["useFakeHardwareAddress"]).toLowerCase() == "true" && Boolean(this.flashVars["userId"]))
            {
               this.userId = this.flashVars["userId"];
            }
         }
         return this.userId;
      }
      
      public function getUserIdByType(param1:String) : String
      {
         return "";
      }
      
      public function setUserIdToStore(param1:String, param2:String) : void
      {
      }
      
      public function getAuthKey() : String
      {
         throw new Error("method getPlatformType must be overridden in descendants");
      }
      
      public function getPlatformType() : SocialType
      {
         throw new Error("method getPlatformType must be overridden in descendants");
      }
      
      public function getSupportedSocialNetworkTypes() : Array
      {
         throw new Error("method getPlatformType must be overridden in descendants");
      }
      
      public function refreshAuthKey() : void
      {
         dispatchEvent(new GetAuthKeyEvent(true));
      }
      
      public function clearUserId(param1:String) : void
      {
      }
      
      public function get isCreatedId() : Boolean
      {
         return false;
      }
      
      public function get isUserIdEquals() : Boolean
      {
         return false;
      }
      
      public function get localStoreType() : String
      {
         return "";
      }
      
      public function set localStoreType(param1:String) : void
      {
      }
   }
}

