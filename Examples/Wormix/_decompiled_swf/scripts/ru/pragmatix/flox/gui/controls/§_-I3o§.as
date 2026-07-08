package ru.pragmatix.flox.gui.controls
{
   import §_-02q§.§_-11O§;
   import §_-02q§.§_-4s§;
   import §_-43V§.*;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-W1r§;
   import §_-82a§.*;
   import §_-91A§.§_-sH§;
   import §_-931§.§_-02O§;
   import §_-931§.§_-i2r§;
   import §_-931§.§_-sz§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.*;
   import §_-B2z§.§_-63i§;
   import §_-Bv§.§_-G3P§;
   import §_-D3A§.§_-TZ§;
   import §_-F39§.§_-i1i§;
   import §_-Ly§.§_-7f§;
   import §_-S1N§.§_-zk§;
   import §_-Y1b§.§_-OP§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-f1G§.*;
   import §_-fo§.*;
   import §_-hO§.§_-v24§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-p14§.§_-A3Y§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-F4§;
   import §_-zI§.§_-X1q§;
   import com.greensock.loading.LoaderMax;
   import com.hurlant.crypto.tls.*;
   import com.hurlant.math.BigInteger;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.MovieClip;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.geom.Point;
   import flash.text.TextFieldAutoSize;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.client.SteamTransactionCommitRequest;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.ammo.phase.HarmPhase;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   use namespace gestouch_internal;
   
   public class §_-I3o§ extends §_-zi§ implements §_-u2S§
   {
      
      protected var §_-x1M§:String = "loading";
      
      protected var §_-x3§:String;
      
      protected var §_-rP§:§_-02O§;
      
      protected var okBtn:§_-zk§;
      
      protected var loading:MovieClip;
      
      protected var §_-i2K§:§_-t1R§;
      
      protected var §_-1k§:§_-t1R§;
      
      protected var §_-Co§:§_-t1R§;
      
      protected var panel:flash.display.DisplayObject;
      
      protected var §_-G9§:String = "panel";
      
      public function §_-I3o§(param1:DisplayObjectContainer)
      {
         var _loc2_:§_-i2r§ = null;
         this.§_-rP§ = new §_-02O§("ok");
         super(param1);
         this.loading = getChildByName("loading") as MovieClip;
         if(Boolean(this.loading) && Boolean(this.loading.text))
         {
            _loc2_ = §_-U2y§("loading.text",§_-7f§.LOADING);
            _loc2_.autoSize = TextFieldAutoSize.LEFT;
         }
         if(this.§_-rP§.name)
         {
            this.okBtn = §_-02H§(this.§_-rP§.name);
            if(this.okBtn)
            {
               this.okBtn.§_-BQ§(this.onOkClick);
               §_-11O§.§_-S1U§(this.okBtn,this.§_-rP§);
            }
         }
         this.panel = getChildByName(this.§_-G9§);
      }
      
      public function §_-H3b§(param1:Boolean) : void
      {
         if(this.loading)
         {
            if(param1 && !§_-j2l§().contains(this.loading))
            {
               §_-72v§(this.loading);
               §_-4s§.§_-k1B§(this.loading);
            }
            else if(!param1 && §_-j2l§().contains(this.loading))
            {
               §_-k6§(this.loading);
            }
         }
         if(this.okBtn)
         {
            this.okBtn.§_-tx§(param1);
         }
      }
      
      protected function onOkClick(param1:MouseEvent) : void
      {
         this.§_-b1H§();
      }
      
      protected function §_-b1H§() : void
      {
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
      
      public function set §_-72C§(param1:String) : void
      {
         this.§_-e1Q§(param1);
      }
      
      public function set §_-sU§(param1:String) : void
      {
         this.§_-kX§(param1);
      }
      
      public function §_-kX§(param1:String) : void
      {
         this.§_-rP§.name = param1;
      }
      
      public function §_-e1Q§(param1:String) : void
      {
         this.§_-rP§.text = param1;
         §_-11O§.§_-S1U§(this.okBtn,this.§_-rP§);
      }
      
      public function §_-12E§(param1:String) : void
      {
         this.§_-rP§.stringId = param1;
         §_-11O§.§_-S1U§(this.okBtn,this.§_-rP§);
      }
      
      override public function destroy() : void
      {
         this.okBtn = null;
         this.loading = null;
         this.panel = null;
         super.destroy();
      }
   }
}

