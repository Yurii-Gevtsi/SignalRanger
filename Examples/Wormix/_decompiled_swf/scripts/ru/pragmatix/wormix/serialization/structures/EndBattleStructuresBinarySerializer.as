package ru.pragmatix.wormix.serialization.structures
{
   import §_-21A§.§_-A2x§;
   import §_-A1K§.§_-TA§;
   import §_-J3L§.§_-K2i§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-Y1b§.§_-22N§;
   import §_-b1R§.§_-Bx§;
   import §_-o14§.*;
   import §_-rM§.§_-02j§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import feathers.controls.Label;
   import feathers.layout.VerticalAlign;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.pvp.messages.ex.CancelBattle;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   
   public class EndBattleStructuresBinarySerializer implements §_-02j§
   {
      
      public function EndBattleStructuresBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:EndBattleStructure = EndBattleStructure(param1);
         param2.writeInt(_loc4_.startBattleTime);
         param2.writeInt(_loc4_.finishBattleTime);
         param2.writeShort(_loc4_.result);
         param2.writeShort(_loc4_.type);
         param2.writeInt(_loc4_.expBonus);
         param2.writeShort(_loc4_.missionId);
         SerializationUtils.writeStructureArray(param2 as ByteArray,WeaponStructure,_loc4_.items);
         param2.writeShort(_loc4_.banType);
         param2.writeUTF(_loc4_.banNote);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:EndBattleStructure = new EndBattleStructure();
         _loc3_.startBattleTime = param1.readInt();
         _loc3_.finishBattleTime = param1.readInt();
         _loc3_.result = param1.readShort();
         _loc3_.type = param1.readShort();
         _loc3_.expBonus = param1.readInt();
         _loc3_.missionId = param1.readShort();
         _loc3_.items = SerializationUtils.readStructureArray(param1 as ByteArray,WeaponStructure);
         _loc3_.banType = param1.readShort();
         _loc3_.banNote = param1.readUTF();
         return _loc3_;
      }
   }
}

