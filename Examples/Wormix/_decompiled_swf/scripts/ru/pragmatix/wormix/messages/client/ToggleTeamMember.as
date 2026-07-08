package ru.pragmatix.wormix.messages.client
{
   import §_-CF§.§_-p2U§;
   import §_-DJ§.§_-J2l§;
   import §_-P1B§.§_-b21§;
   import §_-P1B§.§_-bb§;
   import §_-P1z§.*;
   import §_-YF§.§_-Zi§;
   import §_-j1w§.§_-L1x§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import com.hurlant.math.*;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.weapons.ammo.feats.IFrameFeat;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class ToggleTeamMember
   {
      
      public var teamMemberId:int;
      
      public var active:Boolean;
      
      public function ToggleTeamMember(param1:int, param2:Boolean)
      {
         super();
         this.teamMemberId = param1;
         this.active = param2;
      }
   }
}

