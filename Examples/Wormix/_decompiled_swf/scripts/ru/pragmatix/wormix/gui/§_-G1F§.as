package ru.pragmatix.wormix.gui
{
   import §_-21p§.§_-4w§;
   import §_-31W§.§_-r26§;
   import §_-43V§.Random;
   import §_-5Y§.§_-h2c§;
   import §_-B1y§.§_-F1P§;
   import §_-D3A§.§_-TZ§;
   import §_-G2H§.*;
   import §_-J31§.§_-e2J§;
   import §_-L1H§.§_-C3e§;
   import §_-P2i§.§_-b7§;
   import §_-Y1O§.*;
   import §_-Y1b§.§_-81l§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-gG§.§_-8u§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-Vr§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Ia§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.hurlant.crypto.tls.*;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import flash.globalization.DateTimeFormatter;
   import flash.globalization.LocaleID;
   import flash.utils.ByteArray;
   import org.gestouch.core.Touch;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.BossTypesTab;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-B1m§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class §_-G1F§
   {
      
      private static const §_-U28§:String = "timerLabel";
      
      private var _display:DisplayObject;
      
      private var _visible:Boolean;
      
      private var §_-y1b§:DisplayObject;
      
      private var §_-g2P§:§_-C2c§;
      
      private var date:Date;
      
      private var §_-320§:DateTimeFormatter;
      
      public function §_-G1F§(param1:Sprite, param2:§_-C2c§)
      {
         super();
         this._display = param1;
         this.§_-g2P§ = param2;
         if(param1)
         {
            this.§_-y1b§ = param1.getChildByName(§_-U28§);
         }
         this.date = new Date();
         this.§_-320§ = new DateTimeFormatter(LocaleID.DEFAULT);
         this.§_-320§.setDateTimePattern("HH:mm:ss");
         this.§_-A3O§();
         this.§_-o2J§(param2.§_-H3K§);
      }
      
      public function §_-7N§(param1:§_-C2c§) : void
      {
         if(param1 != this.§_-g2P§)
         {
            this.§_-j2O§();
            this.§_-g2P§ = param1;
            this.§_-A3O§();
            this.§_-o2J§(this.§_-g2P§.§_-H3K§);
         }
      }
      
      private function §_-ti§(param1:Event) : void
      {
         this.§_-o2J§(Math.max(0,param1.data as uint));
      }
      
      private function §_-o2J§(param1:uint) : void
      {
         var _loc2_:Number = NaN;
         if(this.§_-y1b§)
         {
            _loc2_ = §_-72u§.§_-74§(param1);
            if(_loc2_ <= 0)
            {
               this.date.setTime(param1 * §_-72u§.§_-E1O§);
               this.§_-y1b§["text"] = this.§_-320§.formatUTC(this.date);
            }
            else
            {
               this.§_-y1b§["text"] = §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.DAY_TEXT),{"value":_loc2_.toString()});
            }
         }
      }
      
      private function §_-A3O§() : void
      {
         this.§_-g2P§.addEventListener(Event.CHANGE,this.§_-ti§);
      }
      
      private function §_-j2O§() : void
      {
         if(Boolean(this.§_-g2P§) && this.§_-g2P§.hasEventListener(Event.CHANGE))
         {
            this.§_-g2P§.removeEventListener(Event.CHANGE,this.§_-ti§);
         }
      }
      
      public function get visible() : Boolean
      {
         return this._visible;
      }
      
      public function set visible(param1:Boolean) : void
      {
         if(param1 != this._visible)
         {
            if(param1)
            {
               this.§_-A3O§();
            }
            else
            {
               this.§_-j2O§();
            }
         }
         this._visible = param1;
      }
      
      public function dispose() : void
      {
         if(this._display)
         {
            this._display.dispose();
         }
         this.§_-j2O§();
         if(this.§_-y1b§)
         {
            this.§_-y1b§.dispose();
            this.§_-y1b§ = null;
         }
         this.§_-g2P§ = null;
      }
      
      public function get display() : DisplayObject
      {
         return this._display;
      }
   }
}

