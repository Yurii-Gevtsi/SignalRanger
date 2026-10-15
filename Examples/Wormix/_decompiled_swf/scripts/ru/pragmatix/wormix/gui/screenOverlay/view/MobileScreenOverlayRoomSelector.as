package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-82l§.§_-A2Y§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-G2H§.*;
   import §_-Ly§.§_-81L§;
   import §_-Tm§.§_-RF§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2W§.§_-21w§;
   import §_-x2Q§.§_-dU§;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.core.PopUpManager;
   import feathers.core.ToggleGroup;
   import feathers.layout.AnchorLayoutData;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import starling.animation.Transitions;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Quad;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class MobileScreenOverlayRoomSelector extends LayoutGroup implements §_-n2N§, §_-o2R§
   {
      
      private static const §_-k2u§:int = 200;
      
      private static const §_-W1Z§:int = -1;
      
      private static const STATE_SHOWN:int = 0;
      
      private static const §_-w2Y§:int = 1;
      
      private static const §_-F10§:int = 2;
      
      private var battleButton:ContainerButtonWithStates;
      
      private var toggleGroup:ToggleGroup;
      
      private var §_-J22§:MobileScreenOverlayNavButton;
      
      private var §_-B3c§:Number;
      
      private var §_-N1M§:String;
      
      private var §_-62x§:String;
      
      private var _data:§_-r1d§;
      
      private var backBtnDelayCallId:uint;
      
      private var backBtnState:int = 0;
      
      public function MobileScreenOverlayRoomSelector()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         var _loc2_:MobileScreenOverlayNavButton = null;
         super.initialize();
         this.toggleGroup = new ToggleGroup();
         this.toggleGroup.isSelectionRequired = true;
         this.toggleGroup.addEventListener(Event.CHANGE,this.§_-h1c§);
         this.battleButton = getChildByName("battleButton") as ContainerButtonWithStates;
         this.battleButton.addEventListener(Event.TRIGGERED,this.§_-82i§);
         var _loc1_:Dictionary = new Dictionary();
         _loc1_["navBtn_home"] = this.§_-i1w§(§_-21w§.§_-G31§,"base_overlay_icon_home");
         _loc1_["navBtn_armory"] = this.§_-i1w§(§_-21w§.§_-F3d§,"base_overlay_icon_arsenal");
         _loc1_["navBtn_team"] = this.§_-i1w§(§_-21w§.§_-H2E§,"base_overlay_icon_team");
         _loc1_["navBtn_wardrobe"] = this.§_-i1w§(§_-21w§.§_-G2M§,"base_overlay_icon_wardrobe");
         _loc1_["navBtn_race"] = this.§_-i1w§(§_-21w§.§_-jc§,"base_overlay_icon_race");
         _loc1_["navBtn_achieve"] = this.§_-i1w§(§_-21w§.§_-l1e§,"base_overlay_icon_achieve");
         _loc1_["navBtn_craft"] = this.§_-i1w§(§_-21w§.§_-43d§,"base_overlay_icon_craft");
         var _loc3_:int = 0;
         while(_loc3_ < numChildren)
         {
            _loc2_ = getChildAt(_loc3_) as MobileScreenOverlayNavButton;
            if(_loc2_)
            {
               _loc2_.data = _loc1_[_loc2_.name];
               this.toggleGroup.addItem(_loc2_);
               if(_loc2_.name == "navBtn_home")
               {
                  this.§_-J22§ = _loc2_;
                  this.§_-B3c§ = this.§_-J22§.y;
                  this.§_-J22§.visible = false;
                  this.§_-J22§.y = this.§_-B3c§ + §_-k2u§;
               }
            }
            _loc3_++;
         }
         var _loc4_:LayoutGroup = parent as LayoutGroup;
         var _loc5_:AnchorLayoutData = _loc4_.layoutData as AnchorLayoutData;
      }
      
      override protected function draw() : void
      {
         var text:String = null;
         var selectedIndex:int = 0;
         var i:int = 0;
         var backBtnVisibleExpected:Boolean = false;
         var isBackBtnShouldShow:Boolean = false;
         var isBackBtnShouldHide:Boolean = false;
         var hideTweenDelay:Number = NaN;
         var button:MobileScreenOverlayNavButton = null;
         var tabData:§_-B1m§ = null;
         var raceRoomBtn:MobileScreenOverlayNavButton = null;
         var homeRoom:MobileScreenOverlayNavButton = null;
         var iconName:String = null;
         super.draw();
         if(isInvalid(§_-dU§.§_-h26§))
         {
            text = §_-s2a§.§_-K3t§(§_-s2a§.BATTLE_BOSS_BTN);
            this.battleButton.setTextInSkins("label",text);
         }
         if(isInvalid(INVALIDATION_FLAG_SELECTED))
         {
            if(!this.§_-J22§)
            {
               return;
            }
            selectedIndex = -1;
            i = 0;
            while(i < this.toggleGroup.numItems)
            {
               button = this.toggleGroup.getItemAt(i) as MobileScreenOverlayNavButton;
               if(button)
               {
                  tabData = button.data;
                  if(tabData.screenId == this.§_-62x§)
                  {
                     selectedIndex = i;
                     break;
                  }
               }
               i++;
            }
            if(selectedIndex > -1)
            {
               this.toggleGroup.selectedIndex = selectedIndex;
            }
            backBtnVisibleExpected = this.§_-62x§ != §_-21w§.§_-G31§;
            isBackBtnShouldShow = this.§_-N1M§ == §_-21w§.§_-G31§ && this.§_-62x§ != §_-21w§.§_-G31§;
            isBackBtnShouldHide = this.§_-N1M§ != §_-21w§.§_-G31§ && this.§_-62x§ == §_-21w§.§_-G31§;
            hideTweenDelay = 0.5;
            if(isBackBtnShouldHide)
            {
               if(!backBtnVisibleExpected && this.§_-J22§.visible && this.backBtnState != §_-F10§)
               {
                  this.backBtnState = §_-F10§;
                  Starling.juggler.removeTweens(this.§_-J22§);
                  Starling.juggler.tween(this.§_-J22§,hideTweenDelay,{
                     "y":this.§_-B3c§ + §_-k2u§,
                     "transition":Transitions.EASE_IN_BACK
                  });
                  this.backBtnDelayCallId = Starling.juggler.delayCall(function():void
                  {
                     §_-J22§.visible = false;
                     backBtnState = §_-W1Z§;
                  },hideTweenDelay);
               }
            }
            else if(isBackBtnShouldShow)
            {
               if(backBtnVisibleExpected && (!this.§_-J22§.visible || this.backBtnState != §_-w2Y§))
               {
                  this.backBtnState = §_-w2Y§;
                  Starling.juggler.removeTweens(this.§_-J22§);
                  Starling.juggler.removeByID(this.backBtnDelayCallId);
                  this.§_-J22§.y = this.§_-B3c§ + §_-k2u§;
                  this.§_-J22§.visible = true;
                  Starling.juggler.tween(this.§_-J22§,hideTweenDelay,{
                     "y":this.§_-B3c§,
                     "transition":Transitions.EASE_OUT_BACK
                  });
                  this.backBtnDelayCallId = Starling.juggler.delayCall(function():void
                  {
                     backBtnState = STATE_SHOWN;
                     backBtnDelayCallId = 0;
                  },hideTweenDelay);
               }
            }
         }
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            if(this._data)
            {
               raceRoomBtn = this.toggleGroup.getItemAt(4) as MobileScreenOverlayNavButton;
               if(raceRoomBtn)
               {
                  raceRoomBtn.isEnabled = this._data.§_-x2k§;
                  raceRoomBtn.alpha = this._data.§_-x2k§ ? 1 : 0.5;
               }
               homeRoom = this.toggleGroup.getItemAt(0) as MobileScreenOverlayNavButton;
               if(homeRoom)
               {
                  iconName = this._data.§_-x2k§ ? "base_overlay_icon_home" : "base_overlay_leave";
                  homeRoom.data = this.§_-i1w§(§_-21w§.§_-G31§,iconName);
               }
            }
         }
      }
      
      private function §_-82i§(param1:Event) : void
      {
         param1.stopPropagation();
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         dispatchEventWith(Event.TRIGGERED);
      }
      
      private function §_-h1c§(param1:Event) : void
      {
         var _loc3_:§_-B1m§ = null;
         if(this.toggleGroup.selectedIndex != -1)
         {
            §_-h2B§.play(§_-81L§.§_-o1Z§);
         }
         var _loc2_:MobileScreenOverlayNavButton = this.toggleGroup.selectedItem as MobileScreenOverlayNavButton;
         if(_loc2_)
         {
            _loc3_ = _loc2_.data as §_-B1m§;
            if(_loc3_)
            {
               dispatchEventWith(Event.CHANGE,false,_loc3_.screenId);
            }
         }
      }
      
      public function §_-b1j§(param1:String) : void
      {
         this.§_-N1M§ = this.§_-62x§;
         this.§_-62x§ = param1;
         invalidate(INVALIDATION_FLAG_SELECTED);
      }
      
      public function setData(param1:§_-r1d§) : void
      {
         this._data = param1;
         invalidate(INVALIDATION_FLAG_DATA);
      }
      
      private function §_-i1w§(param1:String, param2:String) : §_-B1m§
      {
         return new §_-B1m§(param1,param2);
      }
   }
}

