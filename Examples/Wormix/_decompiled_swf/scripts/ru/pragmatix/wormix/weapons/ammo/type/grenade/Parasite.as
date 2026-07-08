package ru.pragmatix.wormix.weapons.ammo.type.grenade
{
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-93e§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.weapons.ammo.GenericGrenade;
   import starling.events.Event;
   
   public class Parasite extends GenericGrenade
   {
      
      protected var blinkClip:String;
      
      public function Parasite(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Number, param6:uint)
      {
         super(param1,param2,param3,param4,param5,param6,0.5,150);
         bounceSounds = [§_-d27§.§_-02z§];
         physParams.collidesWith |= PhysParams.COLLIDES_WITH_UNIT;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
      }
      
      public function setBlinkingClip(param1:String) : void
      {
         this.blinkClip = param1;
      }
      
      private function onCollide(param1:Event) : void
      {
         if(param1.data.other != null && param1.data.other is §_-01J§ && owner.squad.getTeamId() != §_-01J§(param1.data.other).squad.getTeamId() && !§_-01J§(param1.data.other).buffs.getBuff(§_-93e§.NAME))
         {
            §_-01J§(param1.data.other).buffs.place(this.getDebuff());
            inflictDamage(param1.data.other,paramSet.damage.value,0,0);
            disposed = true;
            §_-h2B§.play(§_-d27§.§_-T1F§);
         }
      }
      
      protected function getDebuff() : §_-93e§
      {
         return new §_-93e§(owner,Parasite,paramSet,this.blinkClip);
      }
      
      override protected function onStopMoving(param1:Event) : void
      {
         super.onStopMoving(param1);
         inflictDamage(null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,30,§_-d27§.§_-H1G§);
         disposed = true;
      }
   }
}

