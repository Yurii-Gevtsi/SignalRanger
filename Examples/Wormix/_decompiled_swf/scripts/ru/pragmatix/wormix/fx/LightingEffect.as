package ru.pragmatix.wormix.fx
{
   import §_-G16§.§_-33h§;
   import §_-G16§.§_-Y2x§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.brokenLine.BrokenLine;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.events.Event;
   
   public class LightingEffect implements §_-Y2x§
   {
      
      private static var pool:§_-33h§ = new §_-33h§(LightingEffect);
      
      private var ray:BrokenLine;
      
      private var duration:uint;
      
      private var period:uint;
      
      private var counter:uint;
      
      private var dispersion:uint;
      
      private var step:uint;
      
      private var stepRevert:Number;
      
      private var steps:uint;
      
      private var x1:int;
      
      private var y1:int;
      
      private var x2:int;
      
      private var y2:int;
      
      private var currAngle:Number;
      
      private var steps_x_cosa:Number;
      
      private var steps_x_sina:Number;
      
      private var dispersion_x_cosa:Number;
      
      private var dispersion_x_sina:Number;
      
      private var src:§_-F3o§;
      
      private var trg:§_-F3o§;
      
      private var srcPhysicModel:IPhysicModel;
      
      private var trgPhysicalModel:IPhysicModel;
      
      private var updateFunction:Function;
      
      public function LightingEffect()
      {
         super();
         this.ray = new BrokenLine(2,0);
      }
      
      public static function initPool() : void
      {
         pool.fill(20);
      }
      
      public static function clearPool() : void
      {
         pool.empty();
      }
      
      public static function createFromPool() : LightingEffect
      {
         var _loc1_:LightingEffect = pool.get() as LightingEffect;
         if(_loc1_)
         {
            _loc1_.revive();
         }
         return _loc1_;
      }
      
      public function revive() : void
      {
         this.src = null;
         this.trg = null;
      }
      
      public function fromPointToPoint(param1:int, param2:int, param3:int, param4:int, param5:uint, param6:uint, param7:uint, param8:uint, param9:uint) : void
      {
         this.x1 = param1;
         this.y1 = param2;
         this.x2 = param3;
         this.y2 = param4;
         this.updateFunction = this.updateWithPoints;
         this.steps = §_-H3D§.distance(param1 - param3,param2 - param4) / param6;
         var _loc10_:Number = Math.atan2(param4 - param2,param3 - param1);
         var _loc11_:Number = Math.cos(_loc10_);
         var _loc12_:Number = Math.sin(_loc10_);
         this.steps_x_cosa = param6 * _loc11_;
         this.steps_x_sina = param6 * _loc12_;
         this.dispersion_x_cosa = param8 * _loc11_;
         this.dispersion_x_sina = param8 * _loc12_;
         this.init(param7,param6,param5,param8,param9);
      }
      
      public function fromObjectToPoint(param1:§_-F3o§, param2:int, param3:int, param4:uint, param5:uint, param6:uint, param7:uint) : void
      {
         this.srcPhysicModel = param1 ? param1.getPhysicModel() : null;
         if(!param1.disposed && Boolean(this.srcPhysicModel))
         {
            this.src = param1;
            this.x1 = this.srcPhysicModel.x;
            this.y1 = this.srcPhysicModel.y;
            this.x2 = param2;
            this.y2 = param3;
            this.updateFunction = this.updateWithObjects;
            this.stepRevert = 1 / param5;
            this.init(param6,param5,param4,param7,1);
         }
      }
      
      public function fromObjectToObject(param1:§_-F3o§, param2:§_-F3o§, param3:uint, param4:uint, param5:uint, param6:uint) : void
      {
         this.srcPhysicModel = param1 ? param1.getPhysicModel() : null;
         this.trgPhysicalModel = param2 ? param2.getPhysicModel() : null;
         if(Boolean(!param1.disposed && this.srcPhysicModel) && Boolean(!param2.disposed) && Boolean(this.trgPhysicalModel))
         {
            this.src = param1;
            this.trg = param2;
            this.x1 = this.srcPhysicModel.x;
            this.y1 = this.srcPhysicModel.y;
            this.x2 = this.trgPhysicalModel.x;
            this.y2 = this.trgPhysicalModel.y;
            this.updateFunction = this.updateWithObjects;
            this.stepRevert = 1 / param4;
            this.init(param5,param4,param3,param6,1);
         }
      }
      
      private function init(param1:uint, param2:uint, param3:uint, param4:uint, param5:uint) : void
      {
         this.ray.setColor(param1);
         this.duration = param3;
         this.dispersion = param4;
         this.period = param5;
         this.step = param2;
         this.counter = 0;
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(this.ray,SortingLayer.EFFECT_LAYER);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
      }
      
      public function onFrame(param1:Event) : void
      {
         this.updateFunction();
      }
      
      private function updateWithPoints() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         if(++this.counter <= this.duration)
         {
            if(this.counter % this.period == 0)
            {
               this.ray.beginDraw();
               this.ray.moveTo(this.x1,this.y1);
               _loc3_ = 1;
               while(_loc3_ < this.steps)
               {
                  _loc1_ = this.x1 + _loc3_ * this.steps_x_cosa + (Math.random() * 2 - 1) * this.dispersion_x_sina;
                  _loc2_ = this.y1 + _loc3_ * this.steps_x_sina + (Math.random() * 2 - 1) * this.dispersion_x_cosa;
                  this.ray.lineTo(_loc1_,_loc2_);
                  _loc3_++;
               }
               this.ray.lineTo(this.x2,this.y2);
               this.ray.endDraw();
            }
         }
         else
         {
            this.clear();
         }
      }
      
      private function clear() : void
      {
         §_-X1j§.§_-12n§.scene.removeElement(this.ray);
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
         pool.put(this);
      }
      
      private function updateWithObjects() : void
      {
         if(Boolean(this.src) && Boolean(!this.src.disposed) && Boolean(this.srcPhysicModel))
         {
            this.x1 = this.srcPhysicModel.x;
            this.y1 = this.srcPhysicModel.y;
         }
         if(Boolean(this.trg) && Boolean(!this.trg.disposed) && Boolean(this.trgPhysicalModel))
         {
            this.x2 = this.trgPhysicalModel.x;
            this.y2 = this.trgPhysicalModel.y;
         }
         var _loc1_:int = this.x2 - this.x1;
         var _loc2_:int = this.y2 - this.y1;
         var _loc3_:int = §_-H3D§.distance(_loc1_,_loc2_);
         this.steps = _loc3_ * this.stepRevert;
         var _loc4_:Number = Math.atan2(_loc2_,_loc1_);
         this.currAngle = _loc4_;
         var _loc5_:Number = Math.cos(_loc4_);
         var _loc6_:Number = Math.sin(_loc4_);
         this.steps_x_cosa = this.step * _loc5_;
         this.steps_x_sina = this.step * _loc6_;
         this.dispersion_x_cosa = this.dispersion * _loc5_;
         this.dispersion_x_sina = this.dispersion * _loc6_;
         this.updateWithPoints();
      }
      
      public function removeFromPool() : void
      {
         this.ray.dispose();
         this.src = null;
         this.srcPhysicModel = null;
         this.updateFunction = null;
      }
   }
}

