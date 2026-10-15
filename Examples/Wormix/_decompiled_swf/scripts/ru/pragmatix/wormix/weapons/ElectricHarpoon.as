package ru.pragmatix.wormix.weapons
{
   import §_-31N§.§_-42G§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-b1F§.§_-G4§;
   import §_-b1F§.§_-v2Q§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.RopeHarpoon;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import ru.pragmatix.wormix.weapons.render.ClipRenderer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class ElectricHarpoon extends Weapon
   {
      
      private const §_-LI§:int = -7;
      
      private const §_-a23§:int = 50;
      
      private const §_-6P§:§_-TA§ = new §_-TA§(90);
      
      private var rope:RopeHarpoon;
      
      private var frames:int = 0;
      
      private var victim:§_-01J§;
      
      private var dragDamage:§_-TA§;
      
      private var §_-725§:Point;
      
      private var §_-E2a§:int;
      
      private var §_-U2X§:int;
      
      public function ElectricHarpoon(param1:uint = 0)
      {
         super(param1);
         MAX_POWER = MIN_POWER = 80;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         if(param1["trajectoryCrosshair"].value)
         {
            §_-z1W§(new §_-N1Z§(MAX_POWER,null,§_-N1Z§.§_-q17§));
         }
         else
         {
            §_-z1W§(new §_-W2f§());
         }
         this.dragDamage = new §_-TA§(param1["dragDamage"].value);
         super.§_-z2Y§(param1);
      }
      
      override public function detach() : void
      {
         if(Boolean(owner) && Boolean(owner.graphics))
         {
            owner.graphics.§_-i§();
         }
         super.detach();
         this.§_-V1E§();
         if(this.§_-725§)
         {
            Pool.putPoint(this.§_-725§);
            this.§_-725§ = null;
         }
      }
      
      override public function deactivate() : void
      {
         super.deactivate();
         this.§_-z2B§();
      }
      
      override protected function shoot() : void
      {
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            ControlSets.UNIT_CONTROL_SET.deactivate();
         }
         this.§_-725§ = getBarrelPoint(shotParams.angle);
         this.rope = new RopeHarpoon(owner,paramSet,shotParams.angle,MAX_POWER,this.§_-725§);
         this.rope.addEventListener(§_-L3Q§.§_-o13§,this.§_-y13§);
         this.rope.addEventListener(§_-L3Q§.DISPOSED,this.§_-92D§);
         nextTurnOnStable();
         owner.graphics.hideHands(renderer.getHideHandsPattern());
         if(renderer is ClipRenderer)
         {
            (renderer as ClipRenderer).removeHook();
         }
      }
      
      private function §_-y13§(param1:Event) : void
      {
         this.rope.removeEventListener(§_-L3Q§.§_-o13§,this.§_-y13§);
         if(owner != null)
         {
            if(param1.data.other != null && param1.data.other is §_-01J§ && param1.data.other != owner)
            {
               this.victim = §_-01J§(param1.data.other);
               this.§_-M1i§();
            }
            else
            {
               §_-G4§.§_-Ee§(owner,this.rope.x,this.rope.y,null,paramSet.damage.value * 1.5,paramSet.damageRadius.value,paramSet.explosionRadius.value,true,false,DamageType.SHOCK);
               Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMALL,this.rope.x,this.rope.y,paramSet.explosionRadius.value * 2);
               if(!§_-X1j§.§_-12n§.gameMaster.§_-U2O§())
               {
                  ControlSets.UNIT_CONTROL_SET.activate();
               }
               this.detach();
               this.§_-z2B§();
               nextTurnOnStable();
            }
         }
         else
         {
            this.§_-z2B§();
         }
         this.rope.disposed = true;
      }
      
      private function §_-M1i§() : void
      {
         §_-G4§.§_-Ee§(owner,this.rope.x,this.rope.y,this.victim as §_-F3o§,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value,false,false,DamageType.SHOCK);
         §_-h2B§.play(§_-d27§.§_-Z1d§);
         if(Boolean(owner) && !this.victim.disposed)
         {
            this.§_-E2a§ = this.victim.speedX;
            this.§_-U2X§ = this.victim.speedY;
            this.victim.graphics.§_-310§();
            this.victim.x = this.rope.x;
            this.victim.y = this.rope.y;
            this.victim.§_-41F§(this.§_-725§,RopePhysicModel.MAX_LENGTH);
            this.victim.setSpeedXY(0,-5);
            this.victim.keepUnstable = true;
            §_-K2b§();
            owner.graphics.hideHands(renderer.getHideHandsPattern());
            RopePhysicModel(this.victim.getPhysicModel()).deltaLength = this.§_-LI§;
            §_-TZ§.addListener(owner as EventDispatcher,§_-L3Q§.§_-Qw§,this.§_-aH§);
         }
         else if(!§_-X1j§.§_-12n§.gameMaster.§_-U2O§())
         {
            ControlSets.UNIT_CONTROL_SET.activate();
         }
         if(Boolean(this.victim) && this.victim.disposed)
         {
            this.detach();
         }
      }
      
      public function §_-aH§(param1:Event, param2:§_-v2Q§) : void
      {
         §_-TZ§.removeListener(param1.target,§_-L3Q§.§_-Qw§,this.§_-aH§);
         this.§_-V1E§();
         this.§_-z2B§();
         this.detach();
      }
      
      private function §_-z2B§() : void
      {
         this.§_-V1E§();
         §_-G1Z§();
         if(owner)
         {
            §_-TZ§.removeListener(owner as EventDispatcher,§_-L3Q§.§_-Qw§,this.§_-aH§);
         }
      }
      
      override protected function onFrame(param1:Event) : void
      {
         ++this.frames;
         if(!owner)
         {
            return;
         }
         if(Boolean(this.victim) && !this.victim.disposed)
         {
            if(this.frames % 7 == 0)
            {
               §_-G4§.§_-Ee§(owner,this.victim.x,this.victim.y,this.victim as §_-F3o§,this.dragDamage.value,0,0,false,false,DamageType.SHOCK);
               if(Boolean(owner) && Boolean(this.victim) && Boolean(this.§_-725§))
               {
                  Effects.lightning(this.§_-725§.x,this.§_-725§.y,this.victim.x,this.victim.y,4,10,13948116,5,1);
               }
               §_-h2B§.play(§_-d27§.§_-Z1d§);
            }
            if(this.frames % 10 == 0)
            {
               this.victim.addSpeedXY(0,-5);
            }
         }
         var _loc2_:Number = Physics.distance(owner as §_-F3o§,this.victim as §_-F3o§);
         if(!this.victim || this.victim.disposed || this.frames >= this.§_-6P§.value || _loc2_ < this.§_-a23§ || _loc2_ > RopePhysicModel.MAX_LENGTH + 100)
         {
            this.frames = 0;
            this.§_-V1E§();
            this.§_-z2B§();
            if(!§_-X1j§.§_-12n§.gameMaster.§_-U2O§())
            {
               ControlSets.UNIT_CONTROL_SET.activate();
            }
            this.detach();
            nextTurnOnStable();
         }
      }
      
      private function §_-V1E§() : void
      {
         var _loc1_:RopePhysicModel = null;
         if(this.victim == null)
         {
            return;
         }
         if(!this.victim.disposed && this.victim.getPhysicModel() is RopePhysicModel)
         {
            _loc1_ = RopePhysicModel(this.victim.getPhysicModel());
            _loc1_.clear();
            _loc1_.deltaLength = 0;
            this.victim.keepUnstable = false;
            this.victim.§_-dz§();
            this.victim.setSpeedXY(this.§_-E2a§,this.§_-U2X§);
            this.victim.graphics.§_-41n§();
         }
         this.victim = null;
      }
      
      private function §_-92D§(param1:Event) : void
      {
         this.§_-tg§();
         if(this.victim == null)
         {
            if(Boolean(§_-X1j§.§_-12n§.gameMaster) && !§_-X1j§.§_-12n§.gameMaster.§_-U2O§())
            {
               ControlSets.UNIT_CONTROL_SET.activate();
            }
            this.detach();
         }
      }
      
      private function §_-tg§() : void
      {
         this.rope.removeEventListener(§_-L3Q§.§_-o13§,this.§_-y13§);
         this.rope.removeEventListener(§_-L3Q§.DISPOSED,this.§_-92D§);
         this.rope = null;
      }
   }
}

