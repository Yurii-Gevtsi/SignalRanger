package ru.pragmatix.wormix.game.rules
{
   import §_-31N§.§_-Yy§;
   import §_-62§.*;
   import §_-91A§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.Cookie;
   import §_-C2t§.§_-92W§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-CR§.§_-72p§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-fH§;
   import §_-K35§.§_-99§;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-U16§;
   import §_-U1i§.§_-XA§;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-dS§.§_-D3a§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-v1A§.*;
   import §_-z1g§.§_-5c§;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-F2L§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.LayoutGroup;
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
   import flash.display.SimpleButton;
   import flash.geom.ColorTransform;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-3p§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.client.PumpReactionRate;
   import ru.pragmatix.wormix.messages.server.AddToGroupResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.TouchEvent;
   import starling.text.TextFieldAutoSize;
   import starling.textures.Texture;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class §_-I1Q§ implements §_-l1m§
   {
      
      private const §_-dZ§:uint = 4;
      
      private const §_-f1R§:uint = 2;
      
      protected var squads:Vector.<§_-V23§>;
      
      private var profiles:Vector.<UserProfile>;
      
      private var §_-u1n§:Vector.<uint>;
      
      private var mapRecord:§_-D3a§;
      
      private var seed:int;
      
      protected var §_-717§:uint;
      
      public function §_-I1Q§(param1:§_-O2i§)
      {
         super();
         this.§_-717§ = param1.§_-sP§();
         this.profiles = param1.§_-D1h§();
         this.§_-u1n§ = param1.§_-E3P§();
         this.mapRecord = param1.mapRecord;
         this.seed = param1.seed;
      }
      
      public function dispose() : void
      {
         this.squads = null;
         this.profiles = null;
         this.§_-u1n§ = null;
         this.mapRecord = null;
      }
      
      public function §_-e2d§() : void
      {
      }
      
      public function §_-l26§(param1:§_-hK§) : void
      {
      }
      
      public function §_-73f§() : §_-3p§
      {
         return null;
      }
      
      public function §_-p1t§(param1:§_-J1§, param2:§_-XA§) : §_-hK§
      {
         return §_-Q2u§.§_-C1b§(param1,param2);
      }
      
      public function §_-Q1M§(param1:§_-O2i§, param2:§_-hK§) : §_-73y§
      {
         return null;
      }
      
      protected function §_-cM§(param1:§_-y1o§) : void
      {
         var _loc3_:UserProfile = null;
         this.squads = new Vector.<§_-V23§>(0);
         var _loc2_:uint = this.profiles.length <= 2 ? this.§_-dZ§ : this.§_-f1R§;
         var _loc4_:uint = 0;
         var _loc5_:int = int(this.profiles.length);
         while(_loc4_ < _loc5_)
         {
            _loc3_ = UserProfile(this.profiles[_loc4_]);
            this.squads[_loc4_] = §_-Q2u§.§_-x2K§(_loc3_,"Squad " + _loc4_,_loc4_,_loc4_ == this.§_-717§ ? §_-y1o§.§_-F2p§ : param1,this.§_-u1n§[_loc4_],§_-Q2u§.§_-33X§(_loc3_,_loc2_));
            _loc4_++;
         }
         §_-Q2u§.§_-T2R§(this.squads,this.mapRecord,this.seed);
      }
      
      public function §_-024§() : Boolean
      {
         return true;
      }
   }
}

