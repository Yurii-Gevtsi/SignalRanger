package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Ia§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   
   public class StaticCharge extends §_-Qi§
   {
      
      private static const MIN_POWER:Number = 5;
      
      private static const VERTICAL_SPEED:Number = 23;
      
      private static const MAX_SPEED_DAMAGE:Number = 60;
      
      private static const JUMP_RADIUS:uint = 400;
      
      public function StaticCharge(param1:§_-01J§, param2:§_-C3e§, param3:int, param4:int, param5:Boolean)
      {
         var powers:Array;
         var units:Vector.<§_-01J§>;
         var jumpTargets:Array = null;
         var unit:§_-01J§ = null;
         var newJumpTarget:§_-01J§ = null;
         var sign:int = 0;
         var owner:§_-01J§ = param1;
         var paramSet:§_-C3e§ = param2;
         var x:int = param3;
         var y:int = param4;
         var jump:Boolean = param5;
         super(owner,x,y,false,§_-n18§,DamageType.SHOCK);
         this.x = x;
         this.y = y;
         powers = [];
         units = Physics.getUnitsInCircle(x,y,paramSet.damageRadius.value);
         if(units.length > 0 && jump)
         {
            jumpTargets = [];
            for each(unit in §_-X1j§.§_-12n§.gameMaster.§_-83I§())
            {
               if(units.indexOf(unit) == -1)
               {
                  jumpTargets.push(unit);
               }
            }
            newJumpTarget = Physics.nearestObjectToPoint(x,y,jumpTargets,JUMP_RADIUS) as §_-01J§;
            if(newJumpTarget)
            {
               Effects.lightningOnObjects(units[0] as §_-F3o§,newJumpTarget as §_-F3o§,15,5,13948116,6);
               units.push(newJumpTarget);
            }
         }
         for each(unit in units)
         {
            if(unit != owner)
            {
               inflictDamage(unit as §_-F3o§,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value,false);
               sign = §_-H3D§.sign(unit.x - owner.x);
               powers.push(sign * Math.max(MIN_POWER,Math.abs(unit.speedX)));
               §_-G4§.§_-u1v§(unit as §_-F3o§,0,-VERTICAL_SPEED,false);
               inflictDamage(unit as §_-F3o§,0,0,0,false);
               unit.rotateUntilStable();
               unit.graphics.§_-310§();
               unit.addEventListener(§_-L3Q§.§_-o13§,function(param1:Event):void
               {
                  var _loc3_:§_-01J§ = null;
                  var _loc4_:Number = NaN;
                  if(param1.data.other == null)
                  {
                     _loc3_ = §_-01J§(param1.currentTarget);
                     _loc3_.graphics.§_-41n§();
                     if(_loc3_.speed > 20)
                     {
                        _loc4_ = Math.min(_loc3_.speed,MAX_SPEED_DAMAGE);
                        §_-G4§.§_-Ee§(owner,_loc3_.x,_loc3_.y,_loc3_ as §_-F3o§,_loc4_,0,0,true);
                     }
                     _loc3_.removeEventListener(§_-L3Q§.§_-o13§,arguments.callee);
                  }
               });
            }
         }
         §_-Ia§.§_-33o§(2,this.pushDamagedUnits,units,powers);
      }
      
      private function pushDamagedUnits(param1:Vector.<§_-01J§>, param2:Array) : void
      {
         var _loc3_:§_-01J§ = null;
         for each(_loc3_ in param1)
         {
            if(_loc3_ != owner)
            {
               if(!_loc3_.disposed && !_loc3_.stable)
               {
                  §_-G4§.§_-u1v§(_loc3_ as §_-F3o§,param2.shift(),0,true);
               }
            }
         }
      }
   }
}

