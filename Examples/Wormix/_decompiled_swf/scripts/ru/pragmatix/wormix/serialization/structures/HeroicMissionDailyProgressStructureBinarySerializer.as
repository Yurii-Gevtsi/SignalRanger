package ru.pragmatix.wormix.serialization.structures
{
   import §_-12W§.*;
   import §_-31W§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-P1z§.*;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-Z1K§.§_-q24§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-02j§;
   import feathers.controls.List;
   import feathers.controls.Scroller;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionDailyProgressStructure;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class HeroicMissionDailyProgressStructureBinarySerializer implements §_-02j§
   {
      
      public function HeroicMissionDailyProgressStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ByteArray = param1 as ByteArray;
         var _loc4_:HeroicMissionDailyProgressStructure = new HeroicMissionDailyProgressStructure();
         _loc4_.defeatCount = _loc3_.readInt();
         _loc4_.winCount = _loc3_.readInt();
         return _loc4_;
      }
   }
}

