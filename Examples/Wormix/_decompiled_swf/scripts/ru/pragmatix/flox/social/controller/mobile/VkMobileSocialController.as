package ru.pragmatix.flox.social.controller.mobile
{
   import §_-12W§.*;
   import §_-21A§.*;
   import §_-23P§.*;
   import §_-31N§.§_-42G§;
   import §_-31W§.AchievLevelUpMessage;
   import §_-31W§.§_-8§;
   import §_-31W§.§_-c2J§;
   import §_-31Y§.§_-D3B§;
   import §_-43V§.Random;
   import §_-62§.*;
   import §_-82a§.*;
   import §_-82l§.§_-A2Y§;
   import §_-91A§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-CF§.§_-x2t§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-T2G§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-G16§.§_-33h§;
   import §_-H2g§.§_-di§;
   import §_-K3r§.§_-E3O§;
   import §_-K3r§.§_-U2b§;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-L2F§.§_-D2p§;
   import §_-N8§.§_-o2w§;
   import §_-TF§.§_-q1j§;
   import §_-Ty§.§_-K3a§;
   import §_-U1Y§.*;
   import §_-X14§.*;
   import §_-fo§.*;
   import §_-hO§.§_-BY§;
   import §_-hO§.§_-y2Z§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-cZ§;
   import §_-l1K§.§_-d2j§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.*;
   import §_-r1C§.§_-h2B§;
   import §_-w2W§.AchieveRoomScreenDesktop;
   import §_-w2W§.ArmoryScreenDesktop;
   import §_-w2W§.CraftRoomScreenDesktop;
   import §_-w2W§.DesktopRaceRoomScreen;
   import §_-w2W§.MainMenuScreenDesktop;
   import §_-w2W§.TeamRoomScreenDesktop;
   import §_-w2W§.WardrobeRoomScreen;
   import §_-w2W§.§_-21w§;
   import §_-w2W§.§_-72M§;
   import §_-w2W§.§_-HV§;
   import §_-w2W§.§_-I2R§;
   import §_-w2W§.§_-I3c§;
   import §_-w2W§.§_-h1Z§;
   import §_-wD§.*;
   import §_-z1g§.§_-C32§;
   import §_-z1g§.§_-M2v§;
   import com.distriqt.extension.cloudstorage.CloudStorage;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.math.BigInteger;
   import com.hurlant.util.§_-F2L§;
   import config.§_-vR§;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ToggleButton;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.Loader;
   import flash.display.LoaderInfo;
   import flash.display.Shape;
   import flash.display.SimpleButton;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.events.SecurityErrorEvent;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.net.Socket;
   import flash.net.URLRequest;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.ICustomInviter;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.Gender;
   import ru.pragmatix.flox.social.model.IUserProfile;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.flox.social.post.INotifier;
   import ru.pragmatix.flox.social.post.IWallPoster;
   import ru.pragmatix.flox.social.response.LoadFriendsResponse;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controller.analytics.Analytics;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.BossTypesTab;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.§_-11V§;
   import ru.pragmatix.wormix.gui.screenOverlay.§_-O5§;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.LowerRankInClanResponse;
   import ru.pragmatix.wormix.messages.client.ChooseBonusItem;
   import ru.pragmatix.wormix.messages.client.Link;
   import ru.pragmatix.wormix.messages.client.PumpReactionRate;
   import ru.pragmatix.wormix.messages.server.Pong;
   import ru.pragmatix.wormix.messages.structures.SocialLinkInfoStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Stage;
   import starling.events.Event;
   import starling.events.TouchEvent;
   import starling.textures.Texture;
   import starling.textures.TextureSmoothing;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class VkMobileSocialController extends BaseMobileSocialController implements IWallPoster, INotifier, ICustomInviter
   {
      
      private static var isInitied:Boolean = false;
      
      public function VkMobileSocialController(param1:SocialAppContext)
      {
         super(param1);
         appId = flashVars["vkAppId"];
         appDownloadLink = flashVars["vkAppLink"];
      }
      
      override public function init(param1:Function) : void
      {
         var _loc2_:SocialResponse = new SocialResponse("init",this.getType().name);
         _loc2_.success = false;
         param1(_loc2_);
      }
      
      private function onGetUserProfileAfterInit(param1:SocialResponse, param2:Function, param3:SocialResponse) : void
      {
         if(!param1.success)
         {
            this.logout();
         }
         param2(param3);
      }
      
      override public function login(param1:Function) : void
      {
         param1();
      }
      
      private function vkLoginEventHandler(param1:String, param2:Function) : void
      {
         var _loc4_:SocialResponse = null;
         var _loc3_:Boolean = param1 == null;
         if(_loc3_)
         {
            this.getUserProfile(param2);
         }
         else
         {
            _loc4_ = new SocialResponse();
            _loc4_.apiMethod = "login";
            _loc4_.success = false;
            _loc4_.opType = this.getType().name;
            param2(_loc4_);
         }
      }
      
      override public function logout() : void
      {
         super.logout();
      }
      
      override public function isLoggedIn() : Boolean
      {
         return false;
      }
      
      private function getUserProfile(param1:Function) : void
      {
         var _loc2_:SocialResponse = new SocialResponse();
         _loc2_.apiMethod = "users.get";
         _loc2_.opType = this.getType().name;
         param1(_loc2_);
      }
      
      private function onGetUserProfileSuccess(param1:Object, param2:*, param3:SocialResponse, param4:Function) : void
      {
         var _loc5_:SocialUserData = this.convertDataToSocialUserData(param1[0]);
         setOwnerSocialUserData(_loc5_);
         param3.success = true;
         param4(param3);
      }
      
      private function onGetUserProfileFail(param1:String, param2:SocialResponse, param3:Function) : void
      {
         param2.success = false;
         param3(param2);
      }
      
      override public function getAppFriends(param1:Function) : void
      {
         if(!friendsLoaded)
         {
            param1();
         }
         else
         {
            generateFriendsIds(param1);
         }
      }
      
      private function onGetAppFriendsSuccess(param1:Object, param2:*, param3:Function) : void
      {
         var _loc4_:String = (param1 as Array).join(",");
         param3();
      }
      
      private function onGetAppFriendsProfilesSuccess(param1:Object, param2:*, param3:Function) : void
      {
         var _loc4_:Object = null;
         for each(_loc4_ in param1)
         {
            if(_loc4_.id != getUserId())
            {
               loadedProfiles[_loc4_.id] = this.convertDataToSocialUserData(_loc4_);
            }
         }
         friendsLoaded = true;
         generateFriendsIds(param3);
      }
      
      public function getFriendsToInvite(param1:Function) : void
      {
         var _loc2_:Object = {
            "extended":1,
            "count":1000,
            "offset":0,
            "type":"invite",
            "fields":"photo_100,photo_200,sex"
         };
         var _loc3_:Object = {
            "callback":param1,
            "data":[]
         };
         this.collectData("apps.getFriendsList",_loc2_,_loc3_,this.onFriendsToInviteCollected);
      }
      
      private function onFriendsToInviteCollected(param1:Object) : void
      {
         var _loc3_:Object = null;
         var _loc2_:LoadFriendsResponse = new LoadFriendsResponse();
         for each(_loc3_ in param1.data)
         {
            _loc2_.uids.push(this.convertDataToSocialUserData(_loc3_));
         }
         param1.callback(_loc2_);
      }
      
      private function collectData(param1:String, param2:Object, param3:Object, param4:Function) : void
      {
         param4();
      }
      
      private function onCollectDataSuccess(param1:Object, param2:*, param3:String, param4:Object, param5:Object, param6:Function) : void
      {
         var _loc7_:Object = null;
         for each(_loc7_ in param1.items)
         {
            param5.data.push(_loc7_);
         }
         if(param1.items.length == param4.count)
         {
            param4.offset += param4.count;
            this.collectData(param3,param4,param5,param6);
         }
         else
         {
            param6(param5);
         }
      }
      
      public function inviteFriend(param1:String, param2:String, param3:Object, param4:Function) : void
      {
         var _loc5_:Object = {
            "user_id":param1,
            "text":param2,
            "type":"invite"
         };
         if(param4 != null)
         {
            param4();
         }
      }
      
      private function onFriendInviteSuccess(param1:Object, param2:*, param3:Function) : void
      {
         param3(new SocialResponse("apps.sendRequest",this.getType().name,true));
      }
      
      private function onFriendInviteFail(param1:String, param2:Function) : void
      {
         param2(new SocialResponse("apps.sendRequest",this.getType().name,false));
      }
      
      override public function wallPost(param1:String, param2:String, param3:String = null, param4:Function = null) : void
      {
         var _loc5_:Loader = null;
         if(param3)
         {
            _loc5_ = new Loader();
            §_-83E§.§_-43T§(_loc5_.contentLoaderInfo,[flash.events.Event.COMPLETE,IOErrorEvent.IO_ERROR,SecurityErrorEvent.SECURITY_ERROR],§_-bs§.§_-d1Z§(this.loaderEventHandler,this,[param1,param2,param4],true));
            _loc5_.load(new URLRequest(param3));
         }
         else
         {
            this.showShareDialog(param1,param2,null,param4);
         }
      }
      
      private function loaderEventHandler(param1:flash.events.Event, param2:String, param3:String, param4:Function = null) : void
      {
         if(param1.type == flash.events.Event.COMPLETE)
         {
            this.showShareDialog(param2,param3,((param1.target as LoaderInfo).content as Bitmap).bitmapData,param4);
         }
         else
         {
            executeCallbackWithResult(param4,false);
         }
      }
      
      override public function wallPostLoadedImg(param1:String, param2:String, param3:flash.display.DisplayObject, param4:Function = null) : void
      {
         if(param3 is Bitmap)
         {
            this.showShareDialog(param1,param2,(param3 as Bitmap).bitmapData,param4);
         }
      }
      
      private function showShareDialog(param1:String, param2:String, param3:BitmapData, param4:Function = null) : void
      {
         if(!param2)
         {
            param2 = "Wormix";
         }
         if(param4 != null)
         {
            param4(false);
         }
      }
      
      override protected function convertDataToSocialUserData(param1:Object) : SocialUserData
      {
         var _loc2_:SocialUserData = new SocialUserData();
         _loc2_.setSocialId(param1.id);
         _loc2_.setFirstName(param1.first_name);
         _loc2_.setLastName(param1.last_name);
         _loc2_.setPhoto(param1.photo_100);
         _loc2_.setPhotoMedium(param1.photo_100);
         _loc2_.setPhotoBig(param1.photo_200);
         _loc2_.setGender(param1.sex == 1 ? Gender.MALE : Gender.FEMALE);
         return _loc2_;
      }
      
      override public function notify(param1:IUserProfile, param2:String, param3:String, param4:Function = null, param5:String = null) : void
      {
         var _loc6_:String = Boolean(param1) && Boolean(param1.getSocialData()) ? param1.getSocialData().getSocialId() : "";
         var _loc7_:Object = {
            "user_id":_loc6_,
            "text":param2,
            "type":"request"
         };
         if(param4 != null)
         {
            param4(false);
         }
      }
      
      override public function getType() : SocialType
      {
         return SocialType.VK;
      }
      
      override public function getAuthKey() : String
      {
         return null;
      }
   }
}

