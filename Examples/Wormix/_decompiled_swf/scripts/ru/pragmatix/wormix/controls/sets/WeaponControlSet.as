package ru.pragmatix.wormix.controls.sets
{
   import §_-Tm§.§_-RF§;
   import flash.events.Event;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.weapons.interfaces.§_-P2v§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-o2d§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   
   public class WeaponControlSet extends AbstractControlSet
   {
      
      private var upPressed:Boolean;
      
      private var downPressed:Boolean;
      
      private var leftPressed:Boolean;
      
      private var rightPressed:Boolean;
      
      public function WeaponControlSet()
      {
         super();
      }
      
      override protected function activateInternal() : void
      {
         super.activateInternal();
         this.upPressed = false;
         this.downPressed = false;
         this.leftPressed = false;
         this.rightPressed = false;
         this.rightPressed = false;
      }
      
      override protected function onFrame(param1:Event) : void
      {
         if(state.value == DEACTIVATED.value)
         {
            return;
         }
         if(!getActiveUnit())
         {
            deactivate();
            return;
         }
         var _loc2_:§_-P2v§ = getActiveUnit().weapon;
         if(!isPressed(Command.Up) && _loc2_ is §_-o2d§ && this.upPressed)
         {
            this.upPressed = false;
            §_-RF§.fire(§_-RF§.§_-gI§);
         }
         if(!isPressed(Command.Down) && _loc2_ is §_-o2d§ && this.downPressed)
         {
            this.downPressed = false;
            §_-RF§.fire(§_-RF§.§_-N2h§);
         }
         if(!isPressed(Command.Left) && _loc2_ is §_-jm§ && this.leftPressed)
         {
            this.leftPressed = false;
            §_-RF§.fire(§_-RF§.§_-839§);
         }
         if(!isPressed(Command.Right) && _loc2_ is §_-jm§ && this.rightPressed)
         {
            this.rightPressed = false;
            §_-RF§.fire(§_-RF§.§_-sw§);
         }
         if(isPressed(Command.Up) && _loc2_ is §_-o2d§ && !this.upPressed)
         {
            this.upPressed = true;
            §_-RF§.fire(§_-RF§.§_-o1G§);
         }
         if(isPressed(Command.Down) && _loc2_ is §_-o2d§ && !this.downPressed)
         {
            this.downPressed = true;
            §_-RF§.fire(§_-RF§.§_-E3N§);
         }
         if(isPressed(Command.Left) && _loc2_ is §_-jm§ && !this.leftPressed)
         {
            this.leftPressed = true;
            §_-RF§.fire(§_-RF§.§_-Ad§);
         }
         if(isPressed(Command.Right) && _loc2_ is §_-jm§ && !this.rightPressed)
         {
            this.rightPressed = true;
            §_-RF§.fire(§_-RF§.§_-xU§);
         }
         if(isPressed(Command.Shoot) && _loc2_ is §_-y2Q§ && !isPrevPressed(Command.Shoot))
         {
            §_-RF§.fire(§_-RF§.§_-h1F§);
         }
         super.onFrame(param1);
      }
   }
}

