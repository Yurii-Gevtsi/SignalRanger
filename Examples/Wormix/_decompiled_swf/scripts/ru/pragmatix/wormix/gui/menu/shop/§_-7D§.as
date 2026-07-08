package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-12a§.§_-f1J§;
   import §_-62§.*;
   import §_-B1y§.§_-bs§;
   import §_-B1y§.§_-ts§;
   import §_-C3f§.§_-4I§;
   import §_-Cl§.*;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-YF§.§_-q2v§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-lD§;
   import §_-lh§.§_-z2A§;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import feathers.controls.Button;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import flash.display.DisplayObject;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   
   public class §_-7D§ extends §_-fH§
   {
      
      public function §_-7D§()
      {
         super(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_BUY),§_-s2a§.§_-K3t§(§_-s2a§.ERROR_NOT_ENOUGH_MONEY) + "<br/>" + §_-s2a§.§_-K3t§(§_-s2a§.GO_AT_BANK),§_-fH§.§_-X2g§,null,§_-d27§.§_-b2H§);
         (okBtn as Button).label = §_-s2a§.§_-K3t§(§_-s2a§.BUTTON_GO_AT_BANK);
      }
      
      override protected function §_-O2y§() : void
      {
         super.§_-O2y§();
         §_-xQ§(§_-s2a§.BUTTON_CANCEL);
         §_-u2m§ = §_-NY§.§_-w1y§;
         soundOnShow = §_-d27§.§_-b2H§;
         destroyOnRemove = true;
      }
      
      override protected function §_-b1H§() : void
      {
         hide();
         §_-N1V§.§_-42k§.§_-B1f§();
      }
   }
}

