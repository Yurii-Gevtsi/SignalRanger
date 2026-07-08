package ru.pragmatix.wormix.serialization.client
{
   import §_-5Y§.§_-h2c§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2S§.§_-d2z§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-DJ§.§_-fH§;
   import §_-H3§.*;
   import §_-RE§.§_-r2q§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-fo§.§_-A29§;
   import §_-fo§.§_-r5§;
   import §_-hO§.§_-v24§;
   import §_-j22§.§_-5g§;
   import §_-n1w§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-zI§.§_-8D§;
   import com.hurlant.crypto.tls.*;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.SteamTransactionCommitRequest;
   import ru.pragmatix.wormix.messages.server.GetDividendResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.display.BlendMode;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.TextureSmoothing;
   
   use namespace bi_internal;
   
   public class SteamTransactionCommitRequestBinarySerializer implements §_-D2L§
   {
      
      public function SteamTransactionCommitRequestBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 6003;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SteamTransactionCommitRequest = SteamTransactionCommitRequest(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.purchaseOrderId);
         _loc5_.writeUTF(_loc3_.purchaseTransactionId);
         _loc5_.writeUTF(_loc3_.sessionId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:SteamTransactionCommitRequest = new SteamTransactionCommitRequest();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.purchaseOrderId = _loc4_.readInt();
         _loc3_.purchaseTransactionId = _loc4_.readUTF();
         _loc3_.sessionId = _loc4_.readUTF();
         _loc3_.isSecure = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

