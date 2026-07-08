package ru.pragmatix.flox.social.model
{
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-62§.*;
   import §_-63M§.§_-F3f§;
   import §_-73d§.§_-x2C§;
   import §_-931§.§_-i2r§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-bs§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-b2K§;
   import §_-L2F§.§_-D2p§;
   import §_-P2i§.§_-b7§;
   import §_-R2I§.PvpTab;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-S1N§.*;
   import §_-Tm§.§_-RF§;
   import §_-Ty§.§_-K3a§;
   import §_-Vg§.*;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-Z1K§.§_-A2n§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-gG§.§_-Nl§;
   import §_-hO§.§_-v24§;
   import §_-j1w§.§_-B2i§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-p24§.MissionTab;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import §_-x2Q§.§_-dU§;
   import §_-zI§.§_-F3q§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import com.hurlant.util.der.§_-p1B§;
   import com.worlize.websocket.§_-x2j§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.LayoutGroup;
   import feathers.core.ITextRenderer;
   import feathers.data.IListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayoutData;
   import flash.events.MouseEvent;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Timer;
   import org.gestouch.core.GestureState;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.BossTypesTab;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class SocialUserData
   {
      
      private var socialId:String;
      
      private var email:String;
      
      private var firstName:String;
      
      private var lastName:String;
      
      private var nickName:String;
      
      private var photo:Object;
      
      private var photoMedium:Object;
      
      private var photoBig:Object;
      
      private var birthday:Date;
      
      private var gender:Gender = Gender.MALE;
      
      private var pageUrl:String;
      
      private var mobile:Boolean;
      
      public function SocialUserData()
      {
         super();
      }
      
      public function hasMobile() : Boolean
      {
         return this.mobile;
      }
      
      public function setHasMobile(param1:Boolean) : void
      {
         this.mobile = param1;
      }
      
      public function getPageUrl() : String
      {
         return this.pageUrl;
      }
      
      public function setPageUrl(param1:String) : void
      {
         this.pageUrl = param1;
      }
      
      public function getSocialId() : String
      {
         return this.socialId;
      }
      
      public function getEmail() : String
      {
         return this.email;
      }
      
      public function getFirstName() : String
      {
         return this.firstName;
      }
      
      public function getLastName() : String
      {
         return this.lastName;
      }
      
      public function getNickName() : String
      {
         return this.nickName;
      }
      
      public function getPhoto() : Object
      {
         return this.photo;
      }
      
      public function getBirthday() : Date
      {
         return this.birthday;
      }
      
      public function getGender() : Gender
      {
         return this.gender;
      }
      
      public function isMale() : Boolean
      {
         return Gender.MALE.equals(this.gender);
      }
      
      public function isFemale() : Boolean
      {
         return Gender.FEMALE.equals(this.gender);
      }
      
      public function setSocialId(param1:String) : void
      {
         this.socialId = param1;
      }
      
      public function setEmail(param1:String) : void
      {
         this.email = param1;
      }
      
      public function setFirstName(param1:String) : void
      {
         this.firstName = param1;
      }
      
      public function setLastName(param1:String) : void
      {
         this.lastName = param1;
      }
      
      public function setNickName(param1:String) : void
      {
         this.nickName = param1;
      }
      
      public function setPhoto(param1:Object) : void
      {
         this.photo = param1;
      }
      
      public function setBirthday(param1:Date) : void
      {
         this.birthday = param1;
      }
      
      public function setGender(param1:Gender) : void
      {
         this.gender = param1;
      }
      
      public function getPhotoMedium() : Object
      {
         return this.photoMedium;
      }
      
      public function setPhotoMedium(param1:Object) : void
      {
         this.photoMedium = param1;
      }
      
      public function getPhotoBig() : Object
      {
         return this.photoBig;
      }
      
      public function setPhotoBig(param1:Object) : void
      {
         this.photoBig = param1;
      }
      
      public function toString() : String
      {
         return "SocialUserData {" + " id: " + this.socialId + ", " + this.getFirstName() + " " + this.getLastName() + this.getNickName() + ", gender: " + this.getGender() + ", bDate: " + (this.getBirthday() ? this.getBirthday().toLocaleDateString() : null) + ", photo: " + this.getPhoto().toString() + " }";
      }
   }
}

