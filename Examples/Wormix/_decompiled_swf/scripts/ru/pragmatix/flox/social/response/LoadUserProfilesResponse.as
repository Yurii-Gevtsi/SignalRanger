package ru.pragmatix.flox.social.response
{
   import §_-P2i§.*;
   import §_-zI§.*;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import starling.events.Event;
   
   public class LoadUserProfilesResponse extends SocialResponse
   {
      
      private var log:§_-11S§ = new §_-11S§(LoadUserProfilesResponse);
      
      private var socialUserProfiles:Object = {};
      
      public function LoadUserProfilesResponse()
      {
         super(null,"LOAD.USER.PROFILES");
      }
      
      public function addSocialUserData(param1:SocialUserData) : void
      {
         this.socialUserProfiles[param1.getSocialId()] = param1;
      }
      
      public function getSocialUserData(param1:String) : SocialUserData
      {
         var _loc2_:SocialUserData = this.socialUserProfiles[param1];
         if(!_loc2_)
         {
            this.log.§_-E2X§("SocialUserData not found by socialId " + param1);
         }
         return _loc2_;
      }
   }
}

