package ru.pragmatix.wormix.messages.server
{
   import §_-f1Q§.*;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-r1C§.§_-h2B§;
   import feathers.events.FeathersEventType;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import starling.events.Event;
   
   public class BuyRaceResult extends PurchaseResponse
   {
      
      public var race:§_-L1x§;
      
      public var races:int;
      
      public function BuyRaceResult()
      {
         super();
      }
   }
}

