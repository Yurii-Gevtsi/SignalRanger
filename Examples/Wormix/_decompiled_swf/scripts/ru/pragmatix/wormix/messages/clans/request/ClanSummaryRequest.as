package ru.pragmatix.wormix.messages.clans.request
{
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-91B§.§_-z2l§;
   import §_-fo§.§_-r5§;
   import com.hurlant.crypto.tls.§_-21v§;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   
   public class ClanSummaryRequest
   {
      
      public var clanId:int;
      
      public var scope:int;
      
      public function ClanSummaryRequest(param1:int, param2:int)
      {
         super();
         this.clanId = param1;
         this.scope = param2;
      }
   }
}

