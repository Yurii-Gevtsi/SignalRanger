package ru.pragmatix.wormix.serialization.server
{
   import §_-J3L§.§_-h1D§;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-j22§.§_-5g§;
   import §_-jF§.ClanInviteRenderer;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import feathers.controls.Button;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ListCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.*;
   import ru.pragmatix.wormix.messages.server.SteamTransactionInitResponse;
   import starling.events.Event;
   
   public class SteamTransactionInitResponseBinarySerializer implements §_-D2L§
   {
      
      public function SteamTransactionInitResponseBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 6002;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SteamTransactionInitResponse = SteamTransactionInitResponse(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.purchaseOrderId);
         _loc5_.writeUTF(_loc3_.purchaseTransactionId);
         _loc5_.writeUTF(_loc3_.countryCode);
         _loc5_.writeUTF(_loc3_.currencyCode);
         _loc5_.writeUTF(_loc3_.socialParams);
         _loc5_.writeInt(_loc3_.errorCode);
         _loc5_.writeUTF(_loc3_.errorMessage);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SteamTransactionInitResponse = new SteamTransactionInitResponse();
         _loc3_.purchaseOrderId = param1.readInt();
         _loc3_.purchaseTransactionId = param1.readUTF();
         _loc3_.countryCode = param1.readUTF();
         _loc3_.currencyCode = param1.readUTF();
         _loc3_.socialParams = param1.readUTF();
         _loc3_.errorCode = param1.readInt();
         _loc3_.errorMessage = param1.readUTF();
         return _loc3_;
      }
   }
}

