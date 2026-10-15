package ru.pragmatix.wormix.messages.clans.main.server
{
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import flash.display.*;
   import flash.events.*;
   import flash.geom.Point;
   import flash.utils.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   
   public class ClanInviteStructure
   {
      
      public var clanId:int;
      
      public var clanName:String;
      
      public var clanEmblem:Vector.<uint> = new Vector.<uint>();
      
      public var profileId:uint;
      
      public var stringProfileId:String;
      
      public var rank:int;
      
      public var name:String;
      
      public var inviteDate:int;
      
      public function ClanInviteStructure()
      {
         super();
      }
   }
}

