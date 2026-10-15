package ru.pragmatix.wormix.weapons
{
   import §_-31W§.§_-Kb§;
   import §_-F39§.§_-a2W§;
   import §_-Tm§.§_-RF§;
   import §_-kP§.CraftTreePanel;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-u1m§.§_-51A§;
   import §_-w2i§.§_-Zd§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.cloud.FreezingCloud;
   import ru.pragmatix.wormix.weapons.ammo.cloud.GasCloud;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.GasGrenade;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.extensions.ColorArgb;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   public class PushingMineWeapon extends §_-51A§
   {
      
      private static const §_-W1w§:uint = 150;
      
      private static const §_-c1L§:uint = 90;
      
      private static const §_-Z1Z§:int = 14;
      
      private var target:§_-01J§;
      
      private var §_-L0§:Weapon;
      
      private var §_-I2N§:Boolean = false;
      
      private var damaged:Boolean = false;
      
      private var pushPower:int;
      
      public function PushingMineWeapon()
      {
         super(null);
         §_-C3j§ = false;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         this.pushPower = param1["pushPower"] ? int(param1["pushPower"]) : §_-Z1Z§;
         §_-z1W§(new §_-cZ§(param1["groundSetup"] ? null : this.§_-413§));
      }
      
      override protected function §_-D2a§() : void
      {
      }
      
      override protected function §_-H1g§() : Mine
      {
         var _loc1_:int = this.target ? int(§_-c1L§) : int(§_-W1w§);
         return new §_-Y9§(owner,owner.x,owner.y,paramSet,this.target,_loc1_,this.pushPower);
      }
      
      override protected function shoot() : void
      {
         if(!this.target)
         {
            this.findTarget();
         }
         §_-h2B§.play(§_-d27§.§_-G3Z§);
         §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(this.target ? §_-s2a§.PUSHING_MINE_UNIT_SETUP : §_-s2a§.PUSHING_MINE_GROUND_SETUP),true);
         this.§_-I2N§ = true;
         this.§_-H1g§();
         this.detach();
         §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = !this.damaged;
      }
      
      private function §_-413§() : Boolean
      {
         if(this.findTarget())
         {
            return true;
         }
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.PUSHING_MINE_USELESS),true);
         }
         return false;
      }
      
      private function findTarget() : §_-01J§
      {
         var _loc1_:Point = null;
         var _loc2_:Vector.<§_-01J§> = null;
         _loc1_ = Pool.getPoint(owner.x + Physics.UNIT_HALF_WIDTH * owner.direction,owner.y);
         _loc2_ = §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner);
         var _loc3_:Vector.<§_-01J§> = Physics.getUnitsInCircle(_loc1_.x,_loc1_.y,Physics.UNIT_HALF_HEIGHT,_loc2_);
         Pool.putPoint(_loc1_);
         if(_loc3_.length > 0)
         {
            this.target = _loc3_[0];
         }
         return this.target;
      }
      
      override public function jumpShoot(param1:Boolean) : void
      {
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-RF§.fire(§_-RF§.§_-e2p§);
         }
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         var unit:§_-01J§ = param1;
         super.attach(unit);
         §_-Zd§.once(owner as EventDispatcher,§_-L3Q§.§_-Qw§,function():void
         {
            if(active)
            {
               damaged = true;
            }
         });
         if(!(owner.§_-81o§ is PushingMineWeapon && this.§_-L0§ != null))
         {
            this.§_-L0§ = owner.§_-81o§ == null || owner.§_-81o§ is SwitchTurn || owner.§_-81o§ is PushingMineWeapon ? §_-73v§.§_-z1F§(owner.record) : owner.§_-81o§;
         }
      }
      
      override public function detach() : void
      {
         §_-YH§ = true;
         var _loc1_:§_-01J§ = owner;
         super.detach();
         if(this.§_-I2N§ && !this.damaged)
         {
            _loc1_.§_-81o§ = this.§_-L0§;
            _loc1_.reattachWeapon();
            this.§_-L0§ = null;
         }
         this.§_-I2N§ = false;
         this.damaged = false;
         this.target = null;
      }
   }
}

