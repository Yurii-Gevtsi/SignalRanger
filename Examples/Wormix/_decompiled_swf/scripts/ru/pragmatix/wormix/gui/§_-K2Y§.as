package ru.pragmatix.wormix.gui
{
   import §_-31W§.*;
   import §_-5Y§.*;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-73I§.§_-Y1l§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.*;
   import §_-B2z§.§_-63i§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.*;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-T22§;
   import §_-DJ§.§_-T2G§;
   import §_-H2g§.§_-s1d§;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-O2L§.*;
   import §_-P1z§.*;
   import §_-P2i§.§_-G3J§;
   import §_-P2i§.§_-b7§;
   import §_-RE§.§_-r2q§;
   import §_-S1N§.§_-R1U§;
   import §_-SP§.§_-M4§;
   import §_-T2h§.*;
   import §_-TF§.§_-q1j§;
   import §_-TF§.§_-y2n§;
   import §_-Tm§.§_-RF§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1b§.§_-81l§;
   import §_-ZP§.§_-b23§;
   import §_-e2r§.*;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-j1w§.§_-lD§;
   import §_-jF§.*;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-lh§.§_-z2A§;
   import §_-r1C§.§_-h2B§;
   import §_-u2J§.§_-A0§;
   import §_-w§.§_-63m§;
   import §_-w2W§.WardrobeRoomScreen;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-y1S§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.loading.*;
   import com.greensock.loading.core.LoaderCore;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import config.§_-V2w§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.NumericStepper;
   import feathers.controls.Panel;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.ToggleButton;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.PopUpManager;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.VerticalAlign;
   import feathers.themes.styles.AlternateButtonsStyles;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.utils.setTimeout;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.gui.controls.§_-yf§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.server.UserIsBanned;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.LightningBullet;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.extensions.ParticleAura;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starlingbuilder.engine.UIBuilder;
   
   use namespace bi_internal;
   
   public class §_-K2Y§ extends Panel implements §_-T22§
   {
      
      protected var text:Label;
      
      protected var §_-Kx§:Object;
      
      private var §_-p1y§:Boolean = false;
      
      private var §_-gN§:Array = new Array();
      
      private var §_-S2I§:Array = new Array();
      
      public function §_-K2Y§(param1:Object = null)
      {
         super();
         this.§_-Kx§ = param1;
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         var _loc1_:Image = new Image(Application.assetManager.getTexture("LoadingMessageWithIcon_back"));
         this.backgroundSkin = _loc1_;
         _loc1_.scale9Grid = Pool.getRectangle(18,18,1,1);
         this.padding = 10;
         this.paddingRight = 20;
         this.paddingLeft = 20;
         this.minWidth = 315;
         this.minHeight = 90;
         this.text = new Label();
         this.text.name = "text";
         this.text.styleName = AlternateTextStyles.DARK_BROWN_NORMAL_BIGGER_LEFT;
         addChild(this.text as DisplayObject);
         this.layout = new AnchorLayout();
         var _loc2_:AnchorLayoutData = new AnchorLayoutData();
         _loc2_.horizontalCenter = _loc2_.verticalCenter = 0;
         (this.text as FeathersControl).layoutData = _loc2_;
      }
      
      override public function dispose() : void
      {
         var _loc1_:Image = null;
         if(Boolean(backgroundSkin) && backgroundSkin is Image)
         {
            _loc1_ = backgroundSkin as Image;
            Pool.putRectangle(_loc1_.scale9Grid);
            _loc1_.scale9Grid = null;
         }
         super.dispose();
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            this.update();
         }
      }
      
      public function update() : void
      {
         var _loc1_:Label = null;
         var _loc2_:String = null;
         var _loc3_:Object = null;
         if(this.§_-Kx§ != null)
         {
            for(_loc2_ in this.§_-Kx§)
            {
               _loc1_ = getChildByName(_loc2_) as Label;
               if(_loc1_)
               {
                  _loc3_ = this.§_-Kx§[_loc2_];
                  if(_loc3_ is String)
                  {
                     _loc1_.text = _loc3_ as String;
                  }
                  else if(_loc3_.text)
                  {
                     _loc1_.text = _loc3_.text;
                  }
                  else if(_loc3_.textId)
                  {
                     if(_loc3_.textId)
                     {
                        _loc1_.text = §_-s2a§.§_-K3t§(_loc3_.textId);
                     }
                  }
                  _loc1_.invalidate();
               }
            }
            invalidate(INVALIDATION_FLAG_SIZE);
         }
      }
      
      public function show(param1:Function = null, ... rest) : void
      {
         this.§_-Kx§.descr = "";
         this.§_-Q2j§(true,param1,rest);
      }
      
      public function §_-x2V§(param1:Function = null, ... rest) : void
      {
         this.§_-Kx§.descr = "";
         this.§_-Q2j§(false,param1,rest);
      }
      
      public function §_-91V§(param1:String, param2:Function = null, ... rest) : void
      {
         this.§_-Kx§.descr = §_-s2a§.§_-K3t§(param1);
         this.§_-Q2j§(true,param2,rest);
      }
      
      public function §_-B2h§(param1:String, param2:Function = null, ... rest) : void
      {
         this.§_-Kx§.descr = §_-s2a§.§_-K3t§(param1);
         this.§_-Q2j§(false,param2,rest);
      }
      
      protected function §_-Q2j§(param1:Boolean = true, param2:Function = null, param3:Array = null) : void
      {
         if(!this.§_-h1C§())
         {
            this.§_-gN§.push(param2);
            this.§_-S2I§.push(param3);
            if(!this.§_-p1y§)
            {
               this.§_-p1y§ = true;
            }
            PopUpManager.addPopUp(this,param1,true,this.§_-12y§);
            §_-T2G§.§_-F1w§(this);
            invalidate(INVALIDATION_FLAG_DATA);
         }
         else
         {
            this.executeCallback(param2,param3);
         }
      }
      
      override protected function feathersControl_addedToStageHandler(param1:Event) : void
      {
         super.feathersControl_addedToStageHandler(param1);
         this.§_-p1y§ = false;
         setTimeout(this.§_-T2W§,30);
      }
      
      public function hide() : void
      {
         if(this.§_-h1C§())
         {
            PopUpManager.removePopUp(this);
         }
         §_-T2G§.§_-53B§(this);
      }
      
      public function §_-h1C§() : Boolean
      {
         return PopUpManager.isPopUp(this);
      }
      
      protected function §_-12y§() : DisplayObject
      {
         var _loc1_:Quad = new Quad(Application.§_-i1N§.stageWidth,Application.§_-i1N§.stageHeight,§_-81L§.§_-YC§);
         _loc1_.alpha = §_-81L§.§_-a1e§;
         return _loc1_;
      }
      
      private function §_-T2W§() : void
      {
         var _loc1_:int = int(this.§_-gN§.length);
         while(--_loc1_ > -1)
         {
            this.executeCallback(this.§_-gN§.pop(),this.§_-S2I§.pop());
         }
      }
      
      private function executeCallback(param1:Function, param2:Array) : void
      {
         if(param1 != null)
         {
            if(param2 != null && param2.length > 0)
            {
               param1.apply(this,param2);
            }
            else
            {
               param1();
            }
            param1 = null;
            param2 = null;
         }
      }
      
      public function §_-X2O§() : void
      {
      }
   }
}

