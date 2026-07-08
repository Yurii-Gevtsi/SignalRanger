package ru.pragmatix.flox.social.controller.mobile
{
   import §_-21p§.§_-4w§;
   import §_-2k§.§_-51r§;
   import §_-2k§.§_-m2i§;
   import §_-31N§.§_-42G§;
   import §_-31W§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-83E§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.EmojiShower;
   import §_-F39§.§_-GP§;
   import §_-H3§.*;
   import §_-L1H§.§_-C3e§;
   import §_-P2i§.§_-b7§;
   import §_-TF§.§_-F1B§;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-g2r§.§_-03A§;
   import §_-jF§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-m1g§.RatingPanel;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s1Q§.§_-W2D§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-H3D§;
   import §_-w2W§.*;
   import §_-x2Q§.§_-dU§;
   import §_-z20§.§_-pl§;
   import com.mesmotronic.ane.AndroidFullScreen;
   import com.worlize.websocket.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.ImageLoader;
   import feathers.controls.Panel;
   import feathers.controls.ScrollBarDisplayMode;
   import feathers.controls.ScrollInteractionMode;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.display.DisplayObject;
   import flash.display.Shape;
   import flash.errors.IllegalOperationError;
   import flash.events.IEventDispatcher;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.text.TextField;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.AbstractAuthorizeSocialController;
   import ru.pragmatix.flox.social.model.IUserProfile;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.flox.social.response.LoadFriendsResponse;
   import ru.pragmatix.flox.social.response.LoadUserProfilesResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.SearchTheHouse;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.BunkerBusterMissile;
   import ru.pragmatix.wormix.weapons.ammo.GuardingBot;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class BaseMobileSocialController extends AbstractAuthorizeSocialController
   {
      
      private const MY_PROFILE:String = "MY_PROFILE";
      
      protected var appId:String;
      
      protected var appDownloadLink:String;
      
      protected var clubLink:String;
      
      protected var loadedProfiles:Dictionary = new Dictionary();
      
      protected var friendsLoaded:Boolean = false;
      
      public function BaseMobileSocialController(param1:SocialAppContext)
      {
         super(param1);
      }
      
      override public function getAppLink(param1:String = null) : String
      {
         return this.appDownloadLink;
      }
      
      override public function getClubLink() : String
      {
         return this.clubLink;
      }
      
      override public function getUserId() : String
      {
         return this.loadedProfiles[this.MY_PROFILE].getSocialId();
      }
      
      override public function logout() : void
      {
         this.loadedProfiles = new Dictionary();
         this.friendsLoaded = false;
      }
      
      override public function dispose() : void
      {
      }
      
      override public function getUserProfiles(param1:Array, param2:Function) : void
      {
         var _loc4_:String = null;
         var _loc3_:LoadUserProfilesResponse = new LoadUserProfilesResponse();
         for each(_loc4_ in param1)
         {
            if(this.loadedProfiles[_loc4_])
            {
               _loc3_.addSocialUserData(this.loadedProfiles[_loc4_]);
            }
         }
         param2(_loc3_);
      }
      
      protected function convertDataToSocialUserData(param1:Object) : SocialUserData
      {
         throw new IllegalOperationError();
      }
      
      protected function setOwnerSocialUserData(param1:SocialUserData) : void
      {
         this.loadedProfiles[this.MY_PROFILE] = param1;
         this.loadedProfiles[param1.getSocialId()] = param1;
      }
      
      public function getOwnerSocialUserData() : SocialUserData
      {
         return this.loadedProfiles[this.MY_PROFILE];
      }
      
      protected function generateFriendsIds(param1:Function, param2:Boolean = true) : void
      {
         var _loc5_:SocialUserData = null;
         var _loc3_:SocialUserData = this.loadedProfiles[this.MY_PROFILE];
         var _loc4_:LoadFriendsResponse = new LoadFriendsResponse();
         _loc4_.success = param2;
         for each(_loc5_ in this.loadedProfiles)
         {
            if(_loc5_.getSocialId() != _loc3_.getSocialId())
            {
               _loc4_.uids.push(_loc5_.getSocialId());
            }
         }
         param1(_loc4_);
      }
      
      public function wallPost(param1:String, param2:String, param3:String = null, param4:Function = null) : void
      {
      }
      
      public function wallPostLoadedImg(param1:String, param2:String, param3:flash.display.DisplayObject, param4:Function = null) : void
      {
      }
      
      public function notify(param1:IUserProfile, param2:String, param3:String, param4:Function = null, param5:String = null) : void
      {
      }
      
      public function returnFriendNotification(param1:IUserProfile, param2:String, param3:Function, param4:String = null) : void
      {
      }
      
      protected function executeCallbackWithResult(param1:Function, param2:Boolean) : void
      {
         if(param1 != null)
         {
            param1(param2);
         }
      }
   }
}

