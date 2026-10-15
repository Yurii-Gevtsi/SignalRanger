package ru.pragmatix.wormix.utils
{
   import §_-11L§.Archdemon;
   import §_-11L§.Hacker;
   import §_-11L§.Paladin;
   import §_-11L§.Phantoms;
   import §_-11L§.Thieves;
   import §_-11L§.§_-P25§;
   import §_-21p§.§_-N1H§;
   import §_-82l§.§_-1I§;
   import §_-B1y§.§_-83E§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-H3§.*;
   import §_-L2F§.§_-D2p§;
   import §_-RE§.*;
   import §_-Y1O§.*;
   import §_-Yk§.*;
   import §_-Z1K§.§_-q24§;
   import §_-d2p§.*;
   import §_-j22§.§_-5g§;
   import §_-k2s§.§_-b0§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-iI§;
   import §_-k2s§.§_-uU§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.X509CertificateCollection;
   import §_-qH§.§_-S25§;
   import §_-s1Q§.Artifacts;
   import §_-w2i§.§_-52k§;
   import §_-z1g§.§_-522§;
   import §_-z1g§.§_-y1S§;
   import com.hurlant.crypto.tls.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.ImageLoader;
   import feathers.controls.Label;
   import feathers.controls.text.TextFormatUtils;
   import feathers.themes.FontName;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.errors.IllegalOperationError;
   import flash.events.IEventDispatcher;
   import flash.geom.ColorTransform;
   import flash.geom.Matrix;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.CustomColorMatrixFilter;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.DistributePointsResult;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Stage;
   import starling.events.Event;
   import starling.filters.ColorMatrixFilter;
   import starling.rendering.Painter;
   import starling.textures.Texture;
   import starling.textures.TextureSmoothing;
   import starling.utils.Pool;
   
   public class §_-c1S§
   {
      
      private static const §_-j18§:String = "RatingPanel_nonTopCell_icon_1";
      
      private static const §_-T1R§:String = "RatingPanel_nonTopCell_icon_2";
      
      private static const §_-U1w§:String = "RatingPanel_nonTopCell_icon_3";
      
      private static const §_-6Y§:uint = 5939473;
      
      private static const §_-e9§:uint = 14104889;
      
      private static const §_-u26§:String = "change_delta_up";
      
      private static const §_-P22§:String = "change_delta_down";
      
      private static const §_-a2C§:String = "change_delta_equal";
      
      public function §_-c1S§()
      {
         super();
      }
      
      public static function §_-q13§(param1:flash.display.DisplayObject, param2:Image = null) : Image
      {
         var _loc8_:Rectangle = null;
         var _loc9_:Image = null;
         if(!param1)
         {
            return null;
         }
         var _loc3_:Rectangle = param1.getRect(param1);
         var _loc4_:Number = _loc3_.width;
         var _loc5_:Number = _loc3_.height;
         if(_loc4_ < 1 || _loc5_ < 1)
         {
            if(param2)
            {
               param2.texture.dispose();
               param2.texture = Texture.empty(1,1);
               return param2;
            }
            return new Image(Texture.empty(1,1));
         }
         var _loc6_:BitmapData = new BitmapData(_loc4_,_loc5_);
         var _loc7_:ColorTransform = new ColorTransform();
         _loc7_.alphaMultiplier = 0;
         _loc8_ = Pool.getRectangle(0,0,_loc3_.width,_loc3_.height);
         _loc6_.colorTransform(_loc8_,_loc7_);
         _loc6_.draw(param1,new Matrix(1,0,0,1,-_loc3_.x,-_loc3_.y));
         _loc9_ = §_-K18§(param2,_loc6_);
         _loc9_.x = param1.x + _loc3_.x;
         _loc9_.y = param1.y + _loc3_.y;
         _loc9_.scaleX = param1.scaleX;
         _loc9_.scaleY = param1.scaleY;
         §_-F1m§(_loc9_,Math.ceil(param1.width),Math.ceil(param1.height));
         Pool.putRectangle(_loc8_);
         return _loc9_;
      }
      
      private static function §_-K18§(param1:Image, param2:BitmapData) : Image
      {
         var _loc3_:Number = Starling.current.contentScaleFactor;
         var _loc4_:Image = param1;
         if(_loc4_)
         {
            if(_loc4_.texture.width > 0)
            {
               if(param2.width > _loc4_.texture.width || param2.height > _loc4_.texture.height)
               {
                  _loc4_.texture.dispose();
                  _loc4_.texture = Texture.fromBitmapData(param2,false,false,_loc3_);
                  _loc4_.readjustSize();
               }
               else
               {
                  _loc4_.texture.root.uploadBitmapData(param2);
               }
            }
            else
            {
               _loc4_.texture.dispose();
               _loc4_.texture = Texture.fromBitmapData(param2,false,false,_loc3_);
            }
            return _loc4_;
         }
         return new Image(Texture.fromBitmapData(param2,false,false,_loc3_));
      }
      
      private static function §_-F1m§(param1:starling.display.DisplayObject, param2:Number, param3:Number) : void
      {
         if(param1.width != param2)
         {
            param1.width = param2;
         }
         if(param1.height != param3)
         {
            param1.height = param3;
         }
      }
      
      public static function §_-ku§(param1:flash.display.DisplayObject, param2:Image = null) : Image
      {
         var _loc3_:Boolean = param2 == null;
         var _loc4_:Image = §_-c1S§.§_-q13§(param1,param2);
         if(_loc3_)
         {
            §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(_loc4_,SortingLayer.POINTERS_LAYER);
         }
         return _loc4_;
      }
      
      public static function §_-CC§(param1:starling.display.DisplayObject) : void
      {
         var _loc2_:Matrix = param1.transformationMatrix;
         var _loc3_:Point = Pool.getPoint(param1.width / 2,param1.height / 2);
         _loc2_.transformPoint(_loc3_);
         Pool.putPoint(_loc3_);
         if(_loc2_.a > 0)
         {
            _loc2_.a = -1 * _loc2_.a;
            _loc2_.tx = param1.x;
         }
         else
         {
            _loc2_.a = -1 * _loc2_.a;
            _loc2_.tx = param1.x;
         }
         param1.transformationMatrix = _loc2_;
      }
      
      public static function dispose(param1:starling.display.DisplayObject) : DisplayObjectContainer
      {
         if(param1)
         {
            if(param1.parent)
            {
               param1.parent.removeChild(param1);
            }
            param1.dispose();
         }
         return null;
      }
      
      public static function §_-DE§(param1:Image) : Image
      {
         if(Boolean(param1) && Boolean(param1.texture))
         {
            param1.texture.dispose();
         }
         dispose(param1);
         return null;
      }
      
      public static function §_-Z1T§(param1:Starling, param2:starling.display.DisplayObject, param3:Function) : void
      {
         §_-52k§.single(param1,Event.RENDER,§_-y2J§,null,[param1,param2,param3]);
      }
      
      private static function §_-y2J§(param1:Starling, param2:starling.display.DisplayObject, param3:Function) : void
      {
         var _loc4_:Rectangle = param2.getBounds(param2);
         var _loc5_:BitmapData = new BitmapData(_loc4_.width,_loc4_.height,true);
         var _loc6_:Stage = param1.stage;
         var _loc7_:Painter = param1.painter;
         _loc7_.pushState();
         _loc7_.state.renderTarget = null;
         _loc7_.state.setProjectionMatrix(_loc4_.x,_loc4_.y,_loc6_.stageWidth,_loc6_.stageHeight,_loc6_.stageWidth,_loc6_.stageHeight,_loc6_.cameraPosition);
         _loc7_.clear();
         param2.setRequiresRedraw();
         param2.render(_loc7_);
         _loc7_.finishMeshBatch();
         _loc7_.context.drawToBitmapData(_loc5_);
         _loc7_.popState();
         param3(_loc5_);
         Pool.putRectangle(_loc4_);
      }
      
      public static function §_-MU§(param1:int, param2:Label, param3:ImageLoader) : void
      {
         var _loc4_:String = "";
         var _loc5_:String = "";
         if(param1 > 0)
         {
            _loc4_ = param1.toString();
            if(param1 == 1)
            {
               _loc5_ = §_-j18§;
            }
            else if(param1 == 2)
            {
               _loc5_ = §_-T1R§;
            }
            else if(param1 == 3)
            {
               _loc5_ = §_-U1w§;
            }
         }
         if(param2)
         {
            param2.text = _loc4_;
         }
         if(param3)
         {
            §_-y1p§(param3,_loc5_);
         }
      }
      
      public static function §_-62q§(param1:int, param2:int, param3:Label, param4:int, param5:ImageLoader, param6:Boolean = true) : void
      {
         var _loc10_:int = 0;
         var _loc11_:Object = null;
         var _loc7_:String = "";
         var _loc8_:uint = 0;
         var _loc9_:String = "";
         if(param1 > 0)
         {
            _loc10_ = param2 != 0 ? int(param2 - param1) : 0;
            _loc7_ = _loc10_ != 0 ? Math.abs(_loc10_).toString() : "";
            if(_loc10_ > 0)
            {
               _loc8_ = §_-6Y§;
               _loc9_ = §_-u26§;
            }
            else if(_loc10_ < 0)
            {
               _loc8_ = §_-e9§;
               _loc9_ = §_-P22§;
            }
            else
            {
               _loc9_ = §_-a2C§;
            }
         }
         if(param3)
         {
            param3.text = _loc7_;
            if(param6)
            {
               _loc11_ = TextFormatUtils.createBitmapFontTextFormat(param4,_loc8_,TextFormatAlign.LEFT);
            }
            else
            {
               _loc11_ = TextFormatUtils.createFlashTextFormat(FontName.WORMIX_NORMAL,param4,_loc8_,TextFormatAlign.LEFT);
            }
            param3.textRendererProperties.textFormat = _loc11_;
         }
         §_-y1p§(param5,_loc9_);
      }
      
      private static function §_-y1p§(param1:ImageLoader, param2:String) : void
      {
         if(param1)
         {
            if(param2)
            {
               param1.source = Application.assetManager.getTexture(param2);
            }
            else
            {
               param1.source = "";
            }
         }
      }
   }
}

