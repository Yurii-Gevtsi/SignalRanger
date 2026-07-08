package ru.pragmatix.wormix.serialization.client
{
   import §_-12a§.§_-G2l§;
   import §_-21p§.§_-4w§;
   import §_-62§.§_-O2H§;
   import §_-Vg§.*;
   import §_-Y1O§.*;
   import §_-j1w§.§_-B2i§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-lp§;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.client.GetDividend;
   import ru.pragmatix.wormix.messages.server.RepeatPvpBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   
   public class GetDividendBinarySerializer implements §_-D2L§
   {
      
      public function GetDividendBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 133;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetDividend = GetDividend(param1);
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
         return null;
      }
   }
}

