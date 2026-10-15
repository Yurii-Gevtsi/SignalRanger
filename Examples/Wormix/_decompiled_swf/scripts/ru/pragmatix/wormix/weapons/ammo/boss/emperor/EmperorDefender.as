package ru.pragmatix.wormix.weapons.ammo.boss.emperor
{
   import §_-b1F§.§_-G4§;
   import §_-b1F§.§_-v2Q§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class EmperorDefender extends EmperorTerracottaWarrior
   {
      
      private static const BEFORE_DAMAGED_HANDLER_NAME:String = "Defender_before_damaged_handler";
      
      private var beforeDamageHandlerID:String;
      
      public function EmperorDefender(param1:§_-01J§, param2:Number, param3:Number)
      {
         armatureName = "TerraccotaDefender";
         name = §_-s2a§.§_-K3t§(§_-s2a§.BOSS_EMPEROR_DEFENDER);
         super(param1,param2,param3);
         this.beforeDamageHandlerID = this.owner.attributes.hp.§_-No§(this.beforeDamaged,BEFORE_DAMAGED_HANDLER_NAME);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(!owner.disposed)
         {
            owner.attributes.hp.§_-n2U§(this.beforeDamageHandlerID);
         }
         super.defaultOnDisposed(param1);
      }
      
      protected function beforeDamaged(param1:Event, param2:§_-v2Q§) : void
      {
         var _loc3_:Number = NaN;
         if(!owner.disposed && param2.power > 0 && !this.disposed && attributes.hp.value > 0)
         {
            _loc3_ = param2.power > attributes.hp.value ? attributes.hp.value : param2.power;
            §_-G4§.inflictDamage(param2.source,this,_loc3_,0,0);
            Effects.floatingText("(" + Math.ceil(_loc3_) + ")",Effects.TEXT_COLOR_GOLD,owner.x,owner.y + owner.height * 0.5,20);
            param2.power -= _loc3_;
         }
      }
   }
}

