package ru.pragmatix.wormix.gui
{
   import §_-12W§.*;
   import §_-23P§.§_-u1z§;
   import §_-2U§.*;
   import §_-31W§.§_-r26§;
   import §_-52L§.§_-u2x§;
   import §_-533§.§_-c2b§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-zF§;
   import §_-82l§.§_-A2Y§;
   import §_-9§.§_-52A§;
   import §_-91A§.*;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-Af§.*;
   import §_-B1y§.Cookie;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-c1G§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.§_-i1i§;
   import §_-G16§.*;
   import §_-G2H§.*;
   import §_-H3§.*;
   import §_-J31§.§_-e2J§;
   import §_-OA§.§_-k2K§;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-Ta§.*;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-tc§;
   import §_-Y1b§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-a19§.*;
   import §_-e2r§.*;
   import §_-fo§.§_-r5§;
   import §_-fo§.§_-v2P§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.*;
   import §_-l1g§.§_-Rs§;
   import §_-l2r§.§_-K1t§;
   import §_-lh§.§_-z2A§;
   import §_-m1g§.§_-K2W§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-737§;
   import §_-sQ§.§_-H3D§;
   import §_-sQ§.§_-V1n§;
   import §_-w2W§.§_-21w§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import §_-z1g§.§_-5c§;
   import §_-z1g§.§_-82X§;
   import §_-z1g§.§_-y1S§;
   import §_-zI§.§_-sI§;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.greensock.*;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.plugins.*;
   import com.hurlant.util.der.§_-UO§;
   import config.§_-V2w§;
   import dragonBones.animation.WorldClock;
   import dragonBones.objects.DragonBonesData;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.ITextRenderer;
   import feathers.layout.AnchorLayout;
   import feathers.layout.ILayoutData;
   import feathers.layout.ILayoutDisplayObject;
   import flash.display.MovieClip;
   import flash.events.Event;
   import flash.events.TimerEvent;
   import flash.filters.BlurFilter;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.system.System;
   import flash.text.TextFormat;
   import flash.utils.Dictionary;
   import org.gestouch.core.GestureState;
   import ru.pragmatix.flox.social.controller.ISocialPaymentController;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-B1m§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.EndTurn;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.DynamiteStick;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-qY§ extends LayoutGroup
   {
      
      private const §_-2G§:String = "C_";
      
      private var container:§_-E2w§;
      
      private var §_-zf§:Boolean = false;
      
      private var §_-V29§:Dictionary = new Dictionary();
      
      private var §_-ZD§:Dictionary = new Dictionary();
      
      public function §_-qY§(param1:§_-E2w§, param2:Number, param3:Number)
      {
         super();
         this.container = param1;
         width = param2;
         height = param3;
         layout = new AnchorLayout();
         this.§_-I2w§();
         this.§_-7R§();
         this.§_-y2P§();
         this.§_-32e§();
      }
      
      public static function §_-9H§(param1:LayoutGroup, param2:DisplayObject, param3:int = -1, param4:ILayoutData = null, param5:Number = 0, param6:Number = 0) : DisplayObject
      {
         var _loc7_:ILayoutDisplayObject = param2 as ILayoutDisplayObject;
         if(!_loc7_)
         {
            _loc7_ = new LayoutGroup();
            param2.x = param5;
            param2.y = param6;
            LayoutGroup(_loc7_).addChild(param2);
         }
         else
         {
            _loc7_.x = param5;
            _loc7_.y = param6;
         }
         if(param4)
         {
            _loc7_.layoutData = param4;
         }
         if(param3 < 0)
         {
            param1.addChild(DisplayObject(_loc7_));
         }
         else
         {
            param1.addChildAt(DisplayObject(_loc7_),param3);
         }
         return DisplayObject(_loc7_);
      }
      
      protected function §_-I2w§() : void
      {
      }
      
      protected function §_-7R§() : void
      {
      }
      
      protected function §_-y2P§() : void
      {
      }
      
      protected function §_-I3V§(param1:String) : DisplayObject
      {
         return this.container.getChildByName(param1);
      }
      
      public function §_-r1t§(param1:DisplayObject, param2:ILayoutData = null) : void
      {
         §_-9H§(this,param1,-1,param2);
      }
      
      public function §_-uY§(param1:DisplayObject, param2:int, param3:ILayoutData = null) : void
      {
         §_-9H§(this,param1,param2,param3);
      }
      
      public function §_-I1m§(param1:DisplayObject, param2:Number) : void
      {
         param1.scaleX = param1.scaleY = param2;
      }
      
      public function §_-11l§() : void
      {
         if(this.§_-zf§)
         {
            return;
         }
         this.§_-w7§();
         this.§_-zf§ = true;
      }
      
      protected function §_-w7§() : void
      {
         var _loc3_:ILayoutDisplayObject = null;
         var _loc1_:int = 0;
         var _loc2_:int = numChildren;
         while(_loc1_ < _loc2_)
         {
            _loc3_ = getChildAt(_loc1_) as ILayoutDisplayObject;
            if(_loc3_)
            {
               _loc3_.layoutData = null;
            }
            _loc1_++;
         }
      }
      
      public function §_-91z§() : void
      {
         if(!this.§_-zf§)
         {
            return;
         }
         this.§_-vj§();
         validateChildren();
         validate();
         this.§_-zf§ = false;
      }
      
      protected function §_-vj§() : void
      {
         this.§_-y2P§();
      }
      
      protected function §_-92r§(param1:DisplayObject, param2:Boolean, param3:String = null) : void
      {
         if(param2)
         {
            this.§_-V29§[param1] = param3;
         }
         else if(this.§_-V29§[param1])
         {
            delete this.§_-V29§[param1];
         }
      }
      
      public function §_-D1P§() : void
      {
         var _loc1_:Object = null;
         for(_loc1_ in this.§_-V29§)
         {
            this.§_-K3T§(_loc1_,this.§_-V29§[_loc1_]);
         }
      }
      
      public function §_-E2e§() : void
      {
         var _loc1_:Object = null;
         for(_loc1_ in this.§_-V29§)
         {
            Cookie.setValue(this.§_-2G§ + this.§_-V29§[_loc1_],undefined);
         }
      }
      
      protected function §_-K3T§(param1:Object, param2:String) : void
      {
         var _loc3_:Object = null;
         if(!this.§_-F2V§(param1))
         {
            _loc3_ = {
               "x":(param1.x + param1.width * 0.5) / Starling.current.stage.stageWidth,
               "y":(param1.y + param1.height * 0.5) / Starling.current.stage.stageHeight
            };
            Cookie.setValue(this.§_-2G§ + param2,_loc3_);
         }
      }
      
      public function §_-EW§() : void
      {
         var _loc1_:Object = null;
         var _loc2_:String = null;
         var _loc3_:Object = null;
         var _loc4_:Point = null;
         for(_loc1_ in this.§_-V29§)
         {
            _loc2_ = this.§_-V29§[_loc1_];
            _loc3_ = Cookie.getValue(this.§_-2G§ + _loc2_,null);
            if(_loc3_)
            {
               if(!this.§_-zf§)
               {
                  this.§_-11l§();
               }
               _loc4_ = Pool.getPoint(_loc3_.x * Starling.current.stage.stageWidth - _loc1_.width * 0.5,_loc3_.y * Starling.current.stage.stageHeight - _loc1_.height * 0.5);
               this.§_-U2r§(DisplayObject(_loc1_),_loc4_);
               Pool.putPoint(_loc4_);
            }
         }
         if(this.§_-Q2V§())
         {
            this.§_-91z§();
         }
      }
      
      protected function §_-U2r§(param1:DisplayObject, param2:Point) : void
      {
         param1.x = Math.max(0,Math.min(width - param1.width,param2.x));
         param1.y = Math.max(0,Math.min(height - param1.height,param2.y));
      }
      
      public function §_-Q1a§(param1:Number) : void
      {
      }
      
      public function §_-32e§() : void
      {
         var _loc1_:Object = null;
         validateChildren();
         validate();
         for(_loc1_ in this.§_-V29§)
         {
            this.§_-ZD§[_loc1_] = {
               "x":_loc1_.x,
               "y":_loc1_.y
            };
         }
      }
      
      protected function §_-Q2V§() : Boolean
      {
         var _loc2_:Object = null;
         var _loc1_:Boolean = true;
         for(_loc2_ in this.§_-V29§)
         {
            _loc1_ &&= this.§_-F2V§(_loc2_);
            if(!_loc1_)
            {
               break;
            }
         }
         return _loc1_;
      }
      
      private function §_-F2V§(param1:Object) : Boolean
      {
         return Math.abs(param1.x - this.§_-ZD§[param1].x) < 1 && Math.abs(param1.y - this.§_-ZD§[param1].y) < 1;
      }
      
      override public function dispose() : void
      {
         this.§_-ZD§ = null;
         this.§_-V29§ = null;
         this.container = null;
         super.dispose();
      }
   }
}

