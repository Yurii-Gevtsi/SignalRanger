package ru.pragmatix.wormix.messages.client
{
   import §_-RE§.§_-52p§;
   import §_-Vg§.*;
   import com.hurlant.util.der.*;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.structures.PositionStructure;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   
   public class SearchTheHouse
   {
      
      public var sessionKey:String;
      
      public var friendId:uint;
      
      public var keyNum:int;
      
      public function SearchTheHouse(param1:String = "", param2:uint = 0, param3:int = 0)
      {
         super();
         this.sessionKey = param1;
         this.friendId = param2;
         this.keyNum = param3;
      }
   }
}

