package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-21A§.ChooseFriendForBossMenu;
   import §_-31N§.§_-42G§;
   import §_-31W§.§_-Kb§;
   import §_-31Y§.§_-D3B§;
   import §_-51M§.Cat;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-63U§.*;
   import §_-73I§.§_-q15§;
   import §_-82a§.SwitchedRenderTextures;
   import §_-82a§.§_-yb§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.*;
   import §_-B2z§.Server;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-c1G§;
   import §_-C3f§.§_-1Y§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-F39§.*;
   import §_-GQ§.§_-31v§;
   import §_-H2g§.§_-di§;
   import §_-H3§.*;
   import §_-L1H§.§_-C3e§;
   import §_-N8§.*;
   import §_-TF§.§_-Z1U§;
   import §_-TF§.§_-q1j§;
   import §_-TF§.§_-y2n§;
   import §_-Ty§.*;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1b§.§_-s2Z§;
   import §_-YF§.§_-q2v§;
   import §_-fo§.*;
   import §_-g2e§.§_-q2Z§;
   import §_-g2r§.§_-03A§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-m1g§.§_-K2W§;
   import §_-m2s§.*;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import §_-z1g§.§_-82X§;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-M2v§;
   import §_-z1g§.§_-lp§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import com.worlize.websocket.§_-03n§;
   import com.worlize.websocket.§_-Y2T§;
   import com.worlize.websocket.§_-ue§;
   import feathers.controls.Button;
   import feathers.controls.Scroller;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.layout.Direction;
   import feathers.layout.RelativePosition;
   import flash.display.BitmapData;
   import flash.events.*;
   import flash.filters.ColorMatrixFilter;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getDefinitionByName;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.response.photo.PhotoSaveResponse;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-12u§;
   import ru.pragmatix.wormix.game.ai.§_-H3h§;
   import ru.pragmatix.wormix.game.ai.§_-dG§;
   import ru.pragmatix.wormix.game.ai.§_-n2q§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.server.ConnectResult;
   import ru.pragmatix.wormix.messages.server.FastBattleResult;
   import ru.pragmatix.wormix.messages.server.IncomingPayment;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import starling.animation.Transitions;
   import starling.animation.Tween;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   
   public class §_-nx§ extends DefaultListItemRenderer implements §_-Z1U§
   {
      
      public static const §_-K2K§:Number = 254;
      
      public static const §_-r1a§:Number = 215;
      
      public static const §_-MG§:Number = 15;
      
      public static const §_-72A§:Number = 255 / 215;
      
      public static const §_-U1I§:Number = 0.9;
      
      private static var §_-p28§:int = 0;
      
      private const §_-G3S§:Number = 90;
      
      private var container:Sprite;
      
      private var §_-L28§:§_-G1F§;
      
      private var iconContainer:DisplayObjectContainer;
      
      private var iconSource:§_-di§;
      
      private var icon:DisplayObject;
      
      private var title:ITextRenderer;
      
      private var fuzyPrice:ITextRenderer;
      
      private var rubyPrice:ITextRenderer;
      
      private var background:§_-yb§;
      
      private var priceBackground:§_-yb§;
      
      private var rubyIcon:§_-yb§;
      
      private var fuzyIcon:§_-yb§;
      
      private var purchased:DisplayObject;
      
      private var hasImprovements:DisplayObject;
      
      private var lock:DisplayObject;
      
      private var lock_text:ITextRenderer;
      
      private var selectionFrame:Image;
      
      private var §_-J2K§:Object = new Object();
      
      public function §_-nx§()
      {
         super();
         ++§_-p28§;
         this.container = §_-YQ§.§_-qM§(§_-q2v§.§_-1m§,§_-q2v§.§_-A2T§);
         addChild(this.container);
         var _loc1_:DisplayObject = this.container.getChildByName("background") as DisplayObject;
         var _loc2_:DisplayObject = this.container.getChildByName("priceBackground") as DisplayObject;
         this.iconContainer = this.container.getChildByName("icon") as DisplayObjectContainer;
         this.title = this.container.getChildByName("title") as ITextRenderer;
         this.fuzyPrice = this.container.getChildByName("fuzyPrice") as ITextRenderer;
         this.rubyPrice = this.container.getChildByName("rubyPrice") as ITextRenderer;
         var _loc3_:DisplayObject = this.container.getChildByName("fuzyIcon") as DisplayObject;
         var _loc4_:DisplayObject = this.container.getChildByName("rubyIcon") as DisplayObject;
         this.purchased = this.container.getChildByName("purchased") as DisplayObject;
         this.hasImprovements = this.container.getChildByName("hasImprovements") as DisplayObject;
         this.lock = this.container.getChildByName("lock");
         this.lock_text = this.container.getChildByName("lock_text") as ITextRenderer;
         this.selectionFrame = this.container.getChildByName("selectionFrame") as Image;
         this.fuzyIcon = this.§_-P1x§(_loc3_,"fuzyIcon");
         this.rubyIcon = this.§_-P1x§(_loc4_,"rubyIcon");
         this.background = this.§_-P1x§(_loc1_,"background");
         this.background.touchable = true;
         this.priceBackground = this.§_-P1x§(_loc2_,"priceBackground");
         this.priceBackground.touchable = true;
         if(SystemUtil.isDesktop)
         {
            §_-q1j§.register(this);
         }
         width = this.container.width;
         height = this.container.height;
      }
      
      private function §_-P1x§(param1:DisplayObject, param2:String) : §_-yb§
      {
         SwitchedRenderTextures.§_-X2m§(param1,param2);
         var _loc3_:§_-yb§ = SwitchedRenderTextures.§_-O1u§(param2);
         _loc3_.x = param1.x;
         _loc3_.y = param1.y;
         _loc3_.name = param2;
         this.container.addChildAt(_loc3_,this.container.getChildIndex(param1));
         param1.removeFromParent();
         return _loc3_;
      }
      
      override protected function commitData() : void
      {
         var _loc2_:§_-2m§ = null;
         var _loc3_:§_-Oi§ = null;
         var _loc4_:Boolean = false;
         var _loc5_:§_-O2H§ = null;
         var _loc6_:uint = 0;
         if(_data == null)
         {
            return;
         }
         var _loc1_:§_-W1r§ = §_-W1r§(_data);
         this.title.text = §_-s2a§.§_-K3t§(_loc1_.getName());
         if(this.icon)
         {
            this.icon.width = this.§_-G3S§;
            this.icon.height = this.§_-G3S§;
         }
         this.setIcon(_loc1_);
         if(_loc1_)
         {
            _loc2_ = Application.userProfile.§_-pd§();
            _loc3_ = _loc2_.§_-d0§(_loc1_.getId());
            _loc4_ = _loc1_ is §_-O2H§;
            _loc5_ = _loc4_ ? §_-O2H§(_loc1_) : null;
            if(_loc4_ && _loc5_.isComplex())
            {
               _loc6_ = §_-Cq§.§_-sj§(_loc5_.getId());
            }
         }
      }
      
      private function setIcon(param1:§_-W1r§) : void
      {
         var _loc2_:DisplayObject = null;
         this.§_-m1n§(this.iconContainer);
         if(param1 is §_-O2H§)
         {
            _loc2_ = §_-q15§.§_-82W§(param1 as §_-O2H§);
            this.§_-22j§(_loc2_,this.iconContainer);
         }
         else if(param1 is §_-L1r§)
         {
            this.iconSource = §_-q15§.§_-zE§(param1 as §_-L1r§);
            _loc2_ = this.iconSource.display;
            _loc2_.scale = 0.7;
         }
         this.§_-71o§(_loc2_,param1);
         this.§_-FB§(this.iconContainer,_loc2_);
      }
      
      private function §_-22j§(param1:DisplayObject, param2:DisplayObjectContainer) : void
      {
         var _loc3_:Number = param2.width / param1.width;
         var _loc4_:Number = param2.height / param1.height;
         param1.scale = Math.min(_loc3_,_loc4_);
      }
      
      private function §_-72I§(param1:§_-W1r§) : Boolean
      {
         var _loc3_:§_-O2H§ = null;
         var _loc4_:§_-Oi§ = null;
         var _loc2_:Boolean = false;
         if(Boolean(Application.userProfile) && Boolean(Application.userProfile.§_-33n§()) && param1 is §_-O2H§)
         {
            _loc3_ = §_-O2H§(param1);
            _loc4_ = Application.userProfile.§_-pd§().§_-d0§(param1.getId());
            if(param1.§_-T1J§() <= Application.userProfile.getLevel() && param1.§_-91e§() > 0 && (_loc3_.isComplex() && (!_loc4_ || !_loc4_.§_-n1u§()) || !_loc3_.isComplex() && _loc3_.§_-8C§() == -1 && !_loc4_))
            {
               _loc2_ = true;
            }
         }
         return _loc2_;
      }
      
      private function §_-I1u§(param1:§_-W1r§) : void
      {
         this.§_-L1Y§(this.fuzyPrice as DisplayObject);
         this.§_-L1Y§(this.fuzyIcon);
         this.§_-L1Y§(this.rubyPrice as DisplayObject);
         this.§_-L1Y§(this.rubyIcon);
         this.fuzyPrice.text = param1.§_-91e§().toString();
         this.fuzyIcon.visible = this.fuzyPrice.visible = param1.§_-91e§() > 0;
         this.rubyPrice.text = param1.§_-gc§().toString();
         §§push(this.rubyIcon);
         §§push(this.rubyPrice);
         var _temp_2:* = param1.§_-gc§() > 0;
         §§pop().visible = §§pop().visible = param1.§_-gc§() > 0;
         §_-J2h§.§_-i1z§(this.fuzyPrice,this.rubyPrice);
         §_-J2h§.§_-i1z§(this.fuzyIcon,this.rubyIcon);
      }
      
      private function §_-J2I§(param1:Boolean) : void
      {
         if(param1)
         {
            this.§_-S19§(false);
            if(!this.§_-L28§)
            {
               this.createTimer();
            }
            else
            {
               this.§_-L28§.visible = true;
            }
            Application.userProfile.§_-pk§().addEventListener(starling.events.Event.COMPLETE,this.removeTimer);
         }
         else
         {
            if(this.§_-L28§)
            {
               this.§_-L28§.dispose();
               this.§_-L28§ = null;
            }
            Application.userProfile.§_-pk§().removeEventListener(starling.events.Event.COMPLETE,this.removeTimer);
         }
         this.fuzyIcon.visible = this.fuzyPrice.visible = this.rubyIcon.visible = this.rubyPrice.visible = !param1;
         this.priceBackground.visible = !param1;
      }
      
      private function removeTimer(param1:starling.events.Event) : void
      {
         this.§_-J2I§(false);
      }
      
      private function createTimer() : void
      {
         this.§_-L28§ = new §_-G1F§(§_-YQ§.§_-qM§(§_-q2v§.§_-438§,§_-q2v§.§_-A2T§),Application.userProfile.§_-pk§());
         var _loc1_:DisplayObject = this.§_-L28§.display;
         _loc1_.x = 15;
         _loc1_.y = 145;
         _loc1_.scale = §_-U1I§;
         this.container.addChildAt(_loc1_,this.container.getChildIndex(this.background) + 1);
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(_data == null || !this.isInvalid(INVALIDATION_FLAG_DATA))
         {
            return;
         }
         var _loc1_:§_-W1r§ = §_-W1r§(_data);
         var _loc2_:Boolean = §_-G1p§.§_-61d§(_loc1_);
         var _loc3_:Boolean = §_-Cq§.§_-hr§(_loc1_ as §_-O2H§);
         if(!_loc2_ && this.§_-72I§(_loc1_))
         {
            this.§_-J2I§(true);
         }
         else
         {
            this.§_-J2I§(false);
            this.§_-I1u§(_loc1_);
         }
         var _loc4_:Boolean = !§_-F4§.§_-EY§(_loc1_ as §_-1Y§) || !_loc1_.§_-gc§() && !§_-F4§.§_-f16§(_loc1_ as §_-1Y§);
         var _loc5_:int = _loc1_.§_-T1J§() > Application.userProfile.getLevel() ? int(_loc1_.§_-T1J§()) : 0;
         var _loc6_:Boolean = !_loc2_ && !_loc3_ && _loc1_ is §_-O2H§ && _loc5_ > 0;
         this.§_-S19§(_loc4_,_loc5_,_loc6_);
         this.§_-21E§(_loc2_);
         this.§_-nN§(_loc3_);
      }
      
      private function §_-m1n§(param1:DisplayObjectContainer) : void
      {
         var _loc2_:DisplayObject = param1.getChildByName("itemIcon");
         if(_loc2_)
         {
            param1.removeChild(_loc2_);
            _loc2_.dispose();
         }
         this.§_-U1j§();
      }
      
      private function §_-U1j§() : void
      {
         if(this.iconSource)
         {
            this.iconSource.dispose();
            this.iconSource = null;
         }
      }
      
      private function §_-71o§(param1:DisplayObject, param2:§_-W1r§) : void
      {
         if(param2 is §_-L1r§)
         {
            param1.y = §_-MG§;
         }
      }
      
      private function §_-FB§(param1:DisplayObjectContainer, param2:DisplayObject) : void
      {
         param2.name = "itemIcon";
         param1.addChild(param2);
      }
      
      private function §_-L1Y§(param1:DisplayObject) : void
      {
         if(this.§_-J2K§[param1.name] == null)
         {
            this.§_-J2K§[param1.name] = Pool.getPoint(param1.x,param1.y);
         }
         else
         {
            param1.x = this.§_-J2K§[param1.name].x;
            param1.y = this.§_-J2K§[param1.name].y;
         }
      }
      
      private function §_-S19§(param1:Boolean, param2:int = 0, param3:Boolean = false) : void
      {
         this.lock.visible = param1 || param3;
         this.lock_text.visible = param1 && param2 > 0 || param3;
         this.lock_text.text = param2.toString();
         if(param1)
         {
            this.fuzyIcon.switched = true;
            this.rubyIcon.switched = true;
            this.background.switched = true;
            this.priceBackground.switched = true;
         }
         else
         {
            this.fuzyIcon.switched = false;
            this.rubyIcon.switched = false;
            this.background.switched = false;
            this.priceBackground.switched = false;
         }
      }
      
      private function §_-21E§(param1:Boolean) : void
      {
         this.purchased.visible = param1;
      }
      
      private function §_-nN§(param1:Boolean) : void
      {
         this.hasImprovements.visible = param1;
      }
      
      override public function getBounds(param1:DisplayObject, param2:Rectangle = null) : Rectangle
      {
         return this.container.getBounds(param1,param2);
      }
      
      override public function setSize(param1:Number, param2:Number) : void
      {
         var _loc3_:Number = Math.min(param1 / §_-K2K§,param2 / §_-r1a§);
         this.container.width = §_-K2K§ * _loc3_;
         this.container.height = §_-r1a§ * _loc3_;
         super.setSize(this.container.width,this.container.height);
         padding = 0;
      }
      
      public function getOrigin() : DisplayObject
      {
         return this;
      }
      
      public function §_-8I§() : DisplayObject
      {
         return §_-y2n§.§_-i1N§.§_-8N§(data as §_-W1r§);
      }
      
      public function §_-52e§() : Object
      {
         return new <String>[RelativePosition.BOTTOM,RelativePosition.TOP];
      }
      
      override public function dispose() : void
      {
         var _loc1_:Point = null;
         for each(_loc1_ in this.§_-J2K§)
         {
            Pool.putPoint(_loc1_);
         }
         this.§_-J2K§ = null;
         §_-p28§ = Math.max(§_-p28§ - 1,0);
         if(§_-p28§ == 0)
         {
            SwitchedRenderTextures.clearAll();
         }
         Application.userProfile.§_-pk§().removeEventListener(starling.events.Event.COMPLETE,this.removeTimer);
         this.container = null;
         this.background = null;
         if(this.iconContainer)
         {
            this.iconContainer.dispose();
            this.iconContainer = null;
         }
         this.§_-U1j§();
         if(this.icon)
         {
            this.icon.dispose();
            this.icon = null;
         }
         if(this.title)
         {
            this.title.dispose();
            this.title = null;
         }
         if(this.fuzyPrice)
         {
            this.fuzyPrice.dispose();
            this.fuzyPrice = null;
         }
         if(this.rubyPrice)
         {
            this.rubyPrice.dispose();
            this.rubyPrice = null;
         }
         if(this.rubyIcon)
         {
            this.rubyIcon.dispose();
            this.rubyIcon = null;
         }
         if(this.fuzyIcon)
         {
            this.fuzyIcon.dispose();
            this.fuzyIcon = null;
         }
         if(this.purchased)
         {
            this.purchased.dispose();
            this.purchased = null;
         }
         if(this.lock)
         {
            this.lock.dispose();
            this.lock = null;
         }
         if(this.lock_text)
         {
            this.lock_text.dispose();
            this.lock_text = null;
         }
         if(this.§_-L28§)
         {
            this.§_-L28§.dispose();
            this.§_-L28§ = null;
         }
         if(SystemUtil.isDesktop)
         {
            §_-q1j§.remove(this);
         }
         super.dispose();
      }
      
      override public function set isSelected(param1:Boolean) : void
      {
         super.isSelected = param1;
         this.selectionFrame.visible = param1;
      }
   }
}

