package ru.pragmatix.wormix.gui.menu.shop.desktop
{
   import §_-03T§.§_-2p§;
   import §_-Y1O§.§_-V23§;
   import §_-b1F§.§_-G4§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-zI§.§_-X1q§;
   import feathers.controls.List;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.FeathersControl;
   import flash.display.StageOrientation;
   import flash.events.StageOrientationEvent;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.BuyVipResult;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   import starling.utils.MathUtil;
   
   public class DesktopShopnfoRequirementRenderer extends DefaultListItemRenderer
   {
      
      private var binder:Binder;
      
      public function DesktopShopnfoRequirementRenderer()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.binder = new Binder();
         Application.uiBuilder.create(Application.assetManager.getObject("shop_info_requirements_renderer"),true,this.binder);
         addChild(this.binder._panel);
      }
      
      override protected function commitData() : void
      {
         if(!data)
         {
            return;
         }
         this.binder._text.text = data.toString();
         this.binder._text.validate();
         this.binder._panel.validate();
         this.height = this.binder._panel.height;
      }
      
      override public function set owner(param1:List) : void
      {
         super.owner = param1;
         if(owner)
         {
            this.binder._panel.width = owner.width;
            this.width = owner.width;
         }
      }
   }
}

import feathers.controls.Label;
import feathers.controls.LayoutGroup;

class Binder
{
   
   public var _panel:LayoutGroup;
   
   public var _text:Label;
   
   public function Binder()
   {
      super();
   }
}
