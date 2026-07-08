package ru.pragmatix.wormix.controls
{
   import feathers.controls.ToggleButton;
   import flash.utils.Dictionary;
   import starling.events.Event;
   
   public class ToggleButtonsHandler
   {
      
      private var buttonsHandler:Object;
      
      private var callbacks:Dictionary;
      
      private var configs:Array;
      
      private var enabled:Boolean;
      
      private var inited:Boolean = false;
      
      public function ToggleButtonsHandler(param1:Object, param2:Array)
      {
         super();
         this.buttonsHandler = param1;
         this.configs = param2;
      }
      
      public function initHandlers() : void
      {
         var _loc1_:Object = null;
         var _loc2_:ToggleButton = null;
         if(this.inited)
         {
            return;
         }
         this.callbacks = new Dictionary();
         this.inited = true;
         this.enabled = true;
         for each(_loc1_ in this.configs)
         {
            _loc2_ = this.buttonsHandler[_loc1_.button] as ToggleButton;
            _loc2_.addEventListener(Event.CHANGE,this.onButtonChanged);
            this.callbacks[_loc2_] = _loc1_.callback;
         }
      }
      
      public function removeHandlers() : void
      {
         var _loc1_:Object = null;
         var _loc2_:ToggleButton = null;
         if(this.inited)
         {
            return;
         }
         this.inited = false;
         this.enabled = false;
         for each(_loc1_ in this.configs)
         {
            _loc2_ = this.buttonsHandler[_loc1_.button] as ToggleButton;
            _loc2_.removeEventListener(Event.CHANGE,this.onButtonChanged);
         }
         this.callbacks = null;
      }
      
      private function onButtonChanged(param1:Event) : void
      {
         if(!this.enabled)
         {
            return;
         }
         var _loc2_:Function = this.callbacks[param1.currentTarget];
         if(_loc2_ != null)
         {
            _loc2_(ToggleButton(param1.currentTarget).isSelected);
         }
      }
      
      public function setEnabled(param1:Boolean) : void
      {
         var _loc2_:Object = null;
         var _loc3_:ToggleButton = null;
         this.enabled = param1;
         for each(_loc2_ in this.configs)
         {
            _loc3_ = this.buttonsHandler[_loc2_.button] as ToggleButton;
            _loc3_.isEnabled = param1;
         }
      }
   }
}

