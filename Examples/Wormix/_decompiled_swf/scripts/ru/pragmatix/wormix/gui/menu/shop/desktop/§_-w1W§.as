package ru.pragmatix.wormix.gui.menu.shop.desktop
{
   import §_-12a§.§_-6p§;
   import §_-31W§.*;
   import §_-43V§.*;
   import §_-5Y§.ChatInputPanel;
   import §_-5Y§.StickerPanel;
   import §_-5Y§.§_-81g§;
   import §_-5Y§.§_-h2c§;
   import §_-5Y§.§_-t2O§;
   import §_-62§.*;
   import §_-63U§.§_-21K§;
   import §_-79§.§_-sd§;
   import §_-9§.§_-52A§;
   import §_-91A§.*;
   import §_-91B§.§_-z2l§;
   import §_-931§.§_-sz§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.*;
   import §_-B2z§.Server;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-c1G§;
   import §_-H3§.*;
   import §_-J31§.§_-R1h§;
   import §_-J31§.§_-e2J§;
   import §_-P1z§.*;
   import §_-T2h§.*;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-I3t§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-X14§.§_-J2b§;
   import §_-Y1O§.*;
   import §_-Y1b§.§_-81l§;
   import §_-YF§.§_-G2U§;
   import §_-YF§.§_-q2v§;
   import §_-Z15§.*;
   import §_-ZB§.§_-A10§;
   import §_-aM§.*;
   import §_-g2C§.§_-k1p§;
   import §_-g2C§.§_-p8§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-Rs§;
   import §_-lh§.§_-z2A§;
   import §_-q26§.§_-Z25§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-s20§.§_-6a§;
   import §_-s20§.§_-J2g§;
   import §_-z20§.§_-Tn§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.BigInteger;
   import config.§_-B1K§;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.controls.NumericStepper;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.HorizontalLayoutData;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.layout.VerticalLayoutData;
   import feathers.themes.styles.AlternateTextStyles;
   import feathers.utils.textures.TextureCache;
   import flash.display.*;
   import flash.display3D.*;
   import flash.errors.IllegalOperationError;
   import flash.events.*;
   import flash.geom.Point;
   import flash.utils.*;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.ShopItemRenderer;
   import ru.pragmatix.wormix.gui.menu.shop.§_-Vo§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-X1Y§;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.clans.response.ListClansResponse;
   import ru.pragmatix.wormix.messages.client.ReorderGroup;
   import ru.pragmatix.wormix.messages.server.EndTurnResponse;
   import ru.pragmatix.wormix.messages.structures.DailyBonusStructure;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.messages.structures.ProfileDoubleKeyStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.MolotovProjectile;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class §_-w1W§ extends §_-Vo§ implements §_-X1Y§
   {
      
      private const §_-91S§:int = 4;
      
      private const §_-93d§:int = 4;
      
      public function §_-w1W§()
      {
         super();
         §_-v1M§ = null;
      }
      
      override protected function §_-u1i§() : void
      {
         binder._itemsList.itemRendererFactory = this.§_-g1M§;
         (binder._itemsList.layout as TiledRowsLayout).verticalGap = §_-kV§() * 0.04;
         super.§_-u1i§();
      }
      
      override protected function §_-F14§() : void
      {
         binder._tabBar.dataProvider = new ListCollection([§_-s2a§.§_-K3t§(§_-s2a§.WEAPON_TAB),§_-s2a§.§_-K3t§(§_-s2a§.HATS),§_-s2a§.§_-K3t§(§_-s2a§.ARTIFACTS)]);
      }
      
      override protected function §_-Y1U§() : void
      {
         var _loc3_:int = 0;
         var _loc1_:Array = [];
         switch(binder._tabBar.selectedIndex)
         {
            case 0:
               _loc1_.push(§_-B1K§.§_-c1c§);
               _loc1_.push(§_-B1K§.§_-C23§);
               _loc1_.push(§_-B1K§.§_-A2L§);
               _loc1_.push(§_-B1K§.§_-x1w§);
               §_-v1M§ = DesktopShopWeaponInfoPanel;
               break;
            case 1:
               _loc1_.push(§_-B1K§.HAT);
               §_-v1M§ = DesktopShopEquipInfoPanel;
               break;
            case 2:
               _loc1_.push(§_-B1K§.ARTIFACT);
               §_-v1M§ = DesktopShopEquipInfoPanel;
         }
         var _loc2_:Array = [];
         for each(_loc3_ in _loc1_)
         {
            _loc2_ = _loc2_.concat(§_-G1p§.§_-O1b§(_loc3_,true));
         }
         §_-71p§();
         §_-B3H§();
         if(binder._itemsList.dataProvider)
         {
            binder._itemsList.dataProvider.removeAll();
            binder._itemsList.validate();
         }
         this.§_-t1p§(_loc2_);
      }
      
      override protected function §_-t1p§(param1:Array) : void
      {
         §_-ou§.§_-au§(param1);
         super.§_-t1p§(param1);
      }
      
      private function §_-g1M§() : IListItemRenderer
      {
         var _loc1_:ShopItemRenderer = null;
         _loc1_ = new ShopItemRenderer();
         _loc1_.width = §_-G3c§();
         _loc1_.height = §_-kV§() * 0.82;
         return _loc1_;
      }
      
      override protected function §_-03a§(param1:starling.events.Event) : void
      {
         §_-h2B§.play(§_-d27§.§_-F1z§);
         super.§_-03a§(param1);
      }
      
      override public function update(param1:int) : void
      {
         super.update(param1);
         info.setItem(binder._itemsList.selectedItem);
      }
      
      override protected function §_-t1n§() : Number
      {
         return this.§_-91S§;
      }
      
      override protected function §_-22Z§() : Number
      {
         return this.§_-93d§;
      }
      
      override public function dispose() : void
      {
         super.dispose();
      }
   }
}

