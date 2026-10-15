package ru.pragmatix.wormix.gui.menu.arena.boss
{
   import §_-21A§.§_-02r§;
   import §_-73I§.§_-K2s§;
   import §_-73d§.§_-h2X§;
   import §_-82a§.SwitchedRenderTextures;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.*;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-82u§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-RE§.§_-52p§;
   import §_-RE§.§_-qW§;
   import §_-RE§.§_-r2q§;
   import §_-T2h§.*;
   import §_-Y1Q§.§_-61Z§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-c2O§.§_-03I§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-d2j§;
   import §_-nZ§.§_-821§;
   import §_-w2W§.WardrobeRoomScreen;
   import §_-z1g§.§_-y1S§;
   import com.greensock.TweenMax;
   import feathers.controls.List;
   import feathers.core.ITextRenderer;
   import flash.errors.IllegalOperationError;
   import flash.events.TimerEvent;
   import flash.utils.ByteArray;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.Texture;
   
   public class §_-m20§ extends §_-x2u§
   {
      
      protected static var §_-J2p§:int = -1;
      
      private var isLoaded:Boolean;
      
      public function §_-m20§()
      {
         var _loc1_:DisplayObjectContainer = §_-YQ§.§_-qM§(§_-q2v§.§_-B3C§,§_-q2v§.§_-h2y§);
         super(_loc1_,§_-02r§,§_-821§);
         this.isLoaded = false;
      }
      
      override protected function §_-QB§() : Array
      {
         var _loc1_:Vector.<HeroicMissionStructure> = §_-qW§.getInstance().§_-33l§();
         var _loc2_:Array = [];
         var _loc3_:uint = 0;
         while(_loc3_ < _loc1_.length)
         {
            _loc2_.push(_loc1_[_loc3_]);
            _loc3_++;
         }
         return _loc2_;
      }
      
      override protected function activate() : void
      {
         var _loc1_:ITextRenderer = §_-Hw§.getChildByName("everyDayText") as ITextRenderer;
         if(_loc1_)
         {
            _loc1_.text = §_-s2a§.§_-K3t§(§_-s2a§.TITLE_HEROIC_BOSSES_PANEL);
         }
         if(§_-SR§ as §_-02r§)
         {
            (§_-SR§ as §_-02r§).§_-L1y§(§_-X1j§.§_-A24§.§_-mV§);
            (§_-SR§ as §_-02r§).update();
         }
         super.activate();
         this.§_-P1I§(§_-i2C§,§_-SR§.selectedIndex);
      }
      
      override protected function §_-p2N§(param1:Event) : void
      {
         this.§_-m7§(§_-SR§.selectedIndex);
         this.§_-P1I§(§_-i2C§,§_-SR§.selectedIndex);
      }
      
      public function §_-P1I§(param1:§_-03I§, param2:int) : void
      {
         var _loc3_:HeroicMissionStructure = null;
         if(param2 < 0)
         {
            return;
         }
         _loc3_ = §_-qW§.getInstance().§_-33l§()[param2];
         var _loc4_:Boolean = §_-r2q§.§_-Q2p§(_loc3_.bossId1) && §_-r2q§.§_-Q2p§(_loc3_.bossId2);
         var _loc5_:§_-52p§ = §_-52p§.§_-g2U§()[param2];
         if(!§_-r2q§.§_-UJ§)
         {
            param1.§_-x2i§(_loc5_);
         }
         else if(!_loc4_)
         {
            param1.§_-511§(_loc5_);
         }
         else
         {
            param1.§_-23j§(_loc5_);
         }
      }
      
      override public function dispose() : void
      {
         super.dispose();
      }
      
      override protected function §_-I1p§() : int
      {
         return §_-J2p§;
      }
      
      override protected function §_-m7§(param1:int) : void
      {
         §_-J2p§ = param1;
      }
   }
}

