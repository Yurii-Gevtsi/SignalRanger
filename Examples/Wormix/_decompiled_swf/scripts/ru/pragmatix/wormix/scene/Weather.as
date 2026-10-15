package ru.pragmatix.wormix.scene
{
   import §_-k13§.§_-j23§;
   import flash.system.Capabilities;
   
   public class Weather
   {
      
      public static const CLEAR:uint = 0;
      
      public static const SNOW:uint = 1;
      
      public static const RAIN:uint = 2;
      
      private var curWeather:int = 0;
      
      private var isEnabled:Boolean;
      
      private var particleObjectsPool:§_-j23§;
      
      public function Weather()
      {
         super();
         var _loc1_:String = Capabilities.os;
         this.isEnabled = _loc1_.search("iPhone3,1") == -1;
      }
      
      public function update() : void
      {
         var _loc1_:int = 0;
         var _loc2_:uint = 0;
         if(this.isEnabled && this.curWeather == SNOW)
         {
            _loc1_ = 2;
            _loc2_ = §_-j23§.§_-31j§;
            while(--_loc1_ > -1)
            {
               this.particleObjectsPool.createParticle(_loc2_).init();
            }
         }
      }
      
      public function change(param1:int = 0) : void
      {
         if(this.curWeather != param1)
         {
            this.curWeather = param1;
            if(this.isEnabled)
            {
               this.particleObjectsPool = §_-j23§.getInstance();
               this.particleObjectsPool.clearAll();
               if(this.curWeather == SNOW)
               {
                  this.particleObjectsPool.§_-I3B§(§_-j23§.§_-31j§);
               }
            }
         }
      }
      
      public function dispose() : void
      {
      }
   }
}

