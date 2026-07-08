package ru.pragmatix.wormix.controls
{
   import flash.ui.GameInputControl;
   import flash.ui.GameInputDevice;
   
   public class GamepadControlsMap
   {
      
      private static const defaultData:Object = getDefaultData();
      
      public var deviceName:String;
      
      public var isDefault:Boolean;
      
      private var map:Object = {};
      
      public function GamepadControlsMap(param1:String = null, param2:Boolean = false)
      {
         super();
         this.deviceName = param1;
         this.isDefault = param2;
      }
      
      private static function getDefaultData() : Object
      {
         var _loc1_:Object = {};
         _loc1_[§_-vm§.§_-M2X§] = Command.Left;
         _loc1_[§_-vm§.§_-g25§] = Command.Right;
         _loc1_[§_-vm§.§_-I10§] = Command.Up;
         _loc1_[§_-vm§.§_-G2j§] = Command.Down;
         _loc1_[§_-vm§.§_-r1S§] = Command.Jump;
         _loc1_[§_-vm§.§_-G3B§] = Command.BackJump;
         _loc1_[§_-vm§.§_-K2a§] = Command.Shoot;
         _loc1_[§_-vm§.§_-F3F§] = Command.Alt_Shift;
         _loc1_[§_-vm§.§_-N1e§] = Command.Bag;
         _loc1_[§_-vm§.§_-m2L§] = Command.Escape;
         return _loc1_;
      }
      
      public static function getDefaultMapForDevice(param1:GameInputDevice) : GamepadControlsMap
      {
         var _loc5_:GameInputControl = null;
         var _loc6_:Command = null;
         var _loc2_:GamepadControlsMap = new GamepadControlsMap(param1.name,true);
         var _loc3_:int = 0;
         var _loc4_:int = param1.numControls;
         while(_loc3_ < _loc4_)
         {
            _loc5_ = param1.getControlAt(_loc3_);
            _loc6_ = defaultData[_loc5_.name] as Command;
            if(_loc6_)
            {
               _loc2_.setControlByParams(_loc5_.id,1,_loc6_);
            }
            _loc3_++;
         }
         return _loc2_;
      }
      
      public static function deserialize(param1:Object) : GamepadControlsMap
      {
         var data:Object = param1;
         var map:GamepadControlsMap = new GamepadControlsMap();
         try
         {
            map.deserialize(data);
         }
         catch(e:Error)
         {
            map = null;
         }
         return map;
      }
      
      public function setControl(param1:GameInputControl, param2:Command) : void
      {
         this.setControlByParams(param1.id,param1.value,param2);
      }
      
      public function setControlByParams(param1:String, param2:Number, param3:Command) : void
      {
         var _loc4_:GamepadControlsMapData = new GamepadControlsMapData(param1,param2,param3);
         this.map[_loc4_.getKey()] = _loc4_;
      }
      
      public function getKeysByControlId(param1:String) : Vector.<String>
      {
         var _loc3_:GamepadControlsMapData = null;
         var _loc2_:Vector.<String> = new Vector.<String>(0);
         for each(_loc3_ in this.map)
         {
            if(_loc3_.id == param1)
            {
               _loc2_.push(_loc3_.getKey());
            }
         }
         return _loc2_;
      }
      
      public function match(param1:GameInputControl) : Command
      {
         var _loc2_:GamepadControlsMapData = this.map[GamepadControlsMapData.getKey(param1.id,param1.value)] as GamepadControlsMapData;
         return _loc2_ ? _loc2_.command : null;
      }
      
      public function matchByKey(param1:String) : Command
      {
         var _loc2_:GamepadControlsMapData = this.map[param1] as GamepadControlsMapData;
         return _loc2_ ? _loc2_.command : null;
      }
      
      public function matchByParams(param1:String, param2:int) : Command
      {
         var _loc3_:GamepadControlsMapData = this.map[GamepadControlsMapData.getKey(param1,param2)] as GamepadControlsMapData;
         return _loc3_ ? _loc3_.command : null;
      }
      
      public function serialize() : Object
      {
         var _loc2_:String = null;
         var _loc1_:Object = {};
         for(_loc2_ in this.map)
         {
            _loc1_[_loc2_] = GamepadControlsMapData(this.map[_loc2_]).serialize();
         }
         return {
            "deviceName":this.deviceName,
            "map":_loc1_
         };
      }
      
      public function deserialize(param1:Object) : GamepadControlsMap
      {
         var _loc3_:Object = null;
         var _loc4_:GamepadControlsMapData = null;
         this.deviceName = param1.deviceName;
         this.map = {};
         var _loc2_:Object = param1.map;
         for each(_loc3_ in _loc2_)
         {
            _loc4_ = GamepadControlsMapData.deserialize(_loc3_);
            if(_loc4_)
            {
               this.map[_loc4_.getKey()] = _loc4_;
            }
         }
         return this;
      }
   }
}

