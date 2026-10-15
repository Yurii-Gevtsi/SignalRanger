package ru.pragmatix.wormix.weapons.ammo
{
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-w2i§.§_-Zd§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class HideableGuardingBot extends InvisibleGuardingBot
   {
      
      private static const REVEAL_FRAMES_ON_SOT:Number = 30;
      
      private var hidden:Boolean = false;
      
      public function HideableGuardingBot(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Boolean = true)
      {
         super(param1,param2,param3,param4,param5,false);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.onSOT,§_-4V§.§_-fV§);
         §_-23f§ = false;
      }
      
      private function onSOT(param1:Event) : void
      {
         if(§_-X1j§.§_-12n§.gameMaster.activeSquad.getTeamId() == owner.squad.getTeamId() && !this.hidden)
         {
            timer.start();
            this.hidden = true;
         }
         else if(this.hidden)
         {
            clip.alpha = 1;
            §_-Zd§.§_-33o§(REVEAL_FRAMES_ON_SOT,timer.start);
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.onSOT);
         super.defaultOnDisposed(param1);
      }
   }
}

