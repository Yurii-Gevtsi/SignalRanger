package ru.pragmatix.wormix.serialization.clans.request
{
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-YF§.§_-g2W§;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.PostClanNewsRequest;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   import starling.events.Event;
   
   public class PostClanNewsRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function PostClanNewsRequestBinarySerializer()
      {
         super();
      }
      
      override protected function §_-41X§(param1:Object) : ByteArray
      {
         var _loc2_:PostClanNewsRequest = PostClanNewsRequest(param1);
         var _loc3_:ByteArray = new ByteArray();
         _loc3_.writeUTF(_loc2_.text);
         return _loc3_;
      }
      
      override public function getCommandId() : uint
      {
         return ClanMessages.POST_NEWS_REQUEST;
      }
   }
}

