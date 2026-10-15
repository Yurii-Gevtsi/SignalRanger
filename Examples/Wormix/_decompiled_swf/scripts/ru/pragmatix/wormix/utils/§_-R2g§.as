package ru.pragmatix.wormix.utils
{
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-E1I§.§_-b2n§;
   import §_-H16§.*;
   import §_-SP§.§_-M4§;
   import §_-d26§.§_-d1H§;
   import §_-k2s§.§_-Go§;
   import §_-k2s§.§_-b0§;
   import §_-l1K§.§_-d2j§;
   import feathers.controls.text.BitmapFontTextRenderer;
   import feathers.controls.text.CustomTextFieldTextRenderer;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.events.FeathersEventType;
   import feathers.text.BitmapFontTextFormat;
   import flash.text.AntiAliasType;
   import flash.text.GridFitType;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.wormix.model.rewards.*;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   import starling.text.BitmapFont;
   import starling.text.TextField;
   import starling.utils.MathUtil;
   
   public class §_-R2g§
   {
      
      public function §_-R2g§()
      {
         super();
      }
      
      public static function createTextRenderer(param1:String, param2:uint, param3:uint = 16777215, param4:String = "center", param5:Boolean = true, param6:Boolean = false, param7:int = 0) : ITextRenderer
      {
         var _loc9_:BitmapFontTextFormat = null;
         var _loc10_:BitmapFontTextRenderer = null;
         var _loc11_:TextFormat = null;
         var _loc12_:TextFieldTextRenderer = null;
         var _loc8_:BitmapFont = TextField.getBitmapFont(param1);
         if(_loc8_)
         {
            _loc9_ = new BitmapFontTextFormat(param1,param2,param3,param4);
            _loc9_.letterSpacing = param7;
            _loc10_ = new BitmapFontTextRenderer();
            _loc10_.textFormat = _loc9_;
            _loc10_.wordWrap = param5;
            _loc10_.useSeparateBatch = false;
            _loc10_.touchable = false;
            return _loc10_;
         }
         _loc11_ = new TextFormat(param1,param2,param3);
         _loc11_.align = param4;
         _loc11_.rightMargin = 1;
         _loc11_.letterSpacing = param7;
         _loc12_ = new CustomTextFieldTextRenderer();
         _loc12_.textFormat = _loc11_;
         _loc12_.antiAliasType = AntiAliasType.ADVANCED;
         _loc12_.gridFitType = GridFitType.NONE;
         _loc12_.isHTML = param6;
         _loc12_.wordWrap = param5;
         return _loc12_;
      }
      
      public static function §_-g1H§(param1:ITextRenderer, param2:String = null, param3:uint = 0, param4:uint = 0, param5:String = null) : void
      {
         var _loc6_:TextFormat = null;
         var _loc7_:BitmapFontTextFormat = null;
         if(param1 is TextFieldTextRenderer)
         {
            _loc6_ = (param1 as TextFieldTextRenderer).textFormat;
            if(param2)
            {
               _loc6_.font = param2;
            }
            if(param3 != 0)
            {
               _loc6_.size = param3;
            }
            if(param4 != 0)
            {
               _loc6_.color = param4;
            }
            if(param5)
            {
               _loc6_.align = param5;
            }
            (param1 as TextFieldTextRenderer).textFormat = _loc6_;
         }
         else if(param1 is BitmapFontTextRenderer)
         {
            _loc7_ = (param1 as BitmapFontTextRenderer).textFormat;
            if(Boolean(param2) && Boolean(TextField.getBitmapFont(param2)))
            {
               _loc7_.font = TextField.getBitmapFont(param2);
            }
            if(param3 != 0)
            {
               _loc7_.size = param3;
            }
            if(param4 != 0)
            {
               _loc7_.color = param4;
            }
            if(param5)
            {
               _loc7_.align = param5;
            }
            (param1 as BitmapFontTextRenderer).textFormat = _loc7_;
         }
         (param1 as FeathersControl).invalidate(FeathersControl.INVALIDATION_FLAG_STYLES);
      }
      
      public static function §_-E12§(param1:ITextRenderer, param2:uint = 0, param3:uint = 0, param4:String = null) : void
      {
         var _loc6_:Object = null;
         var _loc5_:String = "textFormat";
         if(Boolean(param1) && (param1 as Object).hasOwnProperty(_loc5_))
         {
            _loc6_ = param1[_loc5_];
            if(_loc6_)
            {
               _loc6_.color = param3;
               _loc6_.size = param2;
               _loc6_.size = param4;
               param1[_loc5_] = _loc6_;
               (param1 as FeathersControl).invalidate(FeathersControl.INVALIDATION_FLAG_STYLES);
            }
         }
      }
      
      public static function §_-t8§(param1:ITextRenderer, param2:uint = 0) : void
      {
         var _loc4_:Object = null;
         var _loc3_:String = "textFormat";
         if(Boolean(param1) && (param1 as Object).hasOwnProperty(_loc3_))
         {
            _loc4_ = param1[_loc3_];
            if(_loc4_)
            {
               _loc4_.color = param2;
               param1[_loc3_] = _loc4_;
               (param1 as FeathersControl).invalidate(FeathersControl.INVALIDATION_FLAG_STYLES);
            }
         }
      }
      
      public static function §_-k2g§(param1:ITextRenderer, param2:Boolean) : void
      {
         if(param1 is TextFieldTextRenderer)
         {
            (param1 as TextFieldTextRenderer).isHTML = param2;
         }
      }
      
      public static function §_-1c§(param1:ITextRenderer, param2:String) : void
      {
         if(param1 is TextFieldTextRenderer)
         {
            (param1 as TextFieldTextRenderer).textFormat.align = param2;
         }
         if(param1 is BitmapFontTextRenderer)
         {
            (param1 as BitmapFontTextRenderer).textFormat.align = param2;
         }
      }
   }
}

