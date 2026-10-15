package ru.pragmatix.wormix.serialization.server
{
   import §_-CR§.§_-72p§;
   import §_-K35§.§_-99§;
   import §_-L1H§.§_-b2K§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-j22§.§_-5g§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-wD§.*;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.messages.client.SteamProductsInfoRequest;
   import ru.pragmatix.wormix.messages.server.GetDividendResult;
   import ru.pragmatix.wormix.messages.server.SteamProductsInfoResponse;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   
   public class GetDividendResultBinarySerializer implements §_-D2L§
   {
      
      public function GetDividendResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10133;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetDividendResult = GetDividendResult(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.result);
         _loc5_.writeInt(_loc3_.money);
         _loc5_.writeInt(_loc3_.realMoney);
         _loc5_.writeUTF(_loc3_.sessionKey);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetDividendResult = new GetDividendResult();
         _loc3_.result = param1.readShort();
         _loc3_.money = param1.readInt();
         _loc3_.realMoney = param1.readInt();
         _loc3_.sessionKey = param1.readUTF();
         return _loc3_;
      }
   }
}

