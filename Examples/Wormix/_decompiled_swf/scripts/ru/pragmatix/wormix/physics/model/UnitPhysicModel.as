package ru.pragmatix.wormix.physics.model
{
   import §_-31N§.§_-42G§;
   import §_-A1K§.§_-TA§;
   import §_-b1F§.§_-G4§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-z1x§;
   import starling.events.Event;
   
   public class UnitPhysicModel extends AnimalPhysicModel implements §_-z1x§
   {
      
      private static const MIN_ROLLING:uint = 5;
      
      private static const MAX_ROLLING:uint = 12;
      
      private static const MAX_ROLL_DISTANCE:uint = 100;
      
      private static const UNIT_CRAWL_HEIGHT:§_-TA§ = new §_-TA§(10);
      
      private static const TEN:§_-TA§ = new §_-TA§(10);
      
      private static const ROLLING_REDUCE_FACTOR:§_-TA§ = new §_-TA§(9);
      
      private var rollDistance:uint;
      
      public var rolling:int = 0;
      
      public var rollable:Boolean = false;
      
      private var walkEnergy:int = 0;
      
      private var hasUnitCollision:Boolean;
      
      public function UnitPhysicModel(param1:PhysParams, param2:int = 10)
      {
         super(param1,UNIT_CRAWL_HEIGHT.value,Physics.UNIT_HALF_HEIGHT);
      }
      
      protected function stopUnit() : void
      {
         if(speed > 8)
         {
            §_-h2B§.play(§_-d27§.§_-G2Y§);
         }
         setSpeedXY(0,0);
         if(keepUnstable)
         {
            dispatchEvent(new Event(§_-42G§.§_-X1C§));
         }
         else
         {
            stable = true;
         }
      }
      
      override public function processCollision(param1:§_-F3o§, param2:int, param3:int, param4:Object) : void
      {
         var _loc5_:int = 0;
         var _loc6_:Boolean = false;
         var _loc7_:Number = NaN;
         var _loc8_:int = 0;
         var _loc9_:Number = NaN;
         if(param1 == null)
         {
            physParams.epsilonSpeedCheck();
            if(HitTest.unit(param2,param3))
            {
               §_-G4§.§_-Ee§(null,param2,param3,null,0,0,Physics.UNIT_HALF_HEIGHT + 1,false);
            }
            _loc6_ = true;
            if(physParams.dy > 0 && HitTest.unit(x,y + 1))
            {
               if(Math.abs(physParams.dx / physParams.dy) < 0.1 || speed < 4)
               {
                  _loc5_ = 1;
                  while(_loc5_ <= crawlHeight)
                  {
                     if(!HitTest.unit(x,y - _loc5_))
                     {
                        break;
                     }
                     _loc5_++;
                  }
                  if(_loc5_ < crawlHeight)
                  {
                     y -= _loc5_;
                     this.stopUnit();
                     _loc6_ = false;
                  }
               }
               else if(this.rollable && speed > 1)
               {
                  _loc5_ = 1;
                  while(_loc5_ <= crawlHeight)
                  {
                     if(!HitTest.unit(x,y - _loc5_))
                     {
                        break;
                     }
                     _loc5_++;
                  }
                  if(_loc5_ < crawlHeight)
                  {
                     y -= _loc5_;
                     _loc7_ = Physics.getNormalInPoint(x,y + halfHeight);
                     _loc8_ = angle > _loc7_ ? -1 : 1;
                     _loc9_ = Math.abs(angle - _loc7_) / MathUtil.PI_HALF * (MAX_ROLLING - MIN_ROLLING) + MIN_ROLLING;
                     this.rolling = _loc8_ * _loc9_;
                     setSpeedXY(0,0);
                     _loc6_ = false;
                  }
               }
            }
            else
            {
               physParams.dy *= -0.5;
            }
            if(_loc6_)
            {
               x = param2;
               if(!HitTest.gobjUpDown(param2,param3 + 1,halfHeight,false,false))
               {
                  y = param3 + 1;
               }
               else
               {
                  y = param3;
                  this.stopUnit();
               }
               physParams.dx *= -0.3;
            }
            physParams.epsilonSpeedCheck(0.001);
            stop();
         }
      }
      
      override public function nextState() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         if(this.rolling != 0)
         {
            if(Math.abs(this.rolling) <= 1 || this.rollDistance > MAX_ROLL_DISTANCE)
            {
               setSpeedXY(0,0);
               this.rolling = 0;
            }
            else
            {
               Effects.createDragonbonesAnimationEffect(EffectType.UNIT_TRAIL,x,y + Physics.UNIT_HALF_HEIGHT,5);
               this.rolling *= ROLLING_REDUCE_FACTOR.value / TEN.value;
               _loc1_ = this.rolling;
               _loc2_ = 1;
               while(_loc2_ <= Math.abs(_loc1_))
               {
                  ++this.rollDistance;
                  if(!this.nextRollPos(x + §_-H3D§.sign(_loc1_)))
                  {
                     break;
                  }
                  _loc2_++;
               }
            }
         }
         else if(crawling != 0)
         {
            this.walkEnergy += crawling;
            _loc3_ = §_-H3D§.sign(this.walkEnergy);
            while(Math.abs(this.walkEnergy) > TEN.value)
            {
               this.walkEnergy = _loc3_ * (Math.abs(this.walkEnergy) - TEN.value);
               if(!nextCrawlPos(x + _loc3_))
               {
                  this.walkEnergy = 0;
                  break;
               }
            }
         }
         else
         {
            super.nextState();
         }
         if(stable)
         {
            this.rollDistance = 0;
         }
         if(!physParams.disposed)
         {
            checkBounds();
         }
      }
      
      private function nextRollPos(param1:int) : Boolean
      {
         this.hasUnitCollision = false;
         Physics.processEachUnitCollidingUnits(this,this);
         if(!this.hasUnitCollision)
         {
            if(HitTest.gobjLeftRight(param1,y,halfHeight,false,this.rolling < 0))
            {
               if(tryWallCrawl(param1,y,false,this.rolling < 0) || tryWallCrawl(param1,y,true,this.rolling < 0))
               {
                  this.rolling *= 0.9;
                  return true;
               }
               this.rolling *= 0.5;
            }
            else
            {
               if(tryCrawlDown(param1,y))
               {
                  return true;
               }
               physParams.dx = this.rolling / 2;
               this.rolling = 0;
               stable = false;
               x = param1;
            }
         }
         return false;
      }
      
      public function processUnit(param1:§_-01J§) : Boolean
      {
         this.hasUnitCollision = true;
         if(Boolean(§_-X1j§.§_-12n§) && Boolean(§_-X1j§.§_-12n§.gameMaster) && §_-X1j§.§_-12n§.gameMaster.activeUnit == param1)
         {
            return false;
         }
         param1.stable = false;
         var _loc2_:UnitPhysicModel = UnitPhysicModel(param1.getPhysicModel());
         _loc2_.rollable = true;
         _loc2_.rolling = this.rolling * (ROLLING_REDUCE_FACTOR.value / TEN.value);
         this.rolling *= 0.4;
         return false;
      }
   }
}

