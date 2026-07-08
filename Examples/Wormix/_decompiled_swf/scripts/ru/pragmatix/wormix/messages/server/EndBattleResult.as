package ru.pragmatix.wormix.messages.server
{
   import §_-92a§.ArenaMenu;
   import §_-937§.§_-81u§;
   import §_-937§.§_-BO§;
   import §_-937§.§_-T15§;
   import §_-B1y§.Cookie;
   import §_-B2z§.§_-63i§;
   import §_-CR§.§_-72p§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-fH§;
   import §_-H2g§.§_-s1d§;
   import §_-K35§.§_-99§;
   import §_-P1B§.§_-bb§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-l1g§.§_-L3Q§;
   import dragonBones.events.EventObject;
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
   import flash.text.TextField;
   import flash.text.TextFormat;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.messages.SecureMessage;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.FreezingMissile;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   public class EndBattleResult extends SecureMessage
   {
      
      public static const OK:int = 0;
      
      public static const CHEAT:int = 1;
      
      public static const DUPLICATED:int = 2;
      
      public static const INVALID:int = 3;
      
      public var validateResult:int;
      
      public var result:uint;
      
      public var battleId:int;
      
      public var missionId:int;
      
      public var award:Array;
      
      public var advPurchases:Array;
      
      public var sessionKey:String;
      
      public function EndBattleResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "EndBattleResult{" + "validateResult=" + this.validateResult + ",result=" + this.result + ",battleId=" + this.battleId + ",missionId=" + this.missionId + ",sessionKey=" + this.sessionKey + ",isSecure=" + isSecure + "}";
      }
   }
}

