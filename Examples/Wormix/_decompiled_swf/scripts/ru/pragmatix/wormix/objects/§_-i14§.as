package ru.pragmatix.wormix.objects
{
   import §_-31Y§.§_-D3B§;
   import §_-62§.§_-zF§;
   import §_-73I§.§_-q15§;
   import §_-91A§.§_-N2O§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.Cookie;
   import §_-CF§.§_-gE§;
   import §_-D3A§.§_-TZ§;
   import §_-F39§.*;
   import §_-G2H§.*;
   import §_-P1z§.*;
   import §_-TF§.§_-y2n§;
   import §_-Y1O§.§_-42h§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1b§.§_-81l§;
   import §_-b1F§.§_-G4§;
   import §_-hO§.§_-BY§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-2m§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-qH§.§_-AC§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-wD§.*;
   import §_-z1g§.§_-lp§;
   import §_-zI§.*;
   import dragonBones.animation.WorldClock;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.NumericStepper;
   import feathers.data.ListCollection;
   import feathers.events.CollectionEventType;
   import feathers.events.FeathersEventType;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.server.RefundPvpBattleLossResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.NotStuckingPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.events.KeyboardEvent;
   import starling.text.TextField;
   import starling.text.TextFieldAutoSize;
   import starling.utils.Pool;
   
   public class §_-i14§ extends §_-A16§ implements §_-N2O§
   {
      
      private var hat:§_-zF§;
      
      public function §_-i14§(param1:String, param2:int, param3:int, param4:§_-01J§, param5:§_-zF§)
      {
         super(param1,param2,param3,param4);
         this.hat = param5;
         this.§_-F1S§();
         §_-Vw§(0,0);
      }
      
      override public function §_-93A§() : void
      {
         tags = new <§_-D3B§>[§_-D3B§.§_-l1Q§];
      }
      
      override protected function §_-gf§() : Number
      {
         return 1;
      }
      
      override public function §_-F1S§() : void
      {
         var _loc1_:Rectangle = null;
         if(§_-z1H§)
         {
            _loc1_ = §_-z1H§.getBounds(§_-z1H§);
            physParams.setGroundShift(Math.round((_loc1_.left + _loc1_.right) * §_-z1H§.scaleX * 0.5),Math.round(_loc1_.bottom * §_-z1H§.scaleX * 0.8));
            Pool.putRectangle(_loc1_);
         }
      }
      
      override protected function §_-Z2i§(param1:String) : void
      {
         var _loc2_:DisplayObject = null;
         var _loc4_:Point = null;
         §_-j2P§ = SortingLayer.DROPPED_HAT_LAYER;
         if(§_-q15§.§_-Wj§(param1))
         {
            armature = §_-q15§.§_-R26§(param1);
            if(armature)
            {
               _loc2_ = armature.display;
               WorldClock.clock.add(armature);
            }
         }
         else
         {
            _loc2_ = §_-q15§.§_-L31§(param1);
            _loc4_ = §_-AC§.§_-1Q§(param1);
            _loc2_.pivotX = -_loc4_.x * 3;
            _loc2_.pivotY = -_loc4_.y * 3;
            Pool.putPoint(_loc4_);
         }
         var _loc3_:Sprite = new Sprite();
         _loc3_.addChild(_loc2_);
         _loc3_.scaleX = _loc3_.scaleY = §_-K1t§.§_-h2z§;
         §_-z1H§ = _loc3_;
      }
      
      override protected function §_-s25§(param1:PhysParams) : IPhysicModel
      {
         return new NotStuckingPhysicModel(param1,Physics.UNIT_HALF_HEIGHT,100);
      }
      
      public function get hatId() : int
      {
         return this.hat.getId();
      }
      
      override protected function §_-a2v§() : void
      {
      }
      
      public function §_-Xo§() : DisplayObject
      {
         if(!disposed)
         {
            return §_-y2n§.§_-i1N§.§_-8N§(this.hat);
         }
         return null;
      }
      
      public function §_-b2T§() : void
      {
         dispatchEventWith(§_-BY§.HIDE);
      }
      
      public function §_-Xb§() : DisplayObject
      {
         var _loc1_:Sprite = null;
         var _loc2_:LayoutGroup = null;
         _loc1_ = §_-z1H§ as Sprite;
         _loc2_ = new LayoutGroup();
         _loc2_.width = 40;
         _loc2_.height = 40;
         _loc2_.alignPivot();
         _loc2_.name = "battle_hint_origin";
         _loc1_.addChild(_loc2_);
         return _loc2_;
      }
      
      public function §_-k1K§() : void
      {
         var _loc1_:Sprite = §_-z1H§ as Sprite;
         var _loc2_:LayoutGroup = _loc1_.getChildByName("battle_hint_origin") as LayoutGroup;
         if(_loc2_)
         {
            _loc2_.removeFromParent(true);
         }
      }
   }
}

