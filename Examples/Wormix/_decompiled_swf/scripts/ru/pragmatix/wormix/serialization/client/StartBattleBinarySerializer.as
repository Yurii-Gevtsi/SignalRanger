package ru.pragmatix.wormix.serialization.client
{
   import §_-A1K§.§_-TA§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-T2y§.*;
   import §_-j22§.§_-5g§;
   import §_-n1w§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-lp§;
   import flash.display.BitmapData;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.structures.SimpleProfileStructure;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class StartBattleBinarySerializer implements §_-D2L§
   {
      
      public function StartBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 6;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:StartBattle = StartBattle(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.missionId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:StartBattle = new StartBattle();
         _loc3_.missionId = param1.readShort();
         return new StartBattle();
      }
   }
}

