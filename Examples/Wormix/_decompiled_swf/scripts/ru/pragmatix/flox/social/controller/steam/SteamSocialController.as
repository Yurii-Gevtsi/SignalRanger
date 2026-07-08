package ru.pragmatix.flox.social.controller.steam
{
   import §_-2U§.*;
   import §_-31N§.§_-42G§;
   import §_-52L§.*;
   import §_-62§.§_-o6§;
   import §_-63M§.§_-F3f§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-Ly§.§_-81L§;
   import §_-N8§.*;
   import §_-T1t§.§_-u19§;
   import §_-TF§.§_-q1j§;
   import §_-U1i§.§_-J1§;
   import §_-Y1O§.*;
   import §_-Z1K§.*;
   import §_-j22§.§_-5g§;
   import §_-j2R§.§_-73G§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-w2i§.§_-Zd§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import §_-zI§.§_-X1q§;
   import com.amanitadesign.steam.FRESteamWorks;
   import com.amanitadesign.steam.FriendConstants;
   import com.greensock.loading.*;
   import config.§_-vR§;
   import feathers.controls.List;
   import flash.display.BitmapData;
   import flash.display.MovieClip;
   import flash.display.Shape;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.events.NativeWindowDisplayStateEvent;
   import flash.events.StatusEvent;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.media.SoundTransform;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.controller.mobile.BaseMobileSocialController;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.flox.social.profilePage.IProfilePageShower;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.flox.social.response.email.EmailSendResponse;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-B1m§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.GetProfiles;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-m1x§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class SteamSocialController extends BaseMobileSocialController implements IProfilePageShower
   {
      
      private var steam:FRESteamWorks;
      
      private var friendsAvatarDic:Dictionary;
      
      public function SteamSocialController(param1:SocialAppContext)
      {
         super(param1);
         this.steam = SteamWorks.getInstance();
         appId = this.steam.getAppID().toString();
         appDownloadLink = flashVars["steamAppLink"] + appId;
         this.friendsAvatarDic = new Dictionary();
      }
      
      override public function init(param1:Function) : void
      {
         param1(new SocialResponse(null,this.getType().name,this.steam.isReady));
         this.fillOwnInfo();
         this.fillFriends();
      }
      
      private function fillOwnInfo() : void
      {
         var _loc1_:SocialUserData = this.convertDataToSocialUserData(this.steam.getUserID());
         setOwnerSocialUserData(_loc1_);
      }
      
      private function fillFriends() : void
      {
         var _loc2_:String = null;
         var _loc1_:int = this.steam.getFriendCount(FriendConstants.FRIENDFLAG_All);
         var _loc3_:int = 0;
         while(_loc3_ < _loc1_)
         {
            _loc2_ = this.steam.getFriendByIndex(_loc3_,FriendConstants.FRIENDFLAG_All);
            loadedProfiles[_loc2_] = this.convertDataToSocialUserData(_loc2_);
            _loc3_++;
         }
         friendsLoaded = true;
      }
      
      override public function getUserId() : String
      {
         return this.steam.getUserID();
      }
      
      override public function getAppFriends(param1:Function) : void
      {
         generateFriendsIds(param1);
      }
      
      override protected function convertDataToSocialUserData(param1:Object) : SocialUserData
      {
         var _loc2_:String = param1 as String;
         var _loc3_:SocialUserData = new SocialUserData();
         _loc3_.setSocialId(_loc2_);
         _loc3_.setFirstName(this.steam.getFriendPersonaName(_loc2_));
         _loc3_.setLastName("");
         var _loc4_:BitmapData = this.friendsAvatarDic[_loc2_];
         if(_loc4_ == null)
         {
            _loc4_ = this.friendsAvatarDic[_loc2_] = this.steam.getMediumFriendAvatar(_loc2_);
         }
         _loc3_.setPhotoBig(_loc4_);
         return _loc3_;
      }
      
      override public function login(param1:Function) : void
      {
         param1(new SocialResponse(null,this.getType().name,true));
      }
      
      override public function logout() : void
      {
      }
      
      override public function getType() : SocialType
      {
         return SocialType.STEAM;
      }
      
      override public function isLoggedIn() : Boolean
      {
         return true;
      }
      
      override public function dispose() : void
      {
         this.steam.dispose();
      }
      
      public function isProfilePageAvailable() : Boolean
      {
         return true;
      }
      
      public function showProfilePage(param1:String) : void
      {
         SteamWorks.getInstance().activateGameOverlayToUser("steamid",param1);
      }
   }
}

