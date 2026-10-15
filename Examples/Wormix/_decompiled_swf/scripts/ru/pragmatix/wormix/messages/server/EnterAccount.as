package ru.pragmatix.wormix.messages.server
{
   import §_-P2i§.§_-b7§;
   import §_-Ta§.VipInfoView;
   import §_-f1Q§.*;
   import §_-gG§.§_-uZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.X509Certificate;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import com.greensock.*;
   import com.greensock.core.*;
   import feathers.controls.Label;
   import feathers.controls.Panel;
   import feathers.layout.VerticalLayout;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.utils.Dictionary;
   import ru.pragmatix.flox.gui.controls.§_-yf§;
   import ru.pragmatix.flox.social.model.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.request.PostClanNewsRequest;
   import ru.pragmatix.wormix.messages.clans.response.PostClanNewsResponse;
   import ru.pragmatix.wormix.messages.structures.LoginAwardStructure;
   import ru.pragmatix.wormix.messages.structures.ReconnectToSimpleBattleResultStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class EnterAccount
   {
      
      public var userProfileStructure:UserProfileStructure;
      
      public var userProfileStructures:Array;
      
      public var loginAwards:Array;
      
      public var onlineFriends:int;
      
      public var friends:int;
      
      public var sessionKey:String;
      
      public var availableSearchKeys:int;
      
      public var reagents:Vector.<int>;
      
      public var curSoloMissionId:int;
      
      public var curCooperativeMissionId:int;
      
      public var renameAct:int;
      
      public var serverTime:int;
      
      public var invites:Vector.<ClanInviteStructure>;
      
      public var backpackConfig1:Array;
      
      public var backpackConfig2:Array;
      
      public var backpackConfig3:Array;
      
      public var activeBackpackConfig:int;
      
      public var hotkeys:Array;
      
      public var loginSequence:int;
      
      public var races:int;
      
      public var selectRaceTime:int;
      
      public var depositStructures:Array;
      
      public var skins:Array;
      
      public var lastPaymentTime:int;
      
      public var restrictions:Array;
      
      public var cookies:Dictionary;
      
      public var vipSubscriptionId:int;
      
      public var reconnectResult:ReconnectToSimpleBattleResultStructure;
      
      public var socialLinks:Array;
      
      public var ads:Array;
      
      public var isSecure:Boolean;
      
      public function EnterAccount()
      {
         super();
      }
      
      public function toString() : String
      {
         return "EnterAccount{userProfileStructure=" + String(this.userProfileStructure) + "}";
      }
      
      public function getLoginAwardByKind(param1:§_-qU§) : LoginAwardStructure
      {
         var _loc2_:LoginAwardStructure = null;
         for each(_loc2_ in this.loginAwards)
         {
            if(_loc2_.awardType == param1.getId())
            {
               return _loc2_;
            }
         }
         return null;
      }
   }
}

