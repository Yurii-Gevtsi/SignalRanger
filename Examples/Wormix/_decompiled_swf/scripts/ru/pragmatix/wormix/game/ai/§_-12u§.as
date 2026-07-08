package ru.pragmatix.wormix.game.ai
{
   import §_-x2Q§.§_-dU§;
   import flash.geom.Point;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   import starling.utils.Pool;
   import starlingbuilder.extensions.uicomponents.ImageButton;
   
   public class §_-12u§
   {
      
      private static const §_-K16§:uint = 200;
      
      private static const §_-o1o§:Number = 40;
      
      public function §_-12u§()
      {
         super();
      }
      
      public static function move(param1:§_-01J§, param2:int, param3:uint = 200) : Point
      {
         var _loc4_:uint = Math.abs(param1.record.getLevel() - §_-o1o§);
         return §_-pz§(param1.x,param1.y,param2,param3,_loc4_);
      }
      
      public static function §_-pz§(param1:int, param2:int, param3:int, param4:uint = 200, param5:uint = 100) : Point
      {
         var _loc7_:Point = null;
         var _loc6_:Point = Pool.getPoint(param1,param2);
         var _loc8_:* = 0;
         do
         {
            _loc7_ = §_-h1V§(_loc6_,param3,param5);
            if(_loc7_ != null)
            {
               Pool.putPoint(_loc6_);
               _loc6_ = _loc7_;
            }
         }
         while(_loc7_ != null && _loc8_++ < param4);
         if(Math.abs(_loc6_.x - param1) < 5)
         {
            _loc6_.x = param1;
            _loc6_.y = param2;
         }
         else
         {
            _loc6_.x -= param3 * 5;
         }
         return _loc6_;
      }
      
      public static function §_-h1V§(param1:Point, param2:int, param3:uint) : Point
      {
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:Vector.<§_-F3o§> = null;
         var _loc8_:§_-Q2U§ = null;
         var _loc4_:Point = Pool.getPoint(param1.x + param2,param1.y);
         if(param3 < 100)
         {
            _loc7_ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([§_-Q2U§]);
            for each(_loc8_ in _loc7_)
            {
               if(_loc8_.getDangerRadius() > 0 && MathUtil.distance(_loc8_.x,_loc8_.y,_loc4_.x,_loc4_.y) <= _loc8_.getDangerRadius() && (param3 > 0 ? Boolean(param3 < §_-X1j§.randomSeed.§_-Fh§(0,100)) : Boolean(true)))
               {
                  Pool.putPoint(_loc4_);
                  return null;
               }
            }
         }
         if(HitTest.unit(_loc4_.x,_loc4_.y))
         {
            _loc5_ = 1;
            while(_loc5_ < 10)
            {
               _loc6_ = _loc4_.y - _loc5_;
               if(!HitTest.unit(_loc4_.x,_loc6_))
               {
                  _loc4_.y = _loc6_;
                  return _loc4_;
               }
               _loc5_++;
            }
            Pool.putPoint(_loc4_);
            return null;
         }
         _loc5_ = 1;
         while(_loc5_ < 10)
         {
            _loc6_ = _loc4_.y + _loc5_;
            if(HitTest.unit(_loc4_.x,_loc6_))
            {
               _loc4_.y = _loc6_ - 1;
               return _loc4_;
            }
            _loc5_++;
         }
         Pool.putPoint(_loc4_);
         return null;
      }
   }
}

