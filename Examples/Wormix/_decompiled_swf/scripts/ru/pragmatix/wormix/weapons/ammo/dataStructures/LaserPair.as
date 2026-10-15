package ru.pragmatix.wormix.weapons.ammo.dataStructures
{
   import §_-L1H§.§_-C3e§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import starling.utils.Pool;
   
   public class LaserPair
   {
      
      public var owner:§_-01J§;
      
      public var paramSet:§_-C3e§;
      
      public var firstActivator:§_-F3o§;
      
      public var secondActivator:§_-F3o§;
      
      public var firstActivatorPos:Point;
      
      public var secondActivatorPos:Point;
      
      public var color:uint;
      
      private var disposeActivators:Boolean;
      
      public function LaserPair(param1:§_-01J§, param2:§_-C3e§, param3:§_-F3o§, param4:§_-F3o§, param5:uint = 0, param6:Boolean = true)
      {
         super();
         this.owner = param1;
         this.paramSet = param2;
         this.firstActivator = param3;
         this.secondActivator = param4;
         this.firstActivatorPos = Pool.getPoint(param3.x,param3.y);
         this.secondActivatorPos = Pool.getPoint(param4.x,param4.y);
         this.color = param5;
         this.disposeActivators = param6;
      }
      
      public function isActive() : Boolean
      {
         return this.firstActivator && this.secondActivator && !this.firstActivator.disposed && !this.secondActivator.disposed;
      }
      
      public function isDamaged() : Boolean
      {
         return Boolean(this.firstActivator) && this.firstActivator.disposed || Boolean(this.secondActivator) && this.secondActivator.disposed;
      }
      
      public function destroy() : void
      {
         if(Boolean(this.disposeActivators) && Boolean(this.firstActivator) && Boolean(this.secondActivator))
         {
            this.firstActivator.disposed = this.secondActivator.disposed = true;
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,this.firstActivator.x,this.firstActivator.y);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,this.secondActivator.x,this.secondActivator.y);
         }
         this.disposeActivators = false;
         this.owner = null;
         this.paramSet = null;
         this.firstActivator = null;
         this.secondActivator = null;
         Pool.putPoint(this.firstActivatorPos);
         Pool.putPoint(this.secondActivatorPos);
         this.firstActivatorPos = null;
         this.secondActivatorPos = null;
      }
      
      public function updateActivatorsPosition() : void
      {
         if(Boolean(this.firstActivator) && !this.firstActivator.disposed)
         {
            this.firstActivatorPos.setTo(this.firstActivator.x,this.firstActivator.y);
         }
         if(Boolean(this.secondActivator) && !this.secondActivator.disposed)
         {
            this.secondActivatorPos.setTo(this.secondActivator.x,this.secondActivator.y);
         }
      }
   }
}

