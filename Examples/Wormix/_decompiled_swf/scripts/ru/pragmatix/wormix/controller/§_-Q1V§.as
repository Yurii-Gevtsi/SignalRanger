package ru.pragmatix.wormix.controller
{
   import §_-21A§.*;
   import §_-21p§.§_-4w§;
   import §_-31W§.§_-r26§;
   import §_-43V§.Random;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-63M§.§_-F3f§;
   import §_-82a§.*;
   import §_-82l§.§_-A2Y§;
   import §_-91A§.*;
   import §_-A1K§.§_-TA§;
   import §_-Af§.§_-k1I§;
   import §_-B1y§.*;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-92W§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.NewParamsWindow;
   import §_-DJ§.§_-J2l§;
   import §_-G2H§.*;
   import §_-L1H§.§_-b2K§;
   import §_-OJ§.§_-32g§;
   import §_-RE§.§_-r2q§;
   import §_-U1i§.*;
   import §_-W§.§_-ix§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-tc§;
   import §_-YF§.*;
   import §_-Z1K§.§_-A2n§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-ZB§.§_-A10§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-e2r§.*;
   import §_-fo§.§_-r5§;
   import §_-g2r§.§_-x8§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-Oi§;
   import §_-k2s§.§_-b0§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-e1k§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-Rs§;
   import §_-l1g§.§_-f2L§;
   import §_-l2r§.§_-K1t§;
   import §_-lh§.§_-z2A§;
   import §_-m0§.§_-a1O§;
   import §_-m2s§.§_-R11§;
   import §_-q26§.*;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-s1Q§.Artifacts;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-sQ§.§_-H3D§;
   import §_-u2J§.§_-A0§;
   import §_-w2i§.§_-Ia§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-y1S§;
   import §_-zI§.§_-sI§;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.distriqt.extension.pushnotifications.events.PushNotificationGroupEvent;
   import com.greensock.OverwriteManager;
   import com.greensock.TweenLite;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.*;
   import com.greensock.loading.core.*;
   import com.greensock.plugins.AutoAlphaPlugin;
   import com.greensock.plugins.BevelFilterPlugin;
   import com.greensock.plugins.BezierPlugin;
   import com.greensock.plugins.BezierThroughPlugin;
   import com.greensock.plugins.BlurFilterPlugin;
   import com.greensock.plugins.ColorMatrixFilterPlugin;
   import com.greensock.plugins.ColorTransformPlugin;
   import com.greensock.plugins.DropShadowFilterPlugin;
   import com.greensock.plugins.EndArrayPlugin;
   import com.greensock.plugins.FrameLabelPlugin;
   import com.greensock.plugins.FramePlugin;
   import com.greensock.plugins.GlowFilterPlugin;
   import com.greensock.plugins.HexColorsPlugin;
   import com.greensock.plugins.RemoveTintPlugin;
   import com.greensock.plugins.RoundPropsPlugin;
   import com.greensock.plugins.ShortRotationPlugin;
   import com.greensock.plugins.TintPlugin;
   import com.greensock.plugins.TweenPlugin;
   import com.greensock.plugins.VisiblePlugin;
   import com.greensock.plugins.VolumePlugin;
   import com.hurlant.math.BigInteger;
   import config.§_-B1K§;
   import config.§_-V2w§;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.LayoutGroup;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.ILayoutData;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import flash.display.Bitmap;
   import flash.display.DisplayObject;
   import flash.events.Event;
   import flash.events.SecurityErrorEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.social.response.email.EmailSendResponse;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.AbstractAuraEffect;
   import ru.pragmatix.wormix.fx.ArmatureAuraEffect;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.UfoTargetPointer;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.game.ai.*;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.*;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.JoinToChat;
   import ru.pragmatix.wormix.messages.client.JoinToChatResult;
   import ru.pragmatix.wormix.messages.client.LeaveFromChat;
   import ru.pragmatix.wormix.messages.client.PostToChat;
   import ru.pragmatix.wormix.messages.server.ChatMessageEvent;
   import ru.pragmatix.wormix.messages.server.Pong;
   import ru.pragmatix.wormix.messages.structures.PvpProfileStructure;
   import ru.pragmatix.wormix.messages.structures.ReconnectToSimpleBattleResultStructure;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.messages.structures.UnitStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.structures.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.Align;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-Q1V§ extends EventDispatcher
   {
      
      private static const §_-ov§:int = 60 * 60 * 24 * 30 * 1;
      
      private var §_-d1a§:§_-J2g§;
      
      private var §_-42E§:Boolean = false;
      
      private var §_-Zo§:int = -1;
      
      private var §_-z2e§:§_-A10§;
      
      public function §_-Q1V§(param1:§_-A10§)
      {
         super();
         this.§_-z2e§ = param1;
         this.isAvailable = this.§_-z2e§.§_-k2A§();
      }
      
      public function §_-11w§(param1:int) : void
      {
         if(this.§_-z2e§.§_-k2A§())
         {
            return;
         }
         var _loc2_:int = §_-vR§.§_-42H§ ? §_-ov§ : int(§_-ov§ / 30);
         var _loc3_:Boolean = param1 > 0 && §_-72u§.§_-M16§(param1) < _loc2_;
         this.isAvailable = _loc3_;
      }
      
      protected function get isAvailable() : Boolean
      {
         return this.§_-42E§;
      }
      
      protected function set isAvailable(param1:Boolean) : void
      {
         if(this.§_-42E§ != param1)
         {
            this.§_-42E§ = param1;
            dispatchEventWith(§_-Rs§.§_-12m§);
         }
      }
      
      public function getAvailable() : Boolean
      {
         return this.isAvailable;
      }
      
      public function activate() : void
      {
         §_-J2g§.§_-i1N§.addEventListener(§_-R1P§.SESSION_KEY_CHANGED,this.§_-S9§);
         dispatchEventWith(§_-Rs§.§_-v1§,false,§_-s2a§.§_-K3t§(§_-s2a§.CONNECTING));
         this.§_-G3t§();
      }
      
      public function deactivate() : void
      {
         §_-J2g§.§_-i1N§.removeEventListener(§_-R1P§.SESSION_KEY_CHANGED,this.§_-S9§);
         var _loc1_:LeaveFromChat = new LeaveFromChat();
         this.§_-X1I§(_loc1_);
         this.§_-C3W§();
      }
      
      private function §_-G3t§() : void
      {
         this.§_-h6§();
         if(this.§_-d1a§.§_-yt§())
         {
            this.§_-P1b§();
         }
         else
         {
            this.§_-N2C§();
         }
      }
      
      private function §_-N2C§() : void
      {
         this.§_-V1e§();
         this.§_-C3W§();
         this.§_-Zo§ = Starling.juggler.delayCall(this.§_-G3t§,1);
      }
      
      private function §_-V1e§() : void
      {
         if(this.§_-Zo§ > 0)
         {
            Starling.juggler.removeByID(this.§_-Zo§);
            this.§_-Zo§ = -1;
         }
      }
      
      private function §_-h6§() : void
      {
         this.§_-d1a§ = §_-J2g§.§_-i1N§;
         this.§_-d1a§.addListener(JoinToChatResult,this.§_-E1M§);
         this.§_-d1a§.addListener(ChatMessageEvent,this.§_-H3M§);
      }
      
      private function §_-C3W§() : void
      {
         this.§_-V1e§();
         if(this.§_-d1a§)
         {
            this.§_-d1a§.removeListener(JoinToChatResult,this.§_-E1M§);
            this.§_-d1a§.removeListener(ChatMessageEvent,this.§_-H3M§);
            this.§_-d1a§ = null;
         }
      }
      
      private function §_-S9§(param1:Object) : void
      {
         if(§_-J2g§.§_-i1N§.§_-yt§())
         {
            dispatchEventWith(§_-Rs§.§_-v1§,false,§_-s2a§.§_-K3t§(§_-s2a§.CONNECTING));
            this.§_-N2C§();
         }
      }
      
      public function §_-v1F§(param1:String) : void
      {
         var _loc2_:PostToChat = new PostToChat();
         _loc2_.message = param1;
         _loc2_.action = §_-A2Y§.§_-vu§.type;
         this.§_-X1I§(_loc2_);
      }
      
      public function §_-HU§(param1:§_-R11§) : void
      {
         var _loc2_:PostToChat = new PostToChat();
         _loc2_.message = §_-R11§.§_-53y§(param1);
         _loc2_.action = §_-A2Y§.§_-82y§.type;
         this.§_-X1I§(_loc2_);
      }
      
      private function §_-X1I§(param1:Object) : void
      {
         if(this.§_-d1a§)
         {
            this.§_-d1a§.send(param1);
         }
      }
      
      private function §_-P1b§() : void
      {
         var _loc1_:JoinToChat = new JoinToChat();
         this.§_-X1I§(_loc1_);
      }
      
      private function §_-E1M§(param1:§_-63i§) : void
      {
         var _loc2_:JoinToChatResult = param1.message as JoinToChatResult;
         dispatchEventWith(§_-Rs§.§_-M1X§,false,_loc2_.chatHistory);
      }
      
      private function §_-H3M§(param1:§_-63i§) : void
      {
         var _loc2_:ChatMessageEvent = param1.message as ChatMessageEvent;
         dispatchEventWith(§_-Rs§.§_-P8§,false,_loc2_);
      }
   }
}

