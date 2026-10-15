package ru.pragmatix.wormix.fx
{
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.events.Event;
   
   public class DragonbonesAnimationEffect extends BaseEffect implements IPooledEffect
   {
      
      private var armature:§_-di§;
      
      public function DragonbonesAnimationEffect(param1:EffectType)
      {
         super(param1);
         this.createArmature();
      }
      
      private function createArmature() : void
      {
         if(this.armature)
         {
            if(this.armature.display)
            {
               this.armature.display.removeFromParent();
            }
            this.armature.dispose();
         }
         this.armature = §_-K1t§.buildArmature(_effectType.args[0],§_-K1t§.§_-G1E§,§_-K1t§.§_-B1s§);
         this.armature.invalidUpdate();
         this.armature.stopAnimation();
      }
      
      override public function init(param1:int, param2:int, param3:Array) : void
      {
         this.armature.display.x = param1;
         this.armature.display.y = param2;
         var _loc4_:Number = Number(_effectType.args[1]);
         this.armature.display.scaleX = this.armature.display.scaleY = 1 / _loc4_ * param3[0];
         WorldClock.clock.add(this.armature);
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(this.armature.display,SortingLayer.EFFECT_LAYER);
         this.armature.playAnimation(§_-K1t§.§_-G1E§);
         this.armature.addEventListener(§_-s1d§.COMPLETE,this.onAnimationCompleted);
         this.armature.display.addEventListener(Event.REMOVED_FROM_STAGE,this.clear);
      }
      
      private function clear(param1:Event) : void
      {
         this.remove();
      }
      
      override public function setDefault() : void
      {
         this.armature.removeEventListeners(§_-s1d§.COMPLETE);
         this.armature.stopAnimation();
         WorldClock.clock.remove(this.armature);
         if(this.armature.display)
         {
            this.armature.display.removeEventListeners(Event.REMOVED_FROM_STAGE);
            this.armature.display.removeFromParent();
            this.armature.display.visible = true;
         }
      }
      
      override public function dispose() : void
      {
         if(this.armature)
         {
            this.setDefault();
            this.armature.dispose();
            this.armature = null;
         }
      }
      
      override public function remove() : void
      {
         this.setDefault();
         _effectType.getObjectPool().putObject(this,_effectType);
      }
      
      private function onAnimationCompleted(param1:Event) : void
      {
         this.remove();
      }
   }
}

