package ru.pragmatix.wormix.serialization.wager
{
   import §_-21p§.§_-4w§;
   import §_-31W§.AchievRoomPanel;
   import §_-5Y§.§_-h2c§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-fH§;
   import §_-Ta§.*;
   import §_-j22§.§_-5g§;
   import §_-k2s§.§_-Go§;
   import §_-k2s§.§_-iI§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.hurlant.math.BigInteger;
   import feathers.controls.Button;
   import feathers.core.FeathersControl;
   import feathers.layout.Direction;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.wager.WagerBattleRequest;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import starling.events.Event;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.utils.Pool;
   
   public class WagerBattleRequestBinarySerializer implements §_-D2L§
   {
      
      public function WagerBattleRequestBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 2005;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:WagerBattleRequest = WagerBattleRequest(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.host);
         _loc5_.writeInt(_loc3_.port);
         _loc5_.writeInt(_loc3_.wager);
         _loc5_.writeUnsignedInt(_loc3_.mapId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:WagerBattleRequest = new WagerBattleRequest();
         _loc4_.host = _loc3_.readUTF();
         _loc4_.port = _loc3_.readInt();
         _loc4_.wager = _loc3_.readInt();
         _loc4_.mapId = _loc3_.readUnsignedInt();
         var _loc5_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc5_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
         return _loc4_;
      }
   }
}

