package ru.pragmatix.flox.social.controller
{
   import §_-02q§.§_-4s§;
   import §_-31W§.*;
   import §_-31u§.§_-l1n§;
   import §_-52L§.§_-u2x§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.*;
   import §_-63U§.§_-412§;
   import §_-73d§.§_-g1J§;
   import §_-73d§.§_-v1o§;
   import §_-82a§.§_-c2B§;
   import §_-82a§.§_-zv§;
   import §_-82l§.§_-1I§;
   import §_-A1K§.§_-TA§;
   import §_-A1m§.MultipartURLLoader;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.*;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-p2U§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.*;
   import §_-H16§.*;
   import §_-L1H§.§_-C3e§;
   import §_-N8§.*;
   import §_-OA§.§_-k2K§;
   import §_-R2S§.Farmer;
   import §_-R2S§.Hunter;
   import §_-R2S§.Miner;
   import §_-R2S§.§_-P1U§;
   import §_-R2S§.§_-z2x§;
   import §_-RE§.§_-52p§;
   import §_-RE§.§_-w0§;
   import §_-T1t§.§_-u19§;
   import §_-Ta§.*;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-Y1Q§.§_-61Z§;
   import §_-Y1Q§.§_-F1g§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-b1F§.§_-v2Q§;
   import §_-g1P§.§_-z1n§;
   import §_-j1w§.§_-lD§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.§_-Dd§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-s1Q§.Artifacts;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-z20§.*;
   import com.hurlant.math.BigInteger;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.List;
   import feathers.controls.ToggleButton;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.core.PopUpManager;
   import flash.errors.IllegalOperationError;
   import flash.events.ErrorEvent;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.events.IOErrorEvent;
   import flash.events.SecurityErrorEvent;
   import flash.net.URLLoader;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getQualifiedClassName;
   import org.swiftsuspenders.Injector;
   import org.swiftsuspenders.§_-71M§;
   import org.swiftsuspenders.§_-T1p§;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.logger.ConsolePublisher;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.flox.social.model.ResponseFormat;
   import ru.pragmatix.flox.social.model.SocialErrorCode;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-B1m§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.main.server.InviteFriendToClan;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.response.treasury.DonateClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.GetFriendListPageResult;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.*;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.boss.emperor.EmperorTerracottaWarrior;
   import ru.pragmatix.wormix.weapons.ammo.boss.scientist.ScientistGenerator;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   
   public class AbstractSocialController implements ISocialController
   {
      
      protected var flashVars:Object;
      
      protected var log:§_-11S§ = new §_-11S§(AbstractSocialController);
      
      private var appLink:String;
      
      private var clubLink:String;
      
      protected var context:§_-o2k§;
      
      public function AbstractSocialController(param1:§_-o2k§)
      {
         super();
         this.flashVars = param1.§_-E1e§();
         this.context = param1;
         this.appLink = param1.§_-JF§().appLink || "";
         this.clubLink = param1.§_-JF§().clubLink || "";
      }
      
      public function getDomain() : String
      {
         throw new IllegalOperationError();
      }
      
      public function getClubLink() : String
      {
         return this.getDomain() + "/" + this.clubLink;
      }
      
      public function getAppLink(param1:String = null) : String
      {
         return this.getDomain() + "/" + this.appLink;
      }
      
      protected function logError(param1:SocialResponse) : void
      {
         this.log.error("Social API request \'" + param1.apiMethod + "\' failed wirh error code: " + param1.errorCode.code + " msg: " + param1.errorCode.msg);
      }
      
      protected function multipartLoadNoParse(param1:MultipartURLLoader, param2:ResponseFormat, param3:String, param4:Function, param5:Function, param6:SocialResponse, param7:Boolean = true) : void
      {
         var _loc8_:Function = §_-bs§.§_-d1Z§(this.parse,this,[param2,param4,param5,param6,param7],true);
         §_-83E§.§_-43T§(param1,[flash.events.Event.COMPLETE,IOErrorEvent.IO_ERROR,SecurityErrorEvent.SECURITY_ERROR],_loc8_);
         param1.load(param3);
      }
      
      protected function parse(param1:flash.events.Event, param2:ResponseFormat, param3:Function, param4:Function, param5:SocialResponse, param6:Boolean = true, param7:Boolean = true) : void
      {
         var _loc9_:Object = null;
         var _loc8_:Object = null;
         if(param1.target is URLLoader)
         {
            _loc9_ = param1.target.data;
         }
         else if(param1.target is MultipartURLLoader)
         {
            _loc9_ = param1.target.loader.data;
         }
         if(param1.type == flash.events.Event.COMPLETE)
         {
            switch(param2)
            {
               case ResponseFormat.XML:
                  _loc8_ = new XML(_loc9_);
                  if(param6)
                  {
                     param5.errorCode = this.parseXmlError(_loc8_ as XML);
                  }
                  break;
               case ResponseFormat.JSON:
                  _loc8_ = new §_-k2K§(_loc9_ as String).getValue();
                  if(param6)
                  {
                     param5.errorCode = this.parseJsonError(_loc8_);
                  }
            }
         }
         else
         {
            param5.errorCode = SocialErrorCode.createError(param1 as ErrorEvent);
         }
         if(Boolean(param5.errorCode) && param7)
         {
            param5.success = false;
            this.logError(param5);
            if(param4 != null)
            {
               param4(param5);
            }
         }
         else
         {
            param5.success = true;
            param3(_loc8_,param5,param4);
         }
      }
      
      protected function parseXmlError(param1:XML) : SocialErrorCode
      {
         throw new IllegalOperationError();
      }
      
      protected function parseJsonError(param1:Object) : SocialErrorCode
      {
         throw new IllegalOperationError();
      }
      
      public function getUserId() : String
      {
         throw new IllegalOperationError();
      }
      
      public function getAuthKey() : String
      {
         throw new IllegalOperationError();
      }
      
      public function getAppFriends(param1:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function getUserProfiles(param1:Array, param2:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function getAlbums(param1:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function createAlbum(param1:String, param2:String, param3:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function getAlbumUploadServer(param1:String, param2:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function savePhotoInAlbum(param1:Array, param2:String, param3:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function isAlbumsAvailable() : Boolean
      {
         return false;
      }
      
      public function getType() : SocialType
      {
         throw new IllegalOperationError();
      }
      
      public function getMaxProfilesInRequest() : uint
      {
         throw new IllegalOperationError();
      }
      
      public function getAllFriends(param1:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function changeSettingsIfNeeded(param1:SocialApiSettings, param2:Function, param3:Function = null) : void
      {
         if(!this.isSettingAvailable(param1))
         {
            this.changeSettings(param1,param2,param3);
         }
         else
         {
            param2();
         }
      }
      
      public function isSettingAvailable(param1:SocialApiSettings) : Boolean
      {
         return false;
      }
      
      public function changeSettings(param1:SocialApiSettings, param2:Function, param3:Function = null) : void
      {
         throw new IllegalOperationError();
      }
      
      public function showInviteFriendsDialog(param1:String = null) : void
      {
         throw new IllegalOperationError();
      }
      
      public function isVideoAvailable() : Boolean
      {
         return false;
      }
      
      public function isYoutubeAvailable() : Boolean
      {
         return false;
      }
      
      public function getVideoUploadServer(param1:String, param2:String, param3:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function uploadVideo(param1:String, param2:ByteArray, param3:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function getLanguageFromList(param1:Array) : §_-p2U§
      {
         throw new IllegalOperationError();
      }
      
      public function init(param1:Function) : void
      {
         param1(new SocialResponse(null,"INIT",true));
      }
      
      public function getTypeIdSocial() : Class
      {
         return int;
      }
   }
}

