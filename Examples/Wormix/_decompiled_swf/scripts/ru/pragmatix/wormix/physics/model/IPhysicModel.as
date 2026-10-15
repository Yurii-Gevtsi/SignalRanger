package ru.pragmatix.wormix.physics.model
{
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import starling.events.Event;
   
   public interface IPhysicModel
   {
      
      function checkBounds() : void;
      
      function nextState() : void;
      
      function hitTestGround(param1:int, param2:int) : Boolean;
      
      function hitTestObject(param1:§_-F3o§) : Boolean;
      
      function getHitObjects(param1:Array) : void;
      
      function setSpeedVector(param1:Number, param2:Number) : void;
      
      function addSpeedVector(param1:Number, param2:Number) : void;
      
      function setSpeedXY(param1:Number, param2:Number) : void;
      
      function addSpeedXY(param1:Number, param2:Number) : void;
      
      function clear() : void;
      
      function get isStoped() : Boolean;
      
      function get speedX() : Number;
      
      function get speedY() : Number;
      
      function get speed() : Number;
      
      function get angle() : Number;
      
      function get x() : int;
      
      function set x(param1:int) : void;
      
      function get y() : int;
      
      function set y(param1:int) : void;
      
      function stop() : void;
      
      function get stable() : Boolean;
      
      function set stable(param1:Boolean) : void;
      
      function get lastTravelledDistance() : uint;
      
      function get keepUnstable() : Boolean;
      
      function set keepUnstable(param1:Boolean) : void;
      
      function addEventListener(param1:String, param2:Function) : void;
      
      function removeEventListener(param1:String, param2:Function) : void;
      
      function dispatchEvent(param1:Event) : void;
      
      function hasEventListener(param1:String, param2:Function = null) : Boolean;
   }
}

