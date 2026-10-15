package ru.pragmatix.wormix.physics.model
{
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-X1i§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   
   public class AmmoPhysicModel extends GenericPhysicModel
   {
      
      private var ignoredInitialized:Boolean = false;
      
      private var ignoredUnits:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
      
      private var collisionPretenders:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
      
      private var ignoreEnemiesOnSpawn:Boolean = true;
      
      private var stopOnCollisionProcess:Boolean = true;
      
      public function AmmoPhysicModel(param1:PhysParams)
      {
         super(param1);
      }
      
      public function set stopOnCollision(param1:Boolean) : void
      {
         this.stopOnCollisionProcess = param1;
      }
      
      public function checkEnemiesOnStart() : void
      {
         this.ignoreEnemiesOnSpawn = false;
      }
      
      public function addToIgnored(param1:§_-F3o§) : void
      {
         this.ignoredUnits.push(param1);
      }
      
      override public function getHitObjects(param1:Array) : void
      {
         §_-F2L§.§_-11m§(this.collisionPretenders,this.checkHitObject,param1);
      }
      
      private function checkHitObject(param1:§_-F3o§, param2:Array) : Boolean
      {
         if(this.ignoredUnits.indexOf(param1) == -1 && this != param1.§_-Y2I§ && hitTestObject(param1))
         {
            param2.push(param1);
            this.ignoredUnits.push(param1);
         }
         return false;
      }
      
      private function setCollisionPretenders() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:§_-X1i§ = null;
         this.collisionPretenders.splice(0,this.collisionPretenders.length);
         if(physParams.collidesWith != PhysParams.COLLIDES_WITH_NOTHING && physParams.collidesWith != PhysParams.COLLIDES_WITH_GROUND)
         {
            if(speedX > 0)
            {
               _loc1_ = x;
               _loc2_ = x + speedX;
            }
            else
            {
               _loc2_ = x;
               _loc1_ = x + speedX;
            }
            if(speedY > 0)
            {
               _loc3_ = y;
               _loc4_ = y + speedY;
            }
            else
            {
               _loc4_ = y;
               _loc3_ = y + speedY;
            }
            _loc5_ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache;
            if(physParams.collidesWithUnit)
            {
               this.collisionPretenders = this.collisionPretenders.concat(Physics.getGameObjectsInZone(_loc1_ - Physics.UNIT_HALF_WIDTH,_loc3_ - Physics.UNIT_HALF_HEIGHT,_loc2_ + Physics.UNIT_HALF_WIDTH,_loc4_ + Physics.UNIT_HALF_HEIGHT,_loc5_.§_-434§(CollideType.UNIT)));
            }
            if(physParams.collidesWithAmmo)
            {
               this.collisionPretenders = this.collisionPretenders.concat(Physics.getGameObjectsInZone(_loc1_,_loc3_,_loc2_,_loc4_,_loc5_.§_-434§(CollideType.AMMO)));
            }
            this.collisionPretenders = this.collisionPretenders.concat(Physics.getGameObjectsInZone(_loc1_,_loc3_,_loc2_,_loc4_,_loc5_.§_-434§(CollideType.OTHER)));
         }
      }
      
      override public function nextState() : void
      {
         var _loc1_:§_-F3o§ = null;
         if(!this.ignoredInitialized)
         {
            this.initIgnored();
            this.ignoredInitialized = true;
         }
         var _loc2_:int = int(this.ignoredUnits.length);
         while(--_loc2_ > -1)
         {
            _loc1_ = this.ignoredUnits[_loc2_];
            if(!hitTestObject(_loc1_))
            {
               this.ignoredUnits.splice(_loc2_,1);
            }
         }
         this.setCollisionPretenders();
         super.nextState();
      }
      
      private function initIgnored() : void
      {
         if(this.ignoreEnemiesOnSpawn)
         {
            §_-F2L§.§_-11m§(§_-X1j§.§_-12n§.gameMaster.§_-D3n§(),this.checkAndAddToIgnored);
         }
      }
      
      private function checkAndAddToIgnored(param1:§_-01J§) : Boolean
      {
         if(hitTestObject(param1 as §_-F3o§))
         {
            this.ignoredUnits.push(param1);
         }
         return false;
      }
      
      public function isObjectIgnored(param1:§_-F3o§) : Boolean
      {
         return this.ignoredUnits.indexOf(param1) != -1;
      }
      
      override public function hitTestGround(param1:int, param2:int) : Boolean
      {
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         var _loc7_:int = 0;
         var _loc3_:uint = 0;
         if(_loc3_ == 0)
         {
            return §_-X1j§.§_-12n§.scene.hitTestPoint(x,y);
         }
         var _loc4_:Boolean = false;
         if(physParams.collidesWithGround)
         {
            _loc5_ = Math.cos(angle + MathUtil.PI_HALF);
            _loc6_ = Math.sin(angle + MathUtil.PI_HALF);
            _loc7_ = -_loc3_;
            while(_loc7_ <= _loc3_)
            {
               if(§_-X1j§.§_-12n§.scene.hitTestPoint(x + _loc7_ * _loc5_,y + _loc7_ * _loc6_))
               {
                  _loc4_ = true;
                  break;
               }
               _loc7_++;
            }
         }
         return _loc4_;
      }
      
      override public function processCollision(param1:§_-F3o§, param2:int, param3:int, param4:Object) : void
      {
         if(!physParams.ignoresTerrain)
         {
            x = param2;
            y = param3;
            if(this.stopOnCollisionProcess && Boolean(param4.stopObject))
            {
               stop();
            }
         }
      }
      
      override public function clear() : void
      {
         this.ignoredUnits = null;
         this.collisionPretenders = null;
      }
      
      public function setDefault() : void
      {
         this.ignoredUnits.splice(0,this.ignoredUnits.length);
         this.collisionPretenders.splice(0,this.collisionPretenders.length);
         this.ignoredInitialized = false;
         this.ignoreEnemiesOnSpawn = true;
         this.stopOnCollisionProcess = true;
      }
   }
}

