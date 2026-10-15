package ru.pragmatix.wormix.messages.clans.structures
{
   import feathers.controls.List;
   import feathers.data.ListCollection;
   import feathers.events.CollectionEventType;
   import flash.events.TimerEvent;
   import flash.utils.Dictionary;
   import flash.utils.Timer;
   import starling.core.Starling;
   import starling.events.KeyboardEvent;
   
   public class ClanInviteTO
   {
      
      public var clanId:int;
      
      public var profileId:int;
      
      public var inviteDate:int;
      
      public function ClanInviteTO()
      {
         super();
      }
   }
}

