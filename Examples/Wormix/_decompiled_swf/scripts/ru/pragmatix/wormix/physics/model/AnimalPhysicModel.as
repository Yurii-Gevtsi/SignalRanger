package ru.pragmatix.wormix.physics.model
{
   import §_-31N§.§_-42G§;
   import §_-b1F§.§_-G4§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   
   public class AnimalPhysicModel extends GenericPhysicModel
   {
      
      private static const DEFAULT_ANIMAL_CRAWL_HEIGHT:int = 15;
      
      private static const DEFAULT_ANIMAL_HALF_HEIGHT:uint = 15;
      
      public var crawling:int = 0;
      
      protected var crawlHeight:int;
      
      protected var halfHeight:uint;
      
      public function AnimalPhysicModel(param1:PhysParams, param2:int = 15, param3:uint = 15)
      {
         super(param1);
         this.crawlHeight = param2;
         this.halfHeight = param3;
      }
      
      override public function hitTestGround(param1:int, param2:int) : Boolean
      {
         var _loc3_:int = x - param1;
         var _loc4_:int = y - param2;
         if(Math.abs(_loc3_) <= 1 && Math.abs(_loc4_) <= 1)
         {
            return _loc3_ != 0 && HitTest.gobjLeftRight(x,y,this.halfHeight,false,_loc3_ < 0) || _loc4_ != 0 && HitTest.gobjUpDown(x,y,this.halfHeight,false,_loc4_ < 0);
         }
         return HitTest.gameObject(x,y,this.halfHeight);
      }
      
      override public function processCollision(param1:§_-F3o§, param2:int, param3:int, param4:Object) : void
      {
         var _loc5_:int = 0;
         if(param1 == null)
         {
            if(HitTest.gameObject(param2,param3,this.halfHeight))
            {
               §_-G4§.§_-Ee§(null,param2,param3,null,0,0,this.halfHeight + 3,false);
            }
            if(speedY > 0 && (Math.abs(speedX / speedY) < 0.2 || speedX + speedY < 20) && HitTest.gameObject(x,y + 1,this.halfHeight))
            {
               _loc5_ = 1;
               while(_loc5_ <= 6)
               {
                  if(!HitTest.gameObject(x,y - _loc5_,this.halfHeight))
                  {
                     break;
                  }
                  _loc5_++;
               }
               if(_loc5_ != 0 && _loc5_ < 6)
               {
                  y -= _loc5_;
                  setSpeedXY(0,0);
                  stop();
                  if(keepUnstable)
                  {
                     dispatchEventWith(§_-42G§.§_-X1C§);
                  }
                  else
                  {
                     stable = true;
                  }
               }
               else
               {
                  x = param2;
                  y = param3;
                  physParams.dx *= -0.3;
                  if(physParams.dy < 0)
                  {
                     physParams.dy *= -0.8;
                  }
               }
            }
            else
            {
               x = param2;
               y = param3;
               physParams.dx *= -0.6;
               if(physParams.dy < 0)
               {
                  physParams.dy *= -0.8;
               }
            }
            physParams.epsilonSpeedCheck(0.001);
            stop();
         }
      }
      
      override public function nextState() : void
      {
         var _loc1_:int = 0;
         if(this.crawling != 0)
         {
            _loc1_ = 1;
            while(_loc1_ <= Math.abs(this.crawling))
            {
               if(!this.nextCrawlPos(x + §_-H3D§.sign(this.crawling)))
               {
                  break;
               }
               _loc1_++;
            }
         }
         else
         {
            super.nextState();
         }
      }
      
      protected function tryWallCrawl(param1:int, param2:int, param3:Boolean, param4:Boolean) : Boolean
      {
         var _loc5_:int = 0;
         if(!HitTest.gobjUpDown(param1,param2,this.halfHeight,false,param3,false))
         {
            _loc5_ = 1;
            while(_loc5_ <= this.crawlHeight)
            {
               param2 += param3 ? -1 : 1;
               if(HitTest.gobjUpDown(param1,param2,this.halfHeight,false,param3,false))
               {
                  break;
               }
               if(!HitTest.gobjLeftRight(param1,param2,this.halfHeight,false,param4) && HitTest.gobjUpDown(param1,param2 + 1,this.halfHeight,false,false))
               {
                  x = param1;
                  y = param2;
                  return true;
               }
               _loc5_++;
            }
         }
         return false;
      }
      
      protected function tryCrawlDown(param1:int, param2:int) : Boolean
      {
         var _loc3_:int = 1;
         while(_loc3_ < this.crawlHeight)
         {
            if(++param2 > getSinkLevel())
            {
               break;
            }
            if(HitTest.gobjUpDown(param1,param2,this.halfHeight,false,false))
            {
               x = param1;
               y = param2 - 1;
               return true;
            }
            _loc3_++;
         }
         return false;
      }
      
      protected function nextCrawlPos(param1:int) : Boolean
      {
         if(HitTest.gobjLeftRight(param1,y,this.halfHeight,false,this.crawling < 0))
         {
            if(this.tryWallCrawl(param1,y,false,this.crawling < 0) || this.tryWallCrawl(param1,y,true,this.crawling < 0))
            {
               return true;
            }
            dispatchEventWith(§_-42G§.§_-G36§);
         }
         else
         {
            if(this.tryCrawlDown(param1,y))
            {
               return true;
            }
            stable = false;
            x = param1;
            y = y + this.crawlHeight - 1;
            dispatchEventWith(§_-42G§.§_-Lz§);
         }
         return false;
      }
   }
}

