package ru.pragmatix.wormix.controls.sets
{
   import §_-A1K§.§_-TA§;
   import §_-Tm§.§_-RF§;
   import §_-hO§.§_-h2f§;
   import §_-l1g§.§_-L3Q§;
   import flash.events.Event;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class AbstractControlSet
   {
      
      public const ACTIVATED:§_-TA§ = new §_-TA§(-242);
      
      public const DEACTIVATED:§_-TA§ = new §_-TA§(-687);
      
      protected var state:§_-TA§ = this.DEACTIVATED;
      
      public function AbstractControlSet()
      {
         super();
      }
      
      public function activate() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(this.state == this.DEACTIVATED && this.canActivate(_loc1_))
         {
            this.state = this.ACTIVATED;
            this.activateInternal();
         }
      }
      
      public function deactivate() : void
      {
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(this.state == this.ACTIVATED && this.canDeactivate(_loc1_))
         {
            this.state = this.DEACTIVATED;
            this.deactivateInternal();
         }
      }
      
      protected function canActivate(param1:§_-eX§) : Boolean
      {
         return Boolean(param1) && param1.§_-E3b§();
      }
      
      protected function canDeactivate(param1:§_-eX§) : Boolean
      {
         return true;
      }
      
      protected function activateInternal() : void
      {
         Application.§_-Y1V§.addEventListener(§_-h2f§.§_-a1F§,this.onFrame,false,int.MAX_VALUE);
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc1_) && Boolean(_loc1_.activeUnit))
         {
            _loc1_.activeUnit.addEventListener(§_-L3Q§.DISPOSED,this.onActiveUnitDisposed);
         }
      }
      
      protected function deactivateInternal() : void
      {
         Application.§_-Y1V§.removeEventListener(§_-h2f§.§_-a1F§,this.onFrame);
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc1_) && Boolean(_loc1_.activeUnit))
         {
            _loc1_.activeUnit.removeEventListener(§_-L3Q§.DISPOSED,this.onActiveUnitDisposed);
         }
      }
      
      private function onActiveUnitDisposed(param1:starling.events.Event) : void
      {
         this.deactivate();
      }
      
      protected function onFrame(param1:flash.events.Event) : void
      {
         var _loc2_:§_-01J§ = this.getActiveUnit();
         if(!this.isPrevPressed(Command.Bag) && this.isPressed(Command.Bag))
         {
            if(_loc2_.§_-vp§)
            {
               §_-RF§.fire(§_-RF§.§_-WT§);
            }
         }
      }
      
      protected function isPressed(param1:Command) : Boolean
      {
         return ControlSystems.instance.isCommandActive(param1);
      }
      
      protected function isPrevPressed(param1:Command) : Boolean
      {
         return ControlSystems.instance.wasCommandActive(param1);
      }
      
      protected function canProcess(param1:Command) : Boolean
      {
         return ControlSystems.instance.canCommandProcess(param1);
      }
      
      final protected function getActiveUnit() : §_-01J§
      {
         return §_-X1j§.§_-12n§.gameMaster.activeUnit;
      }
   }
}

