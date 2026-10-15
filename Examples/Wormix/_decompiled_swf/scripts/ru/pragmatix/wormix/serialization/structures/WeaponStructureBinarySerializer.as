package ru.pragmatix.wormix.serialization.structures
{
   import §_-51p§.§_-W2e§;
   import §_-62§.§_-G1p§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-bs§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-hO§.§_-v24§;
   import §_-jF§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-02j§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.GetFriendsForBossResult;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.utils.Pool;
   
   public class WeaponStructureBinarySerializer implements §_-02j§
   {
      
      public function WeaponStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:WeaponStructure = WeaponStructure(param1);
         param2.writeInt(_loc4_.getId());
         param2.writeInt(_loc4_.getCount());
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:int = param1.readInt();
         var _loc4_:int = param1.readInt();
         return new WeaponStructure(_loc3_,_loc4_);
      }
   }
}

