package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31Y§.§_-D3B§;
   import §_-L1H§.§_-C3e§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   
   public class RopeHarpoon extends Rope
   {
      
      public function RopeHarpoon(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:Point)
      {
         super(param1,param2,param3,param4,0,param5);
         tags = new <§_-D3B§>[§_-D3B§.§_-J1i§];
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.NONE);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND + PhysParams.COLLIDES_WITH_UNIT;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
   }
}

