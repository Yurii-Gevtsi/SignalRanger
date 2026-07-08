package ru.pragmatix.wormix.controls.sets
{
   import §_-Tm§.§_-RF§;
   import §_-l1K§.§_-m2§;
   import flash.events.Event;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class CrosshairControlSet extends AbstractControlSet
   {
      
      public function CrosshairControlSet()
      {
         super();
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var _loc3_:§_-m2§ = null;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         if(state.value == DEACTIVATED.value)
         {
            return;
         }
         var _loc2_:§_-01J§ = getActiveUnit();
         if(_loc2_.weapon)
         {
            _loc3_ = _loc2_.weapon.§_-G3H§();
            if(!isPressed(Command.Shoot) && _loc3_.§_-K1C§(§_-RF§.§_-G1H§) && _loc3_.§_-5p§())
            {
               if(_loc3_.isActive())
               {
                  §_-RF§.fire(§_-RF§.§_-G1H§);
               }
            }
            _loc4_ = isPressed(Command.Shift) || isPressed(Command.Alt_Shift) ? 1 : 0;
            if(isPressed(Command.Left) && _loc3_.§_-K1C§(§_-RF§.§_-hX§) && !isPrevPressed(Command.Left))
            {
               §_-RF§.fire(§_-RF§.§_-hX§);
            }
            if(isPressed(Command.Right) && _loc3_.§_-K1C§(§_-RF§.§_-W3§) && !isPrevPressed(Command.Right))
            {
               §_-RF§.fire(§_-RF§.§_-W3§);
            }
            if(isPressed(Command.Up) && _loc3_.§_-K1C§(§_-RF§.§_-XX§))
            {
               if(!_loc3_.§_-938§() || !isPrevPressed(Command.Up))
               {
                  §_-RF§.fire(§_-RF§.§_-XX§,_loc4_);
               }
            }
            if(isPressed(Command.Down) && _loc3_.§_-K1C§(§_-RF§.§_-537§))
            {
               if(!_loc3_.§_-938§() || !isPrevPressed(Command.Down))
               {
                  §_-RF§.fire(§_-RF§.§_-537§,_loc4_);
               }
            }
            if(isPressed(Command.Shoot) && _loc3_.§_-K1C§(§_-RF§.§_-St§) && !_loc3_.§_-5p§() && !isPrevPressed(Command.Shoot))
            {
               _loc5_ = isPressed(Command.Alt) || isPressed(Command.Alt_Shift) ? 1 : 0;
               §_-RF§.fire(§_-RF§.§_-St§,_loc5_);
            }
         }
         super.onFrame(param1);
      }
   }
}

