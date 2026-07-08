package ru.pragmatix.flox.social.controller.mobile
{
   import §_-03T§.§_-2p§;
   import §_-12a§.§_-6p§;
   import §_-43V§.Random;
   import §_-62§.§_-O2H§;
   import §_-91A§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A1m§.MultipartURLLoader;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C3f§.§_-4I§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-F39§.§_-r2l§;
   import §_-H16§.*;
   import §_-J31§.§_-R1h§;
   import §_-J3L§.§_-K2i§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-P1B§.§_-b21§;
   import §_-P2i§.§_-G3J§;
   import §_-RE§.*;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-U1Y§.*;
   import §_-Y1O§.§_-42h§;
   import §_-Y1Q§.§_-61Z§;
   import §_-YF§.§_-q2v§;
   import §_-Yk§.*;
   import §_-Z1K§.§_-q24§;
   import §_-a19§.*;
   import §_-a29§.§_-42z§;
   import §_-b1F§.§_-G4§;
   import §_-f1G§.§_-kf§;
   import §_-g2r§.§_-03A§;
   import §_-gG§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-f2L§;
   import §_-r1C§.§_-U2p§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-lp§;
   import §_-z1g§.§_-y1S§;
   import §_-zI§.§_-X1q§;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-a2P§;
   import com.hurlant.util.der.§_-p1B§;
   import feathers.controls.Button;
   import feathers.controls.ScrollPolicy;
   import feathers.core.FeathersControl;
   import feathers.core.IPopUpManager;
   import feathers.data.VectorCollection;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.StageOrientation;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.events.StageOrientationEvent;
   import flash.geom.Point;
   import flash.media.SoundTransform;
   import flash.net.Socket;
   import flash.net.URLLoaderDataFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.setTimeout;
   import org.gestouch.core.GestureState;
   import org.gestouch.core.Touch;
   import org.gestouch.core.gestouch_internal;
   import org.swiftsuspenders.§_-71M§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.ICustomInviter;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.Gender;
   import ru.pragmatix.flox.social.model.IUserProfile;
   import ru.pragmatix.flox.social.model.SocialErrorCode;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.flox.social.post.INotifier;
   import ru.pragmatix.flox.social.post.IWallPoster;
   import ru.pragmatix.flox.social.response.LoadFriendsResponse;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.ChooseBonusItemResult;
   import ru.pragmatix.wormix.messages.server.EndBattleResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   use namespace gestouch_internal;
   
   public class FbMobileSocialController extends BaseMobileSocialController implements IWallPoster, INotifier, ICustomInviter
   {
      
      private var facebookAppAccessToken:String;
      
      private var facebookAppLinkURL:String;
      
      public function FbMobileSocialController(param1:SocialAppContext)
      {
         super(param1);
         appDownloadLink = flashVars["appDownloadLink"];
         clubLink = flashVars["clubLink"];
         appId = flashVars["facebookAppId"];
         this.facebookAppAccessToken = flashVars["facebookAppAccessToken"];
         this.facebookAppLinkURL = flashVars["FacebookApplicationLinkURL"];
      }
      
      override public function init(param1:Function) : void
      {
         var initResult:Boolean = false;
         var callback:Function = param1;
         initResult = false;
         setTimeout(function():void
         {
            var _loc1_:SocialResponse = new SocialResponse("init",getType().name,initResult);
            if(initResult == true && isLoggedIn())
            {
               getUserProfile(§_-bs§.§_-d1Z§(onGetUserProfileAfterInit,this,[callback,_loc1_],true));
            }
            else
            {
               callback(_loc1_);
            }
         },500);
      }
      
      private function onGetUserProfileAfterInit(param1:SocialResponse, param2:Function, param3:SocialResponse) : void
      {
         if(!param1.success)
         {
            this.logout();
         }
         param2(param3);
      }
      
      override public function getType() : SocialType
      {
         return SocialType.FACEBOOK;
      }
      
      override public function login(param1:Function) : void
      {
         param1();
      }
      
      private function fbLoginEventHandler(param1:*, param2:Function) : void
      {
         var _loc3_:SocialResponse = new SocialResponse();
         _loc3_.apiMethod = "login";
         _loc3_.success = false;
         _loc3_.opType = this.getType().name;
         _loc3_.errorCode = new SocialErrorCode(param1.errorCode,param1.errorMessage);
         param2(_loc3_);
      }
      
      private function getUserProfile(param1:Function) : void
      {
         var _loc2_:FbMobileSocialController = this;
         param1(new SocialResponse("login",this.getType().name,false));
      }
      
      override public function logout() : void
      {
         super.logout();
      }
      
      override public function isLoggedIn() : Boolean
      {
         return false;
      }
      
      override public function getAppFriends(param1:Function) : void
      {
         var _loc2_:Object = null;
         var _loc3_:Object = null;
         if(!friendsLoaded)
         {
            _loc2_ = {
               "limit":1000,
               "offset":0,
               "installed":true,
               "fields":"first_name,last_name,picture,gender"
            };
            _loc3_ = {
               "callback":param1,
               "data":[]
            };
            this.collectDataFromAppFriendsRequest(_loc2_,_loc3_,this.onFinishAppFriendCollect);
         }
         else
         {
            generateFriendsIds(param1);
         }
      }
      
      private function onFinishAppFriendCollect(param1:Object) : void
      {
         var obj:Object = null;
         var data:Object = param1;
         try
         {
            for each(obj in data.data)
            {
               loadedProfiles[obj.id] = this.convertCollectedData(obj);
            }
         }
         catch(e:Error)
         {
            trace("Error in converting data to SocialUserData: " + e.message);
         }
         friendsLoaded = true;
         generateFriendsIds(data.callback);
      }
      
      override public function wallPost(param1:String, param2:String, param3:String = null, param4:Function = null) : void
      {
         var _loc8_:Object = null;
         var _loc5_:FbMobileSocialController = this;
         var _loc6_:String = Boolean(param2) && Boolean(param2 != "") && param2 != " " ? param2 : appDownloadLink;
         var _loc7_:String = String(flashVars["platform"]).toLowerCase();
         if(_loc7_ == SocialType.MOBILE_ANDROID.name)
         {
            _loc8_ = {};
            _loc8_.caption = param1;
            _loc8_.link = _loc6_;
            _loc8_.picture = param3;
            executeCallbackWithResult(param4,false);
         }
         else if(_loc7_ == SocialType.MOBILE_IOS.name)
         {
            executeCallbackWithResult(param4,false);
         }
      }
      
      override public function wallPostLoadedImg(param1:String, param2:String, param3:flash.display.DisplayObject, param4:Function = null) : void
      {
         var _loc5_:MultipartURLLoader = null;
         if(param3 is Bitmap)
         {
            _loc5_ = new MultipartURLLoader();
            _loc5_.dataFormat = URLLoaderDataFormat.TEXT;
            _loc5_.§_-62R§("filename","picture.jpg");
            _loc5_.§_-62R§("published",false);
            _loc5_.§_-pj§(§_-42z§.encode((param3 as Bitmap).bitmapData),"picture.jpg","Filedata","application/octet-stream");
            §_-83E§.§_-C38§(_loc5_.loader,IOErrorEvent.IO_ERROR,§_-bs§.§_-d1Z§(executeCallbackWithResult,this,[param4,false],false),flash.events.Event.COMPLETE,§_-bs§.§_-d1Z§(this.onImageLoadComplete,this,[param1,param2,param4],true));
            _loc5_.load("https://graph.facebook.com/" + getUserId() + "/photos");
         }
         else
         {
            executeCallbackWithResult(param4,false);
         }
      }
      
      private function onImageLoadComplete(param1:flash.events.Event, param2:String, param3:String, param4:Function = null) : void
      {
         var _loc5_:Object = JSON.parse(param1.target.data);
         var _loc6_:Object = {"fields":"images"};
         executeCallbackWithResult(param4,false);
      }
      
      private function postTheBiggestImage(param1:Array, param2:String, param3:String, param4:Function = null) : void
      {
         var _loc7_:Object = null;
         var _loc5_:String = "";
         var _loc6_:int = 0;
         for each(_loc7_ in param1)
         {
            if(_loc7_.width > _loc6_)
            {
               _loc6_ = int(_loc7_.width);
               _loc5_ = _loc7_.source;
            }
         }
         if(_loc5_ != "")
         {
            this.wallPost(param2,param3,_loc5_,param4);
         }
      }
      
      public function getFriendsToInvite(param1:Function) : void
      {
         var _loc2_:Object = {
            "limit":1000,
            "offset":0,
            "fields":"first_name,last_name,picture"
         };
         var _loc3_:Object = {
            "callback":param1,
            "data":[]
         };
         this.collectDataFromPaginationRequest("me/invitable_friends",_loc2_,_loc3_,this.onFinishFriendsToInviteCollect);
      }
      
      private function collectDataFromAppFriendsRequest(param1:Object, param2:Object, param3:Function) : void
      {
         param3(param2);
      }
      
      private function collectDataFromPaginationRequest(param1:String, param2:Object, param3:Object, param4:Function) : void
      {
         param4(param3);
      }
      
      private function onFinishFriendsToInviteCollect(param1:Object) : void
      {
         var obj:Object = null;
         var data:Object = param1;
         var response:LoadFriendsResponse = new LoadFriendsResponse();
         response.apiMethod = "friends to invite";
         response.success = false;
         response.opType = this.getType().name;
         try
         {
            for each(obj in data.data)
            {
               response.uids.push(this.convertCollectedData(obj,true));
            }
         }
         catch(e:Error)
         {
            §§push(e);
            var _temp_1:* = e;
            e = §§pop();
            trace("Error in converting data to SocialUserData: " + e.message);
         }
         data.callback(response);
      }
      
      public function inviteFriend(param1:String, param2:String, param3:Object, param4:Function) : void
      {
         param4();
      }
      
      override public function notify(param1:IUserProfile, param2:String, param3:String, param4:Function = null, param5:String = null) : void
      {
         var _loc6_:String = Boolean(param1) && Boolean(param1.getSocialData()) ? param1.getSocialData().getSocialId() : "";
         if(param4 != null)
         {
            param4(false);
         }
      }
      
      private function convertCollectedData(param1:Object, param2:Boolean = false) : SocialUserData
      {
         var _loc4_:String = null;
         var _loc3_:SocialUserData = this.convertDataToSocialUserData(param1);
         if(param2)
         {
            _loc4_ = param1.picture.data.url;
            _loc3_.setPhoto(_loc4_);
            _loc3_.setPhotoMedium(_loc4_);
            _loc3_.setPhotoBig(_loc4_);
         }
         else
         {
            _loc4_ = "https://graph.facebook.com/" + param1.id + "/picture";
            _loc3_.setPhoto(_loc4_ + "?width=100&height=100");
            _loc3_.setPhotoMedium(_loc3_.getPhoto());
            _loc3_.setPhotoBig(_loc4_ + "?width=200&height=200");
         }
         return _loc3_;
      }
      
      override protected function convertDataToSocialUserData(param1:Object) : SocialUserData
      {
         var _loc2_:SocialUserData = new SocialUserData();
         _loc2_.setFirstName(param1.first_name);
         _loc2_.setLastName(param1.last_name);
         _loc2_.setSocialId(param1.id);
         _loc2_.setGender(param1.gender == "male" ? Gender.MALE : Gender.FEMALE);
         return _loc2_;
      }
      
      private function convertGVFacebookFriendToSocialUserData(param1:*) : SocialUserData
      {
         var _loc2_:SocialUserData = new SocialUserData();
         _loc2_.setFirstName(param1.name);
         _loc2_.setGender(param1.gender == "male" ? Gender.MALE : Gender.FEMALE);
         var _loc3_:String = "https://graph.facebook.com/" + param1.id + "/picture";
         _loc2_.setPhoto(_loc3_ + "?width=100&height=100");
         _loc2_.setPhotoMedium(_loc2_.getPhoto());
         _loc2_.setPhotoBig(_loc3_ + "?width=200&height=200");
         _loc2_.setSocialId(param1.id);
         _loc2_.setPageUrl(param1.link);
         return _loc2_;
      }
   }
}

