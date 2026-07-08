package ru.pragmatix.wormix.gui.menu.arena.boss
{
   import §_-5Y§.§_-h2c§;
   import §_-91B§.§_-z2l§;
   import §_-92a§.§_-Y2Y§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-x2t§;
   import §_-T2h§.*;
   import §_-TF§.§_-q1j§;
   import §_-Ta§.*;
   import §_-bI§.§_-d2V§;
   import §_-d26§.§_-c2E§;
   import §_-l2r§.§_-K1t§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ScrollPolicy;
   import feathers.core.ITextRenderer;
   import feathers.layout.AnchorLayout;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.pvp.messages.client.PvpCommand;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   public class §_-K2L§ extends EventDispatcher
   {
      
      private static const §_-Mx§:String = "content";
      
      private static const §_-f1f§:String = "back";
      
      private static const §_-7O§:String = "battleBtn";
      
      private static const §_-Ht§:String = "title";
      
      private var content:Sprite;
      
      private var button:Button;
      
      private var §_-z16§:§_-Y2Y§;
      
      private var _type:String;
      
      public function §_-K2L§(param1:Sprite, param2:String)
      {
         super();
         this._type = param2;
         this.content = param1.getChildByName(§_-Mx§) as Sprite;
         this.button = param1.getChildByName(§_-f1f§) as Button;
         this.button.addEventListener(Event.TRIGGERED,this.§_-r16§);
         (this.content.getChildByName("title") as ITextRenderer).text = §_-s2a§.§_-K3t§(this.§_-31q§(param2));
         ((this.content.getChildByName(§_-7O§) as Sprite).getChildByName(§_-Ht§) as ITextRenderer).text = §_-s2a§.§_-K3t§(§_-s2a§.BUTTON_SELECT);
         this.§_-z16§ = new §_-Y2Y§(this.button,this.content);
      }
      
      private function §_-31q§(param1:String) : String
      {
         var _loc2_:String = "";
         switch(param1)
         {
            case BossTypesTab.§_-M2n§:
               _loc2_ = §_-s2a§.BOSS_SINGLE_MODE_BTN;
               break;
            case BossTypesTab.§_-B1u§:
               _loc2_ = §_-s2a§.BOSS_COOP_MODE_BTN;
               break;
            case BossTypesTab.§_-i1p§:
               _loc2_ = §_-s2a§.SUPER_BOSS_TAB_NAME;
         }
         return _loc2_;
      }
      
      private function §_-r16§(param1:Event) : void
      {
         §_-X1j§.§_-A24§.§_-2z§ = this._type;
         dispatchEventWith(Event.TRIGGERED,false,this._type);
      }
      
      public function dispose() : void
      {
         this.content = null;
         if(this.button)
         {
            if(this.button.hasEventListener(Event.TRIGGERED))
            {
               this.button.removeEventListener(Event.TRIGGERED,this.§_-r16§);
            }
            this.button = null;
         }
         if(this.§_-z16§)
         {
            this.§_-z16§.dispose();
            this.§_-z16§ = null;
         }
      }
   }
}

