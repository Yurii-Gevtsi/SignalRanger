package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-51M§.Cat;
   import §_-DJ§.§_-73§;
   import §_-H16§.*;
   import §_-K35§.§_-gY§;
   import §_-Ly§.§_-81L§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-lp§;
   import feathers.utils.touch.TapToTrigger;
   import flash.utils.ByteArray;
   import org.swiftsuspenders.§_-71M§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.treasury.DonateClanRequest;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-O1f§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.extensions.QuadSection;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   public class §_-32C§ implements §_-O1f§
   {
      
      private var _avatarContainer:Sprite;
      
      private var _levelContainer:Sprite;
      
      private var §_-BZ§:QuadSection;
      
      private var §_-01w§:§_-gY§;
      
      private var _nameLabel:CustomLabel;
      
      private var §_-O24§:§_-r1d§;
      
      private var §_-O1S§:ContainerButtonWithStates;
      
      private var §_-X1k§:String = "";
      
      private var §_-53r§:String = "";
      
      private var §_-329§:String = "";
      
      public function §_-32C§(param1:Sprite, param2:Sprite, param3:CustomLabel)
      {
         super();
         this._avatarContainer = param1;
         this._levelContainer = param2;
         this._nameLabel = param3;
         this.§_-BZ§ = this._levelContainer.getChildByName("progress") as QuadSection;
         this.§_-O1S§ = this._levelContainer.getChildByName("levelBtn") as ContainerButtonWithStates;
         this.§_-01w§ = new §_-gY§(this._avatarContainer);
         new TapToTrigger(this._avatarContainer);
         §_-X1j§.localizationService.§_-yh§(this.§_-O1S§,"toolTip",this.§_-um§);
      }
      
      public function dispose() : void
      {
         §_-X1j§.localizationService.removeObject(this.§_-O1S§);
         this.§_-q2M§();
         this.§_-Y2v§();
         this.§_-01w§.dispose();
      }
      
      private function §_-53G§(param1:§_-r1d§) : void
      {
         var _loc2_:UserProfile = param1.userProfile;
         var _loc3_:int = int(§_-lp§.§_-L1M§(_loc2_.getLevel()));
         var _loc4_:int = _loc2_.§_-p25§();
         this.§_-53r§ = _loc4_.toString();
         this.§_-329§ = _loc3_.toString().toUpperCase();
         this.§_-X1k§ = _loc2_.getLevel().toString();
         this.§_-O1S§.setTextInSkins("value",this.§_-X1k§);
         this.§_-O1S§.toolTip = this.§_-um§();
         this.§_-BZ§.ratio = _loc4_ / _loc3_;
         this._nameLabel.text = _loc2_.getCharName().toString();
         this._nameLabel.textFormat.color = param1.§_-x2k§ ? FontColor.NAME_ORANGE_COLOR : FontColor.NAME_GREEN_COLOR;
         this._nameLabel.validate();
         var _loc5_:Number = this._nameLabel.width / this._nameLabel.scale;
         if(_loc5_ > 280)
         {
            this._nameLabel.scale = 280 / _loc5_;
         }
         else
         {
            this._nameLabel.scale = 1;
         }
         this.§_-01w§.§_-82v§(_loc2_);
         this.§_-q2M§();
         if(param1.§_-x2k§)
         {
            this.§_-O1S§.addEventListener(Event.TRIGGERED,this.§_-B3i§);
            this._avatarContainer.addEventListener(Event.TRIGGERED,this.§_-B3i§);
         }
         this.§_-O1S§.enabled = param1.§_-x2k§;
      }
      
      private function §_-B3i§(param1:Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         §_-73§.§_-D3u§(§_-q2v§.§_-023§);
      }
      
      public function §_-32X§(param1:§_-r1d§) : void
      {
         this.§_-Y2v§();
         this.§_-O24§ = param1;
         this.§_-O24§.addEventListener(Event.CHANGE,this.§_-E28§);
         this.§_-O24§.addEventListener(Event.UPDATE,this.§_-Z2B§);
         this.§_-53G§(param1);
      }
      
      private function §_-Z2B§(param1:Event) : void
      {
         this.§_-53G§(this.§_-O24§);
      }
      
      private function §_-E28§(param1:Event) : void
      {
         var _loc2_:§_-r1d§ = param1.target as §_-r1d§;
         this.§_-53G§(_loc2_);
      }
      
      private function §_-Y2v§() : void
      {
         if(this.§_-O24§)
         {
            this.§_-O24§.removeEventListener(Event.CHANGE,this.§_-E28§);
            this.§_-O24§.removeEventListener(Event.UPDATE,this.§_-Z2B§);
            this.§_-O24§ = null;
         }
      }
      
      private function §_-q2M§() : void
      {
         this._avatarContainer.removeEventListener(Event.TRIGGERED,this.§_-B3i§);
         this.§_-O1S§.removeEventListener(Event.TRIGGERED,this.§_-B3i§);
      }
      
      private function §_-um§() : String
      {
         var _loc1_:Object = {
            "cur":this.§_-53r§,
            "max":this.§_-329§
         };
         var _loc2_:String = StringUtil.format(§_-s2a§.EXP_QTIP,_loc1_);
         return §_-s2a§.§_-K3t§(§_-s2a§.LEVEL_QTIP) + ": " + this.§_-X1k§ + "\n" + _loc2_;
      }
   }
}

