package ru.pragmatix.wormix.weapons.ammo.ammoFactory
{
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   
   public class AmmosFactory
   {
      
      private static var instance:AmmosFactory;
      
      private static const OBJECTS_TOTAL:int = 2;
      
      public static const OBJECT_BULLET:int = 0;
      
      public static const OBJECT_BURNING_OIL:int = 1;
      
      public static const BULLET_SIMPLE:int = 0;
      
      public static const BULLET_KALASH:int = 1;
      
      public static const BULLET_PIERCING:int = 2;
      
      public static const BULLET_REDUCTING:int = 3;
      
      public static const BULLET_SLOWING:int = 4;
      
      public static const BULLET_FIRE:int = 5;
      
      public static const BURNING_OIL_SKIN:int = 0;
      
      private var cache:Array;
      
      private var availableParticleIndexes:Array;
      
      public function AmmosFactory()
      {
         super();
         this.clearAll();
         instance = this;
      }
      
      public static function getInstance() : AmmosFactory
      {
         if(instance == null)
         {
            return new AmmosFactory();
         }
         return instance;
      }
      
      public function createObject(param1:int) : IPooledAmmo
      {
         var _loc3_:IPooledAmmo = null;
         var _loc2_:int = int(this.availableParticleIndexes[param1]);
         if(_loc2_ == -1)
         {
            switch(param1)
            {
               case OBJECT_BULLET:
                  _loc3_ = new Bullet();
                  break;
               case OBJECT_BURNING_OIL:
                  _loc3_ = new BurningOil();
            }
            this.cache[param1].push(_loc3_);
         }
         else
         {
            this.availableParticleIndexes[param1] = _loc2_ - 1;
            _loc3_ = this.cache[param1][_loc2_];
         }
         return _loc3_;
      }
      
      public function removeObject(param1:IPooledAmmo, param2:int) : void
      {
         this.cache[param2][this.availableParticleIndexes[param2] + 1] = param1;
         this.availableParticleIndexes[param2] += 1;
      }
      
      public function clearAll() : void
      {
         this.cache = [];
         this.availableParticleIndexes = [];
         var _loc1_:uint = 0;
         while(_loc1_ < OBJECTS_TOTAL)
         {
            this.cache.push([]);
            this.availableParticleIndexes.push(-1);
            _loc1_++;
         }
      }
      
      public function createPoolOfObjects(param1:int, param2:int = 100) : void
      {
         var _loc3_:* = param2;
         while(_loc3_--)
         {
            this.createObject(param1);
         }
         this.availableParticleIndexes[param1] = param2 - 1;
      }
   }
}

