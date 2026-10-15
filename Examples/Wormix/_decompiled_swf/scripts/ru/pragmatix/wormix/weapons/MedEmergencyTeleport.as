package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import §_-Y1b§.§_-OP§;
   import §_-x22§.§_-xY§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.utils.Pool;
   
   public class MedEmergencyTeleport extends EmergencyTeleport implements §_-O1c§
   {
      
      public function MedEmergencyTeleport()
      {
         super();
      }
      
      override protected function §_-At§() : Point
      {
         var _loc3_:Vector.<§_-F3o§> = null;
         var _loc4_:§_-xY§ = null;
         var _loc5_:Point = null;
         var _loc6_:§_-xY§ = null;
         var _loc1_:Boolean = property["toMedkit"] ? Boolean(property["toMedkit"].value) : false;
         var _loc2_:uint = uint(§_-TA§(getProperty("teleportRadius")).value);
         if(_loc1_ && !shotParams.§_-R18§)
         {
            _loc3_ = Physics.getGameObjectsInCircle(owner.x,owner.y,_loc2_,§_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([§_-xY§]));
            for each(_loc6_ in _loc3_)
            {
               _loc5_ = Pool.getPoint(_loc6_.x,_loc6_.y);
               if(§_-OP§.§_-ig§(_loc5_,owner) && (!_loc4_ || _loc4_.§_-F22§() < _loc6_.§_-F22§()))
               {
                  _loc4_ = _loc6_;
               }
               Pool.putPoint(_loc5_);
            }
            if(_loc4_)
            {
               return Pool.getPoint(_loc4_.x,_loc4_.y - Physics.UNIT_HALF_HEIGHT);
            }
         }
         return super.§_-At§();
      }
   }
}

