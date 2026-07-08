package ru.pragmatix.wormix.serialization.client
{
   import §_-12W§.§_-221§;
   import §_-62§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-RE§.§_-r2q§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1b§.*;
   import §_-j1w§.§_-lD§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.clearTimeout;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.client.SendWipeConfirmCode;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.common.post.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class SendWipeConfirmCodeBinarySerializer implements §_-D2L§
   {
      
      public function SendWipeConfirmCodeBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 52;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SendWipeConfirmCode = SendWipeConfirmCode(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.level);
         _loc5_.writeInt(_loc3_.experience);
         _loc5_.writeInt(_loc3_.rating);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SendWipeConfirmCode = new SendWipeConfirmCode();
         _loc3_.level = param1.readInt();
         _loc3_.experience = param1.readInt();
         _loc3_.rating = param1.readInt();
         return _loc3_;
      }
   }
}

