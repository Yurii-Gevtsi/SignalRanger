package ru.pragmatix.wormix.fx
{
   import ru.pragmatix.wormix.fx.trail.VertexTrail;
   
   public class EffectType
   {
      
      public static const EXPLOSION_SMALL:EffectType = new EffectType("EXPLOSION_SMALL",null);
      
      public static const EXPLOSION_MEDIUM:EffectType = new EffectType("EXPLOSION_MEDIUM",null);
      
      public static const EXPLOSION_LARGE:EffectType = new EffectType("EXPLOSION_LARGE",null);
      
      public static const EXPLOSION_DRILL:EffectType = new EffectType("EXPLOSION_DRILL",null);
      
      public static const EXPLOSION_BULLET:EffectType = new EffectType("EXPLOSION_BULLET",null);
      
      public static const EXPLOSION_ELECTRIC:EffectType = new EffectType("EXPLOSION_ELECTRIC",null);
      
      public static const FIRE:EffectType = new EffectType("FIRE",DragonbonesAnimationEffect,"ElectricExplosion",65);
      
      public static const EXPLOSION_FIRE_END:EffectType = new EffectType("EXPLOSION_FIRE_END",DragonbonesAnimationEffect,"ElectricExplosion",65);
      
      public static const EXPLOSION_TUTORIAL:EffectType = new EffectType("EXPLOSION_TUTORIAL",DragonbonesAnimationEffect,"LearningMissionMarkExplosion",100);
      
      public static const EXPLOSION_ELECTRIC_SMALL:EffectType = new EffectType("EXPLOSION_ELECTRIC_SMALL",null);
      
      public static const EXPLOSION_POISON_SMALL:EffectType = new EffectType("EXPLOSION_POISON_SMALL",DragonbonesAnimationEffect,"PoisonExplosionSmall",100);
      
      public static const EXPLOSION_POISON_LARGE:EffectType = new EffectType("EXPLOSION_POISON_LARGE",DragonbonesAnimationEffect,"PoisonExplosionLarge",400);
      
      public static const EXPLOSION_SUPPLY:EffectType = new EffectType("EXPLOSION_SUPPLY",DragonbonesAnimationEffect,"AppearAnimation",200);
      
      public static const EXPLOSION_SMOKE:EffectType = new EffectType("EXPLOSION_SMOKE",null);
      
      public static const EXPLOSION_DEMONIC:EffectType = new EffectType("EXPLOSION_DEMONIC",null);
      
      public static const TRAJECTORY_TRAIL:EffectType = new EffectType("TRAJECTORY_TRAIL",VertexTrail);
      
      public static const TEXT:EffectType = new EffectType("TEXT",TextEffect);
      
      public static const DRAGON_FIRE_STEP:EffectType = new EffectType("DRAGON_FIRE_STEP",TextEffect);
      
      public static const UNIT_TRAIL:EffectType = new EffectType("UNIT_TRAIL",null);
      
      protected static var _effectObjectPool:EffectsPool = EffectsPool.getInstance();
      
      private var _name:String;
      
      private var _effectClass:Class;
      
      private var _args:Array;
      
      public function EffectType(param1:String, param2:Class, ... rest)
      {
         super();
         this._name = param1;
         this._effectClass = param2;
         this._args = rest;
      }
      
      public function get name() : String
      {
         return this._name;
      }
      
      public function get args() : Array
      {
         return this._args;
      }
      
      public function createInstance() : IPooledEffect
      {
         return new this._effectClass(this);
      }
      
      public function getObjectPool() : EffectsPool
      {
         return _effectObjectPool;
      }
   }
}

