package ru.pragmatix.wormix.gui
{
   import §_-21p§.§_-4w§;
   import §_-2U§.*;
   import §_-31N§.§_-42G§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-C2t§.§_-wL§;
   import §_-CF§.§_-E19§;
   import §_-F39§.§_-Pn§;
   import §_-L1H§.§_-C3e§;
   import §_-YF§.*;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-v2Q§;
   import §_-f1G§.§_-kf§;
   import §_-j22§.§_-5g§;
   import §_-jF§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.*;
   import §_-w2i§.*;
   import §_-zI§.§_-sI§;
   import feathers.controls.Button;
   import feathers.controls.ToggleButton;
   import feathers.layout.AnchorLayoutData;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.LowerRankInClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.Link;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   use namespace gestouch_internal;
   
   public class §_-L1X§ extends §_-qY§
   {
      
      private static const LEFT_BUTTON_POSITION:String = "LEFT_BUTTON_POSITION";
      
      private static const RIGHT_BUTTON_POSITION:String = "RIGHT_BUTTON_POSITION";
      
      private static const UP_BUTTON_POSITION:String = "UP_BUTTON_POSITION";
      
      private static const DOWN_BUTTON_POSITION:String = "DOWN_BUTTON_POSITION";
      
      private static const JUMP_BUTTON_POSITION:String = "JUMP_BUTTON_POSITION";
      
      private static const §_-X1a§:String = "BACK_BUTTON_POSITION";
      
      private static const SHOOT_BUTTON_POSITION:String = "SHOOT_BUTTON_POSITION";
      
      private static const SHIFT_BUTTON_POSITION:String = "SHIFT_BUTTON_POSITION";
      
      private static const ALT_BUTTON_POSITION:String = "ALT_BUTTON_POSITION";
      
      private static const BAG_BUTTON_POSITION:String = "BAG_BUTTON_POSITION";
      
      public var shootButton:Button;
      
      public var jumpButton:Button;
      
      public var backJumpButton:Button;
      
      public var bagButton:Button;
      
      public var shiftButton:Button;
      
      public var altButton:ToggleButton;
      
      public var leftButton:Button;
      
      public var rightButton:Button;
      
      public var upButton:Button;
      
      public var downButton:Button;
      
      public var §_-mp§:§_-Pn§;
      
      public var §_-w12§:§_-Pn§;
      
      public var buttons:Vector.<Button>;
      
      public function §_-L1X§(param1:§_-E2w§, param2:Number, param3:Number)
      {
         super(param1,param2,param3);
      }
      
      override protected function §_-I2w§() : void
      {
         this.shootButton = §_-I3V§("shoot") as Button;
         this.§_-mp§ = new §_-Pn§(this.shootButton);
         this.jumpButton = §_-I3V§("jump") as Button;
         this.§_-j6§(this.jumpButton);
         this.backJumpButton = §_-I3V§("backJump") as Button;
         this.§_-j6§(this.backJumpButton);
         this.bagButton = §_-I3V§("backpack") as Button;
         this.§_-w12§ = new §_-Pn§(this.bagButton);
         this.leftButton = §_-I3V§("left") as Button;
         this.rightButton = §_-I3V§("right") as Button;
         this.upButton = §_-I3V§("up") as Button;
         this.downButton = §_-I3V§("down") as Button;
         this.shiftButton = §_-I3V§("shift") as Button;
         this.altButton = §_-I3V§("alt") as ToggleButton;
         §§push(this);
         var _temp_7:* = new <Button>[this.upButton,this.downButton,this.leftButton,this.rightButton,this.jumpButton,this.backJumpButton,this.shiftButton,this.altButton,this.bagButton,this.shootButton];
         §§pop().buttons = new <Button>[this.upButton,this.downButton,this.leftButton,this.rightButton,this.jumpButton,this.backJumpButton,this.shiftButton,this.altButton,this.bagButton,this.shootButton];
      }
      
      override protected function §_-7R§() : void
      {
         §_-92r§(this.leftButton,true,LEFT_BUTTON_POSITION);
         §_-92r§(this.rightButton,true,RIGHT_BUTTON_POSITION);
         §_-92r§(this.upButton,true,UP_BUTTON_POSITION);
         §_-92r§(this.downButton,true,DOWN_BUTTON_POSITION);
         §_-92r§(this.bagButton,true,BAG_BUTTON_POSITION);
         §_-92r§(this.shootButton,true,SHOOT_BUTTON_POSITION);
         §_-92r§(this.shiftButton,true,SHIFT_BUTTON_POSITION);
         §_-92r§(this.altButton,true,ALT_BUTTON_POSITION);
         §_-92r§(this.jumpButton,true,JUMP_BUTTON_POSITION);
         §_-92r§(this.backJumpButton,true,§_-X1a§);
      }
      
      private function §_-j6§(param1:Button) : void
      {
         param1.pivotX = param1.width / 2;
      }
      
      override protected function §_-y2P§() : void
      {
         var _loc1_:AnchorLayoutData = null;
         var _loc2_:Number = 3;
         var _loc3_:Number = 10;
         §_-r1t§(this.leftButton,new AnchorLayoutData(NaN,NaN,_loc3_,_loc3_));
         _loc1_ = new AnchorLayoutData(NaN,NaN,NaN,_loc2_,NaN,0);
         _loc1_.leftAnchorDisplayObject = _loc1_.verticalCenterAnchorDisplayObject = this.leftButton;
         §_-r1t§(this.downButton,_loc1_);
         _loc1_ = new AnchorLayoutData(NaN,NaN,_loc2_,NaN,0);
         _loc1_.bottomAnchorDisplayObject = _loc1_.horizontalCenterAnchorDisplayObject = this.downButton;
         §_-r1t§(this.upButton,_loc1_);
         _loc1_ = new AnchorLayoutData(NaN,NaN,NaN,_loc2_,NaN,0);
         _loc1_.leftAnchorDisplayObject = _loc1_.verticalCenterAnchorDisplayObject = this.downButton;
         §_-r1t§(this.rightButton,_loc1_);
         §_-r1t§(this.backJumpButton,new AnchorLayoutData(NaN,this.backJumpButton.width - _loc3_ * 3,_loc3_));
         _loc1_ = new AnchorLayoutData(NaN,_loc2_,NaN,NaN,NaN,0);
         _loc1_.rightAnchorDisplayObject = _loc1_.verticalCenterAnchorDisplayObject = this.backJumpButton;
         §_-r1t§(this.jumpButton,_loc1_);
         _loc1_ = new AnchorLayoutData(NaN,_loc2_,NaN,NaN,NaN,0);
         _loc1_.rightAnchorDisplayObject = _loc1_.verticalCenterAnchorDisplayObject = this.jumpButton;
         §_-r1t§(this.shiftButton,_loc1_);
         _loc1_ = new AnchorLayoutData(NaN,_loc2_,-_loc3_ * 3,NaN);
         _loc1_.bottomAnchorDisplayObject = _loc1_.horizontalCenterAnchorDisplayObject = this.backJumpButton;
         §_-r1t§(this.bagButton,_loc1_);
         _loc1_ = new AnchorLayoutData(NaN,_loc2_,_loc2_,NaN);
         _loc1_.bottomAnchorDisplayObject = _loc1_.horizontalCenterAnchorDisplayObject = this.bagButton;
         §_-r1t§(this.shootButton,_loc1_);
         _loc1_ = new AnchorLayoutData(NaN,_loc2_,NaN,NaN,NaN,0);
         _loc1_.rightAnchorDisplayObject = _loc1_.verticalCenterAnchorDisplayObject = this.shootButton;
         §_-r1t§(this.altButton,_loc1_);
      }
      
      override public function §_-Q1a§(param1:Number) : void
      {
         §_-I1m§(this.jumpButton,param1);
         §_-I1m§(this.backJumpButton,param1);
         §_-I1m§(this.shootButton,param1);
         §_-I1m§(this.shiftButton,param1);
         §_-I1m§(this.altButton,param1);
         §_-I1m§(this.bagButton,param1);
         §_-I1m§(this.upButton,param1);
         §_-I1m§(this.downButton,param1);
         §_-I1m§(this.rightButton,param1);
         §_-I1m§(this.leftButton,param1);
      }
      
      public function §_-WM§(param1:Boolean) : void
      {
         if(param1 && this.jumpButton.scaleX > 0 || !param1 && this.jumpButton.scaleX < 0)
         {
            this.jumpButton.scaleX = -this.jumpButton.scaleX;
            this.backJumpButton.scaleX = -this.backJumpButton.scaleX;
         }
      }
      
      override public function set visible(param1:Boolean) : void
      {
         this.jumpButton.visible = this.backJumpButton.visible = this.shootButton.visible = this.leftButton.visible = this.rightButton.visible = this.upButton.visible = this.downButton.visible = this.shiftButton.visible = this.altButton.visible = param1;
      }
      
      override public function dispose() : void
      {
         if(this.jumpButton)
         {
            this.jumpButton.dispose();
         }
         this.jumpButton = null;
         if(this.backJumpButton)
         {
            this.backJumpButton.dispose();
         }
         this.backJumpButton = null;
         if(this.§_-mp§)
         {
            this.§_-mp§.dispose();
         }
         this.§_-mp§ = null;
         if(this.§_-w12§)
         {
            this.§_-w12§.dispose();
         }
         this.§_-w12§ = null;
         if(this.shootButton)
         {
            this.shootButton.dispose();
         }
         this.shootButton = null;
         if(this.leftButton)
         {
            this.leftButton.dispose();
         }
         this.leftButton = null;
         if(this.rightButton)
         {
            this.rightButton.dispose();
         }
         this.rightButton = null;
         if(this.upButton)
         {
            this.upButton.dispose();
         }
         this.upButton = null;
         if(this.downButton)
         {
            this.downButton.dispose();
         }
         this.downButton = null;
         if(this.bagButton)
         {
            this.bagButton.dispose();
         }
         this.bagButton = null;
         if(this.shiftButton)
         {
            this.shiftButton.dispose();
         }
         this.shiftButton = null;
         if(this.altButton)
         {
            this.altButton.dispose();
         }
         this.altButton = null;
         super.dispose();
      }
   }
}

