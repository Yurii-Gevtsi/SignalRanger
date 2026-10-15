package ru.pragmatix.wormix.serialization.clans.structures
{
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-02j§;
   import com.distriqt.extension.inappbilling.Purchase;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.AnimationStatesRenderer;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.utils.SystemUtil;
   
   public class ClanMemberTOBinarySerializer implements §_-02j§
   {
      
      public function ClanMemberTOBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ClanMemberTO = new ClanMemberTO();
         _loc3_.profileId = param1.readInt();
         _loc3_.socialProfileId = param1.readUTF();
         _loc3_.clanId = param1.readInt();
         _loc3_.rank = param1.readShort();
         _loc3_.name = param1.readUTF();
         _loc3_.online = param1.readBoolean();
         _loc3_.rating = param1.readInt();
         _loc3_.seasonRating = param1.readInt();
         _loc3_.oldPlace = param1.readInt();
         _loc3_.lastLoginTime = param1.readInt();
         _loc3_.donation = param1.readInt();
         _loc3_.donationCurrSeason = param1.readInt();
         _loc3_.donationPrevSeason = param1.readInt();
         _loc3_.cashedMedals = param1.readInt();
         _loc3_.expelPermit = param1.readBoolean();
         _loc3_.muteMode = param1.readBoolean();
         _loc3_.joinDate = param1.readUnsignedInt();
         _loc3_.hostProfileId = param1.readInt();
         _loc3_.weeklyRating = param1.readInt();
         _loc3_.yesterdayRating = param1.readInt();
         return _loc3_;
      }
   }
}

