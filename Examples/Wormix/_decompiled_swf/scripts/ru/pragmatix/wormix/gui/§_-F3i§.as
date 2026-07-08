package ru.pragmatix.wormix.gui
{
   import §_-2k§.*;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-zF§;
   import §_-82l§.§_-A2Y§;
   import §_-931§.§_-02O§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-b2K§;
   import §_-L2F§.§_-I1q§;
   import §_-L2F§.§_-t2Z§;
   import §_-Ly§.§_-7f§;
   import §_-RE§.§_-qW§;
   import §_-SP§.§_-M4§;
   import §_-T2V§.§_-13F§;
   import §_-T2h§.*;
   import §_-Tm§.§_-RF§;
   import §_-Vg§.*;
   import §_-Y1O§.*;
   import §_-Y1b§.*;
   import §_-YF§.§_-q2v§;
   import §_-Z2S§.§_-yB§;
   import §_-b1F§.§_-G4§;
   import §_-b1R§.§_-C3P§;
   import §_-d26§.§_-d1H§;
   import §_-j22§.§_-5g§;
   import §_-k2s§.§_-e1U§;
   import §_-l1K§.§_-F3m§;
   import §_-l1K§.§_-I7§;
   import §_-l1K§.§_-d2j§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-Rs§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-y1S§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.LoaderCore;
   import com.hurlant.crypto.tls.§_-e1A§;
   import feathers.controls.Button;
   import feathers.controls.List;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.IFeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.data.IListCollection;
   import feathers.data.ListCollection;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.LayoutBoundsResult;
   import feathers.layout.ViewPortBounds;
   import feathers.utils.touch.TapToTrigger;
   import flash.events.*;
   import flash.geom.Point;
   import flash.globalization.DateTimeFormatter;
   import flash.text.TextFormat;
   import flash.utils.*;
   import org.as3commons.zip.*;
   import org.swiftsuspenders.*;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.response.edit.CloseExitClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.Disconnect;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.messages.server.BuyUnlockMissionResult;
   import ru.pragmatix.wormix.messages.server.ProfilesResult;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.GameObjectUtils;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.social.*;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.text.TextField;
   import starling.utils.Align;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class §_-F3i§ extends §_-E2w§
   {
      
      protected var §_-rP§:§_-02O§ = new §_-02O§("ok");
      
      protected var okBtn:DisplayObject;
      
      protected var loading:DisplayObjectContainer;
      
      protected var §_-i2K§:§_-t1R§;
      
      protected var §_-1k§:§_-t1R§;
      
      protected var §_-Co§:§_-t1R§;
      
      protected var panel:DisplayObject;
      
      protected var §_-G9§:String = "panel";
      
      public function §_-F3i§(param1:DisplayObjectContainer)
      {
         super(param1);
         this.loading = getChildByName("loading") as DisplayObjectContainer;
         if(this.§_-rP§.name)
         {
            this.okBtn = getChildByName(this.§_-rP§.name);
         }
         this.§_-P27§();
         §_-YQ§.§_-S1U§(this.okBtn,this.§_-rP§);
         this.panel = getChildByName(this.§_-G9§);
      }
      
      override protected function §_-q2a§() : void
      {
         super.§_-q2a§();
         if(this.okBtn)
         {
            if(this.okBtn is Button)
            {
               this.okBtn.addEventListener(starling.events.Event.TRIGGERED,this.§_-x2X§);
            }
            else
            {
               this.okBtn.addEventListener(starling.events.TouchEvent.TOUCH,this.onOkClick);
            }
         }
      }
      
      public function §_-H3b§(param1:Boolean) : void
      {
         if(this.loading)
         {
            if(param1 && !§_-j2l§().contains(this.loading))
            {
               §_-j2l§().addChild(this.loading);
            }
            else if(!param1 && §_-j2l§().contains(this.loading))
            {
               §_-j2l§().removeChild(this.loading);
            }
         }
         if(Boolean(this.okBtn) && this.okBtn is Button)
         {
            (this.okBtn as Button).isEnabled = !param1;
         }
      }
      
      protected function onOkClick(param1:starling.events.TouchEvent) : void
      {
         if(!param1 || Boolean(param1.getTouch(param1.currentTarget as DisplayObject,TouchPhase.ENDED)))
         {
            this.§_-b1H§();
         }
      }
      
      protected function §_-x2X§(param1:starling.events.Event) : void
      {
         this.§_-b1H§();
      }
      
      protected function §_-b1H§() : void
      {
         dispatchEvent(new starling.events.Event(§_-M4§.MENU_OK_ACTION));
      }
      
      protected function §_-93S§(param1:starling.events.Event = null) : void
      {
         this.§_-P27§();
         §_-YQ§.§_-S1U§(this.okBtn,this.§_-rP§);
      }
      
      private function §_-P27§() : void
      {
         var _loc1_:DisplayObject = null;
         if(Boolean(this.loading) && Boolean(this.loading.getChildByName("text")))
         {
            _loc1_ = this.loading.getChildByName("text");
            if(_loc1_ is TextField)
            {
               (_loc1_ as TextField).text = §_-s2a§.§_-K3t§(§_-7f§.LOADING);
               (_loc1_ as TextField).format.horizontalAlign = Align.LEFT;
            }
            else if(_loc1_ is ITextRenderer)
            {
               (_loc1_ as ITextRenderer).text = §_-s2a§.§_-K3t§(§_-7f§.LOADING);
            }
         }
      }
      
      public function activate() : void
      {
         if(this.§_-i2K§)
         {
            §_-h2B§.play(this.§_-i2K§);
         }
         if(this.§_-Co§)
         {
            §_-h2B§.§_-S1§(this.§_-Co§);
         }
      }
      
      public function deactivate() : void
      {
         if(this.§_-Co§)
         {
            §_-h2B§.§_-C3l§();
         }
         if(this.§_-1k§)
         {
            §_-h2B§.play(this.§_-1k§);
         }
      }
      
      public function §_-zz§(param1:§_-t1R§) : void
      {
         this.§_-i2K§ = param1;
      }
      
      public function §_-c28§(param1:§_-t1R§) : void
      {
         this.§_-Co§ = param1;
      }
      
      override public function get width() : Number
      {
         return this.panel ? this.panel.width : super.width;
      }
      
      override public function set height(param1:Number) : void
      {
         if(this.panel)
         {
            this.panel.height = param1;
         }
         else
         {
            super.height = param1;
         }
      }
      
      override public function get height() : Number
      {
         return this.panel ? this.panel.height : super.height;
      }
      
      override public function set width(param1:Number) : void
      {
         if(this.panel)
         {
            this.panel.width = param1;
         }
         else
         {
            super.width = param1;
         }
      }
      
      public function §_-kX§(param1:String) : void
      {
         this.§_-rP§.name = param1;
      }
      
      public function §_-e1Q§(param1:String) : void
      {
         this.§_-rP§.text = param1;
         §_-YQ§.§_-S1U§(this.okBtn,this.§_-rP§);
      }
      
      public function §_-12E§(param1:String) : void
      {
         this.§_-rP§.stringId = param1;
         §_-YQ§.§_-S1U§(this.okBtn,this.§_-rP§);
      }
      
      override public function dispose() : void
      {
         if(this.okBtn)
         {
            this.okBtn.dispose();
            this.okBtn = null;
         }
         if(this.loading)
         {
            this.loading.dispose();
            this.loading = null;
         }
         if(this.panel)
         {
            this.panel.dispose();
            this.panel = null;
         }
         super.dispose();
      }
   }
}

