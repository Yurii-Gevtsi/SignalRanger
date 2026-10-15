package ru.pragmatix.wormix.physics
{
   public class Joint
   {
      
      public var x:int;
      
      public var y:int;
      
      public var dir:int;
      
      public var angle:Number;
      
      public var b:Boolean;
      
      public var length:Number;
      
      public function Joint(param1:int, param2:int, param3:int, param4:Number, param5:Number)
      {
         super();
         this.x = param1;
         this.y = param2;
         this.dir = param3;
         this.angle = param4;
         this.length = param5;
      }
   }
}

