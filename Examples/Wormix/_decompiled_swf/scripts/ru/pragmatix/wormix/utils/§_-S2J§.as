package ru.pragmatix.wormix.utils
{
   import §_-73I§.*;
   import §_-DJ§.§_-fH§;
   import §_-Y1O§.§_-V23§;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-Z2z§;
   import §_-j22§.§_-5g§;
   import §_-r1C§.§_-h2B§;
   import feathers.controls.Label;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.messages.server.AddToGroupResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   
   public class §_-S2J§
   {
      
      private var _label:Label;
      
      private var _fitWidth:Number;
      
      private var _fitHeight:Number;
      
      public function §_-S2J§(param1:Label, param2:Number = 0, param3:Number = 0)
      {
         super();
         this._label = param1;
         this._fitWidth = param2;
         this._fitHeight = param3;
         this._label.addEventListener(Event.RESIZE,this.§_-W1X§);
      }
      
      public function dispose() : void
      {
         if(this._label)
         {
            this._label.removeEventListener(Event.RESIZE,this.§_-W1X§);
         }
         this._label = null;
      }
      
      private function §_-W1X§(param1:Event) : void
      {
         var _loc2_:Number = this._label.width / this._label.scale;
         var _loc3_:Number = this._label.height / this._label.scale;
         var _loc4_:Number = Boolean(this._fitWidth) && _loc2_ > this._fitWidth ? this._fitWidth / _loc2_ : 1;
         var _loc5_:Number = Boolean(this._fitHeight) && _loc3_ > this._fitHeight ? this._fitHeight / _loc3_ : 1;
         var _loc6_:Number = Math.min(_loc4_,_loc5_);
         if(this._label.scale != _loc6_)
         {
            this._label.scale = _loc6_;
         }
      }
   }
}

