package ru.pragmatix.wormix.physics
{
   public class ComponentForce extends Force
   {
      
      private var xComp0:Number;
      
      private var yComp0:Number;
      
      private var xCompPositive:Number = 0;
      
      private var yCompPositive:Number = 0;
      
      private var xCompNegative:Number = 0;
      
      private var yCompNegative:Number = 0;
      
      private var xLimit:Number;
      
      private var yLimit:Number;
      
      public function ComponentForce(param1:String, param2:Number = 0, param3:Number = 0, param4:Number = NaN, param5:Number = NaN)
      {
         var _loc6_:Number = Math.atan2(param3,param2);
         var _loc7_:Number = Math.sqrt(param2 * param2 + param3 * param3);
         super(param1,_loc6_,_loc7_);
         this.xComp0 = param2;
         this.yComp0 = param3;
         this.xLimit = param4;
         this.yLimit = param5;
      }
      
      public function set xPos(param1:Number) : void
      {
         this.xCompPositive = param1;
         this.recalc();
      }
      
      public function set yPos(param1:Number) : void
      {
         this.yCompPositive = param1;
         this.recalc();
      }
      
      public function set xNeg(param1:Number) : void
      {
         this.xCompNegative = param1;
         this.recalc();
      }
      
      public function set yNeg(param1:Number) : void
      {
         this.yCompNegative = param1;
         this.recalc();
      }
      
      private function recalc() : void
      {
         ax = this.xComp0 + this.xCompPositive - this.xCompNegative;
         ay = this.yComp0 + this.yCompPositive - this.yCompNegative;
         if(!isNaN(this.xLimit))
         {
            if(ax < -this.xLimit)
            {
               ax = -this.xLimit;
            }
            else if(ax > this.xLimit)
            {
               ax = this.xLimit;
            }
         }
         if(!isNaN(this.yLimit))
         {
            if(ay < -this.yLimit)
            {
               ay = -this.yLimit;
            }
            else if(ay > this.yLimit)
            {
               ay = this.yLimit;
            }
         }
      }
   }
}

