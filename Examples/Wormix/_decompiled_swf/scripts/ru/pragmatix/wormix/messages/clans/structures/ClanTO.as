package ru.pragmatix.wormix.messages.clans.structures
{
   import §_-73d§.§_-x2C§;
   import §_-D3A§.§_-TZ§;
   import §_-l1K§.§_-d2j§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-zI§.§_-92Q§;
   import §_-zI§.§_-X1q§;
   import flash.events.Event;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   
   public class ClanTO
   {
      
      public static const SCOPE_HEADER:int = 0;
      
      public static const SCOPE_MEMBER_HEADER:int = 1;
      
      public static const SCOPE_ALL:int = 65535;
      
      public var id:int;
      
      public var name:String;
      
      public var level:int;
      
      public var size:int;
      
      public var rating:int;
      
      public var seasonRating:int;
      
      public var joinRating:int;
      
      public var createDate:int;
      
      public var emblem:Vector.<uint> = new Vector.<uint>();
      
      public var clanDescription:String;
      
      public var reviewState:int;
      
      public var topPlace:int;
      
      public var prevSeasonTopPlace:int;
      
      public var members:Vector.<ClanMemberTO> = new Vector.<ClanMemberTO>(0);
      
      public var isExitClosed:Boolean;
      
      public var treasury:int;
      
      public var medalPrice:int;
      
      public var cashedMedals:int;
      
      public function ClanTO()
      {
         super();
      }
      
      public function isBlocked() : Boolean
      {
         return this.reviewState == -1;
      }
      
      public function getCapacityWithSize() : String
      {
         return this.size.toString() + "/" + §_-92Q§.getLevel(this.level).capacity.toString();
      }
      
      public function isLeader(param1:int) : Boolean
      {
         return this.getRank(param1) == §_-X1q§.§_-O2V§;
      }
      
      public function getRank(param1:int) : int
      {
         var _loc2_:ClanMemberTO = null;
         for each(_loc2_ in this.members)
         {
            if(_loc2_.profileId == param1)
            {
               return _loc2_.rank;
            }
         }
         return -1;
      }
      
      public function copyFrom(param1:ClanTO) : void
      {
         this.id = param1.id;
         this.name = param1.name;
         this.level = param1.level;
         this.size = param1.size;
         this.rating = param1.rating;
         this.seasonRating = param1.seasonRating;
         this.joinRating = param1.joinRating;
         this.createDate = param1.createDate;
         this.emblem = param1.emblem.concat();
         this.clanDescription = param1.clanDescription;
         this.reviewState = param1.reviewState;
         this.topPlace = param1.topPlace;
         this.prevSeasonTopPlace = param1.prevSeasonTopPlace;
         this.members = param1.members.concat();
         this.isExitClosed = param1.isExitClosed;
         this.treasury = param1.treasury;
         this.medalPrice = param1.medalPrice;
         this.cashedMedals = param1.cashedMedals;
      }
   }
}

