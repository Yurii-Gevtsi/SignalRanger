package ru.pragmatix.wormix.controller
{
   import §_-D3A§.§_-DI§;
   import §_-J3L§.§_-h1D§;
   import §_-Ly§.§_-81L§;
   import §_-Yk§.§_-m2w§;
   import §_-Z15§.*;
   import §_-hO§.§_-h2f§;
   import §_-j2R§.§_-73G§;
   import §_-l1K§.§_-cZ§;
   import §_-u13§.§_-F3§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.util.der.*;
   import dragonBones.animation.WorldClock;
   import feathers.data.ListCollection;
   import flash.display.*;
   import flash.display3D.*;
   import flash.events.*;
   import flash.utils.*;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class §_-32w§ extends flash.events.EventDispatcher
   {
      
      private var _timer:Timer;
      
      public function §_-32w§()
      {
         super();
         this._timer = new Timer(30);
         this._timer.addEventListener(TimerEvent.TIMER,this.§_-g3§,false,int.MAX_VALUE);
         this._timer.start();
      }
      
      private function §_-g3§(param1:TimerEvent) : void
      {
         dispatchEvent(new flash.events.Event(§_-h2f§.§_-a1F§));
         dispatchEvent(new flash.events.Event(§_-h2f§.CLEAR));
      }
      
      public function dispose() : void
      {
         this._timer.removeEventListener(TimerEvent.TIMER,this.§_-g3§);
         this._timer.stop();
         this._timer = null;
      }
   }
}

