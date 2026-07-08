package ru.pragmatix.wormix.physics.model
{
   import §_-b1F§.§_-G4§;
   import §_-sQ§.§_-H3D§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.utils.Pool;
   
   public class CatPhysicModel extends UnitPhysicModel
   {
      
      private static const GROUND:uint = 0;
      
      private static const CEIL:uint = 1;
      
      private static const LEFT_WALL:uint = 2;
      
      private static const RIGHT_WALL:uint = 3;
      
      private static const NORMAL_CHECK_RADIUS:uint = 20;
      
      private var attach:uint = 0;
      
      public var slopeAngle:Number = 1.5707963267948966;
      
      public function CatPhysicModel(param1:PhysParams)
      {
         super(param1);
      }
      
      override protected function stopUnit() : void
      {
         super.stopUnit();
         this.attach = GROUND;
      }
      
      override public function hitTestGround(param1:int, param2:int) : Boolean
      {
         var _loc3_:int = x - param1;
         var _loc4_:int = y - param2;
         if(Math.abs(_loc3_) <= 1 && Math.abs(_loc4_) <= 1)
         {
            return HitTest.circle(x,y,false);
         }
         return HitTest.circle(x,y);
      }
      
      override protected function nextCrawlPos(param1:int) : Boolean
      {
         var _loc2_:Boolean = this.nextCrawlStep(§_-H3D§.sign(param1 - x));
         this.slopeAngle = §_-H3D§.§_-r2H§(this.slopeAngle,Physics.getNormal(param1,y,NORMAL_CHECK_RADIUS,this.slopeAngle),MathUtil.PI_ONE_16);
         return _loc2_;
      }
      
      private function nextCrawlStep(param1:int, param2:uint = 0) : Boolean
      {
         if(param2 > 4)
         {
            stable = false;
            return false;
         }
         switch(this.attach)
         {
            case GROUND:
               if(!HitTest.circle(x,y + 1,false))
               {
                  stable = false;
                  return false;
               }
               if(!HitTest.circle(x + param1,y,false))
               {
                  x += param1;
                  if(HitTest.circle(x,y + 1,false))
                  {
                     return true;
                  }
                  this.attach = param1 > 0 ? LEFT_WALL : RIGHT_WALL;
                  return this.nextCrawlStep(param1,param2 + 1);
               }
               this.attach = param1 > 0 ? RIGHT_WALL : LEFT_WALL;
               return this.nextCrawlStep(param1,param2 + 1);
               break;
            case CEIL:
               if(!HitTest.circle(x - param1,y,false))
               {
                  x -= param1;
                  if(HitTest.circle(x,y - 1,false))
                  {
                     return true;
                  }
                  this.attach = param1 > 0 ? RIGHT_WALL : LEFT_WALL;
                  return this.nextCrawlStep(param1,param2 + 1);
               }
               this.attach = param1 > 0 ? LEFT_WALL : RIGHT_WALL;
               return this.nextCrawlStep(param1,param2 + 1);
               break;
            case LEFT_WALL:
               if(!HitTest.circle(x,y + param1,false))
               {
                  y += param1;
                  if(!HitTest.circle(x - 1,y,false))
                  {
                     this.attach = param1 > 0 ? CEIL : GROUND;
                     --x;
                  }
                  return true;
               }
               this.attach = param1 > 0 ? GROUND : CEIL;
               return this.nextCrawlStep(param1,param2 + 1);
               break;
            case RIGHT_WALL:
               if(!HitTest.circle(x,y - param1,false))
               {
                  y -= param1;
                  if(!HitTest.circle(x + 1,y,false))
                  {
                     this.attach = param1 > 0 ? GROUND : CEIL;
                     ++x;
                  }
                  return true;
               }
               this.attach = param1 > 0 ? CEIL : GROUND;
               return this.nextCrawlStep(param1,param2 + 1);
               break;
            default:
               return false;
         }
      }
      
      override public function processCollision(param1:§_-F3o§, param2:int, param3:int, param4:Object) : void
      {
         var _loc5_:int = 0;
         if(param1 == null)
         {
            physParams.epsilonSpeedCheck();
            if(HitTest.circle(param2,param3))
            {
               §_-G4§.§_-Ee§(null,x,y,null,0,0,Physics.UNIT_HALF_HEIGHT + 1,false);
            }
            else
            {
               _loc5_ = int(GROUND);
               if(HitTest.circle(param2,param3 + 1,false))
               {
                  x = param2;
                  y = param3;
                  _loc5_ = int(GROUND);
               }
               else if(HitTest.circle(param2 - 1,param3,false))
               {
                  x = param2;
                  y = param3;
                  _loc5_ = int(LEFT_WALL);
               }
               else if(HitTest.circle(param2 + 1,param3,false))
               {
                  x = param2;
                  y = param3;
                  _loc5_ = int(RIGHT_WALL);
               }
               else if(HitTest.circle(param2,param3 - 1,false))
               {
                  x = param2;
                  y = param3;
                  _loc5_ = int(CEIL);
               }
               else if(HitTest.circle(param2 - 1,param3 + 1,false))
               {
                  x = param2 - 1;
                  y = param3;
                  _loc5_ = int(GROUND);
               }
               else if(HitTest.circle(param2 + 1,param3 + 1,false))
               {
                  x = param2 + 1;
                  y = param3;
                  _loc5_ = int(GROUND);
               }
               else if(HitTest.circle(param2 - 1,param3 - 1,false))
               {
                  x = param2;
                  y = param3 - 1;
                  _loc5_ = int(LEFT_WALL);
               }
               else if(HitTest.circle(param2 + 1,param3 - 1,false))
               {
                  x = param2;
                  y = param3 - 1;
                  _loc5_ = int(RIGHT_WALL);
               }
               else
               {
                  x = param2;
                  y = param3;
                  _loc5_ = int(GROUND);
               }
               this.slopeAngle = Physics.getNormal(x,y,NORMAL_CHECK_RADIUS,this.slopeAngle);
               this.stopUnit();
               stop();
               this.attach = _loc5_;
            }
         }
      }
      
      private function nearestGroundPoint() : Point
      {
         var _loc1_:Number = Math.cos(this.slopeAngle);
         var _loc2_:Number = Math.sin(this.slopeAngle);
         var _loc3_:Number = x;
         var _loc4_:Number = y;
         var _loc5_:int = 0;
         while(_loc5_ <= Physics.UNIT_HALF_HEIGHT)
         {
            _loc3_ += _loc1_;
            _loc4_ += _loc2_;
            if(§_-X1j§.§_-12n§.scene.hitTestPoint(int(_loc3_),int(_loc4_)))
            {
               break;
            }
            _loc5_++;
         }
         return Pool.getPoint(int(_loc3_),int(_loc4_));
      }
   }
}

