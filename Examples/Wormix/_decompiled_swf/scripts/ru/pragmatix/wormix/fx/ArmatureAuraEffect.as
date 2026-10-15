package ru.pragmatix.wormix.fx
{
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-F0§;
   import §_-H2g§.§_-S2O§;
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class ArmatureAuraEffect extends AbstractAuraEffect implements §_-F0§
   {
      
      private static const IDLE_ANIM_NAME:String = "idle";
      
      private static const APPEAR_ANIM_NAME:String = "appear";
      
      private static const FADING_ANIM_NAME:String = "disappear";
      
      private static const ICON_BONE_NAME:String = "icon";
      
      private static const DEFAULT_ICON_Y_OFFSET:int = 55;
      
      private var armature:§_-di§;
      
      private var disposed:Boolean;
      
      private var iconIsUnderCharacter:Boolean;
      
      public function ArmatureAuraEffect(param1:§_-01J§, param2:String)
      {
         super(param1);
         this.armature = §_-K1t§.buildArmature(param2);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.onStartNewTurn);
         param1.addEventListener(§_-L3Q§.DISPOSED,this.removeAura);
         this.iconIsUnderCharacter = true;
         this.disposed = false;
      }
      
      private function removeAura(param1:Event) : void
      {
         this.dispose();
      }
      
      override public function onAppear() : void
      {
         this.armature.addEventListener(§_-s1d§.COMPLETE,this.onAppearAnimComplete);
         this.armature.playAnimation(APPEAR_ANIM_NAME,0,-1);
         owner.graphics.§_-oK§(1,this.armature.display,0,0,true,false);
         WorldClock.clock.add(this);
         this.onStartNewTurn();
      }
      
      private function onAppearAnimComplete(param1:Event) : void
      {
         this.armature.removeEventListener(§_-s1d§.COMPLETE,this.onAppearAnimComplete);
         this.armature.playAnimation(IDLE_ANIM_NAME);
      }
      
      override public function onFading() : void
      {
         if(this.armature)
         {
            if(!owner.disposed)
            {
               this.armature.playAnimation(FADING_ANIM_NAME);
               this.armature.addEventListener(§_-s1d§.COMPLETE,this.onAnimationComplete);
            }
            else
            {
               this.disposed = true;
            }
         }
      }
      
      private function onAnimationComplete(param1:Event) : void
      {
         param1.currentTarget.removeEventListener(§_-s1d§.COMPLETE,this.onAnimationComplete);
         this.disposed = true;
      }
      
      public function advanceTime(param1:Number) : void
      {
         if(this.disposed)
         {
            this.dispose();
         }
         else if(Boolean(this.armature) && Boolean(this.armature.display))
         {
            this.armature.advanceTime(param1);
         }
      }
      
      override public function dispose() : void
      {
         if(owner)
         {
            owner.removeEventListener(§_-L3Q§.DISPOSED,this.removeAura);
         }
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.onStartNewTurn);
         WorldClock.clock.remove(this);
         if(this.armature)
         {
            if(this.armature.hasEventListener(§_-s1d§.COMPLETE))
            {
               this.armature.removeEventListener(§_-s1d§.COMPLETE,this.onAppearAnimComplete);
            }
            if(this.armature.display)
            {
               this.armature.display.removeFromParent();
            }
            this.armature.dispose();
            this.armature = null;
         }
      }
      
      private function onStartNewTurn(param1:Event = null) : void
      {
         var _loc2_:Boolean = Boolean(§_-X1j§.§_-12n§.gameMaster) && §_-X1j§.§_-12n§.gameMaster.activeUnit == owner;
         if(_loc2_ && !this.iconIsUnderCharacter)
         {
            this.changeIconBoneOffset(DEFAULT_ICON_Y_OFFSET);
            this.iconIsUnderCharacter = true;
         }
         else if(!_loc2_ && this.iconIsUnderCharacter)
         {
            this.changeIconBoneOffset(-DEFAULT_ICON_Y_OFFSET);
            this.iconIsUnderCharacter = false;
         }
      }
      
      private function changeIconBoneOffset(param1:int) : void
      {
         var _loc2_:§_-S2O§ = this.armature.getBone(ICON_BONE_NAME);
         if(_loc2_)
         {
            this.armature.invalidUpdate();
         }
      }
   }
}

