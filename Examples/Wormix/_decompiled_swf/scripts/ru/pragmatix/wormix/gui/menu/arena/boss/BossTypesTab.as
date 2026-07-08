package ru.pragmatix.wormix.gui.menu.arena.boss
{
   import §_-51W§.§_-G2h§;
   import §_-62§.*;
   import §_-73I§.*;
   import §_-79§.§_-11E§;
   import §_-937§.§_-BO§;
   import §_-C3f§.§_-yG§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-q2F§;
   import §_-F39§.§_-a2W§;
   import §_-J3L§.§_-h1D§;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-U16§;
   import §_-U1i§.§_-hK§;
   import §_-YF§.§_-q2v§;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.AutoSizeMode;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.core.ITextRenderer;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.text.TextFormatAlign;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   public class BossTypesTab extends §_-G2h§
   {
      
      private static const §_-62b§:String = "content";
      
      private static const §_-11s§:String = "all";
      
      public static const §_-M2n§:String = "single";
      
      public static const §_-B1u§:String = "coop";
      
      public static const §_-i1p§:String = "heroic";
      
      private var display:Sprite;
      
      private var §_-Xe§:ScreenNavigator;
      
      private var §_-kh§:Boolean;
      
      public function BossTypesTab()
      {
         super();
         this.§_-kh§ = false;
      }
      
      override public function activate() : void
      {
         super.activate();
         if(§_-X1j§.§_-A24§.§_-2z§ != null && §_-X1j§.§_-A24§.§_-2z§ != this.§_-Xe§.activeScreenID)
         {
            this.§_-Xe§.showScreen(§_-X1j§.§_-A24§.§_-2z§);
         }
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.display = §_-YQ§.§_-qM§(§_-q2v§.§_-qf§,§_-q2v§.§_-h2y§);
         this.display.touchable = true;
         addChild(this.display);
         this.§_-53q§(this.display.getChildByName(§_-62b§) as Sprite);
         var _loc1_:ITextRenderer = this.display.getChildByName(§_-I3s§) as ITextRenderer;
         §_-R2g§.§_-1c§(_loc1_,TextFormatAlign.CENTER);
         §_-R2g§.§_-k2g§(_loc1_,true);
         _loc1_.text = §_-s2a§.§_-K3t§(§_-s2a§.BOSSES_DESCR);
      }
      
      override public function update() : void
      {
         if(this.§_-Xe§.activeScreen is §_-x2u§)
         {
            (this.§_-Xe§.activeScreen as §_-x2u§).update();
         }
         super.update();
      }
      
      public function §_-81S§(param1:String) : void
      {
         this.§_-kh§ = true;
         (this.§_-Xe§.activeScreen as §_-x2u§).§_-N28§(param1);
      }
      
      private function §_-53q§(param1:Sprite) : void
      {
         param1.touchable = true;
         var _loc2_:ScreenNavigator = new ScreenNavigator();
         _loc2_.autoSizeMode = AutoSizeMode.CONTENT;
         param1.addChild(_loc2_);
         var _loc3_:ScreenNavigatorItem = new ScreenNavigatorItem(new §_-P2p§());
         _loc3_.setScreenIDForEvent(§_-M2n§,§_-M2n§);
         _loc3_.setScreenIDForEvent(§_-B1u§,§_-B1u§);
         _loc3_.setScreenIDForEvent(§_-i1p§,§_-i1p§);
         _loc2_.addScreen(§_-11s§,_loc3_);
         _loc3_ = new ScreenNavigatorItem(new §_-K1P§());
         _loc3_.setScreenIDForEvent(starling.events.Event.CANCEL,§_-11s§);
         _loc2_.addScreen(§_-M2n§,_loc3_);
         _loc3_ = new ScreenNavigatorItem(new §_-136§());
         _loc3_.setScreenIDForEvent(starling.events.Event.CANCEL,§_-11s§);
         _loc2_.addScreen(§_-B1u§,_loc3_);
         _loc3_ = new ScreenNavigatorItem(new §_-m20§());
         _loc3_.setScreenIDForEvent(starling.events.Event.CANCEL,§_-11s§);
         _loc2_.addScreen(§_-i1p§,_loc3_);
         var _loc4_:String = §_-X1j§.§_-A24§.§_-2z§;
         if(_loc4_)
         {
            _loc2_.showScreen(_loc4_);
         }
         else
         {
            _loc2_.showScreen(§_-11s§);
         }
         _loc2_.addEventListener(starling.events.Event.CHANGE,this.§_-L7§);
         this.§_-Xe§ = _loc2_;
      }
      
      private function §_-L7§(param1:starling.events.Event) : void
      {
         if(this.§_-Xe§.activeScreen is §_-x2u§)
         {
            (this.§_-Xe§.activeScreen as §_-x2u§).update();
         }
         else
         {
            §_-X1j§.§_-A24§.§_-2z§ = null;
         }
      }
      
      public function §_-31a§() : Boolean
      {
         if(this.§_-kh§ || this.§_-Xe§.activeScreenID == §_-11s§)
         {
            return false;
         }
         this.§_-Xe§.showScreen(§_-11s§);
         §_-h2B§.play(§_-81L§.§_-Cu§);
         return true;
      }
      
      override public function dispose() : void
      {
         if(this.§_-Xe§)
         {
            this.§_-Xe§.dispose();
            this.§_-Xe§ = null;
         }
         this.display = null;
         super.dispose();
      }
   }
}

