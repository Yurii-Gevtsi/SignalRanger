package ru.pragmatix.wormix.controller
{
   import §_-02q§.§_-11O§;
   import §_-12W§.*;
   import §_-31W§.*;
   import §_-52V§.§_-R9§;
   import §_-5Y§.ChatInputPanel;
   import §_-5Y§.StickerPanel;
   import §_-5Y§.§_-81g§;
   import §_-5Y§.§_-h2c§;
   import §_-5Y§.§_-t2O§;
   import §_-79§.*;
   import §_-82a§.SwitchedRenderTextures;
   import §_-82a§.§_-yb§;
   import §_-931§.§_-02O§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.Cookie;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-92W§;
   import §_-C2t§.§_-L3H§;
   import §_-C3f§.§_-Z1Y§;
   import §_-CF§.§_-x2t§;
   import §_-Cl§.§_-L2z§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-G2k§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-G2H§.*;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-Ta§.*;
   import §_-U1i§.§_-81M§;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-Vx§.§_-V2t§;
   import §_-X14§.§_-J2b§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-61Z§;
   import §_-YF§.§_-f2a§;
   import §_-Z1K§.§_-q24§;
   import §_-ZB§.§_-A10§;
   import §_-aM§.*;
   import §_-bI§.*;
   import §_-dS§.§_-D3a§;
   import §_-fo§.*;
   import §_-g2C§.§_-k1p§;
   import §_-g2C§.§_-p8§;
   import §_-g2r§.§_-03A§;
   import §_-g2r§.§_-x8§;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-hv§.*;
   import §_-j1w§.*;
   import §_-k1u§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-Rs§;
   import §_-m2s§.*;
   import §_-o14§.*;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import §_-sQ§.§_-V1n§;
   import §_-z1g§.§_-F4§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import config.§_-p1i§;
   import feathers.controls.GroupedList;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.Scroller;
   import feathers.controls.ToggleButton;
   import feathers.controls.renderers.DefaultGroupedListHeaderOrFooterRenderer;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.core.IFeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.HorizontalLayoutData;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.layout.VerticalLayoutData;
   import feathers.themes.styles.AlternateTextStyles;
   import feathers.utils.textures.TextureCache;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.net.SharedObject;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import flash.utils.getDefinitionByName;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.ShopItemRenderer;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.UserIsBanned;
   import ru.pragmatix.wormix.messages.structures.*;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-41p§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.FreezingMissile;
   import ru.pragmatix.wormix.weapons.ammo.GuardingBot;
   import starling.display.Button;
   import starling.display.Canvas;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.utils.ScaleMode;
   
   use namespace bi_internal;
   
   public class §_-21j§ extends EventDispatcher
   {
      
      private static const LOAD_SUCCESS:String = "LOAD_SUCCESS";
      
      private static const LOAD_FAIL:String = "LOAD_FAIL";
      
      private var §_-D3d§:Array = [];
      
      private var §_-83y§:uint;
      
      private const §_-e1§:uint = 30;
      
      private const §_-Mh§:uint = 5;
      
      private var §_-M1D§:uint;
      
      private const §_-62u§:uint = 100;
      
      private var §_-QC§:Scene = null;
      
      private var mapLoader:§_-f2a§;
      
      private var _mapLoadData:§_-03A§;
      
      public function §_-21j§()
      {
         super();
      }
      
      public function dispose() : void
      {
         this.clearMapLoader();
      }
      
      public function §_-y1a§(param1:uint, param2:Vector.<UserProfile>, param3:Boolean = false, param4:Boolean = false) : void
      {
         var id:uint = param1;
         var profiles:Vector.<UserProfile> = param2;
         var noMapGround:Boolean = param3;
         var indestructibleGround:Boolean = param4;
         §_-J2l§.§_-e1L§(§_-s2a§.BATTLE_LOAD_MAP_CONFIG,function():void
         {
            _mapLoadData = new §_-03A§();
            _mapLoadData.noMapGround = noMapGround;
            _mapLoadData.indestructibleGround = indestructibleGround;
            _mapLoadData.profiles = profiles;
            _mapLoadData.mapRecord = getMapConfigById(id);
            if(!_mapLoadData.mapRecord)
            {
               dispatchEventWith(§_-x8§.ERROR,false,_mapLoadData);
            }
            else
            {
               loadMapConfig(_mapLoadData);
            }
         });
      }
      
      private function getMapConfigById(param1:uint) : §_-D3a§
      {
         var _loc2_:§_-D3a§ = §_-p1i§.§_-o26§(param1);
         if(_loc2_ == null)
         {
         }
         return _loc2_;
      }
      
      public function §_-43Y§(param1:§_-81M§ = null) : uint
      {
         this.§_-z1a§(param1);
         var _loc2_:uint = this.§_-D3d§.length;
         var _loc3_:uint = (_loc2_ - this.§_-M1D§) * (this.§_-62u§ - this.§_-e1§) + this.§_-M1D§ * this.§_-62u§;
         var _loc4_:uint = 0;
         var _loc5_:uint = MathUtil.random(0,_loc3_);
         var _loc6_:uint = 0;
         var _loc7_:uint = 0;
         while(_loc7_ < _loc2_)
         {
            if(_loc7_ < _loc2_ - this.§_-M1D§ || this.§_-M1D§ <= 0)
            {
               _loc4_ += this.§_-62u§ - this.§_-e1§;
            }
            else
            {
               _loc4_ += this.§_-62u§;
            }
            if(_loc5_ <= _loc4_)
            {
               _loc6_ = _loc7_;
               if(_loc6_ >= _loc2_ - this.§_-M1D§)
               {
                  --this.§_-M1D§;
               }
               break;
            }
            _loc7_++;
         }
         var _loc8_:uint = uint(this.§_-D3d§[_loc6_].getId());
         this.§_-D3d§.splice(_loc6_,1);
         return _loc8_;
      }
      
      public function §_-yw§(param1:§_-D3a§, param2:uint) : Array
      {
         var _loc5_:Array = null;
         var _loc6_:Array = null;
         var _loc3_:§_-V1n§ = new §_-V1n§(param2);
         var _loc4_:Array = [];
         if(Boolean(param1.§_-P1w§) && param1.§_-P1w§.length > 0)
         {
            _loc4_ = §_-F2L§.§_-CT§(param1.§_-P1w§,_loc3_);
            _loc4_[0] = §_-F2L§.§_-CT§(_loc4_[0].getPositions(),_loc3_);
            _loc4_[1] = §_-F2L§.§_-CT§(_loc4_[1].getPositions(),_loc3_);
         }
         else if(Boolean(param1.§_-FV§ && param1.§_-FV§.length > 0) && Boolean(param1.§_-42I§) && param1.§_-42I§.length > 0)
         {
            _loc5_ = §_-F2L§.§_-CT§(param1.§_-FV§,_loc3_);
            _loc6_ = §_-F2L§.§_-CT§(param1.§_-42I§,_loc3_);
            _loc4_[0] = _loc5_;
            _loc4_[1] = _loc6_;
         }
         return _loc4_;
      }
      
      private function loadMapConfig(param1:§_-03A§) : void
      {
         var mapLoadData:§_-03A§ = param1;
         §_-J2l§.§_-e1L§(§_-s2a§.BATTLE_LOAD_MAP,function():void
         {
            clearMapLoader();
            if(mapLoadData.mapRecord.isLoaded())
            {
               onMapLoadingSuccess();
            }
            else
            {
               mapLoader = new §_-f2a§(mapLoadData.mapRecord);
               mapLoader.addEventListener(§_-x8§.ERROR,onMapLoadingError);
               mapLoader.addEventListener(§_-x8§.SUCCESS,onMapLoadingSuccess);
               mapLoader.load();
            }
         });
      }
      
      private function onMapLoadingSuccess(param1:starling.events.Event = null) : void
      {
         this._mapLoadData.success = true;
         dispatchEventWith(§_-x8§.SUCCESS,false,this._mapLoadData);
      }
      
      private function onMapLoadingError(param1:starling.events.Event) : void
      {
         §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ERROR),§_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.ERROR_SCENE_LOAD),{"id":this._mapLoadData.mapRecord.getId()}));
         dispatchEventWith(§_-x8§.ERROR,false,this._mapLoadData);
      }
      
      private function clearMapLoader() : void
      {
         if(this.mapLoader)
         {
            this.mapLoader.destroy();
         }
         this.mapLoader = null;
      }
      
      public function §_-n2n§(param1:§_-D3a§, param2:Boolean = false, param3:Boolean = false) : void
      {
         var mapRecord:§_-D3a§ = param1;
         var noMapGround:Boolean = param2;
         var indestructibleGround:Boolean = param3;
         §_-J2l§.§_-e1L§(§_-s2a§.BATTLE_LOAD_MAP,function():void
         {
            var _loc1_:Scene = new Scene();
            _loc1_.create(mapRecord,noMapGround,indestructibleGround,onSceneCreated);
         });
      }
      
      private function onSceneCreated(param1:Scene) : void
      {
         §_-J2l§.§_-m1I§();
         this.§_-QC§ = param1;
         dispatchEventWith(starling.events.Event.COMPLETE,false,param1);
      }
      
      private function §_-z1a§(param1:§_-81M§) : void
      {
         if(this.§_-D3d§.length == 0)
         {
            this.§_-D3d§ = this.§_-HQ§(this.§_-T1Z§(param1));
            this.§_-83y§ = Application.userProfile.getLevel();
         }
         else if(this.§_-83y§ != Application.userProfile.getLevel())
         {
            this.§_-s1f§(Application.userProfile.getLevel(),param1);
            this.§_-83y§ = Application.userProfile.getLevel();
         }
      }
      
      private function §_-s1f§(param1:uint, param2:§_-81M§) : void
      {
         var _loc4_:§_-D3a§ = null;
         var _loc3_:Array = §_-p1i§.§_-73p§();
         var _loc5_:int = 0;
         var _loc6_:* = _loc3_;
         for each(_loc5_ in _loc6_)
         {
            var _temp_1:* = _loc5_;
            _loc4_ = _loc5_;
            if(Boolean(_loc4_.minLevel == param1) && Boolean(_loc4_) && _loc4_.§_-p1s§(param2))
            {
               this.§_-D3d§.push(_loc4_);
            }
         }
      }
      
      public function §_-T1Z§(param1:§_-81M§ = null, param2:Boolean = false) : Array
      {
         var _loc5_:§_-D3a§ = null;
         var _loc3_:Array = §_-p1i§.§_-73p§();
         var _loc4_:Array = [];
         for each(_loc5_ in _loc3_)
         {
            if(§_-F4§.§_-Ue§(_loc5_,param2) && _loc5_.§_-p1s§(param1))
            {
               _loc4_.push(_loc5_);
            }
         }
         return _loc4_;
      }
      
      private function §_-HQ§(param1:Array) : Array
      {
         var _loc4_:§_-D3a§ = null;
         var _loc5_:uint = 0;
         var _loc6_:uint = 0;
         var _loc7_:Number = NaN;
         var _loc8_:uint = 0;
         var _loc2_:uint = 30;
         var _loc3_:Number = §_-p1i§.§_-73p§().length / _loc2_;
         for each(_loc4_ in param1)
         {
            _loc4_.attractiveness = _loc4_.minLevel + _loc4_.id / _loc3_;
         }
         _loc5_ = param1.length / 2;
         while(_loc5_ > 0)
         {
            _loc6_ = _loc5_;
            while(_loc6_ < param1.length)
            {
               _loc7_ = Number(param1[_loc6_].attractiveness);
               _loc8_ = _loc6_;
               while(_loc8_ >= _loc5_ && param1[_loc8_ - _loc5_].attractiveness > _loc7_)
               {
                  param1[_loc8_].attractiveness = param1[_loc8_ - _loc5_].attractiveness;
                  _loc8_ -= _loc5_;
               }
               param1[_loc8_].attractiveness = _loc7_;
               _loc6_++;
            }
            _loc5_ = Math.round(_loc5_ / 2.2);
         }
         this.§_-M1D§ = this.§_-Mh§;
         return param1;
      }
      
      public function §_-tJ§() : void
      {
         if(Boolean(this.§_-QC§) && this.§_-QC§.isCreated())
         {
            this.§_-QC§.dispose();
         }
         this.§_-QC§ = null;
      }
   }
}

