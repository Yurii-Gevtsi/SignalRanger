package ru.pragmatix.wormix.controller
{
   import §_-21p§.§_-4w§;
   import §_-31W§.§_-Kb§;
   import §_-31W§.§_-Y1v§;
   import §_-31W§.§_-aO§;
   import §_-31W§.§_-r26§;
   import §_-43V§.Random;
   import §_-51M§.Cat;
   import §_-51W§.§_-SN§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-gr§;
   import §_-62§.§_-o6§;
   import §_-73I§.§_-K2s§;
   import §_-73d§.§_-pT§;
   import §_-82l§.§_-A2Y§;
   import §_-91A§.*;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A1m§.*;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.Cookie;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-C3f§.§_-4I§;
   import §_-C3f§.§_-8B§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-CR§.§_-72p§;
   import §_-Cl§.*;
   import §_-DJ§.§_-Es§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.*;
   import §_-G2H§.*;
   import §_-H16§.*;
   import §_-H2g§.§_-di§;
   import §_-J3b§.*;
   import §_-Jd§.BonusStat;
   import §_-Jd§.§_-11K§;
   import §_-K35§.§_-p1m§;
   import §_-K3r§.§_-E3O§;
   import §_-Ly§.§_-81L§;
   import §_-Nq§.§_-G1J§;
   import §_-Nq§.§_-Ow§;
   import §_-Nq§.§_-s1O§;
   import §_-Nq§.§_-uh§;
   import §_-TF§.§_-P19§;
   import §_-Ta§.*;
   import §_-U1F§.§_-B3b§;
   import §_-U1Y§.*;
   import §_-U1i§.§_-b1k§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-vx§;
   import §_-YF§.§_-Zi§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-v2Q§;
   import §_-g2C§.GlobalChatTextMessageItemRenderer;
   import §_-g2C§.§_-62U§;
   import §_-g2C§.§_-I1R§;
   import §_-g2C§.§_-K2A§;
   import §_-g2C§.§_-K2u§;
   import §_-g2C§.§_-p8§;
   import §_-g2C§.§_-w1K§;
   import §_-g2e§.§_-q2Z§;
   import §_-j1w§.§_-B2i§;
   import §_-j22§.§_-5g§;
   import §_-j2R§.§_-73G§;
   import §_-jF§.*;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-k1u§.§_-Vr§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-C3A§;
   import §_-k2s§.§_-fG§;
   import §_-kP§.*;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-nE§.DowngradeCraftWeaponScreen;
   import §_-nE§.InfoCraftWeaponScreen;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.UpgradeCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-q11§.RaceRoomAbilityRenderer;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-sQ§.§_-H3D§;
   import §_-u13§.MAC;
   import §_-u2J§.§_-T1K§;
   import §_-w2i§.§_-Ia§;
   import §_-x2Q§.§_-dU§;
   import §_-y1s§.*;
   import §_-z1g§.*;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.*;
   import com.hurlant.crypto.*;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-F2L§;
   import com.hurlant.util.§_-a2P§;
   import dragonBones.Armature;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.List;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.controls.ScrollBarDisplayMode;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.data.ArrayCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.layout.VerticalLayoutData;
   import feathers.skins.ImageSkin;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.themes.styles.AlternateTextStyles;
   import feathers.utils.textures.TextureCache;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.system.ApplicationDomain;
   import flash.system.LoaderContext;
   import flash.system.SecurityDomain;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.setTimeout;
   import org.swiftsuspenders.Injector;
   import org.swiftsuspenders.§_-71M§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.CommandState;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.messages.clans.main.IncomingClanMessage;
   import ru.pragmatix.wormix.messages.clans.main.server.InviteFriendToClan;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.PostToClanChatResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.AchieveLogin;
   import ru.pragmatix.wormix.messages.client.AddToGroup;
   import ru.pragmatix.wormix.messages.client.BuyBattle;
   import ru.pragmatix.wormix.messages.client.BuyRace;
   import ru.pragmatix.wormix.messages.client.BuyReactionRate;
   import ru.pragmatix.wormix.messages.client.BuyUnlockMission;
   import ru.pragmatix.wormix.messages.client.ResetParameters;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.server.AddToGroupResult;
   import ru.pragmatix.wormix.messages.server.BuyBattleResult;
   import ru.pragmatix.wormix.messages.server.BuyRaceResult;
   import ru.pragmatix.wormix.messages.server.BuyReactionRateResult;
   import ru.pragmatix.wormix.messages.server.BuyUnlockMissionResult;
   import ru.pragmatix.wormix.messages.server.ResetParametersResult;
   import ru.pragmatix.wormix.messages.server.UpgradeWeaponResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.AtomBomb;
   import ru.pragmatix.wormix.weapons.ammo.phase.HarmPhase;
   import starling.core.Starling;
   import starling.display.Button;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.filters.ColorMatrixFilter;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class §_-MJ§
   {
      
      private var §_-TR§:Vector.<§_-yG§> = new Vector.<§_-yG§>();
      
      private var §_-417§:Vector.<Function> = new Vector.<Function>();
      
      public function §_-MJ§()
      {
         super();
      }
      
      public function dispose() : void
      {
      }
      
      public function §_-Hf§(param1:§_-E19§, param2:Function) : void
      {
         this.§_-N1X§(new §_-yG§(param1,§_-4I§.§_-C39§),param2,this.§_-ep§,[param1.id,false]);
      }
      
      public function buyFiveBattles(param1:§_-E19§, param2:Function) : void
      {
         this.§_-N1X§(new §_-yG§(param1,§_-4I§.§_-R10§),param2,this.§_-ep§,[param1.id,true]);
      }
      
      private function §_-ep§(param1:int, param2:Boolean) : void
      {
         §_-d21§.§_-51l§(new BuyBattle(param1,param2),BuyBattleResult,this.§_-Uq§,this.§_-f2z§);
      }
      
      private function §_-Uq§(param1:§_-63i§, ... rest) : void
      {
         var _loc4_:int = 0;
         var _loc5_:String = null;
         var _loc6_:AdvertisingPurchaseStructure = null;
         var _loc3_:BuyBattleResult = BuyBattleResult(param1.message);
         if(_loc3_.advPurchases.length)
         {
            for each(_loc6_ in _loc3_.advPurchases)
            {
               _loc4_ = _loc6_.nextTime - §_-X1j§.serverTime;
               _loc5_ = §_-72u§.§_-I1I§(_loc4_ * §_-72u§.§_-E1O§);
               if(_loc6_.name == §_-B3b§.§_-C39§)
               {
               }
            }
         }
         this.§_-vK§(_loc3_.result);
      }
      
      public function §_-I3K§(param1:int, param2:Function) : void
      {
         var _loc3_:§_-x2t§ = new §_-x2t§(§_-4I§.REACTION.§_-R1T§ * param1 / 3,§_-4I§.REACTION.realPrice * param1 / 3);
         this.§_-N1X§(new §_-yG§(§_-E19§.§_-51D§,_loc3_),param2,this.§_-lK§,[param1]);
      }
      
      private function §_-lK§(param1:int) : void
      {
         var _loc2_:Object = new BuyReactionRate(param1);
         §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc2_,BuyReactionRateResult,this.§_-2D§,this.§_-f2z§);
      }
      
      private function §_-2D§(param1:§_-63i§) : void
      {
         this.§_-vK§(BuyReactionRateResult(param1.message).result);
      }
      
      public function §_-w2c§(param1:§_-E19§, param2:Function) : void
      {
         this.§_-N1X§(new §_-yG§(param1,§_-4I§.§_-y1F§),param2,this.§_-xr§,[param1.id]);
      }
      
      private function §_-xr§(param1:int) : void
      {
         var _loc2_:Object = new ResetParameters(param1);
         §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc2_,ResetParametersResult,this.§_-Z1W§,this.§_-f2z§);
      }
      
      private function §_-Z1W§(param1:§_-63i§) : void
      {
         this.§_-vK§(ResetParametersResult(param1.message).result);
      }
      
      public function §_-v1r§(param1:UserProfile, param2:§_-E19§, param3:uint, param4:uint, param5:int, param6:Function) : void
      {
         this.§_-N1X§(new §_-yG§(param2,new §_-x2t§(param3,param3)),param6,this.§_-61u§,[param1.getId(),param2,param4,param5]);
      }
      
      private function §_-61u§(param1:int, param2:§_-E19§, param3:uint, param4:int) : void
      {
         var _loc5_:Object = new AddToGroup(param1,param2.id,param3,param4);
         §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc5_,AddToGroupResult,this.§_-iL§,this.§_-f2z§);
      }
      
      public function §_-Wi§(param1:UserProfile, param2:§_-E19§, param3:uint, param4:uint, param5:Function) : void
      {
         this.§_-N1X§(new §_-yG§(param2,new §_-x2t§(param3,param3)),param5,this.§_-W2l§,[param1.getId(),param2,param4]);
      }
      
      private function §_-W2l§(param1:int, param2:§_-E19§, param3:uint) : void
      {
         var _loc4_:Object = new AddToGroup(param1,param2.id,param3);
         §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc4_,AddToGroupResult,this.§_-iL§,this.§_-f2z§);
      }
      
      private function §_-iL§(param1:§_-63i§) : void
      {
         this.§_-vK§(AddToGroupResult(param1.message).result);
      }
      
      public function §_-43m§(param1:int, param2:§_-E19§, param3:uint, param4:Function) : void
      {
         this.§_-N1X§(new §_-yG§(param2,new §_-x2t§(param3,param3)),param4,this.§_-F1N§,[param1,param2]);
      }
      
      private function §_-F1N§(param1:int, param2:§_-E19§) : void
      {
         var _loc3_:Object = new BuyRace(param1,param2);
         §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc3_,BuyRaceResult,this.§_-l2H§,this.§_-f2z§);
      }
      
      private function §_-l2H§(param1:§_-63i§) : void
      {
         var _loc2_:BuyRaceResult = BuyRaceResult(param1.message);
         §_-N1V§.postController.§_-n2r§(_loc2_.race.§_-wz§());
         this.§_-vK§(_loc2_.result);
      }
      
      public function §_-b15§(param1:int, param2:§_-E19§, param3:uint, param4:Function) : void
      {
         this.§_-N1X§(new §_-yG§(param2,new §_-x2t§(param3,param3)),param4,this.§_-33F§,[param1]);
      }
      
      private function §_-33F§(param1:int) : void
      {
         var _loc2_:Object = new BuyUnlockMission(param1);
         §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc2_,BuyUnlockMissionResult,this.§_-Y1u§,this.§_-f2z§);
      }
      
      private function §_-Y1u§(param1:§_-63i§) : void
      {
         this.§_-vK§(BuyUnlockMissionResult(param1.message).result);
      }
      
      private function §_-N1X§(param1:§_-yG§, param2:Function, param3:Function, param4:Array = null) : void
      {
         if(§_-33r§.§_-Xj§(param1))
         {
            this.§_-TR§.push(param1);
            this.§_-417§.push(param2);
            param3.apply(null,param4);
         }
         else
         {
            param2.call(null,false);
         }
      }
      
      private function §_-f2z§(param1:Object = null, ... rest) : void
      {
         Starling.juggler.delayCall(this.§_-vK§,0.1,§_-8B§.§_-5X§);
      }
      
      private function §_-vK§(param1:int) : void
      {
         var _loc3_:§_-yG§ = null;
         var _loc4_:String = null;
         if(param1 == §_-8B§.SUCCESS)
         {
            _loc3_ = this.§_-TR§.shift() as §_-yG§;
            if(_loc3_)
            {
               Application.userProfile.§_-dE§(_loc3_.price,_loc3_.moneyType);
               if(_loc3_.moneyType == §_-E19§.§_-51D§)
               {
                  §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-b22§],[_loc3_.price.realPrice]);
               }
               else if(_loc3_.moneyType == §_-E19§.§_-i2d§)
               {
                  §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-T20§],[_loc3_.price.§_-R1T§]);
               }
            }
         }
         else
         {
            _loc4_ = "";
            switch(param1)
            {
               case §_-8B§.§_-Pf§:
                  _loc4_ = §_-s2a§.§_-K3t§(§_-s2a§.ERROR_ADVERTISING_LIMIT_REACHED);
                  break;
               case §_-8B§.NOT_ENOUGH_MONEY:
                  _loc4_ = §_-s2a§.§_-K3t§(§_-s2a§.ERROR_NOT_ENOUGH_MONEY);
                  break;
               default:
                  _loc4_ = §_-s2a§.§_-K3t§(§_-s2a§.ERROR_SAVING);
            }
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_BUY),_loc4_);
         }
         var _loc2_:Function = this.§_-417§.shift();
         if(_loc2_ != null)
         {
            _loc2_(param1 == §_-8B§.SUCCESS);
         }
         _loc3_ = null;
      }
   }
}

