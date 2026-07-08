package ru.pragmatix.wormix.serialization.clans.main.server
{
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.serialization.BaseCommandSerializer;
   
   public class GetJoinClansMainRequestBinarySerializer extends BaseCommandSerializer
   {
      
      public function GetJoinClansMainRequestBinarySerializer()
      {
         super();
      }
      
      override public function getCommandId() : uint
      {
         return 98;
      }
   }
}

