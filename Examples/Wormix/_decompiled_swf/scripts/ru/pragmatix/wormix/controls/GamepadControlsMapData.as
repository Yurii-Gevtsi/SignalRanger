package ru.pragmatix.wormix.controls
{
   public class GamepadControlsMapData
   {
      
      private static const DELIM:String = ":";
      
      public var sign:int;
      
      public var id:String;
      
      public var command:Command;
      
      public function GamepadControlsMapData(param1:String, param2:Number, param3:Command)
      {
         super();
         this.id = param1;
         this.sign = param2 >= 0 ? 1 : -1;
         this.command = param3;
      }
      
      public static function getKey(param1:String, param2:Number) : String
      {
         return param1 + DELIM + (param2 >= 0 ? "+" : "-");
      }
      
      public static function getKeysByControlId(param1:String) : Vector.<String>
      {
         var _loc2_:Vector.<String> = new Vector.<String>(0);
         _loc2_.push(getKey(param1,1));
         _loc2_.push(getKey(param1,-1));
         return _loc2_;
      }
      
      public static function parseKey(param1:String, param2:Object) : void
      {
         var _loc3_:int = param1.indexOf(DELIM);
         param2.id = param1.substr(0,_loc3_);
         param2.sign = param1.substr(_loc3_ + 1) == "+" ? 1 : -1;
      }
      
      public static function deserialize(param1:Object) : GamepadControlsMapData
      {
         var data:Object = param1;
         var controlsMapData:GamepadControlsMapData = new GamepadControlsMapData(null,0,null);
         try
         {
            controlsMapData.deserialize(data);
         }
         catch(e:Error)
         {
            controlsMapData = null;
         }
         return controlsMapData;
      }
      
      public function getKey() : String
      {
         return GamepadControlsMapData.getKey(this.id,this.sign);
      }
      
      public function serialize() : Object
      {
         return {
            "id":this.id,
            "sign":this.sign,
            "command":this.command.name
         };
      }
      
      public function deserialize(param1:Object) : void
      {
         this.id = param1.id;
         this.sign = param1.sign;
         this.command = Command.getByName(param1.command);
      }
   }
}

