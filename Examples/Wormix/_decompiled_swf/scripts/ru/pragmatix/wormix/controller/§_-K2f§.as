package ru.pragmatix.wormix.controller
{
   import §_-62§.§_-F1D§;
   import §_-63k§.*;
   import §_-931§.§_-sz§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.Cookie;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-Cl§.§_-L2z§;
   import §_-Cl§.§_-li§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.DepositIncomeWindow;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-G16§.§_-33h§;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-L2F§.*;
   import §_-Ly§.§_-81L§;
   import §_-SP§.§_-M4§;
   import §_-T1t§.§_-u19§;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.§_-RF§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-81l§;
   import §_-Y1b§.§_-OP§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-j1w§.§_-L1x§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-p18§.§_-V14§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-u1m§.*;
   import §_-u2J§.§_-T1K§;
   import §_-w2i§.§_-52k§;
   import §_-zI§.§_-U1A§;
   import §_-zI§.§_-X1q§;
   import com.distriqt.extension.cloudstorage.CloudStorage;
   import com.hurlant.crypto.tls.§_-F16§;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import config.§_-V2w§;
   import dragonBones.animation.WorldClock;
   import feathers.core.ITextRenderer;
   import feathers.themes.FontColor;
   import feathers.utils.touch.TapToTrigger;
   import flash.desktop.NativeApplication;
   import flash.display.BitmapData;
   import flash.display.Shape;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.events.InvokeEvent;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.system.LoaderContext;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.analytics.Analytics;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.client.GetDividend;
   import ru.pragmatix.wormix.messages.server.GetDividendResult;
   import ru.pragmatix.wormix.messages.structures.DepositStructure;
   import ru.pragmatix.wormix.messages.utils.Result;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.phase.HealBolt;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.textures.TextureSmoothing;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-K2f§ extends EventDispatcher
   {
      
      private const §_-02Q§:int = 5;
      
      private var §_-QW§:Array;
      
      private var §_-f28§:Boolean;
      
      private var disposed:Boolean;
      
      public function §_-K2f§()
      {
         super();
         this.§_-QW§ = [];
         this.§_-f28§ = false;
         this.disposed = false;
      }
      
      public function dispose() : void
      {
         this.§_-QW§ = [];
         this.disposed = true;
      }
      
      public function §_-11r§(param1:DepositStructure) : void
      {
         this.§_-QW§.push(param1);
         if(param1.todayDividend > 0)
         {
            this.§_-f28§ = true;
         }
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      public function §_-i1y§(param1:Array) : void
      {
         var _loc2_:DepositStructure = null;
         this.§_-QW§ = [];
         for each(_loc2_ in param1)
         {
            this.§_-QW§.push(_loc2_);
            if(_loc2_.todayDividend > 0)
            {
               this.§_-f28§ = true;
            }
         }
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      public function get §_-j1j§() : Boolean
      {
         var _loc1_:String = null;
         var _loc2_:String = null;
         if(this.§_-QW§.length >= this.§_-02Q§)
         {
            _loc1_ = §_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION);
            _loc2_ = §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.DEPOSIT_MAX_COUNT_LIMIT),{"count":this.§_-02Q§});
            §_-fH§.§_-h2r§(_loc1_,_loc2_);
            return false;
         }
         return true;
      }
      
      public function §_-63D§() : Array
      {
         return this.§_-QW§;
      }
      
      public function get §_-E3e§() : Boolean
      {
         return Boolean(this.§_-QW§) && this.§_-QW§.length > 0;
      }
      
      public function get §_-w1m§() : Boolean
      {
         return this.§_-f28§;
      }
      
      public function §_-535§(param1:§_-E19§) : int
      {
         var _loc3_:DepositStructure = null;
         var _loc2_:int = 0;
         var _loc4_:* = int(this.§_-QW§.length);
         while(_loc4_--)
         {
            _loc3_ = this.§_-QW§[_loc4_];
            if(_loc3_.moneyType == param1.id)
            {
               _loc2_ += _loc3_.dividendsByDays[_loc3_.progress];
            }
         }
         return _loc2_;
      }
      
      public function §_-H3q§() : void
      {
         var depositDialog:DepositIncomeWindow = new DepositIncomeWindow(this.§_-63D§(),this.§_-w1m§);
         §_-52k§.single(depositDialog,§_-M4§.WINDOW_CLOSE,function(param1:starling.events.Event):void
         {
            dispatchEventWith(starling.events.Event.CHANGE);
         });
         depositDialog.show();
      }
      
      public function §_-T2Q§() : void
      {
         §_-J2l§.§_-u2b§.show(this.§_-s21§);
      }
      
      private function §_-s21§() : void
      {
         var _loc1_:GetDividend = null;
         if(this.§_-f28§)
         {
            _loc1_ = new GetDividend();
            §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc1_,GetDividendResult,this.§_-G2B§,this.§_-8Y§);
         }
         else
         {
            this.§_-8Y§();
         }
      }
      
      private function §_-G2B§(param1:§_-63i§) : void
      {
         if(this.disposed)
         {
            return;
         }
         var _loc2_:GetDividendResult = GetDividendResult(param1.message);
         if(_loc2_.result == Result.SUCCESS)
         {
            Application.userProfile.§_-H3i§(_loc2_.realMoney);
            Application.userProfile.§_-oA§(_loc2_.money);
            §_-X1j§.§_-j14§.trackMoney(_loc2_.money,_loc2_.realMoney,Analytics.BUY_VIRTUAL_CURRENCY);
            this.§_-c2V§();
         }
         this.§_-f28§ = false;
         this.§_-8Y§();
      }
      
      private function §_-8Y§(param1:* = null) : void
      {
         if(this.disposed)
         {
            return;
         }
         §_-J2l§.§_-u2b§.hide();
         dispatchEventWith(starling.events.Event.CHANGE);
      }
      
      private function §_-c2V§() : void
      {
         var _loc2_:DepositStructure = null;
         var _loc1_:int = int(this.§_-QW§.length);
         while(--_loc1_ > -1)
         {
            _loc2_ = this.§_-QW§[_loc1_];
            if(_loc2_.progress == 4)
            {
               this.§_-QW§.pop();
            }
         }
      }
   }
}

