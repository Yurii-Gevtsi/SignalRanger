package ru.pragmatix.wormix.model.user
{
   import §_-03T§.§_-2p§;
   import §_-b1F§.§_-G4§;
   import §_-r1C§.§_-h2B§;
   import §_-x2Q§.§_-dU§;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   import starling.utils.MathUtil;
   
   public class §_-uH§
   {
      
      private var _level:uint = 0;
      
      private var §_-4h§:uint = 0;
      
      private var §_-cF§:uint = 0;
      
      private var §_-226§:uint = 0;
      
      private var §_-31R§:uint = 0;
      
      public function §_-uH§(param1:uint = 1)
      {
         super();
         this.§_-N1n§(param1);
      }
      
      public function §_-N1n§(param1:uint) : void
      {
         this._level = param1;
         this.§_-4h§ = param1 * 2;
         this.refresh();
      }
      
      public function §_-eQ§(param1:uint) : void
      {
         this.§_-cF§ = MathUtil.clamp(param1,0,this.§_-4h§);
         this.refresh();
      }
      
      public function §_-q1H§(param1:uint) : void
      {
         this.§_-226§ = MathUtil.clamp(param1,0,this.§_-4h§);
         this.refresh();
      }
      
      private function refresh() : void
      {
         this.§_-31R§ = this.§_-4h§ - this.§_-226§ - this.§_-cF§;
      }
      
      public function toString() : String
      {
         return "SkillPointsModel{" + "level=" + this._level + ",total=" + this.§_-4h§ + ",attack=" + this.§_-cF§ + ",armor=" + this.§_-226§ + ",free=" + this.§_-31R§ + "}";
      }
      
      public function get level() : uint
      {
         return this._level;
      }
      
      public function get §_-VY§() : uint
      {
         return this.§_-4h§;
      }
      
      public function get attack() : uint
      {
         return this.§_-cF§;
      }
      
      public function get armor() : uint
      {
         return this.§_-226§;
      }
      
      public function get §_-41C§() : uint
      {
         return this.§_-31R§;
      }
   }
}

