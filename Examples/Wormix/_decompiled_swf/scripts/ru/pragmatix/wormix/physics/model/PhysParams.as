package ru.pragmatix.wormix.physics.model
{
   import §_-A1K§.§_-7u§;
   import ru.pragmatix.wormix.physics.*;
   import ru.pragmatix.wormix.utils.MathUtil;
   
   public class PhysParams
   {
      
      public static const COLLIDES_WITH_NOTHING:int = 16;
      
      public static const COLLIDES_WITH_GROUND:int = 65552;
      
      public static const COLLIDES_WITH_UNIT:int = 1048592;
      
      public static const COLLIDES_WITH_AMMO:int = 16777232;
      
      public static const COLLIDES_WITH_ALL:int = 17891344;
      
      protected var _forces:Object = {};
      
      protected var _collidesWith:int = 16;
      
      protected var _sinkable:Boolean = false;
      
      protected var _collideType:int = 0;
      
      internal var _stable:§_-7u§ = new §_-7u§(false);
      
      internal var _disposed:Boolean = false;
      
      internal var stopped:Boolean = false;
      
      internal var _x:int;
      
      internal var _y:int;
      
      internal var _groundShiftX:int = 0;
      
      internal var _groundShiftY:int = 0;
      
      private var _px:Number;
      
      private var _py:Number;
      
      private var _dx:Number = 0;
      
      private var _dy:Number = 0;
      
      protected var _ignoresTerrain:Boolean = false;
      
      public var optimizedMovement:Boolean = true;
      
      public function PhysParams(param1:int)
      {
         super();
         this.collideType = param1;
      }
      
      public function addForce(param1:Force) : void
      {
         this._forces[param1.name] = param1;
      }
      
      public function hasForce(param1:String) : Boolean
      {
         return this._forces[param1] != null;
      }
      
      public function removeForce(param1:String) : void
      {
         delete this._forces[param1];
      }
      
      public function removeAllForces() : void
      {
         this._forces = {};
      }
      
      public function get forces() : Object
      {
         return this._forces;
      }
      
      public function epsilonSpeedCheck(param1:Number = NaN) : void
      {
         if(isNaN(param1))
         {
            param1 = Physics.EPSILON;
         }
         if(Math.abs(this._dx) < param1)
         {
            this._dx = 0;
         }
         if(Math.abs(this._dy) < param1)
         {
            this._dy = 0;
         }
      }
      
      public function get collidesWith() : int
      {
         return this._collidesWith;
      }
      
      public function set collidesWith(param1:int) : void
      {
         this._collidesWith = param1;
      }
      
      public function get collideType() : int
      {
         return this._collideType;
      }
      
      public function set collideType(param1:int) : void
      {
         this._collideType = param1;
      }
      
      public function get ignoresTerrain() : Boolean
      {
         return this._ignoresTerrain;
      }
      
      public function set ignoresTerrain(param1:Boolean) : void
      {
         this._ignoresTerrain = param1;
      }
      
      public function get collidesWithGround() : Boolean
      {
         return (this._collidesWith & COLLIDES_WITH_GROUND) > COLLIDES_WITH_NOTHING;
      }
      
      public function get collidesWithUnit() : Boolean
      {
         return (this._collidesWith & COLLIDES_WITH_UNIT) > COLLIDES_WITH_NOTHING;
      }
      
      public function get collidesWithAmmo() : Boolean
      {
         return (this._collidesWith & COLLIDES_WITH_AMMO) > COLLIDES_WITH_NOTHING;
      }
      
      public function get sinkable() : Boolean
      {
         return this._sinkable;
      }
      
      public function set sinkable(param1:Boolean) : void
      {
         this._sinkable = param1;
      }
      
      public function get groundShiftX() : int
      {
         return this._groundShiftX;
      }
      
      public function get groundShiftY() : int
      {
         return this._groundShiftY;
      }
      
      public function setGroundShift(param1:int, param2:int) : void
      {
         this._x -= param1 - this._groundShiftX;
         this._y -= param2 - this._groundShiftY;
         this._px = this._x;
         this._py = this._y;
         this._groundShiftX = param1;
         this._groundShiftY = param2;
      }
      
      public function get disposed() : Boolean
      {
         return this._disposed;
      }
      
      public function set disposed(param1:Boolean) : void
      {
         this._disposed = param1;
      }
      
      public function toString() : String
      {
         return "PhysParams{_stable=" + String(this._stable.value) + ",stopped=" + String(this.stopped) + ",_x=" + String(this._x) + ",_y=" + String(this._y) + ",px=" + String(this._px) + ",py=" + String(this._py) + ",dx=" + String(this._dx) + ",dy=" + String(this._dy) + "}";
      }
      
      public function get dx() : Number
      {
         return this._dx;
      }
      
      public function set dx(param1:Number) : void
      {
         this._dx = MathUtil.§_-F1V§(param1);
      }
      
      public function get dy() : Number
      {
         return this._dy;
      }
      
      public function set dy(param1:Number) : void
      {
         this._dy = MathUtil.§_-F1V§(param1);
      }
      
      public function get px() : Number
      {
         return this._px;
      }
      
      public function set px(param1:Number) : void
      {
         this._px = MathUtil.§_-F1V§(param1);
      }
      
      public function get py() : Number
      {
         return this._py;
      }
      
      public function set py(param1:Number) : void
      {
         this._py = MathUtil.§_-F1V§(param1);
      }
   }
}

