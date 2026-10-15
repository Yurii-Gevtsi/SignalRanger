package ru.pragmatix.wormix.objects
{
   import §_-12a§.§_-f1J§;
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-62§.*;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-L2F§.§_-D2p§;
   import §_-P2i§.§_-b7§;
   import §_-Ty§.§_-K3a§;
   import §_-Y1b§.*;
   import §_-b1F§.§_-G4§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2W§.§_-21w§;
   import §_-zI§.*;
   import com.distriqt.extension.inappbilling.Purchase;
   import feathers.core.ToggleGroup;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.NotStuckingPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.weapons.Girder;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.text.TextField;
   import starling.text.TextFieldAutoSize;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-X1i§
   {
      
      private static const §_-43t§:int = -1;
      
      private static const §_-8b§:uint = 0;
      
      private static const §_-C2a§:uint = 1;
      
      private static const UNIT:uint = 2;
      
      private static const §_-K17§:uint = 3;
      
      private static const §_-r2E§:uint = 4;
      
      private static const §_-X28§:uint = 5;
      
      private static const §_-X2l§:uint = 6;
      
      private var storage:Array = [new Vector.<§_-F3o§>(0),new Vector.<§_-F3o§>(0),new Vector.<§_-01J§>(0),new Vector.<§_-F3o§>(0),new Vector.<§_-F3o§>(0),new Vector.<§_-F3o§>(0),new Vector.<§_-F3o§>(0)];
      
      private var §_-w2e§:Vector.<Vector.<int>> = new Vector.<Vector.<int>>(0);
      
      private var §_-X1Q§:Vector.<int>;
      
      public function §_-X1i§()
      {
         §§push(this);
         var _temp_4:* = new <int>[0,0,0,0,0,0,0];
         §§pop().§_-X1Q§ = new <int>[0,0,0,0,0,0,0];
         super();
      }
      
      public function §_-a1N§() : Vector.<§_-F3o§>
      {
         return this.storage[§_-8b§];
      }
      
      public function §_-a2S§() : Vector.<§_-F3o§>
      {
         return this.storage[§_-C2a§];
      }
      
      public function §_-L1c§(param1:uint) : §_-F3o§
      {
         var _loc2_:Vector.<§_-F3o§> = this.storage[§_-C2a§];
         var _loc3_:int = int(_loc2_.length);
         while(--_loc3_ > -1)
         {
            if(_loc2_[_loc3_].getId() == param1)
            {
               return _loc2_[_loc3_];
            }
         }
         return null;
      }
      
      public function §_-MM§(param1:Array) : Vector.<§_-F3o§>
      {
         var _loc3_:§_-F3o§ = null;
         var _loc4_:Class = null;
         var _loc2_:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
         for each(_loc3_ in this.storage[§_-C2a§])
         {
            for each(_loc4_ in param1)
            {
               if(_loc3_ is _loc4_)
               {
                  _loc2_.push(_loc3_);
                  break;
               }
            }
         }
         return _loc2_;
      }
      
      public function §_-A1n§(param1:Class, param2:Function) : void
      {
         var _loc4_:§_-F3o§ = null;
         var _loc3_:Vector.<§_-F3o§> = this.storage[§_-C2a§];
         var _loc5_:int = int(_loc3_.length);
         while(--_loc5_ > -1)
         {
            _loc4_ = _loc3_[_loc5_];
            if(_loc4_ is param1)
            {
               if(param2(_loc4_))
               {
                  break;
               }
            }
         }
      }
      
      public function §_-JB§(param1:§_-D3B§) : Vector.<§_-F3o§>
      {
         var _loc3_:§_-F3o§ = null;
         var _loc2_:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
         for each(_loc3_ in this.storage[§_-C2a§])
         {
            if(_loc3_.§_-l28§(param1))
            {
               _loc2_.push(_loc3_);
            }
         }
         return _loc2_;
      }
      
      public function getUnits() : Vector.<§_-01J§>
      {
         return this.storage[UNIT];
      }
      
      public function §_-434§(param1:uint) : Vector.<§_-F3o§>
      {
         return this.storage[§_-K17§ + param1];
      }
      
      public function §_-DH§() : Boolean
      {
         var _loc1_:§_-F3o§ = null;
         for each(_loc1_ in this.storage[§_-8b§])
         {
            if(_loc1_.type == ObjectType.GAME_OBJECT && !_loc1_.stable)
            {
               return false;
            }
         }
         return true;
      }
      
      public function §_-4c§() : Vector.<§_-F3o§>
      {
         var _loc2_:§_-F3o§ = null;
         var _loc1_:Vector.<§_-F3o§> = new Vector.<§_-F3o§>(0);
         for each(_loc2_ in this.storage[§_-8b§])
         {
            if(_loc2_.type == ObjectType.GAME_OBJECT && !_loc2_.stable)
            {
               _loc1_.push(_loc2_);
            }
         }
         return _loc1_;
      }
      
      public function put(param1:§_-F3o§, param2:Boolean) : void
      {
         var _loc3_:Vector.<int> = new Vector.<int>(this.storage.length,true);
         _loc3_[§_-8b§] = this.§_-RJ§(param1,§_-8b§);
         _loc3_[§_-C2a§] = param2 ? int(this.§_-RJ§(param1,§_-C2a§)) : §_-43t§;
         _loc3_[UNIT] = param1 is §_-01J§ ? int(this.§_-RJ§(param1,UNIT)) : §_-43t§;
         _loc3_[§_-K17§] = §_-43t§;
         var _loc4_:uint = §_-r2E§;
         while(_loc4_ <= §_-X2l§)
         {
            _loc3_[_loc4_] = param1.physParams.collideType == _loc4_ - §_-K17§ ? int(this.§_-RJ§(param1,_loc4_)) : §_-43t§;
            _loc4_++;
         }
         this.§_-w2e§.push(_loc3_);
      }
      
      private function §_-RJ§(param1:§_-F3o§, param2:uint) : uint
      {
         var _loc3_:int = int(this.storage[param2].length);
         this.storage[param2].push(param1);
         ++this.§_-X1Q§[param2];
         return _loc3_;
      }
      
      public function §_-f1h§(param1:int) : §_-F3o§
      {
         var _loc2_:§_-F3o§ = this.storage[§_-8b§][param1];
         var _loc3_:Vector.<int> = this.§_-w2e§[param1];
         var _loc4_:int = 0;
         while(_loc4_ < this.storage.length)
         {
            if(_loc3_[_loc4_] != -1)
            {
               this.storage[_loc4_].splice(_loc3_[_loc4_],1);
               var _loc5_:Vector.<int> = this.§_-X1Q§;
               var _loc6_:int;
               var _loc7_:Number = _loc5_[_loc6_ = _loc4_] - 1;
               _loc5_[_loc6_] = _loc7_;
            }
            _loc4_++;
         }
         this.§_-63C§(param1);
         return _loc2_;
      }
      
      private function §_-63C§(param1:int) : void
      {
         var _loc4_:uint = 0;
         var _loc2_:uint = this.storage.length;
         var _loc3_:int = param1 + 1;
         while(_loc3_ < this.§_-w2e§.length)
         {
            _loc4_ = 0;
            while(_loc4_ < _loc2_)
            {
               if(this.§_-w2e§[param1][_loc4_] >= 0 && this.§_-w2e§[_loc3_][_loc4_] > 0)
               {
                  --this.§_-w2e§[_loc3_][_loc4_];
               }
               _loc4_++;
            }
            _loc3_++;
         }
         this.§_-w2e§.splice(param1,1);
      }
      
      private function §_-31J§() : String
      {
         var _loc1_:String = "sizes:";
         var _loc2_:int = 0;
         while(_loc2_ < this.storage.length)
         {
            _loc1_ += " " + this.storage[_loc2_].length;
            _loc2_++;
         }
         return _loc1_;
      }
      
      public function clear() : void
      {
         var _loc1_:int = 0;
         while(_loc1_ < this.storage.length)
         {
            this.storage[_loc1_] = null;
            _loc1_++;
         }
         this.storage = null;
      }
      
      public function §_-Z1m§() : Boolean
      {
         var _loc1_:int = 0;
         if(this.§_-X1Q§.length == this.storage.length)
         {
            _loc1_ = 0;
            while(_loc1_ < this.§_-X1Q§.length)
            {
               if(this.§_-X1Q§[_loc1_] != this.storage[_loc1_].length)
               {
                  return false;
               }
               _loc1_++;
            }
            return true;
         }
         return false;
      }
   }
}

