package ru.pragmatix.flox.social.utils.id
{
   import §_-CF§.§_-x2t§;
   import com.mesmotronic.ane.AndroidID;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.storage.StorageType;
   
   public class AndroidAccountController extends MobilePlatformAccountController
   {
      
      public function AndroidAccountController(param1:Object)
      {
         super(param1);
         _localStoreType = StorageType.LOCAL_SHARED;
      }
      
      override public function getUserId() : String
      {
         var _loc1_:Boolean = false;
         if(super.getUserId())
         {
            return userId;
         }
         var _loc2_:String = this.getHardwareId();
         var _loc3_:String = this.getUserIdByType(_localStoreType);
         trace("hardwareId (Android) < " + _loc2_);
         trace("localUserId (Android) < " + _loc3_);
         if(_loc3_)
         {
            userId = _loc3_;
         }
         else if(_loc2_)
         {
            _loc1_ = true;
            userId = _loc2_;
         }
         if(!userId)
         {
            _loc1_ = true;
            _isCreatedId = true;
            userId = this.getUserIdByType(UserIdType.GENERATED);
         }
         if(_loc1_)
         {
            setUserIdToStore(userId,_localStoreType);
         }
         trace("userId (Android) >> " + userId);
         return userId;
      }
      
      override public function getUserIdByType(param1:String) : String
      {
         switch(param1)
         {
            case UserIdType.HARDWARE:
               return this.getHardwareId();
            default:
               return super.getUserIdByType(param1);
         }
      }
      
      private function getHardwareId() : String
      {
         return AndroidID.isSupported ? AndroidID.ANDROID_ID : "";
      }
      
      override public function get isUserIdEquals() : Boolean
      {
         return this.getUserIdByType(_localStoreType) == this.getHardwareId();
      }
      
      override public function getPlatformType() : SocialType
      {
         return SocialType.MOBILE_ANDROID;
      }
   }
}

