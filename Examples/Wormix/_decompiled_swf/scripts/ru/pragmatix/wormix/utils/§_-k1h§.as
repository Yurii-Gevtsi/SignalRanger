package ru.pragmatix.wormix.utils
{
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-9§.§_-52A§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-CR§.§_-72p§;
   import §_-DJ§.§_-fH§;
   import §_-K35§.§_-99§;
   import §_-L1H§.§_-C3e§;
   import §_-L1w§.§_-CD§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-P1z§.*;
   import §_-S§.*;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-Vg§.*;
   import §_-Y1b§.§_-OP§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-ZP§.§_-H1H§;
   import §_-j22§.§_-5g§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-l1g§.§_-T2N§;
   import §_-q26§.§_-Z25§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-737§;
   import §_-w2i§.§_-Ia§;
   import §_-x22§.§_-xY§;
   import §_-y1s§.§_-O1z§;
   import §_-zI§.§_-8D§;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import config.§_-vR§;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.desktop.NativeApplication;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.messages.clans.response.treasury.DonateClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.DistributePointsResult;
   import ru.pragmatix.wormix.messages.server.NeedMoneyResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.extensions.ParticleAura;
   import starling.text.TextField;
   import starling.utils.Pool;
   
   public class §_-k1h§
   {
      
      public function §_-k1h§()
      {
         super();
      }
      
      public static function §_-Se§(param1:DisplayObject, param2:Number) : void
      {
         param1.y = param2 - param1.height;
      }
      
      public static function §_-83g§(param1:DisplayObject) : void
      {
         param1.y = param1.parent.height - param1.height;
      }
      
      public static function §_-YU§(param1:DisplayObject) : void
      {
         §_-J3S§(param1,Starling.current.stage.stageWidth,Starling.current.stage.stageHeight);
      }
      
      public static function §_-k1B§(param1:DisplayObject) : void
      {
         §_-J3S§(param1,param1.parent.width,param1.parent.height);
      }
      
      public static function §_-yp§(param1:DisplayObject, param2:DisplayObject) : void
      {
         §_-p2m§(param1,param2);
         §_-am§(param1,param2);
      }
      
      public static function centerY(param1:DisplayObject, param2:Number, param3:Number) : void
      {
         param1.y = (param2 + param3 - param1.height) / 2;
      }
      
      public static function centerX(param1:DisplayObject, param2:Number, param3:Number) : void
      {
         param1.x = (param2 + param3 - param1.width) / 2;
      }
      
      public static function §_-l15§(param1:DisplayObject) : void
      {
         centerX(param1,0,param1.parent.width);
      }
      
      public static function §_-H1c§(param1:DisplayObject) : void
      {
         centerY(param1,0,param1.parent.width);
      }
      
      public static function §_-p2m§(param1:DisplayObject, param2:DisplayObject) : void
      {
         centerX(param1,param2.x,param2.x + param2.width);
      }
      
      public static function §_-am§(param1:DisplayObject, param2:DisplayObject) : void
      {
         centerY(param1,param2.y,param2.y + param2.height);
      }
      
      public static function §_-JK§(param1:DisplayObject, param2:Number, param3:Number, param4:Number, param5:Number) : void
      {
         centerX(param1,param2,param4);
         centerY(param1,param3,param5);
      }
      
      private static function §_-J3S§(param1:DisplayObject, param2:Number, param3:Number) : void
      {
         param1.x = (param2 - param1.width) / 2;
         param1.y = (param3 - param1.height) / 2;
      }
      
      public static function §_-1a§(param1:DisplayObject, param2:DisplayObject, param3:Number = 0) : void
      {
         param1.x = param2.x - param1.width - param3;
      }
      
      public static function §_-42m§(param1:DisplayObject, param2:DisplayObject, param3:Number = 0) : void
      {
         param1.x = param2.x + param2.width + param3;
      }
      
      public static function §_-w2M§(param1:DisplayObject, param2:DisplayObject, param3:Number = 0) : void
      {
         param1.y = param2.y - param1.height - param3;
      }
      
      public static function §_-i2S§(param1:DisplayObject, param2:DisplayObject, param3:Number = 0) : void
      {
         param1.y = param2.y + param2.height + param3;
      }
      
      public static function §_-Wx§(param1:DisplayObject, param2:Number, param3:Number) : Number
      {
         var _loc4_:Number = param2 / param1.width;
         var _loc5_:Number = param3 / param1.height;
         return Math.min(_loc4_,_loc5_);
      }
      
      public static function §_-v2D§(param1:DisplayObject, param2:Number, param3:Number) : Number
      {
         var _loc4_:Number = param2 / param1.width;
         var _loc5_:Number = param3 / param1.height;
         return Math.max(_loc4_,_loc5_);
      }
      
      public static function §_-Y2L§(param1:DisplayObject, param2:DisplayObject) : void
      {
         §_-F21§(param1,param2.width,param2.height);
      }
      
      public static function §_-2w§(param1:DisplayObject, param2:DisplayObject) : void
      {
         §_-p1n§(param1,param2.width,param2.height);
      }
      
      public static function §_-F21§(param1:DisplayObject, param2:Number, param3:Number) : void
      {
         var _loc4_:Number = §_-Wx§(param1,param2,param3);
         param1.width *= _loc4_;
         param1.height *= _loc4_;
      }
      
      public static function §_-p1n§(param1:DisplayObject, param2:Number, param3:Number) : void
      {
         var _loc4_:Number = §_-v2D§(param1,param2,param3);
         param1.width *= _loc4_;
         param1.height *= _loc4_;
      }
      
      public static function getChildByName(param1:DisplayObjectContainer, param2:String) : DisplayObject
      {
         var _loc4_:Number = NaN;
         var _loc5_:String = null;
         var _loc3_:DisplayObjectContainer = param1;
         while(true)
         {
            _loc4_ = param2.indexOf(".");
            if(_loc4_ == -1)
            {
               break;
            }
            _loc5_ = param2.substr(0,_loc4_);
            param2 = param2.substr(_loc4_ + 1);
            _loc3_ = _loc3_.getChildByName(_loc5_) as DisplayObjectContainer;
         }
         return _loc3_.getChildByName(param2);
      }
      
      public static function §_-2P§(param1:DisplayObjectContainer, param2:String) : TextField
      {
         return getChildByName(param1,param2) as TextField;
      }
   }
}

