package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-937§.§_-n1b§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-N8§.§_-o2w§;
   import §_-YF§.§_-q2v§;
   import §_-b1F§.§_-v2Q§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-52k§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.events.DeviceOrientationEvent;
   import feathers.controls.LayoutGroup;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import flash.errors.IllegalOperationError;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.utils.id.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.main.server.InviteFriendToClan;
   import ru.pragmatix.wormix.messages.client.SetHotkeys;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmoWithArmature;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class SetHotkeysBinarySerializer implements §_-D2L§
   {
      
      public function SetHotkeysBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 128;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SetHotkeys = SetHotkeys(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         SerializationUtils.writeShortArray(_loc5_,_loc3_.hotkeys);
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

