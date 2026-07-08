package ru.pragmatix.wormix.social
{
   import §_-03T§.§_-K3B§;
   import §_-12W§.§_-221§;
   import §_-51W§.§_-SN§;
   import §_-5P§.§_-H1u§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-gr§;
   import §_-72j§.§_-H1j§;
   import §_-72j§.§_-Q1F§;
   import §_-72j§.§_-Z6§;
   import §_-72j§.§_-a1S§;
   import §_-72j§.§_-i2G§;
   import §_-72j§.§_-p1G§;
   import §_-82l§.§_-1I§;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-Bv§.§_-G3P§;
   import §_-C2M§.DesktopTutorialPanel;
   import §_-C2M§.MobileTutorialPanel;
   import §_-C2M§.§_-o2f§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-Es§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-F39§.*;
   import §_-GQ§.§_-31v§;
   import §_-H3§.*;
   import §_-J3b§.*;
   import §_-L1H§.§_-C3e§;
   import §_-Nq§.§_-G1J§;
   import §_-Nq§.§_-Ow§;
   import §_-Nq§.§_-s1O§;
   import §_-Nq§.§_-uh§;
   import §_-RE§.§_-r2q§;
   import §_-SP§.§_-M4§;
   import §_-T1t§.§_-u19§;
   import §_-Ta§.*;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1O§.*;
   import §_-YF§.§_-Zi§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-ZB§.§_-32D§;
   import §_-ZB§.§_-81R§;
   import §_-ZB§.§_-A10§;
   import §_-ZB§.§_-GG§;
   import §_-ZB§.§_-R2Y§;
   import §_-ZB§.§_-V2N§;
   import §_-d2p§.*;
   import §_-e2r§.*;
   import §_-fo§.*;
   import §_-hO§.AttackDodgedEvent;
   import §_-hO§.§_-729§;
   import §_-hO§.§_-v24§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-Oi§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-C3A§;
   import §_-k2s§.§_-fG§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-f2L§;
   import §_-lh§.§_-z2A§;
   import §_-n1w§.*;
   import §_-o14§.§_-Dd§;
   import §_-p14§.§_-A3Y§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.*;
   import §_-s1Q§.Artifacts;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u2J§.§_-T1K§;
   import §_-w2i§.*;
   import §_-z1g§.*;
   import §_-z20§.*;
   import com.distriqt.extension.application.Application;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.hurlant.crypto.tls.§_-F16§;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import com.hurlant.math.BigInteger;
   import config.§_-V2w§;
   import config.§_-xD§;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.data.ArrayCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import flash.errors.IllegalOperationError;
   import flash.events.TimerEvent;
   import flash.filesystem.File;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.system.ApplicationDomain;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.Timer;
   import org.gestouch.core.GestureState;
   import org.gestouch.core.Touch;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.IAuthorizeSocialController;
   import ru.pragmatix.flox.social.controller.ICustomInviter;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.controller.SocialApiSettings;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.controller.mobile.BaseMobileSocialController;
   import ru.pragmatix.flox.social.controller.mobile.VkMobileSocialController;
   import ru.pragmatix.flox.social.lang.ILangController;
   import ru.pragmatix.flox.social.lang.LangControllerFactory;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.flox.social.photo.IPhotoUploader;
   import ru.pragmatix.flox.social.photo.PhotoUploaderFactory;
   import ru.pragmatix.flox.social.response.LoadFriendsResponse;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-42p§;
   import ru.pragmatix.wormix.controller.§_-I3A§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controller.§_-b1g§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-fT§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.Connect;
   import ru.pragmatix.wormix.messages.client.Disconnect;
   import ru.pragmatix.wormix.messages.server.ConnectResult;
   import ru.pragmatix.wormix.messages.structures.*;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.utils.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-N1V§
   {
      
      public static var §_-S2X§:ILangController;
      
      public static var settings:§_-A10§;
      
      public static var §_-I39§:§_-32D§;
      
      public static var §_-42k§:§_-R2Y§;
      
      public static var postController:§_-81R§;
      
      public static var §_-aR§:§_-GG§;
      
      public static var wipeController:§_-V2N§;
      
      public static var photoUploader:IPhotoUploader;
      
      public static var §_-W1m§:§_-42p§;
      
      private static var socialControllers:§_-11f§;
      
      private static var §_-k2a§:Function;
      
      private static var _dispatcher:EventDispatcher;
      
      private static var disposed:Boolean;
      
      private static var §_-E7§:Boolean;
      
      private static var §_-Gp§:Boolean;
      
      private static var §_-1u§:Function;
      
      private static const §_-z2N§:String = "Wormix";
      
      public function §_-N1V§()
      {
         super();
      }
      
      public static function dispose() : void
      {
         if(!disposed)
         {
            disposed = true;
            §_-E7§ = false;
            §_-Gp§ = false;
            if(settings)
            {
               settings.dispose();
               settings = null;
            }
            if(§_-42k§)
            {
               §_-42k§.dispose();
               §_-42k§ = null;
            }
            if(postController)
            {
               postController.dispose();
               postController = null;
            }
            if(wipeController)
            {
               if(wipeController is §_-K3B§)
               {
                  (wipeController as §_-K3B§).destroy();
               }
               wipeController = null;
            }
            if(§_-W1m§)
            {
               §_-W1m§.destroy();
               §_-W1m§ = null;
            }
            §_-I39§ = null;
            §_-S2X§ = null;
            §_-aR§ = null;
            §_-k2a§ = null;
         }
      }
      
      public static function create() : void
      {
         disposed = false;
      }
      
      public static function init(param1:SocialAppContext) : void
      {
         socialControllers = new §_-L23§(param1,§_-X1j§.§_-It§.getSupportedSocialNetworkTypes(),SocialType.NONE);
         socialControllers.init(§_-bs§.§_-d1Z§(onInit,null));
      }
      
      private static function onInit() : void
      {
         var _loc1_:IPlatformAccountController = §_-X1j§.§_-It§;
         var _loc2_:ISocialController = socialControllers.getActive();
         settings = §_-p1G§.create(_loc1_,_loc2_);
         §_-S2X§ = LangControllerFactory.create(§_-X1j§.§_-82T§.§_-I9§());
         §_-I39§ = §_-a1S§.create(_loc1_.getPlatformType());
         §_-aR§ = §_-H1j§.create(_loc2_);
         photoUploader = PhotoUploaderFactory.getPhotoUploader("","",Starling.current.nativeStage,"",_loc2_);
         §_-W1m§ = new §_-42p§();
         dispatcher.dispatchEventWith(Event.COMPLETE);
         dispatcher.dispatchEventWith(§_-f2L§.§_-Rx§);
      }
      
      public static function §_-t1N§(param1:ISocialController) : void
      {
         postController = §_-Z6§.create(§_-X1j§.context,param1);
         wipeController = §_-i2G§.create(param1);
         §_-xD§.init(param1);
      }
      
      public static function §_-51K§() : void
      {
         §_-42k§ = §_-Q1F§.create(§_-X1j§.§_-It§.getPlatformType());
         §_-42k§.§_-j1E§();
      }
      
      public static function §_-72Z§(param1:UserProfile) : void
      {
         var _loc2_:BaseMobileSocialController = null;
         var _loc3_:String = null;
         var _loc5_:SocialType = null;
         var _loc4_:Array = socialControllers.§_-Nb§();
         for each(_loc5_ in _loc4_)
         {
            if(!_loc5_.equals(SocialType.NONE))
            {
               _loc2_ = socialControllers.§_-61b§(_loc5_);
               _loc3_ = param1.getSocialIds()[_loc5_];
               if(!_loc2_.isLoggedIn() && Boolean(_loc3_))
               {
                  disconnectFromSocialNetwork(param1,_loc5_);
               }
            }
         }
      }
      
      public static function addListener(param1:String, param2:Function) : void
      {
         dispatcher.addEventListener(param1,param2);
      }
      
      public static function removeListener(param1:String, param2:Function) : void
      {
         dispatcher.removeEventListener(param1,param2);
      }
      
      public static function §_-E3l§() : Boolean
      {
         return §_-aR§ != null;
      }
      
      public static function §_-e2O§() : Boolean
      {
         return wipeController != null;
      }
      
      public static function §_-03z§() : String
      {
         return §_-z2N§;
      }
      
      private static function §_-ZZ§() : Array
      {
         return socialControllers.§_-Nb§();
      }
      
      public static function §_-J3X§() : void
      {
         §_-xJ§(§_-p2h§);
      }
      
      private static function §_-p2h§(param1:Boolean) : void
      {
         var _loc2_:ICustomInviter = socialController as ICustomInviter;
         if(_loc2_)
         {
            §_-J2l§.§_-u2b§.show(_loc2_.getFriendsToInvite,§_-825§);
         }
      }
      
      private static function §_-825§(param1:LoadFriendsResponse) : void
      {
         var response:LoadFriendsResponse = param1;
         §_-J2l§.§_-u2b§.hide();
         if(response.uids.length > 0)
         {
            §_-73§.§_-D3u§(§_-q2v§.SELECT_FRIEND_TO_INVITE,function(param1:§_-73y§):void
            {
               param1.data = response.uids;
            });
         }
         else
         {
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.NO_FRIENDS_FOR_INVITE));
         }
      }
      
      public static function §_-xJ§(param1:Function, param2:Boolean = false) : void
      {
         var _loc4_:Function = null;
         var _loc3_:Boolean = socialController.isLoggedIn();
         if(!_loc3_ || param2)
         {
            if(!_loc3_ && !socialController.getType().equals(SocialType.NONE))
            {
               _loc4_ = §_-bs§.§_-d1Z§(§_-aY§,null,[param1]);
               socialControllers.change(SocialType.NONE,_loc4_);
            }
            else
            {
               §_-aY§(param1);
            }
         }
         else
         {
            param1(false);
         }
      }
      
      private static function §_-aY§(param1:Function) : void
      {
         var socialTypes:Array = null;
         var callback:Function = param1;
         socialTypes = socialControllers.§_-Nb§();
         socialTypes.sort(function(param1:SocialType, param2:SocialType):int
         {
            if(param1.servercode > param2.servercode)
            {
               return -1;
            }
            if(param1.servercode < param2.servercode)
            {
               return 1;
            }
            return 0;
         });
         if(socialTypes.length)
         {
            §_-73§.§_-D3u§(§_-q2v§.§_-e5§,function(param1:§_-73y§):void
            {
               param1.data = socialTypes;
               §_-52k§.single(param1,§_-M4§.CHANGE,onSocialNetworkSelected,null,[callback],true);
            });
         }
         else
         {
            §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.SOCIAL_NETWORKS_NOT_AVAILABLE));
            callback(false);
         }
      }
      
      private static function onSocialNetworkSelected(param1:Event, param2:Function) : void
      {
         var _loc3_:SocialType = param1.data as SocialType;
         var _loc4_:SocialType = socialController.getType();
         var _loc5_:Function = §_-bs§.§_-d1Z§(§_-c2W§,null,[param2,_loc4_],true);
         §_-J2l§.§_-u2b§.show(socialControllers.change,_loc3_,_loc5_);
      }
      
      private static function §_-c2W§(param1:SocialResponse, param2:Function, param3:SocialType) : void
      {
         var response:SocialResponse = param1;
         var callback:Function = param2;
         var prevSocType:SocialType = param3;
         SystemUtil.executeWhenApplicationIsActive(function():void
         {
            var _loc1_:BaseMobileSocialController = null;
            §_-J2l§.§_-u2b§.hide();
            if(response.success)
            {
               _loc1_ = socialController;
               postController.dispose();
               postController = §_-Z6§.create(§_-X1j§.context,_loc1_);
               wipeController = §_-i2G§.create(_loc1_);
               photoUploader = PhotoUploaderFactory.getPhotoUploader("","",Starling.current.nativeStage,"",_loc1_);
               disconnectFromSocialNetwork(Application.userProfile,prevSocType);
            }
            dispatcher.dispatchEventWith(§_-f2L§.§_-Rx§);
            §_-I3A§.clear();
            §_-N1V§.§_-qw§();
            callback(response.success);
         });
      }
      
      public static function §_-M1F§(param1:Function = null) : void
      {
         var _loc2_:IAuthorizeSocialController = socialController;
         if(Boolean(_loc2_) && _loc2_.isLoggedIn())
         {
            if(Application.userProfile)
            {
               Application.userProfile.§_-c7§(_loc2_.getType());
            }
            _loc2_.logout();
         }
         if(param1 != null)
         {
            param1();
         }
      }
      
      public static function §_-qw§() : void
      {
         if(!Application.userProfile)
         {
            return;
         }
         if(socialController.isLoggedIn())
         {
            socialController.getAppFriends(§_-g2R§);
         }
         else
         {
            _dispatcher.dispatchEventWith(§_-f2L§.USER_FRIENDS_LOADED);
         }
      }
      
      private static function §_-g2R§(param1:LoadFriendsResponse) : void
      {
         var sc:ISocialController = null;
         var response:LoadFriendsResponse = param1;
         if(§_-Gp§ || §_-E7§)
         {
            return;
         }
         §_-Gp§ = true;
         sc = socialController;
         Application.userProfile.getSocialIds()[sc.getType()] = sc.getUserId();
         new §_-Zi§().§_-I3W§([Application.userProfile],function(param1:Array, param2:Boolean):void
         {
            var connect:Connect = null;
            var profiles:Array = param1;
            var result:Boolean = param2;
            §_-Gp§ = false;
            dispatcher.dispatchEventWith(§_-f2L§.USER_SOCIAL_DATA_CHANGED,profiles[0]);
            connect = new Connect();
            connect.authKey = sc is VkMobileSocialController ? sc.getAuthKey() : §_-H1u§.getAuthKey(false);
            connect.socialNetId = sc.getType().servercode;
            connect.socialProfileId = sc.getUserId();
            connect.isSecure = true;
            if(!connect.socialProfileId)
            {
               return;
            }
            connect.ids = response.uids;
            §_-E7§ = true;
            SystemUtil.executeWhenApplicationIsActive(function():void
            {
               if(§_-J2g§.§_-i1N§.isOpened)
               {
                  §_-1u§ = §_-bs§.§_-d1Z§(§_-33V§,§_-J2g§,[response.uids],true);
                  §_-J2g§.§_-o1R§(ConnectResult,§_-1u§);
                  §_-J2g§.§_-h18§(connect);
               }
               else
               {
                  §_-d21§.§_-G2K§(§_-J2g§.§_-i1N§,connect,ConnectResult,§_-33V§,response.uids);
               }
            });
         });
      }
      
      private static function §_-33V§(param1:§_-63i§, param2:Array) : void
      {
         var connectResult:ConnectResult = null;
         var friendProfiles:Array = null;
         var profiles:Array = null;
         var profile:UserProfile = null;
         var i:int = 0;
         var e:§_-63i§ = param1;
         var friendsIds:Array = param2;
         if(!disposed)
         {
            §_-E7§ = false;
            connectResult = ConnectResult(e.message);
            friendProfiles = §_-I3A§.§_-31X§(connectResult.userProfileStructures,friendsIds,int.MAX_VALUE,connectResult.onlineFriends,connectResult.friends);
            profiles = [];
            i = 0;
            while(i < connectResult.userProfileStructures.length)
            {
               profile = friendProfiles[i];
               profile.setId(connectResult.userProfileStructures[i].id);
               profile.§_-620§(connectResult.userProfileStructures[i].socialIds);
               profiles.push(profile);
               i++;
            }
            new §_-Zi§().§_-I3W§(profiles,function(param1:Array, param2:Boolean):void
            {
               var _loc4_:UserProfile = null;
               var _loc3_:Object = {};
               var _loc5_:int = 0;
               while(_loc5_ < param1.length)
               {
                  _loc4_ = param1[_loc5_];
                  _loc3_[_loc4_.getId()] = _loc4_.§_-b1b§();
                  _loc5_++;
               }
               Application.userProfile.build.§_-tn§(_loc3_);
               _dispatcher.dispatchEventWith(§_-f2L§.USER_FRIENDS_LOADED);
            });
         }
      }
      
      private static function get dispatcher() : EventDispatcher
      {
         if(!_dispatcher)
         {
            _dispatcher = new EventDispatcher();
         }
         return _dispatcher;
      }
      
      private static function disconnectFromSocialNetwork(param1:UserProfile, param2:SocialType) : void
      {
         var disconnectMsg:Disconnect = null;
         var userProfile:UserProfile = param1;
         var socialType:SocialType = param2;
         try
         {
            if(socialType.equals(SocialType.NONE))
            {
               return;
            }
            disconnectMsg = new Disconnect();
            disconnectMsg.socialNetId = socialType.servercode;
            §_-J2g§.§_-i1N§.send(disconnectMsg);
            userProfile.getSocialIds()[socialType] = "";
         }
         catch(e:Error)
         {
         }
      }
      
      public static function get socialController() : BaseMobileSocialController
      {
         return socialControllers.getActive();
      }
      
      public static function get §_-e28§() : String
      {
         if(Boolean(socialControllers.getActive()) && !socialControllers.getActive().getType().equals(SocialType.NONE))
         {
            return §_-Qd§.§_-31Q§(socialControllers.getActive().getType());
         }
         return "";
      }
      
      public static function §_-G1w§() : void
      {
         if(!socialControllers.getActive())
         {
            return;
         }
         var _loc1_:String = "";
         switch(socialControllers.getActive().getType())
         {
            case SocialType.VK:
               _loc1_ = "https://vk.ru/club186867404";
         }
         if(_loc1_)
         {
            navigateToURL(new URLRequest(_loc1_),"_blank");
         }
      }
      
      public static function §_-S1u§() : void
      {
         if(§_-1u§ != null)
         {
            §_-J2g§.§_-fy§(ConnectResult,§_-1u§);
            §_-1u§ = null;
         }
         §_-M1F§();
         dispatcher.dispatchEventWith(§_-f2L§.§_-Rx§);
      }
      
      public static function get §_-hQ§() : Boolean
      {
         return §_-E7§;
      }
   }
}

