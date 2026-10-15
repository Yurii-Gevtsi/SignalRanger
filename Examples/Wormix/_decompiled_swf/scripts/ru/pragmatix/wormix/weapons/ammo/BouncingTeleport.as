package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   
   public class BouncingTeleport extends GenericGrenade
   {
      
      private var goodLocation:Boolean = false;
      
      private var teleX:int;
      
      private var teleY:int;
      
      private var relocate:Boolean = false;
      
      public function BouncingTeleport(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint)
      {
         super(param1,param1.x,param1.y,param2,param3,param4,0.5,180);
      }
      
      override protected function initPhysicModel() : void
      {
         super.initPhysicModel();
         physParams.sinkable = owner.physParams.sinkable;
      }
      
      override protected function onStopMoving(param1:Event) : void
      {
         super.onStopMoving(param1);
         this.teleX = this.x;
         this.teleY = this.y - Physics.UNIT_HALF_HEIGHT - 10;
         this.prepareTeleport(10);
      }
      
      public function prepareTeleport(param1:uint) : void
      {
         if(!this.relocate)
         {
            this.relocate = true;
            if(armature)
            {
               armature.playAnimation(§_-K1t§.§_-J2e§);
            }
            §_-Ia§.§_-33o§(param1,this.startTeleport);
         }
      }
      
      private function startTeleport() : void
      {
         if(!owner.disposed && !disposed)
         {
            this.teleX = x;
            this.teleY = y - Physics.UNIT_HALF_HEIGHT - 10;
            owner.stable = false;
            owner.keepUnstable = true;
            this.goodLocation = true;
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,this.teleX,this.teleY,30);
            if(HitTest.unit(this.teleX,this.teleY))
            {
               §_-h2B§.play(§_-d27§.§_-S14§);
               §_-G4§.§_-Ee§(owner,this.teleX,this.teleY,null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value,false);
            }
            else
            {
               §_-h2B§.play(§_-d27§.TELEPORT);
            }
            if(HitTest.unit(this.teleX,this.teleY))
            {
               this.goodLocation = false;
            }
            if(!owner.disposed)
            {
               owner.keepUnstable = false;
            }
         }
         if(!disposed)
         {
            disposed = true;
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(§_-X1j§.§_-12n§.gameMaster)
         {
            if(!owner.disposed)
            {
               if(this.goodLocation)
               {
                  Physics.teleportUnit(owner as §_-F3o§,this.teleX,this.teleY,this.reattachTeleport);
               }
               owner.keepUnstable = false;
               this.reattachTeleport();
            }
         }
         super.defaultOnDisposed(param1);
      }
      
      override protected function defaultOnSink(param1:Event) : void
      {
         super.defaultOnSink(param1);
         this.reattachTeleport();
      }
      
      private function reattachTeleport() : void
      {
         var _loc2_:§_-01J§ = null;
         if(Boolean(owner) && Boolean(owner.weapon))
         {
            _loc2_ = owner;
            owner.weapon.detach();
            _loc2_.reattachWeapon();
         }
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc1_) && _loc1_.§_-42C§())
         {
            if(_loc1_.§_-E3b§())
            {
               ControlSets.UNIT_CONTROL_SET.activate();
            }
            §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = true;
         }
      }
   }
}

