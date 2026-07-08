package ru.pragmatix.wormix.controller
{
   import §_-31N§.§_-42G§;
   import §_-73d§.§_-g1J§;
   import §_-931§.§_-i2r§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-fH§;
   import §_-Ly§.§_-81L§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import §_-z1g§.§_-522§;
   import dragonBones.animation.WorldClock;
   import flash.events.Event;
   import flash.geom.Rectangle;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import org.gestouch.core.Touch;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.SteamTransactionInitRequest;
   import ru.pragmatix.wormix.messages.server.ReorderGroupResult;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class §_-yM§ extends EventDispatcher
   {
      
      private var _starling:Starling;
      
      private var §_-33J§:Number;
      
      public function §_-yM§(param1:Starling)
      {
         super();
         this._starling = param1;
         if(this._starling)
         {
            this._starling.nativeStage.addEventListener(flash.events.Event.ENTER_FRAME,this.§_-52a§);
         }
         this.§_-33J§ = getTimer();
      }
      
      public function dispose() : void
      {
         if(this._starling)
         {
            this._starling.nativeStage.removeEventListener(flash.events.Event.ENTER_FRAME,this.§_-52a§);
         }
      }
      
      private function §_-52a§(param1:flash.events.Event) : void
      {
         var _loc2_:Number = getTimer();
         var _loc3_:Number = _loc2_ - this.§_-33J§;
         this.§_-33J§ = _loc2_;
         WorldClock.clock.advanceTime(_loc3_ / 1000);
      }
   }
}

