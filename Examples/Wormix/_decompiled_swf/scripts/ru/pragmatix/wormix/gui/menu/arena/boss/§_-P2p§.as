package ru.pragmatix.wormix.gui.menu.arena.boss
{
   import §_-CF§.§_-p2U§;
   import §_-DJ§.§_-J2l§;
   import §_-P1z§.*;
   import §_-RE§.§_-r2q§;
   import §_-TF§.§_-P19§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1Q§.§_-aG§;
   import §_-YF§.§_-Zi§;
   import §_-YF§.§_-q2v§;
   import §_-j1w§.§_-L1x§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-U2p§;
   import §_-s20§.§_-J2g§;
   import §_-z1g§.§_-y1S§;
   import com.greensock.TweenLite;
   import feathers.utils.textures.TextureCache;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.feats.IFrameFeat;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class §_-P2p§ extends Sprite
   {
      
      private var display:DisplayObjectContainer;
      
      private var §_-N21§:Array = [];
      
      public function §_-P2p§()
      {
         super();
         this.display = §_-YQ§.§_-qM§(§_-q2v§.§_-i2z§,§_-q2v§.§_-h2y§);
         this.display.touchable = true;
         addChild(this.display);
         this.§_-I2§(this.display);
      }
      
      private function §_-I2§(param1:DisplayObjectContainer) : void
      {
         this.§_-N21§[BossTypesTab.§_-M2n§] = this.§_-w2k§(param1.getChildByName("bosses") as Sprite,BossTypesTab.§_-M2n§);
         this.§_-N21§[BossTypesTab.§_-B1u§] = this.§_-w2k§(param1.getChildByName("coop") as Sprite,BossTypesTab.§_-B1u§);
         this.§_-N21§[BossTypesTab.§_-i1p§] = this.§_-w2k§(param1.getChildByName("super") as Sprite,BossTypesTab.§_-i1p§);
      }
      
      private function §_-w2k§(param1:Sprite, param2:String) : §_-K2L§
      {
         var _loc3_:§_-K2L§ = new §_-K2L§(param1,param2);
         _loc3_.addEventListener(Event.TRIGGERED,this.§_-ZL§);
         return _loc3_;
      }
      
      private function §_-ZL§(param1:Event) : void
      {
         dispatchEventWith(param1.data as String,false);
      }
      
      override public function dispose() : void
      {
         var _loc1_:* = 0;
         if(this.§_-N21§)
         {
            _loc1_ = int(this.§_-N21§.length);
            while(_loc1_--)
            {
               this.§_-N21§[_loc1_].removeEventListener(Event.TRIGGERED,this.§_-ZL§);
               this.§_-N21§[_loc1_].dispose();
            }
            this.§_-N21§ = null;
         }
         this.display = null;
         super.dispose();
      }
   }
}

