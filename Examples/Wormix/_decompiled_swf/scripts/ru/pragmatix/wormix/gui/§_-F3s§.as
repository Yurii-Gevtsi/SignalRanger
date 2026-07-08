package ru.pragmatix.wormix.gui
{
   import §_-12a§.KamikazeBuff;
   import §_-21A§.*;
   import §_-43V§.Random;
   import §_-51M§.Cat;
   import §_-62§.*;
   import §_-73d§.§_-pT§;
   import §_-82a§.*;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-CF§.§_-E19§;
   import §_-CR§.§_-S2o§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-TF§.§_-H2K§;
   import §_-TF§.§_-P19§;
   import §_-TF§.§_-q1j§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-g2e§.§_-q2Z§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import com.greensock.OverwriteManager;
   import com.greensock.TweenLite;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import com.greensock.plugins.AutoAlphaPlugin;
   import com.greensock.plugins.BevelFilterPlugin;
   import com.greensock.plugins.BezierPlugin;
   import com.greensock.plugins.BezierThroughPlugin;
   import com.greensock.plugins.BlurFilterPlugin;
   import com.greensock.plugins.ColorMatrixFilterPlugin;
   import com.greensock.plugins.ColorTransformPlugin;
   import com.greensock.plugins.DropShadowFilterPlugin;
   import com.greensock.plugins.EndArrayPlugin;
   import com.greensock.plugins.FrameLabelPlugin;
   import com.greensock.plugins.FramePlugin;
   import com.greensock.plugins.GlowFilterPlugin;
   import com.greensock.plugins.HexColorsPlugin;
   import com.greensock.plugins.RemoveTintPlugin;
   import com.greensock.plugins.RoundPropsPlugin;
   import com.greensock.plugins.ShortRotationPlugin;
   import com.greensock.plugins.TintPlugin;
   import com.greensock.plugins.TweenPlugin;
   import com.greensock.plugins.VisiblePlugin;
   import com.greensock.plugins.VolumePlugin;
   import com.hurlant.math.BigInteger;
   import dragonBones.animation.WorldClock;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.events.FeathersEventType;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanEmblemResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanInviteTO;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.messages.structures.UnitStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.serialization.structures.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   import starling.textures.Texture;
   import starling.utils.Align;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   public class §_-F3s§ extends §_-E2w§
   {
      
      private var btn:DisplayObject;
      
      private var scrollArea:DisplayObject;
      
      private var §_-R1F§:int = 0;
      
      private var maxValue:int = 100;
      
      private var vertical:Boolean;
      
      protected var §_-u2L§:int;
      
      protected var §_-S2f§:int;
      
      private var value:int;
      
      protected var §_-cT§:DisplayObject;
      
      protected var §_-G§:DisplayObject;
      
      protected var step:uint;
      
      private var isDragging:Boolean;
      
      public function §_-F3s§(param1:DisplayObjectContainer, param2:Boolean = true, param3:uint = 0)
      {
         super(param1);
         this.btn = getChildByName("btn");
         this.scrollArea = getChildByName("scrollArea");
         this.vertical = param2;
         if(param2)
         {
            this.§_-S2f§ = this.scrollArea.y;
            this.§_-u2L§ = this.scrollArea.height;
         }
         else
         {
            this.§_-S2f§ = this.scrollArea.x;
            this.§_-u2L§ = this.scrollArea.width;
         }
         this.btn.addEventListener(TouchEvent.TOUCH,this.§_-01s§);
         this.isDragging = false;
         this.§_-cT§ = getChildByName("up_arrow");
         this.§_-G§ = getChildByName("down_arrow");
         if(Boolean(this.§_-cT§) && !this.§_-G§)
         {
            throw new IllegalOperationError("\'down_arrow\' child not found. ScrollBar must contain \'up_arrow\' and \'down_arrow\'. Or should not contain any of the above");
         }
         if(!this.§_-cT§ && Boolean(this.§_-G§))
         {
            throw new IllegalOperationError("\'up_arrow\' child not found. ScrollBar must contain \'up_arrow\' and \'down_arrow\'. Or should not contain any of the above");
         }
         if(this.§_-cT§)
         {
            this.§_-cT§.addEventListener(TouchEvent.TOUCH,this.§_-c17§);
         }
         if(this.§_-G§)
         {
            this.§_-G§.addEventListener(TouchEvent.TOUCH,this.§_-w10§);
         }
         this.step = param3;
      }
      
      private function §_-01s§(param1:TouchEvent) : void
      {
         var _loc2_:Point = null;
         var _loc3_:Point = null;
         var _loc4_:int = 0;
         if(param1.getTouch(this.btn,TouchPhase.BEGAN))
         {
            this.isDragging = true;
         }
         if(param1.getTouch(this.btn,TouchPhase.ENDED))
         {
            this.isDragging = false;
         }
         if(this.isDragging && this.getContainer().visible)
         {
            _loc2_ = Pool.getPoint();
            _loc3_ = Pool.getPoint(param1.touches[0].globalX,param1.touches[0].globalY);
            if((_loc3_.x < 5 || _loc3_.x > getContainer().stage.width - 5) && (_loc3_.y < 5 || _loc3_.y > getContainer().stage.height - 5))
            {
               this.isDragging = false;
            }
            getContainer().globalToLocal(_loc3_,_loc2_);
            _loc4_ = this.vertical ? int(_loc2_.y) : int(_loc2_.x);
            this.setValue(this.§_-R1F§ + Math.ceil((this.maxValue - this.§_-R1F§) * (_loc4_ - this.§_-S2f§) / this.§_-u2L§));
            Pool.putPoint(_loc3_);
            Pool.putPoint(_loc2_);
         }
      }
      
      public function §_-i2y§(param1:int) : void
      {
         this.§_-R1F§ = param1;
      }
      
      public function §_-E2N§(param1:int) : void
      {
         this.maxValue = param1;
      }
      
      public function §_-CA§() : int
      {
         return this.maxValue;
      }
      
      public function §_-Eq§() : int
      {
         return this.§_-R1F§;
      }
      
      public function getValue() : int
      {
         return this.value;
      }
      
      public function setValue(param1:int, param2:Boolean = true) : void
      {
         var _loc3_:int = 0;
         if(param1 > this.maxValue)
         {
            param1 = this.maxValue;
         }
         else if(param1 < this.§_-R1F§)
         {
            param1 = this.§_-R1F§;
         }
         if(param1 != this.value)
         {
            this.value = param1;
            _loc3_ = this.§_-S2f§ + (param1 - this.§_-R1F§) * this.§_-u2L§ / (this.maxValue - this.§_-R1F§);
            this.§_-I2X§(_loc3_);
            if(param2)
            {
               this.dispatchEvent(new starling.events.Event(starling.events.Event.CHANGE));
            }
         }
      }
      
      private function §_-I2X§(param1:int) : void
      {
         if(this.vertical)
         {
            this.btn.y = param1;
         }
         else
         {
            this.btn.x = param1;
         }
      }
      
      private function §_-w10§(param1:TouchEvent) : void
      {
         if(param1.getTouch(param1.currentTarget as DisplayObject,TouchPhase.ENDED))
         {
            this.setValue(this.getValue() + this.§_-11C§());
         }
      }
      
      private function §_-c17§(param1:TouchEvent) : void
      {
         if(param1.getTouch(param1.currentTarget as DisplayObject,TouchPhase.ENDED))
         {
            this.setValue(this.getValue() - this.§_-11C§());
         }
      }
      
      public function §_-cv§(param1:int) : void
      {
         this.step = param1;
      }
      
      public function §_-11C§() : int
      {
         return this.step ? int(this.step) : int(Math.max((this.§_-CA§() - this.§_-Eq§()) / 10,1));
      }
      
      override public function dispose() : void
      {
         if(this.btn)
         {
            this.btn.addEventListener(TouchEvent.TOUCH,this.§_-01s§);
            this.btn.dispose();
            this.btn = null;
         }
         if(this.scrollArea)
         {
            this.scrollArea.dispose();
            this.scrollArea = null;
         }
         if(this.§_-cT§)
         {
            this.§_-cT§.dispose();
            this.§_-cT§ = null;
         }
         if(this.§_-G§)
         {
            this.§_-G§.dispose();
            this.§_-G§ = null;
         }
         super.dispose();
      }
   }
}

