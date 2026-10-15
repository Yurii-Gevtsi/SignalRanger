package ru.pragmatix.wormix.objects
{
   import §_-73I§.*;
   import §_-91A§.§_-7b§;
   import §_-91A§.§_-82C§;
   import §_-91A§.§_-Wy§;
   import §_-91A§.§_-dA§;
   import §_-91A§.§_-df§;
   import §_-91A§.§_-j1G§;
   import §_-91A§.§_-te§;
   import §_-A2U§.§_-A23§;
   import §_-B2z§.§_-63i§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-H16§.*;
   import §_-H2g§.§_-di§;
   import §_-J3L§.§_-h1D§;
   import §_-L1H§.§_-b2K§;
   import §_-TF§.§_-y2n§;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-U1Y§.§_-t1b§;
   import §_-Vg§.*;
   import §_-Y1O§.*;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-G4§;
   import §_-hO§.§_-h2f§;
   import §_-j1y§.§_-i1b§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-o14§.§_-Dd§;
   import §_-p14§.§_-02J§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import feathers.controls.Panel;
   import feathers.core.FeathersControl;
   import feathers.data.IListCollection;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.utils.getDefinitionByName;
   import org.gestouch.core.GestureState;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.response.ListClansResponse;
   import ru.pragmatix.wormix.messages.clans.response.treasury.DonateClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.CatPhysicModel;
   import ru.pragmatix.wormix.physics.model.NotStuckingPhysicModel;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-L2i§;
   import ru.pragmatix.wormix.weapons.Girder;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   
   public class §_-93l§
   {
      
      private var scene:Scene;
      
      private var §_-Y1V§:flash.events.EventDispatcher;
      
      private var §_-Gj§:§_-X1i§;
      
      private var §_-iN§:int = 0;
      
      private var §_-X2F§:Boolean = false;
      
      private var §_-zJ§:Vector.<§_-Wy§>;
      
      private var §_-52w§:§_-X2V§;
      
      private var §_-223§:uint = 1;
      
      public function §_-93l§(param1:Scene, param2:flash.events.EventDispatcher)
      {
         var _loc3_:§_-Wy§ = null;
         this.§_-Gj§ = new §_-X1i§();
         this.§_-zJ§ = new Vector.<§_-Wy§>();
         this.§_-52w§ = new §_-X2V§();
         super();
         this.scene = param1;
         this.§_-Y1V§ = param2;
         param1.addEventListener(starling.events.Event.ENTER_FRAME,this.§_-22u§);
         param2.addEventListener(§_-h2f§.§_-a1F§,this.process);
         this.§_-52w§.run();
         this.§_-zJ§ = new <§_-Wy§>[new §_-te§(),new §_-df§(),new §_-7b§(),new §_-j1G§(),§_-t1b§.§_-2K§(),new §_-82C§(),new §_-dA§()];
         for each(_loc3_ in this.§_-zJ§)
         {
            _loc3_.activate();
         }
      }
      
      public function §_-qj§() : uint
      {
         return this.§_-223§++;
      }
      
      public function get cache() : §_-X1i§
      {
         return this.§_-Gj§;
      }
      
      private function process(param1:flash.events.Event) : void
      {
         var _loc4_:Vector.<§_-F3o§> = null;
         var _loc5_:§_-F3o§ = null;
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         if(this.§_-iN§ % 100 == 0 && !§_-X1j§.§_-G1a§.§_-Gb§())
         {
            this.§_-y1B§();
         }
         if(this.§_-X2F§)
         {
            this.shutdown();
            return;
         }
         var _loc2_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(_loc2_)
         {
            if(!this.§_-52w§.paused && !(_loc2_.§_-e2H§() && !this.§_-33a§.§_-92Y§() && _loc2_.§_-J33§()))
            {
               ++this.§_-iN§;
            }
         }
         else
         {
            ++this.§_-iN§;
         }
         if(this.§_-iN§ == int.MAX_VALUE)
         {
            this.§_-iN§ = int.MIN_VALUE;
         }
         var _loc3_:Boolean = this.§_-52w§.§_-l2M§(this.§_-iN§);
         if(Boolean(_loc2_) && Boolean(_loc2_.§_-m2C§()))
         {
            _loc2_.§_-m2C§().onFrame();
         }
         if(this.§_-X2F§)
         {
            this.shutdown();
            return;
         }
         if(_loc3_ == true)
         {
            _loc4_ = this.§_-Gj§.§_-a1N§();
            _loc6_ = -1;
            if(_loc2_)
            {
               _loc7_ = int(_loc4_.length);
               while(++_loc6_ < _loc7_)
               {
                  _loc5_ = _loc4_[_loc6_];
                  _loc5_.nextState();
                  if(_loc5_ is §_-01J§)
                  {
                     (_loc5_ as §_-01J§).graphics.§_-r2n§(_loc5_.clip.visible);
                  }
               }
            }
            _loc6_ = int(_loc4_.length);
            while(--_loc6_ > -1)
            {
               _loc5_ = _loc4_[_loc6_];
               if(_loc5_.disposed)
               {
                  this.§_-32a§(_loc6_);
               }
            }
            this.scene.updateWeather();
            this.scene.dispatchEventWith(§_-L3Q§.FRAME);
            this.scene.stable = this.§_-O2P§();
            if(this.scene.stable)
            {
               this.scene.dispatchEventWith(§_-L3Q§.SCENE_STABLE);
            }
         }
      }
      
      private function §_-22u§(param1:starling.events.Event) : void
      {
         var _loc3_:§_-F3o§ = null;
         var _loc4_:Vector.<§_-F3o§> = null;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         this.scene.updateOnEnterFrame();
         var _loc2_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(_loc2_)
         {
            _loc4_ = this.§_-Gj§.§_-a1N§();
            _loc5_ = int(_loc4_.length);
            _loc6_ = -1;
            while(++_loc6_ < _loc5_)
            {
               _loc3_ = _loc4_[_loc6_];
               _loc3_.updateClipPos();
            }
         }
      }
      
      public function §_-O2P§() : Boolean
      {
         return this.§_-Gj§.§_-DH§();
      }
      
      protected function §_-33Q§() : void
      {
         var _loc2_:Object = null;
         var _loc1_:uint = 0;
         for each(_loc2_ in Application.§_-i1N§)
         {
            _loc1_++;
         }
      }
      
      public function §_-L2X§(param1:§_-F3o§, param2:Boolean = false, param3:Boolean = false) : void
      {
         if(Boolean(param1.clip) && Boolean(this.scene))
         {
            this.scene.addElementToCertainSortingLayer(param1.clip,param1.§_-V2p§,param2 ? 0 : -1);
            param1.§_-q1Q§();
            if(param1.§_-92S§)
            {
               this.scene.sceneCamera.followObject(param1);
            }
         }
         this.§_-Gj§.put(param1,!param3);
         if(!param3)
         {
            this.§_-d2E§(param1);
         }
      }
      
      public function §_-l2E§(param1:§_-F3o§) : void
      {
         var _loc2_:int = this.§_-Gj§.§_-a1N§().indexOf(param1);
         if(_loc2_ >= 0)
         {
            this.§_-Gj§.§_-f1h§(_loc2_);
            this.§_-Gj§.put(param1,true);
         }
      }
      
      public function §_-d2E§(param1:§_-F3o§) : void
      {
         var _loc2_:§_-Wy§ = null;
         for each(_loc2_ in this.§_-zJ§)
         {
            if(_loc2_.test(param1))
            {
               _loc2_.addGameObject(param1);
            }
         }
      }
      
      public function §_-42N§(param1:§_-F3o§) : void
      {
         var _loc2_:§_-Wy§ = null;
         for each(_loc2_ in this.§_-zJ§)
         {
            if(_loc2_.test(param1))
            {
               _loc2_.§_-p1R§(param1);
            }
         }
      }
      
      private function §_-32a§(param1:int) : void
      {
         var _loc2_:§_-F3o§ = null;
         _loc2_ = this.§_-Gj§.§_-f1h§(param1);
         if(!_loc2_ || _loc2_.physParams == null)
         {
            return;
         }
         _loc2_.disposed = true;
         _loc2_.dispatchEventWith(§_-L3Q§.DISPOSED);
         _loc2_.defaultOnDisposed();
         if(_loc2_.clip)
         {
            this.scene.removeElement(_loc2_.clip);
         }
         this.§_-42N§(_loc2_);
      }
      
      public function §_-y1B§() : void
      {
         this.§_-X2F§ = true;
      }
      
      public function shutdown() : void
      {
         var _loc2_:§_-Wy§ = null;
         if(this.§_-52w§ == null)
         {
            return;
         }
         this.§_-52w§.stop();
         this.§_-52w§ = null;
         var _loc1_:int = int(this.§_-Gj§.§_-a1N§().length);
         while(--_loc1_ > -1)
         {
            this.§_-32a§(_loc1_);
         }
         _loc1_ = int(this.§_-Gj§.§_-a1N§().length);
         while(--_loc1_ > -1)
         {
            this.§_-32a§(_loc1_);
         }
         this.§_-Gj§.clear();
         this.scene.removeEventListener(starling.events.Event.ENTER_FRAME,this.§_-22u§);
         this.§_-Y1V§.removeEventListener(§_-h2f§.§_-a1F§,this.process);
         this.scene = null;
         this.§_-Y1V§ = null;
         for each(_loc2_ in this.§_-zJ§)
         {
            _loc2_.deactivate();
         }
         this.§_-zJ§ = null;
      }
      
      public function §_-C6§() : Boolean
      {
         return this.§_-X2F§;
      }
      
      public function §_-O1C§() : int
      {
         return this.§_-iN§;
      }
      
      public function reset() : void
      {
         this.§_-iN§ = 0;
         if(this.§_-52w§)
         {
            this.§_-52w§.reset();
         }
         if(this.scene)
         {
            this.scene.dispatchEvent(§_-K1I§.empty(1));
         }
      }
      
      public function get §_-33a§() : §_-02J§
      {
         return this.§_-52w§.§_-33a§;
      }
      
      public function §_-01O§(param1:Boolean) : void
      {
         if(this.§_-52w§)
         {
            this.§_-52w§.§_-01O§(param1);
         }
      }
      
      public function §_-01t§() : Boolean
      {
         if(this.§_-52w§)
         {
            return this.§_-52w§.§_-01t§();
         }
         return false;
      }
      
      public function §_-42D§() : void
      {
         if(this.§_-52w§)
         {
            this.§_-52w§.§_-42D§();
         }
      }
      
      public function §_-s11§() : Boolean
      {
         if(this.§_-52w§)
         {
            return this.§_-52w§.§_-s11§();
         }
         return false;
      }
      
      public function §_-9I§(param1:Array) : void
      {
         this.§_-52w§.§_-9I§(param1);
      }
      
      public function pause() : void
      {
         if(this.§_-52w§)
         {
            this.§_-52w§.paused = true;
         }
      }
      
      public function resume() : void
      {
         if(this.§_-52w§)
         {
            this.§_-52w§.paused = false;
         }
      }
      
      public function §_-m2m§() : Boolean
      {
         return this.§_-52w§ != null;
      }
   }
}

