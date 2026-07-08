package ru.pragmatix.wormix.weapons
{
   import §_-DJ§.§_-J2l§;
   import §_-QD§.§_-G2z§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Zd§;
   import §_-z1w§.*;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.server.GetRatingResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class BatDownGradable extends Bat implements §_-O1c§
   {
      
      public function BatDownGradable()
      {
         super();
      }
      
      override protected function §_-jr§(param1:§_-01J§) : void
      {
         if(Boolean(property["groundBreak"]) && !shotParams.§_-R18§)
         {
            this.§_-mN§(param1);
         }
      }
      
      public function §_-mN§(param1:§_-01J§, param2:uint = 35, param3:Number = 0.9) : void
      {
         var startX:int = 0;
         var startY:int = 0;
         var tmpOwner:§_-01J§ = null;
         var unit:§_-01J§ = param1;
         var maxDepth:uint = param2;
         var speedReduction:Number = param3;
         startX = unit.x;
         startY = unit.y;
         tmpOwner = owner;
         var onCollide:Function = function(param1:Event):void
         {
            var _loc5_:int = 0;
            var _loc6_:uint = 0;
            var _loc11_:Point = null;
            var _loc2_:uint = Physics.UNIT_HALF_HEIGHT;
            var _loc3_:Number = unit.speed * speedReduction;
            var _loc4_:Number = unit.angle;
            _loc5_ = int(Math.floor(§_-H3D§.distance(unit.x - startX,unit.y - startY)));
            _loc6_ = uint(0.02 * _loc5_);
            var _loc7_:uint = _loc6_ > maxDepth ? maxDepth : _loc6_;
            var _loc8_:Point = HitTest.castRayVec(unit.x,unit.y,_loc4_,_loc2_);
            if(!_loc8_)
            {
               _loc8_ = MathUtil.§_-UW§(_loc2_,_loc4_);
               _loc8_.x += unit.x;
               _loc8_.y += unit.y;
            }
            var _loc9_:Number = _loc3_ * 2;
            §_-G4§.§_-Ee§(tmpOwner,unit.x,unit.y,unit as §_-F3o§,int(_loc9_),0,0,false,true);
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,_loc8_.x,_loc8_.y,_loc2_,§_-d27§.§_-S14§);
            var _loc10_:int = 0;
            while(_loc10_ < _loc7_)
            {
               _loc11_ = MathUtil.§_-UW§(_loc10_,_loc4_);
               §_-X1j§.§_-12n§.scene.drawDamage(_loc8_.x + _loc11_.x,_loc8_.y + _loc11_.y,_loc2_);
               §_-X1j§.§_-12n§.scene.drawDamage(_loc8_.x - _loc11_.x,_loc8_.y - _loc11_.y,_loc2_);
               Pool.putPoint(_loc11_);
               _loc10_++;
            }
            unit.x = _loc8_.x;
            unit.y = _loc8_.y;
            unit.setSpeedVector(_loc4_,_loc3_);
            Pool.putPoint(_loc8_);
         };
         §_-Zd§.once(unit as EventDispatcher,§_-L3Q§.§_-o13§,onCollide);
      }
   }
}

