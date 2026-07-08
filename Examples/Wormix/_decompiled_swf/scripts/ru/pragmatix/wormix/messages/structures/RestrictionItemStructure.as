package ru.pragmatix.wormix.messages.structures
{
   import §_-62§.§_-T1y§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class RestrictionItemStructure
   {
      
      public var endDate:int;
      
      public var reason:int;
      
      public var blocks:Vector.<int>;
      
      public function RestrictionItemStructure(param1:int, param2:int, param3:Vector.<int>)
      {
         super();
         this.endDate = param1;
         this.reason = param2;
         this.blocks = param3;
      }
      
      public function toString() : String
      {
         return "RestrictionItemStructure{" + " endDate=" + this.endDate + " reason=" + this.reason + " blocks=" + this.blocks + " }";
      }
   }
}

