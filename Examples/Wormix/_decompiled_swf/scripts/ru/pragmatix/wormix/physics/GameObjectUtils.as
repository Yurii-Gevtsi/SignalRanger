package ru.pragmatix.wormix.physics
{
   import §_-31Y§.§_-D3B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-i14§;
   
   public class GameObjectUtils
   {
      
      public function GameObjectUtils()
      {
         super();
      }
      
      public static function updateTracePointsPosition(param1:Array, param2:int, param3:int, param4:Number, param5:Number, param6:Vector.<Force> = null, param7:uint = 10, param8:uint = 4) : void
      {
         var _loc11_:Number = NaN;
         var _loc12_:Number = NaN;
         var _loc13_:Object = null;
         var _loc14_:Force = null;
         var _loc16_:uint = 0;
         var _loc17_:int = 0;
         param6 ||= new Vector.<Force>();
         var _loc9_:Number = param5 * Math.cos(param4);
         var _loc10_:Number = param5 * Math.sin(param4);
         var _loc15_:int = 0;
         while(_loc15_ < param7)
         {
            _loc11_ = _loc9_ / param8;
            _loc12_ = _loc10_ / param8;
            _loc16_ = 0;
            while(_loc16_ < param8)
            {
               _loc13_ = param1[_loc16_ + _loc15_ * param8];
               if(_loc13_)
               {
                  _loc13_.x = param2 + _loc16_ * _loc11_;
                  _loc13_.y = param3 + _loc16_ * _loc12_;
               }
               _loc16_++;
            }
            param2 += _loc9_;
            param3 += _loc10_;
            _loc17_ = int(param6.length);
            while(--_loc17_ > -1)
            {
               _loc14_ = param6[_loc17_];
               _loc9_ += _loc14_.ax;
               _loc10_ += _loc14_.ay;
            }
            _loc15_++;
         }
      }
      
      public static function spliceDisposedObjects(param1:Vector.<§_-F3o§>) : void
      {
         var _loc2_:* = 0;
         if(Boolean(param1) && Boolean(param1.length))
         {
            _loc2_ = int(param1.length - 1);
            while(_loc2_ > -1)
            {
               if(param1[_loc2_] == null || param1[_loc2_].disposed)
               {
                  param1.splice(_loc2_,1);
               }
               _loc2_--;
            }
         }
      }
      
      public static function spliceDisposedUnits(param1:Vector.<§_-01J§>) : void
      {
         var _loc2_:* = 0;
         if(Boolean(param1) && Boolean(param1.length))
         {
            _loc2_ = int(param1.length - 1);
            while(_loc2_ > -1)
            {
               if(param1[_loc2_] == null || param1[_loc2_].disposed)
               {
                  param1.splice(_loc2_,1);
               }
               _loc2_--;
            }
         }
      }
      
      public static function getHatsUseReady(param1:§_-01J§) : Vector.<§_-F3o§>
      {
         var allHats:Vector.<§_-F3o§> = null;
         var hatsInRadius:Vector.<§_-F3o§> = null;
         var unit:§_-01J§ = param1;
         allHats = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-JB§(§_-D3B§.§_-l1Q§);
         hatsInRadius = Physics.getGameObjectsInCircle(unit.x,unit.y,§_-X1j§.§_-12n§.gameMaster.§_-W2j§().§_-z1z§,allHats);
         var hatsReady:Vector.<§_-F3o§> = hatsInRadius.filter(function(param1:§_-i14§, ... rest):Boolean
         {
            return param1.stable && !param1.disposed;
         });
         return hatsReady;
      }
   }
}

