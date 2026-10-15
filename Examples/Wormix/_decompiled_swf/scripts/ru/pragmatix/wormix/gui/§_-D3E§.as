package ru.pragmatix.wormix.gui
{
   import §_-62§.§_-W1r§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-E19§;
   import §_-Cl§.*;
   import §_-H2g§.§_-di§;
   import §_-Y1Q§.§_-61Z§;
   import §_-ZP§.§_-YG§;
   import §_-ZP§.§_-w1J§;
   import §_-b19§.*;
   import §_-j1w§.*;
   import §_-l1g§.§_-F1X§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-wD§.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Label;
   import feathers.controls.ScrollPolicy;
   import feathers.core.FeathersControl;
   import feathers.events.FeathersEventType;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.geom.Rectangle;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.GetAchievementsResult;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.social.common.post.*;
   import starling.core.Starling;
   import starling.display.Canvas;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class §_-D3E§ extends §_-K2Y§
   {
      
      private var descr:Label;
      
      private var armature:§_-di§;
      
      public function §_-D3E§(param1:Object = null)
      {
         super(param1);
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this._clipContent = false;
         this.armature = §_-K1t§.buildArmature("loading_animation",§_-K1t§.§_-G1E§,§_-K1t§.§_-a1j§);
         this.armature.display.x = 34;
         this.armature.display.y = 53;
         this.armature.animation.play();
         addChild(this.armature.display);
         this.descr = new Label();
         this.descr.styleName = AlternateTextStyles.DARK_BROWN_MICRO_LEFT;
         this.descr.x = 90;
         this.descr.y = 50;
         this.descr.name = "descr";
         addChild(this.descr);
         text.x = 90;
         text.y = 18;
         (text as FeathersControl).layoutData = null;
         this.height = 90;
         this.verticalScrollPolicy = ScrollPolicy.OFF;
      }
      
      override protected function feathersControl_addedToStageHandler(param1:Event) : void
      {
         super.feathersControl_addedToStageHandler(param1);
         WorldClock.clock.add(this.armature);
      }
      
      override protected function feathersControl_removedFromStageHandler(param1:Event) : void
      {
         super.feathersControl_removedFromStageHandler(param1);
         WorldClock.clock.remove(this.armature);
      }
   }
}

