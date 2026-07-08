package ru.pragmatix.wormix.scene.water
{
   import §_-G16§.§_-33h§;
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import flash.display.BitmapData;
   import flash.display.SpreadMethod;
   import flash.geom.Rectangle;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.SystemUtil;
   
   public class Water extends Sprite
   {
      
      private static const WATER_SCALE9GRID:Rectangle = new Rectangle(1,1,1,1);
      
      private const FRONT:String = "front";
      
      private const BACK:String = "back";
      
      private const BUBBLE_MAX_AMOUNT:uint = 10;
      
      private const BUBBLE_TIMER_INTERVAL:uint = 5;
      
      private const BUBBLE_CREATION_CHANCE:Number = 0.65;
      
      private var bubbleTimer:uint;
      
      private var frontWave:Sprite;
      
      private var summ:int = -10;
      
      private const waterWidth:Number = 2800;
      
      private var waterHeight:Number;
      
      private var frontWaterColors:Array = [2045289,4081559];
      
      private var frontWaterAlphas:Array = [1,0.6];
      
      private var frontWaterRatios:Array = [40,255];
      
      private var backWaterColors:Array = [2045289,5463733];
      
      private var backWaterAlphas:Array = [0.4,0.5];
      
      private var backWaterRatios:Array = [55,255];
      
      private var waterSpreadMethod:String;
      
      private var waterBaseBitmapData:BitmapData;
      
      private var waterTopBitmapData:BitmapData;
      
      private var waterBaseImage:Image;
      
      private var waterTopImage:Image;
      
      private const BUBBLES_Y_OFFSET:Number = 20;
      
      private var bubblesPool:§_-33h§;
      
      private var activeBubbles:Vector.<BubbleObject>;
      
      public function Water()
      {
         super();
         this.addEventListener(Event.ADDED_TO_STAGE,this.onAddedToStage);
         this.bubblesPool = new §_-33h§(BubbleObject);
         this.activeBubbles = new Vector.<BubbleObject>();
      }
      
      private function onAddedToStage(param1:Event) : void
      {
         this.removeEventListener(Event.ADDED_TO_STAGE,this.onAddedToStage);
         this.bubbleTimer = Starling.juggler.repeatCall(this.onBubbleTimer,this.BUBBLE_TIMER_INTERVAL);
         this.frontWave = new Sprite();
         addChild(this.frontWave);
         this.frontWave.x = 0;
         this.frontWave.y = 100;
         this.createWave(this.frontWave);
      }
      
      private function onBubbleTimer() : void
      {
         var _loc1_:BubbleObject = null;
         var _loc2_:§_-di§ = null;
         var _loc3_:DisplayObject = null;
         if(Math.random() <= this.BUBBLE_CREATION_CHANCE)
         {
            _loc1_ = this.bubblesPool.get() as BubbleObject;
            _loc2_ = _loc1_.armature;
            _loc2_.userData = _loc1_;
            WorldClock.clock.add(_loc2_);
            _loc2_.playAnimation(§_-K1t§.§_-Y1c§,-1,-1,3);
            _loc2_.addEventListener(§_-s1d§.LOOP_COMPLETE,this.onBubbleAnimationComplete);
            _loc3_ = _loc1_.armature.display;
            _loc3_.x = Math.floor(this.waterWidth / this.BUBBLE_MAX_AMOUNT + 1) * (Math.random() * this.BUBBLE_MAX_AMOUNT + 1);
            _loc3_.y = Math.max(this.frontWave.y,this.waterHeight - this.frontWave.y) + this.BUBBLES_Y_OFFSET;
            this.frontWave.addChild(_loc3_);
            this.activeBubbles.push(_loc1_);
         }
      }
      
      private function onBubbleAnimationComplete(param1:Event) : void
      {
         var _loc2_:§_-di§ = param1.target as §_-di§;
         var _loc3_:BubbleObject = _loc2_.userData as BubbleObject;
         this.removeBubbleArmature(_loc2_);
         if(this.bubblesPool)
         {
            this.bubblesPool.put(_loc3_);
            this.activeBubbles.splice(this.activeBubbles.indexOf(_loc3_),1);
         }
      }
      
      private function removeBubbleArmature(param1:§_-di§) : void
      {
         WorldClock.clock.remove(param1);
         param1.removeEventListener(§_-s1d§.LOOP_COMPLETE,this.onBubbleAnimationComplete);
         param1.stopAnimation();
         this.frontWave.removeChild(param1.display);
      }
      
      public function destroy() : void
      {
         Starling.juggler.removeByID(this.bubbleTimer);
         this.removeActiveBubbles();
         this.bubblesPool.dispose();
         this.bubblesPool = null;
         this.destroyWaves();
         this.frontWave.dispose();
         super.dispose();
      }
      
      private function removeActiveBubbles() : void
      {
         var _loc1_:* = int(this.activeBubbles.length);
         while(_loc1_--)
         {
            this.removeBubbleArmature(this.activeBubbles[_loc1_].armature);
            this.activeBubbles[_loc1_].dispose();
         }
         this.activeBubbles = null;
      }
      
      override public function get width() : Number
      {
         return this.waterWidth;
      }
      
      override public function set height(param1:Number) : void
      {
         this.waterHeight = param1;
      }
      
      override public function get height() : Number
      {
         return this.waterHeight;
      }
      
      override public function set x(param1:Number) : void
      {
         super.x = param1;
      }
      
      override public function get x() : Number
      {
         return super.x;
      }
      
      override public function set y(param1:Number) : void
      {
         super.y = param1 - 100;
      }
      
      override public function get y() : Number
      {
         return super.y + 100;
      }
      
      public function setColor(param1:int, param2:int) : void
      {
         this.setBackColors(param1,param2);
         this.setFrontColors(param1,param2);
      }
      
      public function setFrontColors(param1:int, param2:int) : void
      {
         this.frontWaterColors = [param1,param2];
      }
      
      public function setBackColors(param1:int, param2:int) : void
      {
         this.backWaterColors = [param1,param2];
      }
      
      public function setFrontAlphas(param1:Number, param2:Number) : void
      {
         this.frontWaterAlphas = [param1,param2];
      }
      
      public function setBackAlphas(param1:Number, param2:Number) : void
      {
         this.backWaterAlphas = [param1,param2];
      }
      
      public function setFrontRatios(param1:Array) : void
      {
         this.frontWaterRatios = param1;
      }
      
      public function setBackRatios(param1:Array) : void
      {
         this.backWaterRatios = param1;
      }
      
      private function createCreast(param1:Sprite, param2:String) : Sprite
      {
         var _loc5_:Image = null;
         var _loc3_:Image = param1.getChildByName("base") as Image;
         if(_loc3_)
         {
            param1.removeChild(_loc3_);
            _loc3_.dispose();
            _loc3_ = null;
         }
         else
         {
            if(param2 == this.BACK)
            {
               param1.y -= 10;
            }
            _loc5_ = this.createWaterTop(this.waterWidth,2,9223423);
            param1.addChild(_loc5_);
         }
         var _loc4_:Image = this.createWaterBase(this.waterWidth,this.waterHeight - 1,this.frontWaterColors[1],this.frontWaterAlphas[1]);
         _loc4_.y = 1;
         param1.addChild(_loc4_).name = "base";
         return param1;
      }
      
      private function createWaterBase(param1:Number, param2:Number, param3:uint, param4:Number = 1) : Image
      {
         if(this.waterBaseBitmapData)
         {
            this.waterBaseBitmapData.dispose();
         }
         if(this.waterBaseImage)
         {
            this.waterBaseImage.texture.dispose();
         }
         this.waterBaseBitmapData = new BitmapData(3,3,false,param3);
         var _loc5_:Texture = Texture.fromBitmapData(this.waterBaseBitmapData,false);
         this.waterBaseImage = new Image(_loc5_);
         this.waterBaseImage.scale9Grid = WATER_SCALE9GRID;
         this.waterBaseImage.width = param1;
         this.waterBaseImage.height = param2;
         this.waterBaseImage.alpha = param4;
         return this.waterBaseImage;
      }
      
      private function createWaterTop(param1:Number, param2:Number, param3:uint, param4:Number = 1) : Image
      {
         if(this.waterTopBitmapData)
         {
            this.waterTopBitmapData.dispose();
         }
         if(this.waterTopImage)
         {
            this.waterTopImage.texture.dispose();
         }
         this.waterTopBitmapData = new BitmapData(3,param2,false,param3);
         var _loc5_:Texture = Texture.fromBitmapData(this.waterTopBitmapData,false);
         this.waterTopImage = new Image(_loc5_);
         this.waterTopImage.scale9Grid = WATER_SCALE9GRID;
         this.waterTopImage.width = param1;
         this.waterTopImage.alpha = param4;
         return this.waterTopImage;
      }
      
      private function createWave(param1:Sprite) : void
      {
         this.waterSpreadMethod = SpreadMethod.PAD;
         var _loc2_:String = param1 == this.frontWave ? this.FRONT : this.BACK;
         this.createCreast(param1,_loc2_);
      }
      
      public function increaseWaterLevel(param1:Number) : void
      {
         this.waterHeight += param1;
         this.y -= param1;
         this.summ -= param1;
         SystemUtil.executeWhenApplicationIsActive(this.createWave,this.frontWave);
      }
      
      private function destroyWaves() : void
      {
         if(this.waterBaseImage.parent)
         {
            this.waterBaseImage.parent.removeChild(this.waterBaseImage);
         }
         this.waterBaseImage.texture.dispose();
         this.waterBaseImage.dispose();
         this.waterBaseImage = null;
         this.waterBaseBitmapData.dispose();
         this.waterBaseBitmapData = null;
         if(this.waterTopImage.parent)
         {
            this.waterTopImage.parent.removeChild(this.waterBaseImage);
         }
         this.waterTopImage.texture.dispose();
         this.waterTopImage.dispose();
         this.waterTopImage = null;
         this.waterTopBitmapData.dispose();
         this.waterTopBitmapData = null;
      }
   }
}

