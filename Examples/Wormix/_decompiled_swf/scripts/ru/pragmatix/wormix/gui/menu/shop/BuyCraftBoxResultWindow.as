package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-73y§;
   import §_-L2F§.§_-D2p§;
   import §_-Y1b§.§_-OP§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-jF§.§_-lx§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-e1k§;
   import §_-k2s§.§_-iI§;
   import config.§_-V2w§;
   import feathers.controls.Button;
   import feathers.controls.List;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.ITextRenderer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.AwardTypes;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   
   public class BuyCraftBoxResultWindow extends §_-73y§
   {
      
      private var binder:Binder;
      
      public function BuyCraftBoxResultWindow(param1:Array = null)
      {
         var _loc2_:GenericAwardStructure = null;
         var _loc3_:int = 0;
         var _loc4_:§_-8c§ = null;
         var _loc5_:DefaultListItemRenderer = null;
         this.binder = new Binder();
         Application.uiBuilder.create(Application.assetManager.getObject("buy_craft_box_result_window"),true,this.binder);
         for each(_loc2_ in param1)
         {
            _loc3_ = _loc2_.itemId;
            switch(_loc2_.awardKind)
            {
               case AwardTypes.MONEY:
                  _loc4_ = new §_-iI§(§_-E19§.§_-i2d§,_loc2_.count);
                  break;
               case AwardTypes.REAL_MONEY:
                  _loc4_ = new §_-iI§(§_-E19§.§_-51D§,_loc2_.count);
                  break;
               case AwardTypes.REAGENTS:
                  _loc4_ = new §_-e1k§(_loc3_,_loc2_.count);
                  break;
               case AwardTypes.STUFF:
                  if(§_-V2w§.§_-L2o§(_loc3_))
                  {
                     _loc4_ = new §_-e1k§(_loc3_,_loc2_.count);
                  }
                  else
                  {
                     _loc4_ = new §_-e1U§(_loc3_);
                  }
            }
            if(_loc4_)
            {
               _loc5_ = new §_-lx§();
               _loc5_.data = _loc4_;
               this.binder._icons.addChild(_loc5_);
            }
         }
         this.binder._content.height = NaN;
         this.binder._panel.height = NaN;
         super(this.binder._panel);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this.binder = null;
      }
      
      override protected function §_-O2y§() : void
      {
         super.§_-O2y§();
         §_-PQ§ = "closeButtonContainer.closeBtn";
         §_-rP§.name = "_content.buttons.okBtn";
         this.binder._title.text = §_-s2a§.§_-K3t§(§_-s2a§.TITLE_BUY);
         this.binder._msg.text = §_-s2a§.§_-K3t§(§_-s2a§.CRAFT_BOX_BUY_RESULT);
         destroyOnRemove = true;
      }
   }
}

import feathers.controls.Label;
import feathers.controls.LayoutGroup;

class Binder
{
   
   public var _title:Label;
   
   public var _msg:Label;
   
   public var _panel:LayoutGroup;
   
   public var _content:LayoutGroup;
   
   public var _icons:LayoutGroup;
   
   public function Binder()
   {
      super();
   }
}
