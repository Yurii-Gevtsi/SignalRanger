package ru.pragmatix.wormix.serialization.client
{
   import §_-03T§.§_-2p§;
   import §_-DJ§.§_-J2l§;
   import §_-b1F§.§_-G4§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.util.der.§_-UO§;
   import flash.geom.Point;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import flash.utils.CompressionAlgorithm;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.client.DesyncLog;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.§_-kQ§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   
   public class DesyncLogBinarySerializer implements §_-D2L§
   {
      
      public function DesyncLogBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 134;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:int = 0;
         var _loc9_:String = null;
         var _loc10_:ByteArray = null;
         var _loc11_:int = 0;
         var _loc4_:DesyncLog = DesyncLog(param1);
         var _loc5_:§_-5g§ = new §_-5g§();
         _loc5_.§_-c2D§(this.getCommandId());
         var _loc6_:ByteArray = new ByteArray();
         _loc6_.writeUnsignedInt(_loc4_.battleId);
         _loc6_.writeShort(_loc4_.battleType);
         _loc6_.writeShort(_loc4_.wager);
         _loc6_.writeInt(_loc4_.mapId);
         var _loc7_:int = _loc4_.missionIds != null ? int(_loc4_.missionIds.length) : 0;
         _loc6_.writeShort(_loc7_);
         if(_loc7_ > 0)
         {
            _loc3_ = 0;
            while(_loc3_ < _loc7_)
            {
               _loc6_.writeShort(_loc4_.missionIds[_loc3_]);
               _loc3_++;
            }
         }
         var _loc8_:String = "";
         for each(_loc9_ in _loc4_.battleLogs)
         {
            _loc8_ += _loc9_ + "\n";
         }
         _loc10_ = new ByteArray();
         _loc10_.writeUTFBytes(_loc8_);
         _loc10_.compress(CompressionAlgorithm.LZMA);
         _loc11_ = int(_loc10_.length);
         _loc6_.writeShort(_loc11_);
         if(_loc11_ > 0)
         {
            _loc6_.writeBytes(_loc10_);
         }
         _loc5_.setLength(_loc6_.length);
         _loc5_.write(param2);
         param2.writeBytes(_loc6_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
   }
}

