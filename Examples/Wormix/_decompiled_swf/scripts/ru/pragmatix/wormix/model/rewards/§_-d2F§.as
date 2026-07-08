package ru.pragmatix.wormix.model.rewards
{
   import §_-62§.§_-W1r§;
   import §_-ZP§.§_-YG§;
   import §_-ZP§.§_-w1J§;
   import flash.utils.Dictionary;
   
   public class §_-d2F§
   {
      
      public var money:int;
      
      public var realMoney:int;
      
      public var battlesCount:int;
      
      public var §_-u28§:int;
      
      public var §_-R2R§:int;
      
      public var items:Dictionary = new Dictionary();
      
      public var reagents:Dictionary = new Dictionary();
      
      public var reaction:int;
      
      public var exp:int;
      
      public var bonusExp:int;
      
      public var pack:§_-P2q§;
      
      public var §_-52h§:String;
      
      public var bonusMessage:String;
      
      public var §_-v1F§:String;
      
      public var §_-f1A§:§_-W1r§;
      
      public var attach:Object;
      
      public var type:§_-qU§;
      
      public function §_-d2F§()
      {
         super();
      }
      
      public function toString() : String
      {
         return "LoginBonusStructure{" + "money=" + String(this.money) + ",realMoney=" + String(this.realMoney) + ",battlesCount=" + String(this.battlesCount) + ",bonusMessage=" + this.bonusMessage + "}";
      }
   }
}

