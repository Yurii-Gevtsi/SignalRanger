package ru.pragmatix.wormix.serialization.structures
{
   import §_-31W§.AwardCell;
   import §_-62§.§_-O2H§;
   import §_-73I§.§_-Y1l§;
   import §_-A1K§.§_-TA§;
   import §_-Ly§.§_-81L§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalLayout;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.RepeatBossBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.DepositStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class DepositStructureBinarySerializer implements §_-02j§
   {
      
      public function DepositStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:DepositStructure = new DepositStructure();
         _loc3_.moneyType = param1.readShort();
         var _loc4_:uint = param1.readUnsignedShort();
         _loc3_.dividendsByDays = [];
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            _loc3_.dividendsByDays[_loc5_] = param1.readInt();
            _loc5_++;
         }
         _loc3_.startDate = param1.readInt();
         _loc3_.progress = param1.readInt();
         _loc3_.todayDividend = param1.readInt();
         return _loc3_;
      }
   }
}

