package ru.pragmatix.wormix.fx
{
   import §_-H2g§.§_-F0§;
   import com.greensock.TweenLite;
   import dragonBones.animation.WorldClock;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   
   public class TextEffect extends Sprite implements IPooledEffect, §_-F0§
   {
      
      private static var textObjectsColorsVector:Vector.<uint> = new Vector.<uint>(0);
      
      protected var _effectType:EffectType;
      
      private var textObject:ITextRenderer;
      
      private var lifetime:Number = 0;
      
      private var curLifetime:Number = 0;
      
      private var currentFontName:String;
      
      private var currentColor:uint;
      
      public function TextEffect(param1:EffectType)
      {
         super();
         this._effectType = param1;
         this.textObject = §_-R2g§.createTextRenderer(BitmapFonts.GROBOLD_WITH_SHADOWS,16,16777215);
         this.currentFontName = BitmapFonts.GROBOLD_WITH_SHADOWS;
         this.currentColor = 16777215;
         addChild(this.textObject as DisplayObject);
      }
      
      public function init(param1:int, param2:int, param3:Array) : void
      {
         this.lifetime = param3[3];
         var _loc4_:String = param3[4] != undefined ? param3[4] : BitmapFonts.GROBOLD_WITH_SHADOWS;
         var _loc5_:int = int(param3[1]);
         this.currentColor = param3[2];
         var _loc6_:String = param3[0];
         if(this.currentFontName != _loc4_)
         {
            this.currentFontName = _loc4_;
            (this.textObject as DisplayObject).removeFromParent(true);
            this.textObject = §_-R2g§.createTextRenderer(_loc4_,_loc5_,this.currentColor);
            addChild(this.textObject as DisplayObject);
         }
         else
         {
            §_-R2g§.§_-g1H§(this.textObject,null,_loc5_,this.currentColor);
         }
         this.textObject.text = _loc6_;
         x = param1;
         y = param2;
         WorldClock.clock.add(this);
         var _loc7_:int = textObjectsColorsVector.indexOf(this.currentColor);
         if(_loc7_ < 0)
         {
            _loc7_ = int(textObjectsColorsVector.length);
         }
         textObjectsColorsVector[_loc7_] = this.currentColor;
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(this,SortingLayer.TEXT_EFFECT_LAYER,_loc7_);
         (this.textObject as FeathersControl).invalidate(FeathersControl.INVALIDATION_FLAG_ALL);
         visible = true;
      }
      
      public function setDefault() : void
      {
         x = -500;
         y = -500;
         this.curLifetime = 0;
         this.lifetime = 0;
         WorldClock.clock.remove(this);
         if(§_-X1j§.§_-12n§.scene)
         {
            §_-X1j§.§_-12n§.scene.removeElement(this);
         }
         textObjectsColorsVector.splice(textObjectsColorsVector.indexOf(this.currentColor),1);
      }
      
      public function remove() : void
      {
         TweenLite.killTweensOf(this);
         this.setDefault();
         this._effectType.getObjectPool().putObject(this,this._effectType);
      }
      
      override public function dispose() : void
      {
         this.remove();
      }
      
      public function advanceTime(param1:Number) : void
      {
         if(this.curLifetime > this.lifetime)
         {
            this.remove();
         }
         else
         {
            this.curLifetime += param1;
         }
      }
      
      public function attach(param1:§_-F3o§, param2:Array) : void
      {
      }
   }
}

