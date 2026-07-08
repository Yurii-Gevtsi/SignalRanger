package ru.pragmatix.wormix.messages.structures
{
   import §_-rM§.§_-61j§;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.server.AssembleStuffResult;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   
   public class SimpleProfileStructure
   {
      
      public var id:int;
      
      public var name:String;
      
      public var armor:int;
      
      public var attack:int;
      
      public var level:int;
      
      public var money:int;
      
      public var realMoney:int;
      
      public var reactionRate:int;
      
      public var rating:int;
      
      public var groupCount:int;
      
      public var hatId:int;
      
      public var raceId:int;
      
      public var artifactId:int;
      
      public var socialOwnerIds:Dictionary;
      
      public var clanMember:ClanMemberStructure;
      
      public var rank:int;
      
      public var skin:int;
      
      public var vipExpiryTime:int;
      
      public function SimpleProfileStructure()
      {
         super();
      }
   }
}

