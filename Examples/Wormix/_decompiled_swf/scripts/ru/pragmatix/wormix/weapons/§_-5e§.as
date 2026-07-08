package ru.pragmatix.wormix.weapons
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-l1g§.§_-L3Q§;
   import §_-m0§.§_-X1F§;
   import §_-o14§.*;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-M2v§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import ru.pragmatix.wormix.weapons.ammo.abstracts.AbstractStickyAmmo;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class §_-5e§ extends AbstractStickyAmmo
   {
      
      private static const §_-S2j§:§_-TA§ = new §_-TA§(350);
      
      private static const §_-51z§:§_-TA§ = new §_-TA§(30);
      
      private var paramSet:§_-C3e§;
      
      private var §_-a2i§:uint;
      
      private var §_-L1F§:Boolean = false;
      
      private var §_-O2c§:Vector.<LaserRay> = new Vector.<LaserRay>(0);
      
      private var unstabilizer:§_-F3o§;
      
      private var maxRadius:uint;
      
      public function §_-5e§(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:uint)
      {
         super(param1,param2,param3,param4);
         tags = new <§_-D3B§>[§_-D3B§.§_-f1U§];
         this.maxRadius = §_-S2j§.value + param5;
         this.paramSet = param2;
         this.§_-a2i§ = param1 != null ? §_-M2v§.§_-42j§(param1.squad.getNumber()).value : §_-M2v§.UNKNOWN.value;
         §_-Y2I§.addEventListener(§_-42G§.§_-K2J§,this.onStable);
         §_-Y2I§.addEventListener(§_-42G§.§_-o1a§,this.onUnstable);
         this.§_-O2c§ = new Vector.<LaserRay>(0);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
         this.makeUnstabilizer();
      }
      
      override protected function onCollide(param1:Event) : void
      {
         if(!this.§_-L1F§)
         {
            x = param1.data.x;
            y = param1.data.y;
            §_-h2B§.play(§_-d27§.§_-i1Q§);
            this.§_-L1F§ = true;
            dispatchEventWith(§_-X1F§.§_-C3Z§(),false,§_-X1F§.create(this,false));
         }
         super.onCollide(param1);
      }
      
      private function onStable(param1:Event) : void
      {
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-A1n§(§_-5e§,this.§_-732§);
         §_-Ia§.§_-33o§(5,this.destroyUnstabilizer);
      }
      
      private function §_-732§(param1:§_-F3o§) : Boolean
      {
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:Number = NaN;
         var _loc8_:Number = NaN;
         var _loc9_:Boolean = false;
         var _loc10_:Boolean = false;
         var _loc11_:LaserRay = null;
         var _loc2_:§_-5e§ = param1 as §_-5e§;
         if(Boolean(_loc2_ && _loc2_ != this) && Boolean(_loc2_.stable) && owner.squad == _loc2_.owner.squad)
         {
            _loc3_ = x;
            _loc4_ = y;
            _loc5_ = _loc2_.x;
            _loc6_ = _loc2_.y;
            _loc7_ = this.maxRadius * this.maxRadius;
            _loc8_ = §_-51z§.value * §_-51z§.value;
            _loc9_ = MathUtil.§_-Ih§(_loc3_,_loc4_,_loc5_,_loc6_) <= _loc7_;
            _loc10_ = MathUtil.§_-Ih§(_loc3_,_loc4_,_loc5_,_loc6_) >= _loc8_;
            if(_loc9_ && _loc10_)
            {
               _loc11_ = new LaserRay(owner,this.paramSet.clone(),this,_loc2_,this.§_-a2i§);
               this.§_-aU§(_loc11_);
               _loc2_.§_-aU§(_loc11_);
               §_-h2B§.play(§_-d27§.§_-21k§);
            }
         }
         return false;
      }
      
      private function onUnstable(param1:Event) : void
      {
         this.§_-M2V§();
         this.makeUnstabilizer();
      }
      
      private function nextTurn(param1:Event) : void
      {
         if(this.§_-O2c§.length == 0)
         {
            disposed = true;
         }
      }
      
      private function §_-aU§(param1:LaserRay) : void
      {
         this.§_-O2c§.push(param1);
      }
      
      private function §_-M2V§() : void
      {
         var _loc1_:LaserRay = null;
         for each(_loc1_ in this.§_-O2c§)
         {
            if(_loc1_.physParams)
            {
               _loc1_.disposed = true;
            }
         }
         this.§_-O2c§ = new Vector.<LaserRay>(0);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         this.§_-M2V§();
         this.destroyUnstabilizer();
         if(!this.§_-L1F§)
         {
            dispatchEventWith(§_-X1F§.§_-C3Z§(),false,§_-X1F§.create(this,false));
         }
         §_-Y2I§.removeEventListener(§_-42G§.§_-K2J§,this.onStable);
         §_-Y2I§.removeEventListener(§_-42G§.§_-o1a§,this.onUnstable);
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_DRILL,x,y,12);
         super.defaultOnDisposed(param1);
      }
      
      private function makeUnstabilizer() : void
      {
         this.destroyUnstabilizer();
         this.unstabilizer = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
      }
      
      private function destroyUnstabilizer() : void
      {
         if(Boolean(this.unstabilizer) && !this.unstabilizer.disposed)
         {
            this.unstabilizer.disposed = true;
            this.unstabilizer = null;
         }
      }
   }
}

