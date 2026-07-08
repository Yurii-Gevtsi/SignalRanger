package ru.pragmatix.wormix.pvp.messages.structures
{
   import §_-12W§.*;
   import §_-CR§.§_-72p§;
   import §_-D3A§.§_-TZ§;
   import §_-K35§.§_-99§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-b1F§.*;
   import §_-jF§.*;
   import §_-l1g§.§_-L3Q§;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-a2P§;
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
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class PositionStructure
   {
      
      public var x:int;
      
      public var y:int;
      
      public function PositionStructure(param1:int = 0, param2:int = 0)
      {
         super();
         this.x = param1;
         this.y = param2;
      }
      
      public function toString() : String
      {
         return "PositionStructure{ x=" + String(this.x) + ",y=" + String(this.y) + "}";
      }
   }
}

