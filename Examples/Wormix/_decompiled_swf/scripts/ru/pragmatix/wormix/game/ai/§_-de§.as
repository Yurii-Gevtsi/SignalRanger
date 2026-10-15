package ru.pragmatix.wormix.game.ai
{
   import ru.pragmatix.flox.logger.§_-V2x§;
   
   public class §_-de§
   {
      
      public var weaponId:uint;
      
      public var minLevel:uint;
      
      public var maxLevel:uint;
      
      public var maxShots:int;
      
      public var §_-K1W§:Number;
      
      public var shots:uint = 0;
      
      public function §_-de§(param1:uint, param2:Number = 1, param3:int = -1, param4:uint = 0, param5:uint = 4294967295)
      {
         super();
         this.weaponId = param1;
         this.§_-K1W§ = param2;
         if(param3 != -1)
         {
            this.shots = param3;
         }
         this.maxShots = param3;
         this.minLevel = param4;
         this.maxLevel = param5;
      }
   }
}

