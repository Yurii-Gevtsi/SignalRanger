package ru.pragmatix.wormix.serialization.client
{
   import §_-A2U§.§_-A23§;
   import §_-B2z§.§_-63i§;
   import §_-Cl§.*;
   import §_-DJ§.§_-J2l§;
   import §_-W§.§_-ix§;
   import §_-fo§.§_-r5§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-zI§.§_-g1a§;
   import com.hurlant.crypto.tls.§_-21v§;
   import feathers.controls.Button;
   import feathers.controls.ButtonGroup;
   import feathers.core.ITextRenderer;
   import flash.display.MovieClip;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.GetReagentsForProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   
   public class GetReagentsForProfileBinarySerializer implements §_-D2L§
   {
      
      public function GetReagentsForProfileBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetReagentsForProfile = GetReagentsForProfile(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.profileId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GetReagentsForProfile = new GetReagentsForProfile();
         _loc3_.profileId = param1.readUnsignedInt();
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return 85;
      }
   }
}

