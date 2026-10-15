package ru.pragmatix.flox.social.utils.id
{
   import §_-B1y§.Cookie;
   import §_-Vu§.MD5;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.storage.EncryptedLocalStorageCookie;
   import ru.pragmatix.flox.social.utils.storage.StorageType;
   
   public class MobilePlatformAccountController extends BasePlatformAccountController
   {
      
      public static const MOBILE_SOCIAL_USER_ID:String = "MobileSocialUserId";
      
      protected const USER_ID_COOKEY_KEY:String = "MobileSocialUserId";
      
      protected var _isCreatedId:Boolean;
      
      protected var _localStoreType:String;
      
      public function MobilePlatformAccountController(param1:Object)
      {
         super(param1);
      }
      
      override public function getAuthKey() : String
      {
         return MD5.§_-bf§(userId + secretKey);
      }
      
      override public function getSupportedSocialNetworkTypes() : Array
      {
         return [SocialType.NONE];
      }
      
      override public function getUserIdByType(param1:String) : String
      {
         switch(param1)
         {
            case UserIdType.GENERATED:
               return GUID.create();
            case StorageType.LOCAL_SHARED:
               return Cookie.getStringValue(MOBILE_SOCIAL_USER_ID);
            case StorageType.LOCAL_ENCRYPTED:
               return EncryptedLocalStorageCookie.getStringValue(MOBILE_SOCIAL_USER_ID);
            default:
               return super.getUserIdByType(param1);
         }
      }
      
      override public function setUserIdToStore(param1:String, param2:String) : void
      {
         if(!param2)
         {
            throw new Error("arg storeType in method setUserIdToStore must be defined!");
         }
         switch(param2)
         {
            case StorageType.LOCAL_SHARED:
               Cookie.setValue(MOBILE_SOCIAL_USER_ID,param1);
               break;
            case StorageType.LOCAL_ENCRYPTED:
               EncryptedLocalStorageCookie.setStringValue(MOBILE_SOCIAL_USER_ID,param1);
         }
      }
      
      override public function clearUserId(param1:String) : void
      {
         if(!param1)
         {
            throw new Error("arg storeType in method clearUserId must be defined!");
         }
         this.setUserIdToStore("",param1);
      }
      
      override public function get isCreatedId() : Boolean
      {
         return this._isCreatedId;
      }
      
      override public function get isUserIdEquals() : Boolean
      {
         return false;
      }
      
      override public function get localStoreType() : String
      {
         return this._localStoreType;
      }
      
      override public function set localStoreType(param1:String) : void
      {
         if(!param1)
         {
            throw new Error("arg value in set method localStoreType must be defined!");
         }
         switch(param1)
         {
            case StorageType.LOCAL_SHARED:
            case StorageType.LOCAL_ENCRYPTED:
               this._localStoreType = param1;
         }
         if(!this._localStoreType)
         {
            throw new Error("arg value in set method localStoreType is wrong, must be StorageType.LOCAL_*!");
         }
      }
   }
}

