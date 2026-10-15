package ru.pragmatix.wormix.controls.sets
{
   import §_-51M§.Dragon;
   import §_-Tm§.§_-RF§;
   import flash.events.Event;
   import ru.pragmatix.wormix.controls.Command;
   
   public class FlyingDragonControlSet extends AbstractControlSet
   {
      
      private var leftPressed:Boolean;
      
      private var rightPressed:Boolean;
      
      private var downPressed:Boolean;
      
      private var stopFlightPressed:Boolean = false;
      
      public function FlyingDragonControlSet()
      {
         super();
      }
      
      override protected function activateInternal() : void
      {
         super.activateInternal();
         this.leftPressed = false;
         this.rightPressed = false;
         this.stopFlightPressed = isPressed(Command.Jump);
      }
      
      override protected function onFrame(param1:Event) : void
      {
         if(state.value == DEACTIVATED.value)
         {
            return;
         }
         var _loc2_:Dragon = getActiveUnit() as Dragon;
         if(!_loc2_)
         {
            return;
         }
         if(!isPressed(Command.Jump) && this.stopFlightPressed)
         {
            this.stopFlightPressed = false;
         }
         if(isPressed(Command.Jump))
         {
            if(!this.stopFlightPressed)
            {
               this.stopFlightPressed = true;
               §_-RF§.fire(§_-RF§.§_-Y2b§);
            }
         }
         if(!isPressed(Command.Left) && this.leftPressed)
         {
            this.leftPressed = false;
            if(!this.rightPressed)
            {
               §_-RF§.fire(§_-RF§.§_-P20§);
            }
         }
         if(isPressed(Command.Left) && !this.leftPressed)
         {
            this.leftPressed = true;
            §_-RF§.fire(§_-RF§.§_-f12§);
         }
         if(!isPressed(Command.Right) && this.rightPressed)
         {
            this.rightPressed = false;
            if(!this.leftPressed)
            {
               §_-RF§.fire(§_-RF§.§_-lG§);
            }
         }
         if(isPressed(Command.Right) && !this.rightPressed)
         {
            this.rightPressed = true;
            §_-RF§.fire(§_-RF§.§_-p1Q§);
         }
         if(!isPressed(Command.Down) && this.downPressed)
         {
            this.downPressed = false;
            if(!this.downPressed)
            {
               §_-RF§.fire(§_-RF§.§_-L1Q§);
            }
         }
         if(isPressed(Command.Down) && !this.downPressed)
         {
            this.downPressed = true;
            §_-RF§.fire(§_-RF§.§_-p2B§);
         }
         if(isPressed(Command.Up) && _loc2_.§_-OF§())
         {
            §_-RF§.fire(§_-RF§.§_-U23§);
         }
         super.onFrame(param1);
      }
   }
}

