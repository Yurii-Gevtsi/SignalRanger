package ru.pragmatix.wormix.weapons
{
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-j1w§.§_-L1x§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-u1m§.§_-61A§;
   import §_-w2i§.§_-Zd§;
   import §_-z1g§.§_-lp§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.ZombieGrave;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-c2a§;
   import starling.events.Event;
   
   public class ZombieMarch extends §_-61A§ implements §_-c2a§
   {
      
      public function ZombieMarch()
      {
         super();
      }
      
      override protected function §_-D2a§(param1:Function) : void
      {
         §_-z1W§(§_-vO§.§_-Bu§(param1));
      }
      
      override protected function §_-B1T§(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:Number, param6:Number) : void
      {
         var _loc7_:int = -1;
         while(_loc7_ <= 1)
         {
            this.§_-hR§(param3 + 120 * _loc7_,param4);
            _loc7_++;
         }
         super.§_-B1T§(param1,param2,param3,param4,param5,param6);
      }
      
      override protected function §_-22V§(param1:int, param2:int, param3:int) : Number
      {
         return param2;
      }
      
      override protected function §_-h2d§(param1:Number, param2:Number, param3:Boolean) : §_-di§
      {
         return Effects.putSimpleDisappearingCrossMark(param1,param2);
      }
      
      private function §_-hR§(param1:int, param2:int) : void
      {
         var name:String = null;
         var level:int = 0;
         var grave:ZombieGrave = null;
         var nextTurn:Boolean = false;
         var x:int = param1;
         var y:int = param2;
         name = §_-s2a§.§_-K3t§("RACE_ZOMBIE_NAME");
         level = Math.min(owner.record.getLevel(),§_-lp§.getMaxLevel());
         grave = new ZombieGrave(§_-L1x§.§_-p21§.§_-wz§().graveType,x,y,owner,name,level,owner.squad,0.5);
         grave.§_-E5§(UserProfile.custom(name,level,§_-L1x§.§_-p21§,18 + 2 * level,level,level));
         nextTurn = false;
         §_-Zd§.once(grave,§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            §_-G4§.§_-Ee§(grave.§_-l2Z§(),grave.x,grave.y,null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value,true);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,grave.x,grave.y,paramSet.explosionRadius.value,§_-d27§.§_-S14§);
            grave.setSpeedXY(0,-4);
            if(!nextTurn)
            {
               if(owner)
               {
                  owner.§_-01D§();
               }
               nextTurn = true;
               nextTurnOnStable();
            }
         });
      }
   }
}

