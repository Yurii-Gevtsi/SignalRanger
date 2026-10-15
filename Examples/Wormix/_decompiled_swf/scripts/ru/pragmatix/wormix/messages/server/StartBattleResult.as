package ru.pragmatix.wormix.messages.server
{
   import §_-937§.§_-81u§;
   import §_-937§.§_-BO§;
   import §_-937§.§_-T15§;
   import §_-B1y§.Cookie;
   import §_-C2t§.§_-92W§;
   import §_-CR§.§_-72p§;
   import §_-D3A§.§_-TZ§;
   import §_-K35§.§_-99§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-jF§.ClanInviteRenderer;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import com.hurlant.crypto.tls.§_-A3§;
   import feathers.controls.Button;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import flash.net.Socket;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.*;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.GuardingBot;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class StartBattleResult
   {
      
      public var battleId:uint;
      
      public var reagentsForBattle:Vector.<int> = new Vector.<int>();
      
      public function StartBattleResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "StartBattleResult{battleId=" + String(this.battleId) + ",reagentsForBattle=" + String(this.reagentsForBattle) + "}";
      }
   }
}

