package ru.pragmatix.wormix.messages.structures
{
   import §_-zI§.*;
   import flash.events.EventPhase;
   import flash.events.TouchEvent;
   import org.gestouch.core.gestouch_internal;
   
   use namespace gestouch_internal;
   
   public class BossWormStructure
   {
      
      public var isPlayerTeam:Boolean;
      
      public var hp:int;
      
      public function BossWormStructure(param1:Boolean = true, param2:int = 0)
      {
         super();
         this.isPlayerTeam = param1;
         this.hp = param2;
      }
      
      public function toString() : String
      {
         return "BossWormStructure{" + " isPlayerTeam=" + this.isPlayerTeam + ", hp=" + this.hp + " }";
      }
   }
}

