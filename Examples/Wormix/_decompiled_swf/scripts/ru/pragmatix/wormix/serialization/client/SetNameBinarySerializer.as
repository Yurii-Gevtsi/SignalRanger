package ru.pragmatix.wormix.serialization.client
{
   import §_-73I§.*;
   import §_-82l§.§_-1I§;
   import §_-A1K§.§_-TA§;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-J2l§;
   import §_-H3§.*;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-U1i§.§_-hK§;
   import §_-Z1K§.§_-q24§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-Ia§;
   import com.greensock.loading.core.*;
   import flash.events.Event;
   import flash.events.ProgressEvent;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.client.SetName;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import starling.display.Image;
   import starling.events.Event;
   import starling.extensions.ParticleAura;
   import starling.utils.Pool;
   
   public class SetNameBinarySerializer implements §_-D2L§
   {
      
      public function SetNameBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 104;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SetName = SetName(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.name);
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

