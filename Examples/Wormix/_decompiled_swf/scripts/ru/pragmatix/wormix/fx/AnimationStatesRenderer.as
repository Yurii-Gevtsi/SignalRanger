package ru.pragmatix.wormix.fx
{
   import §_-H2g§.§_-di§;
   import §_-hO§.§_-03k§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.utils.§_-13j§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   
   public class AnimationStatesRenderer
   {
      
      private static const STATE_APPEAR:String = "appear";
      
      private static const STATE_IDLE:String = "idle";
      
      private static const STATE_FADING:String = "fading";
      
      private var state:String;
      
      private var targetClip:DisplayObjectContainer;
      
      private var isPlaying:Boolean = false;
      
      private var timer:§_-13j§;
      
      private var armature:§_-di§;
      
      private var fadeAddedToScene:Boolean = false;
      
      private var className:String;
      
      public function AnimationStatesRenderer(param1:DisplayObjectContainer, param2:String)
      {
         super();
         this.targetClip = param1;
         this.className = param2;
         this.timer = new §_-13j§(0,1);
         this.timer.addEventListener(§_-03k§.§_-cE§,this.onFrameTimerComplete);
      }
      
      public function start() : void
      {
         if(this.isPlaying)
         {
            throw new Error("Already playing!");
         }
         this.isPlaying = true;
         this.armature = §_-K1t§.buildArmature(this.className);
         WorldClock.clock.add(this.armature);
         if(this.armature.animation.hasAnimation(§_-K1t§.§_-XN§))
         {
            this.playAnimation(§_-K1t§.§_-XN§);
            this.addAnimation();
            this.state = STATE_APPEAR;
            this.timer.restart(this.getTotalFrames(§_-K1t§.§_-XN§));
         }
         else
         {
            this.idlePlay();
         }
      }
      
      private function playAnimation(param1:String) : void
      {
         this.armature.playAnimation(param1);
      }
      
      private function idlePlay() : void
      {
         this.playAnimation(§_-K1t§.§_-G1E§);
         this.addAnimation();
         this.state = STATE_IDLE;
      }
      
      public function fade(param1:Boolean = false) : void
      {
         if(this.state == STATE_FADING)
         {
            return;
         }
         if(this.armature.animation.hasAnimation(§_-K1t§.§_-s1g§))
         {
            this.fadeAddedToScene = param1;
            this.playAnimation(§_-K1t§.§_-s1g§);
            if(param1)
            {
               this.targetClip.parent.addChildAt(this.armature.display,this.targetClip.parent.getChildIndex(this.targetClip));
               this.armature.display.x = this.targetClip.x;
               this.armature.display.y = this.targetClip.y;
            }
            else
            {
               this.addAnimation();
            }
            this.timer.restart(this.getTotalFrames(§_-K1t§.§_-s1g§));
            this.state = STATE_FADING;
         }
      }
      
      public function stop() : void
      {
         this.destroyAnimation();
      }
      
      private function onFrameTimerComplete(param1:§_-03k§) : void
      {
         switch(this.state)
         {
            case STATE_APPEAR:
               this.idlePlay();
               break;
            case STATE_IDLE:
               break;
            case STATE_FADING:
               if(this.fadeAddedToScene)
               {
                  this.armature.display.removeFromParent();
               }
               else
               {
                  this.deleteAnimation();
               }
               this.fadeAddedToScene = false;
               this.destroyAnimation();
         }
      }
      
      private function addAnimation() : void
      {
         this.armature.display.name = "animation";
         this.targetClip.addChild(this.armature.display);
      }
      
      private function deleteAnimation() : void
      {
         if(Boolean(this.armature) && Boolean(this.armature.display) && this.armature.display.name == "animation")
         {
            WorldClock.clock.remove(this.armature);
            this.armature.display.removeFromParent(true);
            this.armature = null;
         }
      }
      
      private function destroyAnimation() : void
      {
         this.targetClip = null;
         this.isPlaying = false;
      }
      
      public function getClip() : DisplayObject
      {
         return this.armature.display;
      }
      
      private function getTotalFrames(param1:String) : int
      {
         return this.armature.animation.getState(param1).totalTime * 30;
      }
   }
}

