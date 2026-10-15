package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-22U§;
   import §_-l1g§.§_-L3Q§;
   import §_-qH§.§_-S25§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   
   public class VilePumpkin extends GenericGrenade
   {
      
      private static const DEBUFF_DAMAGE:§_-TA§ = new §_-TA§(10);
      
      private static const DEBUFF_DURATION:§_-TA§ = new §_-TA§(6);
      
      public function VilePumpkin(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Number, param6:uint)
      {
         super(param1,param2,param3,param4,param5,param6,0.5,150);
         bounceSounds = [§_-d27§.§_-I1v§];
         physParams.collidesWith |= PhysParams.COLLIDES_WITH_UNIT;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
      }
      
      protected function explode() : void
      {
         inflictDamage(null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
         disposed = true;
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,paramSet.damageRadius.value * 1.5,§_-d27§.§_-S14§);
      }
      
      override protected function onStopMoving(param1:Event) : void
      {
         super.onStopMoving(param1);
         this.explode();
      }
      
      private function onCollide(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         if(param1.data.other != null && param1.data.other is §_-01J§ && owner.squad.getTeamId() != §_-01J§(param1.data.other).squad.getTeamId())
         {
            _loc2_ = §_-01J§(param1.data.other);
            if(_loc2_.§_-vt§)
            {
               _loc2_.buffs.place(new §_-22U§(owner,DEBUFF_DURATION.value,DEBUFF_DAMAGE.value,§_-S25§.PUMPKIN_GREEN));
               disposed = true;
            }
            else
            {
               this.explode();
            }
         }
      }
   }
}

