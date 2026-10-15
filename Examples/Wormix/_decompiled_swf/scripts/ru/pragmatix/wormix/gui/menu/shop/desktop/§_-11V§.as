package ru.pragmatix.wormix.gui.menu.shop.desktop
{
   import §_-21p§.§_-4w§;
   import §_-73d§.§_-V19§;
   import §_-TF§.§_-F1B§;
   import §_-TF§.§_-q1j§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-p24§.*;
   import §_-r1C§.§_-h2B§;
   import com.distriqt.extension.inappbilling.PurchaseRequest;
   import com.hurlant.math.*;
   import feathers.controls.LayoutGroup;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.HorizontalLayoutData;
   import feathers.layout.VerticalAlign;
   import flash.text.TextFormatAlign;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.gui.menu.§_-k25§;
   import ru.pragmatix.wormix.messages.clans.*;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.text.TextFormat;
   import starling.textures.Texture;
   import starling.utils.Align;
   import starling.utils.Color;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   use namespace bi_internal;
   
   public class §_-11V§ extends §_-k25§
   {
      
      private const §_-r2U§:Number = 50;
      
      private const §_-Y2§:String = "BuyHatDialog_cellsContainer_cells_template";
      
      public function §_-11V§(param1:DisplayObjectContainer)
      {
         super(param1);
      }
      
      override protected function initialize() : void
      {
         _itemRendererFactory = this.§_-zT§;
         super.initialize();
      }
      
      private function §_-zT§() : IListItemRenderer
      {
         var downSkin:Sprite;
         var downImage:Image;
         var result:DefaultListItemRenderer = null;
         result = new DefaultListItemRenderer();
         result.labelFactory = function():ITextRenderer
         {
            return new TextFieldTextRenderer();
         };
         result.fontStyles = new TextFormat(BitmapFonts.AD_LIB,22,8089696,Align.CENTER);
         result.selectedFontStyles = new TextFormat(BitmapFonts.AD_LIB,22,11493202,Align.CENTER);
         downSkin = new Sprite();
         downSkin.addChild(new Image(Texture.empty(this.§_-r2U§,this.§_-r2U§)));
         downImage = new Image(Application.assetManager.getTexture(this.§_-Y2§));
         downImage.width = downImage.height = this.§_-r2U§;
         downSkin.addChild(downImage);
         result.defaultSelectedSkin = downSkin;
         result.defaultSkin = new Image(Texture.empty(this.§_-r2U§,this.§_-r2U§));
         return result;
      }
   }
}

