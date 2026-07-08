package ru.pragmatix.wormix.weapons.ammo.type.mine
{
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-m0§.§_-X1F§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.weapons.ammo.GuardingBot;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class StickyGuardingBot extends GuardingBot
   {
      
      private static const FIRST_SHOT_FRAME:int = 0;
      
      private static const LAST_SHOT_FRAME:int = 30;
      
      private var landed:Boolean = false;
      
      private var lockPosition:Point;
      
      private var dummy:§_-F3o§;
      
      private var checked:Boolean;
      
      public function StickyGuardingBot(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:Boolean = true)
      {
         super(param1,param2,param3,param4,param5);
         addEventListener(§_-L3Q§.MINE_SET_UP,this.onMineSetup);
         firstShotFrame = FIRST_SHOT_FRAME;
         lastShotFrame = LAST_SHOT_FRAME + SHOT_PERIOD;
         this.dummy = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(isExploding || this.landed)
         {
            if(!this.lockPosition && physParams.collidesWithGround && HitTest.gameObject(x,y,10))
            {
               this.lockPosition = Pool.getPoint(x,y);
               physParams.removeAllForces();
            }
            else
            {
               if(!physParams.forces[Force.GRAVITY_NAME])
               {
                  physParams.addForce(Force.GRAVITY);
               }
               if(this.lockPosition)
               {
                  Pool.putPoint(this.lockPosition);
                  this.lockPosition = null;
               }
            }
         }
         if(this.lockPosition)
         {
            stable = true;
            setSpeedXY(0,0);
            x = this.lockPosition.x;
            y = this.lockPosition.y;
         }
      }
      
      override protected function defaultOnCollide(param1:Event) : void
      {
         super.defaultOnCollide(param1);
         if(!isExploding)
         {
            setSpeedXY(0,0);
            §_-h2B§.play(§_-d27§.§_-h1O§);
            if(!this.landed)
            {
               this.landed = true;
               mineStopped = true;
               this.checkSetup();
            }
            if(roaming)
            {
               stable = true;
            }
            else
            {
               physParams.removeAllForces();
               §_-Y2I§.stop();
            }
         }
      }
      
      protected function onMineSetup(param1:Event) : void
      {
         if(this.landed && !(param1.data as §_-X1F§).triggered)
         {
            physParams.removeAllForces();
            physParams.addForce(Force.GRAVITY);
            setSpeedXY(0,0);
            stable = true;
         }
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.AMMO);
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
      
      override protected function checkSetup() : void
      {
         super.checkSetup();
         if(Boolean(roaming) && Boolean(this.dummy) && !this.checked)
         {
            this.checked = true;
            §_-Zd§.§_-33o§(1,function():void
            {
               dummy.disposed = true;
               dummy = null;
            });
         }
      }
      
      override protected function beforeExplosion() : void
      {
         var explosionDummy:§_-F3o§ = null;
         explosionDummy = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
         §_-Zd§.§_-33o§(45,function():void
         {
            if(explosionDummy)
            {
               explosionDummy.disposed = true;
            }
         });
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         removeEventListener(§_-L3Q§.MINE_SET_UP,this.onMineSetup);
         if(Boolean(this.dummy) && !this.dummy.disposed)
         {
            this.dummy.disposed = true;
            this.dummy = null;
         }
         if(this.lockPosition)
         {
            Pool.putPoint(this.lockPosition);
            this.lockPosition = null;
         }
      }
   }
}

