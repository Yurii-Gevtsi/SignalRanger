package ru.pragmatix.flox.gui.controls
{
   import §_-02q§.§_-U2a§;
   import §_-03T§.§_-2p§;
   import §_-03T§.§_-K3B§;
   import §_-12W§.§_-J3J§;
   import §_-12W§.§_-X2w§;
   import §_-12W§.§_-l23§;
   import §_-21p§.§_-4w§;
   import §_-31W§.AwardCell;
   import §_-43V§.Random;
   import §_-51M§.Dragon;
   import §_-52L§.§_-u2x§;
   import §_-62§.*;
   import §_-63M§.§_-F3f§;
   import §_-73I§.§_-Y1l§;
   import §_-91A§.*;
   import §_-91B§.§_-QK§;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.*;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F2M§.*;
   import §_-F39§.*;
   import §_-G2H§.*;
   import §_-GQ§.§_-31v§;
   import §_-H2g§.§_-di§;
   import §_-HD§.*;
   import §_-I1l§.*;
   import §_-J31§.§_-e2J§;
   import §_-K35§.§_-gY§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-Ly§.§_-I2K§;
   import §_-O2L§.*;
   import §_-OJ§.§_-32g§;
   import §_-OJ§.§_-GU§;
   import §_-OJ§.§_-I3e§;
   import §_-OJ§.§_-L3B§;
   import §_-OJ§.§_-S1v§;
   import §_-OJ§.§_-Z1U§;
   import §_-OJ§.§_-jt§;
   import §_-OJ§.§_-sY§;
   import §_-P2i§.§_-b7§;
   import §_-RE§.§_-D3v§;
   import §_-T2h§.*;
   import §_-Ta§.*;
   import §_-U1i§.§_-O2i§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-yA§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1b§.§_-81l§;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.§_-N3§;
   import §_-YF§.§_-q2v§;
   import §_-ZP§.§_-H1H§;
   import §_-ZP§.§_-YG§;
   import §_-ZP§.§_-w1J§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-b1R§.§_-81E§;
   import §_-b1R§.§_-C3P§;
   import §_-fo§.§_-21L§;
   import §_-fo§.§_-r5§;
   import §_-g2r§.§_-03A§;
   import §_-g2r§.§_-x8§;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-uZ§;
   import §_-hO§.§_-03k§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-k13§.§_-j23§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-m0§.§_-a1O§;
   import §_-n1w§.X509Certificate;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-p14§.§_-A3Y§;
   import §_-p18§.§_-vd§;
   import §_-q26§.*;
   import §_-qH§.§_-S25§;
   import §_-r1C§.*;
   import §_-w2W§.WardrobeRoomScreen;
   import §_-w2i§.§_-Ia§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.*;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.core.*;
   import com.hurlant.math.BigInteger;
   import com.hurlant.util.der.§_-p1B§;
   import dragonBones.animation.WorldClock;
   import dragonBones.starling.StarlingFactory;
   import dragonBones.starling.StarlingTextureAtlasData;
   import dragonBones.textures.TextureAtlasData;
   import feathers.controls.AutoSizeMode;
   import feathers.controls.List;
   import feathers.controls.NumericStepper;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.controls.ScrollPolicy;
   import feathers.core.ITextRenderer;
   import feathers.data.ArrayCollection;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.ILayout;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.utils.textures.TextureCache;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.MovieClip;
   import flash.display.Shape;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.events.MouseEvent;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import flash.media.Sound;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.UfoTargetPointer;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.§_-136§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.§_-K1P§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.§_-P2p§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.§_-m20§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.NotInClanChatScreen;
   import ru.pragmatix.wormix.gui.menu.shop.§_-s1V§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.edit.RenameClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.clans.response.LowerRankInClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.AddToGroupResult;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.items.craft.§_-13q§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-a1t§ extends §_-K3B§ implements §_-yf§, §_-S1v§, §_-Z1U§
   {
      
      protected var isInitialized:Boolean;
      
      public var destroyOnRemove:Boolean = false;
      
      protected var container:flash.display.DisplayObject;
      
      public var disabled:Boolean;
      
      private var hintable:§_-Z1U§;
      
      protected var §_-p2T§:String;
      
      protected var §_-F11§:§_-t1R§;
      
      protected var §_-V28§:§_-t1R§;
      
      private var _id:String;
      
      private var §_-C5§:§_-2p§;
      
      private var §_-zd§:§_-32g§;
      
      private var §_-h2b§:Boolean = §_-81L§.§_-sq§;
      
      public function §_-a1t§(param1:flash.display.DisplayObject)
      {
         super();
         this.§_-C5§ = new §_-2p§();
         this.§_-O2y§();
         if(!this.id)
         {
            if(param1)
            {
               this._id = param1.name;
            }
            else
            {
               this._id = §_-I2K§.§_-wu§();
            }
         }
         §_-U2a§.created(this);
         this.container = param1;
         this.§_-Ii§(flash.events.Event.ADDED_TO_STAGE,this.onAddedToStage);
         this.§_-Ii§(flash.events.Event.REMOVED_FROM_STAGE,this.onRemovedFromStage);
         this.§_-Ii§(this.§_-p2T§,this.§_-Y1W§);
         if(this.§_-V28§)
         {
            this.§_-Ii§(MouseEvent.ROLL_OVER,this.§_-J2n§);
         }
      }
      
      override public function destroy() : void
      {
         §_-U2a§.§_-R22§(this);
         if(this.hintable)
         {
            this.hintable.destroy();
            this.hintable = null;
         }
         this.§_-C5§.destroy();
         this.§_-C5§ = null;
         this.isInitialized = false;
         this.container = null;
         this.§_-F11§ = null;
         this.§_-V28§ = null;
         this.§_-zd§ = null;
         super.destroy();
      }
      
      protected function §_-Y1W§(param1:flash.events.Event) : void
      {
         this.§_-l1S§();
      }
      
      public function getContainer() : flash.display.DisplayObject
      {
         return this.container;
      }
      
      public function §_-tx§(param1:Boolean) : void
      {
         this.disabled = param1;
      }
      
      public function §_-i2B§() : Boolean
      {
         return this.disabled;
      }
      
      public function isVisible() : Boolean
      {
         return this.container.visible;
      }
      
      public function §_-c1v§(param1:Boolean) : void
      {
         this.container.visible = param1;
      }
      
      protected function onAddedToStage(param1:flash.events.Event) : void
      {
         if(!this.isInitialized)
         {
            this.§_-q2a§();
            this.isInitialized = true;
         }
      }
      
      protected function onRemovedFromStage(param1:flash.events.Event) : void
      {
         if(this.destroyOnRemove)
         {
            this.destroy();
         }
      }
      
      public function §_-11P§(param1:String, param2:§_-sY§ = null, param3:String = null, param4:TextFormat = null) : void
      {
         this.§_-e1g§(§_-32g§.§_-Oh§(param1,param3,param4),§_-L3B§.§_-xE§(param2));
      }
      
      public function §_-gJ§(param1:String, param2:§_-sY§ = null, param3:String = null, param4:TextFormat = null) : void
      {
         this.§_-e1g§(§_-32g§.§_-CW§(param1,param3,param4),§_-L3B§.§_-xE§(param2));
      }
      
      public function §_-JM§(param1:String, param2:§_-L3B§ = null, param3:String = null, param4:TextFormat = null) : void
      {
         this.§_-e1g§(§_-32g§.§_-Oh§(param1,param3,param4),param2);
      }
      
      public function §_-RL§() : §_-jt§
      {
         if(this.hintable)
         {
            return this.hintable.§_-RL§();
         }
         return null;
      }
      
      private function §_-e1g§(param1:§_-32g§, param2:§_-L3B§) : void
      {
         this.§_-zd§ = param1;
         this.§_-A1§(§_-I3e§.§_-i1N§,param2);
      }
      
      public function §_-A1§(param1:§_-jt§, param2:§_-L3B§ = null) : void
      {
         if(!this.hintable && Boolean(param1))
         {
            this.hintable = new §_-GU§(this,param2);
         }
         else
         {
            this.hintable.§_-Jn§();
         }
         if(param1)
         {
            this.hintable.§_-A1§(param1,param2);
         }
      }
      
      protected function §_-l1S§() : void
      {
         if(this.§_-F11§)
         {
            §_-h2B§.play(this.§_-F11§);
         }
      }
      
      protected function §_-F2N§() : void
      {
         if(this.hintable)
         {
            this.hintable.removeHintImmediately();
         }
      }
      
      protected function §_-q2a§() : void
      {
      }
      
      protected function §_-O2y§() : void
      {
         this.§_-p2T§ = §_-81L§.§_-a1Z§ ? MouseEvent.MOUSE_DOWN : MouseEvent.CLICK;
      }
      
      protected function §_-J2n§(param1:MouseEvent) : void
      {
         if(this.§_-V28§)
         {
            §_-h2B§.play(this.§_-V28§);
         }
      }
      
      public function get x() : Number
      {
         return this.container.x;
      }
      
      public function set x(param1:Number) : void
      {
         this.container.x = param1;
      }
      
      public function get y() : Number
      {
         return this.container.y;
      }
      
      public function set y(param1:Number) : void
      {
         this.container.y = param1;
      }
      
      public function get width() : Number
      {
         return this.container.width;
      }
      
      public function set width(param1:Number) : void
      {
         this.container.width = param1;
      }
      
      public function get height() : Number
      {
         return this.container.height;
      }
      
      public function set height(param1:Number) : void
      {
         this.container.height = param1;
      }
      
      public function §_-l1l§(param1:flash.display.DisplayObjectContainer) : void
      {
         param1.addChild(this.container);
      }
      
      public function §_-f9§(param1:flash.display.DisplayObjectContainer) : void
      {
         param1.removeChild(this.container);
      }
      
      public function §_-Y1L§(param1:§_-t1R§) : void
      {
         this.§_-F11§ = param1;
      }
      
      public function §_-rA§() : §_-t1R§
      {
         return this.§_-F11§;
      }
      
      public function §_-p1V§(param1:§_-t1R§) : void
      {
         this.§_-V28§ = param1;
      }
      
      public function get id() : String
      {
         return this._id;
      }
      
      public function set id(param1:String) : void
      {
         this._id = param1;
      }
      
      protected function §_-PP§(param1:String, param2:Function, param3:Boolean = false) : void
      {
         this.§_-C5§.removeEventListener(this.container,param1,param2,param3);
      }
      
      protected function §_-Ii§(param1:String, param2:Function, param3:Boolean = false, param4:int = 0, param5:Boolean = false) : void
      {
         this.§_-C5§.addEventListener(this.container,param1,param2,param3,param4,param5);
      }
      
      protected function §_-d1W§(param1:IEventDispatcher, param2:String, param3:Function, param4:Boolean = false) : void
      {
         this.§_-C5§.removeEventListener(param1,param2,param3,param4);
      }
      
      protected function §_-t22§(param1:IEventDispatcher, param2:String, param3:Function, param4:Boolean = false, param5:int = 0, param6:Boolean = false) : void
      {
         this.§_-C5§.addEventListener(param1,param2,param3,param4,param5,param6);
      }
      
      public function §_-h1C§() : Boolean
      {
         return Boolean(this.container.stage) && this.isVisible();
      }
      
      public function get removeAllWhenDisabled() : Boolean
      {
         return this.§_-h2b§;
      }
      
      public function set removeAllWhenDisabled(param1:Boolean) : void
      {
         this.§_-h2b§ = param1;
      }
      
      public function §_-3d§() : Object
      {
         return this.§_-zd§;
      }
      
      public function §_-G1l§() : flash.display.DisplayObject
      {
         return this.container;
      }
      
      public function §_-Q2b§(param1:§_-L3B§) : void
      {
         if(this.hintable)
         {
            this.hintable.§_-Q2b§(param1);
         }
      }
      
      public function §_-Jn§() : void
      {
         if(this.hintable)
         {
            this.hintable.§_-Jn§();
         }
      }
      
      public function removeHintImmediately() : void
      {
         if(this.hintable)
         {
            this.hintable.removeHintImmediately();
         }
      }
   }
}

