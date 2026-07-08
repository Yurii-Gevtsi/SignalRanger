package ru.pragmatix.wormix.weapons
{
   import §_-5Y§.§_-h2c§;
   import §_-63U§.§_-21K§;
   import §_-G2H§.*;
   import §_-g1P§.§_-L38§;
   import §_-g1P§.§_-z1n§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import feathers.core.FeathersControl;
   import flash.geom.Point;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.sticky.ShardMine;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class GrabberGun extends GravityGun implements §_-O1c§
   {
      
      private static const §_-K1i§:uint = 350;
      
      private static const SEARCH_ANGLE:Number = Math.PI / 12;
      
      private static const §_-B1x§:uint = 40;
      
      private static const §_-aE§:uint = 50;
      
      private static const §_-6d§:Array = [Mine,§_-5e§,ShardMine,§_-L38§];
      
      private var canCarry:Boolean;
      
      private var cargo:§_-F3o§;
      
      private var carrying:Boolean = false;
      
      private var §_-x1J§:int;
      
      private var §_-8k§:Boolean;
      
      public function GrabberGun()
      {
         super();
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         this.§_-8k§ = Boolean(param1["attraction"]) && Boolean(param1["attraction"].value);
         super.§_-z2Y§(param1);
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
         this.canCarry = this.§_-8k§;
         this.carrying = false;
      }
      
      override protected function shoot() : void
      {
         if(this.carrying && !this.cargo.disposed)
         {
            if(this.cargo is §_-z1n§)
            {
               §_-z1n§(this.cargo).setExplosive(true);
            }
            §_-G1Z§();
            this.cargo.addSpeedVector(shotParams.angle,MAX_POWER);
            this.§_-g2m§();
            §_-h2B§.play(§_-d27§.§_-Lp§);
         }
         else if(this.canCarry && shotParams.§_-R18§ && this.§_-Nf§(shotParams.angle))
         {
            this.canCarry = false;
            §_-h2B§.play(§_-d27§.§_-zr§);
         }
         else
         {
            §_-13v§();
            this.canCarry = false;
         }
         §_-5s§();
      }
      
      private function §_-Nf§(param1:Number) : Boolean
      {
         var _loc2_:§_-W2f§ = null;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:Vector.<§_-F3o§> = null;
         var _loc7_:§_-F3o§ = null;
         var _loc8_:Number = NaN;
         var _loc9_:int = 0;
         _loc2_ = §_-W2f§(§_-G3H§());
         _loc3_ = _loc2_.baseX;
         _loc4_ = _loc2_.baseY;
         _loc5_ = Physics.getGameObjectsInCone(_loc3_,_loc4_,§_-K1i§,param1,SEARCH_ANGLE,§_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§(§_-6d§));
         var _loc6_:Vector.<§_-F3o§> = Physics.sortByDistanceToPoint(_loc3_,_loc4_,§_-F2L§.§_-n2u§(_loc5_));
         for each(_loc7_ in _loc6_)
         {
            _loc8_ = Math.atan2(_loc7_.y - _loc4_,_loc7_.x - _loc3_);
            _loc9_ = §_-H3D§.distance(_loc7_.x - _loc3_,_loc7_.y - _loc4_);
            if(!HitTest.castRayVec(_loc3_,_loc4_,_loc8_,_loc9_ - 5))
            {
               this.§_-p§(_loc7_);
               return true;
            }
         }
         return false;
      }
      
      private function §_-p§(param1:§_-F3o§) : void
      {
         this.cargo = param1;
         this.§_-g2m§();
         var _loc2_:§_-W2f§ = §_-W2f§(§_-G3H§());
         new §_-21K§(_loc2_.baseX,_loc2_.baseY,§_-B1x§ * 2,this.cargo,this.§_-Ps§);
         this.cargo.setSpeedVector(_loc2_.getAngle(),-§_-aE§);
      }
      
      private function §_-Ps§(param1:§_-21K§) : void
      {
         if(Boolean(this.cargo) && Boolean(this.cargo.clip))
         {
            this.§_-L2V§();
            §_-K2b§();
         }
      }
      
      override protected function onFrame(param1:Event) : void
      {
         this.§_-L2V§();
      }
      
      private function §_-L2V§() : void
      {
         var _loc1_:§_-W2f§ = null;
         var _loc2_:Number = NaN;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         if(Boolean(this.cargo) && !this.cargo.disposed)
         {
            _loc1_ = §_-W2f§(§_-G3H§());
            _loc2_ = _loc1_.getAngle();
            _loc3_ = isActive() ? int(_loc1_.baseX + int(§_-B1x§ * Math.cos(_loc2_))) : int(owner.x + §_-B1x§ * owner.direction * Math.cos(_loc2_));
            _loc4_ = isActive() ? int(_loc1_.baseY + int(§_-B1x§ * Math.sin(_loc2_))) : owner.y;
            this.cargo.clip.scaleY = Math.abs(this.cargo.clip.scaleY) * clip.scaleY;
            this.cargo.x = _loc3_;
            this.cargo.y = _loc4_;
            this.cargo.clip.rotation = clip.rotation;
            this.cargo.setSpeedXY(0,0);
            this.cargo.stable = false;
            this.cargo.clip.visible = isActive();
         }
      }
      
      private function §_-g2m§() : void
      {
         var _loc1_:Point = null;
         this.carrying = !this.carrying;
         if(!this.cargo.disposed)
         {
            this.cargo.disabled = this.carrying;
            if(this.carrying)
            {
               §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-42N§(this.cargo);
            }
            else
            {
               §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-d2E§(this.cargo);
            }
            if(this.carrying)
            {
               this.§_-x1J§ = this.cargo.physParams.collidesWith;
               this.cargo.physParams.collidesWith = CollideType.NONE;
            }
            else
            {
               this.§_-w2V§();
               this.cargo.physParams.collidesWith = this.§_-x1J§;
               if(!owner.disposed)
               {
                  this.cargo.x = owner.x;
                  this.cargo.y = owner.y;
               }
               if(HitTest.point(this.cargo.x,this.cargo.y))
               {
                  _loc1_ = Pool.getPoint();
                  if(Physics.nearestFreePoint(this.cargo.x,this.cargo.y,HitTest.point,100,_loc1_))
                  {
                     this.cargo.x = _loc1_.x;
                     this.cargo.y = _loc1_.y;
                  }
                  Pool.putPoint(_loc1_);
               }
            }
         }
      }
      
      private function §_-w2V§() : void
      {
         this.cargo.clip.rotation = 0;
         this.cargo.clip.scaleY = Math.abs(this.cargo.clip.scaleY);
      }
      
      override public function detach() : void
      {
         if(Boolean(this.carrying) && Boolean(this.cargo) && !this.cargo.disposed)
         {
            §_-G1Z§();
            this.§_-g2m§();
            this.cargo.clip.rotation = 0;
            this.cargo.clip.visible = true;
         }
         this.cargo = null;
         super.detach();
      }
   }
}

