package ru.pragmatix.wormix.weapons
{
   import §_-21p§.§_-N1H§;
   import §_-31W§.§_-r26§;
   import §_-A1K§.§_-TA§;
   import §_-P2i§.§_-b7§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-H3D§;
   import §_-su§.§_-21h§;
   import §_-su§.§_-Tu§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-o2d§;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-Y1z§ extends Weapon implements §_-jm§, §_-o2d§
   {
      
      private static const §_-s2H§:uint = 7;
      
      protected const §_-m2y§:String = "vehicle_force";
      
      private var maxFuel:uint;
      
      private var §_-f2J§:uint;
      
      private var §_-01P§:Number;
      
      private var §_-l10§:Number;
      
      private var §_-h1l§:Force;
      
      private var §_-q2n§:uint;
      
      protected var §_-l2D§:int;
      
      private var distance:uint;
      
      private var §_-C1h§:Point;
      
      private var counter:§_-93W§;
      
      protected var §_-02Z§:§_-Qi§ = null;
      
      private var §_-F1R§:Class;
      
      protected var §_-32z§:Boolean = true;
      
      public function §_-Y1z§(param1:Class, param2:uint = 1800, param3:uint = 8, param4:Number = 1.5, param5:Number = 0.5)
      {
         super();
         this.§_-F1R§ = param1;
         this.maxFuel = param2;
         this.§_-f2J§ = param3;
         this.§_-01P§ = param4;
         this.§_-l10§ = param5;
         this.§_-q2n§ = param2 / 100;
         §_-z1W§(new §_-cZ§());
         §_-B2r§ = true;
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         var _loc2_:§_-TA§ = property["maxFuel"] as §_-TA§;
         this.maxFuel = _loc2_ ? uint(_loc2_.value) : this.maxFuel;
         super.attach(param1);
      }
      
      override public function activate() : void
      {
         if(this.§_-02Z§ == null)
         {
            super.activate();
         }
      }
      
      override public function detach() : void
      {
         if(owner == null)
         {
            return;
         }
         if(this.§_-02Z§ != null && !this.§_-32z§)
         {
            this.§_-A35§(true);
         }
         super.detach();
         if(this.§_-C1h§)
         {
            Pool.putPoint(this.§_-C1h§);
            this.§_-C1h§ = null;
         }
      }
      
      override protected function shoot() : void
      {
         owner.§_-d2T§(clip);
         this.§_-02Z§ = new this.§_-F1R§(owner);
         if(this.§_-02Z§ is §_-Tu§)
         {
            §_-Tu§(this.§_-02Z§).init(paramSet,paramSetEx);
         }
         else if(this.§_-02Z§ is §_-21h§)
         {
            §_-21h§(this.§_-02Z§).init(paramSet);
         }
         this.§_-l2D§ = this.maxFuel;
         this.distance = 0;
         this.§_-C1h§ = Pool.getPoint(this.§_-02Z§.x,this.§_-02Z§.y);
         this.§_-h1l§ = new Force(this.§_-m2y§,-MathUtil.PI_HALF,Force.GRAVITY.module / 4 * 3);
         this.§_-02Z§.physParams.addForce(this.§_-h1l§);
         this.counter = new §_-93W§(this.maxFuel / this.§_-q2n§,Effects.FUEL_COLOR,Effects.FUEL_SHADOW,-1,Effects.CRITICAL_TIME_COLOR,Effects.CRITICAL_TIME_SHADOW);
         this.counter.attach(this.§_-02Z§);
         this.§_-32z§ = false;
         ControlSets.UNIT_CONTROL_SET.deactivate();
         ControlSets.WEAPON_CONTROL_SET.activate();
         §_-b7§.§_-j1X§(owner);
         §_-K2b§();
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var _loc2_:uint = 0;
         if(this.§_-02Z§ != null && owner != null)
         {
            if(!this.§_-02Z§.disposed)
            {
               this.§_-l2D§ -= §_-s2H§;
               _loc2_ = this.distance;
               this.distance += §_-H3D§.distance(this.§_-02Z§.x - this.§_-C1h§.x,this.§_-02Z§.y - this.§_-C1h§.y);
               if(uint(this.distance / Physics.METER / 5) > uint(_loc2_ / Physics.METER / 5) && §_-X1j§.§_-12n§.battleStats.§_-B2c§())
               {
                  Effects.floatingText("5",Effects.MISC_COLOR,this.§_-02Z§.x,this.§_-02Z§.y);
               }
               this.§_-C1h§.x = this.§_-02Z§.x;
               this.§_-C1h§.y = this.§_-02Z§.y;
               if(this.§_-02Z§.speed > this.§_-f2J§)
               {
                  this.§_-02Z§.setSpeedVector(this.§_-02Z§.angle,this.§_-f2J§);
               }
               this.counter.update(this.§_-l2D§ / this.§_-q2n§);
               if(this.§_-l2D§ <= 0)
               {
                  this.§_-A35§(true);
               }
               else if(this.§_-l2D§ > 150 && §_-X1j§.§_-12n§.gameMaster.§_-E3b§())
               {
                  §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"launchpad fuel hacked"));
               }
            }
            else
            {
               this.§_-A35§(false);
            }
         }
      }
      
      protected function §_-n2z§() : void
      {
      }
      
      protected function §_-A35§(param1:Boolean) : void
      {
         §_-G1Z§();
         ControlSets.WEAPON_CONTROL_SET.deactivate();
         ControlSets.UNIT_CONTROL_SET.activate();
         if(param1 && !this.§_-02Z§.disposed)
         {
            this.§_-n2z§();
         }
         if(§_-X1j§.§_-12n§.gameMaster.activeSquad.isLocal())
         {
            §_-X1j§.§_-12n§.battleStats.increment(§_-r26§.§_-AH§,this.distance / Physics.METER);
         }
         §_-X1j§.§_-12n§.scene.sceneCamera.followObject(§_-X1j§.§_-12n§.gameMaster.activeUnit as §_-F3o§);
         if(!§_-X1j§.§_-12n§.gameMaster.§_-U2O§())
         {
            §_-X1j§.§_-12n§.scene.showArrowOnUnit(§_-X1j§.§_-12n§.gameMaster.activeUnit);
         }
         nextTurnOnStable();
         this.§_-32z§ = true;
         this.§_-02Z§ = null;
         this.detach();
      }
      
      public function weaponLeftPressed() : void
      {
         if(this.§_-h1l§.ax > -this.§_-01P§)
         {
            this.§_-h1l§.ax -= this.§_-l10§;
         }
      }
      
      public function weaponLeftReleased() : void
      {
         if(this.§_-h1l§.ax < this.§_-01P§)
         {
            this.§_-h1l§.ax += this.§_-l10§;
         }
      }
      
      public function weaponRightPressed() : void
      {
         if(this.§_-h1l§.ax < this.§_-01P§)
         {
            this.§_-h1l§.ax += this.§_-l10§;
         }
      }
      
      public function weaponRightReleased() : void
      {
         if(this.§_-h1l§.ax > -this.§_-01P§)
         {
            this.§_-h1l§.ax -= this.§_-l10§;
         }
      }
      
      public function weaponUpPressed() : void
      {
         if(this.§_-h1l§.ay > -this.§_-01P§)
         {
            this.§_-h1l§.ay -= this.§_-l10§;
         }
      }
      
      public function weaponUpReleased() : void
      {
         if(this.§_-h1l§.ay < this.§_-01P§)
         {
            this.§_-h1l§.ay += this.§_-l10§;
         }
      }
      
      public function weaponDownPressed() : void
      {
         if(this.§_-h1l§.ay < this.§_-01P§)
         {
            this.§_-h1l§.ay += this.§_-l10§;
         }
      }
      
      public function weaponDownReleased() : void
      {
         if(this.§_-h1l§.ay > -this.§_-01P§)
         {
            this.§_-h1l§.ay -= this.§_-l10§;
         }
      }
   }
}

