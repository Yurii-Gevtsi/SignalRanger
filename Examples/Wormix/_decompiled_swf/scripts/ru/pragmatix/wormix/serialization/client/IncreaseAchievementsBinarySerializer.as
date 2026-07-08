package ru.pragmatix.wormix.serialization.client
{
   import §_-21p§.§_-4w§;
   import §_-62§.§_-W1r§;
   import §_-82a§.SwitchedRenderTextures;
   import §_-937§.§_-n1b§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.*;
   import §_-B2z§.§_-82u§;
   import §_-CF§.§_-x2t§;
   import §_-j22§.§_-5g§;
   import §_-lh§.§_-z2A§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.events.DeviceOrientationEvent;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import feathers.controls.LayoutGroup;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import flash.errors.IllegalOperationError;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.clearTimeout;
   import flash.utils.getQualifiedClassName;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.ai.*;
   import ru.pragmatix.wormix.messages.clans.request.TopClansRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.TopClansResponse;
   import ru.pragmatix.wormix.messages.client.IncreaseAchievements;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class IncreaseAchievementsBinarySerializer implements §_-D2L§
   {
      
      public function IncreaseAchievementsBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 3003;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:IncreaseAchievements = IncreaseAchievements(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.sessionId);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.achievementsIndex);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.achievementsRise);
         SerializationUtils.writeIntArray(_loc5_,_loc3_.boolAchievements);
         _loc5_.writeUnsignedInt(_loc3_.timeScale);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc6_:uint = 0;
         var _loc3_:IncreaseAchievements = new IncreaseAchievements();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.sessionId = _loc4_.readUTF();
         var _loc5_:uint = 0;
         _loc3_.achievementsIndex = new Array();
         _loc6_ = _loc4_.readUnsignedShort();
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            _loc3_.achievementsIndex[_loc5_] = _loc4_.readByte();
            _loc5_++;
         }
         _loc5_ = 0;
         _loc3_.achievementsRise = new Array();
         _loc6_ = _loc4_.readUnsignedShort();
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            _loc3_.achievementsRise[_loc5_] = _loc4_.readShort();
            _loc5_++;
         }
         _loc3_.boolAchievements = [];
         _loc6_ = _loc4_.readUnsignedShort();
         _loc5_ = 0;
         while(_loc5_ < _loc6_)
         {
            _loc3_.boolAchievements[_loc5_] = _loc4_.readInt();
            _loc5_++;
         }
         _loc3_.timeScale = _loc4_.readUnsignedInt();
         _loc3_.isSecure = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

