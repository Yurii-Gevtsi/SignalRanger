package ru.pragmatix.wormix.game
{
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-A18§;
   import §_-Y1O§.§_-71D§;
   import §_-Y1O§.§_-V23§;
   import §_-b1F§.§_-v2Q§;
   import §_-hO§.§_-02h§;
   import §_-w2i§.§_-33j§;
   import ru.pragmatix.wormix.game.ai.§_-S1i§;
   import ru.pragmatix.wormix.game.rules.§_-P2S§;
   import ru.pragmatix.wormix.messages.structures.ReconnectToSimpleBattleResultStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.utils.§_-z1x§;
   import starling.events.Event;
   
   public interface §_-eX§
   {
      
      function §_-vs§() : §_-E3j§;
      
      function dispose() : void;
      
      function §_-ma§(param1:§_-02h§) : void;
      
      function nextTurn() : void;
      
      function §_-A3P§() : Boolean;
      
      function §_-63§(param1:ReconnectToSimpleBattleResultStructure) : void;
      
      function deactivate() : void;
      
      function §_-n25§() : void;
      
      function §_-i1I§() : void;
      
      function §_-z2C§(param1:Event) : void;
      
      function §_-AK§(param1:Boolean = false) : void;
      
      function §_-W1o§() : void;
      
      function §_-il§() : void;
      
      function §_-n1Z§() : Boolean;
      
      function §_-h1Y§() : void;
      
      function activateUnit() : void;
      
      function deactivateUnit() : void;
      
      function §_-n14§(param1:§_-01J§) : void;
      
      function §_-VR§() : void;
      
      function §_-12G§() : Array;
      
      function §_-83I§(param1:§_-01J§ = null, param2:Boolean = false) : Vector.<§_-01J§>;
      
      function §_-D2A§(param1:§_-01J§ = null) : Vector.<§_-01J§>;
      
      function §_-b2W§(param1:§_-01J§ = null, param2:Boolean = false, param3:Boolean = false) : Vector.<§_-01J§>;
      
      function §_-939§(param1:§_-01J§ = null) : Vector.<§_-01J§>;
      
      function §_-D3n§(param1:Boolean = false) : Vector.<§_-01J§>;
      
      function §_-13w§(param1:§_-z1x§, param2:Boolean = false) : void;
      
      function §_-62V§() : Vector.<§_-01J§>;
      
      function §_-d1R§() : uint;
      
      function §_-B3T§() : uint;
      
      function dispatchNext(param1:Event) : void;
      
      function onFrameTimerComplete(param1:Event) : void;
      
      function get needToWaitActiveWeaponActionEnd() : Boolean;
      
      function §_-f13§() : void;
      
      function §_-I2u§() : void;
      
      function §_-K3D§() : void;
      
      function §_-338§() : void;
      
      function increaseWaterLevel() : void;
      
      function §_-D38§(param1:Function, ... rest) : Boolean;
      
      function §_-239§() : void;
      
      function §_-E3b§() : Boolean;
      
      function §_-y2v§() : Boolean;
      
      function §_-e2H§() : Boolean;
      
      function §_-J33§() : Boolean;
      
      function §_-A1C§() : void;
      
      function §_-T1E§(param1:int) : void;
      
      function §_-21q§(param1:int, param2:Array) : void;
      
      function §_-4Y§() : Array;
      
      function §_-72U§() : UserProfile;
      
      function §_-vJ§(param1:UserProfile) : §_-V23§;
      
      function nextTurnOnStable() : void;
      
      function §_-VI§() : void;
      
      function §_-i2L§() : void;
      
      function §_-O1x§() : void;
      
      function §_-x1O§(param1:Event, param2:§_-v2Q§) : void;
      
      function §_-11§(param1:§_-01J§, param2:uint) : void;
      
      function §_-A1N§() : Vector.<int>;
      
      function §_-q1K§(param1:int) : void;
      
      function §_-s13§() : uint;
      
      function §_-82g§() : uint;
      
      function §_-h2Z§(param1:Boolean) : void;
      
      function §_-p17§() : Boolean;
      
      function §_-v1V§() : Boolean;
      
      function §_-J3U§() : void;
      
      function §_-hx§(param1:uint) : void;
      
      function §_-c2w§(param1:Boolean = true) : void;
      
      function §_-kB§() : Boolean;
      
      function §_-36§(param1:Boolean = true) : void;
      
      function §_-41q§() : void;
      
      function §_-Rk§() : Boolean;
      
      function §_-U2O§() : Boolean;
      
      function §_-42C§() : Boolean;
      
      function §_-T1r§() : Boolean;
      
      function §_-W2j§() : §_-73e§;
      
      function §_-G2t§(param1:§_-A18§) : void;
      
      function §_-A1L§() : int;
      
      function §_-4n§() : uint;
      
      function §_-L2x§(param1:uint) : void;
      
      function §_-HR§() : §_-71D§;
      
      function §_-72B§() : uint;
      
      function §_-m2C§() : §_-S1i§;
      
      function §_-91D§(param1:uint) : Boolean;
      
      function get activeSquad() : §_-V23§;
      
      function set activeSquad(param1:§_-V23§) : void;
      
      function get activeUnit() : §_-01J§;
      
      function set activeUnit(param1:§_-01J§) : void;
      
      function §_-YK§() : int;
      
      function get nextTurnEvents() : §_-33j§;
      
      function set nextTurnEvents(param1:§_-33j§) : void;
      
      function get nextTurnTimer() : FrameTimer;
      
      function set nextTurnTimer(param1:FrameTimer) : void;
      
      function get damageAggregator() : §_-P2S§;
      
      function get charging() : Boolean;
      
      function set charging(param1:Boolean) : void;
      
      function §_-QS§() : uint;
      
      function §_-p1a§() : Boolean;
   }
}

