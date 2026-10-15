package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31Y§.§_-D3B§;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-g1P§.§_-L38§;
   import §_-hO§.SupplyPickedUpEvent;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.brokenLine.BrokenLine;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Rope extends §_-Qi§
   {
      
      public static const DEFAULT_ROPE_LENGTH:uint = RopePhysicModel.MAX_LENGTH - 50;
      
      protected static const BARREL:Point = Pool.getPoint(-15,0);
      
      private var rope:BrokenLine;
      
      protected var maxLength:§_-TA§;
      
      protected var startPoint:Point;
      
      protected var endPoint:Point;
      
      private var _disposeOnPicked:§_-L38§;
      
      private var _onPicked:Boolean;
      
      private var ropeStartPoint:Point;
      
      public function Rope(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:uint = 0, param6:Point = null, param7:Boolean = false)
      {
         super(param1,param1.x,param1.y,false,-1);
         this.maxLength = new §_-TA§(param5 == 0 ? int(DEFAULT_ROPE_LENGTH) : int(param5));
         if(param6)
         {
            this.ropeStartPoint = Pool.getPoint(param6.x,param6.y);
         }
         this.startPoint = Pool.getPoint();
         this.endPoint = Pool.getPoint();
         tags = new <§_-D3B§>[§_-D3B§.§_-f1U§];
         if(param7)
         {
            tags.push(§_-D3B§.§_-J1i§);
         }
         var _loc8_:Point = §_-fm§(param3,5,this.ropeStartPoint);
         initialize(_loc8_,param2.ammoClassName,true);
         Pool.putPoint(_loc8_);
         physParams.removeAllForces();
         setSpeedVector(param3,param4);
         §_-z1H§.rotation = param3;
         addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         §_-32§ = §_-uN§;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.SUPPLY_PICKED_UP,this.supplyPicked);
      }
      
      override protected function initPhysicModel() : void
      {
         physParams = new PhysParams(CollideType.NONE);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         physParams.optimizedMovement = true;
         §_-Y2I§ = new AmmoPhysicModel(physParams);
      }
      
      protected function onCollide(param1:Event) : void
      {
         setSpeedXY(0,0);
         §_-Y2I§.stop();
         §_-32§ = §_-8G§;
         removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         this.deleteRopeShape();
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.SUPPLY_PICKED_UP,this.supplyPicked);
         §_-h2B§.play(§_-d27§.§_-M1L§);
      }
      
      protected function onFrame(param1:Event) : void
      {
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         if(this.ropeStartPoint)
         {
            this.startPoint = this.ropeStartPoint;
         }
         else if(!owner.disposed)
         {
            _loc3_ = owner.x + -owner.direction * BARREL.x;
            _loc4_ = owner.y + BARREL.y;
            this.startPoint.setTo(_loc3_,_loc4_);
         }
         var _loc2_:Point = Pool.getPoint(x,y);
         if(owner.disposed || §_-H3D§.distance(this.startPoint.x - x,this.startPoint.y - y) >= this.maxLength.value)
         {
            removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
            disposed = true;
            §_-z1H§.visible = false;
         }
         if(!this.rope)
         {
            this.rope = new BrokenLine(4,16448250);
            §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(this.rope,SortingLayer.AMMO_LAYER);
         }
         if(!disposed)
         {
            this.rope.clear();
            this.rope.moveTo(x,y);
            this.rope.lineTo(owner.x,owner.y);
            this.endPoint.setTo(_loc2_.x,_loc2_.y);
         }
         Pool.putPoint(_loc2_);
         if(§_-z1H§)
         {
            §_-z1H§.x = this.endPoint.x;
            §_-z1H§.y = this.endPoint.y;
         }
      }
      
      private function supplyPicked(param1:SupplyPickedUpEvent) : void
      {
         if(disposed)
         {
            return;
         }
         if(!this._disposeOnPicked)
         {
            return;
         }
         if(param1.§_-i1P§ == this || param1.§_-m1G§ == this._disposeOnPicked)
         {
            this._onPicked = true;
            disposed = true;
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
         this.deleteRopeShape();
         super.defaultOnDisposed(param1);
      }
      
      protected function deleteRopeShape() : void
      {
         if(this.rope == null)
         {
            return;
         }
         §_-X1j§.§_-12n§.scene.removeElement(this.rope);
         this.rope.dispose();
         this.rope = null;
      }
      
      public function set disposeOnPicked(param1:§_-L38§) : void
      {
         this._disposeOnPicked = param1;
      }
      
      public function get onPicked() : Boolean
      {
         return this._onPicked;
      }
   }
}

