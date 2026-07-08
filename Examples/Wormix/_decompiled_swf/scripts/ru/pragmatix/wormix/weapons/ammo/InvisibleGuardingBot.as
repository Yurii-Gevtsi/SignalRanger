package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-hO§.§_-03k§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import starling.events.Event;
   
   public class InvisibleGuardingBot extends GuardingBot
   {
      
      private static const ALPHA_REDUCE_RATE:Number = 0.03;
      
      private static const OWNERS_ALPHA:Number = 0.5;
      
      private var invisibleAlpha:Number;
      
      protected var timer:FrameTimer;
      
      protected var isAllyGuard:Boolean;
      
      public function InvisibleGuardingBot(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Boolean = true, param6:Boolean = true)
      {
         super(param1,param2,param3,param4,param5);
         this.isAllyGuard = §_-X1j§.§_-12n§.gameMaster.§_-YK§() == param1.squad.getTeamId();
         this.invisibleAlpha = this.isAllyGuard ? OWNERS_ALPHA : 0;
         this.timer = new FrameTimer(1);
         this.timer.addEventListener(§_-03k§.§_-836§,this.timerTick);
         if(param6)
         {
            this.timer.start();
         }
      }
      
      private function timerTick(param1:§_-03k§) : void
      {
         this.clip.alpha -= ALPHA_REDUCE_RATE;
         if(this.clip.alpha <= this.invisibleAlpha)
         {
            this.timer.stop();
         }
      }
      
      override protected function beforeExplosion() : void
      {
         if(this.timer)
         {
            this.timer.stop();
         }
         this.clip.alpha = 1;
         super.beforeExplosion();
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.timer != null)
         {
            this.timer.stop();
            this.timer.removeEventListener(§_-03k§.§_-836§,this.timerTick);
            this.timer = null;
         }
         super.defaultOnDisposed(param1);
      }
   }
}

