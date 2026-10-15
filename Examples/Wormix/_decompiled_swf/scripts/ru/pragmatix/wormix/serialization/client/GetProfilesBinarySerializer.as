package ru.pragmatix.wormix.serialization.client
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-DJ§.§_-J2l§;
   import §_-P2i§.§_-b7§;
   import §_-Y1O§.*;
   import §_-Z1K§.*;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.inappbilling.Purchase;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.GetProfiles;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.pvp.messages.client.PvpDropPlayer;
   import starling.events.Event;
   
   public class GetProfilesBinarySerializer implements §_-D2L§
   {
      
      public function GetProfilesBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 5;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetProfiles = GetProfiles(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.sessionKey);
         var _loc6_:uint = 0;
         var _loc7_:uint = _loc3_.ids.length;
         _loc5_.writeShort(_loc7_);
         _loc6_ = 0;
         while(_loc6_ < _loc7_)
         {
            _loc5_.writeUTF(_loc3_.ids[_loc6_]);
            _loc6_++;
         }
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetProfiles = new GetProfiles();
         var _loc4_:uint = 0;
         _loc3_.ids = new Vector.<String>(0);
         var _loc5_:uint = uint(param1.readShort());
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            _loc3_.ids[_loc4_] = param1.readUTF();
            _loc4_++;
         }
         return _loc3_;
      }
   }
}

