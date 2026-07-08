package ru.pragmatix.wormix.weapons
{
   import §_-Vg§.*;
   import §_-g1P§.§_-L38§;
   import §_-l1K§.§_-cZ§;
   import §_-x2Q§.§_-dU§;
   import feathers.themes.FontColor;
   import flash.geom.ColorTransform;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.Rope;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.utils.Color;
   
   public class SupplyRopeGun extends RopeGun implements §_-O1c§
   {
      
      public function SupplyRopeGun(param1:uint = 0)
      {
         super(param1);
      }
      
      override protected function §_-h2e§(param1:Number) : void
      {
         var _loc3_:§_-L38§ = null;
         var _loc2_:int = int(MAX_POWER);
         if(shotParams.§_-R18§)
         {
            _loc3_ = this.§_-O2v§();
            if(_loc3_)
            {
               param1 = shotParams.angle;
               _loc2_ = int(MIN_POWER);
            }
         }
         rope = new Rope(owner,paramSet,param1,_loc2_,ropeLength.value,null,true);
         rope.disposeOnPicked = _loc2_ == MIN_POWER ? _loc3_ : null;
      }
      
      private function §_-O2v§() : §_-L38§
      {
         var _loc5_:§_-F3o§ = null;
         var _loc1_:Number = MathUtil.PI_HALF;
         var _loc2_:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([§_-L38§]);
         var _loc3_:Vector.<§_-F3o§> = new Vector.<§_-F3o§>();
         var _loc4_:Vector.<§_-F3o§> = §_-K3W§ ? Physics.getGameObjectsInCone(owner.x,owner.y,ropeLength.value,-_loc1_,_loc1_,_loc2_) : Physics.getGameObjectsInCircle(owner.x,owner.y,ropeLength.value,_loc2_);
         for each(_loc5_ in _loc4_)
         {
            if(!HitTest.castRay(owner.x,owner.y,_loc5_.x - owner.x,_loc5_.y - owner.y))
            {
               _loc3_.push(_loc5_);
            }
         }
         if(_loc3_.length > 0)
         {
            _loc5_ = _loc3_[0];
            shotParams.angle = MathUtil.§_-02X§(owner.x,owner.y,_loc5_.x,_loc5_.y);
            if(!§_-K3W§)
            {
               owner.addSpeedXY(0,1);
            }
            owner.turn(owner.x > _loc5_.x ? Direction.LEFT.value : Direction.RIGHT.value);
            return _loc5_ as §_-L38§;
         }
         return null;
      }
   }
}

