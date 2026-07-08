package ru.pragmatix.wormix.serialization.wager
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-C2M§.§_-u2T§;
   import §_-CF§.§_-x2t§;
   import §_-Cl§.§_-L2z§;
   import §_-Cl§.§_-li§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-H3§.*;
   import §_-S1N§.§_-RO§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1b§.§_-s2Z§;
   import §_-ZP§.§_-w1J§;
   import §_-f1G§.§_-ol§;
   import §_-j22§.§_-5g§;
   import §_-m1g§.§_-K2W§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-z1g§.*;
   import flash.display.DisplayObject;
   import flash.events.TimerEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.ShopResult;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-WU§;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.pvp.messages.wager.WagerBattleRequestError;
   import ru.pragmatix.wormix.serialization.server.*;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class WagerBattleRequestErrorBinarySerializer implements §_-D2L§
   {
      
      public function WagerBattleRequestErrorBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 2008;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:WagerBattleRequestError = WagerBattleRequestError(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.code);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:WagerBattleRequestError = new WagerBattleRequestError();
         _loc3_.code = param1.readShort();
         return _loc3_;
      }
   }
}

