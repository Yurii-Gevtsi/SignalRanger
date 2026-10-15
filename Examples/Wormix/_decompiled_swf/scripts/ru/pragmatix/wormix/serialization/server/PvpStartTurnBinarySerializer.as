package ru.pragmatix.wormix.serialization.server
{
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-v2t§.§_-43O§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.social.§_-N1V§;
   
   public class PvpStartTurnBinarySerializer implements §_-D2L§
   {
      
      public function PvpStartTurnBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1019;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc6_:uint = 0;
         var _loc3_:PvpStartTurn = PvpStartTurn(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.battleId);
         _loc5_.writeByte(_loc3_.nextPlayerNum);
         _loc5_.writeShort(_loc3_.turnNum);
         _loc5_.writeShort(_loc3_.commandNum);
         _loc5_.writeShort(_loc3_.droppedNums.length);
         for each(_loc6_ in _loc3_.droppedNums)
         {
            _loc5_.writeByte(_loc6_);
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
         var _loc4_:PvpStartTurn = new PvpStartTurn();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.nextPlayerNum = _loc3_.readByte();
         _loc4_.turnNum = _loc3_.readShort();
         _loc4_.commandNum = _loc3_.readShort();
         _loc4_.droppedNums = [];
         var _loc5_:uint = uint(_loc3_.readShort());
         var _loc6_:int = 0;
         while(_loc6_ < _loc5_)
         {
            _loc4_.droppedNums.push(_loc3_.readByte());
            _loc6_++;
         }
         var _loc7_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc7_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
         return _loc4_;
      }
   }
}

