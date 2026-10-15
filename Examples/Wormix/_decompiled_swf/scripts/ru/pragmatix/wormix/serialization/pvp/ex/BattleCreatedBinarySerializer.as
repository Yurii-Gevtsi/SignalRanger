package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-Y1O§.*;
   import §_-YF§.§_-G2U§;
   import §_-aM§.*;
   import §_-fo§.§_-r5§;
   import §_-j1w§.§_-B2i§;
   import §_-j22§.§_-5g§;
   import §_-p18§.§_-83X§;
   import §_-p18§.§_-V14§;
   import §_-r1C§.§_-U2p§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-V1n§;
   import §_-t10§.*;
   import §_-z1g§.§_-82X§;
   import feathers.events.FeathersEventType;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.display.MovieClip;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.UfoTargetPointer;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.GetDividend;
   import ru.pragmatix.wormix.messages.server.LinkResult;
   import ru.pragmatix.wormix.messages.server.RepeatPvpBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.PreCalculatedPoints;
   import ru.pragmatix.wormix.messages.structures.PvpProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreated;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import ru.pragmatix.wormix.weapons.ammo.type.RopeHook;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class BattleCreatedBinarySerializer implements §_-D2L§
   {
      
      public function BattleCreatedBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return BattleCreated.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:BattleCreated = new BattleCreated();
         _loc4_.battleType = PvpBattleType.valueOf(_loc3_.readShort());
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.wager = §_-82X§.valueOf(_loc3_.readShort());
         _loc4_.mapId = _loc3_.readUnsignedInt();
         SerializationUtils.readShortVector(_loc3_,_loc4_.missionIds);
         _loc4_.playerStructs = Vector.<PvpProfileStructure>(SerializationUtils.readStructureVector(_loc3_,PvpProfileStructure));
         _loc4_.seed = _loc3_.readInt();
         _loc4_.reagentsForBattle = SerializationUtils.readByteVector(_loc3_);
         _loc4_.questId = _loc3_.readShort();
         _loc4_.preCalculatedPoints = SerializationUtils.readStructureArray(_loc3_,PreCalculatedPoints);
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
            return null;
         }
         return _loc4_;
      }
   }
}

