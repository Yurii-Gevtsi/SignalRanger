package ru.pragmatix.wormix.weapons
{
   import §_-G2H§.*;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   
   public class LaserActivatorDestructible extends §_-Qi§
   {
      
      public function LaserActivatorDestructible(param1:§_-01J§, param2:Point)
      {
         super(param1,param2.x,param2.y,false,-1);
         §_-Nt§ = true;
         physParams.collideType = CollideType.OTHER;
         stable = true;
         initialize(param2,"LaserActivatorDestructible",false,"");
         WorldClock.clock.add(armature);
         armature.animation.play();
         this._width = 30;
         this._height = 30;
      }
   }
}

