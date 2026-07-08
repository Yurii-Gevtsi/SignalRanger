package ru.pragmatix.wormix.controller
{
   import §_-12W§.§_-J3J§;
   import §_-12W§.§_-X2w§;
   import §_-12W§.§_-l23§;
   import §_-21p§.§_-4w§;
   import §_-2k§.§_-51r§;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-63M§.§_-F3f§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.Cookie;
   import §_-CF§.§_-p2U§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.MessageBox;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-DJ§.§_-j15§;
   import §_-L1H§.§_-b2K§;
   import §_-SP§.§_-M4§;
   import §_-Y1Q§.§_-tc§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-ZP§.§_-H1H§;
   import §_-b1F§.§_-G4§;
   import §_-b1F§.§_-v2Q§;
   import §_-f1Q§.*;
   import §_-fo§.§_-r5§;
   import §_-j1w§.§_-B2i§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-p18§.§_-vd§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-w2i§.§_-Ia§;
   import §_-zI§.§_-92Q§;
   import §_-zI§.§_-F3q§;
   import §_-zI§.§_-X1q§;
   import §_-zI§.§_-sI§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import config.§_-V2w§;
   import dragonBones.starling.StarlingFactory;
   import dragonBones.starling.StarlingTextureAtlasData;
   import dragonBones.textures.TextureAtlasData;
   import feathers.controls.LayoutGroup;
   import feathers.layout.ILayoutData;
   import feathers.layout.ILayoutDisplayObject;
   import flash.display.BitmapData;
   import flash.events.*;
   import flash.geom.Rectangle;
   import flash.system.Capabilities;
   import flash.utils.*;
   import org.as3commons.zip.*;
   import ru.pragmatix.flox.social.controller.ICustomInviter;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.lang.ILangController;
   import ru.pragmatix.flox.social.lang.LangControllerFactory;
   import ru.pragmatix.flox.social.model.IUserProfile;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.ChooseBonusItemResult;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.CatPhysicModel;
   import ru.pragmatix.wormix.physics.model.NotStuckingPhysicModel;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class §_-b1g§ extends starling.events.EventDispatcher
   {
      
      private const supportedLanguages:Array;
      
      private const §_-c2S§:String = "SAVED_LANGUAGE";
      
      private var §_-S2X§:ILangController;
      
      private var §_-X1H§:Boolean = false;
      
      public function §_-b1g§()
      {
         var _loc2_:String = null;
         this.supportedLanguages = ["en","ru","zn"];
         super();
         var _loc1_:String = Cookie.getStringValue(this.§_-c2S§);
         if(_loc1_ == null)
         {
            _loc2_ = Capabilities.language;
            if(this.supportedLanguages.indexOf(_loc2_) == -1)
            {
               _loc2_ = this.supportedLanguages[0];
            }
            Cookie.setValue(this.§_-c2S§,_loc2_);
         }
         this.§_-S2X§ = LangControllerFactory.create(this.§_-I9§());
      }
      
      public function §_-I9§() : §_-p2U§
      {
         var _loc1_:String = Cookie.getStringValue(this.§_-c2S§,Capabilities.language);
         return this.§_-Q15§(_loc1_);
      }
      
      public function setLanguage(param1:§_-p2U§) : void
      {
         Cookie.setValue(this.§_-c2S§,param1.§_-b2§());
         this.§_-X1H§ = true;
         §_-X1j§.§_-41H§.§_-I2B§(this.§_-WL§);
      }
      
      private function §_-WL§() : void
      {
         this.§_-X1H§ = false;
         this.§_-S2X§ = LangControllerFactory.create(this.§_-I9§());
         Application.userProfile.§_-Q2S§();
         if(!Application.§_-SH§.§_-x2k§)
         {
            Application.§_-SH§.userProfile.§_-Q2S§();
         }
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      private function §_-Q15§(param1:String) : §_-p2U§
      {
         switch(param1)
         {
            case "en":
               return §_-p2U§.§_-qr§;
            case "ru":
               return §_-p2U§.§_-w13§;
            case "zn":
               return §_-p2U§.§_-U1W§;
            default:
               return new §_-p2U§(param1);
         }
      }
      
      public function get §_-rE§() : Boolean
      {
         return this.§_-X1H§;
      }
      
      public function getDisplayName(param1:IUserProfile) : String
      {
         return this.§_-S2X§.getDisplayName(param1);
      }
      
      public function getPluralForm(param1:int, param2:String) : String
      {
         return this.§_-S2X§.getPluralForm(param1,§_-s2a§.§_-K3t§(param2).split(","));
      }
      
      public function §_-M1S§(param1:§_-p2U§) : String
      {
         var _loc2_:String = "";
         switch(param1.§_-b2§())
         {
            case "ru":
               _loc2_ = §_-s2a§.RUSSIAN_TITLE;
               break;
            case "en":
               _loc2_ = §_-s2a§.ENGLISH_TITLE;
               break;
            case "zn":
               _loc2_ = §_-s2a§.CHINESE_TITLE;
         }
         return _loc2_ ? §_-s2a§.§_-K3t§(_loc2_) : "";
      }
      
      public function §_-L2e§() : Array
      {
         var _loc2_:String = null;
         var _loc1_:Array = [];
         for each(_loc2_ in this.supportedLanguages)
         {
            _loc1_.push(this.§_-Q15§(_loc2_));
         }
         return _loc1_;
      }
   }
}

