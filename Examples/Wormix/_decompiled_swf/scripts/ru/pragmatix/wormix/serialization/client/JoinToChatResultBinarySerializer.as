package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.*;
   import §_-A1K§.§_-TA§;
   import §_-J3L§.§_-K2i§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-lD§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-k2s§.§_-e1k§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.math.*;
   import config.§_-V2w§;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.client.JoinToChatResult;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.display.DisplayObject;
   
   use namespace bi_internal;
   
   public class JoinToChatResultBinarySerializer implements §_-D2L§
   {
      
      public function JoinToChatResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10301;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:JoinToChatResult = JoinToChatResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         SerializationUtils.writeStructureArray(_loc5_,ChatMessage,_loc3_.chatHistory);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:JoinToChatResult = new JoinToChatResult();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.chatHistory = SerializationUtils.readStructureArray(_loc4_,ChatMessage);
         return _loc3_;
      }
   }
}

