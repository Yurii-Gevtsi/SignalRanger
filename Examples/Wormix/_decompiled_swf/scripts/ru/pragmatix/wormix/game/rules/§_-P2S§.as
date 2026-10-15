package ru.pragmatix.wormix.game.rules
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-82l§.§_-1I§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-wL§;
   import §_-D3A§.*;
   import §_-DJ§.§_-J2l§;
   import §_-H3§.*;
   import §_-J31§.§_-R1h§;
   import §_-K35§.*;
   import §_-Ly§.§_-81L§;
   import §_-P2i§.§_-b7§;
   import §_-U1i§.§_-81M§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-yA§;
   import §_-Y1b§.§_-81l§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-b1F§.§_-H3Z§;
   import §_-b1F§.§_-v2Q§;
   import §_-d2p§.*;
   import §_-hO§.§_-y2Z§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-jF§.§_-82f§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.*;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Ia§;
   import §_-wD§.*;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-M2v§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.hurlant.math.BigInteger;
   import com.worlize.websocket.§_-Y2T§;
   import com.worlize.websocket.§_-ue§;
   import dragonBones.animation.WorldClock;
   import feathers.core.ITextRenderer;
   import feathers.data.ArrayCollection;
   import feathers.layout.TiledColumnsLayout;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.mobile.*;
   import ru.pragmatix.flox.social.response.LoadUserProfilesResponse;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-U19§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.*;
   import ru.pragmatix.wormix.messages.clans.request.treasury.CashMedalsClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.treasury.CashMedalsClanResponse;
   import ru.pragmatix.wormix.messages.client.AddToGroup;
   import ru.pragmatix.wormix.messages.server.Pong;
   import ru.pragmatix.wormix.messages.structures.BossWormStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class §_-P2S§
   {
      
      public static const §_-61I§:uint = 800;
      
      public static const §_-NM§:uint = 200;
      
      private var §_-i1q§:int;
      
      private var §_-D20§:int;
      
      private var §_-52z§:int;
      
      private var §_-K3y§:uint;
      
      private var §_-U1q§:uint;
      
      private var suicide:Boolean;
      
      private var §_-B3g§:uint;
      
      private var §_-d1G§:uint;
      
      private var §_-J13§:Boolean = true;
      
      private var unit:§_-01J§;
      
      private var allies:Vector.<§_-01J§>;
      
      private var enemies:Vector.<§_-01J§>;
      
      private var §_-M3§:Array;
      
      private var §_-L1I§:Array;
      
      private var §_-6E§:Array;
      
      private var §_-a5§:Array;
      
      private var §_-jb§:Array;
      
      private var active:Boolean = false;
      
      private var §_-R1Q§:Boolean = false;
      
      private var §_-J3o§:Boolean = false;
      
      public function §_-P2S§()
      {
         super();
      }
      
      private function reset() : void
      {
         this.§_-i1q§ = 0;
         this.§_-D20§ = 0;
         this.§_-52z§ = 0;
         this.§_-B3g§ = 0;
         this.§_-d1G§ = 0;
         this.§_-K3y§ = 0;
         this.§_-U1q§ = 0;
         this.suicide = false;
         this.§_-R1Q§ = false;
         this.§_-J3o§ = false;
         this.§_-M3§ = [];
         this.§_-L1I§ = [];
         this.§_-6E§ = [];
         this.§_-a5§ = [];
         this.§_-jb§ = [];
      }
      
      public function activate(param1:§_-01J§) : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc3_:§_-01J§ = null;
         if(!this.active)
         {
            this.active = true;
            if(§_-X1j§.§_-12n§.scene)
            {
               §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.DAMAGE,this.§_-L2g§);
               §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.§_-51N§,this.§_-Ex§);
               §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.DAMAGE_TRIGGER,this.§_-Ex§);
               §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.NEW_UNIT_ADDED,this.§_-v25§);
               §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.GAME_OBJECT_HP_CHANGE,this.§_-ql§);
            }
            this.unit = param1;
            this.allies = §_-X1j§.§_-12n§.gameMaster.§_-b2W§(param1);
            this.enemies = §_-X1j§.§_-12n§.gameMaster.§_-83I§(param1);
            param1.addEventListener(§_-L3Q§.DISPOSED,this.§_-Q§);
            for each(_loc2_ in this.allies)
            {
               _loc2_.addEventListener(§_-L3Q§.DISPOSED,this.§_-92E§);
            }
            for each(_loc3_ in this.enemies)
            {
               _loc3_.addEventListener(§_-L3Q§.DISPOSED,this.§_-610§);
            }
            this.reset();
         }
      }
      
      public function deactivate() : void
      {
         var _loc1_:§_-01J§ = null;
         var _loc2_:§_-01J§ = null;
         if(this.active)
         {
            this.active = false;
            if(§_-X1j§.§_-12n§.scene)
            {
               §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.DAMAGE,this.§_-L2g§);
               §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.§_-51N§,this.§_-Ex§);
               §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.DAMAGE_TRIGGER,this.§_-Ex§);
               §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.NEW_UNIT_ADDED,this.§_-v25§);
               §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.GAME_OBJECT_HP_CHANGE,this.§_-ql§);
            }
            this.unit.removeEventListener(§_-L3Q§.DISPOSED,this.§_-Q§);
            for each(_loc1_ in this.allies)
            {
               _loc1_.removeEventListener(§_-L3Q§.DISPOSED,this.§_-92E§);
            }
            for each(_loc2_ in this.enemies)
            {
               _loc2_.removeEventListener(§_-L3Q§.DISPOSED,this.§_-610§);
            }
            this.unit = null;
            this.allies = null;
            this.enemies = null;
            this.§_-M3§ = null;
            this.§_-L1I§ = null;
            this.§_-6E§ = null;
            this.§_-a5§ = null;
            this.§_-jb§ = null;
            if(this.§_-J13§ && this.§_-D20§ > 0)
            {
               this.§_-J13§ = false;
            }
         }
      }
      
      private function §_-L2g§(param1:starling.events.Event, param2:§_-v2Q§) : void
      {
         var _loc3_:§_-01J§ = null;
         var _loc4_:int = 0;
         if(!(param2.victim is §_-01J§))
         {
            return;
         }
         _loc3_ = §_-01J§(param2.victim);
         _loc4_ = Math.ceil(_loc3_.attributes.hp.§_-J1G§ ? §_-wL§.§_-D2Q§ * param2.power : param2.power);
         var _loc5_:int = _loc4_ + (_loc3_.disposed ? _loc3_.hp : 0);
         if(this.§_-M3§[_loc3_.getId()])
         {
            this.§_-M3§[_loc3_.getId()].damage = this.§_-M3§[_loc3_.getId()].damage + _loc5_;
         }
         else
         {
            this.§_-M3§[_loc3_.getId()] = {
               "unit":_loc3_,
               "damage":_loc5_
            };
         }
         if(Boolean(_loc3_ == this.unit) && Boolean(param2.source) && param2.source.owner == this.unit)
         {
            this.§_-52z§ += _loc4_;
         }
         else if(this.allies.indexOf(_loc3_) != -1)
         {
            this.§_-i1q§ += _loc4_;
         }
         else if(this.enemies.indexOf(_loc3_) != -1)
         {
            this.§_-D20§ += _loc4_;
         }
         if(!this.§_-R1Q§)
         {
            if((§_-X1j§.§_-12n§.gameMaster.§_-E3b§() || §_-X1j§.§_-12n§.gameMaster.§_-e2H§()) && this.§_-D20§ > §_-U19§.MASSIVE_DAMAGE)
            {
               this.§_-R1Q§ = true;
               Effects.showMonsterEffect(_loc3_.x,_loc3_.y - Physics.UNIT_HEIGHT);
               §_-X1j§.§_-12n§.scene.dispatchEvent(new starling.events.Event(§_-L3Q§.MASSIVE_DAMAGE));
            }
         }
         var _loc6_:int = param2.source.§_-w1U§ - _loc3_.x;
         var _loc7_:int = param2.source.§_-i2D§ - _loc3_.y;
         var _loc8_:int = §_-H3D§.distance(_loc6_,_loc7_);
         if(param2.source.§_-U1o§ && _loc8_ > §_-61I§)
         {
            ++this.§_-B3g§;
            Effects.explodeParticleTrails(_loc3_.x,_loc3_.y,20,16632408);
         }
         if(param2.source.§_-A3d§ && _loc3_ != param2.source.owner && §_-H3D§.sign(-_loc6_) != param2.source.§_-53a§ && _loc8_ > §_-NM§)
         {
            ++this.§_-d1G§;
            Effects.explodeParticleTrails(_loc3_.x,_loc3_.y,20,8760543);
         }
         if(§_-X1j§.§_-12n§.§_-WN§().battleType == §_-81M§.§_-Y1C§ && !§_-X1j§.§_-12n§.gameMaster.activeSquad.isLocal() && !§_-X1j§.§_-12n§.gameMaster.§_-v1V§() && _loc3_.squad.getTeamId() != param2.source.owner.squad.getTeamId() && !this.§_-J3o§)
         {
            this.§_-J3o§ = true;
         }
      }
      
      private function §_-ql§(param1:§_-y2Z§) : void
      {
         if(param1.obj is §_-8K§ && param1.§_-AG§)
         {
            this.§_-Ex§(null,§_-N1C§.create(§_-q24§.§_-X2q§,false,true));
            if(this.§_-L1I§[param1.obj.getId()])
            {
               this.§_-L1I§[param1.obj.getId()].amount = this.§_-L1I§[param1.obj.getId()].amount + param1.amount;
            }
            else
            {
               this.§_-L1I§[param1.obj.getId()] = {
                  "unit":param1.obj,
                  "amount":param1.amount
               };
            }
         }
      }
      
      private function §_-Ex§(param1:starling.events.Event, param2:§_-N1C§) : void
      {
         var _loc3_:WeaponStructure = null;
         for each(_loc3_ in this.§_-6E§)
         {
            if(_loc3_.getId() == param2.recordId)
            {
               _loc3_.countIncrement();
               return;
            }
         }
         this.§_-6E§.push(new WeaponStructure(param2.recordId,1));
      }
      
      private function §_-v25§(param1:starling.events.Event, param2:§_-01J§) : void
      {
         this.§_-y2§.push(new BossWormStructure(!param2.squad.isAi(),param2.hp));
      }
      
      private function §_-Q§(param1:starling.events.Event) : void
      {
         this.§_-Y2c§.push(new BossWormStructure(!this.unit.squad.isAi()));
         this.suicide = true;
      }
      
      private function §_-92E§(param1:starling.events.Event) : void
      {
         var _loc2_:§_-8K§ = §_-8K§(param1.currentTarget);
         this.§_-Y2c§.push(new BossWormStructure(!_loc2_.squad.isAi(),_loc2_.hp));
         ++this.§_-K3y§;
      }
      
      private function §_-610§(param1:starling.events.Event) : void
      {
         var _loc2_:§_-8K§ = §_-8K§(param1.currentTarget);
         this.§_-Y2c§.push(new BossWormStructure(!_loc2_.squad.isAi(),_loc2_.hp));
         ++this.§_-U1q§;
      }
      
      public function get §_-F19§() : int
      {
         return this.§_-i1q§;
      }
      
      public function get §_-G2P§() : int
      {
         return this.§_-D20§;
      }
      
      public function get §_-m16§() : int
      {
         return this.§_-52z§;
      }
      
      public function get §_-h2x§() : int
      {
         return this.§_-52z§ + this.§_-i1q§ + this.§_-D20§;
      }
      
      public function get §_-mx§() : uint
      {
         return this.§_-K3y§;
      }
      
      public function get §_-b2m§() : uint
      {
         return this.§_-U1q§;
      }
      
      public function get §_-T1s§() : Boolean
      {
         return this.suicide;
      }
      
      public function get §_-G12§() : uint
      {
         return this.§_-B3g§;
      }
      
      public function get §_-K3H§() : uint
      {
         return this.§_-d1G§;
      }
      
      public function get §_-d2N§() : Boolean
      {
         return this.§_-J13§;
      }
      
      public function get §_-b2k§() : Array
      {
         return this.§_-M3§;
      }
      
      public function get §_-JI§() : Array
      {
         return this.§_-L1I§;
      }
      
      public function get §_-j1i§() : Array
      {
         return this.§_-6E§;
      }
      
      public function get §_-Y2c§() : Array
      {
         return this.§_-a5§;
      }
      
      public function get §_-y2§() : Array
      {
         return this.§_-jb§;
      }
      
      public function get §_-K3E§() : Boolean
      {
         return this.§_-J3o§;
      }
   }
}

