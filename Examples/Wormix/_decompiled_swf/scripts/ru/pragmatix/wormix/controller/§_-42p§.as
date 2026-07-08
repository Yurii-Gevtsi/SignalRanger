package ru.pragmatix.wormix.controller
{
   import §_-9§.§_-52A§;
   import §_-91A§.*;
   import §_-931§.*;
   import §_-B1y§.§_-F1P§;
   import §_-C2t§.§_-92W§;
   import §_-CF§.§_-p2U§;
   import §_-F2M§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-f2L§;
   import §_-m0§.§_-a1O§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-z1g§.§_-lp§;
   import feathers.data.ArrayCollection;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.UfoTargetPointer;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class §_-42p§ extends EventDispatcher
   {
      
      public function §_-42p§()
      {
         super();
      }
      
      public function destroy() : void
      {
      }
      
      public function §_-r2I§() : void
      {
         §_-I3A§.§_-w1S§(0,§_-I3A§.§_-bz§(),this.§_-83a§);
      }
      
      private function §_-83a§(param1:Array) : void
      {
         dispatchEventWith(§_-f2L§.ALL_FRIENDS_LOADED,false,param1);
      }
   }
}

