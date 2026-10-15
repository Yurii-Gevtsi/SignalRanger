package ru.pragmatix.wormix.serialization.wager
{
   import §_-22z§.*;
   import §_-31W§.*;
   import §_-5Y§.§_-h2c§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.§_-a2W§;
   import §_-Y1b§.§_-OP§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-z1g§.§_-F4§;
   import com.greensock.TweenLite;
   import com.hurlant.util.§_-J1N§;
   import com.hurlant.util.der.*;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.response.LoadUserProfilesResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.wager.WagerBattleRequestResult;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class WagerBattleRequestResultBinarySerializer implements §_-D2L§
   {
      
      public function WagerBattleRequestResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 2006;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:WagerBattleRequestResult = new WagerBattleRequestResult();
         _loc4_.host = _loc3_.readUTF();
         _loc4_.port = _loc3_.readInt();
         _loc4_.mapId = _loc3_.readUnsignedInt();
         _loc3_.readShort();
         _loc4_.enemyProfile = UserProfileStructure(§_-616§.§_-fk§(UserProfileStructure).deserializeStructure(_loc3_,null));
         _loc3_.readShort();
         _loc4_.userProfile = UserProfileStructure(§_-616§.§_-fk§(UserProfileStructure).deserializeStructure(_loc3_,null));
         _loc4_.firstPlayerId = _loc3_.readUnsignedInt();
         _loc4_.seed = _loc3_.readInt();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         var _loc5_:int = int(_loc3_.readUnsignedShort());
         _loc4_.reagentsForBattle = new Vector.<int>();
         var _loc6_:int = 0;
         while(_loc6_ < _loc5_)
         {
            _loc4_.reagentsForBattle[_loc6_] = _loc3_.readByte();
            _loc6_++;
         }
         var _loc7_:Boolean = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         if(!_loc7_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
            return null;
         }
         return _loc4_;
      }
   }
}

