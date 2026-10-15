package ru.pragmatix.wormix.weapons
{
   import §_-21p§.§_-4w§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1g§.§_-L3Q§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.Shard;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.*;
   import starling.events.Event;
   
   public class NailGrenade extends BouncingClusterGrenade
   {
      
      private static const SHOT_DELAY:int = 3;
      
      private static const §_-y1§:uint = 350;
      
      private static const §_-e2W§:uint = 45;
      
      private var exploding:Boolean = false;
      
      private var §_-41L§:uint = 0;
      
      private var §_-B2s§:uint;
      
      private var paramSetEx:§_-C3e§;
      
      private var targets:Vector.<§_-01J§>;
      
      private var §_-F2T§:uint = 0;
      
      public function NailGrenade(param1:§_-01J§, param2:§_-C3e§, param3:ShellBurst, param4:uint, param5:Number, param6:uint, param7:Number, param8:String)
      {
         this.§_-B2s§ = param3.quantityShards.value;
         this.paramSetEx = param3.paramSetEx;
         super(param1,param2,null,§_-e2W§,param5,param6,param7,param8);
      }
      
      override public function explode() : void
      {
         this.targets = Physics.getUnitsInCircle(x,y,§_-y1§);
         if(this.targets.length > 0)
         {
            physParams.removeAllForces();
            setSpeedXY(0,0);
            this.exploding = true;
            frames = 0;
            counter.detach();
         }
         else
         {
            super.explode();
         }
      }
      
      override protected function grenadeTick() : void
      {
         var _loc1_:Vector.<§_-01J§> = null;
         var _loc2_:§_-01J§ = null;
         if(!this.exploding)
         {
            super.grenadeTick();
         }
         else
         {
            if(frames++ % SHOT_DELAY == 0)
            {
               _loc1_ = new Vector.<§_-01J§>(0);
               for each(_loc2_ in this.targets)
               {
                  if(!_loc2_.disposed)
                  {
                     _loc1_.push(_loc2_);
                  }
               }
               this.targets = _loc1_;
               if(this.targets.length > 0)
               {
                  this.§_-63X§();
                  var _loc3_:NailGrenade = this;
                  var _loc4_:Number = _loc3_.§_-41L§ + 1;
                  _loc3_.§_-41L§ = _loc4_;
               }
               else
               {
                  this.§_-41L§ = this.§_-B2s§;
               }
            }
            if(this.§_-41L§ == this.§_-B2s§)
            {
               super.explode();
            }
         }
      }
      
      private function §_-63X§() : void
      {
         var _loc1_:§_-01J§ = null;
         if(this.targets.length > 0)
         {
            ++this.§_-F2T§;
            if(this.§_-F2T§ > 15)
            {
               §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"nail grenade hack"));
            }
            _loc1_ = this.targets[§_-X1j§.randomSeed.§_-Fh§(0,this.targets.length - 1)];
            new Shard(owner,x,y,MathUtil.§_-m5§(this,_loc1_ as §_-F3o§),30,this.paramSetEx).physParams.removeAllForces();
         }
      }
   }
}

