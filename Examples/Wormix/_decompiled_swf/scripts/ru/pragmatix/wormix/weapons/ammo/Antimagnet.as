package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31Y§.§_-D3B§;
   import §_-91A§.§_-sH§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-hO§.§_-y2Z§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class Antimagnet extends Mine implements §_-sH§
   {
      
      private static const DURATION:§_-TA§ = new §_-TA§(10);
      
      private var powerDivisor:int;
      
      private var squadNumber:uint;
      
      protected var turnsLeft:§_-TA§ = new §_-TA§(DURATION.value);
      
      private var loopSound:§_-V1R§;
      
      private var pulse:§_-di§;
      
      public function Antimagnet(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§)
      {
         this.squadNumber = param1.squad.getNumber();
         super(param1,param2,param3,param4);
         mineDistance.value = 400;
         explosionDelay.value = 0;
         this.powerDivisor = 25;
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.checkDuration);
         §_-h2B§.play(§_-d27§.§_-51E§);
      }
      
      override protected function initialize(param1:Point, param2:String = null, param3:Boolean = false, param4:String = "resurces/_ammo/") : void
      {
         super.initialize(param1,param2,param3,param4);
         var _loc5_:DisplayObject = §_-z1H§;
         this.pulse = Effects.createAnimationEffect("PulsatingWave");
         WorldClock.clock.add(this.pulse);
         this.pulse.animation.play();
         §_-z1H§ = new Sprite();
         (§_-z1H§ as Sprite).addChild(_loc5_);
         (§_-z1H§ as Sprite).addChild(this.pulse.display);
      }
      
      override protected function findTarget() : §_-01J§
      {
         var _loc2_:Vector.<§_-F3o§> = null;
         var _loc4_:§_-F3o§ = null;
         var _loc5_:Number = NaN;
         var _loc1_:uint = 0;
         _loc2_ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-434§(CollideType.AMMO);
         var _loc3_:Vector.<§_-F3o§> = Physics.getGameObjectsInCircle(x,y,mineDistance.value,_loc2_);
         for each(_loc4_ in _loc3_)
         {
            if(!_loc4_.disposed && !_loc4_.stable && _loc4_ != this)
            {
               if(!_loc4_.§_-l28§(§_-D3B§.§_-f1U§))
               {
                  _loc5_ = -Math.sqrt((mineDistance.value - §_-H3D§.distance(x - _loc4_.x,y - _loc4_.y)) / this.powerDivisor);
                  _loc4_.addSpeedVector(Math.atan2(y - _loc4_.y,x - _loc4_.x),_loc5_);
                  _loc1_++;
               }
            }
         }
         if(this.loopSound == null && _loc1_ > 0)
         {
            this.loopSound = §_-h2B§.play(§_-d27§.§_-510§);
         }
         if(this.loopSound != null && _loc1_ == 0)
         {
            this.loopSound.stop();
            this.loopSound = null;
         }
         return null;
      }
      
      override protected function explode() : void
      {
      }
      
      private function checkDuration(param1:Event) : void
      {
         if(--this.turnsLeft.value <= 0)
         {
            disposed = true;
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,10,§_-d27§.§_-J1x§);
         }
         else
         {
            dispatchEvent(new §_-y2Z§(this,this.turnsLeft.value));
         }
      }
      
      public function getNameHintInfoPanel() : String
      {
         return "";
      }
      
      public function getSquadNumber() : uint
      {
         return this.squadNumber;
      }
      
      public function get hp() : int
      {
         return this.turnsLeft.value;
      }
      
      public function get doesntCountTurns() : Boolean
      {
         return false;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.pulse)
         {
            if(this.pulse.display.parent)
            {
               (this.pulse.display.parent as DisplayObjectContainer).removeChild(this.pulse.display);
            }
            WorldClock.clock.remove(this.pulse);
            this.pulse.dispose();
            this.pulse = null;
         }
         if(this.loopSound != null)
         {
            this.loopSound.stop();
            this.loopSound = null;
         }
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.checkDuration);
         super.defaultOnDisposed(param1);
      }
   }
}

