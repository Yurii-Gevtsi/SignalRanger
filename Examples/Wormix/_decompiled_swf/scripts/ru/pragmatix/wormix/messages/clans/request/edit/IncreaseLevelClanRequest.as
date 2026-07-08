package ru.pragmatix.wormix.messages.clans.request.edit
{
   import §_-12W§.*;
   import §_-CR§.§_-72p§;
   import §_-K35§.§_-99§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-j22§.§_-5g§;
   import §_-jF§.*;
   import com.hurlant.util.§_-a2P§;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.messages.server.BuyRaceResult;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.StringUtil;
   import starling.events.Event;
   
   public class IncreaseLevelClanRequest
   {
      
      public var level:int;
      
      public var fromTreas:Boolean;
      
      public function IncreaseLevelClanRequest(param1:int, param2:Boolean)
      {
         super();
         this.level = param1;
         this.fromTreas = param2;
      }
   }
}

