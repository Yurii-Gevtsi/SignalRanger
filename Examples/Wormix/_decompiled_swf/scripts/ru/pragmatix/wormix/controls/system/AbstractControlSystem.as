package ru.pragmatix.wormix.controls.system
{
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.CommandState;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.events.EventDispatcher;
   
   public class AbstractControlSystem extends EventDispatcher
   {
      
      private var name:String;
      
      protected var active:Boolean = false;
      
      public function AbstractControlSystem(param1:String)
      {
         super();
         this.name = param1;
      }
      
      public function setActive(param1:Boolean) : void
      {
         if(this.active == param1)
         {
            return;
         }
         §_-J2h§.§_-n1V§(this + ".setActive: " + this.active + " >> " + param1);
         this.active = param1;
         if(this.active)
         {
            this.activate();
         }
         else
         {
            this.deactivate();
         }
      }
      
      public function isActive() : Boolean
      {
         return this.active;
      }
      
      protected function activate() : void
      {
      }
      
      protected function deactivate() : void
      {
      }
      
      public function pause(param1:Boolean) : void
      {
      }
      
      protected function dispatchCommand(param1:Command, param2:CommandState = null) : void
      {
         dispatchEventWith(§_-L3Q§.§_-l5§,false,param1.withState(param2 ? param2 : CommandState.BEGIN));
      }
      
      public function getName() : String
      {
         return this.name;
      }
      
      public function dispose() : void
      {
         this.deactivate();
      }
   }
}

