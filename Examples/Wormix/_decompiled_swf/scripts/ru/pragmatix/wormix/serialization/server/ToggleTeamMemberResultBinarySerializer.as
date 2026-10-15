package ru.pragmatix.wormix.serialization.server
{
   import §_-62§.§_-G1p§;
   import §_-l1K§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.*;
   import ru.pragmatix.wormix.messages.server.ToggleTeamMemberResult;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import starling.display.DisplayObject;
   
   public class ToggleTeamMemberResultBinarySerializer implements §_-D2L§
   {
      
      public function ToggleTeamMemberResultBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ToggleTeamMemberResult = new ToggleTeamMemberResult();
         _loc3_.result = param1.readShort();
         _loc3_.teamMemberId = param1.readInt();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return 10106;
      }
   }
}

