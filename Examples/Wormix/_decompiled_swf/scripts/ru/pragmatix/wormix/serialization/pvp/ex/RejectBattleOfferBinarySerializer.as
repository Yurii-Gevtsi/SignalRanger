package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-12W§.§_-221§;
   import §_-2U§.*;
   import §_-31W§.AchievRoomPanel;
   import §_-62§.§_-L1r§;
   import §_-63U§.SafetyArea;
   import §_-931§.§_-02O§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-E1I§.§_-b2n§;
   import §_-K35§.§_-99§;
   import §_-P1B§.§_-b21§;
   import §_-YF§.§_-q2v§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-vO§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-4l§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import com.hurlant.math.*;
   import feathers.controls.Button;
   import feathers.controls.LayoutGroup;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.TiledColumnsLayout;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.pvp.messages.ex.RejectBattleOffer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class RejectBattleOfferBinarySerializer implements §_-D2L§
   {
      
      public function RejectBattleOfferBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return RejectBattleOffer.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:RejectBattleOffer = RejectBattleOffer(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.profileId);
         _loc5_.writeUTF(_loc3_.profileSocialId);
         _loc5_.writeByte(_loc3_.socialNetworkId);
         _loc5_.writeUTF(_loc3_.authKey);
         _loc5_.writeUTF(_loc3_.playerName);
         _loc5_.writeShort(_loc3_.mainServerId);
         _loc5_.writeUnsignedInt(_loc3_.battleId);
         _loc5_.writeBoolean(_loc3_.manual);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return {};
      }
   }
}

