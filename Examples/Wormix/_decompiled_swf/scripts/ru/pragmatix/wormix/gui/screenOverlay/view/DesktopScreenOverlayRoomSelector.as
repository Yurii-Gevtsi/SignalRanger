package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-Ly§.§_-81L§;
   import §_-r1C§.§_-h2B§;
   import §_-w2W§.§_-21w§;
   import feathers.controls.LayoutGroup;
   import feathers.core.ToggleGroup;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class DesktopScreenOverlayRoomSelector extends LayoutGroup implements §_-o2R§, §_-n2N§
   {
      
      private var §_-OD§:Array;
      
      private var _toggleGroup:ToggleGroup;
      
      private var _data:§_-r1d§;
      
      private var §_-62x§:String;
      
      public function DesktopScreenOverlayRoomSelector()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         var _loc1_:int = 0;
         var _loc3_:DisplayObject = null;
         var _loc4_:§_-B1m§ = null;
         super.initialize();
         this.§_-OD§ = [];
         this.§_-OD§.push(new §_-B1m§(§_-21w§.§_-G31§,"home",§_-s2a§.HOME_QTIP));
         this.§_-OD§.push(new §_-B1m§(§_-21w§.§_-F3d§,"arsenal",§_-s2a§.ARMORY_QTIP));
         this.§_-OD§.push(new §_-B1m§(§_-21w§.§_-H2E§,"team",§_-s2a§.TEAM_QTIP));
         this.§_-OD§.push(new §_-B1m§(§_-21w§.§_-G2M§,"wardrobe",§_-s2a§.WARDROBE_QTIP));
         this.§_-OD§.push(new §_-B1m§(§_-21w§.§_-jc§,"race",§_-s2a§.RACE_QTIP));
         this.§_-OD§.push(new §_-B1m§(§_-21w§.§_-l1e§,"achieve",§_-s2a§.ACHIEV_QTIP));
         this.§_-OD§.push(new §_-B1m§(§_-21w§.§_-43d§,"craft",§_-s2a§.CRAFT_QTIP));
         var _loc2_:int = numChildren;
         _loc1_ = 0;
         while(_loc1_ < _loc2_)
         {
            _loc3_ = getChildAt(_loc1_);
            _loc4_ = this.§_-OD§[_loc1_];
            _loc4_.§_-KA§ = _loc3_.x;
            _loc4_.§_-cq§ = _loc3_.y;
            _loc1_++;
         }
         removeChildren(0,-1,true);
         this._toggleGroup = new ToggleGroup();
         this._toggleGroup.isSelectionRequired = true;
      }
      
      override protected function draw() : void
      {
         var _loc1_:int = 0;
         var _loc2_:§_-B1m§ = null;
         var _loc3_:§_-41v§ = null;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            _loc1_ = this._toggleGroup.selectedIndex;
            this._toggleGroup.removeEventListener(Event.CHANGE,this.§_-13V§);
            this._toggleGroup.removeAllItems();
            if(this._data)
            {
               this.§_-OD§[0].iconName = this._data.§_-x2k§ ? "home" : "leave";
               this.§_-OD§[0].normalColor = this._data.§_-x2k§ ? true : false;
               this.§_-OD§[4].isEnabled = this._data.§_-x2k§;
               _loc4_ = 0;
               while(_loc4_ < this.§_-OD§.length)
               {
                  _loc2_ = this.§_-OD§[_loc4_];
                  _loc3_ = new §_-41v§();
                  _loc3_.data = _loc2_;
                  this._toggleGroup.addItem(_loc3_);
                  addChild(_loc3_);
                  _loc4_++;
               }
            }
            this._toggleGroup.selectedIndex = Math.min(this._toggleGroup.numItems - 1,_loc1_);
            this._toggleGroup.addEventListener(Event.CHANGE,this.§_-13V§);
         }
         if(isInvalid(INVALIDATION_FLAG_SELECTED))
         {
            _loc1_ = this._toggleGroup.selectedIndex;
            _loc5_ = this.§_-q7§(this.§_-62x§);
            if(_loc1_ != _loc5_)
            {
               this._toggleGroup.selectedIndex = _loc5_;
            }
         }
      }
      
      override public function dispose() : void
      {
         this.§_-OD§ = null;
         if(this._toggleGroup)
         {
            this._toggleGroup.removeEventListener(Event.CHANGE,this.§_-13V§);
            this._toggleGroup = null;
         }
      }
      
      private function §_-13V§(param1:Event) : void
      {
         var _loc3_:int = 0;
         var _loc4_:§_-B1m§ = null;
         var _loc2_:ToggleGroup = param1.target as ToggleGroup;
         if(_loc2_)
         {
            _loc3_ = _loc2_.selectedIndex;
            if(_loc3_ != -1)
            {
               §_-h2B§.play(§_-81L§.§_-o1Z§);
            }
            _loc4_ = this.§_-OD§[_loc3_];
            if(_loc4_)
            {
               dispatchEventWith(Event.CHANGE,false,_loc4_.screenId);
            }
         }
      }
      
      private function §_-q7§(param1:String) : int
      {
         var _loc2_:int = -1;
         var _loc3_:int = 0;
         while(_loc3_ < this.§_-OD§.length)
         {
            if(this.§_-OD§[_loc3_].screenId == param1)
            {
               _loc2_ = _loc3_;
               break;
            }
            _loc3_++;
         }
         return _loc2_;
      }
      
      public function §_-b1j§(param1:String) : void
      {
         this.§_-62x§ = param1;
         invalidate(INVALIDATION_FLAG_SELECTED);
      }
      
      public function setData(param1:§_-r1d§) : void
      {
         this._data = param1;
         invalidate(INVALIDATION_FLAG_DATA);
      }
   }
}

