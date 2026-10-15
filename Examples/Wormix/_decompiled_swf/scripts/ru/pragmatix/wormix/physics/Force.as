package ru.pragmatix.wormix.physics
{
   import §_-A1K§.§_-TA§;
   import §_-l1g§.§_-L3Q§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class Force extends EventDispatcher
   {
      
      public static const GRAVITY_NAME:String = "gravity";
      
      public static const GRAVITY:Force = new Force(GRAVITY_NAME,Math.PI / new §_-TA§(2).value,new §_-TA§(1).value);
      
      public static const WIND_NAME:String = "wind";
      
      public static const WIND:Force = new Force(WIND_NAME,0,0);
      
      private static const windPower:§_-TA§ = new §_-TA§(0);
      
      public var name:String;
      
      private var _ax:Number;
      
      private var _ay:Number;
      
      private var _angle:Number;
      
      private var _module:Number;
      
      public function Force(param1:String, param2:Number, param3:Number)
      {
         super();
         this.name = param1;
         this._angle = param2;
         this._module = param3;
         this._ax = Math.cos(param2) * param3;
         this._ay = Math.sin(param2) * param3;
      }
      
      public static function setWind(param1:int) : void
      {
         windPower.value = param1;
         WIND.ax = param1 / 1000;
      }
      
      public static function getWindPower() : int
      {
         return windPower.value;
      }
      
      public function get ax() : Number
      {
         return this._ax;
      }
      
      public function set ax(param1:Number) : void
      {
         this._ax = param1;
         dispatchEvent(new Event(§_-L3Q§.FORCE_CHANGED));
      }
      
      public function get ay() : Number
      {
         return this._ay;
      }
      
      public function set ay(param1:Number) : void
      {
         this._ay = param1;
         dispatchEvent(new Event(§_-L3Q§.FORCE_CHANGED));
      }
      
      public function get angle() : Number
      {
         return this._angle;
      }
      
      public function set angle(param1:Number) : void
      {
         this._angle = param1;
         dispatchEvent(new Event(§_-L3Q§.FORCE_CHANGED));
      }
      
      public function get module() : Number
      {
         return this._module;
      }
      
      public function set module(param1:Number) : void
      {
         this._module = param1;
         dispatchEvent(new Event(§_-L3Q§.FORCE_CHANGED));
      }
      
      public function toString() : String
      {
         return "Force{name=" + String(this.name) + ",_ax=" + String(this._ax) + ",_ay=" + String(this._ay) + ",_angle=" + String(this._angle) + ",_module=" + String(this._module) + "}";
      }
   }
}

