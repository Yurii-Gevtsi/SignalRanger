package ru.pragmatix.wormix.weapons
{
   import §_-31W§.§_-r26§;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-b2K§;
   import §_-P2i§.§_-b7§;
   import §_-Vg§.*;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-hO§.§_-03k§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-33j§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import feathers.themes.FontColor;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.ComponentForce;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.CatPhysicModel;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import ru.pragmatix.wormix.weapons.interfaces.§_-b29§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-o2d§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Color;
   import starling.utils.Pool;
   
   public class Jetpack extends Weapon implements §_-y2Q§, §_-jm§, §_-o2d§, §_-b29§
   {
      
      private static const §_-A17§:String = "jetpack_force";
      
      private static const §_-s2H§:uint = 6;
      
      private const §_-13s§:int = 12;
      
      private const §_-I11§:Number = 1.5;
      
      private const §_-UB§:Number = 0.5;
      
      private const §_-T2B§:§_-TA§ = new §_-TA§(1800);
      
      private const §_-EC§:int = 1;
      
      private const §_-T1§:int = this.§_-T2B§.value / 100;
      
      private const §_-Qv§:uint = this.§_-T2B§.value / this.§_-T1§;
      
      private var f:ComponentForce;
      
      private var §_-l2D§:int;
      
      private var §_-gd§:int;
      
      private var §_-K21§:Boolean = false;
      
      private var counter:§_-93W§;
      
      private var §_-Y2M§:FrameTimer;
      
      private var §_-43s§:§_-33j§;
      
      private var §_-F3j§:§_-V1R§;
      
      private var distance:uint;
      
      private var §_-C1h§:Point;
      
      public function Jetpack()
      {
         super();
         §_-a1R§.value = false;
         §_-N1N§ = false;
         §_-z1W§(new §_-cZ§());
         §_-YH§ = true;
         this.§_-Y2M§ = new FrameTimer(this.§_-EC§);
         this.counter = new §_-93W§(this.§_-T2B§.value / this.§_-T1§,Effects.FUEL_COLOR,Effects.FUEL_SHADOW,-1,Effects.CRITICAL_TIME_COLOR,Effects.CRITICAL_TIME_SHADOW);
      }
      
      override public function activate() : void
      {
         super.activate();
         §_-mG§ = true;
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         this.§_-C1h§ = Pool.getPoint();
         super.attach(param1);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-N1r§);
      }
      
      public function jumpShoot(param1:Boolean) : void
      {
         var _loc2_:Number = NaN;
         var _loc3_:§_-b2K§ = null;
         if(§_-X1j§.§_-12n§.gameMaster.§_-42C§() && !(owner.getPhysicModel() is CatPhysicModel))
         {
            _loc2_ = owner.speedY > 5 ? 5 : owner.speedY;
            owner.setSpeedXY(owner.speedX / 2,_loc2_);
            _loc3_ = new §_-b2K§();
            shootWithParams(_loc3_);
         }
      }
      
      override public function detach() : void
      {
         var _loc2_:uint = 0;
         var _loc1_:Boolean = false;
         if(this.§_-K21§)
         {
            _loc1_ = true;
            this.§_-Y2M§.stop();
            this.§_-Y2M§.removeEventListener(§_-03k§.§_-836§,this.§_-f1i§);
            if(!this.counter.isCompleted())
            {
               this.counter.detach();
            }
            this.stop();
         }
         if(Boolean(this.§_-43s§) && this.§_-43s§.§_-q2§())
         {
            this.§_-43s§.stop();
            this.§_-43s§ = null;
         }
         if(§_-X1j§.§_-12n§.gameMaster.activeSquad.isLocal())
         {
            _loc2_ = this.distance / Physics.METER;
            if(_loc2_ > 0)
            {
               §_-X1j§.§_-12n§.battleStats.increment(§_-r26§.§_-AH§,_loc2_);
               if(§_-X1j§.§_-12n§.battleStats.§_-B2c§())
               {
                  Effects.floatingText(_loc2_.toString(),Effects.MISC_COLOR,owner.x,owner.y);
               }
            }
         }
         this.distance = 0;
         super.detach();
         if(_loc1_ && §_-X1j§.§_-12n§.gameMaster.§_-42C§())
         {
            §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = true;
         }
         Pool.putPoint(this.§_-C1h§);
         this.§_-C1h§ = null;
      }
      
      override protected function shoot() : void
      {
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-Et§);
         §_-mG§ = false;
         this.§_-K21§ = true;
         this.§_-l2D§ = this.§_-T2B§.value;
         this.§_-gd§ = 0;
         this.distance = 0;
         this.§_-C1h§.setTo(owner.x,owner.y);
         owner.§_-01D§();
         owner.graphics.jetpack();
         owner.§_-H2R§();
         this.f = new ComponentForce(§_-A17§,0,-Force.GRAVITY.module * 0.75,this.§_-I11§,this.§_-I11§);
         owner.physParams.addForce(this.f);
         owner.stable = false;
         owner.keepUnstable = true;
         owner.§_-d2T§(clip);
         §_-b7§.§_-H33§(owner);
         ControlSets.UNIT_CONTROL_SET.deactivate();
         ControlSets.WEAPON_CONTROL_SET.activate();
         this.counter.attach(owner as §_-F3o§);
         this.counter.update(this.§_-Qv§);
         this.§_-Y2M§.addEventListener(§_-03k§.§_-836§,this.§_-f1i§);
         this.§_-Y2M§.start();
         this.§_-43s§ = §_-Zd§.once(owner as EventDispatcher,§_-L3Q§.§_-Qw§,function(param1:Event):void
         {
            detach();
         });
         §_-h2B§.play(§_-d27§.§_-s2k§);
         this.§_-F3j§ = §_-h2B§.play(§_-d27§.§_-F3I§,true);
      }
      
      private function stop() : void
      {
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         this.§_-K21§ = false;
         ControlSets.WEAPON_CONTROL_SET.deactivate();
         if(!owner.disposed)
         {
            owner.physParams.removeForce(§_-A17§);
            if(!§_-X1j§.§_-12n§.gameMaster.§_-Rk§())
            {
               ControlSets.UNIT_CONTROL_SET.activate();
            }
            owner.keepUnstable = false;
         }
         if(this.§_-F3j§ != null)
         {
            this.§_-F3j§.stop();
         }
         §_-h2B§.play(§_-d27§.§_-54§);
         this.counter.detach();
         §_-mG§ = true;
      }
      
      private function §_-f1i§(param1:Event = null) : void
      {
         if(owner == null)
         {
            return;
         }
         this.§_-l2D§ -= §_-s2H§;
         this.§_-gd§ += §_-s2H§;
         if(this.§_-gd§ < this.§_-T2B§.value + 200)
         {
            this.distance += §_-H3D§.distance(owner.x - this.§_-C1h§.x,owner.y - this.§_-C1h§.y);
            this.§_-C1h§.x = owner.x;
            this.§_-C1h§.y = owner.y;
            if(owner.speed > this.§_-13s§)
            {
               owner.setSpeedVector(owner.angle,this.§_-13s§);
            }
            this.counter.update(this.§_-l2D§ / this.§_-T1§);
            if(this.§_-l2D§ <= 0)
            {
               this.§_-53x§();
            }
         }
         else if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"jetpack fuel hacked"));
         }
      }
      
      public function isUsed() : Boolean
      {
         return this.§_-K21§;
      }
      
      public function weaponCharge() : void
      {
         this.§_-53x§();
      }
      
      private function §_-53x§() : void
      {
         var _loc1_:§_-01J§ = owner;
         this.detach();
         if(!§_-X1j§.§_-12n§.gameMaster.§_-Rk§() && !§_-X1j§.§_-12n§.gameMaster.§_-U2O§())
         {
            _loc1_.reattachWeapon();
            §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = true;
         }
      }
      
      public function weaponLeftPressed() : void
      {
         if(owner)
         {
            owner.turnLeft();
         }
         this.f.xNeg = this.§_-UB§;
      }
      
      public function weaponLeftReleased() : void
      {
         this.f.xNeg = 0;
      }
      
      public function weaponRightPressed() : void
      {
         if(owner)
         {
            owner.turnRight();
         }
         this.f.xPos = this.§_-UB§;
      }
      
      public function weaponRightReleased() : void
      {
         this.f.xPos = 0;
      }
      
      public function weaponUpPressed() : void
      {
         this.f.yNeg = this.§_-UB§;
      }
      
      public function weaponUpReleased() : void
      {
         this.f.yNeg = 0;
      }
      
      public function weaponDownPressed() : void
      {
         this.f.yPos = this.§_-UB§;
      }
      
      public function weaponDownReleased() : void
      {
         this.f.yPos = 0;
      }
   }
}

