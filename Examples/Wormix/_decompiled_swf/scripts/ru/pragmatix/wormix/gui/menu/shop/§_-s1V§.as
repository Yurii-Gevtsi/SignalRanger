package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-12W§.*;
   import §_-31W§.*;
   import §_-62§.§_-W1r§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-Vg§.§_-M11§;
   import §_-r1C§.§_-h2B§;
   import feathers.core.ITextRenderer;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-W2C§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class §_-s1V§ implements §_-03J§
   {
      
      public function §_-s1V§()
      {
         super();
      }
      
      public function §_-j1p§(param1:§_-E19§, param2:§_-W1r§, param3:uint = 1) : void
      {
         var _loc4_:§_-W2C§ = null;
         var _loc5_:§_-x2t§ = null;
         var _loc6_:§_-x2t§ = null;
         if(param2)
         {
            §_-h2B§.play(§_-d27§.§_-sg§);
            _loc4_ = §_-X1j§.§_-zU§;
            _loc5_ = param2.getPrice();
            _loc6_ = new §_-x2t§(_loc5_.§_-R1T§ * param3 + _loc4_.§_-F20§(),_loc5_.realPrice * param3 + §_-X1j§.§_-zU§.§_-t2L§());
            if(§_-33r§.§_-As§(param1,_loc6_))
            {
               §_-X1j§.§_-zU§.§_-dh§(param2,param3,param1);
            }
         }
      }
   }
}

