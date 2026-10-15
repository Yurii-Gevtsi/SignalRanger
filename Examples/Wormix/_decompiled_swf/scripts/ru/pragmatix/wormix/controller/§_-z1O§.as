package ru.pragmatix.wormix.controller
{
   import §_-12W§.*;
   import §_-22z§.*;
   import §_-62§.§_-zF§;
   import §_-73I§.§_-q15§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.*;
   import §_-B2z§.§_-82u§;
   import §_-Cl§.*;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-K35§.*;
   import §_-SP§.§_-M4§;
   import §_-T2h§.§_-43C§;
   import §_-T2h§.§_-C1R§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-Y1O§.§_-yA§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-d26§.§_-d1H§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.*;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import config.§_-B1K§;
   import feathers.controls.TabBar;
   import flash.errors.IllegalOperationError;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.server.ShopResult;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.weapons.render.IRenderer;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-z1O§
   {
      
      private var §_-Y13§:Number = new Date().time;
      
      private var battlesCount:uint = 0;
      
      private const §_-aa§:int = 5;
      
      public function §_-z1O§()
      {
         super();
      }
      
      public function §_-f2d§(param1:Number) : void
      {
         this.§_-Y13§ = param1;
      }
      
      public function §_-Vl§(param1:uint) : void
      {
         this.battlesCount = param1;
      }
      
      public function §_-w1n§() : uint
      {
         return this.battlesCount;
      }
      
      public function §_-Tv§(param1:uint) : void
      {
         this.§_-Vl§(this.battlesCount + param1);
      }
      
      public function §_-d1q§() : Number
      {
         var _loc1_:Number = new Date().time;
         return this.§_-Y13§ - _loc1_;
      }
      
      public function §_-33m§() : void
      {
         if(this.battlesCount)
         {
            --this.battlesCount;
         }
      }
      
      public function §_-KC§() : void
      {
         this.battlesCount = 0;
         this.§_-Y13§ = new Date().time;
      }
      
      public function §_-M1J§() : Boolean
      {
         return this.battlesCount > 0;
      }
   }
}

