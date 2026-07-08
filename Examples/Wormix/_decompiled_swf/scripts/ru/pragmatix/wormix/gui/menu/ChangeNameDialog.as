package ru.pragmatix.wormix.gui.menu
{
   import §_-12a§.§_-G2l§;
   import §_-21p§.§_-4w§;
   import §_-31W§.§_-Y1v§;
   import §_-31W§.§_-r26§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.*;
   import §_-73I§.§_-Y1l§;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-C2t§.§_-92W§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-V1Y§;
   import §_-DJ§.§_-fH§;
   import §_-G2H§.*;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-Y1O§.§_-V23§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-ZB§.§_-A10§;
   import §_-aM§.*;
   import §_-fo§.§_-r5§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-q26§.§_-Z25§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Ia§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-Uc§;
   import §_-z1g§.§_-lp§;
   import §_-z20§.§_-Tn§;
   import §_-zI§.§_-X1q§;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.hurlant.util.der.§_-UO§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.core.ITextRenderer;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.server.ShopResult;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.serialization.*;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.MolotovProjectile;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   use namespace gestouch_internal;
   
   public class ChangeNameDialog extends §_-V1Y§
   {
      
      public static const §_-CH§:int = 20;
      
      private var binder:Binder;
      
      private var §_-N1I§:String;
      
      private var §_-W2h§:String;
      
      public function ChangeNameDialog()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.binder = new Binder();
         §_-Z1p§("change_name_dialog",this.binder);
         addChild(this.binder._panel);
         this.binder._nameInput.maxChars = §_-CH§;
         §_-J2N§(this.binder._panel,Event.CLOSE,§_-R1t§);
         §_-J2N§(this.binder._nameInput,Event.CHANGE,this.§_-i2X§);
         §_-J2N§(this.binder._saveButton,Event.TRIGGERED,this.§_-y1l§);
         §_-J2N§(this.binder._buyButton,Event.TRIGGERED,this.§_-y1l§);
      }
      
      override public function dispose() : void
      {
         super.dispose();
      }
      
      override protected function draw() : void
      {
         var _loc1_:Boolean = false;
         super.draw();
         if(isInvalid(§_-dU§.§_-h26§))
         {
            this.binder._panel.title = §_-s2a§.§_-K3t§(§_-s2a§.CHANGE_NAME_TITLE);
            this.binder._note.text = §_-s2a§.§_-K3t§(§_-s2a§.CHANGE_NAME_NOTE);
            this.binder._saveButton.label = §_-s2a§.§_-K3t§(§_-s2a§.BUTTON_SAVE);
         }
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            this.binder._nameInput.text = this.§_-N1I§;
            this.binder._buyButton.label = this.§_-W2h§;
            §_-YQ§.§_-Xv§(this.binder._saveButton,this.§_-W2h§ == "");
            §_-YQ§.§_-Xv§(this.binder._buyButton,this.§_-W2h§ != "");
         }
         if(isInvalid(§_-dU§.NAME))
         {
            _loc1_ = this.binder._nameInput.text != this.§_-N1I§;
            this.binder._saveButton.isEnabled = _loc1_;
            this.binder._buyButton.isEnabled = _loc1_;
         }
      }
      
      public function setData(param1:String, param2:String = "") : void
      {
         this.§_-N1I§ = param1;
         this.§_-W2h§ = param2;
         invalidate(INVALIDATION_FLAG_DATA);
      }
      
      private function §_-i2X§(param1:Event) : void
      {
         invalidate(§_-dU§.NAME);
      }
      
      private function §_-y1l§(param1:Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         switch(param1.currentTarget)
         {
            case this.binder._saveButton:
               dispatchEventWith(Event.SELECT,false,this.binder._nameInput.text);
               break;
            case this.binder._buyButton:
               dispatchEventWith(Event.SELECT,false,this.binder._nameInput.text);
         }
      }
   }
}

import feathers.controls.Button;
import feathers.controls.Label;
import feathers.controls.Panel;
import feathers.controls.TextInput;

class Binder
{
   
   public var _panel:Panel;
   
   public var _nameInput:TextInput;
   
   public var _note:Label;
   
   public var _saveButton:Button;
   
   public var _buyButton:Button;
   
   public function Binder()
   {
      super();
   }
}
