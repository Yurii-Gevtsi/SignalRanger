package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import §_-b1F§.§_-G4§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-K3V§ extends Auger
   {
      
      private const §_-i2O§:§_-TA§ = new §_-TA§(30);
      
      public function §_-K3V§()
      {
         super();
      }
      
      override protected function shoot() : void
      {
         var _loc5_:§_-F3o§ = null;
         var _loc6_:§_-Qi§ = null;
         var _loc1_:Point = MathUtil.§_-UW§(this.§_-i2O§.value + this.§_-i2O§.value / 2 - 10,clip.rotation);
         var _loc2_:Point = Pool.getPoint(owner.x,owner.y);
         var _loc3_:Point = _loc1_.add(_loc2_);
         §_-X1j§.§_-12n§.scene.drawDamage(_loc3_.x,_loc3_.y,this.§_-i2O§.value);
         var _loc4_:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-a2S§();
         for each(_loc5_ in Physics.getGameObjectsInCircle(_loc3_.x,_loc3_.y,this.§_-i2O§.value + 5,_loc4_))
         {
            if(_loc5_ != owner)
            {
               _loc6_ = new §_-Qi§(owner,_loc5_.x,_loc5_.y,false,1);
               _loc6_.x = _loc5_.x;
               _loc6_.y = _loc5_.y;
               §_-G4§.inflictDamage(_loc6_,null,DRILL_DAMAGE,5,5);
            }
         }
         detach();
         nextTurnOnStable();
         Pool.putPoint(_loc1_);
         Pool.putPoint(_loc2_);
         Pool.putPoint(_loc3_);
      }
      
      override protected function onFrame(param1:Event) : void
      {
      }
   }
}

