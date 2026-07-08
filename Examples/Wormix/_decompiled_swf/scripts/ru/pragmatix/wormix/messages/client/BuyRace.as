package ru.pragmatix.wormix.messages.client
{
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-fH§;
   import §_-YF§.§_-q2v§;
   import §_-j22§.§_-5g§;
   import §_-r1C§.§_-U2p§;
   import §_-r1C§.§_-h2B§;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.BattleReconnect;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   
   public class BuyRace
   {
      
      public var raceId:int;
      
      public var moneyType:§_-E19§;
      
      public function BuyRace(param1:int = 0, param2:§_-E19§ = null)
      {
         super();
         this.raceId = param1;
         this.moneyType = param2;
      }
   }
}

