package ru.pragmatix.wormix.controls.system
{
   import §_-F2Q§.GestureEvent;
   import §_-hO§.§_-h2f§;
   import flash.events.Event;
   import org.gestouch.core.GestureState;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.CommandState;
   import ru.pragmatix.wormix.controls.GesturesHandler;
   import ru.pragmatix.wormix.controls.ToggleButtonsHandler;
   import ru.pragmatix.wormix.gui.§_-L1X§;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   
   public class TouchControlSystem extends AbstractControlSystem
   {
      
      public static const NAME:String = "Touch";
      
      private var longPressGesturesConfigs:Array;
      
      private var tapGesturesConfigs:Array = [];
      
      private var toggleButtonsConfig:Array;
      
      private var activeCommands:Array;
      
      private var activeFrames:Object;
      
      private var controlsLayout:§_-L1X§;
      
      private var gesturesHandler:GesturesHandler;
      
      private var toggleButtonsHandler:ToggleButtonsHandler;
      
      private var buttonMoveMode:Boolean;
      
      private var nextFrameCommandEnd:Array = [];
      
      private var listened:Boolean;
      
      public function TouchControlSystem(param1:§_-L1X§)
      {
         this.longPressGesturesConfigs = [{
            "button":"jumpButton",
            "callback":this.onJumpButtonTap
         },{
            "button":"backJumpButton",
            "callback":this.onBackJumpButtonTap
         },{
            "button":"shootButton",
            "callback":this.onShootButtonTap
         },{
            "button":"leftButton",
            "callback":this.onLeftButtonTap
         },{
            "button":"rightButton",
            "callback":this.onRightButtonTap
         },{
            "button":"upButton",
            "callback":this.onUpButtonTap
         },{
            "button":"downButton",
            "callback":this.onDownButtonTap
         },{
            "button":"bagButton",
            "callback":this.onBagButtonTap
         },{
            "button":"shiftButton",
            "callback":this.onShiftButtonTap
         }];
         this.toggleButtonsConfig = [{
            "button":"altButton",
            "callback":this.onAltButtonToggle
         }];
         super(NAME);
         this.controlsLayout = param1;
         this.gesturesHandler = new GesturesHandler(param1,this.longPressGesturesConfigs,this.tapGesturesConfigs,null);
         this.toggleButtonsHandler = new ToggleButtonsHandler(param1,this.toggleButtonsConfig);
         this.activeCommands = [];
         this.activeFrames = {};
      }
      
      override protected function activate() : void
      {
         §_-J2h§.§_-n1V§(this + ".activate...");
         this.gesturesHandler.initHandlers();
         this.toggleButtonsHandler.initHandlers();
      }
      
      override protected function deactivate() : void
      {
         var _loc1_:Command = null;
         §_-J2h§.§_-n1V§(this + ".deactivate...");
         this.gesturesHandler.removeHandlers();
         this.toggleButtonsHandler.removeHandlers();
         active = true;
         for each(_loc1_ in this.activeCommands)
         {
            dispatchCommand(_loc1_,CommandState.END);
         }
         this.activeCommands.length = 0;
         this.nextFrameCommandEnd.length = 0;
         active = false;
      }
      
      public function setButtonMoveMode(param1:Boolean) : void
      {
         if(this.buttonMoveMode == param1)
         {
            return;
         }
         this.buttonMoveMode = param1;
         if(this.buttonMoveMode)
         {
            this.gesturesHandler.setNumTapsRequired(100500);
            this.gesturesHandler.setMinLongPressDuration(10000);
            this.toggleButtonsHandler.setEnabled(false);
         }
         else
         {
            this.gesturesHandler.setNumTapsRequired(1);
            this.gesturesHandler.setMinLongPressDuration(1);
            this.toggleButtonsHandler.setEnabled(true);
         }
      }
      
      private function onJumpButtonTap(param1:GestureEvent) : void
      {
         this.dispatchCommandByGesture(Command.Jump,param1.newState,param1.oldState);
      }
      
      private function onBackJumpButtonTap(param1:GestureEvent) : void
      {
         this.dispatchCommandByGesture(Command.BackJump,param1.newState,param1.oldState);
      }
      
      private function onShootButtonTap(param1:GestureEvent) : void
      {
         §_-J2h§.§_-n1V§(this + ".onShootButtonTap...");
         this.dispatchCommandByGesture(Command.Shoot,param1.newState,param1.oldState);
      }
      
      private function onLeftButtonTap(param1:GestureEvent) : void
      {
         this.dispatchCommandByGesture(Command.Left,param1.newState,param1.oldState);
      }
      
      private function onRightButtonTap(param1:GestureEvent) : void
      {
         this.dispatchCommandByGesture(Command.Right,param1.newState,param1.oldState);
      }
      
      private function onUpButtonTap(param1:GestureEvent) : void
      {
         this.dispatchCommandByGesture(Command.Up,param1.newState,param1.oldState);
      }
      
      private function onDownButtonTap(param1:GestureEvent) : void
      {
         this.dispatchCommandByGesture(Command.Down,param1.newState,param1.oldState);
      }
      
      private function onBagButtonTap(param1:GestureEvent) : void
      {
         this.dispatchCommandByGesture(Command.Bag,param1.newState,param1.oldState);
      }
      
      private function onShiftButtonTap(param1:GestureEvent) : void
      {
         this.dispatchCommandByGesture(Command.Shift,param1.newState,param1.oldState);
      }
      
      private function onAltButtonToggle(param1:Boolean) : void
      {
         this.dispatchCommandBySelection(Command.Alt,param1);
      }
      
      private function dispatchCommandByGesture(param1:Command, param2:GestureState, param3:GestureState) : void
      {
         var _loc5_:int = 0;
         if(this.activeFrames[param1] == undefined)
         {
            this.activeFrames[param1] = 0;
         }
         var _loc4_:int = Boolean(§_-X1j§.§_-12n§.scene) && Boolean(§_-X1j§.§_-12n§.scene.gameObjectProcessor) ? §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-O1C§() : 0;
         §_-J2h§.§_-n1V§(this + ".dispatchCommandByGesture: << " + param1.name + " to " + param2 + " from " + param3 + " frame=" + §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-O1C§());
         if(param2 == GestureState.BEGAN || param2 == GestureState.FAILED && param3 == GestureState.POSSIBLE)
         {
            if(this.activeCommands.indexOf(param1) == -1)
            {
               this.activeCommands.push(param1);
            }
            this.activeFrames[param1] = _loc4_;
            dispatchCommand(param1,CommandState.BEGIN);
         }
         else if(param2 == GestureState.ENDED || param2 == GestureState.POSSIBLE)
         {
            if(this.activeFrames[param1] == _loc4_)
            {
               if(!this.listened)
               {
                  this.listened = true;
                  Application.§_-Y1V§.addEventListener(§_-h2f§.§_-a1F§,this.onNextFrame);
               }
               if(this.nextFrameCommandEnd.indexOf(param1) == -1)
               {
                  this.nextFrameCommandEnd.push(_loc4_ + 1,param1);
               }
            }
            else
            {
               _loc5_ = this.activeCommands.indexOf(param1);
               if(_loc5_ != -1)
               {
                  this.activeCommands.removeAt(_loc5_);
                  if(this.activeFrames[param1] < _loc4_)
                  {
                     this.activeFrames[param1] = 0;
                  }
               }
               dispatchCommand(param1,CommandState.END);
            }
         }
      }
      
      private function dispatchCommandBySelection(param1:Command, param2:Boolean) : void
      {
         var _loc3_:int = 0;
         if(param2)
         {
            if(this.activeCommands.indexOf(param1) == -1)
            {
               this.activeCommands.push(param1);
            }
            dispatchCommand(param1,CommandState.BEGIN);
         }
         else
         {
            _loc3_ = this.activeCommands.indexOf(param1);
            if(_loc3_ != -1)
            {
               this.activeCommands.removeAt(_loc3_);
            }
            dispatchCommand(param1,CommandState.END);
         }
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this.toggleButtonsHandler = null;
         this.gesturesHandler.dispose();
         this.gesturesHandler = null;
         this.controlsLayout = null;
         this.activeCommands = null;
         this.activeFrames = null;
         this.nextFrameCommandEnd = null;
      }
      
      private function onNextFrame(param1:Event) : void
      {
         var _loc3_:int = 0;
         var _loc4_:Command = null;
         var _loc5_:int = 0;
         Application.§_-Y1V§.removeEventListener(§_-h2f§.CLEAR,this.onNextFrame);
         this.listened = false;
         var _loc2_:int = Boolean(§_-X1j§.§_-12n§.scene) && Boolean(§_-X1j§.§_-12n§.scene.gameObjectProcessor) ? §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-O1C§() : 0;
         if(Boolean(this.activeCommands) && Boolean(this.nextFrameCommandEnd))
         {
            while(this.nextFrameCommandEnd.length > 1)
            {
               _loc3_ = this.nextFrameCommandEnd.shift();
               _loc4_ = this.nextFrameCommandEnd.shift();
               if(_loc2_ == _loc3_ && this.activeFrames[_loc4_] < _loc2_)
               {
                  _loc5_ = this.activeCommands.indexOf(_loc4_);
                  if(_loc5_ != -1)
                  {
                     this.activeCommands.removeAt(_loc5_);
                     this.activeFrames[_loc4_] = 0;
                  }
                  dispatchCommand(_loc4_,CommandState.END);
               }
            }
         }
      }
   }
}

