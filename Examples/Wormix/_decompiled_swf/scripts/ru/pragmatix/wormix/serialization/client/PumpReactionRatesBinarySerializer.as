package ru.pragmatix.wormix.serialization.client
{
   import §_-2H§.§_-91a§;
   import §_-2H§.§_-A1R§;
   import §_-2H§.§_-N15§;
   import §_-2H§.§_-Y1i§;
   import §_-82l§.§_-1I§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-o2k§;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H3§.*;
   import §_-H9§.§_-NX§;
   import §_-P2i§.§_-b7§;
   import §_-d2p§.*;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-cZ§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.worlize.websocket.§_-x2j§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.PumpReactionRates;
   import ru.pragmatix.wormix.messages.server.BuyBattleResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.SwitchTurn;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   
   public class PumpReactionRatesBinarySerializer implements §_-D2L§
   {
      
      public function PumpReactionRatesBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 82;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:PumpReactionRates = PumpReactionRates(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         var _loc6_:uint = 0;
         var _loc7_:uint = _loc3_.friendIds.length;
         _loc5_.writeShort(_loc7_);
         _loc6_ = 0;
         while(_loc6_ < _loc7_)
         {
            _loc5_.writeUnsignedInt(_loc3_.friendIds[_loc6_]);
            _loc6_++;
         }
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:PumpReactionRates = new PumpReactionRates();
         var _loc5_:uint = 0;
         _loc4_.friendIds = new Array();
         var _loc6_:uint = uint(_loc3_.readShort());
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            _loc4_.friendIds[_loc5_] = _loc3_.readUnsignedInt();
            _loc5_++;
         }
         var _loc7_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc7_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER) + " PumpReactionRate",§_-fH§.§_-L2I§);
            return null;
         }
         return _loc4_;
      }
   }
}

