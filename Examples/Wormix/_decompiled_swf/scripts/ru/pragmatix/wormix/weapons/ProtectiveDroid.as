package ru.pragmatix.wormix.weapons
{
   import §_-91A§.§_-sH§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-v2Q§;
   import §_-hO§.§_-y2Z§;
   import §_-l1g§.§_-L3Q§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import flash.events.TimerEvent;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.GetRating;
   import ru.pragmatix.wormix.messages.server.GetRatingResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.phase.HealBolt;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class ProtectiveDroid extends Mine implements §_-sH§
   {
      
      private var §_-o2c§:§_-01J§;
      
      private var §_-X1s§:§_-TA§;
      
      private var currentHp:§_-TA§ = new §_-TA§(100);
      
      private var §_-h2x§:§_-TA§ = new §_-TA§(0);
      
      public function ProtectiveDroid(param1:§_-01J§, param2:Number, param3:Number, param4:§_-TA§)
      {
         this.§_-o2c§ = param1;
         this.§_-X1s§ = param4;
         var _loc5_:§_-C3e§ = new §_-C3e§();
         _loc5_.ammoClassName = "ProtectiveDroid";
         _loc5_.damage = new §_-TA§(0);
         _loc5_.damageRadius = new §_-TA§(0);
         _loc5_.explosionRadius = new §_-TA§(0);
         _loc5_.pushForce = new §_-TA§(0);
         super(param1,param2,param3,_loc5_);
         §_-Nt§ = true;
         physParams.collidesWith = PhysParams.COLLIDES_WITH_ALL;
         physParams.collideType = CollideType.OTHER;
         §_-TZ§.addListener(param1 as EventDispatcher,§_-L3Q§.§_-Qw§,this.§_-D2U§);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.§_-il§);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.§_-h1Y§);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_LARGE,param2,param3,30);
      }
      
      override protected function initialize(param1:Point, param2:String = null, param3:Boolean = false, param4:String = "resurces/_ammo/") : void
      {
         super.initialize(param1,param2,param3,"");
      }
      
      private function §_-il§(param1:Event) : void
      {
         this.§_-h2x§.value = 0;
      }
      
      private function §_-h1Y§(param1:Event) : void
      {
         if(!this.§_-o2c§.disposed && this.§_-h2x§.value > 0)
         {
            new HealBolt(this.§_-o2c§,this.x,this.y,this.§_-o2c§,this.§_-h2x§.value);
         }
      }
      
      private function §_-D2U§(param1:Event, param2:§_-v2Q§) : void
      {
         var _loc3_:int = int(MathUtil.§_-na§(this.§_-X1s§.value,Math.ceil(param2.power)));
         if(_loc3_ > 0)
         {
            this.§_-h2x§.value += _loc3_;
         }
      }
      
      override public function defaultOnDamaged(param1:Event, param2:§_-v2Q§) : void
      {
         var _loc3_:uint = Math.ceil(param2.power);
         this.currentHp.value -= _loc3_;
         dispatchEvent(new §_-y2Z§(this,_loc3_));
         super.defaultOnDamaged(param1,param2);
         if(_loc3_ > 0)
         {
            Effects.tweenedText(_loc3_.toString(),Effects.DAMAGE_COLOR,x,y - 10,20);
         }
         if(this.currentHp.value <= 0)
         {
            disposed = true;
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC,x,y,§_-z1H§.width * 2.5,§_-d27§.§_-Z1d§);
         §_-TZ§.removeListener(this.§_-o2c§ as EventDispatcher,§_-L3Q§.§_-Qw§,this.§_-D2U§);
         this.§_-o2c§ = null;
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.§_-il§);
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.§_-h1Y§);
         this.§_-X1s§ = null;
         this.currentHp = null;
         this.§_-h2x§ = null;
         super.defaultOnDisposed(param1);
      }
      
      override protected function findTarget() : §_-01J§
      {
         return null;
      }
      
      override protected function updateEnemiesVec() : void
      {
      }
      
      public function getNameHintInfoPanel() : String
      {
         return §_-s2a§.§_-K3t§("BOT_NAME_ENGINEER_DROID");
      }
      
      public function getSquadNumber() : uint
      {
         return this.§_-o2c§.squad.getNumber();
      }
      
      public function get hp() : int
      {
         return this.currentHp.value;
      }
      
      public function get doesntCountTurns() : Boolean
      {
         return true;
      }
   }
}

