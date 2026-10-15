package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-W1r§;
   import §_-82l§.§_-1I§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.*;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-fH§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-XA§;
   import §_-Y1O§.§_-V23§;
   import §_-fo§.§_-r5§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import feathers.controls.Button;
   import feathers.core.ToggleGroup;
   import flash.display.MovieClip;
   import flash.events.TimerEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.AnimationStatesRenderer;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.AuditActionsRequest;
   import ru.pragmatix.wormix.messages.clans.response.AuditActionsResponse;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.textures.Texture;
   
   public class PvpEndTurnBinarySerializer implements §_-D2L§
   {
      
      public function PvpEndTurnBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1014;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:PvpEndTurn = PvpEndTurn(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.turnNum);
         _loc5_.writeShort(_loc3_.commandNum);
         _loc5_.writeUnsignedInt(_loc3_.battleId);
         _loc5_.writeByte(_loc3_.playerNum);
         SerializationUtils.writeByteArray(_loc5_,_loc3_.eliminated);
         SerializationUtils.writeByteArray(_loc5_,_loc3_.collectedReagents);
         SerializationUtils.writeStructureArray(_loc5_,WeaponStructure,_loc3_.items);
         _loc5_.writeInt(_loc3_.battleState);
         _loc5_.writeByte(_loc3_.forced ? 1 : 0);
         _loc5_.writeShort(_loc3_.banType);
         _loc5_.writeUTF(_loc3_.banNote);
         SerializationUtils.writeByteArray(_loc5_,_loc3_.droppedUnits);
         SerializationUtils.writeShortArray(_loc5_,_loc3_.participantsHealthInPercent);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = null;
         var _loc4_:PvpEndTurn = null;
         _loc3_ = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         _loc4_ = new PvpEndTurn();
         _loc4_.turnNum = _loc3_.readShort();
         _loc4_.commandNum = _loc3_.readShort();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.playerNum = _loc3_.readByte();
         _loc4_.eliminated = SerializationUtils.readByteArray(_loc3_);
         _loc4_.collectedReagents = SerializationUtils.readByteArray(_loc3_);
         _loc4_.items = SerializationUtils.readStructureArray(_loc3_,WeaponStructure);
         _loc4_.battleState = _loc3_.readInt();
         _loc4_.forced = _loc3_.readByte() != 0;
         _loc4_.banType = _loc3_.readShort();
         _loc4_.banNote = _loc3_.readUTF();
         _loc4_.droppedUnits = SerializationUtils.readByteArray(_loc3_);
         _loc4_.participantsHealthInPercent = SerializationUtils.readShortArray(_loc3_);
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

