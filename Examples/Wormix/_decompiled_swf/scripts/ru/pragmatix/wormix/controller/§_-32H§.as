package ru.pragmatix.wormix.controller
{
   import §_-H3§.§_-E3m§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-XA§;
   import §_-U1i§.§_-b1k§;
   import §_-U1i§.§_-hK§;
   import §_-s20§.§_-6a§;
   import §_-z1g§.§_-82X§;
   import §_-z1g§.§_-y1S§;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.game.rules.§_-l1m§;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   
   public interface §_-32H§
   {
      
      function dispose() : void;
      
      function isReady() : Boolean;
      
      function §_-9l§() : Boolean;
      
      function §_-gj§() : §_-l1m§;
      
      function §_-nd§() : §_-E3m§;
      
      function §_-E3v§() : void;
      
      function §_-2n§(param1:UserProfile, param2:§_-b1k§) : void;
      
      function §_-y1D§(param1:UserProfile) : void;
      
      function §_-A1Y§(param1:UserProfile, param2:Boolean = true) : void;
      
      function §_-IT§(param1:§_-y1S§) : void;
      
      function §_-D1n§(param1:§_-82X§) : Boolean;
      
      function §_-N2s§(param1:UserProfile, param2:Boolean = false) : Boolean;
      
      function §_-N1O§(param1:UserProfile) : Boolean;
      
      function §_-22K§(param1:UserProfile, param2:uint) : Boolean;
      
      function §_-Q1g§(param1:UserProfile, param2:HeroicMissionStructure) : Boolean;
      
      function §_-j1F§(param1:HeroicMissionStructure) : Boolean;
      
      function get friendInPair() : UserProfile;
      
      function set friendInPair(param1:UserProfile) : void;
      
      function get scene() : Scene;
      
      function §_-a1X§(param1:§_-O2i§) : void;
      
      function §_-H1V§() : void;
      
      function §_-Sd§() : void;
      
      function §_-j4§(param1:§_-O2i§, param2:§_-6a§) : void;
      
      function §_-Q2l§(param1:§_-J1§, param2:§_-XA§, param3:Array = null) : void;
      
      function §_-O11§(param1:Function) : void;
      
      function §_-8z§() : void;
      
      function §_-41l§() : void;
      
      function §_-rJ§() : Boolean;
      
      function §_-H2r§() : void;
      
      function §_-q2B§(param1:String, param2:int, param3:UserProfile = null, param4:Boolean = false, param5:String = "") : void;
      
      function §_-c13§() : UserProfile;
      
      function §_-WN§() : §_-O2i§;
      
      function get isLearning() : Boolean;
      
      function get isBoss() : Boolean;
      
      function §_-V2F§(param1:uint) : UserProfile;
      
      function §_-D3x§(param1:UserProfile) : int;
      
      function get inBattle() : Boolean;
      
      function get isCreatingBattle() : Boolean;
      
      function §_-vP§() : §_-J1§;
      
      function §_-p1t§() : §_-hK§;
      
      function get gameMaster() : §_-eX§;
      
      function set gameMaster(param1:§_-eX§) : void;
      
      function get battleStats() : §_-U19§;
      
      function set battleStats(param1:§_-U19§) : void;
   }
}

