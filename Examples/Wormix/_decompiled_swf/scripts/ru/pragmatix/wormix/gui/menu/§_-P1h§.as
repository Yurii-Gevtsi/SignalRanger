package ru.pragmatix.wormix.gui.menu
{
   import §_-11L§.*;
   import §_-31W§.*;
   import §_-52V§.§_-R9§;
   import §_-533§.§_-c2b§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-o6§;
   import §_-62§.§_-zF§;
   import §_-63U§.§_-41y§;
   import §_-73I§.§_-Y1l§;
   import §_-73d§.§_-V19§;
   import §_-931§.§_-sz§;
   import §_-A1K§.§_-TA§;
   import §_-A2F§.§_-4B§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-1Y§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.*;
   import §_-F39§.§_-D14§;
   import §_-H2g§.§_-di§;
   import §_-J31§.§_-e2J§;
   import §_-J3L§.§_-K2i§;
   import §_-Ly§.§_-81L§;
   import §_-Ly§.§_-I2K§;
   import §_-P1B§.§_-b21§;
   import §_-P1z§.*;
   import §_-S1N§.§_-R1U§;
   import §_-SP§.§_-j1f§;
   import §_-T2h§.§_-92J§;
   import §_-TF§.§_-F1B§;
   import §_-TF§.§_-q1j§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1b§.§_-81l§;
   import §_-Y1b§.§_-OP§;
   import §_-Y23§.§_-ZE§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-b1F§.§_-v2Q§;
   import §_-bI§.*;
   import §_-d26§.§_-d1H§;
   import §_-hO§.§_-729§;
   import §_-hO§.§_-v24§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-u2J§.§_-T1K§;
   import §_-xu§.§_-91H§;
   import §_-zI§.§_-8D§;
   import com.hurlant.crypto.tls.§_-F16§;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import com.hurlant.math.*;
   import feathers.controls.*;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.text.BitmapFontTextRenderer;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.data.*;
   import feathers.layout.*;
   import feathers.text.BitmapFontTextFormat;
   import flash.events.Event;
   import flash.events.MouseEvent;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.*;
   import ru.pragmatix.wormix.messages.clans.*;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.DynamiteStick;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.filters.ColorMatrixFilter;
   import starling.text.TextField;
   import starling.utils.AssetManager;
   import starling.utils.Color;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   use namespace bi_internal;
   
   public class §_-P1h§ extends §_-E2w§
   {
      
      protected var item:Object = null;
      
      private var selected:Boolean;
      
      private var §_-G3G§:DisplayObject;
      
      private var §_-J8§:DisplayObject;
      
      private var selectable:Boolean = true;
      
      private var §_-m2n§:Boolean = false;
      
      protected var §_-h19§:uint;
      
      protected var §_-R24§:uint;
      
      public function §_-P1h§(param1:DisplayObjectContainer)
      {
         super(param1);
         var _loc2_:DisplayObjectContainer = §_-j2l§();
         _loc2_.touchable = true;
         this.§_-D3V§("selected","selection");
         this.setSelected(false);
         if(!this.§_-G3G§)
         {
            this.selectable = false;
         }
      }
      
      override protected function §_-O2y§() : void
      {
         super.§_-O2y§();
         §_-F11§ = §_-81L§.§_-jn§;
      }
      
      public function setItem(param1:Object) : void
      {
         this.item = param1;
      }
      
      override protected function §_-q2a§() : void
      {
         super.§_-q2a§();
         getContainer().addEventListener(TouchEvent.TOUCH,this.§_-23W§);
      }
      
      protected function §_-D3V§(param1:String, param2:String = null) : void
      {
         this.§_-J8§ = getChildByName(param1);
         this.§_-G3G§ = getChildByName(param2);
      }
      
      public function getItem() : Object
      {
         return this.item;
      }
      
      private function §_-23W§(param1:starling.events.Event) : void
      {
         var _loc2_:TouchEvent = param1 as TouchEvent;
         var _loc3_:String = §_-w1L§ ? TouchPhase.BEGAN : TouchPhase.ENDED;
         if(Boolean(_loc2_) && Boolean(_loc2_.getTouch(param1.currentTarget as DisplayObject,_loc3_)))
         {
            this.dispatchEvent(new §_-j1f§(§_-j1f§.CLICK));
         }
      }
      
      public function §_-R2w§(param1:Boolean) : void
      {
         if(this.§_-G3G§)
         {
            this.§_-G3G§.visible = param1;
         }
      }
      
      public function setSelected(param1:Boolean) : void
      {
         this.selected = param1;
         if(this.§_-J8§)
         {
            this.§_-J8§.visible = param1;
         }
         this.§_-R2w§(param1);
      }
      
      public function isSelected() : Boolean
      {
         return this.selected;
      }
      
      public function §_-v12§(param1:Boolean) : void
      {
         this.selectable = param1;
      }
      
      public function §_-Pv§(param1:Boolean) : void
      {
         this.§_-m2n§ = param1;
      }
      
      public function isSelectable() : Boolean
      {
         return this.selectable;
      }
      
      public function §_-V2E§() : Boolean
      {
         return this.§_-m2n§;
      }
      
      public function §_-L3§(param1:uint) : void
      {
         this.§_-h19§ = param1;
      }
      
      public function getCellIndex() : uint
      {
         return this.§_-h19§;
      }
      
      override public function §_-tx§(param1:Boolean) : void
      {
         super.§_-tx§(param1);
         dispatchEvent(new starling.events.Event(§_-K2i§.DISABLED));
      }
      
      public function §_-r2K§(param1:uint) : void
      {
         this.§_-R24§ = param1;
      }
      
      public function §_-z1r§() : uint
      {
         return this.§_-R24§;
      }
      
      override public function §_-3d§() : Object
      {
         return this.getItem();
      }
      
      override public function dispose() : void
      {
         if(getContainer())
         {
            getContainer().removeEventListener(TouchEvent.TOUCH,this.§_-23W§);
            if(getContainer().parent)
            {
               getContainer().parent.removeChild(getContainer());
            }
         }
         this.item = null;
         if(this.§_-J8§)
         {
            this.§_-J8§.dispose();
            this.§_-J8§ = null;
         }
         if(this.§_-G3G§)
         {
            this.§_-G3G§.dispose();
            this.§_-G3G§ = null;
         }
         super.dispose();
      }
   }
}

