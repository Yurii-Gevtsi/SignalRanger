package ru.pragmatix.wormix.serialization.client
{
   import §_-12a§.KamikazeBuff;
   import §_-73d§.§_-pT§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.§_-a2W§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-RF§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-p14§.§_-U1D§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-U1A§;
   import §_-zI§.§_-X1q§;
   import com.hurlant.math.*;
   import dragonBones.animation.WorldClock;
   import feathers.themes.FontColor;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.RestrictionItemStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   
   use namespace bi_internal;
   
   public class PvpActionExBinarySerializer implements §_-D2L§
   {
      
      public function PvpActionExBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1007;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc9_:Array = null;
         var _loc10_:Action = null;
         var _loc11_:uint = 0;
         var _loc12_:uint = 0;
         var _loc3_:PvpActionEx = PvpActionEx(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         var _loc6_:uint = 0;
         var _loc7_:uint = _loc3_.firstFrame;
         while(_loc7_ <= _loc3_.lastFrame)
         {
            if(_loc3_.actionsByFrame[_loc7_])
            {
               _loc6_ += 2;
               _loc5_.writeUnsignedInt(_loc7_);
               _loc9_ = _loc3_.actionsByFrame[_loc7_];
               _loc5_.writeUnsignedInt(_loc9_.length);
               for each(_loc10_ in _loc9_)
               {
                  _loc11_ = §_-RF§.§_-SW§(_loc10_.commandId);
                  _loc6_ += 1 + _loc11_;
                  _loc5_.writeUnsignedInt(_loc10_.commandId);
                  _loc12_ = 0;
                  while(_loc12_ < _loc11_)
                  {
                     _loc5_.writeUnsignedInt(_loc10_.params[_loc12_]);
                     _loc12_++;
                  }
               }
            }
            _loc7_++;
         }
         var _loc8_:ByteArray = new ByteArray();
         _loc8_.writeUnsignedInt(_loc3_.firstFrame);
         _loc8_.writeUnsignedInt(_loc3_.lastFrame);
         _loc8_.writeShort(_loc3_.turnNum);
         _loc8_.writeShort(_loc3_.commandNum);
         _loc8_.writeUnsignedInt(_loc3_.battleId);
         _loc8_.writeByte(_loc3_.playerNum);
         _loc8_.writeShort(_loc6_);
         _loc8_.writeBytes(_loc5_);
         §_-U11§.§_-E2n§(_loc8_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc8_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc8_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc14_:uint = 0;
         var _loc15_:uint = 0;
         var _loc16_:uint = 0;
         var _loc17_:uint = 0;
         var _loc18_:Array = null;
         var _loc19_:uint = 0;
         var _loc20_:uint = 0;
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:uint = _loc3_.readUnsignedInt();
         var _loc5_:uint = _loc3_.readUnsignedInt();
         var _loc6_:uint = uint(_loc3_.readShort());
         var _loc7_:uint = uint(_loc3_.readShort());
         var _loc8_:uint = _loc3_.readUnsignedInt();
         var _loc9_:uint = uint(_loc3_.readByte());
         var _loc10_:Object = new Object();
         var _loc11_:uint = uint(_loc3_.readShort());
         while(_loc11_ > 0)
         {
            _loc11_ -= 2;
            _loc14_ = _loc3_.readUnsignedInt();
            _loc15_ = _loc3_.readUnsignedInt();
            _loc10_[_loc14_] = [];
            _loc16_ = 0;
            while(_loc16_ < _loc15_)
            {
               _loc17_ = _loc3_.readUnsignedInt();
               _loc18_ = [];
               _loc19_ = §_-RF§.§_-SW§(_loc17_);
               _loc11_ -= 1 + _loc19_;
               _loc20_ = 0;
               while(_loc20_ < _loc19_)
               {
                  _loc18_.push(_loc3_.readUnsignedInt());
                  _loc20_++;
               }
               (_loc10_[_loc14_] as Array).push(new Action(_loc17_,_loc18_));
               _loc16_++;
            }
         }
         var _loc12_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc12_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
            if(§_-X1j§.§_-41U§)
            {
               §_-X1j§.§_-41U§.§_-A3u§();
            }
         }
         var _loc13_:PvpActionEx = new PvpActionEx(_loc4_,_loc5_,new §_-U1D§(_loc10_));
         _loc13_.battleId = _loc8_;
         _loc13_.turnNum = _loc6_;
         _loc13_.commandNum = _loc7_;
         _loc13_.playerNum = _loc9_;
         return _loc13_;
      }
   }
}

