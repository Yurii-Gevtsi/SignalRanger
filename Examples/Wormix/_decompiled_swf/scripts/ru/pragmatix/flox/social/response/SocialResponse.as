package ru.pragmatix.flox.social.response
{
   import §_-A1K§.§_-TA§;
   import §_-Vg§.ClanSearchView;
   import §_-Y1Q§.§_-aG§;
   import §_-zI§.§_-F3q§;
   import ru.pragmatix.flox.social.model.SocialErrorCode;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   
   public class SocialResponse
   {
      
      public var success:Boolean;
      
      public var errorCode:SocialErrorCode;
      
      public var apiMethod:String;
      
      public var opType:String;
      
      public function SocialResponse(param1:String = null, param2:String = null, param3:Boolean = false)
      {
         super();
         this.apiMethod = param1;
         this.success = param3;
         this.opType = param2;
      }
      
      public function toString() : String
      {
         return "SocialResponse{success=" + String(this.success) + ",opType=" + String(this.opType) + ",errorCode=" + String(this.errorCode) + "}";
      }
   }
}

