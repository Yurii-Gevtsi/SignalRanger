package ru.pragmatix.wormix.weapons.ammo.type.blocker
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.phase.BlockHealPhase;
   import ru.pragmatix.wormix.weapons.ammo.phase.HarmPhase;
   import starling.events.Event;
   import starling.extensions.ParticleAura;
   
   public class HealBlocker extends AbstractBlocker
   {
      
      public function HealBlocker(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:§_-TA§)
      {
         super(param1,param2,param3,param4,param5);
      }
      
      public function healBlock(param1:§_-01J§) : void
      {
         if(§_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([BlockHealPhase]).length == 0)
         {
            new BlockHealPhase(param1,param1.x,param1.y,this,0);
            new ParticleAura("antitoxine","white_cross",param1 as §_-F3o§,30,null,this.onAuraCreated);
            Effects.aura(param1,"HealBrakeClip",40);
         }
      }
      
      private function onAuraCreated(param1:ParticleAura) : void
      {
         param1.particleSystem.alpha = 0.6;
      }
      
      override protected function onCollide(param1:Event) : void
      {
         destroyDuplicates(HealBlocker);
         super.onCollide(param1);
      }
      
      override protected function onDamaged(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         if(Boolean(param1.data.source) && Boolean(param1.data.source.owner) && !disposed)
         {
            _loc2_ = param1.data.source.owner;
            if(_loc2_.squad.getTeamId() != owner.squad.getTeamId())
            {
               new HarmPhase(owner,x,y,_loc2_,paramSet.damage.value);
            }
         }
         super.onDamaged(param1);
      }
   }
}

