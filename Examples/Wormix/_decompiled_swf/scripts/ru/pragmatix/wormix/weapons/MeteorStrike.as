package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-K4§;
   import §_-A1K§.§_-TA§;
   import §_-H2g§.§_-di§;
   import §_-b1F§.§_-G4§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-u1m§.§_-61A§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.Meteor;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   
   public class MeteorStrike extends §_-61A§
   {
      
      protected static var g:Number = Force.GRAVITY.ay;
      
      public static const POWER:Number = 50;
      
      protected static const §_-93O§:int = 20;
      
      protected var shards:uint = 4;
      
      protected var §_-U2g§:uint = 1;
      
      protected var frames:§_-TA§ = new §_-TA§(0);
      
      protected var unstableObject:§_-F3o§;
      
      protected var §_-yz§:uint;
      
      public function MeteorStrike()
      {
         super(true,false);
         §_-Y2U§ = new §_-K4§();
      }
      
      public static function §_-E2D§(param1:int, param2:int, param3:int, param4:int = -100) : Object
      {
         var _loc5_:Number = §_-X1j§.randomSeed.§_-y2j§() * MathUtil.PI_QUARTER - MathUtil.§_-4S§ + MathUtil.PI_HALF;
         var _loc6_:int = Math.floor(Math.cos(_loc5_) * param3);
         var _loc7_:int = Math.floor(Math.sin(_loc5_) * param3);
         var _loc8_:Number = (Math.sqrt(_loc7_ * _loc7_ - g * 2 * (param4 - param2)) - _loc7_) / g;
         var _loc9_:int = Math.floor(param1 - _loc6_ * _loc8_);
         return {
            "x":_loc9_,
            "y":param4,
            "vx":_loc6_,
            "vy":_loc7_
         };
      }
      
      override protected function §_-D2a§(param1:Function) : void
      {
         §_-z1W§(§_-vO§.§_-Bu§(param1));
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         if(param1["shards"])
         {
            this.shards = §_-TA§(param1["shards"]).value;
         }
         if(param1["missiles"])
         {
            this.§_-U2g§ = §_-TA§(param1["missiles"]).value;
         }
      }
      
      override public function activate() : void
      {
         super.activate();
         if(shooting)
         {
            owner.§_-f2T§();
         }
      }
      
      override protected function §_-h2d§(param1:Number, param2:Number, param3:Boolean) : §_-di§
      {
         return Effects.putSimpleCrossMark(param1,param2);
      }
      
      override protected function shoot() : void
      {
         shooting = true;
         owner.§_-f2T§();
         this.unstableObject = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
         §_-Ia§.§_-33o§(§_-93O§,this.§_-da§);
      }
      
      protected function §_-Ck§() : void
      {
         this.§_-yz§ = 0;
         shooting = false;
         this.detach();
         nextTurnOnStable();
      }
      
      protected function §_-da§() : void
      {
         var _loc1_:Object = null;
         if(this.§_-yz§++ == this.§_-U2g§)
         {
            this.§_-Ck§();
         }
         else
         {
            _loc1_ = §_-E2D§(shotParams.point.x,shotParams.point.y,POWER);
            new Meteor(owner,paramSet,_loc1_.x,_loc1_.y,_loc1_.vx,_loc1_.vy,this.shards);
            §_-Ia§.§_-33o§(§_-93O§,this.§_-da§);
         }
      }
      
      override public function detach() : void
      {
         if(Boolean(this.unstableObject) && !this.unstableObject.disposed)
         {
            this.unstableObject.disposed = true;
            this.unstableObject = null;
         }
         shooting = false;
         super.detach();
      }
   }
}

