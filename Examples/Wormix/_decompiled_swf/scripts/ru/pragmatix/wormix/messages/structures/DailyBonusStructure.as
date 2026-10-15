package ru.pragmatix.wormix.messages.structures
{
   import §_-A1K§.§_-TA§;
   import §_-jF§.ClanInviteRenderer;
   import §_-rM§.§_-61j§;
   import feathers.controls.Button;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ListCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.*;
   import ru.pragmatix.wormix.messages.server.SteamTransactionInitResponse;
   import starling.events.Event;
   
   public class DailyBonusStructure
   {
      
      public var loginSequence:int;
      
      public var dailyBonysType:int;
      
      public var dailyBonusCount:int;
      
      public function DailyBonusStructure()
      {
         super();
      }
      
      public function toString() : String
      {
         return "DailyBonusStructure{loginSequence=" + String(this.loginSequence) + ",dailyBonysType=" + String(this.dailyBonysType) + ",dailyBonusCount=" + String(this.dailyBonusCount) + "}";
      }
   }
}

