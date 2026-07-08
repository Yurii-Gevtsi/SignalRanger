package ru.pragmatix.wormix.messages.structures
{
   import §_-Vg§.*;
   import §_-YF§.§_-q2v§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   
   public class AdvertisingPurchaseStructure
   {
      
      public var name:String;
      
      public var nextTime:int;
      
      public function AdvertisingPurchaseStructure(param1:String = "", param2:int = 0)
      {
         super();
         this.name = param1;
         this.nextTime = param2;
      }
      
      public function toString() : String
      {
         return "AdvertisingPurchaseStructure{" + "name=" + this.name + ",nextTime=" + this.nextTime + "}";
      }
   }
}

