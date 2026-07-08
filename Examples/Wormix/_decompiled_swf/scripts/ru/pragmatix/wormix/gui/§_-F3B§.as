package ru.pragmatix.wormix.gui
{
   import §_-03T§.§_-2p§;
   import §_-21p§.§_-4w§;
   import §_-62§.§_-zF§;
   import §_-63U§.§_-21K§;
   import §_-F2M§.*;
   import §_-J31§.§_-R1h§;
   import §_-Z1w§.§_-s1x§;
   import §_-b1F§.§_-G4§;
   import §_-p24§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import com.hurlant.math.*;
   import feathers.controls.Button;
   import feathers.controls.Callout;
   import feathers.controls.Label;
   import feathers.core.DefaultToolTipManager;
   import feathers.layout.RelativePosition;
   import flash.errors.IllegalOperationError;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import starling.animation.DelayedCall;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   
   use namespace bi_internal;
   
   public class §_-F3B§ extends DefaultToolTipManager
   {
      
      private var §_-Y1§:Callout;
      
      public function §_-F3B§(param1:DisplayObjectContainer)
      {
         super(param1);
      }
      
      override protected function hoverDelayCallback() : void
      {
         var _loc1_:Function = null;
         var _loc2_:Label = null;
         if(!this._toolTip)
         {
            _loc1_ = this._toolTipFactory !== null ? this._toolTipFactory : defaultToolTipFactory;
            _loc2_ = _loc1_();
            _loc2_.maxWidth = 500;
            _loc2_.touchable = false;
            this._toolTip = _loc2_;
         }
         this._toolTip.text = this._target.toolTip;
         this._toolTip.setSize(NaN,NaN);
         this._toolTip.x = NaN;
         this._toolTip.y = NaN;
         this.§_-Y1§ = Callout.show(this._toolTip as DisplayObject,this._target as DisplayObject,[RelativePosition.BOTTOM,RelativePosition.TOP],false);
         this.§_-Y1§.disposeOnSelfClose = true;
         this.§_-Y1§.disposeContent = false;
      }
      
      override protected function root_touchHandler(param1:TouchEvent) : void
      {
         var _loc2_:Touch = null;
         var _loc3_:Number = NaN;
         if(this.§_-Y1§)
         {
            _loc2_ = param1.getTouch(DisplayObject(this._target),null,this._touchPointID);
            if(!_loc2_ || _loc2_.phase !== TouchPhase.HOVER)
            {
               if(this.§_-Y1§)
               {
                  this.§_-Y1§.close();
                  this.§_-Y1§ = null;
               }
               this._touchPointID = -1;
               this._target = null;
               this._hideTime = getTimer();
            }
            return;
         }
         if(this._target !== null)
         {
            _loc2_ = param1.getTouch(DisplayObject(this._target),null,this._touchPointID);
            if(!_loc2_ || _loc2_.phase !== TouchPhase.HOVER)
            {
               Starling.juggler.remove(this._delayedCall);
               this._touchPointID = -1;
               this._target = null;
               return;
            }
            this._toolTipX = _loc2_.globalX;
            this._toolTipY = _loc2_.globalY;
            this._delayedCall.reset(this.hoverDelayCallback,this._showDelay);
         }
         else
         {
            _loc2_ = param1.getTouch(this._root,TouchPhase.HOVER);
            if(!_loc2_)
            {
               return;
            }
            this._target = this.getTarget(_loc2_);
            if(!this._target)
            {
               return;
            }
            this._touchPointID = _loc2_.id;
            this._toolTipX = _loc2_.globalX;
            this._toolTipY = _loc2_.globalY;
            _loc3_ = (getTimer() - this._hideTime) / 1000;
            if(_loc3_ < this._resetDelay)
            {
               this.hoverDelayCallback();
               return;
            }
            if(this._delayedCall)
            {
               this._delayedCall.reset(this.hoverDelayCallback,this._showDelay);
            }
            else
            {
               this._delayedCall = new DelayedCall(this.hoverDelayCallback,this._showDelay);
            }
            Starling.juggler.add(this._delayedCall);
         }
      }
   }
}

