package ru.pragmatix.flox.social.utils.id
{
   import com.amanitadesign.steam.FRESteamWorks;
   import com.amanitadesign.steam.SteamConstants;
   import com.amanitadesign.steam.SteamEvent;
   import com.greensock.events.TweenEvent;
   import com.hurlant.util.§_-J1N§;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.controller.steam.SteamWorks;
   
   public class SteamAccountController extends BasePlatformAccountController
   {
      
      private var steamworks:FRESteamWorks;
      
      private var steamworksAvailable:Boolean = false;
      
      private var encryptedAppTicket:ByteArray;
      
      public function SteamAccountController(param1:Object)
      {
         super(param1);
      }
      
      override public function init() : void
      {
         this.steamworks = SteamWorks.getInstance();
         this.steamworksAvailable = this.steamworks.isReady;
         if(this.steamworksAvailable)
         {
            this.steamworks.addEventListener(SteamEvent.STEAM_RESPONSE,this.steamEventHandler);
            dispatchEvent(new Event(Event.COMPLETE));
         }
         else
         {
            dispatchEvent(new AccountErrorEvent(AccountErrorCode.NOT_AVAILABLE));
         }
      }
      
      override public function refreshAuthKey() : void
      {
         var _loc1_:Boolean = this.steamworks.requestEncryptedAppTicket(new ByteArray());
         if(!_loc1_)
         {
            dispatchEvent(new GetAuthKeyEvent(false));
         }
      }
      
      private function steamEventHandler(param1:SteamEvent) : void
      {
         var _loc2_:Boolean = false;
         if(param1.req_type == SteamConstants.RESPONSE_OnEncryptedAppTicketResponse)
         {
            this.encryptedAppTicket = new ByteArray();
            _loc2_ = this.steamworks.getEncryptedAppTicket(this.encryptedAppTicket);
            if(!_loc2_)
            {
               dispatchEvent(new GetAuthKeyEvent(false));
            }
            else
            {
               dispatchEvent(new GetAuthKeyEvent(true));
            }
         }
      }
      
      override public function getAuthKey() : String
      {
         return §_-J1N§.§_-z2W§(this.encryptedAppTicket);
      }
      
      override public function getUserId() : String
      {
         if(!super.getUserId())
         {
            if(this.steamworksAvailable)
            {
               userId = this.steamworks.getUserID();
            }
         }
         return userId;
      }
      
      override public function getPlatformType() : SocialType
      {
         return SocialType.STEAM;
      }
      
      override public function getSupportedSocialNetworkTypes() : Array
      {
         return [SocialType.STEAM,SocialType.NONE];
      }
   }
}

