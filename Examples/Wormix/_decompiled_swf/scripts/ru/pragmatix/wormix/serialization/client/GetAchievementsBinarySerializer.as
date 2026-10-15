package ru.pragmatix.wormix.serialization.client
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-J2l§;
   import §_-G3w§.*;
   import §_-YF§.§_-q2v§;
   import §_-b1§.*;
   import §_-j22§.§_-5g§;
   import §_-kP§.*;
   import §_-l1K§.*;
   import §_-m1g§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2W§.§_-21w§;
   import feathers.controls.TabBar;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.InteractiveObject;
   import flash.display.Stage;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.client.GetAchievements;
   import ru.pragmatix.wormix.weapons.Girder;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   
   public class GetAchievementsBinarySerializer implements §_-D2L§
   {
      
      public function GetAchievementsBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 3002;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetAchievements = GetAchievements(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         if(_loc3_.profileId == null)
         {
            return;
         }
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.profileId);
         _loc5_.writeByte(_loc3_.investedAwardPoints);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetAchievements = new GetAchievements();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.profileId = _loc4_.readUTF();
         _loc3_.investedAwardPoints = _loc4_.readByte();
         _loc3_.isSecure = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

