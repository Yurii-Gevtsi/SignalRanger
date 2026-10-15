package ru.pragmatix.wormix.gui.menu.clans.chat
{
   import §_-134§.§_-c11§;
   import §_-5Y§.ChatInputPanel;
   import §_-5Y§.StickerPanel;
   import §_-5Y§.§_-81g§;
   import §_-5Y§.§_-h2c§;
   import §_-5Y§.§_-t2O§;
   import §_-x2Q§.§_-dU§;
   import feathers.controls.Screen;
   import feathers.layout.VerticalLayout;
   import feathers.layout.VerticalLayoutData;
   import feathers.utils.textures.TextureCache;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class §_-D2t§ extends Screen
   {
      
      public static const §_-M2p§:String = "send-news";
      
      public static const §_-kZ§:String = "show-clan-info";
      
      public static const SEND_MESSAGE:String = "send-message";
      
      public static const SEND_STICKER:String = "send-sticker";
      
      private var §_-71O§:ClanInfoPanel;
      
      private var §_-21l§:§_-c11§;
      
      private var _inputPanel:ChatInputPanel;
      
      private var §_-M2B§:StickerPanel;
      
      private var §_-K3w§:ClanNewsTO;
      
      private var §_-11Q§:§_-81g§;
      
      public function §_-D2t§()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         var _loc1_:Object = Application.assetManager.getObject("clan_info_panel");
         var _loc2_:Sprite = Application.uiBuilder.create(_loc1_) as Sprite;
         this.§_-71O§ = _loc2_.getChildByName("clanInfoPanel") as ClanInfoPanel;
         this.§_-71O§.addEventListener(Event.TRIGGERED,this.§_-NO§);
         addChild(this.§_-71O§);
         var _loc3_:TextureCache = §_-g20§.§_-W2i§();
         this.§_-21l§ = new §_-c11§([§_-h2c§.CLAN_CHAT],§_-h2c§.CLAN_CHAT,_loc3_);
         addChild(this.§_-21l§);
         this._inputPanel = new ChatInputPanel(§_-h2c§.CLAN_CHAT);
         this._inputPanel.addEventListener(§_-t2O§.SEND_MESSAGE,this.§_-ek§);
         addChild(this._inputPanel);
         this.§_-M2B§ = new StickerPanel(_loc3_);
         this.§_-M2B§.addEventListener(§_-t2O§.SEND_STICKER,this.§_-m2Y§);
         addChild(this.§_-M2B§);
         var _loc4_:VerticalLayout = new VerticalLayout();
         _loc4_.gap = 2;
         this.layout = _loc4_;
         this.§_-71O§.layoutData = new VerticalLayoutData(100);
         this.§_-21l§.layoutData = new VerticalLayoutData(100,100);
         this._inputPanel.layoutData = new VerticalLayoutData(100);
         this.§_-M2B§.layoutData = new VerticalLayoutData(100);
         this.§_-M2B§.height = 0;
         this.§_-11Q§ = new §_-81g§(this._inputPanel,this.§_-M2B§);
      }
      
      public function setData(param1:ClanTO, param2:UserProfile) : void
      {
         this.§_-71O§.setData(param1,param2);
      }
      
      public function update(param1:Vector.<ClanChatMessageTO>, param2:ClanNewsTO) : void
      {
         var _loc4_:ClanChatMessageTO = null;
         var _loc3_:Array = [];
         this.§_-K3w§ = param2;
         if(param2)
         {
            _loc3_.push(param2);
         }
         for each(_loc4_ in param1)
         {
            _loc3_.push(_loc4_);
         }
         this.§_-21l§.§_-C2Q§(§_-h2c§.CLAN_CHAT,_loc3_);
      }
      
      private function §_-NO§(param1:Event) : void
      {
         var _loc2_:String = param1.data as String;
         if(_loc2_ == §_-M2p§)
         {
            this.§_-Pb§(param1);
         }
         else if(_loc2_ == §_-kZ§)
         {
            this.§_-Ge§(param1);
         }
      }
      
      public function §_-o2B§(param1:ClanChatMessageTO) : void
      {
         this.§_-21l§.§_-t2Y§(param1,§_-h2c§.CLAN_CHAT);
      }
      
      public function §_-x1S§(param1:ClanNewsTO) : void
      {
         if(this.§_-K3w§)
         {
            this.§_-21l§.§_-tL§(this.§_-K3w§,§_-h2c§.CLAN_CHAT);
         }
         this.§_-K3w§ = param1;
         if(this.§_-K3w§)
         {
            this.§_-21l§.§_-t2Y§(this.§_-K3w§,§_-h2c§.CLAN_CHAT,0);
         }
      }
      
      private function §_-ek§(param1:Event) : void
      {
         dispatchEventWith(SEND_MESSAGE,false,param1.data);
      }
      
      private function §_-m2Y§(param1:Event) : void
      {
         dispatchEventWith(SEND_STICKER,false,param1.data);
      }
      
      private function §_-Ge§(param1:Event) : void
      {
         dispatchEventWith(§_-kZ§);
      }
      
      private function §_-Pb§(param1:Event) : void
      {
         dispatchEventWith(§_-M2p§);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         if(this._inputPanel)
         {
            this._inputPanel.removeEventListener(§_-t2O§.SEND_MESSAGE,this.§_-ek§);
            this._inputPanel.dispose();
            this._inputPanel = null;
         }
         if(this.§_-11Q§)
         {
            this.§_-11Q§.dispose();
            this.§_-11Q§ = null;
         }
         if(this.§_-21l§)
         {
            this.§_-21l§.dispose();
            this.§_-21l§ = null;
         }
         if(this.§_-M2B§)
         {
            this.§_-M2B§.removeEventListener(§_-t2O§.SEND_STICKER,this.§_-m2Y§);
            this.§_-M2B§.dispose();
            this.§_-M2B§ = null;
         }
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(isInvalid(§_-dU§.§_-h26§))
         {
            this.§_-21l§.invalidate(§_-dU§.§_-h26§);
         }
      }
   }
}

