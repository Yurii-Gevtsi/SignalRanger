package ru.pragmatix.wormix.serialization.client
{
   import §_-51p§.§_-W2e§;
   import §_-5Y§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-a3§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-E19§;
   import §_-CR§.§_-S2o§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-T22§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-P1z§.*;
   import §_-SP§.§_-M4§;
   import §_-Tm§.§_-RF§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y1Q§.§_-aG§;
   import §_-Z1K§.§_-q24§;
   import §_-ZP§.§_-b23§;
   import §_-hO§.§_-v24§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.math.*;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.client.LeaveFromChat;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import ru.pragmatix.wormix.weapons.ammo.CrossbowBolt;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class LeaveFromChatBinarySerializer implements §_-D2L§
   {
      
      public function LeaveFromChatBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 303;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:LeaveFromChat = LeaveFromChat(param1);
         var _temp_1:* = new §_-5g§();
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return new LeaveFromChat();
      }
   }
}

