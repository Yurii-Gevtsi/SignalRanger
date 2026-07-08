package ru.pragmatix.wormix.messages.client
{
   import §_-62§.§_-W1r§;
   import §_-91A§.*;
   import §_-937§.§_-n1b§;
   import §_-CF§.§_-p2U§;
   import §_-CR§.§_-72p§;
   import §_-K35§.§_-99§;
   import §_-P1z§.*;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1b§.§_-81l§;
   import §_-j1w§.§_-L1x§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-kS§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.events.DeviceOrientationEvent;
   import com.greensock.loading.*;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import feathers.controls.LayoutGroup;
   import feathers.controls.NumericStepper;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.utils.IDataInput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class SetLocale
   {
      
      public var locale:int;
      
      public function SetLocale()
      {
         super();
      }
      
      public function setLanguage(param1:§_-p2U§) : void
      {
         switch(param1.§_-b2§())
         {
            case "ru":
               this.locale = 1;
               break;
            case "en":
               this.locale = 2;
               break;
            case "zn":
               this.locale = 3;
               break;
            default:
               this.locale = 0;
         }
      }
   }
}

