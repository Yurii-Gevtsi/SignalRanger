package ru.pragmatix.wormix.serialization.client
{
   import §_-21p§.§_-4w§;
   import §_-73I§.§_-Z2e§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-L2F§.§_-I1q§;
   import §_-L2F§.§_-t2Z§;
   import §_-Y1b§.*;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-T2N§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s1s§.*;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-sQ§.§_-V1n§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-lp§;
   import feathers.data.ArrayCollection;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.messages.server.BuyUnlockMissionResult;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-m1x§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.*;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.extensions.PDParticleSystem;
   
   public class EndBattleBinarySerializer implements §_-D2L§
   {
      
      public function EndBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 84;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:EndBattle = EndBattle(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.result);
         _loc5_.writeShort(_loc3_.type);
         _loc5_.writeInt(_loc3_.expBonus);
         _loc5_.writeInt(_loc3_.battleId);
         _loc5_.writeShort(_loc3_.missionId);
         SerializationUtils.writeStructureArray(_loc5_,WeaponStructure,_loc3_.items);
         _loc5_.writeShort(_loc3_.banType);
         _loc5_.writeUTF(_loc3_.banNote);
         SerializationUtils.writeByteVector(_loc5_,_loc3_.collectedReagents);
         var _loc6_:§_-V1n§ = new §_-V1n§(getTimer());
         _loc5_.writeShort(_loc6_.§_-Fh§(1,20));
         _loc5_.writeShort(_loc3_.totalTurnsCount);
         _loc5_.writeInt(_loc3_.totalDamageToPlayer);
         _loc5_.writeInt(_loc3_.totalDamageToBoss);
         SerializationUtils.writeStructureArray(_loc5_,WeaponStructure,_loc3_.totalUsedItems);
         _loc5_.writeUTF(_loc3_.sessionKey);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:EndBattle = new EndBattle();
         _loc4_.result = _loc3_.readUnsignedInt();
         _loc4_.type = _loc3_.readShort();
         _loc4_.expBonus = _loc3_.readInt();
         _loc4_.battleId = _loc3_.readInt();
         _loc4_.missionId = _loc3_.readShort();
         var _loc5_:uint = 0;
         var _loc6_:uint = _loc3_.readUnsignedShort();
         _loc4_.items = [];
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            _loc3_.readShort();
            _loc4_.items[_loc5_] = §_-616§.§_-fk§(WeaponStructure).deserializeStructure(_loc3_,null);
            _loc5_++;
         }
         _loc4_.banType = _loc3_.readShort();
         _loc4_.banNote = _loc3_.readUTF();
         var _loc7_:int = int(_loc3_.readUnsignedShort());
         _loc4_.collectedReagents = new Vector.<int>(0);
         _loc5_ = 0;
         while(_loc5_ < _loc7_)
         {
            _loc4_.collectedReagents[_loc5_] = _loc3_.readByte();
            _loc5_++;
         }
         _loc4_.totalTurnsCount = _loc3_.readShort();
         _loc4_.totalDamageToPlayer = _loc3_.readInt();
         _loc4_.totalDamageToBoss = _loc3_.readInt();
         _loc6_ = _loc3_.readUnsignedShort();
         _loc4_.totalUsedItems = [];
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            _loc3_.readShort();
            _loc4_.totalUsedItems[_loc5_] = §_-616§.§_-fk§(WeaponStructure).deserializeStructure(_loc3_,null);
            _loc5_++;
         }
         _loc4_.sessionKey = _loc3_.readUTF();
         var _loc8_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc8_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
         return _loc4_;
      }
   }
}

