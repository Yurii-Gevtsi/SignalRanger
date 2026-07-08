package ru.pragmatix.wormix.serialization.client
{
   import §_-12a§.KamikazeBuff;
   import §_-DJ§.§_-fH§;
   import §_-H3§.*;
   import §_-T2h§.*;
   import §_-Tm§.§_-RF§;
   import §_-d26§.§_-c2E§;
   import §_-g2r§.§_-03A§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.Touch;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.RemoveFromGroup;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpCommand;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   
   public class RemoveFromGroupBinarySerializer implements §_-D2L§
   {
      
      public function RemoveFromGroupBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 13;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:RemoveFromGroup = RemoveFromGroup(param1);
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
         var _loc3_:RemoveFromGroup = new RemoveFromGroup();
         _loc3_.profileId = param1.readUnsignedInt();
         return _loc3_;
      }
   }
}

