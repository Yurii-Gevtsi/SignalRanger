package ru.pragmatix.wormix.gui.menu.clans.chat
{
   import §_-DJ§.MessageBox;
   import §_-Vg§.*;
   import §_-jF§.ClanInviteRenderer;
   import §_-r1C§.§_-h2B§;
   import §_-x2Q§.§_-dU§;
   import feathers.controls.Button;
   import feathers.controls.List;
   import feathers.controls.Screen;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ListCollection;
   import feathers.data.VectorCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import starling.events.Event;
   
   public class NotInClanChatScreen extends Screen
   {
      
      public static const §_-u2h§:String = "search-clan";
      
      public static const §_-L1L§:String = "create-clan";
      
      public static const §_-91u§:String = "clan-invite-selected";
      
      private var binder:Binder;
      
      private var invites:Vector.<ClanInviteStructure>;
      
      public function NotInClanChatScreen()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         var invitesLayout:VerticalLayout;
         super.initialize();
         this.binder = new Binder();
         Application.uiBuilder.create(Application.assetManager.getObject("not_in_clan_view"),true,this.binder);
         addChild(this.binder._panel);
         this.binder._controls.buttonProperties.styleName = AlternateButtonsStyles.BUTTON_GREEN;
         this.binder._controls.buttonProperties.width = 200;
         this.binder._controls.dataProvider = new ListCollection([{
            "stringId":§_-s2a§.TAB_SEARCH_CLAN,
            "triggered":this.§_-5l§
         },{
            "stringId":§_-s2a§.TAB_CREATE_CLAN,
            "triggered":this.§_-Pt§
         }]);
         this.binder._controls.buttonInitializer = function(param1:Button, param2:Object):void
         {
            param1.label = §_-s2a§.§_-K3t§(param2.stringId);
            param1.addEventListener(Event.TRIGGERED,param2.triggered);
         };
         this.binder._controls.buttonReleaser = function(param1:Button, param2:Object):void
         {
            param1.label = "";
            param1.removeEventListener(Event.TRIGGERED,param2.triggered);
         };
         this.binder._controls.buttonProperties.scale = 1;
         this.binder._controls.gap = 10;
         this.binder._controls.padding = 20;
         this.binder._controls.horizontalAlign = HorizontalAlign.CENTER;
         this.binder._controls.invalidate();
         invitesLayout = new VerticalLayout();
         invitesLayout.gap = 5;
         invitesLayout.horizontalAlign = HorizontalAlign.LEFT;
         invitesLayout.verticalAlign = VerticalAlign.TOP;
         invitesLayout.hasVariableItemDimensions = true;
         invitesLayout.padding = 5;
         this.binder._invitesList.layout = invitesLayout;
         this.binder._invitesList.horizontalScrollPolicy = ScrollPolicy.OFF;
         this.binder._invitesList.itemRendererType = ClanInviteRenderer;
         this.binder._invitesList.addEventListener(Event.CHANGE,this.§_-13H§);
         this.binder._invitesList.customVerticalScrollBarStyleName = AlternateScrollBarStyles.SIMPLE_NORMAL;
         this.binder._controlPanel.invalidate();
         this.binder._panel.readjustLayout();
      }
      
      public function setData(param1:Vector.<ClanInviteStructure>) : void
      {
         this.invites = param1.concat();
         invalidate(INVALIDATION_FLAG_DATA);
      }
      
      private function §_-13H§(param1:Event) : void
      {
         var _loc2_:List = param1.currentTarget as List;
         if(Boolean(_loc2_) && _loc2_.selectedIndex != -1)
         {
            §_-h2B§.play(§_-d27§.§_-S29§);
            dispatchEventWith(§_-91u§,false,_loc2_.selectedItem);
            _loc2_.selectedIndex = -1;
         }
      }
      
      private function §_-Pt§(param1:Event) : void
      {
         dispatchEventWith(§_-L1L§);
      }
      
      private function §_-5l§(param1:Event) : void
      {
         dispatchEventWith(§_-u2h§);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         if(this.binder)
         {
            this.binder._invitesList.removeEventListener(Event.CHANGE,this.§_-13H§);
         }
         this.binder = null;
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(isInvalid(§_-dU§.§_-h26§))
         {
            this.binder._title.text = §_-s2a§.§_-K3t§(§_-s2a§.NOT_IN_CLAN);
            this.binder._messageTitle.text = §_-s2a§.§_-K3t§(§_-s2a§.CLAN_CHAT_VELCOME);
            this.binder._message.text = §_-s2a§.§_-K3t§(§_-s2a§.CLAN_CHAT_OFFER_CREATE);
            this.binder._controls.invalidate(INVALIDATION_FLAG_DATA);
            if(this.binder._invitesList.dataProvider)
            {
               this.binder._invitesList.dataProvider.updateAll();
            }
         }
         if(isInvalid(INVALIDATION_FLAG_SIZE) && Boolean(_owner))
         {
            this.binder._panel.width = _owner.width;
            this.binder._panel.height = _owner.height;
            this.binder._invitesList.invalidate(INVALIDATION_FLAG_SIZE);
         }
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            this.binder._invitesList.dataProvider = new VectorCollection(this.invites);
            this.binder._invitesList.verticalScrollPolicy = ScrollPolicy.AUTO;
         }
      }
      
      override public function set owner(param1:Object) : void
      {
         super.owner = param1;
         invalidate(INVALIDATION_FLAG_SIZE);
      }
   }
}

import feathers.controls.ButtonGroup;
import feathers.controls.Label;
import feathers.controls.LayoutGroup;
import starlingbuilder.extensions.uicomponents.CustomList;

class Binder
{
   
   public var _panel:LayoutGroup;
   
   public var _controlPanel:LayoutGroup;
   
   public var _title:Label;
   
   public var _messageTitle:Label;
   
   public var _message:Label;
   
   public var _controls:ButtonGroup;
   
   public var _invitesList:CustomList;
   
   public function Binder()
   {
      super();
   }
}
