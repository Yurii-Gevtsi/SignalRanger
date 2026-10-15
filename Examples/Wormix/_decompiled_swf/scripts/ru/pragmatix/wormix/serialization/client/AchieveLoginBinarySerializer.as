package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-zF§;
   import §_-73I§.*;
   import §_-CF§.§_-E19§;
   import §_-E1I§.*;
   import §_-Y1O§.*;
   import §_-j22§.§_-5g§;
   import §_-jF§.§_-lx§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-e1k§;
   import §_-k2s§.§_-iI§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.worlize.websocket.§_-x2j§;
   import config.§_-V2w§;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import flash.display.DisplayObject;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.geom.ColorTransform;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.client.AchieveLogin;
   import ru.pragmatix.wormix.messages.structures.AwardTypes;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import starling.display.DisplayObject;
   
   public class AchieveLoginBinarySerializer implements §_-D2L§
   {
      
      public function AchieveLoginBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 3001;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:AchieveLogin = AchieveLogin(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.applicationId);
         _loc5_.writeUTF(_loc3_.socialNetworkId);
         _loc5_.writeUTF(_loc3_.id);
         _loc5_.writeUTF(_loc3_.authKey);
         _loc5_.writeBoolean(_loc3_.sendAchievements);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:AchieveLogin = new AchieveLogin();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.applicationId = _loc4_.readUTF();
         _loc3_.socialNetworkId = _loc4_.readUTF();
         _loc3_.id = _loc4_.readUTF();
         _loc3_.authKey = _loc4_.readUTF();
         _loc3_.sendAchievements = _loc4_.readBoolean();
         return _loc3_;
      }
   }
}

