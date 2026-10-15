package ru.pragmatix.wormix.serialization.client
{
   import §_-21p§.§_-4w§;
   import §_-62§.§_-W1r§;
   import §_-937§.§_-n1b§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-D3A§.§_-TZ§;
   import §_-P1B§.§_-b21§;
   import §_-b1F§.§_-v2Q§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.events.DeviceOrientationEvent;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.messages.client.DistributePoints;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.utils.*;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class DistributePointsBinarySerializer implements §_-D2L§
   {
      
      public function DistributePointsBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 14;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:DistributePoints = DistributePoints(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.armor);
         _loc5_.writeInt(_loc3_.attack);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:DistributePoints = new DistributePoints();
         _loc3_.armor = param1.readUnsignedInt();
         _loc3_.attack = param1.readInt();
         return _loc3_;
      }
   }
}

