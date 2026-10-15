package ru.pragmatix.wormix.weapons.ammo
{
   import §_-G16§.§_-Y2x§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import starling.events.Event;
   
   public class LightningBullet extends §_-Qi§ implements §_-Y2x§
   {
      
      protected var paramSet:§_-C3e§;
      
      protected var source:§_-F3o§;
      
      protected var victim:§_-01J§ = null;
      
      protected var fxLifeTime:uint;
      
      protected var friendlyFire:Boolean;
      
      public function LightningBullet()
      {
         super(null,0,0,false,0,DamageType.SHOCK);
         §_-92S§ = false;
      }
      
      public static function create(param1:§_-01J§, param2:§_-F3o§, param3:§_-C3e§, param4:Number, param5:uint, param6:uint, param7:uint, param8:Boolean = false) : void
      {
         var _loc9_:LightningBullet = new LightningBullet();
         _loc9_.init(param1,param2,param3,param4,param5,param6,param7,param8,true);
      }
      
      public function init(param1:§_-01J§, param2:§_-F3o§, param3:§_-C3e§, param4:Number, param5:uint, param6:uint, param7:uint, param8:Boolean = false, param9:Boolean = false) : void
      {
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         this.owner = param1;
         this.§_-iN§ = param6;
         this.paramSet = param3;
         this.source = param2;
         this.fxLifeTime = param7;
         this.friendlyFire = param8;
         if(§_-z1H§)
         {
            §_-z1H§.rotation = param4;
         }
         §_-32§ = §_-uN§;
         §_-Y2I§.keepUnstable = true;
         §_-Y2I§.x = param2.x;
         §_-Y2I§.y = param2.y;
         (§_-Y2I§ as AmmoPhysicModel).stopOnCollision = false;
         physParams.removeAllForces();
         setSpeedVector(param4,param5);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      protected function onCollide(param1:Event) : void
      {
         if(param1.data.other == null)
         {
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,this.x,this.y,this.paramSet.damageRadius.value);
            inflictDamage(param1.data.other,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value,true,true);
            disposed = true;
         }
         else if(param1.data.other is §_-01J§ && (this.friendlyFire || §_-01J§(param1.data.other).squad.getTeamId() != owner.squad.getTeamId()))
         {
            this.victim = §_-01J§(param1.data.other);
            inflictDamage(param1.data.other,this.paramSet.damage.value,0,0,true,true);
            disposed = true;
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-h2B§.play(§_-d27§.§_-Z1d§);
         if(this.victim)
         {
            Effects.lightningOnObjects(this.source,this.victim as §_-F3o§,this.fxLifeTime,10,6068463,8);
         }
         else
         {
            Effects.lightningOnObject(this.source,x,y,this.fxLifeTime,10,6068463,8);
         }
         this.source = null;
         this.victim = null;
         super.defaultOnDisposed(param1);
      }
      
      public function getSource() : §_-F3o§
      {
         return this.source;
      }
      
      public function dispose() : void
      {
         this.paramSet = null;
         this.source = null;
         this.victim = null;
      }
      
      public function removeFromPool() : void
      {
         this.dispose();
      }
   }
}

