package ru.pragmatix.wormix.serialization.client
{
   import §_-31N§.§_-42G§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-73d§.§_-x2C§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-Z1K§.§_-q24§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-d2j§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-Zd§;
   import §_-z1g§.§_-82X§;
   import com.distriqt.extension.cloudstorage.events.KeyValueStoreEvent;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.client.GetAchievements;
   import ru.pragmatix.wormix.messages.client.NotifyProfile;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.core.Starling;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class NotifyProfileSerializer implements §_-D2L§
   {
      
      public function NotifyProfileSerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 201;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:NotifyProfile = NotifyProfile(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.recipientProfileId);
         _loc5_.writeShort(_loc3_.socialType.servercode);
         _loc5_.writeUTF(_loc3_.locKey);
         _loc5_.writeShort(_loc3_.locArgs.length);
         var _loc6_:uint = 0;
         while(_loc6_ < _loc3_.locArgs.length)
         {
            _loc5_.writeUTF(_loc3_.locArgs[_loc6_]);
            _loc6_++;
         }
         _loc5_.writeInt(_loc3_.delay);
         _loc5_.writeInt(_loc3_.timeToLive);
         _loc5_.writeUTF(_loc3_.sessionKey);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:NotifyProfile = new NotifyProfile();
         _loc4_.recipientProfileId = _loc3_.readUnsignedInt();
         _loc4_.socialType = SocialType.valueOfServerCode(_loc3_.readByte());
         _loc4_.locKey = _loc3_.readUTF();
         var _loc5_:uint = _loc3_.readUnsignedShort();
         _loc4_.locArgs = new Vector.<String>(_loc5_);
         var _loc6_:int = 0;
         while(_loc6_ < _loc5_)
         {
            _loc4_.locArgs[_loc6_] = _loc3_.readUTF();
            _loc6_++;
         }
         _loc4_.delay = _loc3_.readInt();
         _loc4_.timeToLive = _loc3_.readInt();
         _loc4_.sessionKey = _loc3_.readUTF();
         var _loc7_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc7_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER) + " NotifyProfile",§_-fH§.§_-L2I§);
            return null;
         }
         return _loc4_;
      }
   }
}

