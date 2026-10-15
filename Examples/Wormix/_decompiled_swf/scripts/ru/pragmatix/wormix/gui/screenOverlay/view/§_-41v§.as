package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-F39§.§_-a2W§;
   import §_-Y1b§.§_-OP§;
   import §_-l1g§.§_-L3Q§;
   import feathers.controls.ButtonState;
   import feathers.controls.ToggleButton;
   import feathers.skins.ImageSkin;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.textures.Texture;
   import starling.utils.AssetManager;
   
   public class §_-41v§ extends ToggleButton
   {
      
      private static const §_-3U§:String = "base_overlay_";
      
      private var _data:§_-B1m§;
      
      public function §_-41v§()
      {
         super();
         this.useHandCursor = true;
      }
      
      override public function dispose() : void
      {
         §_-X1j§.localizationService.removeObject(this);
         super.dispose();
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            this.commitData();
         }
      }
      
      private function commitData() : void
      {
         var _loc1_:ImageSkin = null;
         var _loc2_:ImageSkin = null;
         var _loc3_:AssetManager = null;
         var _loc4_:Texture = null;
         var _loc5_:Texture = null;
         var _loc6_:Texture = null;
         var _loc7_:Texture = null;
         var _loc8_:Texture = null;
         var _loc9_:Texture = null;
         var _loc10_:String = null;
         var _loc11_:String = null;
         if(this._data)
         {
            _loc3_ = Application.assetManager;
            _loc4_ = _loc3_.getTexture(§_-3U§ + "nav_btn_up");
            _loc5_ = _loc3_.getTexture(§_-3U§ + "nav_btn_over");
            _loc6_ = _loc3_.getTexture(§_-3U§ + "nav_btn_down");
            _loc7_ = _loc3_.getTexture(§_-3U§ + "nav_btn_friend_default");
            _loc8_ = _loc3_.getTexture(§_-3U§ + "nav_btn_friend_default");
            _loc9_ = _loc3_.getTexture(§_-3U§ + "nav_btn_friend_over");
            x = this._data.§_-KA§;
            y = this._data.§_-cq§;
            _loc1_ = new ImageSkin(this._data.normalColor ? _loc4_ : _loc7_);
            _loc1_.setTextureForState(ButtonState.HOVER,this._data.normalColor ? _loc5_ : _loc8_);
            _loc1_.setTextureForState(ButtonState.DOWN,this._data.normalColor ? _loc6_ : _loc9_);
            _loc1_.setTextureForState(ButtonState.DOWN_AND_SELECTED,this._data.normalColor ? _loc6_ : _loc9_);
            _loc1_.setTextureForState(ButtonState.HOVER_AND_SELECTED,this._data.normalColor ? _loc6_ : _loc9_);
            _loc1_.setTextureForState(ButtonState.UP_AND_SELECTED,this._data.normalColor ? _loc6_ : _loc9_);
            defaultSkin = _loc1_;
            _loc10_ = §_-3U§ + this._data.iconName + "_down";
            _loc11_ = §_-3U§ + this._data.iconName + "_up";
            _loc2_ = new ImageSkin(_loc3_.getTexture(_loc10_));
            _loc2_.setTextureForState(ButtonState.DOWN,_loc3_.getTexture(_loc11_));
            _loc2_.setTextureForState(ButtonState.DOWN_AND_SELECTED,_loc3_.getTexture(_loc11_));
            _loc2_.setTextureForState(ButtonState.HOVER_AND_SELECTED,_loc3_.getTexture(_loc11_));
            _loc2_.setTextureForState(ButtonState.UP_AND_SELECTED,_loc3_.getTexture(_loc11_));
            defaultIcon = _loc2_;
            isEnabled = this._data.isEnabled;
            if(_isEnabled)
            {
               §_-X1j§.localizationService.§_-I2k§(this,"toolTip",this._data.§_-51x§);
            }
         }
      }
      
      public function get data() : §_-B1m§
      {
         return this._data;
      }
      
      public function set data(param1:§_-B1m§) : void
      {
         if(this._data != param1)
         {
            this._data = param1;
            invalidate(INVALIDATION_FLAG_DATA);
         }
      }
   }
}

