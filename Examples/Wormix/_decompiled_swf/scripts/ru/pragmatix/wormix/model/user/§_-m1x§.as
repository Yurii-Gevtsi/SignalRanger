package ru.pragmatix.wormix.model.user
{
   import §_-21p§.§_-4w§;
   import §_-21p§.§_-N1H§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-zF§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-x2t§;
   import §_-G2H§.§_-527§;
   import §_-V1H§.§_-m1d§;
   import §_-Y1Q§.§_-61Z§;
   import §_-j1w§.§_-B2i§;
   import §_-n1w§.X509Certificate;
   import §_-r1C§.§_-h2B§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.phase.FirePhase;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-m1x§
   {
      
      public var hat:§_-zF§;
      
      public var artifact:§_-T1y§;
      
      public var raceRecord:§_-B2i§;
      
      public var §_-g2I§:§_-TA§ = new §_-TA§(0);
      
      public var §_-Z2U§:§_-TA§ = new §_-TA§(0);
      
      public var §_-A3Z§:§_-TA§ = new §_-TA§(0);
      
      public var §_-q1G§:§_-TA§ = new §_-TA§(0);
      
      public var §_-v2n§:§_-TA§ = new §_-TA§(0);
      
      public var bonusAttack:§_-TA§ = new §_-TA§(0);
      
      private var record:UserProfile;
      
      public function §_-m1x§()
      {
         super();
      }
      
      public function §_-Yd§(param1:§_-zF§) : void
      {
         var _loc2_:Boolean = §_-71v§.§_-Q19§(this.record.§_-t2I§());
         if(this.hat)
         {
            this.§_-X20§(this.hat.§_-V1v§(_loc2_));
         }
         this.hat = param1;
         if(param1)
         {
            this.§_-7y§(param1.§_-V1v§(_loc2_));
         }
      }
      
      public function §_-W1V§(param1:§_-T1y§) : void
      {
         var _loc2_:Boolean = §_-71v§.§_-Q19§(this.record.§_-t2I§());
         if(this.artifact)
         {
            this.§_-X20§(this.artifact.§_-V1v§(_loc2_));
         }
         this.artifact = param1;
         if(param1)
         {
            this.§_-7y§(param1.§_-V1v§(_loc2_));
         }
      }
      
      public function §_-2S§(param1:§_-B2i§) : void
      {
         if(this.raceRecord)
         {
            this.§_-X20§(this.raceRecord.§_-V1v§());
         }
         if(this.hat)
         {
            this.hat = §_-G1p§.§_-12J§(this.hat.getId(),param1);
         }
         this.raceRecord = param1;
         this.§_-7y§(param1.§_-V1v§());
      }
      
      private function §_-7y§(param1:§_-N1H§) : void
      {
         this.change(param1,1);
      }
      
      private function §_-X20§(param1:§_-N1H§) : void
      {
         this.change(param1,-1);
      }
      
      private function change(param1:§_-N1H§, param2:int) : void
      {
         var _loc5_:int = 0;
         this.§_-q1G§.value += this.§_-F1e§(param1,§_-x2M§.HP) * param2;
         var _loc3_:int = this.§_-F1e§(param1,§_-x2M§.§_-93u§);
         if(_loc3_ > 0)
         {
            this.§_-q1G§.value += Math.round(MathUtil.§_-na§(_loc3_,this.§_-g2I§.value)) * param2;
         }
         this.bonusAttack.value += this.§_-F1e§(param1,§_-x2M§.ATTACK) * param2 + this.§_-F1e§(param1,§_-x2M§.SQUAD_ATTACK) * param2;
         this.§_-v2n§.value += this.§_-F1e§(param1,§_-x2M§.ARMOR) * param2 + this.§_-F1e§(param1,§_-x2M§.SQUAD_ARMOR) * param2;
         var _loc4_:§_-4w§ = this.§_-k2Q§(param1,§_-x2M§.§_-53e§);
         if(_loc4_)
         {
            _loc5_ = _loc4_.getValue();
            if(_loc5_ > 0)
            {
               this.§_-v2n§.value += §_-m1d§.§_-F1e§(_loc5_,this.record.getLevel()) * param2;
            }
         }
      }
      
      private function §_-F1e§(param1:§_-N1H§, param2:§_-x2M§) : int
      {
         var _loc3_:§_-4w§ = null;
         for each(_loc3_ in param1.§_-b2I§())
         {
            if(_loc3_.getType() == param2)
            {
               return _loc3_.getValue();
            }
         }
         return 0;
      }
      
      private function §_-k2Q§(param1:§_-N1H§, param2:§_-x2M§) : §_-4w§
      {
         var _loc3_:§_-4w§ = null;
         for each(_loc3_ in param1.§_-b2I§())
         {
            if(_loc3_.getType() == param2)
            {
               return _loc3_;
            }
         }
         return null;
      }
      
      public function §_-BU§(param1:UserProfile) : void
      {
         this.record = param1;
      }
   }
}

