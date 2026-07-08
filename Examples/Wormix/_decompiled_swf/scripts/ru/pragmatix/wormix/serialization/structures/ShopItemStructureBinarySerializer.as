package ru.pragmatix.wormix.serialization.structures
{
   import §_-62§.§_-Q2c§;
   import §_-A2U§.*;
   import §_-YF§.§_-q2v§;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.crypto.tls.§_-21v§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.server.RepeatBossBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class ShopItemStructureBinarySerializer implements §_-02j§
   {
      
      public function ShopItemStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:ShopItemStructure = ShopItemStructure(param1);
         param2.writeUnsignedInt(_loc4_.id);
         param2.writeInt(_loc4_.count);
         param2.writeInt(_loc4_.moneyType);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ShopItemStructure = new ShopItemStructure();
         _loc3_.id = param1.readUnsignedInt();
         _loc3_.count = param1.readInt();
         _loc3_.moneyType = param1.readInt();
         return _loc3_;
      }
   }
}

