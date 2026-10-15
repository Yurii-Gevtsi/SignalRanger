package ru.pragmatix.flox.social.utils.id
{
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.storage.StorageType;
   
   public class IosAccountController extends MobilePlatformAccountController
   {
      
      public function IosAccountController(param1:Object)
      {
         super(param1);
         _localStoreType = StorageType.LOCAL_ENCRYPTED;
      }
      
      override public function getUserId() : String
      {
         if(super.getUserId())
         {
            return userId;
         }
         var _loc1_:String = getUserIdByType(_localStoreType);
         trace("localUserId (iOS) < " + _loc1_);
         if(_loc1_)
         {
            userId = _loc1_;
         }
         if(!userId)
         {
            _isCreatedId = true;
            userId = getUserIdByType(UserIdType.GENERATED);
            setUserIdToStore(userId,_localStoreType);
         }
         trace("userId (iOS) >> " + userId);
         return userId;
      }
      
      override public function getPlatformType() : SocialType
      {
         return SocialType.MOBILE_IOS;
      }
   }
}

