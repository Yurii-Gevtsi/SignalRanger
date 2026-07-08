package ru.pragmatix.wormix.serialization.structures
{
   import §_-62§.*;
   import §_-CF§.§_-E19§;
   import §_-F2Q§.GestureEvent;
   import §_-b1F§.§_-v2Q§;
   import §_-l1K§.*;
   import §_-r1C§.§_-U2p§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-02j§;
   import feathers.controls.Callout;
   import feathers.layout.HorizontalAlign;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.GestureState;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class RatingProfileStructureBinarySerializer implements §_-02j§
   {
      
      public function RatingProfileStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:RatingProfileStructure = RatingProfileStructure(param1);
         param2.writeUnsignedInt(_loc4_.id);
         param2.writeUTF(_loc4_.name);
         param2.writeInt(_loc4_.armor);
         param2.writeInt(_loc4_.attack);
         param2.writeInt(_loc4_.level);
         param2.writeInt(_loc4_.money);
         param2.writeInt(_loc4_.realMoney);
         param2.writeInt(_loc4_.reactionRate);
         param2.writeInt(_loc4_.rating);
         param2.writeInt(_loc4_.groupCount);
         param2.writeShort(_loc4_.hatId);
         param2.writeShort(_loc4_.raceId);
         param2.writeUTF(UserProfileStructure.getSocialIdsAsString(_loc4_.socialOwnerIds));
         SerializationUtils.writeStructure(param2 as ByteArray,_loc4_.clanMember,ClanMemberStructure);
         param2.writeByte(_loc4_.rank);
         param2.writeByte(_loc4_.skin);
         param2.writeInt(_loc4_.vipExpiryTime);
         param2.writeInt(_loc4_.dailyRating);
         param2.writeInt(_loc4_.oldPlace);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:RatingProfileStructure = new RatingProfileStructure();
         _loc3_.id = param1.readUnsignedInt();
         _loc3_.name = param1.readUTF();
         _loc3_.armor = param1.readInt();
         _loc3_.attack = param1.readInt();
         _loc3_.level = param1.readInt();
         _loc3_.money = param1.readInt();
         _loc3_.realMoney = param1.readInt();
         _loc3_.reactionRate = param1.readInt();
         _loc3_.rating = param1.readInt();
         _loc3_.groupCount = param1.readInt();
         _loc3_.hatId = param1.readShort();
         _loc3_.raceId = param1.readShort();
         _loc3_.artifactId = param1.readShort();
         _loc3_.socialOwnerIds = UserProfileStructure.parseSocialIdsString(param1.readUTF());
         _loc3_.clanMember = SerializationUtils.readStructure(param1 as ByteArray,ClanMemberStructure) as ClanMemberStructure;
         _loc3_.rank = param1.readByte();
         _loc3_.skin = param1.readByte();
         _loc3_.vipExpiryTime = param1.readInt();
         _loc3_.dailyRating = param1.readInt();
         _loc3_.oldPlace = param1.readInt();
         return _loc3_;
      }
   }
}

