package ru.pragmatix.flox.gui.controls
{
   import §_-02q§.§_-4s§;
   import §_-21p§.§_-4w§;
   import §_-5P§.*;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-73I§.*;
   import §_-91A§.*;
   import §_-92a§.ArenaMenu;
   import §_-931§.§_-i2r§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-C1i§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.§_-i1i§;
   import §_-G16§.*;
   import §_-J3L§.§_-h1D§;
   import §_-Ly§.§_-7f§;
   import §_-Ly§.§_-81L§;
   import §_-O2L§.§_-X1T§;
   import §_-Vu§.MD5;
   import §_-b1F§.*;
   import §_-e2r§.*;
   import §_-fo§.§_-r5§;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-j1w§.*;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import com.hurlant.math.*;
   import config.§_-V2w§;
   import config.§_-vR§;
   import feathers.controls.Button;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.data.ListCollection;
   import feathers.themes.FontSize;
   import flash.display.DisplayObject;
   import flash.display.SimpleButton;
   import flash.events.Event;
   import flash.media.SoundTransform;
   import flash.text.TextField;
   import flash.text.TextFieldAutoSize;
   import flash.text.TextFormat;
   import flash.text.TextFormatAlign;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.messages.client.MobileLogin;
   import ru.pragmatix.wormix.messages.server.AddToGroupResult;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.utils.Result;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.FreezingMissile;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.ScaleMode;
   
   use namespace bi_internal;
   
   public class MessageBox extends §_-X1T§
   {
      
      public static const §_-1B§:int = 0;
      
      public static const §_-X2g§:int = 1;
      
      public static const §_-L2I§:int = 2;
      
      public static const §_-H2d§:int = 3;
      
      private var §_-m2v§:§_-i2r§;
      
      private var §_-RU§:int = 25;
      
      private var §_-33O§:int = 15;
      
      private var callback:Function;
      
      public var §_-t1D§:Boolean = true;
      
      public function MessageBox(param1:String, param2:String, param3:int, param4:Function = null, param5:§_-t1R§ = null, param6:String = null, param7:Boolean = true)
      {
         var _loc9_:DisplayObject = null;
         var _loc10_:Number = NaN;
         super(§_-r5§.§_-tF§(param6 || §_-81L§.§_-I3X§));
         this.§_-t1D§ = param7;
         setTitle(param1);
         this.§_-m2v§ = §_-U2y§("msg");
         var _loc8_:Number = this.§_-m2v§.height;
         this.§_-m2v§.autoSize = TextFieldAutoSize.CENTER;
         this.§_-m2v§.§_-i2u§(param2);
         this.callback = param4;
         if(okBtn)
         {
            okBtn.§_-c1v§(param3 != §_-L2I§);
            okBtn.§_-r1M§(param3 == §_-H2d§ ? §_-7f§.BUTTON_YES : §_-7f§.BUTTON_OK);
         }
         if(§_-51P§)
         {
            §_-51P§.§_-c1v§(param3 == §_-X2g§ || param3 == §_-H2d§);
            §_-51P§.§_-r1M§(§_-7f§.BUTTON_CANCEL);
         }
         this.soundOnShow = param5;
         if(Boolean(param7) && Boolean(okBtn) && Boolean(§_-51P§))
         {
            _loc9_ = §_-y2h§(okBtn,§_-51P§);
            §_-4s§.§_-l15§(_loc9_);
            _loc10_ = height - _loc9_.y;
            height += Math.max(0,this.§_-m2v§.height - _loc8_);
            _loc9_.y = height - _loc10_;
         }
      }
      
      public static function §_-q1a§(param1:String, param2:String, param3:int = 0, param4:Function = null, param5:Function = null) : void
      {
         §_-F3p§(param1,param2,param3,param4,param5).show();
      }
      
      public static function §_-N2W§(param1:String, param2:String, param3:String, param4:int = 0, param5:Function = null, param6:Function = null) : void
      {
         §_-f2c§(param1,param2,param3,param4,param5,param6).show();
      }
      
      public static function §_-F3p§(param1:String, param2:String, param3:int = 0, param4:Function = null, param5:Function = null) : MessageBox
      {
         return §_-f2c§(null,param1,param2,param3,param4,param5);
      }
      
      public static function §_-f2c§(param1:String, param2:String, param3:String, param4:int = 0, param5:Function = null, param6:Function = null) : MessageBox
      {
         var panelClassName:String = param1;
         var title:String = param2;
         var msg:String = param3;
         var buttons:int = param4;
         var onOkClick:Function = param5;
         var onCancelClick:Function = param6;
         var window:MessageBox = new MessageBox(title,msg,buttons,onOkClick,null,panelClassName,true);
         if(onCancelClick != null)
         {
            window.addEventListener(§_-h1D§.WINDOW_CANCEL,function(param1:flash.events.Event):void
            {
               onCancelClick();
            });
         }
         return window;
      }
      
      public static function §_-42r§(param1:Object) : void
      {
         §_-q1a§("DEBUG",param1.toString());
      }
      
      public static function §_-dn§(param1:String = "") : void
      {
         §_-q1a§("TODO",param1);
      }
      
      public static function §_-h2r§(param1:String, param2:String, param3:int = 0, param4:Function = null) : void
      {
         new MessageBox(param1,param2,param3,param4,§_-81L§.§_-WH§).show();
      }
      
      override protected function §_-O2y§() : void
      {
         super.§_-O2y§();
         destroyOnRemove = true;
      }
      
      override protected function §_-b1H§() : void
      {
         if(this.callback != null)
         {
            this.callback();
         }
         super.§_-b1H§();
      }
      
      override public function destroy() : void
      {
         this.§_-m2v§ = null;
         this.callback = null;
         super.destroy();
      }
   }
}

