package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import flash.geom.Point;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.brokenLine.BrokenLine;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-z1x§;
   import ru.pragmatix.wormix.weapons.ammo.dataStructures.LaserPair;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class LaserLineAmmoController extends §_-F3o§ implements §_-z1x§
   {
      
      private static var instance:LaserLineAmmoController;
      
      private static var laserPairsVector:Vector.<LaserPair>;
      
      private static var brokenLine:BrokenLine;
      
      private static var damageAmmo:§_-Qi§;
      
      private static var unitPrevPositions:Dictionary;
      
      private static const CHECK_ENEMY_STEPS_DELAY:int = 1;
      
      private static var isDisposed:Boolean = true;
      
      private static var curStep:int = 0;
      
      private static var firstPoint:Point = Pool.getPoint();
      
      private static var secondPoint:Point = Pool.getPoint();
      
      private var needToDraw:Boolean;
      
      public function LaserLineAmmoController()
      {
         super();
         unitPrevPositions = new Dictionary();
         laserPairsVector = new Vector.<LaserPair>(0);
         damageAmmo = new §_-Qi§(null,0,0,false,1);
         §_-z1H§ = new Sprite();
         _type = ObjectType.BACK_ENVIRONMENT;
         §_-j2P§ = SortingLayer.AMMO_LAYER;
         brokenLine = new BrokenLine(3,0);
         (§_-z1H§ as Sprite).addChild(brokenLine);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         isDisposed = false;
      }
      
      public static function getInstance() : LaserLineAmmoController
      {
         if(!instance || isDisposed)
         {
            instance = new LaserLineAmmoController();
         }
         return instance;
      }
      
      public static function isLaserAmmo(param1:§_-Qi§) : Boolean
      {
         return param1 == damageAmmo;
      }
      
      override public function set y(param1:int) : void
      {
         super.y = 0;
         this.draw();
      }
      
      public function addPair(param1:§_-01J§, param2:§_-C3e§, param3:§_-F3o§, param4:§_-F3o§, param5:uint = 0, param6:Boolean = false, param7:Boolean = true) : void
      {
         laserPairsVector.push(new LaserPair(param1,param2,param3,param4,param5,param7));
         laserPairsVector.sort(this.sortLaserPairFunction);
         curStep = 0;
         if(param6)
         {
            this.draw();
         }
      }
      
      public function getAllLaserPairs() : Vector.<LaserPair>
      {
         return laserPairsVector;
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(curStep++ >= CHECK_ENEMY_STEPS_DELAY)
         {
            curStep = 0;
            this.checkEnemy();
         }
      }
      
      public function draw() : void
      {
         var _loc2_:LaserPair = null;
         var _loc3_:§_-F3o§ = null;
         var _loc4_:§_-F3o§ = null;
         brokenLine.beginDraw();
         var _loc1_:* = int(laserPairsVector.length);
         while(_loc1_--)
         {
            _loc2_ = laserPairsVector[_loc1_];
            if(_loc2_.isActive())
            {
               _loc3_ = _loc2_.firstActivator;
               _loc4_ = _loc2_.secondActivator;
               brokenLine.setColor(_loc2_.color);
               firstPoint.setTo(_loc3_.x,_loc3_.y);
               secondPoint.setTo(_loc4_.x,_loc4_.y);
               brokenLine.drawLine(firstPoint,secondPoint);
            }
            else
            {
               laserPairsVector.splice(_loc1_,1);
               _loc2_.destroy();
            }
         }
         brokenLine.endDraw();
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         var _loc2_:Point = null;
         isDisposed = true;
         (§_-z1H§ as Sprite).removeChild(brokenLine);
         brokenLine.clear();
         brokenLine.dispose();
         for each(_loc2_ in unitPrevPositions)
         {
            Pool.putPoint(_loc2_);
            _loc2_ = null;
         }
         unitPrevPositions = null;
         super.defaultOnDisposed(param1);
      }
      
      private function checkEnemy() : void
      {
         this.needToDraw = false;
         §_-X1j§.§_-12n§.gameMaster.§_-13w§(this);
         if(this.needToDraw)
         {
            this.draw();
         }
      }
      
      public function processUnit(param1:§_-01J§) : Boolean
      {
         var _loc4_:LaserPair = null;
         var _loc5_:Point = null;
         var _loc6_:Point = null;
         var _loc8_:Boolean = false;
         var _loc9_:Number = NaN;
         var _loc10_:§_-C3e§ = null;
         var _loc2_:Boolean = false;
         var _loc3_:* = int(laserPairsVector.length);
         var _loc7_:Point = unitPrevPositions[param1.getId()];
         if(_loc7_ == null)
         {
            unitPrevPositions[param1.getId()] = _loc7_ = Pool.getPoint(param1.x,param1.y);
         }
         while(_loc3_--)
         {
            _loc4_ = laserPairsVector[_loc3_];
            _loc4_.updateActivatorsPosition();
            _loc5_ = _loc4_.firstActivatorPos;
            _loc6_ = _loc4_.secondActivatorPos;
            if(_loc4_.isActive() && _loc4_.owner.squad.getTeamId() != param1.squad.getTeamId())
            {
               _loc8_ = MathUtil.§_-G1o§(_loc5_.x,_loc5_.y,_loc6_.x,_loc6_.y,param1.x,param1.y,30);
               if(!_loc8_)
               {
                  _loc8_ = MathUtil.§_-O1r§(_loc5_.x,_loc5_.y,_loc6_.x,_loc6_.y,_loc7_.x,_loc7_.y,param1.x,param1.y);
                  if(_loc8_)
                  {
                     _loc9_ = MathUtil.distance(_loc7_.x,_loc7_.y,param1.x,param1.y);
                     _loc8_ = _loc9_ * 0.45 <= param1.speed;
                  }
               }
               if(_loc8_)
               {
                  laserPairsVector.splice(_loc3_,1);
                  this.explodeLaserLine(_loc4_.owner,_loc5_.x,_loc5_.y,_loc6_.x,_loc6_.y);
                  damageAmmo.owner = _loc4_.owner;
                  _loc10_ = _loc4_.paramSet;
                  §_-G4§.inflictDamage(damageAmmo,param1 as §_-F3o§,_loc10_.damage.value,_loc10_.damageRadius.value,_loc10_.explosionRadius.value,true,true);
                  Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,param1.x,param1.y,30,§_-d27§.§_-a2n§);
                  _loc4_.destroy();
                  _loc4_ = null;
                  this.needToDraw = true;
                  _loc2_ = true;
                  break;
               }
            }
            else if(_loc4_.isDamaged())
            {
               laserPairsVector.splice(_loc3_,1);
               _loc4_.destroy();
               _loc4_ = null;
               this.needToDraw = true;
               break;
            }
         }
         _loc7_.setTo(param1.x,param1.y);
         return _loc2_;
      }
      
      private function explodeLaserLine(param1:§_-01J§, param2:Number, param3:Number, param4:Number, param5:Number) : void
      {
         var _loc6_:Number = MathUtil.distance(param2,param3,param4,param5);
         var _loc7_:Number = 0;
         var _loc8_:Number = MathUtil.§_-Ih§(param2 + _loc6_,param3,param4,param5);
         var _loc9_:Number = Math.acos(MathUtil.§_-T1l§(1 - _loc8_ / (2 * _loc6_ * _loc6_),100000));
         if(param2 < param4)
         {
            _loc7_ = param3 < param5 ? _loc9_ : -_loc9_;
         }
         else
         {
            _loc7_ = param3 < param5 ? _loc9_ : MathUtil.§_-M2N§ - _loc9_;
         }
         var _loc10_:int = 10;
         var _loc11_:Number = 3 * Math.sin(_loc7_);
         var _loc12_:Number = -3 / Math.cos(_loc7_);
         var _loc13_:int = 0;
         while(_loc13_ < _loc10_)
         {
            §_-X1j§.§_-12n§.scene.drawRotatedRectDamage(param2 + (param4 - param2) * 0.1 * _loc13_ + _loc11_,param3 + (param5 - param3) * 0.1 * _loc13_ + _loc12_,_loc6_ * 0.1 + 3,6,_loc7_);
            _loc13_++;
         }
      }
      
      private function sortLaserPairFunction(param1:LaserPair, param2:LaserPair) : int
      {
         if(param1.owner == param2.owner)
         {
            return 0;
         }
         return -1;
      }
   }
}

