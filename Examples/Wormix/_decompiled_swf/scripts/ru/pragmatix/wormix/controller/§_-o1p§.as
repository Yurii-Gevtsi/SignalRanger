package ru.pragmatix.wormix.controller
{
   import §_-43V§.Random;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-W1r§;
   import §_-71C§.§_-u2q§;
   import §_-82a§.*;
   import §_-92a§.§_-Y2Y§;
   import §_-B1y§.§_-83E§;
   import §_-TF§.§_-q1j§;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-a2P§;
   import config.§_-vR§;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.Label;
   import feathers.controls.ToggleButton;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import flash.events.TimerEvent;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.utils.id.AccountErrorCode;
   import ru.pragmatix.flox.social.utils.id.AccountErrorEvent;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.scene.Weather;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.utils.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.display.Stage;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   
   use namespace bi_internal;
   
   public class §_-o1p§ extends EventDispatcher
   {
      
      private var §_-Oa§:IPlatformAccountController;
      
      private var _context:SocialAppContext;
      
      public function §_-o1p§(param1:SocialAppContext)
      {
         super(this);
         this._context = param1;
      }
      
      public function init() : void
      {
         this.§_-Oa§ = §_-u2q§.create(§_-vR§.§_-I1d§,this._context.§_-E1e§());
         §_-83E§.§_-C38§(this.§_-Oa§ as EventDispatcher,flash.events.Event.COMPLETE,this.§_-e1q§,AccountErrorEvent.ACCOUNT_ERROR,this.§_-M1V§);
         this.§_-Oa§.init();
      }
      
      private function §_-e1q§(param1:flash.events.Event) : void
      {
         dispatchEvent(new flash.events.Event(flash.events.Event.COMPLETE));
      }
      
      private function §_-M1V§(param1:AccountErrorEvent) : void
      {
         var _loc2_:Stage = null;
         _loc2_ = Starling.current.stage;
         var _loc3_:Label = new Label();
         _loc3_.styleName = AlternateTextStyles.WHITE_NORMAL_CENTER;
         _loc3_.width = _loc2_.stageWidth * 0.8;
         _loc3_.x = _loc2_.stageWidth * 0.1;
         _loc3_.y = _loc2_.stageHeight * 0.9;
         var _loc4_:String = "Platform " + this.§_-Oa§.getPlatformType().name + " error.";
         switch(param1.errorID)
         {
            case AccountErrorCode.INIT_ERROR:
               _loc4_ += "\nPlatform init error.";
               break;
            case AccountErrorCode.NOT_AVAILABLE:
               _loc4_ += "\nPlatform not available.";
         }
         _loc3_.text = _loc4_;
         _loc2_.addChild(_loc3_);
      }
      
      public function get §_-It§() : IPlatformAccountController
      {
         return this.§_-Oa§;
      }
   }
}

