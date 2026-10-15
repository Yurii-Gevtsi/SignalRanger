package ru.pragmatix.wormix.serialization.server
{
   import §_-W§.§_-ix§;
   import §_-Y1Q§.§_-61Z§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.themes.FontSize;
   import flash.text.TextFormatAlign;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.EndTurnResponse;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.utils.Result;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.events.Event;
   
   public class EndTurnResponseBinarySerializer implements §_-D2L§
   {
      
      public function EndTurnResponseBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10120;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:EndTurnResponse = new EndTurnResponse();
         _loc3_.result = param1.readShort();
         _loc3_.battleId = param1.readUnsignedInt();
         _loc3_.turnNum = param1.readShort();
         _loc3_.lastTurnNum = param1.readShort();
         return _loc3_;
      }
   }
}

