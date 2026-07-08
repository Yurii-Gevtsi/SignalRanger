package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.brokenLine.BrokenLine;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.filters.GlowFilter;
   import starling.utils.Pool;
   
   public class LaserRay extends §_-Qi§
   {
      
      public static const SEARCH_RADIUS:§_-TA§ = new §_-TA§(30);
      
      private var _firstActivator:§_-F3o§;
      
      private var _secondActivator:§_-F3o§;
      
      private var ray:BrokenLine;
      
      private var color:uint;
      
      private var paramSet:§_-C3e§;
      
      private var destroyActivatorsOnDispose:Boolean;
      
      public function LaserRay(param1:§_-01J§, param2:§_-C3e§, param3:§_-F3o§, param4:§_-F3o§, param5:uint = 16211289, param6:Boolean = true)
      {
         this.color = param5;
         this._firstActivator = param3;
         this._secondActivator = param4;
         this.paramSet = param2;
         this.destroyActivatorsOnDispose = param6;
         §_-In§ = DamageType.SHOCK;
         super(param1,param3.x,param4.y,false,0);
         this.x = param3.x;
         this.y = param3.y;
         §_-z1H§ = new Sprite();
         this.draw();
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         this.initialCheck();
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.NONE);
         §_-Y2I§ = new GenericPhysicModel(physParams);
      }
      
      private function initialCheck() : void
      {
         if(!(Boolean(!disposed && this._firstActivator) && Boolean(this._secondActivator) && this.checkEnemy(false)))
         {
            stable = true;
         }
      }
      
      private function draw() : void
      {
         var _loc7_:GlowFilter = null;
         var _loc1_:int = x = this.firstActivator.x;
         var _loc2_:int = y = this.firstActivator.y;
         var _loc3_:int = this.secondActivator.x - _loc1_;
         var _loc4_:int = this.secondActivator.y - _loc2_;
         var _loc5_:Point = Pool.getPoint();
         MathUtil.§_-UW§(6,Math.atan2(_loc4_,_loc3_),_loc5_);
         var _loc6_:Point = Pool.getPoint(_loc3_ - _loc5_.x,_loc4_ - _loc5_.y);
         if(!this.ray)
         {
            this.ray = new BrokenLine(3,this.color);
            _loc7_ = new GlowFilter(this.color,1,2,1);
            this.ray.filter = _loc7_;
            (§_-z1H§ as DisplayObjectContainer).addChild(this.ray);
         }
         this.ray.clear();
         this.ray.filter.clearCache();
         this.ray.drawLine(_loc5_,_loc6_);
         this.ray.filter.cache();
         Pool.putPoint(_loc5_);
         Pool.putPoint(_loc6_);
      }
      
      private function destroyBarriers() : void
      {
         var _loc2_:Number = NaN;
         var _loc9_:§_-Qi§ = null;
         var _loc1_:Number = MathUtil.distance(x,y,this._secondActivator.x,this._secondActivator.y);
         _loc2_ = Math.atan2(this._secondActivator.y - y,this._secondActivator.x - x);
         var _loc3_:Number = Math.cos(_loc2_);
         var _loc4_:Number = Math.sin(_loc2_);
         var _loc5_:* = 0;
         var _loc6_:int = 3;
         var _loc7_:Number = 0;
         var _loc8_:Number = 0;
         do
         {
            _loc7_ += _loc6_ * _loc3_;
            _loc8_ += _loc6_ * _loc4_;
            if(HitTest.point(x + _loc7_,y + _loc8_))
            {
               _loc9_ = new §_-Qi§(owner,x,y,false,1);
               _loc9_.x = x + _loc7_;
               _loc9_.y = y + _loc8_;
               §_-G4§.inflictDamage(_loc9_,null,0,20,3);
               Effects.attachTrail(this,6,2,16762931,16777215,1);
            }
         }
         while(++_loc5_ * _loc6_ < _loc1_);
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(this._firstActivator.disposed || this._secondActivator.disposed)
         {
            disposed = true;
         }
         else
         {
            this.checkEnemy();
         }
      }
      
      private function checkEnemy(param1:Boolean = true) : Boolean
      {
         var x1:int = 0;
         var y1:int = 0;
         var x2:int = 0;
         var y2:int = 0;
         var considerStable:Boolean = param1;
         if(!this._firstActivator.disposed && !this._secondActivator.disposed)
         {
            x1 = this._firstActivator.x;
            y1 = this._firstActivator.y;
            x2 = this._secondActivator.x;
            y2 = this._secondActivator.y;
            §_-F2L§.§_-13w§(§_-X1j§.§_-12n§.gameMaster.§_-D2A§(owner as §_-01J§),function(param1:§_-01J§):Boolean
            {
               if((!considerStable ? Boolean(!param1.stable) : Boolean(true)) && MathUtil.§_-G1o§(x1,y1,x2,y2,param1.x,param1.y,SEARCH_RADIUS.value))
               {
                  if(destroyActivatorsOnDispose)
                  {
                     _firstActivator.disposed = true;
                     _secondActivator.disposed = true;
                     Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMALL,x1,y1,12);
                     Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMALL,x2,y2,12);
                  }
                  destroyBarriers();
                  explode(param1);
                  disposed = true;
               }
               return false;
            });
            return false;
         }
         return false;
      }
      
      protected function explode(param1:§_-01J§) : void
      {
         inflictDamage(param1 as §_-F3o§,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value,true,true);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMALL,param1.x,param1.y,30,§_-d27§.§_-a2n§);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         this._firstActivator = null;
         this._secondActivator = null;
         this.paramSet = null;
         if(this.ray)
         {
            if(this.ray.filter)
            {
               this.ray.filter.dispose();
            }
            this.ray.removeFromParent(true);
         }
         this.ray = null;
         super.defaultOnDisposed(param1);
      }
      
      public function get firstActivator() : §_-F3o§
      {
         return this._firstActivator;
      }
      
      public function get secondActivator() : §_-F3o§
      {
         return this._secondActivator;
      }
   }
}

