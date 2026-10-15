package ru.pragmatix.wormix.serialization.structures
{
   import §_-G2H§.*;
   import §_-Ta§.*;
   import §_-Y1O§.§_-yA§;
   import §_-ZP§.§_-H1H§;
   import §_-ZP§.§_-w1J§;
   import §_-rM§.§_-02j§;
   import dragonBones.Slot;
   import dragonBones.animation.WorldClock;
   import feathers.controls.List;
   import feathers.data.ListCollection;
   import feathers.events.CollectionEventType;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.ReconnectToSimpleBattleResultStructure;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import starling.core.Starling;
   import starling.display.Image;
   import starling.events.KeyboardEvent;
   import starling.textures.Texture;
   
   public class ReconnectToSimpleBattleResultStructureBinarySerializer implements §_-02j§
   {
      
      public function ReconnectToSimpleBattleResultStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:ReconnectToSimpleBattleResultStructure = ReconnectToSimpleBattleResultStructure(param1);
         param2.writeInt(_loc4_.battleId);
         param2.writeInt(_loc4_.originalBattleId);
         param2.writeShort(_loc4_.missionId);
         param2.writeShort(_loc4_.lastTurnNum);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ReconnectToSimpleBattleResultStructure = new ReconnectToSimpleBattleResultStructure();
         _loc3_.battleId = param1.readInt();
         _loc3_.originalBattleId = param1.readInt();
         _loc3_.missionId = param1.readShort();
         _loc3_.lastTurnNum = param1.readShort();
         return _loc3_;
      }
   }
}

