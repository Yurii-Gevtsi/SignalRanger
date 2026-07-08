package ru.pragmatix.wormix.weapons
{
   import §_-31i§.§_-pb§;
   import §_-73d§.§_-g1J§;
   import §_-79§.§_-ps§;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-F39§.§_-Ey§;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-P2i§.§_-b7§;
   import §_-Tm§.§_-RF§;
   import §_-Y1O§.*;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-b1F§.§_-G4§;
   import §_-j1w§.§_-B2i§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-d2j§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import config.§_-V2w§;
   import flash.events.SecurityErrorEvent;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.interfaces.§_-P2v§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import ru.pragmatix.wormix.weapons.render.IRenderer;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Weapon implements §_-P2v§
   {
      
      public static const §_-8r§:String = "IconBomb";
      
      public static const §_-v1e§:String = "IconExplode";
      
      public static const §_-N1r§:String = "IconPlay";
      
      public static const §_-Et§:String = "IconStop";
      
      public static const §_-mv§:String = "IconUFOGrab";
      
      public static const §_-d2m§:uint = 1;
      
      public static const §_-aD§:uint = 2;
      
      public static const §_-vZ§:uint = 4;
      
      protected var §_-324§:uint;
      
      public var recordId:§_-TA§;
      
      public var shots:§_-TA§ = new §_-TA§(1);
      
      public var shotsLeft:§_-TA§ = new §_-TA§(0);
      
      private var §_-a2H§:Array = [];
      
      public var shotParams:§_-b2K§;
      
      protected var owner:§_-01J§;
      
      protected var §_-r2C§:Array = [];
      
      protected var §_-RS§:int = 0;
      
      protected var active:Boolean = false;
      
      public var clip:DisplayObject = null;
      
      public var §_-R1d§:Point = null;
      
      public var secondaryClip:DisplayObject = null;
      
      protected var MIN_POWER:uint;
      
      protected var MAX_POWER:uint;
      
      protected var §_-N1N§:Boolean = true;
      
      private var §_-53O§:§_-pb§;
      
      protected var renderer:IRenderer;
      
      protected var §_-B2r§:Boolean;
      
      protected var hint:String = "";
      
      protected var paramSet:§_-C3e§;
      
      protected var paramSetEx:§_-C3e§;
      
      protected var shooting:Boolean = false;
      
      protected var §_-a1R§:§_-7u§ = new §_-7u§(true);
      
      protected var §_-mG§:Boolean = true;
      
      protected var property:Object;
      
      protected var §_-YH§:Boolean = false;
      
      private var §_-l1O§:Boolean;
      
      private var isRefreshed:Boolean;
      
      public var §_-J1Q§:Boolean;
      
      public function Weapon(param1:uint = 0)
      {
         super();
         this.§_-B2r§ = this.§_-B2r§ || this is §_-y2Q§;
         this.§_-324§ = param1;
         this.isRefreshed = true;
         this.shotParams = new §_-b2K§();
      }
      
      public function get bulletCount() : int
      {
         return 1;
      }
      
      public function attach(param1:§_-01J§) : void
      {
         this.shooting = false;
         this.owner = param1;
         this.renderer.activate(this.owner);
         this.§_-RS§ = 0;
         this.shotParams.clear();
         this.shotsLeft.value = this.shots.value;
         this.§_-a2H§ = [];
         if(param1.stable)
         {
            this.activate();
         }
         if(param1.crawling != 0)
         {
            this.deactivate();
         }
         var _loc2_:int = 0;
         while(_loc2_ < this.§_-r2C§.length)
         {
            §_-m2§(this.§_-r2C§[_loc2_]).addEventListener(§_-L3Q§.§_-tN§,this.§_-O20§);
            _loc2_++;
         }
         if(§_-X1j§.§_-12n§)
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-X1j§.§_-12n§.scene.gameInterface.§_-51g§(this.recordId.value);
               §_-Ey§.§_-1H§(this.§_-l1O§ ? this.hint : "");
            }
         }
      }
      
      public function detach() : void
      {
         var _loc1_:int = 0;
         if(Boolean(this.owner != null) && Boolean(§_-X1j§.§_-12n§.gameMaster) && §_-X1j§.§_-12n§.gameMaster.activeUnit == this.owner)
         {
            this.§_-J1Q§ = false;
            this.deactivate();
            this.§_-G1Z§();
            this.renderer.deactivate(this.owner);
            this.owner.§_-81o§ = this;
            this.owner.weapon = null;
            _loc1_ = 0;
            while(_loc1_ < this.§_-r2C§.length)
            {
               §_-m2§(this.§_-r2C§[_loc1_]).removeEventListener(§_-L3Q§.§_-tN§,this.§_-O20§);
               _loc1_++;
            }
            §_-Ey§.§_-r2x§();
            this.owner = null;
         }
      }
      
      public function activate() : void
      {
         if(!this.owner || this.owner.disposed)
         {
            return;
         }
         if(!this.active && this.§_-23R§)
         {
            this.active = true;
            if(this.§_-r2C§[this.§_-RS§])
            {
               §_-m2§(this.§_-r2C§[this.§_-RS§]).activate(this.owner);
            }
            this.owner.graphics.hideHands(this.renderer.getHideHandsPattern());
            this.owner.graphics.§_-m14§(false);
            if(this.§_-J1Q§)
            {
               §_-b7§.§_-z2s§(this.owner);
            }
            else
            {
               §_-b7§.§_-63Q§(this.owner);
            }
            §_-X1j§.§_-12n§.scene.gameInterface.§_-U12§();
         }
      }
      
      public function deactivate() : void
      {
         if(this.active)
         {
            this.active = false;
            if(Boolean(§_-X1j§.§_-12n§) && Boolean(§_-X1j§.§_-12n§.gameMaster) && §_-X1j§.§_-12n§.gameMaster.activeUnit == this.owner)
            {
               §_-m2§(this.§_-r2C§[this.§_-RS§]).deactivate();
            }
         }
         if(this.owner)
         {
            this.owner.graphics.§_-i§();
            this.owner.graphics.§_-m14§(true);
            §_-b7§.§_-11U§(this.owner);
         }
      }
      
      public function isActive() : Boolean
      {
         return this.active;
      }
      
      public function rotateWeaponClip(param1:Number) : Number
      {
         if(!this.owner)
         {
            return param1;
         }
         param1 = §_-W2f§.§_-p2Q§(this.owner.direction,param1);
         if(this.clip != null)
         {
            this.clip.rotation = param1;
         }
         if(this.secondaryClip != null)
         {
            this.secondaryClip.rotation = this.owner.direction == §_-8K§.LEFT ? Math.PI : 0;
         }
         return param1;
      }
      
      public function get §_-Dg§() : uint
      {
         return this.MIN_POWER;
      }
      
      public function get §_-k2f§() : uint
      {
         return this.MAX_POWER;
      }
      
      public function §_-E2Y§() : Boolean
      {
         return this.§_-a1R§.value;
      }
      
      public function getBarrelPoint(param1:Number, param2:Point = null) : Point
      {
         var _loc4_:Number = NaN;
         var _loc5_:Number = NaN;
         var _loc3_:Point = param2 ? param2 : Pool.getPoint();
         _loc3_.setTo(this.owner.x,this.owner.y);
         if(this.clip != null && this.§_-R1d§ != null)
         {
            _loc4_ = Math.sin(param1);
            _loc5_ = Math.cos(param1);
            _loc3_.x += this.clip.x + this.§_-R1d§.x * _loc5_ - this.owner.direction * this.§_-R1d§.y * _loc4_;
            _loc3_.y += this.clip.y + this.§_-R1d§.x * _loc4_ + this.owner.direction * this.§_-R1d§.y * _loc5_;
         }
         return _loc3_;
      }
      
      protected function §_-z1W§(param1:§_-m2§) : int
      {
         return uint(this.§_-r2C§.push(param1) - 1);
      }
      
      private function §_-O20§(param1:Event) : void
      {
         var _loc2_:§_-b2K§ = param1.data as §_-b2K§;
         this.§_-GV§(_loc2_);
      }
      
      protected function §_-GV§(param1:§_-b2K§) : void
      {
         if(!this.active || this.shooting)
         {
            param1.clear();
            return;
         }
         this.deactivate();
         this.shotParams.§_-L1P§(param1);
         this.§_-q1b§(this.§_-RS§);
         if(this.§_-r2C§[this.§_-RS§ + 1])
         {
            this.§_-RS§ += 1;
            this.activate();
         }
         else
         {
            this.§_-V2X§();
         }
         this.§_-F2R§();
         param1.clear();
      }
      
      protected function §_-F2R§() : void
      {
         if(this.owner)
         {
            if(this.§_-B2r§)
            {
               this.owner.graphics.§_-i§();
            }
            else
            {
               this.owner.graphics.hideHands(this.renderer.getHideHandsPattern());
            }
         }
      }
      
      public function get needToWaitActionEnd() : Boolean
      {
         return true;
      }
      
      protected function §_-V2X§(param1:Boolean = true) : void
      {
         var _loc2_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(_loc2_.§_-E3b§() && _loc2_.§_-kB§() || _loc2_.§_-Rk§())
         {
            return;
         }
         §_-ps§.§_-93I§(this.owner);
         §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = false;
         --this.shotsLeft.value;
         this.§_-a2H§.push(this.shotsLeft.value);
         this.§_-N1W§();
         §_-X1j§.§_-12n§.scene.dispatchEventWith(§_-L3Q§.§_-51N§,false,§_-N1C§.create(this.recordId.value,this.§_-E2Y§(),this.shotsLeft.value > 0));
         this.shoot();
         if(this.§_-a1R§.value && §_-X1j§.§_-12n§.gameMaster.§_-E3b§() && !§_-X1j§.§_-12n§.gameMaster.§_-v1V§())
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"weapon selection hacked 4"));
         }
         if(param1)
         {
            this.§_-RS§ = 0;
         }
         if(this.shotsLeft.value <= 0)
         {
            if(!this.§_-YH§)
            {
               §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
            }
            if(this.§_-N1N§)
            {
               §_-X1j§.§_-12n§.gameMaster.§_-36§();
            }
         }
         else
         {
            this.activate();
         }
         §_-X1j§.§_-NA§(this + ".performShoot: randomSeed.counter= " + §_-X1j§.randomSeed.counter);
      }
      
      protected function shoot() : void
      {
      }
      
      protected function §_-q1b§(param1:int) : void
      {
      }
      
      protected function onFrame(param1:Event) : void
      {
      }
      
      protected function nextTurnOnStable() : void
      {
         §_-X1j§.§_-12n§.gameMaster.nextTurnOnStable();
      }
      
      protected function §_-K2b§() : void
      {
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
      }
      
      protected function §_-G1Z§() : void
      {
         var _loc1_:Scene = §_-X1j§.§_-12n§.scene;
         if(_loc1_ != null)
         {
            _loc1_.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
         }
      }
      
      public function §_-Q2H§() : §_-C3e§
      {
         return this.paramSet;
      }
      
      public function §_-d1P§(param1:§_-C3e§) : void
      {
         this.paramSet = param1;
      }
      
      public function §_-B1G§(param1:§_-C3e§) : void
      {
         this.paramSetEx = param1;
      }
      
      public function shootWithParams(param1:§_-b2K§) : void
      {
         this.shotParams.clear();
         this.shotParams = param1;
         if(Boolean(param1.angle) && this.clip != null)
         {
            this.clip.rotation = param1.angle;
         }
         var _loc2_:§_-m2§ = this.§_-G3H§();
         if(_loc2_)
         {
            _loc2_.deactivate();
         }
         this.§_-V2X§();
      }
      
      public function §_-G3H§() : §_-m2§
      {
         return this.§_-r2C§[this.§_-RS§];
      }
      
      public function §_-Yz§(param1:uint) : uint
      {
         return this.MIN_POWER + (this.MAX_POWER - this.MIN_POWER) * param1 / §_-d2j§.MAX_POWER;
      }
      
      public function powerToCharge(param1:uint) : uint
      {
         return Math.round((param1 - this.MIN_POWER) * §_-d2j§.MAX_POWER / (this.MAX_POWER - this.MIN_POWER));
      }
      
      public function get §_-Y2U§() : §_-pb§
      {
         return this.§_-53O§;
      }
      
      public function set §_-Y2U§(param1:§_-pb§) : void
      {
         this.§_-53O§ = param1;
      }
      
      public function §_-P1d§() : void
      {
      }
      
      public function §_-02Y§() : void
      {
      }
      
      public function §_-im§() : void
      {
      }
      
      public function §_-u1A§() : void
      {
      }
      
      public function §_-X1c§(param1:uint) : void
      {
      }
      
      public function §_-F2e§() : Boolean
      {
         return (this.§_-324§ & §_-d2m§) > 0;
      }
      
      public function §_-vq§() : Boolean
      {
         return (this.§_-324§ & §_-aD§) > 0;
      }
      
      public function §_-Q2I§() : Boolean
      {
         return (this.§_-324§ & §_-vZ§) > 0;
      }
      
      public function §_-w2P§() : IRenderer
      {
         return this.renderer;
      }
      
      public function §_-W18§(param1:IRenderer) : void
      {
         this.renderer = param1;
      }
      
      public function §_-U1G§(param1:String) : void
      {
         var _loc4_:§_-IE§ = null;
         var _loc5_:Boolean = false;
         this.hint = param1;
         var _loc2_:int = §_-V2w§.§_-em§(this.recordId.value) ? §_-V2w§.§_-5H§(this.recordId.value) : this.recordId.value;
         var _loc3_:Boolean = false;
         for each(_loc4_ in §_-Y2u§.§_-uO§())
         {
            if(_loc4_.prevUpgradeId == _loc2_ || _loc4_.upgradeId == _loc2_)
            {
               _loc3_ = true;
               break;
            }
         }
         _loc5_ = §_-Cq§.§_-327§(_loc2_);
         this.§_-l1O§ = !_loc3_ || _loc5_;
      }
      
      public function setProperty(param1:Object) : void
      {
      }
      
      public function §_-z2Y§(param1:Object) : void
      {
         this.property = param1;
      }
      
      public function getProperty(param1:String) : Object
      {
         return this.property[param1];
      }
      
      public function §_-f22§(param1:String) : Object
      {
         if(this.property[param1] !== undefined)
         {
            return this.property[param1];
         }
         return 0;
      }
      
      public function §_-h1N§() : Boolean
      {
         return false;
      }
      
      public function §_-82J§() : Boolean
      {
         return this.shooting;
      }
      
      public function §_-5p§() : Boolean
      {
         var _loc1_:§_-d2j§ = this.§_-G3H§() as §_-d2j§;
         return Boolean(_loc1_) && _loc1_.§_-5p§();
      }
      
      public function §_-U2Y§() : Boolean
      {
         return this.§_-mG§;
      }
      
      public function get §_-23R§() : Boolean
      {
         return this.isRefreshed;
      }
      
      public function set §_-23R§(param1:Boolean) : void
      {
         this.isRefreshed = param1;
      }
      
      public function get §_-L2s§() : Boolean
      {
         return false;
      }
      
      public function §_-319§() : uint
      {
         var _loc1_:§_-m2§ = this.§_-r2C§[int(this.§_-r2C§.length - 1)];
         if(_loc1_.§_-K1C§(§_-RF§.§_-s17§))
         {
            return §_-RF§.§_-s17§;
         }
         if(_loc1_.§_-K1C§(§_-RF§.§_-G1H§))
         {
            return §_-RF§.§_-G1H§;
         }
         return §_-RF§.§_-St§;
      }
      
      private function §_-N1W§() : void
      {
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§() && !§_-X1j§.§_-12n§.isLearning && (§_-X1j§.§_-12n§.gameMaster.§_-kB§() || this.shotsLeft.value + this.§_-a2H§.length != this.shots.value))
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A3T§,"weapon shots hacked"));
         }
      }
   }
}

