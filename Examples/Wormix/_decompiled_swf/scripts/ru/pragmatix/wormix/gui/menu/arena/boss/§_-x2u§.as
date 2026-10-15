package ru.pragmatix.wormix.gui.menu.arena.boss
{
   import §_-12a§.§_-41b§;
   import §_-21p§.§_-4w§;
   import §_-31W§.*;
   import §_-62§.*;
   import §_-73I§.§_-q15§;
   import §_-82a§.*;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-LE§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-B2z§.§_-63i§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.*;
   import §_-F2Q§.GestureEvent;
   import §_-H16§.*;
   import §_-N8§.§_-o2w§;
   import §_-Nq§.*;
   import §_-TF§.§_-q1j§;
   import §_-YF§.§_-q2v§;
   import §_-b1F§.§_-G4§;
   import §_-c2O§.§_-03I§;
   import §_-c2O§.§_-Bq§;
   import §_-j1w§.§_-lD§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-C3A§;
   import §_-k2s§.§_-fG§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.*;
   import §_-qH§.§_-AC§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.loading.core.*;
   import com.worlize.websocket.*;
   import dragonBones.Slot;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.Callout;
   import feathers.controls.Screen;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import flash.display.BitmapData;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.net.URLRequest;
   import flash.net.URLVariables;
   import flash.utils.clearTimeout;
   import flash.utils.getDefinitionByName;
   import org.gestouch.core.GestureState;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.response.CommonClanResponse;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.KeyboardEvent;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class §_-x2u§ extends Screen
   {
      
      private const §_-cc§:String = "back";
      
      private const §_-3V§:String = "friendName";
      
      protected var §_-q2N§:Button;
      
      protected var friendName:ITextRenderer;
      
      protected var §_-SR§:§_-Bq§;
      
      protected var §_-Hw§:DisplayObjectContainer;
      
      protected var §_-i2C§:§_-03I§;
      
      private var display:DisplayObjectContainer;
      
      private var §_-H2Z§:Class;
      
      private var §_-j2f§:Class;
      
      public function §_-x2u§(param1:DisplayObjectContainer, param2:Class, param3:Class)
      {
         super();
         this.display = param1;
         this.§_-H2Z§ = param2;
         this.§_-j2f§ = param3;
      }
      
      public function §_-N28§(param1:String) : void
      {
         this.§_-q2N§.visible = false;
         §_-R2g§.§_-k2g§(this.friendName,true);
         this.friendName.text = StringUtil.§_-CN§(§_-s2a§.§_-K3t§(§_-s2a§.BOSS_COOP_TITLE),"#866D56") + "  " + StringUtil.§_-CN§(§_-J2h§.§_-j7§(param1),"#2BA0F8");
         this.friendName.visible = true;
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         addChild(this.display);
         this.display.touchable = true;
         this.§_-32j§(this.display);
         this.friendName = this.display.getChildByName(this.§_-3V§) as ITextRenderer;
         this.friendName.visible = false;
         this.§_-SR§ = new this.§_-H2Z§(this.display);
         this.§_-Hw§ = this.display.getChildByName("infoContainer") as DisplayObjectContainer;
         this.§_-Hw§.touchable = true;
         this.§_-i2C§ = new this.§_-j2f§(this.§_-Hw§.getChildByName("bossesInfoContainer"));
         this.§_-SR§.addEventListener(starling.events.Event.CHANGE,this.§_-p2N§);
         this.§_-SR§.addEventListener(FeathersEventType.SCROLL_START,this.§_-vA§);
         this.§_-SR§.addEventListener(FeathersEventType.SCROLL_COMPLETE,this.§_-vA§);
         this.activate();
      }
      
      private function §_-32j§(param1:DisplayObjectContainer) : void
      {
         this.§_-q2N§ = param1.getChildByName(this.§_-cc§) as Button;
         this.§_-q2N§.addEventListener(starling.events.Event.TRIGGERED,this.§_-52u§);
      }
      
      protected function §_-52u§(param1:starling.events.Event) : void
      {
         dispatchEventWith(starling.events.Event.CANCEL);
      }
      
      private function §_-vA§(param1:starling.events.Event) : void
      {
         this.§_-Hw§.touchable = param1.type == FeathersEventType.SCROLL_COMPLETE;
      }
      
      protected function activate() : void
      {
         var _loc1_:Array = this.§_-QB§();
         this.§_-SR§.setData(_loc1_,this.§_-I1p§());
      }
      
      protected function §_-QB§() : Array
      {
         return [];
      }
      
      public function update() : void
      {
         this.§_-SR§.update();
         this.§_-p2N§(null);
      }
      
      protected function §_-p2N§(param1:starling.events.Event) : void
      {
      }
      
      override public function dispose() : void
      {
         if(this.§_-q2N§)
         {
            this.§_-q2N§.removeEventListener(starling.events.Event.TRIGGERED,this.§_-52u§);
            this.§_-q2N§ = null;
         }
         if(this.§_-SR§)
         {
            this.§_-SR§.removeEventListener(starling.events.Event.CHANGE,this.§_-p2N§);
            this.§_-SR§.dispose();
            this.§_-SR§ = null;
         }
         if(this.§_-i2C§)
         {
            this.§_-i2C§.dispose();
            this.§_-i2C§ = null;
            this.§_-Hw§ = §_-c1S§.dispose(this.§_-Hw§);
         }
         super.dispose();
      }
      
      protected function §_-I1p§() : int
      {
         return -1;
      }
      
      protected function §_-m7§(param1:int) : void
      {
      }
   }
}

