package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-Y1b§.§_-OP§;
   import §_-l1K§.§_-cZ§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import com.mesmotronic.ane.AndroidID;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseToPointAmmo;
   import ru.pragmatix.wormix.weapons.interfaces.§_-b29§;
   import starling.utils.Pool;
   
   public class EmergencyTeleport extends Weapon implements §_-b29§
   {
      
      public function EmergencyTeleport()
      {
         super();
         §_-a1R§.value = false;
         §_-N1N§ = false;
         §_-z1W§(new §_-cZ§());
      }
      
      public function jumpShoot(param1:Boolean) : void
      {
         if(!§_-X1j§.§_-12n§.gameMaster.§_-Rk§() && !§_-X1j§.§_-12n§.gameMaster.§_-U2O§())
         {
            shootWithParams(shotParams);
         }
      }
      
      override protected function shoot() : void
      {
         var _loc1_:Point = this.§_-At§();
         var _loc2_:PhaseToPointAmmo = new PhaseToPointAmmo(owner,owner.x,owner.y,true,_loc1_,16777215,60);
         _loc2_.§_-C2P§ = false;
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,owner.x,owner.y,20);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,_loc1_.x,_loc1_.y,20);
         §_-h2B§.play(§_-d27§.EMERGENCY_TELEPORT);
         Physics.teleportUnit(owner as §_-F3o§,_loc1_.x,_loc1_.y);
         var _loc3_:§_-01J§ = owner;
         §_-X1j§.§_-12n§.gameMaster.§_-c2w§();
         §_-Zd§.§_-33o§(1,this.§_-J3t§,_loc3_);
         detach();
      }
      
      protected function §_-At§() : Point
      {
         var _loc5_:Point = null;
         var _loc6_:int = 0;
         var _loc1_:uint = uint(§_-TA§(getProperty("teleportRadius")).value);
         var _loc2_:Boolean = §_-7u§(getProperty("mindDangerObj")).value;
         var _loc3_:uint = 15;
         var _loc4_:int = 82;
         while(!_loc5_)
         {
            _loc6_ = MathUtil.§_-na§(_loc4_,_loc1_);
            _loc5_ = §_-OP§.§_-63A§(owner.x + owner.direction * _loc6_,owner.y,_loc1_,_loc1_ / 2,Physics.UNIT_HALF_HEIGHT * 3,5,_loc2_);
            _loc4_ -= _loc3_;
            if(_loc4_ <= 0)
            {
               break;
            }
         }
         if(!_loc5_)
         {
            _loc5_ = Pool.getPoint(owner.x,owner.y);
         }
         return _loc5_;
      }
      
      private function §_-J3t§(param1:§_-01J§) : void
      {
         if(§_-X1j§.§_-12n§.gameMaster.activeUnit == param1)
         {
            §_-X1j§.§_-12n§.gameMaster.§_-c2w§(false);
            if(§_-X1j§.§_-12n§.gameMaster.§_-42C§())
            {
               param1.reattachWeapon();
               §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = true;
            }
         }
      }
   }
}

