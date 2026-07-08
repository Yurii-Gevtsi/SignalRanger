package ru.pragmatix.wormix.gui
{
   import §_-12W§.*;
   import §_-5P§.§_-H1u§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-zF§;
   import §_-73I§.§_-K2s§;
   import §_-931§.§_-i2r§;
   import §_-B1y§.*;
   import §_-B2z§.Server;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-82u§;
   import §_-B2z§.§_-c1G§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-fH§;
   import §_-G2H§.*;
   import §_-H2g§.§_-di§;
   import §_-H3§.*;
   import §_-K35§.§_-L1O§;
   import §_-K35§.§_-p1m§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.§_-V1m§;
   import §_-T2h§.§_-92J§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1Q§.§_-61Z§;
   import §_-Y1b§.*;
   import §_-YF§.§_-L29§;
   import §_-YF§.§_-Zi§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-d26§.§_-d1H§;
   import §_-fo§.*;
   import §_-j22§.§_-5g§;
   import §_-jF§.§_-U2u§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-l1g§.§_-f2L§;
   import §_-l2r§.§_-K1t§;
   import §_-o23§.§_-z2X§;
   import §_-p18§.§_-vd§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-6a§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-sK§.*;
   import §_-u2J§.§_-A0§;
   import §_-w2W§.§_-21w§;
   import §_-xu§.§_-91H§;
   import §_-zI§.§_-8D§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import com.hurlant.util.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.ToggleButton;
   import feathers.data.ArrayCollection;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import flash.events.IEventDispatcher;
   import flash.events.MouseEvent;
   import flash.geom.Point;
   import flash.net.SharedObject;
   import flash.system.LoaderContext;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.clearTimeout;
   import flash.utils.getQualifiedClassName;
   import flash.utils.getTimer;
   import org.gestouch.core.Touch;
   import ru.pragmatix.flox.gui.controls.*;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.controller.mobile.VkMobileSocialController;
   import ru.pragmatix.flox.social.response.LoadFriendsResponse;
   import ru.pragmatix.flox.social.utils.id.GetAuthKeyEvent;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.§_-738§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.Connect;
   import ru.pragmatix.wormix.messages.client.ReorderGroup;
   import ru.pragmatix.wormix.messages.server.BuyVipResult;
   import ru.pragmatix.wormix.messages.server.ConnectResult;
   import ru.pragmatix.wormix.messages.structures.*;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.*;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.utils.Align;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-P2D§ extends §_-73y§
   {
      
      private static const §_-1e§:String = "next";
      
      private static const §_-01S§:String = "prev";
      
      private static const §_-l1i§:String = "userProfile";
      
      private static const §_-B3t§:String = "worm";
      
      private static const §_-K3c§:int = 450;
      
      private static const §_-n1p§:int = 337;
      
      private static const §_-F28§:int = 15;
      
      private static const §_-G5§:int = 7;
      
      private static const §_-92k§:int = 90;
      
      protected var §_-f1T§:Button;
      
      protected var §_-h1§:Button;
      
      protected var §_-H2J§:List;
      
      protected var §_-53Y§:ListCollection;
      
      protected var §_-NC§:Sprite;
      
      protected var §_-W1D§:Sprite;
      
      protected var §_-t2p§:§_-p1m§;
      
      protected var §_-K11§:§_-91H§;
      
      private var §_-Q2n§:Boolean = false;
      
      public function §_-P2D§(param1:DisplayObjectContainer, param2:Boolean = true)
      {
         super(param1);
         destroyOnRemove = true;
         this.§_-f1T§ = getChildByName(§_-1e§) as Button;
         this.§_-h1§ = getChildByName(§_-01S§) as Button;
         this.§_-NC§ = getChildByName(§_-l1i§) as Sprite;
         this.§_-W1D§ = getChildByName(§_-B3t§) as Sprite;
         this.§_-t2p§ = new §_-p1m§(this.§_-W1D§);
         var _loc3_:TiledRowsLayout = new TiledRowsLayout();
         _loc3_.gap = §_-F28§;
         _loc3_.paging = Direction.VERTICAL;
         _loc3_.horizontalAlign = HorizontalAlign.LEFT;
         this.§_-H2J§ = new List();
         this.§_-H2J§.addEventListener(FeathersEventType.INITIALIZE,this.§_-fQ§);
         this.§_-W1v§();
         this.§_-H2J§.layout = _loc3_;
         this.§_-H2J§.itemRendererType = §_-U2u§;
         this.§_-H2J§.snapToPages = true;
         this.§_-H2J§.horizontalScrollPolicy = ScrollPolicy.OFF;
         this.§_-H2J§.verticalScrollPolicy = ScrollPolicy.OFF;
         this.§_-H2J§.allowMultipleSelection = false;
         this.§_-53Y§ = new ListCollection();
         this.§_-H2J§.dataProvider = this.§_-53Y§;
         §_-72v§(this.§_-H2J§);
         this.§_-H2J§.addEventListener(starling.events.Event.CHANGE,this.§_-B1n§);
         this.§_-H2J§.addEventListener(starling.events.Event.TRIGGERED,this.§_-W1K§);
         this.§_-f1T§.addEventListener(starling.events.Event.TRIGGERED,this.§_-E3y§);
         this.§_-h1§.addEventListener(starling.events.Event.TRIGGERED,this.§_-f1n§);
         this.§_-K11§ = this.§_-c1d§();
         this.§_-Q2n§ = param2;
      }
      
      protected function §_-c1d§() : §_-91H§
      {
         return new §_-91H§(12);
      }
      
      protected function §_-W1v§() : void
      {
         this.§_-H2J§.x = §_-G5§;
         this.§_-H2J§.y = §_-92k§;
         this.§_-H2J§.width = this.§_-H2J§.pageWidth = §_-K3c§;
         this.§_-H2J§.height = this.§_-H2J§.pageHeight = §_-n1p§;
      }
      
      protected function §_-f1n§(param1:starling.events.Event) : void
      {
         var _loc2_:int = Math.max(this.§_-K11§.§_-zY§() - 1,1);
         this.§_-V1c§(_loc2_);
      }
      
      protected function §_-E3y§(param1:starling.events.Event) : void
      {
         var _loc2_:int = Math.min(this.§_-K11§.§_-zY§() + 1,this.§_-K11§.§_-a2k§());
         this.§_-V1c§(_loc2_);
      }
      
      protected function §_-B1n§(param1:starling.events.Event = null) : void
      {
         var _loc2_:UserProfile = this.§_-H2J§.selectedItem as UserProfile;
         if(_loc2_)
         {
            this.§_-o1W§(_loc2_);
         }
      }
      
      protected function §_-W1K§(param1:starling.events.Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-jn§);
      }
      
      protected function §_-o1W§(param1:UserProfile) : void
      {
         if(param1)
         {
            §_-L1O§.§_-z1Z§(this.§_-NC§,param1);
            this.§_-t2p§.setIcon(param1);
         }
         this.§_-NC§.visible = param1 != null;
         this.§_-W1D§.visible = param1 != null;
      }
      
      protected function §_-V1c§(param1:int) : void
      {
         if(this.§_-K11§.§_-a2k§() < param1)
         {
            return;
         }
         this.§_-f1T§.isEnabled = this.§_-h1§.isEnabled = false;
         this.§_-H3b§(true);
         this.§_-K11§.§_-b1S§(param1);
         this.§_-K11§.§_-kr§(this.§_-73W§);
      }
      
      public function §_-73W§(param1:Array) : void
      {
         this.§_-H3b§(false);
         if(!param1 || !this.§_-H2J§ || !this.§_-H2J§.isInitialized)
         {
            return;
         }
         this.§_-L1h§(param1);
         this.§_-bx§(this.§_-K11§.§_-zY§() - 1,this.§_-K11§.§_-01o§());
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      private function §_-L1h§(param1:Array = null) : void
      {
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc2_:int = this.§_-H2J§.selectedIndex;
         var _loc3_:int = this.§_-e2Z§(this.§_-H2J§.selectedItem);
         if(param1)
         {
            if(param1.length != this.§_-53Y§.length)
            {
               this.§_-53Y§.removeAll();
            }
            _loc4_ = int(param1.length);
            _loc5_ = 0;
            while(_loc5_ < _loc4_)
            {
               if(this.§_-t26§(param1[_loc5_]))
               {
                  this.§_-53Y§.setItemAt(param1[_loc5_],_loc5_);
               }
               _loc5_++;
            }
            if(this.§_-53Y§.length)
            {
               if(_loc2_ < 0 || _loc3_ != this.§_-e2Z§(this.§_-H2J§.dataProvider.getItemAt(_loc2_)))
               {
                  this.§_-H2J§.selectedIndex = 0;
               }
               else
               {
                  this.§_-H2J§.selectedIndex = _loc2_;
               }
            }
         }
         else
         {
            this.§_-53Y§.removeAll();
         }
         this.§_-H2J§.validate();
      }
      
      protected function §_-e2Z§(param1:Object) : int
      {
         return this.§_-t26§(param1) ? (param1 as UserProfile).getId() : -1;
      }
      
      protected function §_-t26§(param1:Object) : Boolean
      {
         return Boolean(param1) && param1 is UserProfile;
      }
      
      protected function §_-bx§(param1:int = -1, param2:Number = NaN) : void
      {
         param1 = param1 == -1 ? int(Math.max(this.§_-H2J§.verticalPageIndex,this.§_-H2J§.horizontalPageIndex)) : param1;
         param2 = isNaN(param2) ? Math.max(this.§_-H2J§.verticalPageCount,this.§_-H2J§.horizontalPageCount) : param2;
         this.§_-f1T§.isEnabled = param1 != param2 - 1 && param2 > 0;
         this.§_-h1§.isEnabled = param1 != 0 && param2 > 0;
      }
      
      override public function §_-H3b§(param1:Boolean) : void
      {
         super.§_-H3b§(param1);
         if(this.§_-H2J§)
         {
            this.§_-H2J§.visible = !param1;
         }
      }
      
      private function §_-83m§() : void
      {
         if(this.§_-t2p§)
         {
            this.§_-t2p§.dispose();
            this.§_-t2p§ = null;
         }
      }
      
      override public function dispose() : void
      {
         this.§_-H2J§.removeEventListener(starling.events.Event.CHANGE,this.§_-B1n§);
         this.§_-H2J§.removeEventListener(starling.events.Event.TRIGGERED,this.§_-W1K§);
         this.§_-f1T§.removeEventListener(starling.events.Event.TRIGGERED,this.§_-E3y§);
         this.§_-h1§.removeEventListener(starling.events.Event.TRIGGERED,this.§_-f1n§);
         §_-k6§(this.§_-H2J§);
         this.§_-H2J§.dispose();
         this.§_-H2J§ = null;
         this.§_-53Y§ = null;
         this.§_-W1D§ = null;
         this.§_-83m§();
         super.dispose();
      }
      
      protected function §_-fQ§(param1:starling.events.Event) : void
      {
         this.§_-H2J§.removeEventListener(FeathersEventType.INITIALIZE,this.§_-fQ§);
         if(this.§_-Q2n§)
         {
            this.§_-V1c§(1);
         }
      }
   }
}

