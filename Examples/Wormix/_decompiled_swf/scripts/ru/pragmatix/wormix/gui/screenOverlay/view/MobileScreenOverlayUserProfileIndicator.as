package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-CR§.§_-S2o§;
   import §_-G2H§.*;
   import §_-K35§.§_-gY§;
   import §_-Ly§.§_-81L§;
   import §_-TF§.§_-N1E§;
   import §_-YF§.§_-q2v§;
   import §_-ZP§.§_-w1J§;
   import §_-l1K§.§_-cZ§;
   import §_-r1C§.§_-h2B§;
   import §_-z1g§.§_-lp§;
   import config.§_-vR§;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.utils.touch.TapToTrigger;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-S2J§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.phase.HarmPhase;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.extensions.QuadSection;
   import starling.utils.Pool;
   
   public class MobileScreenOverlayUserProfileIndicator extends LayoutGroup
   {
      
      private var _data:§_-r1d§;
      
      private var §_-D3o§:Label;
      
      private var §_-73C§:Label;
      
      private var §_-S2Q§:Label;
      
      private var §_-b2r§:Label;
      
      private var §_-t2y§:QuadSection;
      
      private var §_-lz§:Sprite;
      
      private var §_-Ol§:§_-S2J§;
      
      private var §_-K3M§:§_-S2J§;
      
      private var §_-tm§:§_-S2J§;
      
      private var §_-N2n§:§_-gY§;
      
      public function MobileScreenOverlayUserProfileIndicator()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-D3o§ = §_-YQ§.getClip("params.name.label",this) as Label;
         this.§_-73C§ = §_-YQ§.getClip("params.fuzy.label",this) as Label;
         this.§_-S2Q§ = §_-YQ§.getClip("params.ruby.label",this) as Label;
         this.§_-b2r§ = §_-YQ§.getClip("level.label",this) as Label;
         this.§_-t2y§ = §_-YQ§.getClip("level.progress",this) as QuadSection;
         this.§_-lz§ = §_-YQ§.getClip("avatar",this) as Sprite;
         this.§_-N2n§ = new §_-gY§(this.§_-lz§);
         new TapToTrigger(this.§_-lz§);
         this.§_-lz§.addEventListener(Event.TRIGGERED,this.§_-y1l§);
         this.§_-Ol§ = new §_-S2J§(this.§_-D3o§,260);
         this.§_-K3M§ = new §_-S2J§(this.§_-73C§,125);
         this.§_-tm§ = new §_-S2J§(this.§_-S2Q§,125);
         var _loc1_:DisplayObject = §_-YQ§.getClip("params.fuzy",this);
         var _loc2_:DisplayObject = §_-YQ§.getClip("params.ruby",this);
         new TapToTrigger(_loc1_);
         new TapToTrigger(_loc2_);
         _loc1_.addEventListener(Event.TRIGGERED,this.§_-D1t§);
         _loc2_.addEventListener(Event.TRIGGERED,this.§_-919§);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         this.§_-N2n§.dispose();
         this.§_-Ol§.dispose();
         this.§_-K3M§.dispose();
         this.§_-tm§.dispose();
         this.§_-lz§.removeEventListener(Event.TRIGGERED,this.§_-y1l§);
         §_-YQ§.getClip("params.fuzy",this).removeEventListener(Event.TRIGGERED,this.§_-D1t§);
         §_-YQ§.getClip("params.ruby",this).removeEventListener(Event.TRIGGERED,this.§_-919§);
      }
      
      override protected function draw() : void
      {
         var _loc1_:UserProfile = null;
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            if(this._data)
            {
               _loc1_ = this._data.userProfile;
               this.§_-D3o§.text = _loc1_.getCharName();
               this.§_-73C§.text = _loc1_.§_-51e§().toString();
               this.§_-S2Q§.text = _loc1_.§_-o1N§().toString();
               this.§_-b2r§.text = _loc1_.getLevel().toString();
               this.§_-t2y§.ratio = _loc1_.§_-p25§() / §_-lp§.§_-L1M§(_loc1_.getLevel());
               this.§_-N2n§.§_-82v§(_loc1_);
               this.§_-lz§.touchable = this._data.§_-x2k§;
            }
            else
            {
               this.§_-D3o§.text = "";
               this.§_-73C§.text = "";
               this.§_-S2Q§.text = "";
               this.§_-b2r§.text = "";
               this.§_-t2y§.ratio = 0;
            }
         }
      }
      
      public function get data() : §_-r1d§
      {
         return this._data;
      }
      
      public function set data(param1:§_-r1d§) : void
      {
         this._data = param1;
         invalidate(INVALIDATION_FLAG_DATA);
      }
      
      private function §_-y1l§(param1:Event) : void
      {
         var _loc2_:String = null;
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         if(param1.currentTarget == this.§_-lz§)
         {
            _loc2_ = §_-q2v§.§_-023§;
         }
         if(_loc2_)
         {
            dispatchEventWith(Event.SELECT,false,_loc2_);
         }
      }
      
      private function §_-D1t§(param1:Event) : void
      {
         this.§_-Dt§(param1.currentTarget as DisplayObject,§_-s2a§.COIN_QTIP);
      }
      
      private function §_-919§(param1:Event) : void
      {
         this.§_-Dt§(param1.currentTarget as DisplayObject,§_-s2a§.RUBY_QTIP);
      }
      
      private function §_-Dt§(param1:DisplayObject, param2:String) : void
      {
         var _loc3_:DisplayObject = new §_-N1E§(§_-s2a§.§_-K3t§(param2));
         §_-S2o§.§_-02T§(_loc3_,param1,null,false);
      }
   }
}

