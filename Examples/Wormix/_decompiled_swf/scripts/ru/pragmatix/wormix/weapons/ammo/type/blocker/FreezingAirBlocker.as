package ru.pragmatix.wormix.weapons.ammo.type.blocker
{
   import §_-12a§.§_-61x§;
   import §_-12a§.§_-c8§;
   import §_-L1H§.§_-C3e§;
   import §_-YF§.§_-q2v§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.phase.HomingFrostPhase;
   import starling.events.Event;
   
   public class FreezingAirBlocker extends AirBlocker
   {
      
      private static const EXTINGUISH_LIMIT:uint = 12;
      
      private static const SMALL_RADIUS_EXTINGUISH_INTERVAL:uint = 3;
      
      private static const LARGE_RADIUS_EXTINGUISH_INTERVAL:uint = 8;
      
      private static const CLASSES:Array = [BurningOil,Bullet];
      
      public function FreezingAirBlocker(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         super(param1,param2,param3,param4,§_-q2v§.§_-J2M§);
         debuffClass = §_-61x§;
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(active)
         {
            return;
         }
         if(!stable)
         {
            return;
         }
         if(Application.scene.gameObjectProcessor.§_-O1C§() % DETECTION_PERIOD == 0)
         {
            if(frames % SMALL_RADIUS_EXTINGUISH_INTERVAL == 0)
            {
               this.unfreezeAllies(radius);
            }
         }
      }
      
      override protected function checkTargets() : void
      {
         super.checkTargets();
         if(frames % LARGE_RADIUS_EXTINGUISH_INTERVAL == 0)
         {
            this.extinguishFire(radius * 2);
         }
         else if(frames % SMALL_RADIUS_EXTINGUISH_INTERVAL == 0)
         {
            this.extinguishFire(radius);
         }
      }
      
      private function extinguishFire(param1:uint) : void
      {
         var _loc2_:Vector.<§_-F3o§> = null;
         var _loc5_:§_-F3o§ = null;
         _loc2_ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§(CLASSES);
         var _loc3_:Vector.<§_-F3o§> = Physics.getGameObjectsInCircle(x,y,param1,_loc2_);
         var _loc4_:uint = 0;
         for each(_loc5_ in _loc3_)
         {
            if(_loc5_ is BurningOil || (_loc5_ as Bullet).damageType == DamageType.FIRE)
            {
               _loc5_.disposed = true;
               if(_loc5_ is Bullet && (_loc5_ as Bullet).damageType == DamageType.FIRE)
               {
                  Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,_loc5_.x,_loc5_.y,10);
               }
               if(++_loc4_ >= EXTINGUISH_LIMIT)
               {
                  break;
               }
            }
         }
      }
      
      private function unfreezeAllies(param1:uint) : void
      {
         var _loc2_:Vector.<§_-01J§> = null;
         var _loc4_:§_-8K§ = null;
         _loc2_ = §_-X1j§.§_-12n§.gameMaster.§_-b2W§(owner,false,true);
         var _loc3_:Vector.<§_-01J§> = Physics.getUnitsInCircle(x,y,param1,_loc2_.concat());
         for each(_loc4_ in _loc3_)
         {
            if(!_loc4_.buffs.hasBuff(§_-c8§.NAME))
            {
               _loc4_.buffs.place(new §_-c8§());
            }
         }
         for each(_loc4_ in _loc2_)
         {
            if(_loc3_.indexOf(_loc4_) == -1 && _loc4_.buffs.hasBuff(§_-c8§.NAME))
            {
               _loc4_.buffs.§_-e1z§(§_-c8§.NAME);
            }
         }
      }
      
      private function releaseAllies() : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc1_:Vector.<§_-01J§> = §_-X1j§.§_-12n§.gameMaster.§_-b2W§(owner,false,true);
         for each(_loc2_ in _loc1_)
         {
            if(_loc2_.buffs.hasBuff(§_-c8§.NAME))
            {
               _loc2_.buffs.§_-e1z§(§_-c8§.NAME);
            }
         }
      }
      
      override protected function onDamaged(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc3_:HomingFrostPhase = null;
         var _loc4_:int = 0;
         if(Boolean(param1.data.source) && Boolean(param1.data.source.owner) && !disposed)
         {
            _loc2_ = param1.data.source.owner;
            if(_loc2_.squad.getTeamId() != owner.squad.getTeamId())
            {
               _loc3_ = new HomingFrostPhase(owner,this.x,this.y,false,_loc2_,paramSet.damage.value);
               _loc4_ = 30;
               _loc3_.setSpeedXY(0,-_loc4_);
               _loc3_.homingFeat.ammoSpeed = _loc4_;
               _loc3_.homingFeat.superHomingDelay = 1;
               _loc3_.homingFeat.delayBeforeAim = 0;
            }
         }
         disposed = true;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(!Application.scene.gameObjectProcessor.§_-C6§())
         {
            this.releaseAllies();
         }
         super.defaultOnDisposed(param1);
      }
   }
}

