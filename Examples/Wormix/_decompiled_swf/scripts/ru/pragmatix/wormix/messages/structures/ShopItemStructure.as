package ru.pragmatix.wormix.messages.structures
{
   import §_-73d§.§_-K4§;
   import §_-A1K§.§_-TA§;
   import §_-x2Q§.§_-dU§;
   import feathers.core.FeathersControl;
   import starling.events.Event;
   
   public class ShopItemStructure
   {
      
      public static const REAL_MONEY:int = 0;
      
      public static const MONEY:int = 1;
      
      public var id:uint;
      
      public var count:int = 0;
      
      public var moneyType:int;
      
      public function ShopItemStructure()
      {
         super();
      }
      
      public function toString() : String
      {
         return "ShopItemStructure{id=" + String(this.id) + ",count=" + String(this.count) + ",moneyType=" + String(this.moneyType) + "}";
      }
   }
}

