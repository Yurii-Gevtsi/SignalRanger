package ru.pragmatix.wormix.serialization.client
{
   import §_-31W§.*;
   import §_-A1K§.§_-TA§;
   import §_-C3f§.§_-Z1Y§;
   import §_-L1H§.§_-b2K§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-j22§.§_-5g§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-Ia§;
   import feathers.controls.GroupedList;
   import feathers.controls.Scroller;
   import feathers.controls.renderers.DefaultGroupedListHeaderOrFooterRenderer;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalLayout;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.client.Disconnect;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.SimpleProfileStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.extensions.ParticleAura;
   
   public class DisconnectBinarySerializer implements §_-D2L§
   {
      
      public function DisconnectBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 60;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:Disconnect = Disconnect(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeShort(_loc3_.socialNetId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:Disconnect = new Disconnect();
         _loc3_.socialNetId = param1.readByte();
         return _loc3_;
      }
   }
}

