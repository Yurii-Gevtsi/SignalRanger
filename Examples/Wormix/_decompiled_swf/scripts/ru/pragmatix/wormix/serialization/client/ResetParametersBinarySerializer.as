package ru.pragmatix.wormix.serialization.client
{
   import §_-12W§.*;
   import §_-21A§.*;
   import §_-82l§.§_-1I§;
   import §_-91A§.*;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-J2l§;
   import §_-H3§.*;
   import §_-P1B§.§_-b21§;
   import §_-U1Y§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-d2p§.*;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.util.§_-a2P§;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.themes.FontSize;
   import flash.geom.Point;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.GestureState;
   import org.gestouch.core.Touch;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.ResetParameters;
   import ru.pragmatix.wormix.messages.server.EndTurnResponse;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.utils.*;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class ResetParametersBinarySerializer implements §_-D2L§
   {
      
      public function ResetParametersBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 15;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ResetParameters = ResetParameters(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.moneyType);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ResetParameters = new ResetParameters();
         _loc3_.moneyType = param1.readInt();
         return _loc3_;
      }
   }
}

