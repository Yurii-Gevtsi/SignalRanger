package ru.pragmatix.wormix.weapons.ammo
{
   import §_-A1K§.§_-TA§;
   import §_-C2t§.§_-C31§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Nl§;
   import §_-j2R§.§_-73G§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-sQ§.§_-H3D§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-D8§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Bullet extends §_-Qi§ implements IPooledAmmo
   {
      
      public static const DEFAULT_TRAIL_COLOR:uint = 16762931;
      
      public static const DEFAULT_TRAIL_COLOR_G:uint = 16777215;
      
      public static const SIMPLE_BULLET_DEFAULT_LIFETIME:uint = 500;
      
      private static const MIN_DAMAGE:uint = 50;
      
      private static const DAMAGE_REDUCTION:uint = 10;
      
      private static const PIERCING_BULLET_MIN_DAMAGE_DIST:int = 100;
      
      private static const PIERCING_BULLET_MAX_DAMAGE_DIST:int = 500;
      
      private static const PIERCING_BULLET_DAMAGE_DIST_DIFF:int = PIERCING_BULLET_MAX_DAMAGE_DIST - PIERCING_BULLET_MIN_DAMAGE_DIST;
      
      private static const REDUCTION_BULLET_ARMOR_REDUCE_AMOUNT:§_-TA§ = new §_-TA§(2);
      
      private static const SLOW_SPEED_BULLET_DURATION_DEBUFF:§_-TA§ = new §_-TA§(2);
      
      private static var ammoObjectsFactory:AmmosFactory = AmmosFactory.getInstance();
      
      protected var paramSet:§_-C3e§;
      
      private var pushForce:Number;
      
      private var silent:Boolean;
      
      private var objectType:int;
      
      private var bulletType:int;
      
      private var hits:uint = 2;
      
      private var damage:uint;
      
      private var unitsHit:Array = [];
      
      private var collideFunction:Function;
      
      private var _createEndParticles:Boolean;
      
      private var armorReductionPc:uint = 0;
      
      private var alwaysCrit:Boolean = false;
      
      public function Bullet()
      {
         super(null,0,0,false,SIMPLE_BULLET_DEFAULT_LIFETIME);
         this.§_-92S§ = false;
         this.§_-32§ = §_-uN§;
         this.objectType = AmmosFactory.OBJECT_BULLET;
         this._createEndParticles = true;
      }
      
      protected function onCollide(param1:Event) : void
      {
         this.collideFunction(param1);
      }
      
      protected function effectExplode(param1:§_-F3o§) : void
      {
         if(param1 == null)
         {
            Effects.explodeParticles(x,y,8);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_BULLET,this.x,this.y,2 * this.paramSet.damageRadius.value);
         }
         else
         {
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_BULLET,this.x,this.y,20);
         }
      }
      
      override protected function defaultOnSink(param1:Event) : void
      {
         var _loc2_:Array = null;
         if(y > 0 && x > -200 && x < §_-X1j§.§_-12n§.scene.sceneW + 200)
         {
            _loc2_ = [§_-d27§.WATER_IMPACT1,§_-d27§.WATER_IMPACT2,§_-d27§.WATER_IMPACT3];
            §_-h2B§.play(§_-d27§.§_-k4§(_loc2_));
            if(this.bulletType == AmmosFactory.BULLET_FIRE)
            {
               Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_BULLET,x,y,this.paramSet.explosionRadius.value * 10);
            }
         }
         disposed = true;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-TZ§.removeListener(this,§_-L3Q§.§_-Qw§,defaultOnDamaged);
         §_-Y2I§.removeEventListener(§_-L3Q§.§_-kl§,this.defaultOnSink);
         disposed = true;
         this.setDefault();
         this.remove();
      }
      
      public function attach(param1:§_-F3o§, param2:Array) : void
      {
      }
      
      public function setDefault() : void
      {
         §_-In§ = DamageType.PHYSICAL;
         §_-22h§ = §_-n18§;
         physParams.removeAllForces();
         physParams.addForce(Force.GRAVITY);
         physParams.optimizedMovement = true;
         physParams.ignoresTerrain = false;
         physParams.sinkable = false;
         this._createEndParticles = true;
         §_-iN§ = SIMPLE_BULLET_DEFAULT_LIFETIME;
         this.paramSet = null;
         this.pushForce = 0;
         this.damage = 0;
         setSpeedXY(0,0);
         this.collideFunction = null;
         this.hits = 0;
         this.unitsHit = [];
         if(§_-z1H§)
         {
            if(§_-z1H§.parent)
            {
               §_-z1H§.parent.removeChild(§_-z1H§);
            }
            §_-z1H§.dispose();
         }
         §_-z1H§ = null;
         (§_-Y2I§ as AmmoPhysicModel).setDefault();
         if(§_-53z§)
         {
            §_-53z§.setTo(0,0);
         }
         else
         {
            §_-53z§ = Pool.getPoint(0,0);
         }
         §_-ni§ = true;
         this.alwaysCrit = false;
         this.armorReductionPc = 0;
      }
      
      public function remove() : void
      {
         ammoObjectsFactory.removeObject(this,this.objectType);
      }
      
      public function dispose() : void
      {
      }
      
      public function isSimpleBullet() : Boolean
      {
         return this.bulletType != AmmosFactory.BULLET_FIRE;
      }
      
      public function initAmmo(param1:int, param2:§_-01J§, param3:Point, param4:§_-C3e§, param5:Number, param6:uint, param7:Number = 2, param8:Boolean = false, param9:Boolean = false, param10:Boolean = true) : void
      {
         var _loc12_:Point = null;
         var _loc11_:§_-D8§ = §_-X1j§.§_-12n§;
         id = Boolean(_loc11_) && Boolean(_loc11_.scene) ? _loc11_.scene.gameObjectProcessor.§_-qj§() : 0;
         this.disposed = false;
         this.owner = param2;
         this.setDefault();
         this.bulletType = param1;
         this.paramSet = param4;
         this.pushForce = param7;
         this.silent = param8;
         this.§_-U1o§ = param9;
         if(§_-X1j§.§_-12n§.scene.hitTestPoint(param3.x,param3.y) || §_-X1j§.§_-12n§.scene.hitTestPoint((param2.x + param3.x) / 2,(param2.y + param3.y) / 2))
         {
            param3.x = param2.x;
            param3.y = param2.y;
         }
         AmmoPhysicModel(§_-Y2I§).checkEnemiesOnStart();
         AmmoPhysicModel(§_-Y2I§).addToIgnored(param2 as §_-F3o§);
         setSpeedVector(param5,param6);
         switch(param1)
         {
            case AmmosFactory.BULLET_KALASH:
               physParams.optimizedMovement = false;
               this.damage = param4.damage.value;
               AmmoPhysicModel(§_-Y2I§).stopOnCollision = false;
               this.collideFunction = this.kalashBulletCollideFunction;
               this.hits = 2;
               break;
            case AmmosFactory.BULLET_PIERCING:
               this.damage = param4.damage.value;
               §_-U1o§ = true;
               physParams.removeForce(Force.GRAVITY_NAME);
               physParams.optimizedMovement = false;
               physParams.ignoresTerrain = true;
               this.collideFunction = this.piercingBulletCollideFunction;
               this.hits = 0;
               break;
            case AmmosFactory.BULLET_REDUCTING:
               this.collideFunction = this.reductionArmorBulletCollideFunction;
               this.hits = 2;
               break;
            case AmmosFactory.BULLET_FIRE:
               _loc12_ = param2.weapon.getBarrelPoint(param5);
               physParams.addForce(Force.WIND);
               physParams.sinkable = true;
               this.collideFunction = this.fireBulletCollideFunction;
               break;
            case AmmosFactory.BULLET_SLOWING:
               this.collideFunction = this.slowSpeedBulletCollideFunction;
               this.hits = 2;
               break;
            default:
               this.collideFunction = this.simpleBulletCollideFunction;
               this.hits = 2;
         }
         if(param10)
         {
            new Shell(param2.x,param2.y).setSpeedXY(-param2.direction * §_-X1j§.randomSeed.§_-Fh§(3,7),-1 * §_-X1j§.randomSeed.§_-Fh§(6,15));
         }
         initialize(_loc12_ ? _loc12_ : param3,param4.ammoClassName,true);
         if(_loc12_)
         {
            Pool.putPoint(_loc12_);
         }
         if(§_-z1H§)
         {
            §_-z1H§.rotation = param5;
         }
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-TZ§.addListener(this,§_-L3Q§.§_-Qw§,defaultOnDamaged);
         §_-Y2I§.addEventListener(§_-L3Q§.§_-kl§,this.defaultOnSink);
         _loc11_.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      public function initAmmoByBulletName(param1:String, param2:§_-01J§, param3:Point, param4:§_-C3e§, param5:Number, param6:uint, param7:Number = 2, param8:Boolean = false, param9:Boolean = false, param10:Boolean = true) : void
      {
         var _loc11_:uint = 0;
         switch(param1)
         {
            case "Bullet":
               _loc11_ = uint(AmmosFactory.BULLET_SIMPLE);
               break;
            case "ReductionArmorBullet":
               _loc11_ = uint(AmmosFactory.BULLET_REDUCTING);
               break;
            case "SlowSpeedBullet":
               _loc11_ = uint(AmmosFactory.BULLET_SLOWING);
               break;
            default:
               throw new Error("NO SUCH BULLET CLASS");
         }
         this.initAmmo(_loc11_,param2,param3,param4,param5,param6,param7,param8,param9,param10);
      }
      
      override public function nextState() : void
      {
         super.nextState();
      }
      
      public function addTrail(param1:uint = 16762931, param2:uint = 16777215, param3:uint = 6, param4:uint = 2, param5:Number = 0.996) : void
      {
         Effects.attachTrail(this,param3,param4,param1,param2,param5);
      }
      
      private function simpleBulletCollideFunction(param1:Event) : void
      {
         this.effectExplode(param1.data.other);
         inflictDamage(param1.data.other,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         if(param1.data.other != null)
         {
            §_-G4§.§_-u1v§(param1.data.other,0,-this.pushForce,true);
         }
         if(!this.silent)
         {
            §_-h2B§.play(Math.random() > 0.5 ? §_-d27§.RIKOSHET1 : §_-d27§.RIKOSHET2);
         }
         disposed = true;
      }
      
      private function kalashBulletCollideFunction(param1:Event) : void
      {
         var _loc2_:§_-F3o§ = param1.data.other;
         if(_loc2_ is §_-01J§ && this.unitsHit.indexOf(_loc2_) != -1)
         {
            return;
         }
         inflictDamage(_loc2_,this.damage,_loc2_ is §_-01J§ ? 0 : this.paramSet.damageRadius.value,_loc2_ is §_-01J§ ? 0 : this.paramSet.explosionRadius.value);
         if(_loc2_ is §_-01J§)
         {
            this.unitsHit.push(_loc2_);
            this.damage *= 0.5;
            _loc2_.addSpeedXY(§_-H3D§.sign(speedX),-1);
         }
         if(--this.hits == 0 || !(_loc2_ is §_-01J§))
         {
            disposed = true;
            removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         }
         if(_loc2_ == null)
         {
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_BULLET,x,y,2 * this.paramSet.damageRadius.value);
         }
      }
      
      private function piercingBulletCollideFunction(param1:Event) : void
      {
         var _loc3_:Number = NaN;
         var _loc6_:§_-C31§ = null;
         if(param1.data.other is §_-01J§ && this.unitsHit.indexOf(param1.data.other) != -1)
         {
            return;
         }
         if(disposed)
         {
            return;
         }
         var _loc2_:Number = MathUtil.distance(owner.x,owner.y,x,y);
         if(_loc2_ < PIERCING_BULLET_MIN_DAMAGE_DIST)
         {
            _loc3_ = this.damage * 0.5;
         }
         else if(_loc2_ < PIERCING_BULLET_MAX_DAMAGE_DIST)
         {
            _loc3_ = this.damage * (1 + (_loc2_ - PIERCING_BULLET_MIN_DAMAGE_DIST) / PIERCING_BULLET_DAMAGE_DIST_DIFF) * 0.5;
         }
         else
         {
            _loc3_ = this.damage;
         }
         var _loc4_:§_-01J§ = param1.data.other is §_-01J§ ? §_-01J§(param1.data.other) : null;
         var _loc5_:uint = _loc4_ != null && this.armorReductionPc > 0 ? uint(Math.round(0.01 * this.armorReductionPc * _loc4_.attributes.armor.value)) : 0;
         if(_loc4_)
         {
            if(this.damage > MIN_DAMAGE)
            {
               this.damage -= DAMAGE_REDUCTION;
            }
            this.unitsHit.push(param1.data.other);
            if(this.alwaysCrit)
            {
               _loc6_ = §_-C31§(owner.buffs.getBuff(§_-C31§.NAME));
               if(_loc6_)
               {
                  _loc6_.§_-C1m§();
               }
            }
         }
         if(_loc5_ > 0)
         {
            _loc4_.attributes.armor.§_-Y2z§(-_loc5_);
         }
         inflictDamage(param1.data.other,_loc3_,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
         if(_loc5_ > 0)
         {
            _loc4_.attributes.armor.§_-Y2z§(_loc5_);
         }
         if(param1.data.other is §_-01J§)
         {
            if(this.damage > 50)
            {
               this.damage -= 10;
            }
            this.unitsHit.push(param1.data.other);
            if(this.alwaysCrit)
            {
               _loc6_ = §_-C31§(owner.buffs.getBuff(§_-C31§.NAME));
               if(_loc6_)
               {
                  _loc6_.§_-C1m§();
               }
            }
         }
         if(this.hits++ == 20 || §_-X1j§.§_-12n§.scene.hitTestPermanent(param1.data.x,param1.data.y))
         {
            disposed = true;
            removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         }
         if(param1.data.other == null)
         {
            Effects.explodeParticles(x,y,3);
         }
      }
      
      private function reductionArmorBulletCollideFunction(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc3_:§_-73G§ = null;
         var _loc4_:uint = 0;
         if(param1.data.other is §_-01J§)
         {
            _loc2_ = §_-01J§(param1.data.other);
            _loc3_ = _loc2_.buffs.getBuff(§_-Nl§.NAME);
            _loc4_ = uint(REDUCTION_BULLET_ARMOR_REDUCE_AMOUNT.value);
            if(_loc3_ != null)
            {
               _loc4_ += _loc3_.getValue();
               _loc2_.buffs.§_-e1z§(§_-Nl§.NAME);
            }
            _loc2_.buffs.place(new §_-Nl§(_loc4_));
         }
         this.simpleBulletCollideFunction(param1);
      }
      
      private function slowSpeedBulletCollideFunction(param1:Event) : void
      {
         if(param1.data.other is §_-01J§)
         {
            §_-01J§(param1.data.other).buffs.place(new §_-M2S§(SLOW_SPEED_BULLET_DURATION_DEBUFF.value));
         }
         this.simpleBulletCollideFunction(param1);
      }
      
      private function fireBulletCollideFunction(param1:Event) : void
      {
         var _loc2_:Number = NaN;
         var _loc3_:IPooledAmmo = null;
         var _loc4_:Point = null;
         var _loc5_:int = 0;
         disposed = true;
         if(this._createEndParticles)
         {
            _loc4_ = Pool.getPoint(§_-Y2I§.x,§_-Y2I§.y);
            _loc5_ = 0;
            while(_loc5_ <= 3)
            {
               _loc2_ = -MathUtil.PI_HALF + §_-X1j§.randomSeed.§_-H3r§(-MathUtil.PI_QUARTER,MathUtil.PI_QUARTER);
               _loc3_ = ammoObjectsFactory.createObject(AmmosFactory.OBJECT_BURNING_OIL);
               _loc3_.initAmmo(AmmosFactory.BURNING_OIL_SKIN,owner,_loc4_,null,_loc2_,4);
               _loc5_++;
            }
            Pool.putPoint(_loc4_);
         }
         §_-In§ = DamageType.FIRE;
         this.inflictDamage(null,this.paramSet.damage.value,this.paramSet.damageRadius.value,this.paramSet.explosionRadius.value);
      }
      
      public function get createEndParticles() : Boolean
      {
         return this._createEndParticles;
      }
      
      public function set createEndParticles(param1:Boolean) : void
      {
         this._createEndParticles = param1;
      }
      
      public function setArmorReductionPc(param1:uint) : void
      {
         this.armorReductionPc = param1;
      }
      
      public function setAntidodge() : void
      {
         §_-ni§ = false;
         this.alwaysCrit = true;
      }
      
      public function setLifeTime(param1:uint) : void
      {
         §_-22h§ = param1;
         §_-iN§ = param1;
      }
   }
}

