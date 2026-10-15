package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-21p§.§_-4w§;
   import §_-j1w§.§_-B2i§;
   import §_-p24§.*;
   import com.hurlant.math.*;
   import flash.utils.ByteArray;
   import org.gestouch.core.Touch;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.ListClansRequest;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.DisplayObject;
   
   use namespace bi_internal;
   
   public class ListClansRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function ListClansRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:ListClansRequest = ListClansRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeUTF(_loc2_.searchPhrase);
         SerializationUtils.writeShortVector(_loc3_,_loc2_.reviewStates);
         _loc3_.writeInt(_loc2_.offset);
         _loc3_.writeInt(_loc2_.limit);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.LIST_CLANS_REQUEST;
      }
   }
}

