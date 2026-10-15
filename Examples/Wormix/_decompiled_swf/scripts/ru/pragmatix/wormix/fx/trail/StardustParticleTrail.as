package ru.pragmatix.wormix.fx.trail
{
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import stardust.player.MovableSimPlayer;
   import starling.animation.IAnimatable;
   import starling.core.Starling;
   import starling.events.Event;
   
   public class StardustParticleTrail implements ITrail, IAnimatable
   {
      
      private var gobj:§_-F3o§;
      
      private var _player:MovableSimPlayer;
      
      public function StardustParticleTrail(param1:MovableSimPlayer)
      {
         super();
         this._player = param1;
      }
      
      private function onTargetDisposed(param1:Event) : void
      {
         this.detach();
      }
      
      public function attachTo(param1:§_-F3o§, ... rest) : void
      {
         this.gobj = param1;
         param1.addEventListener(§_-L3Q§.DISPOSED,this.onTargetDisposed);
         this._player.setPosition(param1.x,param1.y);
         Starling.juggler.add(this);
      }
      
      public function detach() : void
      {
         this._player.deactivate();
         this.gobj.removeEventListener(§_-L3Q§.DISPOSED,this.onTargetDisposed);
         Starling.juggler.remove(this);
      }
      
      public function get player() : MovableSimPlayer
      {
         return this._player;
      }
      
      public function advanceTime(param1:Number) : void
      {
         this._player.setPosition(this.gobj.x,this.gobj.y);
      }
   }
}

