package ru.pragmatix.wormix.serialization.client
{
   import §_-73d§.§_-v1o§;
   import §_-A1K§.§_-TA§;
   import §_-Z1K§.§_-q24§;
   import §_-fo§.*;
   import §_-hO§.§_-729§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-522§;
   import feathers.controls.Button;
   import feathers.controls.ButtonState;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.client.SetLocale;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.Jetpack;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class SetLocaleBinarySerializer implements §_-D2L§
   {
      
      public function SetLocaleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 202;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SetLocale = SetLocale(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.locale);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SetLocale = null;
         _loc3_ = new SetLocale();
         _loc3_.locale = param1.readShort();
         return _loc3_;
      }
   }
}

