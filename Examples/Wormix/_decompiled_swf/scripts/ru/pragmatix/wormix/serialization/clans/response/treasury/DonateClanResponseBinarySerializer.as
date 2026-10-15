package ru.pragmatix.wormix.serialization.clans.response.treasury
{
   import §_-63M§.§_-F3f§;
   import §_-82l§.§_-A2Y§;
   import §_-H2g§.§_-di§;
   import §_-RE§.§_-52p§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-Z1K§.§_-A2n§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-zI§.§_-X1q§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.hurlant.util.der.*;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.clans.response.treasury.DonateClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class DonateClanResponseBinarySerializer implements §_-D2L§
   {
      
      public function DonateClanResponseBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:DonateClanResponse = new DonateClanResponse();
         _loc3_.serviceResult = param1.readShort();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return ClanMessages.DONATE_RESPONSE;
      }
   }
}

