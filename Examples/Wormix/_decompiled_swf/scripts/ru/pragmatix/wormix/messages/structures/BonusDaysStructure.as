package ru.pragmatix.wormix.messages.structures
{
   import §_-A1K§.§_-TA§;
   import §_-l1K§.§_-W2f§;
   import flash.utils.Dictionary;
   
   public class BonusDaysStructure
   {
      
      public var money:int;
      
      public var realMoney:int;
      
      public var battlesCount:int;
      
      public var reagents:Dictionary = new Dictionary();
      
      public var bonusMessage:String = "";
      
      public function BonusDaysStructure()
      {
         super();
      }
      
      public function toString() : String
      {
         return "BonusDaysStructure{money=" + String(this.money) + ",realMoney=" + String(this.realMoney) + ",battlesCount=" + String(this.battlesCount) + ",bonusMessage=" + this.bonusMessage + "}";
      }
   }
}

