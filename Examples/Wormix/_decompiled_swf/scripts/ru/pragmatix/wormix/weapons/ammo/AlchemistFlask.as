package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-W§.§_-Ap§;
   import §_-gG§.§_-Z8§;
   import §_-j2R§.§_-v1T§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import starling.events.Event;
   
   public class AlchemistFlask extends §_-Qi§
   {
      
      public function AlchemistFlask(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         super(param1,param1.x,param1.y,false);
         initialize(param1.weapon.getBarrelPoint(param3),param2.ammoClassName);
         setSpeedVector(param3,param4);
         §_-32§ = §_-h2D§;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-h2B§.play(§_-d27§.§_-G3Z§);
      }
      
      private function trigger(param1:§_-01J§) : void
      {
         var _loc2_:§_-v1T§ = param1.buffs;
         if(!_loc2_.hasBuff(§_-Ap§.NAME))
         {
            if(!_loc2_.hasBuff(§_-Z8§.NAME))
            {
               _loc2_.place(new §_-Z8§(owner));
            }
            else
            {
               _loc2_.§_-e1z§(§_-Z8§.NAME);
            }
         }
      }
      
      protected function onCollide(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         if(!disposed)
         {
            _loc2_ = param1.data.other as §_-01J§;
            if(_loc2_)
            {
               this.trigger(_loc2_);
            }
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,x,y,30,§_-d27§.§_-Q2F§);
            disposed = true;
         }
      }
   }
}

