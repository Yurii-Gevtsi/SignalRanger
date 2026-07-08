package ru.pragmatix.wormix.controller
{
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-73I§.§_-Y1l§;
   import §_-73I§.§_-q15§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-G16§.*;
   import §_-H2g§.§_-di§;
   import §_-S1N§.§_-R1U§;
   import §_-SP§.§_-M4§;
   import §_-Z1w§.§_-s1x§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-s20§.§_-J2g§;
   import §_-v2t§.§_-73F§;
   import com.greensock.OverwriteManager;
   import com.greensock.TweenLite;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import com.greensock.plugins.AutoAlphaPlugin;
   import com.greensock.plugins.BevelFilterPlugin;
   import com.greensock.plugins.BezierPlugin;
   import com.greensock.plugins.BezierThroughPlugin;
   import com.greensock.plugins.BlurFilterPlugin;
   import com.greensock.plugins.ColorMatrixFilterPlugin;
   import com.greensock.plugins.ColorTransformPlugin;
   import com.greensock.plugins.DropShadowFilterPlugin;
   import com.greensock.plugins.EndArrayPlugin;
   import com.greensock.plugins.FrameLabelPlugin;
   import com.greensock.plugins.FramePlugin;
   import com.greensock.plugins.GlowFilterPlugin;
   import com.greensock.plugins.HexColorsPlugin;
   import com.greensock.plugins.RemoveTintPlugin;
   import com.greensock.plugins.RoundPropsPlugin;
   import com.greensock.plugins.ShortRotationPlugin;
   import com.greensock.plugins.TintPlugin;
   import com.greensock.plugins.TweenPlugin;
   import com.greensock.plugins.VisiblePlugin;
   import com.greensock.plugins.VolumePlugin;
   import com.hurlant.util.der.§_-UO§;
   import config.§_-V2w§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Label;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.events.FeathersEventType;
   import feathers.layout.VerticalLayout;
   import flash.events.Event;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.response.photo.PhotoSaveResponse;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.analytics.Analytics;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.shop.BuyCraftBoxResultWindow;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.BuyVipResult;
   import ru.pragmatix.wormix.messages.server.IncomingPayment;
   import ru.pragmatix.wormix.messages.server.OpenDepositResult;
   import ru.pragmatix.wormix.messages.structures.AwardTypes;
   import ru.pragmatix.wormix.messages.structures.DepositStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Align;
   import starling.utils.ScaleMode;
   
   public class §_-33s§
   {
      
      public function §_-33s§()
      {
         super();
         §_-J2g§.§_-o1R§(BuyVipResult,this.§_-od§);
         §_-J2g§.§_-o1R§(OpenDepositResult,this.§_-Q1H§);
      }
      
      public function dispose() : void
      {
         §_-J2g§.§_-fy§(BuyVipResult,this.§_-od§);
         §_-J2g§.§_-fy§(OpenDepositResult,this.§_-Q1H§);
      }
      
      private function §_-od§(param1:§_-63i§) : void
      {
         var _loc2_:BuyVipResult = param1.message as BuyVipResult;
         Application.userProfile.build.§_-I28§(_loc2_.rentedItemsStructure);
         §_-X1j§.§_-G3m§.§_-B3h§ = _loc2_.renameAct;
      }
      
      private function §_-Q1H§(param1:§_-63i§) : void
      {
         var _loc5_:String = null;
         var _loc2_:OpenDepositResult = OpenDepositResult(param1.message);
         var _loc3_:DepositStructure = _loc2_.deposit;
         var _loc4_:int = _loc2_.firstPart;
         §_-X1j§.§_-Rr§.§_-11r§(_loc3_);
         if(_loc3_.moneyType == §_-E19§.§_-i2d§.id)
         {
            _loc5_ = §_-s2a§.COIN_QTIP;
            Application.userProfile.§_-oA§(_loc4_);
            §_-X1j§.§_-j14§.trackMoney(_loc4_,0,Analytics.BUY_VIRTUAL_CURRENCY);
         }
         else if(_loc3_.moneyType == §_-E19§.§_-51D§.id)
         {
            _loc5_ = §_-s2a§.RUBY_QTIP;
            Application.userProfile.§_-H3i§(_loc4_);
            §_-X1j§.§_-j14§.trackMoney(0,_loc4_,Analytics.BUY_VIRTUAL_CURRENCY);
         }
         var _loc6_:String = §_-s2a§.§_-K3t§(_loc5_).toLowerCase();
         var _loc7_:§_-fH§ = new §_-fH§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.DEPOSIT_BUY_SUCCESS),{
            "count":_loc2_.firstPart,
            "moneyType":_loc6_
         }),§_-fH§.§_-1B§);
         _loc7_.addEventListener(§_-M4§.WINDOW_CLOSE,this.§_-io§);
         _loc7_.show();
      }
      
      public function §_-T1h§(param1:IncomingPayment) : void
      {
         var _loc2_:§_-fH§ = null;
         if(param1.moneyType == §_-E19§.§_-i2d§.id)
         {
            Application.userProfile.§_-oA§(param1.count);
            §_-X1j§.§_-j14§.trackMoney(param1.count,0,Analytics.BUY_VIRTUAL_CURRENCY);
            _loc2_ = new §_-fH§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.BANK_INCOME_PAYMENT_COIN),{"count":param1.count}),§_-fH§.§_-1B§);
         }
         else if(param1.moneyType == §_-E19§.§_-51D§.id)
         {
            Application.userProfile.§_-H3i§(param1.count);
            §_-X1j§.§_-j14§.trackMoney(0,param1.count,Analytics.BUY_VIRTUAL_CURRENCY);
            _loc2_ = new §_-fH§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.BANK_INCOME_PAYMENT_RUBY),{"count":param1.count}),§_-fH§.§_-1B§);
         }
         _loc2_.addEventListener(§_-M4§.WINDOW_CLOSE,this.§_-io§);
         _loc2_.show();
      }
      
      public function §_-qp§(param1:IncomingPayment) : void
      {
         var _loc4_:§_-a3§ = null;
         var _loc2_:String = param1.item;
         var _loc3_:String = §_-N1V§.§_-42k§.§_-G1j§(_loc2_);
         for each(_loc4_ in §_-G1p§.§_-M1n§())
         {
            if(_loc4_.productId == _loc3_)
            {
               Application.userProfile.§_-p2c§().§_-Um§(_loc4_.getId());
               §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.NEW_STICKERS_PACK_PURCHASED));
               return;
            }
         }
      }
      
      public function §_-Vb§(param1:IncomingPayment) : void
      {
         var _loc2_:§_-W1r§ = null;
         var _loc4_:GenericAwardStructure = null;
         var _loc5_:BuyCraftBoxResultWindow = null;
         var _loc3_:UserProfile = Application.userProfile;
         for each(_loc4_ in param1.awards)
         {
            switch(_loc4_.awardKind)
            {
               case AwardTypes.MONEY:
                  _loc3_.§_-oA§(_loc4_.count);
                  §_-X1j§.§_-j14§.trackMoney(_loc4_.count,0,Analytics.BUY_VIRTUAL_CURRENCY);
                  break;
               case AwardTypes.REAL_MONEY:
                  _loc3_.§_-H3i§(_loc4_.count);
                  §_-X1j§.§_-j14§.trackMoney(0,_loc4_.count,Analytics.BUY_VIRTUAL_CURRENCY);
                  break;
               case AwardTypes.REAGENTS:
                  _loc2_ = §_-Y2u§.§_-k14§(_loc4_.itemId);
                  _loc3_.§_-11§(_loc2_,_loc4_.count);
                  break;
               case AwardTypes.STUFF:
                  if(§_-V2w§.§_-L2o§(_loc4_.itemId))
                  {
                     _loc2_ = §_-Y2u§.§_-k14§(_loc4_.itemId);
                     _loc3_.§_-11§(_loc2_,_loc4_.count);
                  }
            }
         }
         _loc5_ = new BuyCraftBoxResultWindow(param1.awards);
         _loc5_.show();
      }
      
      private function §_-io§(param1:starling.events.Event) : void
      {
         §_-X1j§.§_-zU§.§_-yI§();
      }
   }
}

