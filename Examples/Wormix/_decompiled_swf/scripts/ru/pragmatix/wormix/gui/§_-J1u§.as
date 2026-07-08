package ru.pragmatix.wormix.gui
{
   import §_-31W§.§_-E1E§;
   import §_-62§.§_-G1p§;
   import §_-A1K§.§_-TA§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73y§;
   import §_-Ly§.§_-81L§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-Y1O§.§_-V23§;
   import §_-Z1K§.§_-q24§;
   import §_-f1G§.*;
   import §_-fo§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import feathers.controls.ImageLoader;
   import feathers.core.ITextRenderer;
   import flash.globalization.DateTimeFormatter;
   import flash.globalization.LocaleID;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import org.gestouch.core.Touch;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.*;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   use namespace gestouch_internal;
   use namespace bi_internal;
   
   public class §_-J1u§ extends §_-fT§
   {
      
      private var _value:uint;
      
      private var _date:Date;
      
      private var §_-v2E§:DateTimeFormatter;
      
      public function §_-J1u§(param1:ITextRenderer)
      {
         super(param1 as DisplayObject);
         this._date = new Date();
         this.§_-v2E§ = new DateTimeFormatter(LocaleID.DEFAULT);
         this.§_-v2E§.setDateTimePattern("HH:mm:ss");
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this._date = null;
         this.§_-v2E§ = null;
      }
      
      public function set value(param1:uint) : void
      {
         this._value = param1;
         this._date.setTime(param1);
         (getContainer() as ITextRenderer).text = this.§_-v2E§.formatUTC(this._date);
      }
      
      public function get value() : uint
      {
         return this._value;
      }
   }
}

