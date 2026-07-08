package ru.pragmatix.wormix.weapons.ammo.type.mine
{
   import §_-31N§.§_-42G§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.Rope;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class PushingMine extends Mine
   {
      
      private static const IGNORE_OBJECTS:Array = [Rope,StickyGuardingBot];
      
      private static const SEARCH_RADIUS:uint = 100;
      
      private static const SEARCH_ANGLE:Number = -MathUtil.PI_HALF;
      
      private static const SEARCH_ANGLE_WIDTH:Number = Math.PI / 6;
      
      private var target:§_-01J§;
      
      private var dummy:§_-F3o§;
      
      private var lifetime:int;
      
      private var pushPower:int;
      
      private var explodeOnTargetFound:Boolean = false;
      
      public function PushingMine(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:§_-01J§, param6:int, param7:int)
      {
         var owner:§_-01J§ = param1;
         var x:int = param2;
         var y:int = param3;
         var paramSet:§_-C3e§ = param4;
         var target:§_-01J§ = param5;
         var timer:int = param6;
         var pushPower:int = param7;
         super(owner,x,y,paramSet,true);
         mineDistance.value = paramSet.explosionRadius.value;
         this.dummy = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
         this.pushPower = pushPower;
         this.lifetime = timer;
         if(target)
         {
            this.place(Pool.getPoint(target.x,target.y));
         }
         else
         {
            §_-Y2I§.keepUnstable = false;
            this.explodeOnTargetFound = true;
            §_-Zd§.once(§_-Y2I§ as EventDispatcher,§_-42G§.§_-K2J§,function():void
            {
               clip.rotation = 0;
            });
         }
      }
      
      private static function isIgnored(param1:§_-F3o§) : Boolean
      {
         var _loc2_:Class = null;
         for each(_loc2_ in IGNORE_OBJECTS)
         {
            if(param1 is _loc2_)
            {
               return true;
            }
         }
         return false;
      }
      
      private function place(param1:Point) : void
      {
         this.y = param1.y + Physics.UNIT_HALF_HEIGHT;
         this.x = param1.x;
         stable = true;
         clip.rotation = 0;
      }
      
      private function search(param1:int) : Vector.<§_-01J§>
      {
         var _loc5_:§_-01J§ = null;
         var _loc6_:Vector.<§_-F3o§> = null;
         var _loc2_:Vector.<§_-01J§> = new Vector.<§_-01J§>();
         var _loc3_:Vector.<§_-F3o§> = new Vector.<§_-F3o§>();
         var _loc4_:Vector.<§_-01J§> = §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner);
         for each(_loc5_ in _loc4_)
         {
            _loc3_.push(_loc5_);
         }
         _loc6_ = Physics.getGameObjectsInCone(x,y,param1,SEARCH_ANGLE,SEARCH_ANGLE_WIDTH,_loc3_);
         for each(_loc5_ in _loc6_)
         {
            if(!HitTest.castRayVec(x,y,SEARCH_ANGLE,Math.abs(y - _loc5_.y)) && this.target != _loc5_)
            {
               _loc2_.push(_loc5_);
            }
         }
         return _loc2_;
      }
      
      override protected function findTarget() : §_-01J§
      {
         var _loc1_:Vector.<§_-01J§> = this.search(SEARCH_RADIUS);
         if(_loc1_.length > 0)
         {
            this.target = _loc1_[0];
         }
         return this.target;
      }
      
      override protected function tick() : void
      {
         if(frames >= this.lifetime)
         {
            this.explode();
         }
         else if(frames % 30 == 0)
         {
            §_-h2B§.play(§_-d27§.§_-27§);
         }
         if(this.explodeOnTargetFound && stable && Boolean(this.findTarget()))
         {
            this.explode();
         }
      }
      
      override protected function explode() : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc3_:uint = 0;
         var _loc4_:Vector.<§_-F3o§> = null;
         var _loc5_:§_-F3o§ = null;
         var _loc1_:Vector.<§_-01J§> = this.search(Physics.UNIT_HEIGHT);
         if(Boolean(this.target) && !this.target.disposed)
         {
            _loc1_.push(this.target);
         }
         for each(_loc2_ in _loc1_)
         {
            if(_loc2_ == this.target)
            {
               _loc2_.x = x;
            }
            §_-G4§.§_-u1v§(_loc2_ as §_-F3o§,0,-this.pushPower,true);
            _loc2_.graphics.damaged();
         }
         _loc3_ = Physics.UNIT_HALF_HEIGHT;
         _loc4_ = Physics.getGameObjectsInZone(x - _loc3_,y - _loc3_,x + _loc3_,y + _loc3_,§_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([§_-Qi§]));
         for each(_loc5_ in _loc4_)
         {
            if(!isIgnored(_loc5_))
            {
               _loc5_.setSpeedXY(0,-this.pushPower);
            }
         }
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMALL,x,y,mineDistance.value * 3,§_-d27§.§_-j1o§);
         disposed = true;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         if(Boolean(this.dummy) && !this.dummy.disposed)
         {
            this.dummy.disposed = true;
         }
      }
   }
}

