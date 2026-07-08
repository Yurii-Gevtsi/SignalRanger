package ru.pragmatix.wormix.weapons.ammo.boss.emperor
{
   import §_-91A§.§_-sH§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-P1B§.§_-EB§;
   import §_-b1F§.§_-v2Q§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   
   public class EmperorTerracottaWarrior extends §_-F3o§ implements §_-sH§
   {
      
      public static const DEFAULT_HP:uint = 50;
      
      public static const BONUS_HP_PER_UNIT:uint = 20;
      
      public static const GROUND_SHIFT:uint = 16;
      
      public var owner:§_-01J§;
      
      public var name:String;
      
      public var direction:int = §_-8K§.LEFT;
      
      protected var armatureName:String;
      
      private var armature:§_-di§;
      
      public function EmperorTerracottaWarrior(param1:§_-01J§, param2:Number, param3:Number)
      {
         super();
         this.owner = param1;
         §_-Nt§ = true;
         var _loc4_:uint = §_-X1j§.§_-12n§.gameMaster.§_-83I§(param1).length;
         §_-33R§ = new §_-EB§(this,DEFAULT_HP + _loc4_ * BONUS_HP_PER_UNIT,0,0);
         this.x = param2;
         this.y = param3;
         _width = Physics.UNIT_WIDTH;
         _height = Physics.UNIT_HEIGHT;
         this.armature = §_-K1t§.buildArmature(this.armatureName,§_-K1t§.§_-G1E§,§_-K1t§.§_-B1s§);
         WorldClock.clock.add(this.armature);
         this.armature.display.scaleX = this.armature.display.scaleY = 0.5;
         this.armature.addEventListener(§_-s1d§.LOOP_COMPLETE,this.gotoIdleAnimation);
         this.armature.playAnimation(§_-K1t§.§_-XN§);
         clip = this.armature.display;
         §_-TZ§.addListener(this,§_-L3Q§.§_-Qw§,this.defaultOnDamaged);
      }
      
      private function gotoIdleAnimation(param1:Event) : void
      {
         this.armature.removeEventListener(§_-s1d§.LOOP_COMPLETE,this.gotoIdleAnimation);
         this.armature.animation.gotoAndStop(§_-K1t§.§_-G1E§,0);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.OTHER);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_ALL;
         physParams.sinkable = true;
         physParams.setGroundShift(0,GROUND_SHIFT);
         §_-Y2I§ = new GenericPhysicModel(physParams);
      }
      
      protected function turn(param1:int) : void
      {
         if(param1 < this.x && this.direction != §_-8K§.LEFT || param1 > this.x && this.direction != §_-8K§.RIGHT)
         {
            clip.scaleX *= -1;
            this.direction = this.direction == §_-8K§.LEFT ? §_-8K§.RIGHT : §_-8K§.LEFT;
         }
      }
      
      public function attackEnemy() : void
      {
      }
      
      override public function defaultOnDamaged(param1:Event, param2:§_-v2Q§) : void
      {
         super.defaultOnDamaged(param1,param2);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-TZ§.removeListener(this,§_-L3Q§.§_-Qw§,this.defaultOnDamaged);
         this.armature.playAnimation(§_-K1t§.§_-s1g§);
         this.armature.addEventListener(§_-s1d§.COMPLETE,this.disposeArmature);
         §_-z1H§ = null;
         super.defaultOnDisposed(param1);
      }
      
      private function disposeArmature(param1:Event) : void
      {
         if(this.armature)
         {
            this.armature.removeEventListener(§_-s1d§.COMPLETE,this.disposeArmature);
            if(this.armature.display)
            {
               this.armature.display.removeFromParent();
            }
            if(WorldClock.clock.contains(this.armature))
            {
               WorldClock.clock.remove(this.armature);
            }
            this.armature.dispose();
            this.armature = null;
         }
      }
      
      public function getNameHintInfoPanel() : String
      {
         return this.name;
      }
      
      public function getSquadNumber() : uint
      {
         return this.owner.getSquadNumber();
      }
      
      public function get hp() : int
      {
         return attributes.hp.value;
      }
      
      public function get doesntCountTurns() : Boolean
      {
         return true;
      }
   }
}

