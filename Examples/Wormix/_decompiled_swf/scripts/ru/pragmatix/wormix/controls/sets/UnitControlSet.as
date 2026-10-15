package ru.pragmatix.wormix.controls.sets
{
   import §_-51M§.Rabbit;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-I3t§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import flash.events.Event;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.physics.model.CatPhysicModel;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.interfaces.§_-b29§;
   
   public class UnitControlSet extends AbstractControlSet
   {
      
      private static const SNEAK:uint = 1;
      
      private static const WALK:uint = 0;
      
      private static const POWER_JUMP:uint = 1;
      
      private static const NORMAL_JUMP:uint = 0;
      
      private static var jumpPressed:Boolean = false;
      
      private var shooting:Boolean = false;
      
      private var catMode:Boolean = false;
      
      private var shiftPressed:Boolean = false;
      
      private var step:int = 0;
      
      public function UnitControlSet()
      {
         super();
      }
      
      override protected function canActivate(param1:§_-eX§) : Boolean
      {
         return param1 && param1.§_-E3b§() && param1.activeUnit && !param1.activeUnit.disposed && !param1.§_-U2O§();
      }
      
      override protected function activateInternal() : void
      {
         super.activateInternal();
         this.shooting = isPressed(Command.Shoot);
         this.shiftPressed = isPressed(Command.Shift);
         this.step = 0;
      }
      
      override protected function deactivateInternal() : void
      {
         super.deactivateInternal();
         var _loc1_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(Boolean(_loc1_) && Boolean(_loc1_.§_-E3b§()) && !_loc1_.activeUnit.disposed)
         {
            §_-RF§.fire(§_-RF§.§_-Ja§);
         }
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var _loc3_:Boolean = false;
         var _loc4_:Boolean = false;
         var _loc5_:Boolean = false;
         var _loc6_:Boolean = false;
         var _loc7_:int = 0;
         var _loc8_:§_-I3t§ = null;
         var _loc9_:Weapon = null;
         var _loc10_:Boolean = false;
         var _loc11_:int = 0;
         if(state.value == DEACTIVATED.value)
         {
            return;
         }
         var _loc2_:§_-01J§ = getActiveUnit();
         if(_loc2_.aiArsenal != null)
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"unit actions on AI turn"));
         }
         if(!_loc2_.disposed)
         {
            if(!isPressed(Command.Left) && _loc2_.crawling < 0 || !isPressed(Command.Right) && _loc2_.crawling > 0)
            {
               §_-RF§.fire(§_-RF§.§_-Ja§);
            }
            if(!isPressed(Command.Shoot) && this.shooting)
            {
               this.shooting = false;
            }
            _loc3_ = isPressed(Command.Shift) || isPressed(Command.Alt_Shift);
            _loc4_ = this.shiftPressed != _loc3_;
            this.shiftPressed = _loc3_;
            if(_loc2_.stable || _loc2_.attributes.jump.§_-SX§())
            {
               _loc5_ = isPressed(Command.Up);
               _loc6_ = (_loc5_) || _loc3_;
               _loc7_ = _loc2_ is Rabbit && _loc6_ ? int(POWER_JUMP) : int(NORMAL_JUMP);
               if(isPressed(Command.Jump) && !this.catMode && !(jumpPressed && _loc2_.getPhysicModel() is CatPhysicModel))
               {
                  §_-RF§.fire(§_-RF§.§_-u21§,_loc7_);
               }
               if(isPressed(Command.BackJump))
               {
                  §_-RF§.fire(§_-RF§.§_-K1K§,_loc7_);
               }
            }
            jumpPressed = isPressed(Command.Jump);
            if(_loc2_.stable)
            {
               if(!_loc3_)
               {
                  this.step = 0;
               }
               if(!isPressed(Command.Left) && this.step == §_-8K§.LEFT)
               {
                  this.step = 0;
               }
               if(!isPressed(Command.Right) && this.step == §_-8K§.RIGHT)
               {
                  this.step = 0;
               }
               if(isPressed(Command.Left) && !isPressed(Command.Right) && (!_loc2_.isImmobile() || _loc2_.direction == §_-8K§.RIGHT))
               {
                  if(_loc2_.crawling >= 0)
                  {
                     if(!this.shiftPressed || this.step == 0)
                     {
                        if(this.shiftPressed && _loc2_.direction == §_-8K§.RIGHT)
                        {
                           this.step = §_-8K§.LEFT;
                        }
                        §_-RF§.fire(§_-RF§.§_-h2w§,this.shiftPressed ? SNEAK : WALK);
                     }
                  }
                  else if(_loc4_)
                  {
                     §_-RF§.fire(§_-RF§.§_-h2w§,this.shiftPressed ? SNEAK : WALK);
                  }
               }
               if(isPressed(Command.Right) && !isPressed(Command.Left) && (!_loc2_.isImmobile() || _loc2_.direction == §_-8K§.LEFT))
               {
                  if(_loc2_.crawling <= 0)
                  {
                     if(!this.shiftPressed || this.step == 0)
                     {
                        if(this.shiftPressed && _loc2_.direction == §_-8K§.LEFT)
                        {
                           this.step = §_-8K§.RIGHT;
                        }
                        §_-RF§.fire(§_-RF§.§_-m2J§,this.shiftPressed ? SNEAK : WALK);
                     }
                  }
                  else if(_loc4_)
                  {
                     §_-RF§.fire(§_-RF§.§_-m2J§,this.shiftPressed ? SNEAK : WALK);
                  }
               }
               if(isPressed(Command.Use) && !isPrevPressed(Command.Use))
               {
                  _loc8_ = _loc2_.§_-F2x§;
                  if(Boolean(_loc2_.§_-vt§ && _loc8_) && Boolean(_loc8_.hats.length > 0) && _loc8_.§_-mA§)
                  {
                     §_-RF§.fire(§_-RF§.§_-oT§,_loc8_.hats[0].getId());
                     _loc8_.§_-F3N§();
                  }
               }
            }
            else if(isPressed(Command.Shoot))
            {
               _loc9_ = _loc2_.weapon;
               if(Boolean((_loc9_) && !_loc9_.§_-82J§()) && Boolean(_loc9_ is §_-b29§) && !this.shooting)
               {
                  this.shooting = true;
                  _loc10_ = isPressed(Command.Alt) || isPressed(Command.Alt_Shift);
                  _loc11_ = _loc10_ ? 1 : 0;
                  §_-RF§.fire(§_-RF§.§_-Z24§,_loc11_);
               }
            }
            if(_loc2_.getPhysicModel() is CatPhysicModel)
            {
               this.catMode = isPressed(Command.Jump);
            }
            else
            {
               this.catMode = false;
            }
         }
         super.onFrame(param1);
      }
   }
}

