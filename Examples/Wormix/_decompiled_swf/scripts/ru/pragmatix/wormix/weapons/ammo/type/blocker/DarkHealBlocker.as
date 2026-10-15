package ru.pragmatix.wormix.weapons.ammo.type.blocker
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-Y1b§.§_-OP§;
   import §_-b1F§.§_-G4§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import starling.events.Event;
   
   public class DarkHealBlocker extends HealBlocker
   {
      
      private static const DAMAGE:§_-TA§ = new §_-TA§(10);
      
      private static const MIN_HP_RATE:Number = 0.1;
      
      public function DarkHealBlocker(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:§_-TA§)
      {
         super(param1,param2,param3,param4,param5);
      }
      
      override protected function checkDuration(param1:Event) : void
      {
         var _loc2_:Boolean = false;
         var _loc3_:§_-01J§ = null;
         super.checkDuration(param1);
         if(!disposed)
         {
            _loc2_ = null != §_-OP§.§_-Ri§(LightHealBlocker,owner,false);
            if(!_loc2_)
            {
               for each(_loc3_ in §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner))
               {
                  if(_loc3_.attributes.hp.§_-uk§() > MIN_HP_RATE)
                  {
                     §_-G4§.inflictDamage(this,_loc3_ as §_-F3o§,DAMAGE.value,0,0,false,true);
                     Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DEMONIC,_loc3_.x,_loc3_.y,60);
                  }
               }
            }
         }
      }
   }
}

