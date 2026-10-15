package ru.pragmatix.flox.social.utils.storage
{
   import §_-31W§.§_-g16§;
   import §_-62§.§_-O2H§;
   import §_-A2F§.§_-4B§;
   import §_-OJ§.§_-L3B§;
   import §_-OJ§.§_-sY§;
   import §_-RE§.§_-D3v§;
   import §_-T2h§.§_-92J§;
   import §_-Y23§.§_-ZE§;
   import §_-d26§.§_-d1H§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-xu§.§_-91H§;
   import feathers.controls.List;
   import feathers.data.ArrayCollection;
   import feathers.layout.Direction;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import flash.data.EncryptedLocalStore;
   import flash.events.MouseEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.swiftsuspenders.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.server.RepeatBossBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class EncryptedLocalStorageCookie
   {
      
      public function EncryptedLocalStorageCookie()
      {
         super();
      }
      
      public static function getStringValue(param1:String) : String
      {
         var _loc3_:String = null;
         var _loc2_:ByteArray = EncryptedLocalStore.getItem(param1);
         if(_loc2_)
         {
            return _loc2_.readUTFBytes(_loc2_.length);
         }
         return "";
      }
      
      public static function setStringValue(param1:String, param2:String) : void
      {
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeUTFBytes(param2);
         EncryptedLocalStore.setItem(param1,_loc3_);
      }
   }
}

