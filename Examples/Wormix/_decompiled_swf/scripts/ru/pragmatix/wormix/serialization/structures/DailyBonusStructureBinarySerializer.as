package ru.pragmatix.wormix.serialization.structures
{
   import §_-62§.§_-W1r§;
   import §_-91A§.*;
   import §_-931§.§_-sz§;
   import §_-B1y§.§_-83E§;
   import §_-L2F§.§_-D2p§;
   import §_-Ty§.§_-K3a§;
   import §_-U1Y§.*;
   import §_-hO§.§_-BY§;
   import §_-l1g§.§_-T2N§;
   import §_-rM§.§_-02j§;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import flash.events.EventDispatcher;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.GetAuthKeyEvent;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.messages.structures.DailyBonusStructure;
   import ru.pragmatix.wormix.objects.*;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.TouchEvent;
   
   public class DailyBonusStructureBinarySerializer implements §_-02j§
   {
      
      public function DailyBonusStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:DailyBonusStructure = DailyBonusStructure(param1);
         param2.writeInt(_loc4_.loginSequence);
         param2.writeInt(_loc4_.dailyBonysType);
         param2.writeInt(_loc4_.dailyBonusCount);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:DailyBonusStructure = new DailyBonusStructure();
         _loc3_.loginSequence = param1.readInt();
         _loc3_.dailyBonysType = param1.readInt();
         _loc3_.dailyBonusCount = param1.readInt();
         return _loc3_;
      }
   }
}

