package ru.pragmatix.wormix.controls.system
{
   import §_-hO§.§_-h2f§;
   import §_-l1g§.§_-L3Q§;
   import flash.events.Event;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.CommandState;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class ControlSystems extends EventDispatcher
   {
      
      public static const instance:ControlSystems = new ControlSystems();
      
      private var systems:Object = {};
      
      private var disableCommands:Object;
      
      private var activeCommands:Object;
      
      private var prevCommands:Object;
      
      public function ControlSystems()
      {
         super();
         this.initCommandStates();
         Application.§_-Y1V§.addEventListener(§_-h2f§.CLEAR,this.clear);
      }
      
      private function initCommandStates() : void
      {
         var _loc1_:Command = null;
         this.clearCommandStates();
         for each(_loc1_ in Command.all)
         {
            this.activeCommands[_loc1_.name] = false;
            this.prevCommands[_loc1_.name] = false;
         }
      }
      
      public function clearCommandStates() : void
      {
         this.disableCommands = {};
         this.activeCommands = {};
         this.prevCommands = {};
      }
      
      public function registerSystem(param1:AbstractControlSystem) : void
      {
         this.unregisterSystem(param1.getName());
         this.systems[param1.getName()] = param1;
         param1.addEventListener(§_-L3Q§.§_-l5§,this.onCommandEvent);
         param1.setActive(true);
      }
      
      public function unregisterSystem(param1:String) : void
      {
         var _loc2_:AbstractControlSystem = this.systems[param1] as AbstractControlSystem;
         if(_loc2_)
         {
            _loc2_.removeEventListener(§_-L3Q§.§_-l5§,this.onCommandEvent);
            _loc2_.dispose();
            delete this.systems[param1];
         }
      }
      
      private function onCommandEvent(param1:starling.events.Event) : void
      {
         var _loc3_:Command = null;
         var _loc2_:AbstractControlSystem = AbstractControlSystem(param1.target);
         if(_loc2_.isActive())
         {
            _loc3_ = Command(param1.data);
            this.processCommand(_loc3_);
         }
      }
      
      private function processCommand(param1:Command) : void
      {
         var _loc2_:Boolean = false;
         if(!this.disableCommands[param1.name])
         {
            _loc2_ = param1.state == CommandState.BEGIN;
            this.activeCommands[param1.name] = _loc2_;
         }
         dispatchEventWith(§_-L3Q§.COMMAND_CONTROL_CHANGED,false,param1);
      }
      
      public function getSystem(param1:String) : AbstractControlSystem
      {
         return this.systems[param1] as AbstractControlSystem;
      }
      
      public function setCommandDisabled(param1:Command, param2:Boolean) : void
      {
         if(param2)
         {
            this.disableCommands[param1.name] = 1;
         }
         else if(this.disableCommands[param1.name])
         {
            delete this.disableCommands[param1.name];
         }
      }
      
      public function isCommandActive(param1:Command) : Boolean
      {
         return this.activeCommands[param1.name];
      }
      
      public function wasCommandActive(param1:Command) : Boolean
      {
         return this.prevCommands[param1.name];
      }
      
      public function canCommandProcess(param1:Command) : Boolean
      {
         return !this.disableCommands[param1.name];
      }
      
      public function dispose() : void
      {
         var _loc1_:AbstractControlSystem = null;
         for each(_loc1_ in this.systems)
         {
            _loc1_.removeEventListener(§_-L3Q§.§_-l5§,this.onCommandEvent);
            _loc1_.dispose();
         }
         this.systems = null;
      }
      
      private function clear(param1:flash.events.Event) : void
      {
         var _loc3_:String = null;
         var _loc2_:Array = Command.all;
         var _loc4_:int = 0;
         while(_loc4_ < _loc2_.length)
         {
            _loc3_ = Command(_loc2_[_loc4_]).name;
            this.prevCommands[_loc3_] = this.activeCommands[_loc3_];
            _loc4_++;
         }
      }
   }
}

