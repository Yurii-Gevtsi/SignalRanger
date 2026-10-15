package ru.pragmatix.wormix.serialization.structures
{
   import §_-62§.*;
   import §_-91A§.*;
   import §_-L2F§.§_-D2p§;
   import §_-RE§.§_-r2q§;
   import §_-U1Y§.*;
   import §_-YF§.§_-q2v§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-02j§;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayoutData;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.messages.structures.UnitStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class UserProfileStructureBinarySerializer implements §_-02j§
   {
      
      public static const SHORT_LENGTH:int = 2;
      
      public static const INT_LENGTH:int = 4;
      
      public function UserProfileStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc6_:uint = 0;
         var _loc7_:TemporalStuffStructure = null;
         var _loc4_:UserProfileStructure = UserProfileStructure(param1);
         param2.writeUnsignedInt(_loc4_.id);
         param2.writeInt(_loc4_.money);
         param2.writeInt(_loc4_.realMoney);
         param2.writeInt(_loc4_.rating);
         SerializationUtils.writeStructureArray(param2 as ByteArray,UnitStructure,_loc4_.unitsGroup);
         var _loc5_:uint = 0;
         _loc6_ = _loc4_.weaponRecordList.length;
         param2.writeShort(_loc6_);
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            param2.writeShort(_loc4_.weaponRecordList[_loc5_].getId());
            param2.writeShort(_loc4_.weaponRecordList[_loc5_].getCount());
            _loc5_++;
         }
         SerializationUtils.writeShortArray(param2 as ByteArray,_loc4_.stuff);
         _loc6_ = _loc4_.temporalStuff.length;
         param2.writeShort(_loc6_);
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            _loc7_ = _loc4_.temporalStuff[_loc5_];
            param2.writeByte(_loc7_.stuffId);
            param2.writeByte(_loc7_.expireDate);
            _loc5_++;
         }
         param2.writeInt(_loc4_.reactionRate);
         param2.writeUTF(UserProfileStructure.getSocialIdsAsString(_loc4_.socialIds));
         SerializationUtils.writeShortArray(param2 as ByteArray,_loc4_.recipes);
         SerializationUtils.writeStructure(param2 as ByteArray,_loc4_.clanMember,ClanMemberStructure);
         param2.writeByte(_loc4_.extraGroupSlotsCount);
         SerializationUtils.writeStructure(param2 as ByteArray,_loc4_.rentedItems,RentedItemsStructure);
         param2.writeInt(_loc4_.rankPoints);
         param2.writeByte(_loc4_.bestRank);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc5_:int = 0;
         var _loc8_:TemporalStuffStructure = null;
         var _loc3_:UserProfileStructure = new UserProfileStructure();
         _loc3_.id = param1.readUnsignedInt();
         _loc3_.money = param1.readInt();
         _loc3_.realMoney = param1.readInt();
         _loc3_.rating = param1.readInt();
         _loc3_.unitsGroup = SerializationUtils.readStructureArray(param1 as ByteArray,UnitStructure);
         var _loc4_:uint = param1.readUnsignedShort();
         _loc3_.weaponRecordList = [];
         _loc5_ = 0;
         while(_loc5_ < _loc4_)
         {
            _loc3_.weaponRecordList[_loc5_] = new WeaponStructure(param1.readShort(),param1.readShort());
            _loc5_++;
         }
         _loc3_.stuff = SerializationUtils.readShortArray(param1 as ByteArray);
         var _loc6_:int = int(param1.readUnsignedShort());
         _loc3_.temporalStuff = [];
         _loc5_ = 0;
         while(_loc5_ < _loc6_ / (SHORT_LENGTH + INT_LENGTH))
         {
            _loc8_ = new TemporalStuffStructure();
            _loc8_.stuffId = param1.readShort();
            _loc8_.expireDate = param1.readInt();
            _loc3_.temporalStuff[_loc5_] = _loc8_;
            _loc5_++;
         }
         _loc3_.reactionRate = param1.readInt();
         var _loc7_:String = param1.readUTF();
         _loc3_.socialIds = UserProfileStructure.parseSocialIdsString(_loc7_);
         _loc3_.recipes = SerializationUtils.readShortArray(param1 as ByteArray);
         _loc3_.clanMember = SerializationUtils.readStructure(param1 as ByteArray,ClanMemberStructure) as ClanMemberStructure;
         _loc3_.extraGroupSlotsCount = param1.readByte();
         _loc3_.rentedItems = SerializationUtils.readStructure(param1 as ByteArray,RentedItemsStructure) as RentedItemsStructure;
         _loc3_.rankPoints = param1.readInt();
         _loc3_.bestRank = param1.readByte();
         return _loc3_;
      }
   }
}

