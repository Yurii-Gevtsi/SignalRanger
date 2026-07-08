package ru.pragmatix.wormix.controls.system
{
   import §_-22z§.§_-f1b§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.CommandState;
   import ru.pragmatix.wormix.gui.§_-gT§;
   import starling.events.KeyboardEvent;
   
   public class KeyboardControlSystem extends AbstractControlSystem
   {
      
      public static const NAME:String = "Keyboard";
      
      private var activeCommands:Array;
      
      public function KeyboardControlSystem()
      {
         super(NAME);
         §_-gT§.init();
         this.activeCommands = [];
      }
      
      override protected function activate() : void
      {
         Application.§_-i1N§.stage.addEventListener(KeyboardEvent.KEY_DOWN,this.keyDownHandler);
         Application.§_-i1N§.stage.addEventListener(KeyboardEvent.KEY_UP,this.keyUpHandler);
      }
      
      override protected function deactivate() : void
      {
         var _loc1_:Command = null;
         Application.§_-i1N§.stage.removeEventListener(KeyboardEvent.KEY_DOWN,this.keyDownHandler);
         Application.§_-i1N§.stage.removeEventListener(KeyboardEvent.KEY_UP,this.keyUpHandler);
         active = true;
         for each(_loc1_ in this.activeCommands)
         {
            dispatchCommand(_loc1_,CommandState.END);
         }
         this.activeCommands.splice(0);
         active = false;
      }
      
      private function keyDownHandler(param1:KeyboardEvent) : void
      {
         var _loc3_:Command = null;
         var _loc2_:uint = param1.keyCode;
         if(§_-f1b§.§_-42c§(_loc2_))
         {
            §_-f1b§.§_-id§(param1.keyCode);
         }
         else
         {
            _loc3_ = §_-gT§.§_-12B§(param1.keyCode);
            if(_loc3_)
            {
               if(this.activeCommands.indexOf(_loc3_) == -1)
               {
                  this.activeCommands.push(_loc3_);
               }
               dispatchCommand(_loc3_,CommandState.BEGIN);
            }
         }
      }
      
      private function keyUpHandler(param1:KeyboardEvent) : void
      {
         var _loc3_:int = 0;
         var _loc2_:Command = §_-gT§.§_-12B§(param1.keyCode);
         if(_loc2_)
         {
            _loc3_ = this.activeCommands.indexOf(_loc2_);
            if(_loc3_ != -1)
            {
               this.activeCommands.removeAt(_loc3_);
            }
            dispatchCommand(_loc2_,CommandState.END);
         }
      }
   }
}

