package ru.pragmatix.wormix.social.utils
{
   import §_-B1y§.§_-F1P§;
   import §_-DJ§.§_-fH§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   
   public class SocialUtils
   {
      
      public function SocialUtils()
      {
         super();
      }
      
      public static function getSocialErrorDescriptionEn(param1:SocialResponse) : String
      {
         var _loc2_:String = param1.apiMethod ? param1.apiMethod : param1.opType;
         return §_-F1P§.format("API request " + _loc2_ + " failed. error code: " + param1.errorCode.code + " msg: " + param1.errorCode.msg);
      }
      
      public static function getSocialErrorDescription(param1:SocialResponse) : String
      {
         var _loc2_:String = param1.apiMethod ? param1.apiMethod : param1.opType;
         return param1.errorCode ? §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.ERROR_SOCIAL_OP),{"error":_loc2_ + " code: " + param1.errorCode.code + " msg: " + param1.errorCode.msg}) : §_-s2a§.§_-K3t§(§_-s2a§.ERROR_SERVER);
      }
      
      public static function showErrorMsg(param1:SocialResponse) : void
      {
         §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ERROR),getSocialErrorDescription(param1),§_-fH§.§_-1B§);
      }
      
      public static function getSocialTypeName(param1:int) : String
      {
         var _loc2_:SocialType = SocialType.valueOfServerCode(param1);
         return _loc2_.name;
      }
   }
}

