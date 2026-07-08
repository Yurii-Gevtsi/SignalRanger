package ru.pragmatix.flox.gui.controls
{
   import §_-OJ§.§_-L3B§;
   import §_-OJ§.§_-jt§;
   import §_-OJ§.§_-sY§;
   import §_-r1C§.§_-t1R§;
   import flash.display.DisplayObject;
   import flash.events.IEventDispatcher;
   import flash.text.TextFormat;
   
   public interface §_-yf§ extends IEventDispatcher
   {
      
      function destroy() : void;
      
      function §_-c1v§(param1:Boolean) : void;
      
      function isVisible() : Boolean;
      
      function §_-h1C§() : Boolean;
      
      function §_-tx§(param1:Boolean) : void;
      
      function §_-i2B§() : Boolean;
      
      function §_-11P§(param1:String, param2:§_-sY§ = null, param3:String = null, param4:TextFormat = null) : void;
      
      function §_-gJ§(param1:String, param2:§_-sY§ = null, param3:String = null, param4:TextFormat = null) : void;
      
      function §_-RL§() : §_-jt§;
      
      function §_-JM§(param1:String, param2:§_-L3B§ = null, param3:String = null, param4:TextFormat = null) : void;
      
      function §_-Y1L§(param1:§_-t1R§) : void;
      
      function §_-rA§() : §_-t1R§;
      
      function §_-p1V§(param1:§_-t1R§) : void;
      
      function get x() : Number;
      
      function set x(param1:Number) : void;
      
      function get y() : Number;
      
      function set y(param1:Number) : void;
      
      function get width() : Number;
      
      function set width(param1:Number) : void;
      
      function get height() : Number;
      
      function set height(param1:Number) : void;
      
      function getContainer() : DisplayObject;
      
      function get id() : String;
      
      function set id(param1:String) : void;
      
      function get removeAllWhenDisabled() : Boolean;
      
      function set removeAllWhenDisabled(param1:Boolean) : void;
   }
}

