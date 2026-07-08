package ru.pragmatix.wormix.weapons.render
{
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-62I§;
   import §_-L1H§.§_-y1M§;
   import §_-j1w§.§_-B2i§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.Slot;
   import dragonBones.animation.WorldClock;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.utils.Pool;
   
   public class ClipRenderer implements IRenderer
   {
      
      private static const BARREL_BONE_NAME:String = "barrel";
      
      private static const AMMO_ARMATURE_PREFIX:String = "resurces/_ammo/";
      
      private var weapon:Weapon;
      
      private var render:§_-y1M§;
      
      private var clip:starling.display.DisplayObject;
      
      private var charParams:§_-62I§;
      
      private var secondaryCharParams:§_-62I§;
      
      private var armature:§_-di§;
      
      private var secondaryArmature:§_-di§;
      
      public function ClipRenderer(param1:Weapon, param2:§_-y1M§)
      {
         super();
         this.weapon = param1;
         this.render = param2;
      }
      
      public function get dx() : Number
      {
         return this.charParams.x;
      }
      
      public function get dy() : Number
      {
         return this.charParams.y;
      }
      
      public function activate(param1:§_-01J§) : void
      {
         var _loc3_:Slot = null;
         var _loc2_:§_-B2i§ = param1.graphics.§_-J3Q§();
         this.charParams = this.render.§_-Q1t§(_loc2_.configName);
         if(this.render.clipClass)
         {
            if(this.clip)
            {
               this.clip.dispose();
            }
            if(this.armature)
            {
               WorldClock.clock.remove(this.armature);
               this.armature.dispose();
               this.armature = null;
            }
         }
         this.clip = new Sprite();
         this.armature = this.createArmature(§_-K1t§.§_-A2u§ + this.render.clipClass);
         if(this.armature)
         {
            (this.clip as Sprite).addChild(this.armature.display);
            _loc3_ = this.armature.getSlot(BARREL_BONE_NAME);
            if(_loc3_)
            {
               if(this.weapon.§_-R1d§)
               {
                  this.weapon.§_-R1d§.setTo(_loc3_.global.x * §_-K1t§.§_-31c§,_loc3_.global.y * §_-K1t§.§_-31c§);
               }
               else
               {
                  this.weapon.§_-R1d§ = Pool.getPoint(_loc3_.global.x * §_-K1t§.§_-31c§,_loc3_.global.y * §_-K1t§.§_-31c§);
               }
            }
            this.showDragondonedHand(_loc2_,this.armature);
            this.weapon.clip = this.clip;
         }
         param1.§_-oK§(this.clip,0,0);
         if(this.render.secondaryCharParams)
         {
            this.secondaryCharParams = this.render.§_-83x§(_loc2_.configName);
         }
         if(this.render.§_-T17§)
         {
            if(this.weapon.secondaryClip)
            {
               param1.§_-d2T§(this.weapon.secondaryClip);
               WorldClock.clock.remove(this.secondaryArmature);
               this.secondaryArmature.dispose();
               this.secondaryArmature = null;
            }
            this.secondaryArmature = this.createArmature(§_-K1t§.§_-A2u§ + this.render.§_-T17§);
            if(!this.secondaryArmature)
            {
               this.secondaryArmature = §_-K1t§.buildArmature(AMMO_ARMATURE_PREFIX + this.render.§_-T17§,§_-K1t§.§_-G1E§,§_-K1t§.§_-t1V§);
            }
            if(this.secondaryArmature)
            {
               this.secondaryArmature.display.scaleX = this.secondaryArmature.display.scaleY = §_-K1t§.§_-31c§;
               this.secondaryArmature.stopAnimation();
               this.weapon.secondaryClip = this.secondaryArmature.display;
            }
            param1.§_-oK§(this.weapon.secondaryClip,this.secondaryCharParams.x,this.secondaryCharParams.y);
         }
      }
      
      private function createArmature(param1:String) : §_-di§
      {
         var _loc2_:§_-di§ = §_-K1t§.buildArmature(param1,§_-K1t§.§_-Y1c§,§_-K1t§.§_-t1V§);
         if(!_loc2_)
         {
            _loc2_ = §_-K1t§.buildArmature(AMMO_ARMATURE_PREFIX + this.render.clipClass,§_-K1t§.§_-G1E§,§_-K1t§.§_-t1V§);
         }
         if(_loc2_)
         {
            _loc2_.display.scaleX = _loc2_.display.scaleY = §_-K1t§.§_-31c§;
            WorldClock.clock.add(_loc2_);
            _loc2_.animation.play();
         }
         return _loc2_;
      }
      
      public function deactivate(param1:§_-01J§) : void
      {
         if(this.weapon.clip != null)
         {
            param1.§_-d2T§(this.weapon.clip);
            (this.clip as Sprite).removeChildren();
            this.weapon.clip.dispose();
            this.weapon.clip = null;
         }
         if(this.weapon.secondaryClip != null)
         {
            param1.§_-d2T§(this.weapon.secondaryClip);
            this.weapon.secondaryClip = null;
         }
         if(this.armature)
         {
            WorldClock.clock.remove(this.armature);
            this.armature.dispose();
            this.armature = null;
         }
         if(this.secondaryArmature)
         {
            WorldClock.clock.remove(this.secondaryArmature);
            this.secondaryArmature.dispose();
            this.secondaryArmature = null;
         }
         if(param1)
         {
            param1.graphics.§_-i§();
         }
      }
      
      public function getHideHandsPattern() : String
      {
         return Boolean(this.charParams) && Boolean(this.charParams.hideHands) ? this.charParams.hideHands : "none";
      }
      
      public function getClip() : starling.display.DisplayObject
      {
         if(this.armature)
         {
            return this.armature.display;
         }
         return this.clip;
      }
      
      public function removeHook() : void
      {
         if(this.armature)
         {
            this.armature.getBone("harpoon").visible = false;
         }
      }
      
      public function showHand(param1:§_-B2i§, param2:flash.display.DisplayObject) : void
      {
         var race:§_-B2i§ = param1;
         var wpClip:flash.display.DisplayObject = param2;
         var hands:Array = new Array();
         var hand:flash.display.DisplayObjectContainer = null;
         var i:int = 0;
         try
         {
            hand = wpClip["hand"] as flash.display.DisplayObjectContainer;
            if(hand)
            {
               hands.push(hand);
            }
            hand = wpClip["second_hand"] as flash.display.DisplayObjectContainer;
            if(hand)
            {
               hands.push(hand);
            }
         }
         catch(e:Error)
         {
         }
         for each(hand in hands)
         {
            i = 0;
            while(i < hand.numChildren)
            {
               if(hand.getChildAt(i).name != race.configName)
               {
                  hand.getChildAt(i).visible = false;
               }
               i++;
            }
         }
      }
      
      public function showDragondonedHand(param1:§_-B2i§, param2:§_-di§) : void
      {
         var slots:Vector.<Slot> = null;
         var slot:Slot = null;
         var j:int = 0;
         var race:§_-B2i§ = param1;
         var armature:§_-di§ = param2;
         var hands:Array = new Array();
         var hand:Slot = null;
         var i:int = 0;
         try
         {
            hand = armature.getSlot("hand");
            if(Boolean(hand) && hand.display is starling.display.DisplayObjectContainer)
            {
               hands.push(hand);
            }
            hand = armature.getSlot("second_hand");
            if(Boolean(hand) && hand.display is starling.display.DisplayObjectContainer)
            {
               hands.push(hand);
            }
         }
         catch(e:Error)
         {
         }
         for each(hand in hands)
         {
            slots = hand.childArmature.getSlots();
            j = int(slots.length);
            while(j--)
            {
               slot = slots[j];
               if(slot.name != race.configName)
               {
                  slot.display.visible = false;
               }
            }
         }
      }
   }
}

