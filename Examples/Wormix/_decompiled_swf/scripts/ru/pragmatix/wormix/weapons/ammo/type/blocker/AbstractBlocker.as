package ru.pragmatix.wormix.weapons.ammo.type.blocker
{
   import §_-91A§.§_-sH§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-S2O§;
   import §_-L1H§.§_-C3e§;
   import §_-hO§.§_-y2Z§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.abstracts.AbstractStickyAmmo;
   import starling.events.Event;
   
   public class AbstractBlocker extends AbstractStickyAmmo implements §_-sH§
   {
      
      private static const UNSTABLE_LIFETIME:uint = 180;
      
      protected var turnsLeft:§_-TA§;
      
      protected var paramSet:§_-C3e§;
      
      private var squadNumber:uint;
      
      public function AbstractBlocker(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:§_-TA§)
      {
         var hand:§_-S2O§;
         var owner:§_-01J§ = param1;
         var paramSet:§_-C3e§ = param2;
         var angle:Number = param3;
         var power:uint = param4;
         var duration:§_-TA§ = param5;
         this.turnsLeft = new §_-TA§(duration.value + 1);
         this.squadNumber = owner.squad.getNumber();
         super(owner,paramSet,angle,power);
         this.paramSet = paramSet;
         §_-Nt§ = true;
         §_-TZ§.addListener(this,§_-L3Q§.§_-Qw§,this.onDamaged);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.checkDuration);
         hand = armature.getBone("hand");
         if(hand)
         {
            hand.visible = false;
         }
         §_-Ia§.§_-33o§(UNSTABLE_LIFETIME,function():void
         {
            if(!disposed && !stable)
            {
               disposed = true;
            }
         });
      }
      
      override protected function initialize(param1:Point, param2:String = null, param3:Boolean = false, param4:String = "resurces/_ammo/") : void
      {
         super.initialize(param1,param2,param3,param4);
         this.changeClipToIdle();
      }
      
      override protected function onCollide(param1:Event) : void
      {
         super.onCollide(param1);
         §_-h2B§.play(§_-d27§.§_-h1O§);
         this.changeClipToIdle();
      }
      
      protected function changeClipToIdle() : void
      {
         armature.playAnimation(§_-K1t§.§_-J2e§);
      }
      
      protected function destroyDuplicates(param1:Class) : void
      {
         var _loc3_:AbstractBlocker = null;
         var _loc2_:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([param1]);
         for each(_loc3_ in _loc2_)
         {
            if(_loc3_ != this && owner.squad.getTeamId() == _loc3_.owner.squad.getTeamId())
            {
               _loc3_.disposed = true;
            }
         }
      }
      
      protected function onDamaged(param1:Event) : void
      {
         disposed = true;
      }
      
      protected function checkDuration(param1:Event) : void
      {
         if(--this.turnsLeft.value <= 0)
         {
            disposed = true;
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
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,x,y,this.paramSet.explosionRadius.value,§_-d27§.§_-Z1d§);
         §_-TZ§.removeListener(this,§_-L3Q§.§_-Qw§,this.onDamaged);
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.checkDuration);
         if(armature)
         {
            WorldClock.clock.remove(armature);
            armature.dispose();
            armature = null;
         }
         super.defaultOnDisposed(param1);
      }
   }
}

