package ru.pragmatix.wormix.serialization.client
{
   import §_-31N§.§_-42G§;
   import §_-B1y§.*;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F2M§.*;
   import §_-H3§.*;
   import §_-L1H§.§_-C3e§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-m1g§.§_-K2W§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u2J§.§_-T1K§;
   import §_-z1g§.§_-82X§;
   import feathers.controls.ProgressBar;
   import feathers.core.ITextRenderer;
   import feathers.layout.Direction;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.GetRating;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   
   public class GetRatingBinarySerializer implements §_-D2L§
   {
      
      public function GetRatingBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 19;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetRating = GetRating(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.ratingType);
         _loc5_.writeShort(_loc3_.battleWager);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return new GetRating();
      }
   }
}

