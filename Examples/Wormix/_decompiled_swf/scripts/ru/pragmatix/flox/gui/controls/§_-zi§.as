package ru.pragmatix.flox.gui.controls
{
   import §_-02q§.§_-4s§;
   import §_-12W§.*;
   import §_-12a§.§_-41b§;
   import §_-12a§.§_-E1Q§;
   import §_-12a§.§_-G2l§;
   import §_-21p§.§_-4w§;
   import §_-31N§.§_-42G§;
   import §_-31W§.§_-E1E§;
   import §_-5Y§.*;
   import §_-62§.*;
   import §_-82l§.§_-A2Y§;
   import §_-91B§.§_-QK§;
   import §_-931§.§_-2o§;
   import §_-931§.§_-i2r§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2F§.§_-4B§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-CR§.§_-72p§;
   import §_-Cl§.*;
   import §_-D3A§.*;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-J2l§;
   import §_-F39§.*;
   import §_-G2H§.*;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-Ly§.§_-n1J§;
   import §_-N8§.§_-o2w§;
   import §_-O2L§.§_-13p§;
   import §_-O2L§.§_-C3d§;
   import §_-O2L§.§_-X1T§;
   import §_-P2i§.§_-b7§;
   import §_-RE§.§_-52p§;
   import §_-S1N§.§_-C14§;
   import §_-S1N§.§_-RO§;
   import §_-S1N§.§_-zk§;
   import §_-TF§.§_-q1j§;
   import §_-TF§.§_-y2n§;
   import §_-Vg§.ClanCreateView;
   import §_-Vu§.MD5;
   import §_-X14§.§_-03Y§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-Y1b§.§_-OP§;
   import §_-Z1w§.§_-w1D§;
   import §_-e2r§.*;
   import §_-fo§.*;
   import §_-g1P§.§_-L38§;
   import §_-g2C§.GlobalChatTextMessageItemRenderer;
   import §_-g2C§.§_-62U§;
   import §_-g2C§.§_-I1R§;
   import §_-g2C§.§_-K2A§;
   import §_-g2C§.§_-K2u§;
   import §_-g2C§.§_-p8§;
   import §_-g2C§.§_-w1K§;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-uZ§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.*;
   import §_-l1g§.§_-F1X§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-l2r§.§_-K1t§;
   import §_-n1w§.X509Certificate;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-sQ§.§_-V1n§;
   import §_-u1m§.*;
   import §_-w2W§.*;
   import §_-wD§.*;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import §_-z20§.*;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-F3q§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import config.§_-B1K§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ButtonState;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.Panel;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.core.ITextRenderer;
   import feathers.core.PopUpManager;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.utils.textures.TextureCache;
   import flash.display.*;
   import flash.errors.IllegalOperationError;
   import flash.events.*;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.net.SharedObject;
   import flash.system.LoaderContext;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.*;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.§_-11V§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.messages.server.ArenaLocked;
   import ru.pragmatix.wormix.messages.server.BuyRaceResult;
   import ru.pragmatix.wormix.messages.server.IncreaseAchievementResult;
   import ru.pragmatix.wormix.messages.server.Pong;
   import ru.pragmatix.wormix.messages.server.StartBattleResult;
   import ru.pragmatix.wormix.messages.structures.BonusDaysStructure;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.pvp.messages.structures.PositionStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.utils.Align;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starlingbuilder.engine.tween.WormixTweenBuilder;
   
   use namespace bi_internal;
   
   public class §_-zi§ extends §_-a1t§ implements §_-t2z§
   {
      
      protected var §_-nJ§:flash.display.DisplayObject;
      
      protected var children:Dictionary = new Dictionary();
      
      public function §_-zi§(param1:flash.display.DisplayObject)
      {
         super(param1);
         if(!param1 is flash.display.DisplayObjectContainer)
         {
            throw new IllegalOperationError("container must be instance of DisplayObjectContainer");
         }
         this.§_-nJ§ = this.getChildByName("disabled");
      }
      
      public function getChildByName(param1:String) : flash.display.DisplayObject
      {
         return §_-4s§.getChildByName(this.§_-j2l§(),param1);
      }
      
      public function §_-f1p§(param1:String) : §_-a1t§
      {
         var _loc3_:§_-a1t§ = null;
         var _loc2_:flash.display.DisplayObject = this.getChildByName(param1);
         if(_loc2_)
         {
            _loc3_ = new §_-a1t§(_loc2_);
            this.§_-D1g§(_loc3_,param1);
            return _loc3_;
         }
         return null;
      }
      
      public function §_-E1C§(param1:String) : §_-zi§
      {
         var _loc3_:§_-zi§ = null;
         var _loc2_:flash.display.DisplayObject = this.getChildByName(param1);
         if(_loc2_)
         {
            _loc3_ = new §_-zi§(_loc2_);
            this.§_-D1g§(_loc3_,param1);
            return _loc3_;
         }
         return null;
      }
      
      public function §_-02H§(param1:String) : §_-zk§
      {
         var _loc3_:§_-zk§ = null;
         var _loc2_:flash.display.DisplayObject = this.getChildByName(param1);
         if(_loc2_)
         {
            _loc3_ = §_-C14§.createButton(_loc2_);
            this.§_-D1g§(_loc3_,param1);
            return _loc3_;
         }
         return null;
      }
      
      public function §_-K3i§(param1:String, param2:String = null) : §_-RO§
      {
         var _loc4_:§_-RO§ = null;
         var _loc3_:flash.display.DisplayObject = this.getChildByName(param1);
         if(_loc3_)
         {
            _loc4_ = §_-C14§.§_-sk§(_loc3_,param2);
            this.§_-D1g§(_loc4_,param1);
            return _loc4_;
         }
         return null;
      }
      
      public function §_-x2b§(param1:String, param2:String, param3:String = null) : §_-RO§
      {
         var _loc5_:§_-RO§ = null;
         var _loc4_:flash.display.DisplayObject = this.getChildByName(param1);
         if(_loc4_)
         {
            _loc5_ = §_-C14§.§_-sk§(_loc4_,param3);
            this.§_-D1g§(_loc5_,param1);
            if(param2)
            {
               _loc5_.§_-r1M§(param2);
            }
            return _loc5_;
         }
         return null;
      }
      
      public function §_-71m§(param1:String) : TextField
      {
         return this.getChildByName(param1) as TextField;
      }
      
      public function §_-U2y§(param1:String, param2:String = null, param3:TextFormat = null, param4:Object = null, param5:Boolean = true) : §_-i2r§
      {
         var _loc6_:§_-i2r§ = null;
         var _loc7_:TextField = null;
         if(this.children[param1])
         {
            _loc6_ = this.children[param1];
         }
         else
         {
            _loc7_ = this.getChildByName(param1) as TextField;
            if(_loc7_)
            {
               _loc6_ = new §_-i2r§(_loc7_);
               this.§_-D1g§(_loc6_,param1);
            }
         }
         if(_loc6_)
         {
            if(param2 != null)
            {
               _loc6_.setStringID(param2,param4);
            }
            if(param3)
            {
               _loc6_.setFormat(param3,param5);
            }
            return _loc6_;
         }
         return null;
      }
      
      public function §_-g2§(param1:String, param2:String = "") : §_-i2r§
      {
         var _loc3_:§_-i2r§ = this.§_-U2y§(param1,param2);
         if(_loc3_)
         {
            _loc3_.§_-h1r§(true);
         }
         return _loc3_;
      }
      
      public function §_-pE§(param1:String, param2:String = "") : §_-2o§
      {
         var _loc3_:§_-2o§ = null;
         var _loc4_:TextField = null;
         if(this.children[param1])
         {
            _loc3_ = this.children[param1];
         }
         else
         {
            var _temp_1:* = this.getChildByName(param1) as TextField;
            _loc4_ = this.getChildByName(param1) as TextField;
            if(_loc4_)
            {
               _loc3_ = new §_-2o§(_loc4_,param2);
               this.§_-D1g§(_loc3_,param1);
            }
         }
         if(_loc3_)
         {
            return _loc3_;
         }
         return null;
      }
      
      public function §_-M1Y§(param1:String, param2:String, param3:Object, param4:Object = null, param5:TextFormat = null) : §_-i2r§
      {
         var _loc6_:§_-i2r§ = this.§_-U2y§(param1);
         if(_loc6_)
         {
            _loc6_.setTemplate(param2,param3,param4);
            if(param5)
            {
               _loc6_.setTextFormat(param5);
            }
         }
         return _loc6_;
      }
      
      public function §_-CS§(param1:String) : MovieClip
      {
         return this.getChildByName(param1) as MovieClip;
      }
      
      private function §_-B1Q§(param1:String) : String
      {
         return this.id + "." + param1;
      }
      
      protected function §_-D1g§(param1:§_-yf§, param2:String) : void
      {
         if(this.children[param2])
         {
            throw new Error("Child \'" + param2 + "\' already exist");
         }
         this.children[param2] = param1;
      }
      
      override public function §_-tx§(param1:Boolean) : void
      {
         var _loc2_:int = 0;
         var _loc3_:Object = null;
         super.§_-tx§(param1);
         if(this.§_-nJ§)
         {
            if(removeAllWhenDisabled)
            {
               _loc2_ = 0;
               while(_loc2_ < this.§_-j2l§().numChildren)
               {
                  _loc3_ = this.§_-j2l§().getChildAt(_loc2_);
                  _loc3_.visible = _loc3_ != this.§_-nJ§ && !param1;
                  _loc2_++;
               }
            }
            this.§_-nJ§.visible = param1;
         }
      }
      
      override protected function §_-q2a§() : void
      {
         super.§_-q2a§();
         if(this.§_-nJ§)
         {
            this.§_-tx§(disabled);
         }
      }
      
      public function addChild(param1:§_-yf§) : void
      {
         this.§_-72v§(param1.getContainer());
      }
      
      public function addChildAt(param1:§_-yf§, param2:int) : void
      {
         this.§_-13K§(param1.getContainer(),param2);
      }
      
      public function removeChild(param1:§_-yf§) : void
      {
         this.§_-k6§(param1.getContainer());
      }
      
      public function §_-72v§(param1:flash.display.DisplayObject) : void
      {
         this.§_-j2l§().addChild(param1);
      }
      
      public function §_-210§(param1:flash.display.DisplayObject) : Boolean
      {
         return this.§_-j2l§().contains(param1);
      }
      
      public function contains(param1:§_-yf§) : Boolean
      {
         return this.§_-210§(param1.getContainer());
      }
      
      public function §_-13K§(param1:flash.display.DisplayObject, param2:int) : void
      {
         this.§_-j2l§().addChildAt(param1,param2);
      }
      
      public function §_-Y2G§(param1:flash.display.DisplayObject) : int
      {
         return this.§_-j2l§().getChildIndex(param1);
      }
      
      public function §_-k6§(param1:flash.display.DisplayObject) : void
      {
         this.§_-j2l§().removeChild(param1);
      }
      
      public function swapChildren(param1:§_-yf§, param2:§_-yf§) : void
      {
         this.§_-j2l§().swapChildren(param1.getContainer(),param2.getContainer());
      }
      
      public function §_-83z§(param1:flash.display.DisplayObject, param2:flash.display.DisplayObject) : void
      {
         this.§_-j2l§().swapChildren(param1,param2);
      }
      
      private function §_-B2Z§(param1:String, param2:TextFormat = null) : TextField
      {
         var _loc3_:TextField = §_-4s§.§_-2P§(this.§_-j2l§(),param1);
         if(param2)
         {
            §_-n1J§.§_-03U§(_loc3_,param2);
         }
         return _loc3_;
      }
      
      public function §_-X2v§(param1:String, param2:Object, param3:TextFormat = null) : void
      {
         var _loc4_:TextField = §_-4s§.§_-2P§(this.§_-j2l§(),param1);
         _loc4_.htmlText = param2.toString();
         §_-n1J§.§_-03U§(_loc4_,param3);
      }
      
      public function §_-H2H§(param1:String, param2:Object, param3:TextFormat = null) : TextField
      {
         var _loc4_:TextField = §_-4s§.§_-2P§(this.§_-j2l§(),param1);
         §_-n1J§.§_-03U§(_loc4_,param3,param2.toString());
         return _loc4_;
      }
      
      public function §_-n2v§(param1:String, param2:Object, param3:TextFormat = null) : void
      {
         §_-n1J§.§_-n2v§(this,param1,param2.toString(),param3);
      }
      
      public function §_-j2l§() : flash.display.DisplayObjectContainer
      {
         return container as flash.display.DisplayObjectContainer;
      }
      
      protected function §_-pt§(... rest) : void
      {
         var _loc4_:Object = null;
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         var _loc7_:Boolean = false;
         var _loc2_:Number = Number.MAX_VALUE;
         var _loc3_:Number = Number.MIN_VALUE;
         for each(_loc4_ in rest)
         {
            _loc7_ = _loc4_ is flash.display.DisplayObject && DisplayObject(_loc4_).visible || _loc4_ is §_-yf§ && §_-yf§(_loc4_).isVisible();
            if(_loc7_)
            {
               _loc2_ = Math.min(_loc2_,_loc4_.x);
               _loc3_ = Math.max(_loc3_,_loc4_.x + _loc4_.width);
            }
         }
         _loc5_ = (width - (_loc3_ - _loc2_)) / 2;
         _loc6_ = _loc2_ - _loc5_;
         for each(_loc4_ in rest)
         {
            _loc7_ = _loc4_ is flash.display.DisplayObject && DisplayObject(_loc4_).visible || _loc4_ is §_-yf§ && §_-yf§(_loc4_).isVisible();
            if(_loc7_)
            {
               _loc4_.x -= _loc6_;
            }
         }
      }
      
      protected function §_-y2h§(... rest) : flash.display.Sprite
      {
         var _loc6_:Object = null;
         var _loc7_:Boolean = false;
         var _loc2_:flash.display.Sprite = new flash.display.Sprite();
         var _loc3_:int = 0;
         var _loc4_:Number = 0;
         var _loc5_:Number = 0;
         for each(_loc6_ in rest)
         {
            _loc7_ = _loc6_ is flash.display.DisplayObject && DisplayObject(_loc6_).visible || _loc6_ is §_-yf§ && §_-yf§(_loc6_).isVisible();
            if(_loc7_)
            {
               if(_loc3_ > 0)
               {
                  _loc6_.x -= _loc4_;
                  _loc6_.y -= _loc5_;
               }
               else
               {
                  _loc4_ = Number(_loc6_.x);
                  _loc5_ = Number(_loc6_.y);
                  _loc6_.x = 0;
                  _loc6_.y = 0;
               }
               _loc6_.y -= _loc2_.y;
               _loc2_.addChild(_loc6_ is flash.display.DisplayObject ? _loc6_ as flash.display.DisplayObject : _loc6_.getContainer());
               _loc3_++;
            }
         }
         _loc2_.x = _loc4_;
         _loc2_.y = _loc5_;
         this.§_-72v§(_loc2_);
         return _loc2_;
      }
      
      override public function destroy() : void
      {
         var _loc1_:§_-yf§ = null;
         for each(_loc1_ in this.children)
         {
            _loc1_.destroy();
         }
         this.children = null;
         this.§_-nJ§ = null;
         super.destroy();
      }
   }
}

