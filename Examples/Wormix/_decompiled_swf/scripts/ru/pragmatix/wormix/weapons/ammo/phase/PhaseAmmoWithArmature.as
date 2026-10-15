package ru.pragmatix.wormix.weapons.ammo.phase
{
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   
   public class PhaseAmmoWithArmature extends PhaseAmmo
   {
      
      private var damage:uint = 0;
      
      private var armatureName:String;
      
      private var atlasName:String;
      
      public function PhaseAmmoWithArmature(param1:§_-01J§, param2:int, param3:int, param4:§_-F3o§, param5:String, param6:int = -1, param7:uint = 0, param8:Function = null)
      {
         if(param6 == -1)
         {
            param6 = 20;
         }
         this.armatureName = param5;
         this.atlasName = this.atlasName;
         this.callback = param8;
         this.damage = param7;
         super(param1,param2,param3,true,param4,0,param6);
      }
      
      override protected function initialize(param1:Point, param2:String = null, param3:Boolean = false, param4:String = "resurces/_ammo/") : void
      {
         this.x = param1.x;
         this.y = param1.y;
         armature = §_-K1t§.buildArmature(this.armatureName);
         WorldClock.clock.add(armature);
         armature.animation.play();
         armature.display.scaleY = target.x < x ? -1 : 1;
         clip = armature.display;
      }
      
      override protected function initTrail() : void
      {
      }
      
      override protected function processMovement() : void
      {
         var _loc1_:Number = NaN;
         var _loc2_:Number = NaN;
         if(Boolean(!disposed) && Boolean(target) && !target.disposed)
         {
            _loc1_ = Math.atan2(target.y - y,target.x - x);
            setSpeedVector(_loc1_,ammoSpeed);
            _loc2_ = MathUtil.distance(x,y,target.x,target.y);
            if(_loc2_ < 16)
            {
               this.doDamage();
               this.clear();
            }
         }
         else
         {
            this.clear();
         }
      }
      
      override protected function clear() : void
      {
         super.clear();
         disposed = true;
      }
      
      override protected function doDamage() : void
      {
         if(!isCleared)
         {
            if(callback != null)
            {
               callback();
               callback = null;
            }
            if(this.damage > 0)
            {
               inflictDamage(target,this.damage,0,0,false);
            }
         }
      }
   }
}

