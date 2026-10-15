package ru.pragmatix.wormix.gui
{
   import §_-12W§.*;
   import §_-21A§.*;
   import §_-31N§.§_-42G§;
   import §_-31W§.*;
   import §_-43V§.Random;
   import §_-52L§.§_-u2x§;
   import §_-5P§.*;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-73I§.*;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C3f§.§_-1Y§;
   import §_-Cl§.*;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H16§.*;
   import §_-H2g§.§_-di§;
   import §_-QD§.§_-G2z§;
   import §_-U1i§.§_-U16§;
   import §_-Vu§.MD5;
   import §_-Vx§.§_-X1N§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-61Z§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-Y1b§.§_-81l§;
   import §_-Y1b§.§_-OP§;
   import §_-Y1b§.§_-jH§;
   import §_-YF§.§_-G2U§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-b1R§.§_-Bx§;
   import §_-bI§.*;
   import §_-d26§.§_-d1H§;
   import §_-eO§.§_-yH§;
   import §_-fo§.*;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-hO§.§_-v24§;
   import §_-j1w§.§_-L1x§;
   import §_-l1g§.§_-L3Q§;
   import §_-p18§.§_-83X§;
   import §_-p18§.§_-V14§;
   import §_-p18§.§_-vd§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-s20§.§_-J2g§;
   import §_-t10§.§_-B29§;
   import §_-t10§.§_-S1b§;
   import §_-t10§.§_-W2z§;
   import §_-t10§.§_-xK§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import §_-ws§.§_-B3I§;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.*;
   import com.greensock.loading.core.*;
   import com.hurlant.crypto.tls.*;
   import com.hurlant.math.BigInteger;
   import com.hurlant.util.*;
   import config.§_-vR§;
   import feathers.controls.Button;
   import feathers.controls.SimpleScrollBar;
   import feathers.controls.ToggleButton;
   import feathers.core.FeathersControl;
   import feathers.events.FeathersEventType;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.geom.ColorTransform;
   import flash.net.URLRequest;
   import flash.net.URLVariables;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getDefinitionByName;
   import org.swiftsuspenders.Injector;
   import org.swiftsuspenders.§_-T1p§;
   import ru.pragmatix.flox.serialization.*;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.UfoTargetPointer;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.menu.§_-738§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.client.AchieveLogin;
   import ru.pragmatix.wormix.messages.client.MobileLogin;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.platform.§_-WK§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YD§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.utils.§_-k1h§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.FreezingMissile;
   import ru.pragmatix.wormix.weapons.ammo.Meteor;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.text.TextField;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-E2w§ extends §_-fT§
   {
      
      protected var §_-nJ§:DisplayObject;
      
      protected var children:Dictionary = new Dictionary();
      
      public function §_-E2w§(param1:DisplayObject)
      {
         super(param1);
         if(!(param1 is DisplayObjectContainer))
         {
            throw new IllegalOperationError("container must be instance of DisplayObjectContainer");
         }
         this.§_-nJ§ = this.getChildByName("disabled");
      }
      
      public function getChildByName(param1:String) : DisplayObject
      {
         return §_-YQ§.getClip(param1,this.§_-j2l§());
      }
      
      public function §_-f1p§(param1:String) : §_-fT§
      {
         var _loc3_:§_-fT§ = null;
         var _loc2_:DisplayObject = this.getChildByName(param1);
         if(_loc2_)
         {
            _loc3_ = new §_-fT§(_loc2_);
            this.§_-D1g§(_loc3_,param1);
            return _loc3_;
         }
         return null;
      }
      
      public function §_-E1C§(param1:String) : §_-E2w§
      {
         var _loc3_:§_-E2w§ = null;
         var _loc2_:DisplayObject = this.getChildByName(param1);
         if(_loc2_)
         {
            _loc3_ = new §_-E2w§(_loc2_);
            this.§_-D1g§(_loc3_,param1);
            return _loc3_;
         }
         return null;
      }
      
      public function §_-71m§(param1:String) : TextField
      {
         return this.getChildByName(param1) as TextField;
      }
      
      protected function §_-D1g§(param1:§_-fT§, param2:String) : void
      {
         if(this.children[param2])
         {
            throw new Error("Child \'" + param2 + "\' already exist");
         }
         this.children[param2] = param1;
      }
      
      override public function §_-tx§(param1:Boolean) : void
      {
         var _loc2_:int = 0;
         var _loc3_:Object = null;
         getContainer().touchable = !param1;
         super.§_-tx§(param1);
         if(this.§_-nJ§)
         {
            if(removeAllWhenDisabled)
            {
               _loc2_ = 0;
               while(_loc2_ < this.§_-j2l§().numChildren)
               {
                  _loc3_ = this.§_-j2l§().getChildAt(_loc2_);
                  _loc3_.visible = _loc3_ != this.§_-nJ§ && !param1;
                  _loc2_++;
               }
            }
            this.§_-nJ§.visible = param1;
         }
      }
      
      override protected function §_-q2a§() : void
      {
         super.§_-q2a§();
         if(this.§_-nJ§)
         {
            this.§_-tx§(disabled);
         }
      }
      
      public function addChild(param1:§_-fT§) : void
      {
         this.§_-72v§(param1.getContainer());
      }
      
      public function addChildAt(param1:§_-fT§, param2:int) : void
      {
         this.§_-13K§(param1.getContainer(),param2);
      }
      
      public function removeChild(param1:§_-fT§) : void
      {
         this.§_-k6§(param1.getContainer());
      }
      
      public function §_-72v§(param1:DisplayObject) : void
      {
         this.§_-j2l§().addChild(param1);
      }
      
      public function §_-210§(param1:DisplayObject) : Boolean
      {
         return this.§_-j2l§().contains(param1);
      }
      
      public function contains(param1:§_-fT§) : Boolean
      {
         return this.§_-210§(param1.getContainer());
      }
      
      public function §_-13K§(param1:DisplayObject, param2:int) : void
      {
         this.§_-j2l§().addChildAt(param1,param2);
      }
      
      public function §_-Y2G§(param1:DisplayObject) : int
      {
         return this.§_-j2l§().getChildIndex(param1);
      }
      
      public function §_-k6§(param1:DisplayObject) : void
      {
         this.§_-j2l§().removeChild(param1);
      }
      
      public function swapChildren(param1:§_-fT§, param2:§_-fT§) : void
      {
         this.§_-j2l§().swapChildren(param1.getContainer(),param2.getContainer());
      }
      
      public function §_-83z§(param1:DisplayObject, param2:DisplayObject) : void
      {
         this.§_-j2l§().swapChildren(param1,param2);
      }
      
      public function §_-j2l§() : DisplayObjectContainer
      {
         return getContainer() as DisplayObjectContainer;
      }
      
      protected function §_-pt§(... rest) : void
      {
         var _loc4_:Object = null;
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         var _loc7_:Boolean = false;
         var _loc2_:Number = Number.MAX_VALUE;
         var _loc3_:Number = Number.MIN_VALUE;
         for each(_loc4_ in rest)
         {
            _loc7_ = _loc4_ is DisplayObject && DisplayObject(_loc4_).visible || _loc4_ is §_-fT§ && §_-fT§(_loc4_).isVisible();
            if(_loc7_)
            {
               _loc2_ = Math.min(_loc2_,_loc4_.x);
               _loc3_ = Math.max(_loc3_,_loc4_.x + _loc4_.width);
            }
         }
         _loc5_ = (width - (_loc3_ - _loc2_)) / 2;
         _loc6_ = _loc2_ - _loc5_;
         for each(_loc4_ in rest)
         {
            _loc7_ = _loc4_ is DisplayObject && DisplayObject(_loc4_).visible || _loc4_ is §_-fT§ && §_-fT§(_loc4_).isVisible();
            if(_loc7_)
            {
               _loc4_.x -= _loc6_;
            }
         }
      }
      
      protected function §_-y2h§(... rest) : Sprite
      {
         var _loc6_:Object = null;
         var _loc7_:Boolean = false;
         var _loc2_:Sprite = new Sprite();
         var _loc3_:int = 0;
         var _loc4_:Number = 0;
         var _loc5_:Number = 0;
         for each(_loc6_ in rest)
         {
            _loc7_ = _loc6_ is DisplayObject && DisplayObject(_loc6_).visible || _loc6_ is §_-fT§ && §_-fT§(_loc6_).isVisible();
            if(_loc7_)
            {
               if(_loc3_ > 0)
               {
                  _loc6_.x -= _loc4_;
                  _loc6_.y -= _loc5_;
               }
               else
               {
                  _loc4_ = Number(_loc6_.x);
                  _loc5_ = Number(_loc6_.y);
                  _loc6_.x = 0;
                  _loc6_.y = 0;
               }
               _loc6_.y -= _loc2_.y;
               _loc2_.addChild(_loc6_ is DisplayObject ? _loc6_ as DisplayObject : _loc6_.getContainer());
               _loc3_++;
            }
         }
         _loc2_.x = _loc4_;
         _loc2_.y = _loc5_;
         this.§_-72v§(_loc2_);
         return _loc2_;
      }
      
      override public function dispose() : void
      {
         var _loc1_:§_-fT§ = null;
         for each(_loc1_ in this.children)
         {
            if(_loc1_)
            {
               _loc1_.dispose();
            }
         }
         this.children = null;
         if(this.§_-nJ§)
         {
            this.§_-nJ§.dispose();
         }
         this.§_-nJ§ = null;
         super.dispose();
      }
   }
}

