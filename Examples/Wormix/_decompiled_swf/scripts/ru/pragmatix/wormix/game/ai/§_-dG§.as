package ru.pragmatix.wormix.game.ai
{
   import §_-62§.*;
   import §_-F39§.*;
   import §_-Tm§.§_-RF§;
   import §_-Z1K§.§_-q24§;
   import §_-hO§.§_-03k§;
   import §_-hO§.§_-729§;
   import §_-j1w§.§_-lD§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import feathers.core.PopUpManager;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class §_-dG§
   {
      
      private static var MAX_ANGLE_MOVEMENT_COUNT:uint = 100;
      
      public function §_-dG§()
      {
         super();
      }
      
      public static function §_-s2s§(param1:Function, param2:§_-01J§, param3:Number) : void
      {
         var ch:§_-W2f§ = null;
         var epsilon:Number = NaN;
         var ft:FrameTimer = null;
         var failoverCounter:uint = 0;
         var cb:Function = param1;
         var unit:§_-01J§ = param2;
         var angle:Number = param3;
         if(!(unit.weapon.§_-G3H§() is §_-W2f§))
         {
            if(§_-X1j§.§_-12n§.scene != null)
            {
               §_-Ia§.§_-33o§(30,cb);
            }
            return;
         }
         if(Math.abs(angle) < MathUtil.PI_HALF)
         {
            §_-RF§.fire(§_-RF§.§_-m2J§);
         }
         else
         {
            §_-RF§.fire(§_-RF§.§_-h2w§);
         }
         §_-RF§.fire(§_-RF§.§_-Ja§);
         ch = §_-W2f§(unit.weapon.§_-G3H§());
         epsilon = ch.§_-K5§() / 2;
         ft = new FrameTimer(1);
         failoverCounter = 0;
         ft.addEventListener(§_-03k§.§_-836§,function(param1:§_-03k§):void
         {
            ++failoverCounter;
            var _loc2_:Number = ch.getAngle();
            var _loc3_:Number = Math.abs(_loc2_ - angle) % Math.PI;
            if(_loc3_ > MathUtil.PI_HALF)
            {
               _loc3_ -= Math.PI;
            }
            if(§_-X1j§.§_-12n§.scene != null && ch.§_-F2k§() && failoverCounter != MAX_ANGLE_MOVEMENT_COUNT && _loc3_ > epsilon)
            {
               if(Math.sin(angle) > Math.sin(_loc2_))
               {
                  §_-RF§.fire(§_-RF§.§_-537§);
               }
               else
               {
                  §_-RF§.fire(§_-RF§.§_-XX§);
               }
            }
            else
            {
               ft.stop();
               cb();
            }
         });
         ft.start();
      }
      
      public static function §_-Zr§(param1:Function) : void
      {
         var cb:Function = param1;
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.SCENE_STABLE,function(param1:Event):void
         {
            cb();
         });
      }
      
      public static function §_-p2L§(param1:Function, param2:§_-01J§, param3:int) : void
      {
         var counter:int;
         var lastX:int;
         var FRAME_LIMIT:int = 0;
         var MAX_ERROR:int = 0;
         var ft:FrameTimer = null;
         var cb:Function = param1;
         var unit:§_-01J§ = param2;
         var x:int = param3;
         FRAME_LIMIT = 150;
         MAX_ERROR = 5;
         if(unit.x < x)
         {
            §_-RF§.fire(§_-RF§.§_-m2J§);
         }
         else
         {
            §_-RF§.fire(§_-RF§.§_-h2w§);
         }
         ft = new FrameTimer(1,151);
         counter = 0;
         lastX = -1;
         ft.addEventListener(§_-03k§.§_-836§,function(param1:§_-03k§):void
         {
            var e:§_-03k§ = param1;
            if(!unit.disposed)
            {
               if(!unit.stable)
               {
                  §_-Zd§.once(unit as EventDispatcher,§_-L3Q§.§_-a1V§,function(param1:Event):void
                  {
                     cb();
                  });
                  ft.stop();
               }
               else
               {
                  if(Math.abs(unit.x - x) < MAX_ERROR || counter++ >= FRAME_LIMIT)
                  {
                     §_-RF§.fire(§_-RF§.§_-Ja§);
                     ft.stop();
                     cb();
                  }
                  lastX = unit.x;
               }
            }
         });
         ft.start();
      }
   }
}

