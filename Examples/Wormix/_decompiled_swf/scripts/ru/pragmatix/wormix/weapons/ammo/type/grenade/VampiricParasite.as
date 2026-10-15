package ru.pragmatix.wormix.weapons.ammo.type.grenade
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-93e§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.phase.HealPhase;
   
   public class VampiricParasite extends Parasite
   {
      
      private static const MAX_HEALTH_GAIN:§_-TA§ = new §_-TA§(100);
      
      public function VampiricParasite(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Number, param6:uint)
      {
         super(param1,param2,param3,param4,param5,param6);
      }
      
      override public function inflictDamage(param1:§_-F3o§, param2:Number, param3:Number, param4:Number, param5:Boolean = true, param6:Boolean = false) : void
      {
         var _loc7_:§_-01J§ = null;
         var _loc8_:int = 0;
         if(param1 != null && param1 is §_-01J§)
         {
            _loc7_ = §_-01J§(param1);
            _loc8_ = _loc7_.hp;
         }
         super.inflictDamage(param1,param2,param3,param4,param5,param6);
         if(Boolean(_loc7_ && _loc7_.disposed) && Boolean(owner) && !owner.disposed)
         {
            new HealPhase(owner,owner.x,owner.y,owner,Math.min(_loc8_,MAX_HEALTH_GAIN.value),true);
         }
      }
      
      override protected function getDebuff() : §_-93e§
      {
         return new §_-93e§(owner,VampiricParasite,paramSet,blinkClip);
      }
   }
}

