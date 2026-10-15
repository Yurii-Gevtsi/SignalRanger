package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-G1p§;
   import §_-82a§.*;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-H3§.*;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-Z1K§.§_-q24§;
   import §_-f1G§.*;
   import §_-g2r§.§_-03A§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-V1n§;
   import feathers.controls.ImageLoader;
   import feathers.layout.RelativePosition;
   import flash.display.BitmapData;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import org.gestouch.core.Touch;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-738§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.client.EndTurn;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.structures.TurnStructure;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   use namespace gestouch_internal;
   
   public class EndTurnBinarySerializer implements §_-D2L§
   {
      
      public function EndTurnBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 120;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc5_:int = 0;
         var _loc3_:EndTurn = EndTurn(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc6_:§_-V1n§ = new §_-V1n§(getTimer());
         var _loc7_:ByteArray = new ByteArray();
         _loc7_.writeShort(_loc3_.missionId);
         _loc7_.writeUnsignedInt(_loc3_.battleId);
         _loc7_.writeShort(_loc6_.§_-Fh§(1,20));
         var _loc8_:ByteArray = new ByteArray();
         §_-616§.§_-fk§(TurnStructure).serializeStructure(_loc3_.turn,_loc8_,null);
         _loc7_.writeShort(_loc8_.length);
         _loc7_.writeBytes(_loc8_);
         _loc7_.writeShort(_loc3_.banType);
         _loc7_.writeUTF(_loc3_.banNote);
         _loc7_.writeUTF(_loc3_.sessionKey);
         §_-U11§.§_-E2n§(_loc7_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc7_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc7_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = null;
         _loc3_ = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:EndTurn = new EndTurn();
         _loc4_.missionId = _loc3_.readShort();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc3_.readShort();
         _loc4_.turn = TurnStructure(§_-616§.§_-fk§(TurnStructure).deserializeStructure(_loc3_,null));
         _loc4_.banType = _loc3_.readShort();
         _loc4_.banNote = _loc3_.readUTF();
         _loc4_.sessionKey = _loc3_.readUTF();
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

