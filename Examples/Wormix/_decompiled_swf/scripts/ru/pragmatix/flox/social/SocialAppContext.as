package ru.pragmatix.flox.social
{
   import §_-21p§.§_-4w§;
   import §_-43V§.Random;
   import §_-51W§.§_-SN§;
   import §_-63U§.§_-21K§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2F§.§_-4B§;
   import §_-B1y§.§_-o2k§;
   import §_-C2t§.§_-wL§;
   import §_-P1B§.§_-b21§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-XA§;
   import §_-Y1O§.*;
   import §_-Z1K§.§_-M6§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-b1F§.§_-v2Q§;
   import §_-gG§.§_-M2S§;
   import §_-j1w§.*;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-Ia§;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-a2P§;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.ArenaResult;
   import ru.pragmatix.wormix.messages.server.ShowSystemMessage;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionDailyProgressStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.GameObjectUtils;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.§_-N1C§;
   import ru.pragmatix.wormix.weapons.ammo.StaticCharge;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   use namespace bi_internal;
   
   public class SocialAppContext extends §_-o2k§
   {
      
      private var socialControllerType:SocialType;
      
      private var _photoUploadUrl:String;
      
      private var resConfigVersion:String;
      
      private var langFilesVersion:String;
      
      public function SocialAppContext()
      {
         super();
      }
      
      public function getPhotoUploadUrl() : String
      {
         return this._photoUploadUrl;
      }
      
      public function setPhotoUploadUrl(param1:String) : void
      {
         this._photoUploadUrl = param1;
      }
      
      public function getSocialType() : SocialType
      {
         return this.socialControllerType;
      }
      
      public function setSocialType(param1:SocialType) : void
      {
         this.socialControllerType = param1;
      }
      
      public function setResConfigVersion(param1:String) : void
      {
         this.resConfigVersion = param1;
      }
      
      public function getResConfigVersion() : String
      {
         return this.resConfigVersion;
      }
      
      public function setLangFilesVersion(param1:String) : void
      {
         this.langFilesVersion = param1;
      }
      
      public function getLangFilesVersion() : String
      {
         return this.langFilesVersion;
      }
   }
}

