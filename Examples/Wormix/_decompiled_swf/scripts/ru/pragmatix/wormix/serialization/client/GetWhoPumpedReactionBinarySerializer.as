package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-F1D§;
   import §_-J3L§.§_-h1D§;
   import §_-T1t§.§_-u19§;
   import §_-Ty§.§_-83N§;
   import §_-Ty§.§_-K3a§;
   import §_-Ty§.§_-U2T§;
   import §_-Ty§.§_-n1U§;
   import §_-Ty§.§_-o1U§;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-a29§.*;
   import §_-hO§.AttackDodgedEvent;
   import §_-j22§.§_-5g§;
   import §_-l1K§.*;
   import §_-nZ§.*;
   import §_-qH§.§_-S25§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s1Q§.Artifacts;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u1T§.*;
   import §_-z20§.*;
   import feathers.controls.Button;
   import feathers.controls.ButtonState;
   import feathers.controls.ToggleButton;
   import flash.geom.*;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialApiSettings;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.client.GetWhoPumpedReaction;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Force;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class GetWhoPumpedReactionBinarySerializer implements §_-D2L§
   {
      
      public function GetWhoPumpedReactionBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 46;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetWhoPumpedReaction = GetWhoPumpedReaction(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeBoolean(_loc3_.todayOnly);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetWhoPumpedReaction = new GetWhoPumpedReaction();
         _loc3_.todayOnly = param1.readBoolean();
         return _loc3_;
      }
   }
}

