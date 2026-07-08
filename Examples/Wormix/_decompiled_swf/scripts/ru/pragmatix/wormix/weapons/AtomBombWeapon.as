package ru.pragmatix.wormix.weapons
{
   import §_-B2z§.*;
   import §_-F39§.§_-a2W§;
   import §_-L1H§.§_-C3e§;
   import §_-r1C§.§_-h2B§;
   import §_-u1m§.§_-61A§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   
   public class AtomBombWeapon extends §_-61A§ implements §_-jm§
   {
      
      public function AtomBombWeapon()
      {
         super(false);
      }
      
      override protected function §_-V1B§(param1:Number, param2:Number) : Boolean
      {
         var _loc4_:§_-7o§ = null;
         var _loc3_:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([§_-7o§]);
         for each(_loc4_ in _loc3_)
         {
            if(_loc4_.owner.getId() == owner.getId())
            {
               §_-h2B§.play(§_-d27§.§_-b2H§);
               if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
               {
                  §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.ATOM_MARK_ALREADY_EXIST),true);
               }
               return false;
            }
         }
         return super.§_-V1B§(param1,param2);
      }
      
      override protected function §_-B1T§(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:Number, param6:Number) : void
      {
         new §_-7o§(param1,param2,shotParams.point.x,shotParams.point.y,param3,param4,param5,param6);
      }
   }
}

