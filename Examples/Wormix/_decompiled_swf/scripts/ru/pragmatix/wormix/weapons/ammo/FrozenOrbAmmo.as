package ru.pragmatix.wormix.weapons.ammo
{
   import §_-L1H§.§_-C3e§;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.type.missile.bazooka.BazookaMissile;
   import starling.events.Event;
   
   public class FrozenOrbAmmo extends BazookaMissile
   {
      
      private static const PI_QUARTER:Number = MathUtil.PI_QUARTER;
      
      private const MAX_FRAME:uint = 8;
      
      private const MAX_STACK_ICE:uint = 2;
      
      private var count:uint = 0;
      
      private var paramSetEx:§_-C3e§;
      
      private var power:Number;
      
      private var angles:Vector.<Number> = new Vector.<Number>();
      
      private var frozenWorms:Vector.<§_-01J§> = new Vector.<§_-01J§>();
      
      private var countFrozen:Vector.<uint> = new Vector.<uint>();
      
      private var ices:Vector.<IceShard> = new Vector.<IceShard>();
      
      public function FrozenOrbAmmo(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:§_-C3e§)
      {
         super(param1,param2,param3,param4);
         §_-In§ = DamageType.COLD;
         this.paramSetEx = param5;
         §_-32§ = §_-uN§;
         this.power = param4 * 0.8;
         var _loc6_:Number = MathUtil.§_-y2w§;
         this.angles[0] = _loc6_;
         var _loc7_:uint = 1;
         while(_loc7_ < 6)
         {
            this.angles[_loc7_] = this.angles[_loc7_ - 1] + _loc6_;
            _loc7_++;
         }
      }
      
      override protected function explode(param1:§_-F3o§) : void
      {
         if(param1 != null && param1 is §_-8K§)
         {
            §_-8K§(param1).buffs.place(new §_-Z2z§());
         }
         Effects.explodeParticles(x,y,16);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,x,y,3 * paramSet.damageRadius.value,§_-d27§.§_-Y24§);
         inflictDamage(param1,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value);
      }
      
      override protected function mainOnFrame() : void
      {
         var _loc1_:uint = 0;
         var _loc2_:uint = 0;
         var _loc3_:uint = 0;
         if(++this.count >= this.MAX_FRAME)
         {
            this.count = 0;
            _loc1_ = this.angles.length;
            _loc2_ = this.ices.length;
            _loc3_ = 0;
            while(_loc3_ < _loc1_)
            {
               this.ices[_loc2_ + _loc3_] = new IceShard(owner,x,y,this.paramSetEx,this.angles[_loc3_],this.power);
               this.ices[_loc2_ + _loc3_].addEventListener(§_-L3Q§.§_-o13§,this.onCollideIce);
               this.angles[_loc3_] += PI_QUARTER;
               _loc3_++;
            }
            §_-h2B§.play(§_-d27§.§_-Lp§);
         }
      }
      
      private function onCollideIce(param1:Event) : void
      {
         var _loc2_:uint = 0;
         var _loc3_:uint = 0;
         var _loc4_:§_-01J§ = null;
         if(param1.data.other is §_-01J§)
         {
            _loc4_ = §_-01J§(param1.data.other);
            _loc3_ = this.frozenWorms.length;
            _loc2_ = 0;
            while(_loc2_ < _loc3_)
            {
               if(_loc4_ == this.frozenWorms[_loc2_])
               {
                  ++this.countFrozen[_loc2_];
                  this.checkFrozenWorm();
                  return;
               }
               _loc2_++;
            }
            this.frozenWorms[this.frozenWorms.length] = _loc4_;
            this.countFrozen[this.countFrozen.length] = 1;
            this.checkFrozenWorm();
         }
         param1.currentTarget.removeEventListener(§_-L3Q§.§_-o13§,this.onCollideIce);
         _loc3_ = this.ices.length;
         _loc2_ = 0;
         while(_loc2_ < _loc3_)
         {
            if(IceShard(param1.currentTarget) == this.ices[_loc2_])
            {
               this.ices.splice(_loc2_,1);
               break;
            }
            _loc2_++;
         }
         if(this.ices.length == 0)
         {
            this.frozenWorms.length = 0;
            this.countFrozen.length = 0;
         }
      }
      
      private function checkFrozenWorm() : void
      {
         var _loc1_:uint = this.frozenWorms.length;
         var _loc2_:uint = 0;
         while(_loc2_ < _loc1_)
         {
            if(this.countFrozen[_loc2_] >= this.MAX_STACK_ICE && this.countFrozen[_loc2_] < this.MAX_STACK_ICE + 1)
            {
               §_-01J§(this.frozenWorms[_loc2_]).buffs.place(new §_-M2S§());
            }
            _loc2_++;
         }
      }
   }
}

