package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-w1O§;
   import §_-A1K§.§_-TA§;
   import §_-H2g§.§_-di§;
   import §_-Y1O§.*;
   import §_-b1F§.§_-G4§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.Shell;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class CircusCannon extends Weapon
   {
      
      private const §_-W1P§:String = "resurces/_ammo/BigCannonWheels";
      
      private const §_-R1O§:String = "BigCannonWheels";
      
      private const §_-CV§:String = "BigCannon";
      
      private const §_-k1l§:uint = 30;
      
      private var §_-mP§:§_-di§;
      
      private var §_-cQ§:DisplayObject;
      
      public function CircusCannon()
      {
         super();
         MIN_POWER = MAX_POWER = 35;
         §_-z1W§(new §_-W2f§(§_-W2f§.§_-G3n§,MathUtil.§_-y2w§,MathUtil.§_-y2w§));
         §_-Y2U§ = new §_-w1O§(MIN_POWER - 5,MAX_POWER + 5,130,false);
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         param1.§_-m1X§ = 0;
         super.attach(param1);
      }
      
      override public function activate() : void
      {
         var _loc1_:§_-W2f§ = null;
         if(isActive())
         {
            return;
         }
         super.activate();
         ControlSets.CROSSHAIR_CONTROL_SET.activate();
         renderer.deactivate(owner);
         renderer.activate(owner);
         owner.graphics.§_-I3E§(this.§_-R1O§,false,true,§_-K1t§.§_-31c§);
         owner.graphics.lock();
         if(!this.§_-mP§)
         {
            this.§_-mP§ = §_-K1t§.buildArmature(this.§_-W1P§,§_-K1t§.§_-G1E§,§_-K1t§.§_-t1V§);
            WorldClock.clock.add(this.§_-mP§);
            this.§_-cQ§ = this.§_-mP§.display;
            this.§_-cQ§.scaleX = this.§_-cQ§.scaleY = §_-K1t§.§_-31c§;
         }
         owner.§_-oK§(this.§_-cQ§,0,0,false);
         if(§_-r2C§[§_-RS§] is §_-W2f§)
         {
            _loc1_ = §_-W2f§(§_-r2C§[§_-RS§]);
            this.rotateWeaponClip(_loc1_.getAngle());
            _loc1_.setAngle(_loc1_.getAngle());
         }
      }
      
      override public function deactivate() : void
      {
         owner.§_-d2T§(this.§_-cQ§);
         owner.graphics.unlock();
         owner.graphics.§_-924§();
         super.deactivate();
      }
      
      override protected function shoot() : void
      {
         var ownerDirection:int;
         var shellWheel:Shell;
         var shellCannon:Shell;
         var initX:int = 0;
         var initY:int = 0;
         var tempOwner:§_-01J§ = null;
         owner.setSpeedVector(shotParams.angle,MAX_POWER);
         owner.rotateUntilStable(false);
         owner.graphics.damaged();
         owner.attachTrail();
         initX = owner.x;
         initY = owner.y;
         ownerDirection = owner.direction;
         owner.x += this.§_-k1l§ * Math.cos(shotParams.angle);
         owner.y += this.§_-k1l§ * Math.sin(shotParams.angle);
         §_-h2B§.play(§_-d27§.§_-o2v§);
         tempOwner = owner;
         §_-Zd§.once(owner as EventDispatcher,§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            var _loc2_:Point = null;
            §_-G4§.§_-Ee§(tempOwner,tempOwner.x,tempOwner.y,null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value,false);
            §_-G4§.§_-Ee§(tempOwner,tempOwner.x,tempOwner.y,tempOwner as §_-F3o§,paramSet.damage.value / 3,0,0,true);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,tempOwner.x,tempOwner.y,3 * paramSet.explosionRadius.value,§_-d27§.§_-33W§);
            if(HitTest.unit(tempOwner.x,tempOwner.y))
            {
               _loc2_ = Pool.getPoint();
               if(Physics.nearestFreePoint(tempOwner.x,tempOwner.y,HitTest.unit,20,_loc2_))
               {
                  tempOwner.x = _loc2_.x;
                  tempOwner.y = _loc2_.y;
               }
               else
               {
                  tempOwner.x = initX;
                  tempOwner.y = initY;
               }
               Pool.putPoint(_loc2_);
               tempOwner.getPhysicModel().stop();
            }
         });
         §_-G4§.§_-Ee§(owner,owner.x,owner.y,null,0,50,30,false);
         ControlSets.UNIT_CONTROL_SET.deactivate();
         shellWheel = new Shell(initX,initY,this.§_-R1O§,1);
         shellWheel.setSpeedXY(-ownerDirection * 7,-5);
         shellCannon = new Shell(initX,initY,this.§_-CV§,1,§_-K1t§.§_-A2u§);
         shellCannon.setSpeedXY(ownerDirection * 6,-5);
         this.detach();
         nextTurnOnStable();
      }
      
      override public function detach() : void
      {
         if(owner)
         {
            owner.§_-d2T§(this.§_-cQ§);
         }
         if(this.§_-cQ§)
         {
            if(this.§_-cQ§.parent)
            {
               this.§_-cQ§.parent.removeChild(this.§_-cQ§);
               this.§_-cQ§.dispose();
               this.§_-cQ§ = null;
            }
         }
         if(this.§_-mP§)
         {
            WorldClock.clock.remove(this.§_-mP§);
            this.§_-mP§.dispose();
            this.§_-mP§ = null;
         }
         super.detach();
      }
      
      override public function rotateWeaponClip(param1:Number) : Number
      {
         if(this.§_-cQ§)
         {
            this.§_-cQ§.scaleX = owner.direction == §_-8K§.LEFT ? §_-K1t§.§_-31c§ : -§_-K1t§.§_-31c§;
         }
         return super.rotateWeaponClip(param1);
      }
   }
}

