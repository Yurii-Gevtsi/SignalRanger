package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.feats.HomingFeat;
   import ru.pragmatix.wormix.weapons.ammo.feats.TargetSeekerFeat;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class IceShard extends §_-Qi§
   {
      
      protected var paramSet:§_-C3e§;
      
      public function IceShard(param1:§_-01J§, param2:Number, param3:Number, param4:§_-C3e§, param5:Number, param6:uint)
      {
         super(param1,param2,param3,false);
         §_-In§ = DamageType.COLD;
         §_-92S§ = false;
         this.paramSet = param4;
         initialize(Pool.getPoint(param2,param3),param4.ammoClassName);
         setSpeedVector(param5,param6);
         if(§_-z1H§)
         {
            §_-z1H§.rotation = param5;
         }
         §_-z1H§.scaleX = §_-z1H§.scaleY = §_-z1H§.scaleY * 0.8;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-U1o§ = true;
         physParams.addForce(Force.WIND);
         physParams.optimizedMovement = false;
         physParams.ignoresTerrain = true;
         this.x = param2;
         this.y = param3;
         var _loc7_:TargetSeekerFeat = new TargetSeekerFeat(this,200,3);
         var _loc8_:HomingFeat = new HomingFeat(this,20,MathUtil.PI_ONE_30);
         _loc8_.setTargetSource(_loc7_);
         §_-P1P§.push(_loc7_,_loc8_);
      }
      
      private function onCollide(param1:Event) : void
      {
         var _loc2_:§_-F3o§ = null;
         var _loc4_:§_-01J§ = null;
         _loc2_ = param1.data.other;
         var _loc3_:Number = 1 * this.paramSet.damage.value;
         _loc4_ = _loc2_ as §_-01J§;
         var _loc5_:Boolean = (Boolean(_loc4_)) && §_-X1j§.§_-12n§.gameMaster.§_-b2W§(owner).indexOf(_loc4_) != -1;
         if(!_loc5_)
         {
            inflictDamage(param1.data.other,_loc3_,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value,true);
         }
         disposed = true;
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,x,y,this.paramSet.damageRadius.value / 4,§_-d27§.§_-T1j§);
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
      }
   }
}

