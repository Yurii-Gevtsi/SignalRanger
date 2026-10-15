package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-A1K§.§_-7u§;
   import §_-C2M§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-J31§.§_-e2J§;
   import §_-S1N§.§_-RO§;
   import §_-Z1w§.§_-s1x§;
   import §_-ZP§.§_-w1J§;
   import §_-f1G§.§_-kf§;
   import §_-k1u§.§_-Vr§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-t1w§.§_-E6§;
   import §_-t1w§.§_-G2O§;
   import §_-t1w§.§_-HW§;
   import §_-t1w§.§_-r1k§;
   import §_-t1w§.§_-uf§;
   import §_-u13§.MAC;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.crypto.*;
   import flash.display.*;
   import flash.events.*;
   import flash.utils.*;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.BossTypesTab;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-YD§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   use namespace gestouch_internal;
   
   public class StartPvpBattleBinarySerializer implements §_-D2L§
   {
      
      public function StartPvpBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return StartPvpBattle.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:StartPvpBattle = new StartPvpBattle();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.sessionKey = _loc3_.readUTF();
         _loc4_.playerNum = _loc3_.readByte();
         return _loc4_;
      }
   }
}

