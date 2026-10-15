package ru.pragmatix.wormix.gui.menu
{
   import §_-02q§.§_-4s§;
   import §_-02q§.§_-U2a§;
   import §_-03T§.§_-2p§;
   import §_-11L§.*;
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-2U§.*;
   import §_-31N§.§_-Yy§;
   import §_-5Y§.*;
   import §_-62§.*;
   import §_-63U§.§_-412§;
   import §_-63U§.§_-NV§;
   import §_-73I§.§_-Y1l§;
   import §_-73d§.§_-a2s§;
   import §_-73d§.§_-pT§;
   import §_-82a§.§_-pp§;
   import §_-83J§.§_-NT§;
   import §_-9§.§_-52A§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A1m§.MultipartURLLoader;
   import §_-A2U§.*;
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B1y§.§_-fM§;
   import §_-B1y§.§_-ts§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-G2m§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-p2U§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-DJ§.§_-j15§;
   import §_-E1I§.§_-b2n§;
   import §_-F2Q§.GestureEvent;
   import §_-F39§.*;
   import §_-G2H§.*;
   import §_-H2g§.§_-di§;
   import §_-HD§.*;
   import §_-J31§.§_-R1h§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-Ly§.§_-I2K§;
   import §_-OA§.§_-k2K§;
   import §_-P1B§.§_-b21§;
   import §_-P1z§.*;
   import §_-R2I§.*;
   import §_-SP§.§_-K2h§;
   import §_-SP§.§_-M4§;
   import §_-SP§.§_-j1f§;
   import §_-SP§.§_-l8§;
   import §_-T1S§.§_-E1a§;
   import §_-T1t§.§_-u19§;
   import §_-T2h§.*;
   import §_-TF§.§_-q1j§;
   import §_-Ta§.§_-G18§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-U16§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1b§.§_-81l§;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.§_-Zi§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-ZP§.§_-b23§;
   import §_-aM§.§_-E3R§;
   import §_-aM§.§_-P1N§;
   import §_-b1F§.§_-G4§;
   import §_-d26§.§_-d1H§;
   import §_-dS§.§_-D3a§;
   import §_-g1P§.§_-L38§;
   import §_-g2r§.§_-03A§;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Nl§;
   import §_-gG§.§_-Z2z§;
   import §_-hO§.§_-v24§;
   import §_-hv§.*;
   import §_-j1w§.*;
   import §_-j22§.§_-5g§;
   import §_-j2R§.§_-73G§;
   import §_-k2s§.§_-Go§;
   import §_-k2s§.§_-b0§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-m0§.§_-X1F§;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-s20§.§_-ZY§;
   import §_-wD§.*;
   import §_-x2Q§.§_-dU§;
   import §_-xu§.§_-91H§;
   import §_-z1g§.§_-522§;
   import §_-z1g§.§_-5c§;
   import §_-z1g§.§_-F4§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-A1s§;
   import §_-zI§.§_-X1q§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.pushnotifications.PushNotifications;
   import com.distriqt.extension.pushnotifications.events.AuthorisationEvent;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.hurlant.math.*;
   import com.hurlant.util.der.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.LayoutGroup;
   import feathers.controls.NumericStepper;
   import feathers.core.PopUpManager;
   import feathers.events.FeathersEventType;
   import feathers.layout.RelativePosition;
   import feathers.themes.FontColor;
   import feathers.utils.touch.TapToTrigger;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.MovieClip;
   import flash.errors.IllegalOperationError;
   import flash.events.*;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.net.SharedObject;
   import flash.net.Socket;
   import flash.net.URLLoader;
   import flash.net.URLRequest;
   import flash.net.URLStream;
   import flash.text.TextField;
   import flash.ui.Multitouch;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.setTimeout;
   import ru.pragmatix.flox.gui.controls.§_-N2r§;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.gui.controls.§_-yf§;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.*;
   import ru.pragmatix.flox.social.model.ResponseFormat;
   import ru.pragmatix.flox.social.model.SocialErrorCode;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.flox.social.response.photo.PhotoSaveResponse;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.request.edit.RenameClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.BuyRace;
   import ru.pragmatix.wormix.messages.client.BuyResetBonusItems;
   import ru.pragmatix.wormix.messages.client.EndTurn;
   import ru.pragmatix.wormix.messages.client.PumpReactionRate;
   import ru.pragmatix.wormix.messages.server.EndTurnResponse;
   import ru.pragmatix.wormix.messages.server.EnterAccount;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.rewards.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.FreezingMissile;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import ru.pragmatix.wormix.weapons.ammo.phase.ChainPhase;
   import ru.pragmatix.wormix.weapons.ammo.phase.FrostBolt;
   import ru.pragmatix.wormix.weapons.ammo.phase.HealBolt;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import ru.pragmatix.wormix.weapons.ammo.phase.PoisonPhase;
   import ru.pragmatix.wormix.weapons.ammo.type.RopeHook;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.extensions.ParticleAura;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   use namespace bi_internal;
   
   public class §_-g2i§ extends §_-73y§
   {
      
      protected static const §_-B23§:String = "cells";
      
      protected static const §_-719§:String = "select";
      
      protected var store:§_-N2r§;
      
      public var §_-jv§:Boolean = true;
      
      public var §_-41Y§:Boolean = false;
      
      public var §_-L1i§:Boolean = false;
      
      private var §_-41V§:Array = [];
      
      protected var §_-c2n§:Array = [];
      
      protected var cells:Vector.<§_-P1h§>;
      
      protected var cellsContainer:Sprite;
      
      protected var §_-A1t§:§_-l8§;
      
      protected var §_-WD§:§_-K2h§ = §_-K2h§.CLICK;
      
      public var §_-T1W§:Boolean = false;
      
      protected var §_-82Z§:starling.display.DisplayObject;
      
      private var §_-sB§:int = 1;
      
      protected var selectable:Boolean = true;
      
      protected var §_-S2K§:Boolean = false;
      
      public function §_-g2i§(param1:DisplayObjectContainer)
      {
         super(param1);
         this.§_-p2O§();
      }
      
      protected function §_-p2O§() : void
      {
         this.§_-235§();
         this.§_-sB§ = this.cells.length;
         this.store = new §_-N2r§([],this.§_-sB§,0);
      }
      
      protected function §_-235§() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:starling.display.DisplayObject = null;
         if(!this.cellsContainer)
         {
            this.cellsContainer = this.§_-mt§();
         }
         this.cells = new Vector.<§_-P1h§>();
         if(this.cellsContainer)
         {
            this.§_-82Z§ = this.cellsContainer.getChildByName(§_-719§);
            _loc1_ = this.cellsContainer.numChildren;
            _loc2_ = 0;
            _loc3_ = 0;
            while(_loc3_ < _loc1_)
            {
               _loc4_ = this.cellsContainer.getChildAt(_loc3_);
               if(_loc4_.name != §_-719§ && Boolean(this.§_-d1Q§(_loc4_,_loc2_)))
               {
                  _loc2_++;
               }
               _loc3_++;
            }
         }
         else
         {
            _loc4_ = null;
            _loc3_ = 0;
            do
            {
               _loc4_ = getChildByName("cell" + _loc3_);
               if(_loc4_)
               {
                  this.§_-d1Q§(_loc4_,_loc3_);
               }
               _loc3_++;
            }
            while(_loc4_);
         }
      }
      
      protected function §_-A3s§(param1:Boolean = false) : void
      {
         var _loc2_:Number = NaN;
         var _loc3_:int = 0;
         var _loc4_:DisplayObjectContainer = null;
         if(this.cells.length < this.store.§_-91s§().length)
         {
            _loc2_ = this.§_-s1n§(param1);
            _loc3_ = int(this.cells.length);
            while(_loc3_ < this.store.§_-91s§().length)
            {
               _loc4_ = this.§_-615§();
               this.setPosition(_loc4_,param1,_loc3_,_loc2_);
               this.cellsContainer.addChild(_loc4_);
               this.§_-d1Q§(_loc4_,_loc3_);
               _loc3_++;
            }
            if(this.§_-82Z§)
            {
               this.cellsContainer.setChildIndex(this.§_-82Z§,this.cellsContainer.numChildren - 1);
            }
         }
         else if(this.cells.length > this.store.§_-91s§().length)
         {
         }
      }
      
      private function §_-s1n§(param1:Boolean) : Number
      {
         if(this.cells.length > 1)
         {
            return param1 ? this.cells[1].y - this.cells[0].y : this.cells[1].x - this.cells[0].x;
         }
         return param1 ? this.cells[0].height : this.cells[0].width;
      }
      
      protected function setPosition(param1:DisplayObjectContainer, param2:Boolean, param3:int, param4:Number) : void
      {
         if(param2)
         {
            param1.x = this.cells[param3 - 1].x;
            param1.y = this.cells[param3 - 1].y + param4;
         }
         else
         {
            param1.y = this.cells[param3 - 1].y;
            param1.x = this.cells[param3 - 1].x + param4;
         }
      }
      
      protected function §_-615§() : DisplayObjectContainer
      {
         throw new IllegalOperationError();
      }
      
      protected function §_-d1Q§(param1:starling.display.DisplayObject, param2:uint) : §_-P1h§
      {
         var _loc3_:§_-P1h§ = this.§_-LP§(param1);
         this.§_-Fq§(_loc3_);
         _loc3_.§_-L3§(param2);
         this.cells[param2] = _loc3_;
         if(this.§_-41Y§)
         {
            _loc3_.§_-Pv§(true);
         }
         return _loc3_;
      }
      
      public function setData(param1:Array) : void
      {
         this.§_-c2n§ = [];
         this.store = new §_-N2r§(param1,this.§_-sB§);
         this.store.§_-b1S§(1);
      }
      
      public function §_-Os§(param1:*) : void
      {
         this.setData(§_-fM§.§_-n2u§(param1));
      }
      
      override protected function §_-Q2j§(param1:Boolean = true) : void
      {
         this.activate();
         if(!this.§_-L1i§)
         {
            super.§_-Q2j§(param1);
         }
      }
      
      override public function hide() : void
      {
         if(!this.§_-L1i§)
         {
            super.hide();
         }
         deactivate();
      }
      
      override public function activate() : void
      {
         super.activate();
         this.update();
      }
      
      public function update() : void
      {
         this.store.§_-kr§(this.§_-93i§);
      }
      
      protected function §_-93i§(param1:Array) : void
      {
         var _loc2_:§_-P1h§ = null;
         var _loc3_:Boolean = false;
         var _loc5_:§_-P1h§ = null;
         this.§_-83w§();
         var _loc4_:int = 0;
         while(_loc4_ < this.cells.length)
         {
            _loc5_ = §_-P1h§(this.cells[_loc4_]);
            this.§_-w2k§(_loc5_,_loc4_ < param1.length ? param1[_loc4_] : null,this.store.§_-aZ§() + _loc4_);
            if(!_loc2_ && this.§_-8s§(_loc5_))
            {
               _loc2_ = _loc5_;
            }
            if(§_-ts§.contains(this.§_-c2n§,param1[_loc4_]) && this.§_-8s§(_loc5_))
            {
               this.§_-J1D§(_loc5_);
               _loc3_ = true;
            }
            _loc4_++;
         }
         if(Boolean(_loc2_) && Boolean(this.§_-jv§) && !_loc3_)
         {
            this.§_-J1D§(_loc2_);
         }
      }
      
      protected function §_-w2k§(param1:§_-P1h§, param2:Object, param3:uint) : void
      {
         param1.§_-r2K§(param3);
         if(this.§_-T1W§ && !param2)
         {
            param1.§_-c1v§(false);
         }
         else
         {
            param1.§_-c1v§(true);
            param1.setItem(param2);
         }
      }
      
      protected function §_-mt§() : Sprite
      {
         return Sprite(getChildByName(§_-B23§));
      }
      
      protected function §_-Fq§(param1:§_-P1h§) : void
      {
         if(this.§_-A1t§)
         {
            param1.addEventListener(this.§_-A1t§.§_-X2T§,this.§_-nV§);
         }
         param1.addEventListener(this.§_-WD§.§_-X2T§,this.§_-FK§);
         param1.addEventListener(§_-j1f§.DISABLED,this.§_-81J§);
      }
      
      private function §_-nV§(param1:starling.events.Event) : void
      {
         var _loc2_:§_-P1h§ = param1.target as §_-P1h§;
         if(this.§_-8s§(_loc2_))
         {
            this.§_-J1D§(_loc2_,true);
            this.§_-b1H§();
         }
      }
      
      protected function §_-LP§(param1:starling.display.DisplayObject) : §_-P1h§
      {
         throw new IllegalOperationError("try to call abstract method");
      }
      
      override protected function §_-b1H§() : void
      {
         dispatchEvent(new starling.events.Event(§_-M4§.MENU_OK_ACTION));
         if(!this.§_-L1i§)
         {
            super.§_-b1H§();
         }
      }
      
      private function §_-B2B§(param1:§_-P1h§) : void
      {
         if(this.§_-82Z§)
         {
            if(param1)
            {
               this.§_-82Z§.visible = true;
               this.§_-82Z§.x = param1.x;
               this.§_-82Z§.y = param1.y;
            }
            else
            {
               this.§_-82Z§.visible = false;
            }
         }
      }
      
      protected function §_-8d§(param1:Boolean) : void
      {
         if(this.§_-82Z§)
         {
            this.§_-82Z§.visible = !param1 && this.§_-V2§() != -1;
         }
      }
      
      public function §_-p2w§() : Object
      {
         if(this.§_-c2n§.length)
         {
            return this.§_-c2n§[0];
         }
         return null;
      }
      
      public function §_-5Z§() : Array
      {
         return this.§_-c2n§;
      }
      
      protected function §_-FK§(param1:starling.events.Event) : void
      {
         var _loc2_:§_-P1h§ = §_-P1h§(param1.target);
         if(this.§_-8s§(_loc2_))
         {
            this.§_-J1D§(_loc2_,!_loc2_.isSelected());
         }
      }
      
      protected function §_-81J§(param1:starling.events.Event) : void
      {
         var _loc2_:§_-P1h§ = §_-P1h§(param1.target);
         if(_loc2_.§_-i2B§())
         {
            this.§_-J1D§(_loc2_,false);
         }
      }
      
      protected function §_-83w§() : void
      {
         var _loc1_:§_-P1h§ = null;
         if(this.§_-41V§ != null && Boolean(this.§_-41V§.length))
         {
            for each(_loc1_ in this.§_-41V§)
            {
               _loc1_.setSelected(false);
            }
         }
         this.§_-41V§ = [];
      }
      
      protected function §_-J1D§(param1:§_-P1h§, param2:Boolean = true) : void
      {
         this.§_-OT§(param1,param2);
         this.§_-R1g§(param1,param2);
         if(param2)
         {
            this.§_-B2B§(param1);
         }
         dispatchEvent(new starling.events.Event(§_-M4§.MENU_ITEM_SELECT));
         this.§_-s2h§(param1,param2);
      }
      
      protected function §_-OT§(param1:§_-P1h§, param2:Boolean = true) : void
      {
      }
      
      protected function §_-s2h§(param1:§_-P1h§, param2:Boolean = true) : void
      {
      }
      
      private function §_-R1g§(param1:§_-P1h§, param2:Boolean = true) : void
      {
         if(this.§_-41Y§)
         {
            if(param2 && !param1.isSelected())
            {
               if(param1.getItem() == null)
               {
                  throw new IllegalOperationError();
               }
               this.§_-c2n§.push(param1.getItem());
               this.§_-41V§.push(param1);
               param1.setSelected(true);
            }
            else if(!param2 && param1.§_-V2E§())
            {
               §_-ts§.remove(this.§_-c2n§,param1.getItem());
               §_-ts§.remove(this.§_-41V§,param1);
               param1.setSelected(false);
            }
         }
         else if(param2)
         {
            if(Boolean(this.§_-41V§.length) && param1 != this.§_-41V§[0])
            {
               §_-P1h§(this.§_-41V§[0]).setSelected(false);
            }
            this.§_-41V§ = [param1];
            this.§_-c2n§ = [param1.getItem()];
            if(!param1.isSelected())
            {
               param1.setSelected(true);
            }
         }
         else if(param1.§_-V2E§())
         {
            param1.setSelected(false);
            this.§_-c2n§ = [];
            this.§_-41V§ = [];
         }
      }
      
      protected function §_-01F§() : void
      {
         var _loc1_:§_-P1h§ = null;
         for each(_loc1_ in this.cells)
         {
            this.§_-G8§(_loc1_);
         }
         this.cells = null;
      }
      
      protected function §_-G8§(param1:§_-P1h§) : void
      {
         if(!param1)
         {
            return;
         }
         param1.removeEventListener(this.§_-WD§.§_-X2T§,this.§_-FK§);
         param1.removeEventListener(§_-j1f§.DISABLED,this.§_-81J§);
         if(this.§_-A1t§)
         {
            param1.removeEventListener(this.§_-A1t§.§_-X2T§,this.§_-nV§);
         }
         param1.dispose();
      }
      
      override public function §_-H3b§(param1:Boolean) : void
      {
         super.§_-H3b§(param1);
         this.§_-mt§().visible = !param1;
      }
      
      protected function §_-8s§(param1:§_-P1h§) : Boolean
      {
         return param1.isSelectable() && Boolean(param1.getItem());
      }
      
      protected function §_-gl§(param1:Array) : void
      {
         var _loc2_:Object = null;
         for each(_loc2_ in param1)
         {
            this.§_-s2K§(_loc2_);
         }
      }
      
      protected function §_-s2K§(param1:Object) : void
      {
         var _loc2_:§_-P1h§ = this.§_-723§(param1);
         if(_loc2_)
         {
            this.§_-J1D§(_loc2_);
         }
      }
      
      public function §_-723§(param1:Object) : §_-P1h§
      {
         var _loc2_:§_-P1h§ = null;
         if(param1 == null)
         {
            return null;
         }
         for each(_loc2_ in this.cells)
         {
            if(_loc2_.getItem() == param1)
            {
               return _loc2_;
            }
         }
         return null;
      }
      
      public function §_-V2§() : int
      {
         if(!this.§_-41V§.length)
         {
            return -1;
         }
         var _loc1_:uint = uint(this.§_-41V§[0].getCellIndex());
         return (this.store.§_-zY§() - 1) * this.store.§_-AM§() + _loc1_;
      }
      
      public function §_-m29§(param1:int) : void
      {
         if(param1 >= this.store.§_-a2k§())
         {
            param1 = 0;
         }
         else if(param1 < 0)
         {
            param1 = this.store.§_-a2k§() - 1;
         }
         this.§_-J1D§(this.cells[param1]);
      }
      
      public function §_-W2t§(param1:Object) : void
      {
         this.§_-c2n§ = [param1];
      }
      
      public function §_-v12§(param1:Boolean) : void
      {
         var _loc2_:§_-P1h§ = null;
         this.selectable = param1;
         for each(_loc2_ in this.cells)
         {
            _loc2_.§_-v12§(param1);
         }
      }
      
      protected function §_-AM§() : uint
      {
         return this.§_-sB§;
      }
      
      override public function dispose() : void
      {
         this.§_-41V§ = null;
         this.§_-c2n§ = null;
         if(this.cellsContainer)
         {
            this.cellsContainer.dispose();
         }
         this.cellsContainer = null;
         if(this.§_-82Z§)
         {
            this.§_-82Z§.dispose();
         }
         this.§_-82Z§ = null;
         this.§_-01F§();
         super.dispose();
      }
   }
}

