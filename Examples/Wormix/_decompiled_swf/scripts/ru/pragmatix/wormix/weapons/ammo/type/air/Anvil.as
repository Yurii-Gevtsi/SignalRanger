package ru.pragmatix.wormix.weapons.ammo.type.air
{
   import §_-31Y§.§_-D3B§;
   import §_-31u§.§_-Hb§;
   import §_-31u§.§_-l1n§;
   import §_-63U§.§_-412§;
   import §_-63U§.§_-NV§;
   import §_-63U§.§_-c1e§;
   import §_-A2F§.§_-4B§;
   import §_-K3q§.§_-L3A§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-fo§.§_-r5§;
   import §_-g1P§.§_-L38§;
   import §_-w2i§.§_-Ia§;
   import flash.display.BitmapData;
   import flash.display.MovieClip;
   import flash.geom.Matrix;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-A16§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.§_-5e§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.boss.emperor.EmperorTerracottaWarrior;
   import ru.pragmatix.wormix.weapons.ammo.boss.scientist.ScientistGenerator;
   import ru.pragmatix.wormix.weapons.ammo.boss.symbiote.SymbioteCocoon;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AbstractBlocker;
   import starling.utils.Pool;
   
   public class Anvil extends §_-Qi§
   {
      
      private static const STOP_ON_BOTTOM_SURFACE_COLLISION_RATE:Number = 0.5;
      
      private static const DISPOSE_ON_COLLIDE:Array = [§_-Hb§,§_-l1n§,AbstractBlocker,§_-L38§,§_-412§,SymbioteCocoon,§_-L3A§];
      
      private static const PUSH_ON_COLLIDE:Array = [Mine,§_-01J§,§_-A16§,§_-5e§,§_-NV§,EmperorTerracottaWarrior];
      
      private static const EXPLODE_ON_COLLIDE:Array = [ScientistGenerator,§_-c1e§];
      
      protected var paramSet:§_-C3e§;
      
      protected var points:Vector.<Point> = new Vector.<Point>();
      
      protected var lastPos:Point;
      
      protected var anvil:MovieClip;
      
      public function Anvil(param1:§_-01J§, param2:§_-C3e§, param3:int, param4:int)
      {
         super(param1,param3,param4,false,§_-Qi§.§_-n18§,DamageType.AIR);
         initialize(Pool.getPoint(param3,param4),param2.ammoClassName);
         tags.push(§_-D3B§.§_-f1U§);
         this.paramSet = param2;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_NOTHING;
         §_-22o§();
         §_-92S§ = false;
         this.createMovieClip();
         this.setBottomCollisionPoints();
         setSpeedXY(0,20);
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      private function createMovieClip() : void
      {
         this.anvil = §_-r5§.§_-tF§("AmAnvil");
         this.anvil.width = width;
         this.anvil.height = height;
         this.anvil.rotation = clip.rotation * 180 / Math.PI;
      }
      
      override public function nextState() : void
      {
         super.nextState();
         this.clearPath();
         this.checkObjectsCollisions();
      }
      
      protected function clearPath() : void
      {
         var _loc2_:Number = NaN;
         var _loc3_:Number = NaN;
         var _loc1_:Point = this.shiftedRegPoint;
         if(this.lastPos)
         {
            _loc2_ = _loc1_.x + width;
            _loc3_ = _loc1_.y + 3;
            §_-X1j§.§_-12n§.scene.drawRectangleDamage(this.lastPos.x,this.lastPos.y,_loc2_,_loc3_);
            §_-4B§.§_-uz§(this.lastPos.x,this.lastPos.y,_loc2_,_loc3_);
         }
         this.lastPos = _loc1_;
      }
      
      private function countSurfaceCollisions(param1:Point) : int
      {
         var _loc3_:Point = null;
         var _loc4_:Point = null;
         var _loc2_:int = 0;
         for each(_loc3_ in this.points)
         {
            _loc4_ = Pool.getPoint(param1.x + _loc3_.x,param1.y + _loc3_.y + 1);
            if(HitTest.point(_loc4_.x,_loc4_.y,true))
            {
               while(HitTest.point(_loc4_.x,_loc4_.y,true))
               {
                  --_loc4_.y;
                  --y;
               }
               return this.points.length;
            }
            if(HitTest.point(_loc4_.x,_loc4_.y))
            {
               _loc2_++;
            }
         }
         return _loc2_;
      }
      
      private function checkObjectsCollisions() : void
      {
         var _loc7_:§_-F3o§ = null;
         var _loc8_:Point = null;
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc12_:int = 0;
         var _loc1_:Boolean = this.isStopped;
         var _loc2_:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§(PUSH_ON_COLLIDE);
         var _loc3_:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§(DISPOSE_ON_COLLIDE);
         var _loc4_:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§(EXPLODE_ON_COLLIDE);
         var _loc5_:Point = this.shiftedRegPoint;
         var _loc6_:Point = Pool.getPoint(x + (width >> 1),y + (height >> 1));
         if(Physics.getGameObjectsInZone(_loc5_.x,_loc5_.y,_loc6_.x,_loc6_.y,_loc4_).length > 0)
         {
            §_-G4§.inflictDamage(this,null,this.paramSet.damage.value,width + 10,width);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_MEDIUM,x,y,3 * (width >> 1));
            disposed = true;
            return;
         }
         for each(_loc7_ in Physics.getGameObjectsInZone(_loc5_.x,_loc5_.y,_loc6_.x,_loc6_.y + (_loc1_ ? 10 : 0),_loc2_))
         {
            for each(_loc8_ in this.points)
            {
               if(_loc5_.x + _loc8_.x == _loc7_.x)
               {
                  _loc9_ = (_loc7_.width >> 1) * _loc7_.clip.scaleX;
                  _loc10_ = (_loc7_.height >> 1) * _loc7_.clip.scaleY;
                  _loc11_ = _loc10_ >> 1;
                  _loc12_ = _loc9_ >> 1;
                  _loc7_.y = _loc5_.y + _loc8_.y + (_loc1_ ? _loc10_ : speedY + _loc10_) + 3;
                  if(_loc7_.x + _loc12_ > _loc6_.x)
                  {
                     _loc7_.x = _loc6_.x - _loc12_;
                  }
                  if(_loc7_.x - _loc12_ < _loc5_.x)
                  {
                     _loc7_.x = _loc5_.x + _loc12_;
                  }
                  while(HitTest.gameObject(_loc7_.x,_loc7_.y,_loc10_,true))
                  {
                     --_loc7_.y;
                  }
                  _loc7_.addSpeedXY(0,1);
                  if(_loc1_)
                  {
                     §_-X1j§.§_-12n§.scene.drawRectangleDamage(_loc7_.x - _loc11_ - 3,_loc5_.y,_loc7_.x + _loc11_ + 3,_loc7_.y + _loc11_);
                     §_-X1j§.§_-12n§.scene.drawDamage(_loc7_.x,_loc7_.y + _loc11_,_loc11_);
                     if(_loc7_.§_-Nt§)
                     {
                        §_-G4§.inflictDamage(this,_loc7_,this.paramSet.damage.value,0,0);
                     }
                     if(!HitTest.gameObject(_loc7_.x,_loc7_.y + _loc10_,_loc10_))
                     {
                        _loc7_.setSpeedXY(0,10);
                     }
                  }
                  break;
               }
            }
         }
         for each(_loc7_ in Physics.getGameObjectsInZone(_loc5_.x,_loc5_.y,_loc6_.x,_loc6_.y,_loc3_))
         {
            §_-G4§.inflictDamage(this,_loc7_,this.paramSet.damage.value * 2,0,0);
            if(!_loc7_.disposed)
            {
               _loc7_.disposed = true;
               Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,_loc7_.x,_loc7_.y,3 * (_loc7_.width >> 1));
            }
         }
         if(_loc1_)
         {
            this.stop();
         }
      }
      
      protected function get isStopped() : Boolean
      {
         var _loc1_:uint = uint(this.countSurfaceCollisions(this.shiftedRegPoint));
         return _loc1_ / this.points.length > STOP_ON_BOTTOM_SURFACE_COLLISION_RATE;
      }
      
      private function stop() : void
      {
         var dummy:§_-F3o§ = null;
         var anvil_clip:MovieClip = null;
         var anvil_x:int = 0;
         var anvil_y:int = 0;
         var anvil_width:int = 0;
         var anvil_heighr:int = 0;
         var anvil_rotation:Number = NaN;
         setSpeedXY(0,0);
         stable = true;
         disposed = true;
         dummy = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
         anvil_clip = this.anvil;
         anvil_x = x;
         anvil_y = y;
         anvil_width = width;
         anvil_heighr = height;
         anvil_rotation = MathUtil.§_-f1V§(clip.rotation);
         §_-Ia§.§_-33o§(2,function():void
         {
            anvil_clip.width = anvil_width;
            anvil_clip.height = anvil_heighr;
            anvil_clip.rotation = anvil_rotation;
            §_-X1j§.§_-12n§.scene.addClipToGround(anvil_clip,anvil_x,anvil_y);
            dummy.disposed = true;
         });
      }
      
      protected function get shiftedRegPoint() : Point
      {
         return Pool.getPoint(x - (width >> 1),y - (height >> 1));
      }
      
      private function setBottomCollisionPoints() : void
      {
         var _loc3_:int = 0;
         var _loc1_:BitmapData = new BitmapData(width,height,true,0);
         _loc1_.draw(this.anvil,new Matrix(1,0,0,1,width >> 1,height >> 1));
         var _loc2_:int = 0;
         while(_loc2_ < width)
         {
            _loc3_ = 0;
            while(_loc3_ < height)
            {
               if(_loc1_.getPixel(_loc2_,_loc3_) != 0 && _loc1_.getPixel(_loc2_,_loc3_ + 1) == 0)
               {
                  this.points.push(Pool.getPoint(_loc2_,_loc3_));
               }
               _loc3_++;
            }
            _loc2_++;
         }
      }
   }
}

