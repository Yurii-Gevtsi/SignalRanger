package ru.pragmatix.flox.logger
{
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-zI§.*;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   
   public class §_-V2x§
   {
      
      public static const §_-02t§:String = "DEFAULT";
      
      public var time:String;
      
      public var loggerId:String;
      
      public var level:§_-93v§;
      
      public var group:String;
      
      public var argument:Object;
      
      public function §_-V2x§(param1:String, param2:Object, param3:§_-93v§, param4:String)
      {
         super();
         this.loggerId = param1;
         this.argument = param2;
         this.level = param3;
         this.group = param4 ? param4 : §_-02t§;
         this.time = this.§_-m2l§();
      }
      
      public static function serialize(param1:§_-V2x§) : Object
      {
         var _loc2_:Object = new Object();
         _loc2_.loggerId = param1.loggerId;
         _loc2_.time = param1.time;
         _loc2_.levelName = param1.level.getName();
         _loc2_.levelValue = param1.level.getValue();
         _loc2_.levelColor = param1.level.getColor();
         _loc2_.groupName = param1.group;
         _loc2_.argument = param1.argument;
         return _loc2_;
      }
      
      public static function deserialize(param1:Object) : §_-V2x§
      {
         var _loc2_:§_-93v§ = §_-93v§["" + param1.levelName];
         var _loc3_:String = String["" + param1.groupName];
         var _loc4_:§_-V2x§ = new §_-V2x§(param1.loggerId,param1.argument,_loc2_,_loc3_);
         _loc4_.time = param1.time;
         return _loc4_;
      }
      
      private function §_-m2l§() : String
      {
         var _loc1_:Date = new Date();
         var _loc2_:String = String(_loc1_.hours);
         if(_loc2_.length < 2)
         {
            _loc2_ = "0" + _loc2_;
         }
         var _loc3_:String = String(_loc1_.minutes);
         if(_loc3_.length < 2)
         {
            _loc3_ = "0" + _loc3_;
         }
         var _loc4_:String = String(_loc1_.seconds);
         if(_loc4_.length < 2)
         {
            _loc4_ = "0" + _loc4_;
         }
         return _loc2_ + ":" + _loc3_ + ":" + _loc4_;
      }
   }
}

