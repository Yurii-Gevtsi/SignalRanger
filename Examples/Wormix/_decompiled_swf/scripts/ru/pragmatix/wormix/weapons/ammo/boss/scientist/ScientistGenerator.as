package ru.pragmatix.wormix.weapons.ammo.boss.scientist
{
   import §_-31N§.§_-42G§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-b1F§.§_-v2Q§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.display.DisplayObject;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class ScientistGenerator extends §_-F3o§
   {
      
      private static const ENABLED_ANIM_NAME:String = "enabled";
      
      private static const DISABLED_ANIM_NAME:String = "disabled";
      
      private static const ARMATURE_NAME:String = "Scientist_Generator";
      
      private static const CHARGE_INDICATOR_BONE_NAME:String = "Generator_indicator";
      
      private static const CHARGE_MAX:uint = 100;
      
      private static const CHARGE_MIN:uint = 0;
      
      private static const DISABLE_EDGE:uint = 20;
      
      public var charge:int = 100;
      
      private var chargeBarDefaultWidth:uint;
      
      private var owner:§_-01J§;
      
      private var armature:§_-di§;
      
      public function ScientistGenerator(param1:§_-01J§, param2:Number, param3:Number)
      {
         super();
         §_-Nt§ = true;
         this.owner = param1;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_AMMO;
         physParams.collideType = CollideType.OTHER;
         stable = true;
         this.x = param2;
         this.y = param3;
         this.armature = §_-K1t§.buildArmature(ARMATURE_NAME,ENABLED_ANIM_NAME,§_-K1t§.§_-B1s§);
         WorldClock.clock.add(this.armature);
         this.armature.playAnimation(ENABLED_ANIM_NAME);
         clip = this.armature.display as DisplayObject;
         _width = (this.armature.display as Sprite).width;
         _height = (this.armature.display as Sprite).height;
         this.chargeBarDefaultWidth = this.armature.getSlot(CHARGE_INDICATOR_BONE_NAME).display.width;
         this.refreshChargeBar();
         §_-TZ§.addListener(this,§_-L3Q§.§_-Qw§,this.defaultOnDamaged);
         §_-Y2I§.addEventListener(§_-42G§.§_-o1a§,this.onUnstable);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      public function charging(param1:uint) : void
      {
         this.charge += param1;
         this.charge = this.charge > CHARGE_MAX ? int(CHARGE_MAX) : this.charge;
         this.refreshChargeBar();
      }
      
      private function refreshChargeBar() : void
      {
         var _loc2_:Quad = null;
         var _loc1_:DisplayObject = this.armature.getSlot(CHARGE_INDICATOR_BONE_NAME).display;
         if(!_loc1_.mask)
         {
            _loc2_ = new Quad(_loc1_.width,_loc1_.height);
            _loc1_.mask = _loc2_;
         }
         _loc1_.mask.width = this.chargeBarDefaultWidth * this.charge / 100;
         this.checkState();
      }
      
      private function checkState() : void
      {
         this.armature.playAnimation(this.isOn ? ENABLED_ANIM_NAME : DISABLED_ANIM_NAME,0);
      }
      
      override public function defaultOnDamaged(param1:Event, param2:§_-v2Q§) : void
      {
         if(param2.source.owner != this.owner)
         {
            this.charge = this.charge < param2.power ? int(CHARGE_MIN) : int(this.charge - param2.power);
            this.refreshChargeBar();
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.armature)
         {
            WorldClock.clock.remove(this.armature);
            this.armature.dispose();
            this.armature = null;
         }
         §_-Y2I§.removeEventListener(§_-42G§.§_-o1a§,this.onUnstable);
         §_-TZ§.removeListener(this,§_-L3Q§.§_-Qw§,this.defaultOnDamaged);
         super.defaultOnDisposed(param1);
      }
      
      private function onUnstable(param1:Event) : void
      {
         stable = true;
      }
      
      public function get isFull() : Boolean
      {
         return this.charge >= CHARGE_MAX;
      }
      
      public function get isOn() : Boolean
      {
         return this.charge > DISABLE_EDGE;
      }
      
      override public function nextState() : void
      {
         super.nextState();
      }
   }
}

