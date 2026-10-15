package ru.pragmatix.wormix.gui.menu.clans.chat
{
   import §_-Vg§.§_-M11§;
   import §_-zI§.§_-X1q§;
   import feathers.controls.Button;
   import feathers.controls.ButtonGroup;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.data.ArrayCollection;
   import feathers.layout.Direction;
   import feathers.layout.VerticalAlign;
   import feathers.skins.ImageSkin;
   import feathers.themes.styles.AlternateButtonsStyles;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import starling.events.Event;
   import starling.textures.Texture;
   
   public class ClanInfoPanel extends LayoutGroup
   {
      
      private var clanEmblemContainer:LayoutGroup;
      
      private var §_-F3E§:Label;
      
      private var buttonGroup:ButtonGroup;
      
      private var §_-f10§:§_-M11§;
      
      private var §_-93m§:ClanTO;
      
      private var §_-z2g§:UserProfile;
      
      public function ClanInfoPanel()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.clanEmblemContainer = getChildByName("clanEmblemContainer") as LayoutGroup;
         this.§_-F3E§ = getChildByName("clanName") as Label;
         this.buttonGroup = getChildByName("buttonGroup") as ButtonGroup;
         this.§_-f10§ = new §_-M11§();
         this.§_-f10§.size = this.clanEmblemContainer.height;
         this.clanEmblemContainer.addChild(this.§_-f10§);
         this.§_-F3E§.text = "";
         this.buttonGroup.verticalAlign = VerticalAlign.MIDDLE;
         this.buttonGroup.direction = Direction.HORIZONTAL;
         this.buttonGroup.distributeButtonSizes = false;
         this.buttonGroup.gap = 10;
         this.buttonGroup.buttonProperties.styleName = AlternateButtonsStyles.BUTTON_QUAD_GREY;
         this.buttonGroup.buttonProperties.width = 50;
         this.buttonGroup.buttonProperties.height = 50;
         this.buttonGroup.buttonInitializer = function(param1:Button, param2:Object):void
         {
            var _loc3_:Texture = Application.assetManager.getTexture(param2.icon);
            var _loc4_:ImageSkin = new ImageSkin(_loc3_);
            _loc4_.width = 25;
            _loc4_.height = 25;
            param1.defaultIcon = _loc4_;
         };
         this.buttonGroup.addEventListener(Event.TRIGGERED,this.§_-y1l§);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this.buttonGroup.removeEventListener(Event.TRIGGERED,this.§_-y1l§);
      }
      
      public function setData(param1:ClanTO, param2:UserProfile) : void
      {
         this.§_-93m§ = param1;
         this.§_-z2g§ = param2;
         invalidate(INVALIDATION_FLAG_DATA);
      }
      
      override protected function draw() : void
      {
         var _loc1_:String = null;
         var _loc2_:Vector.<uint> = null;
         var _loc3_:Array = null;
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            _loc1_ = "";
            _loc2_ = null;
            _loc3_ = [];
            if(this.§_-93m§)
            {
               _loc1_ = this.§_-93m§.name;
               _loc2_ = this.§_-93m§.emblem;
               if(this.§_-93m§.getRank(this.§_-z2g§.getId()) != §_-X1q§.§_-CM§)
               {
                  _loc3_.push({
                     "icon":"icon_note_new",
                     "event_type":§_-D2t§.§_-M2p§
                  });
               }
               _loc3_.push({
                  "icon":"icon_info_new",
                  "event_type":§_-D2t§.§_-kZ§
               });
            }
            this.§_-F3E§.text = _loc1_;
            this.§_-f10§.§_-6B§ = _loc2_;
            this.buttonGroup.dataProvider = new ArrayCollection(_loc3_);
         }
      }
      
      private function §_-y1l§(param1:Event) : void
      {
         dispatchEventWith(Event.TRIGGERED,false,param1.data.event_type);
      }
   }
}

