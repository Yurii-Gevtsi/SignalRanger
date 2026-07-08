package ru.pragmatix.wormix.messages.structures
{
   import §_-73d§.§_-V19§;
   import §_-bI§.*;
   import §_-q26§.*;
   import flash.events.Event;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   
   public class UnitStructure
   {
      
      public var ownerId:int;
      
      public var armor:int;
      
      public var attack:int;
      
      public var level:int;
      
      public var experience:int;
      
      public var hatId:int;
      
      public var raceId:int;
      
      public var skin:int;
      
      public var artifactId:int;
      
      public var socialOwnerIds:Dictionary;
      
      public var name:String;
      
      public var teamMemberType:int;
      
      public var isActive:Boolean = true;
      
      public function UnitStructure()
      {
         super();
      }
      
      public function initFromProfile(param1:UserProfile) : void
      {
         this.ownerId = param1.getId();
         this.socialOwnerIds = param1.getSocialIds();
         this.armor = param1.§_-h2m§();
         this.attack = param1.§_-h2m§();
         this.level = param1.getLevel();
         this.experience = param1.§_-p25§();
         this.teamMemberType = param1.§_-t2I§();
         this.isActive = param1.§_-H38§();
         this.hatId = param1.getHat().getId();
         this.raceId = param1.§_-P2e§().getId();
         this.artifactId = param1.§_-32q§().getId();
         this.name = param1.getCharName();
      }
      
      public function toString() : String
      {
         return "UnitStructure{ownerId=" + String(this.ownerId) + ",socialOwnerIds=" + String(this.socialOwnerIds) + ",armor=" + String(this.armor) + ",attack=" + String(this.attack) + ",level=" + String(this.level) + ",experience=" + String(this.experience) + ",hat=" + String(this.hatId) + "}";
      }
   }
}

