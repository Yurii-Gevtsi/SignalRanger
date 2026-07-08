package ru.pragmatix.wormix.messages.server
{
   import §_-22z§.§_-f1b§;
   import §_-5P§.*;
   import §_-5Y§.§_-h2c§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-81l§;
   import §_-aM§.*;
   import §_-j1w§.§_-L1x§;
   import §_-qH§.§_-S25§;
   import §_-s20§.§_-J2g§;
   import §_-z1g§.§_-F4§;
   import config.§_-vR§;
   import flash.utils.Dictionary;
   import flash.utils.getTimer;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.weapons.ammo.phase.HealBolt;
   
   use namespace gestouch_internal;
   
   public class RepeatPvpBattleAwardResult
   {
      
      public var status:int;
      
      public var adEventId:String;
      
      public var awards:Array;
      
      public var advPurchases:Array;
      
      public var sessionKey:String;
      
      public var battleId:int;
      
      public function RepeatPvpBattleAwardResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "RepeatPvpBattleAwardResult{" + "status=" + this.status + ",adEventId=" + this.adEventId + ",battleId=" + this.battleId + ",sessionKey=" + this.sessionKey + ",awards=" + this.awards + ",advPurchases=" + this.advPurchases + "}";
      }
   }
}

