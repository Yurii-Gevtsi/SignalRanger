package ru.pragmatix.wormix.gui
{
   import §_-12W§.§_-J3J§;
   import §_-12W§.§_-X2w§;
   import §_-12W§.§_-l23§;
   import §_-21A§.*;
   import §_-21p§.§_-4w§;
   import §_-23P§.§_-V2T§;
   import §_-23P§.§_-u1z§;
   import §_-43V§.Random;
   import §_-51p§.§_-W2e§;
   import §_-5Y§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-73d§.§_-x2C§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J1d§;
   import §_-G16§.*;
   import §_-G2H§.*;
   import §_-I1l§.*;
   import §_-J31§.§_-R1h§;
   import §_-Ly§.§_-81L§;
   import §_-Ly§.§_-I2K§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-OJ§.§_-32g§;
   import §_-P2i§.§_-G3J§;
   import §_-P2i§.§_-b7§;
   import §_-RE§.§_-52p§;
   import §_-T2h§.*;
   import §_-Tm§.§_-RF§;
   import §_-Ty§.§_-83N§;
   import §_-Ty§.§_-K3a§;
   import §_-Ty§.§_-U2T§;
   import §_-Ty§.§_-n1U§;
   import §_-Ty§.§_-o1U§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1b§.§_-OP§;
   import §_-Y23§.*;
   import §_-YF§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-hO§.SupplyPickedUpEvent;
   import §_-j1w§.§_-L1x§;
   import §_-jF§.*;
   import §_-k2s§.§_-Go§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-e1k§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-nZ§.*;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-v2t§.§_-43O§;
   import §_-w2i§.§_-Ia§;
   import §_-x22§.§_-xY§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import com.hurlant.util.der.*;
   import com.worlize.websocket.§_-03n§;
   import com.worlize.websocket.§_-Y2T§;
   import com.worlize.websocket.§_-ue§;
   import config.§_-V2w§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.List;
   import feathers.controls.TextInput;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.events.FeathersEventType;
   import flash.desktop.NativeApplication;
   import flash.display.MovieClip;
   import flash.display.SimpleButton;
   import flash.events.MouseEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.media.SoundMixer;
   import flash.net.Socket;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.EndTurn;
   import ru.pragmatix.wormix.messages.server.AddToGroupResult;
   import ru.pragmatix.wormix.messages.server.EndTurnResponse;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.extensions.ParticleAura;
   import starling.text.TextField;
   import starling.text.TextFormat;
   import starling.textures.Texture;
   import starling.utils.Align;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   
   public class §_-fT§ extends EventDispatcher
   {
      
      protected static const §_-41Z§:§_-L1B§ = new §_-L1B§();
      
      protected var isInitialized:Boolean;
      
      public var destroyOnRemove:Boolean = false;
      
      protected var container:DisplayObject;
      
      public var disabled:Boolean;
      
      protected var §_-F11§:§_-t1R§;
      
      private var _id:String;
      
      private var §_-zd§:§_-32g§;
      
      private var §_-h2b§:Boolean = §_-81L§.§_-sq§;
      
      protected var §_-w1L§:Boolean;
      
      public function §_-fT§(param1:DisplayObject)
      {
         super();
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
         this.container = param1;
         this.§_-Ii§(Event.ADDED_TO_STAGE,this.onAddedToStage);
         this.§_-Ii§(Event.REMOVED_FROM_STAGE,this.onRemovedFromStage);
         this.§_-Ii§(TouchEvent.TOUCH,this.§_-01s§);
      }
      
      public function dispose() : void
      {
         if(this.container)
         {
            this.§_-PP§(Event.ADDED_TO_STAGE,this.onAddedToStage);
            this.§_-PP§(Event.REMOVED_FROM_STAGE,this.onRemovedFromStage);
            this.§_-PP§(TouchEvent.TOUCH,this.§_-01s§);
         }
         this.isInitialized = false;
         if(this.container)
         {
            this.container.dispose();
         }
         this.container = null;
         this.§_-F11§ = null;
         this.§_-zd§ = null;
      }
      
      private function §_-01s§(param1:TouchEvent) : void
      {
         var _loc4_:Rectangle = null;
         var _loc2_:String = this.§_-w1L§ ? TouchPhase.BEGAN : TouchPhase.ENDED;
         var _loc3_:Touch = param1.getTouch(this.getContainer(),_loc2_);
         if(_loc3_)
         {
            _loc4_ = this.getContainer().getBounds(this.getContainer().stage);
            if(_loc4_.contains(_loc3_.globalX,_loc3_.globalY))
            {
               this.§_-Y1W§(param1);
            }
            Pool.putRectangle(_loc4_);
         }
      }
      
      protected function §_-Y1W§(param1:Event) : void
      {
         this.§_-l1S§();
      }
      
      public function getContainer() : DisplayObject
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
         if(this.container)
         {
            this.container.visible = param1;
         }
      }
      
      protected function onAddedToStage(param1:Event) : void
      {
         if(!this.isInitialized)
         {
            this.§_-q2a§();
            this.isInitialized = true;
         }
      }
      
      protected function onRemovedFromStage(param1:Event) : void
      {
         if(this.destroyOnRemove)
         {
            SystemUtil.executeWhenApplicationIsActive(this.dispose);
         }
      }
      
      protected function §_-l1S§() : void
      {
         if(this.§_-F11§)
         {
            §_-h2B§.play(this.§_-F11§);
         }
      }
      
      protected function §_-q2a§() : void
      {
      }
      
      protected function §_-O2y§() : void
      {
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
      
      public function §_-l1l§(param1:DisplayObjectContainer) : void
      {
         param1.addChild(this.container);
      }
      
      public function §_-f9§(param1:DisplayObjectContainer) : void
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
         this.container.removeEventListener(param1,param2);
      }
      
      protected function §_-Ii§(param1:String, param2:Function) : void
      {
         this.container.addEventListener(param1,param2);
      }
      
      public function §_-h1C§() : Boolean
      {
         return Boolean(this.container) && Boolean(this.container.stage) && this.isVisible();
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
      
      public function §_-G1l§() : DisplayObject
      {
         return this.container;
      }
   }
}

