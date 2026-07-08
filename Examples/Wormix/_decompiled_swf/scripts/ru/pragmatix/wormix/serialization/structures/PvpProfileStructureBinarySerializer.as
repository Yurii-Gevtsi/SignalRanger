package ru.pragmatix.wormix.serialization.structures
{
   import §_-533§.§_-c2b§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-zF§;
   import §_-Z1K§.§_-q24§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.structures.PvpProfileStructure;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.messages.structures.UnitStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.ammo.DynamiteStick;
   import starling.events.Event;
   import starling.utils.AssetManager;
   
   public class PvpProfileStructureBinarySerializer extends UserProfileStructureBinarySerializer
   {
      
      public function PvpProfileStructureBinarySerializer()
      {
         super();
      }
      
      override public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         super.serializeStructure(param1,param2,param3);
      }
      
      override public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:int = 0;
         var _loc4_:uint = 0;
         var _loc8_:TemporalStuffStructure = null;
         var _loc5_:PvpProfileStructure = new PvpProfileStructure();
         _loc5_.id = param1.readUnsignedInt();
         _loc5_.money = param1.readInt();
         _loc5_.realMoney = param1.readInt();
         _loc5_.rating = param1.readInt();
         _loc5_.unitsGroup = SerializationUtils.readStructureArray(param1 as ByteArray,UnitStructure);
         _loc4_ = param1.readUnsignedShort();
         _loc5_.weaponRecordList = [];
         _loc3_ = 0;
         while(_loc3_ < _loc4_)
         {
            _loc5_.weaponRecordList[_loc3_] = new WeaponStructure(param1.readShort(),param1.readShort());
            _loc3_++;
         }
         _loc5_.stuff = SerializationUtils.readShortArray(param1 as ByteArray);
         var _loc6_:int = int(param1.readUnsignedShort());
         _loc5_.temporalStuff = [];
         _loc3_ = 0;
         while(_loc3_ < _loc6_ / (SHORT_LENGTH + INT_LENGTH))
         {
            _loc8_ = new TemporalStuffStructure();
            _loc8_.stuffId = param1.readShort();
            _loc8_.expireDate = param1.readInt();
            _loc5_.temporalStuff[_loc3_] = _loc8_;
            _loc3_++;
         }
         _loc5_.reactionRate = param1.readInt();
         var _loc7_:String = param1.readUTF();
         _loc5_.socialIds = UserProfileStructure.parseSocialIdsString(_loc7_);
         _loc5_.recipes = SerializationUtils.readShortArray(param1 as ByteArray);
         _loc5_.clanMember = SerializationUtils.readStructure(param1 as ByteArray,ClanMemberStructure) as ClanMemberStructure;
         _loc5_.extraGroupSlotsCount = param1.readByte();
         _loc5_.rentedItems = SerializationUtils.readStructure(param1 as ByteArray,RentedItemsStructure) as RentedItemsStructure;
         _loc5_.rankPoints = param1.readInt();
         _loc5_.bestRank = param1.readByte();
         _loc5_.socialName = param1.readUTF();
         _loc5_.socialNetworkId = param1.readByte();
         _loc5_.playerNum = param1.readByte();
         _loc5_.teamNum = param1.readByte();
         _loc5_.dailyRating = param1.readInt();
         _loc5_.backpackConf = SerializationUtils.readShortArray(param1 as ByteArray);
         _loc5_.activeTeamMembers = SerializationUtils.readStructureArray(param1 as ByteArray,UnitStructure);
         _loc5_.seasonsBestRank = SerializationUtils.readByteArray(param1 as ByteArray);
         return _loc5_;
      }
   }
}

