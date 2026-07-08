package ru.pragmatix.wormix.serialization.server
{
   import §_-62§.§_-G1p§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.§_-A23§;
   import §_-DJ§.§_-fH§;
   import §_-L1w§.§_-CD§;
   import §_-Y1Q§.§_-C2D§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.utils.Align;
   
   public class PvpEndBattleBinarySerializer implements §_-D2L§
   {
      
      public function PvpEndBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1021;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         §_-J2h§.§_-n1V§(this + ".deserializeCommand... <<");
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:PvpEndBattle = new PvpEndBattle();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.turnNum = _loc3_.readShort();
         _loc4_.commandNum = _loc3_.readShort();
         _loc4_.battleResult = _loc3_.readShort();
         _loc4_.ratingPoints = _loc3_.readInt();
         _loc4_.rankPoints = _loc3_.readInt();
         _loc4_.awards = SerializationUtils.readStructureArray(_loc3_,GenericAwardStructure);
         _loc4_.advPurchases = SerializationUtils.readStructureArray(_loc3_,AdvertisingPurchaseStructure);
         §_-J2h§.§_-n1V§(this + " advPurchases");
         _loc4_.confirmed = _loc3_.readByte() != 0;
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
         §_-J2h§.§_-n1V§(this + ".deserializeCommand... >>");
         return _loc4_;
      }
   }
}

