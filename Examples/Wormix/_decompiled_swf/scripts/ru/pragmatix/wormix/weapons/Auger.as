package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import §_-A2F§.§_-4B§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-b1F§.§_-G4§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-zI§.*;
   import dragonBones.Armature;
   import dragonBones.animation.WorldClock;
   import feathers.controls.ImageLoader;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.interfaces.§_-o2d§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import ru.pragmatix.wormix.weapons.render.ClipRenderer;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   
   public class Auger extends Weapon implements §_-y2Q§, §_-o2d§
   {
      
      protected var §_-s2n§:§_-TA§ = new §_-TA§(0);
      
      protected var §_-qa§:DisplayObject;
      
      protected var §_-H1Z§:Boolean = false;
      
      protected var §_-b1p§:int;
      
      protected var §_-xG§:Number;
      
      protected var §_-S1o§:Number;
      
      protected var angle:Number;
      
      protected const DRILL_DAMAGE:uint = 4;
      
      private var drillDistance:§_-TA§;
      
      private const §_-R1q§:Number = 0.5235987755982988;
      
      private const ANGLE_FLUCT:Number = 0.5235987755982988;
      
      private const §_-YE§:Number = 0.5235987755982988;
      
      private const §_-J2o§:Number = 0.5235987755982988;
      
      private var §_-e2s§:§_-V1R§;
      
      private var §_-e1h§:int;
      
      private var armature:§_-di§;
      
      private var setPlug:Boolean = false;
      
      private var §_-g2y§:Vector.<Point>;
      
      public function Auger()
      {
         super();
         §_-z1W§(new §_-W2f§(this.ANGLE_FLUCT,this.§_-YE§,this.§_-J2o§,§_-W2f§.§_-o18§,true));
         §_-YH§ = true;
         §_-B2r§ = false;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         this.drillDistance = §_-TA§(param1.drillDistance);
         super.§_-z2Y§(param1);
      }
      
      override protected function shoot() : void
      {
         this.§_-b1p§ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-O1C§();
         this.§_-g2y§ = new Vector.<Point>();
         if(property["setPlug"])
         {
            this.setPlug = property["setPlug"];
         }
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            ControlSets.UNIT_CONTROL_SET.deactivate();
            ControlSets.WEAPON_CONTROL_SET.activate();
         }
         this.§_-s2n§ = new §_-TA§(0);
         this.§_-H1Z§ = true;
         shooting = true;
         this.§_-e1h§ = owner.direction;
         §_-K2b§();
         this.armature = §_-K1t§.buildArmature("resurces/_ammo/" + paramSet.ammoClassName,§_-K1t§.§_-Y1c§,§_-K1t§.§_-t1V§);
         this.armature.display.scaleX = this.armature.display.scaleY = §_-K1t§.§_-31c§;
         WorldClock.clock.add(this.armature);
         var _loc1_:Armature = this.armature.getSlot("weapon").childArmature;
         _loc1_.animation.gotoAndPlay("play",0,-1,0);
         var _loc2_:DisplayObject = _loc1_.getSlot("masked_clip").display;
         var _loc3_:DisplayObject = _loc1_.getSlot("mask_clip").display;
         _loc2_.mask = _loc3_;
         this.§_-qa§ = new Sprite();
         (this.§_-qa§ as Sprite).addChild(this.armature.display);
         this.§_-qa§.rotation = clip.rotation;
         owner.§_-U8§(this.§_-qa§,ClipRenderer(renderer).dx,ClipRenderer(renderer).dy);
         ClipRenderer(renderer).showDragondonedHand(owner.record.§_-P2e§(),this.armature);
         owner.§_-f2T§();
         owner.graphics.hideHands(renderer.getHideHandsPattern());
         this.§_-xG§ = owner.y;
         this.angle = shotParams.angle;
         this.§_-117§();
         this.§_-e2s§ = §_-h2B§.play(§_-d27§.§_-d2A§,true);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-Et§);
         §_-TZ§.addListener(owner as EventDispatcher,§_-L3Q§.§_-Qw§,this.§_-C17§,§_-4V§.§_-DA§);
      }
      
      private function §_-C17§(param1:Event) : void
      {
         this.detach();
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         param1.§_-m1X§ = 0;
         super.attach(param1);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-N1r§);
      }
      
      override public function detach() : void
      {
         var _loc1_:Point = null;
         if(owner != null)
         {
            this.§_-H1C§();
            owner.§_-m1X§ = 0;
            §_-TZ§.removeListener(owner as EventDispatcher,§_-L3Q§.§_-Qw§,this.§_-C17§);
         }
         super.detach();
         if(Boolean(this.§_-g2y§) && this.§_-g2y§.length > 0)
         {
            for each(_loc1_ in this.§_-g2y§)
            {
               Pool.putPoint(_loc1_);
            }
            this.§_-g2y§.length = 0;
         }
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         ControlSets.WEAPON_CONTROL_SET.deactivate();
         ControlSets.UNIT_CONTROL_SET.activate();
      }
      
      override public function deactivate() : void
      {
         if(§_-82J§())
         {
            if(Boolean(§_-X1j§.§_-12n§.gameMaster) && §_-X1j§.§_-12n§.gameMaster.activeUnit == owner)
            {
               §_-m2§(§_-r2C§[§_-RS§]).deactivate();
            }
            ControlSets.UNIT_CONTROL_SET.deactivate();
            ControlSets.WEAPON_CONTROL_SET.activate();
         }
         else
         {
            super.deactivate();
         }
      }
      
      private function §_-117§() : void
      {
         this.§_-S1o§ = Math.tan(this.angle) * this.§_-e1h§ * owner.attributes.§_-kN§.value / 9;
         if(Math.abs(this.§_-S1o§) < 0.001)
         {
            this.§_-S1o§ = 0;
         }
      }
      
      private function §_-H1C§() : void
      {
         owner.graphics.§_-i§();
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         if(this.§_-H1Z§)
         {
            this.§_-H1Z§ = false;
            shooting = false;
            if(this.§_-e2s§)
            {
               this.§_-e2s§.stop();
               this.§_-e2s§ = null;
            }
            if(this.§_-qa§)
            {
               owner.§_-n28§(this.§_-qa§);
               this.§_-qa§.dispose();
               this.§_-qa§ = null;
            }
            if(this.armature)
            {
               WorldClock.clock.remove(this.armature);
               this.armature.dispose();
               this.armature = null;
            }
            owner.§_-01D§();
            nextTurnOnStable();
            this.§_-s2n§.value = this.drillDistance.value;
         }
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc2_:Number = Math.abs(this.angle);
         var _loc3_:Number = Math.round(_loc2_ / this.ANGLE_FLUCT);
         if(Math.abs(_loc2_ - _loc3_ * this.ANGLE_FLUCT) > 0.1)
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"auger angle hacked"));
            }
            this.detach();
         }
         else if(this.§_-s2n§.value++ >= this.drillDistance.value)
         {
            this.detach();
         }
         else
         {
            this.§_-117§();
            _loc4_ = Math.round(this.§_-xG§ + this.§_-S1o§) - Math.round(this.§_-xG§);
            this.§_-xG§ += this.§_-S1o§;
            _loc5_ = owner.x + (this.§_-e1h§ == §_-8K§.LEFT ? -1 : 1);
            _loc6_ = this.§_-S1o§ < 0 ? -1 : 0;
            _loc7_ = Math.round(owner.y + _loc4_ + _loc6_);
            if(this.§_-s2n§.value % 5 == 0)
            {
               §_-G4§.§_-Ee§(owner,_loc5_,_loc7_,null,this.DRILL_DAMAGE,Physics.UNIT_HALF_HEIGHT * 2,Physics.UNIT_HALF_HEIGHT,false,false,DamageType.FIRE);
            }
            else
            {
               §_-X1j§.§_-12n§.scene.drawDamage(_loc5_,_loc7_,Physics.UNIT_HALF_HEIGHT + 1);
            }
            if(owner != null)
            {
               if(this.setPlug)
               {
                  this.§_-g2y§.push(Pool.getPoint(_loc5_,_loc7_));
                  this.setPlug = !§_-4B§.§_-V1o§(owner,this.§_-g2y§,false);
               }
               if(!owner.isImmobile())
               {
                  if(this.§_-e1h§ == §_-8K§.LEFT)
                  {
                     owner.§_-u2K§();
                  }
                  else
                  {
                     owner.§_-o1k§();
                  }
               }
            }
         }
      }
      
      public function weaponCharge() : void
      {
         this.§_-s2n§.value = this.drillDistance.value;
      }
      
      private function §_-j1V§() : void
      {
         this.§_-E1l§(-this.ANGLE_FLUCT);
      }
      
      protected function §_-xB§() : void
      {
         this.§_-E1l§(this.ANGLE_FLUCT);
      }
      
      private function §_-E1l§(param1:Number) : void
      {
         var _loc2_:int = owner.direction;
         var _loc3_:Number = _loc2_ == Direction.RIGHT.value ? this.angle : this.angle - Math.PI;
         _loc3_ = starling.utils.MathUtil.clamp(_loc3_ + _loc2_ * param1,-this.§_-J2o§,this.§_-YE§);
         this.angle = _loc2_ == Direction.RIGHT.value ? _loc3_ : _loc3_ + Math.PI;
         this.§_-qa§.rotation = this.angle;
      }
      
      public function weaponUpPressed() : void
      {
      }
      
      public function weaponUpReleased() : void
      {
         this.§_-j1V§();
      }
      
      public function weaponDownPressed() : void
      {
      }
      
      public function weaponDownReleased() : void
      {
         this.§_-xB§();
      }
   }
}

