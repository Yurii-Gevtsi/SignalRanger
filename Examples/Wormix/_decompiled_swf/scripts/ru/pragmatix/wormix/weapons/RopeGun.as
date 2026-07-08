package ru.pragmatix.wormix.weapons
{
   import §_-21p§.§_-4w§;
   import §_-31W§.§_-r26§;
   import §_-5Y§.§_-h2c§;
   import §_-82l§.§_-A2Y§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.*;
   import §_-CR§.§_-72p§;
   import §_-K35§.§_-99§;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-Y1O§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.*;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.events.SecurityErrorEvent;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Joint;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.Rope;
   import ru.pragmatix.wormix.weapons.interfaces.§_-b29§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-jm§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-o2d§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   public class RopeGun extends Weapon implements §_-y2Q§, §_-jm§, §_-o2d§, §_-b29§
   {
      
      protected var rope:Rope;
      
      private var §_-01u§:Force = new Force("ROPE_ACC",0,0);
      
      private var §_-43H§:Point;
      
      private var §_-53f§:Boolean;
      
      protected var §_-K3W§:Boolean;
      
      protected var ropeLength:§_-TA§;
      
      protected var §_-n1x§:Texture;
      
      private const §_-I15§:Number = 0.5;
      
      private const §_-Bi§:Number = 10;
      
      private var §_-Cg§:Point;
      
      public function RopeGun(param1:uint = 0)
      {
         super(param1);
         §_-z1W§(new §_-W2f§(§_-W2f§.§_-G3n§,MathUtil.PI_HALF,0));
         MAX_POWER = 50;
         MIN_POWER = 25;
         §_-a1R§.value = false;
         §_-N1N§ = false;
         §_-YH§ = true;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         if(param1["ropeSpeed"])
         {
            MAX_POWER = param1["ropeSpeed"].value;
         }
         if(param1["extraLength"])
         {
            this.ropeLength = new §_-TA§(RopePhysicModel.MAX_LENGTH);
         }
         else
         {
            this.ropeLength = new §_-TA§(RopePhysicModel.MAX_LENGTH - 50);
         }
         this.§_-n1x§ = Application.assetManager.getTexture(param1["weaponClip"]);
         super.§_-z2Y§(param1);
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         this.§_-53f§ = false;
         §_-mG§ = true;
         super.attach(param1);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-N1r§);
      }
      
      override public function activate() : void
      {
         if(active)
         {
            return;
         }
         if(this.rope != null)
         {
            this.rope.disposed = true;
            this.§_-tg§();
            this.§_-9A§();
         }
         this.§_-K3W§ = true;
         super.activate();
      }
      
      override public function deactivate() : void
      {
         super.deactivate();
      }
      
      public function jumpShoot(param1:Boolean) : void
      {
         var _loc2_:§_-b2K§ = null;
         if(owner.crawling)
         {
            return;
         }
         if(this.§_-H2O§)
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-42C§())
            {
               §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = false;
               this.§_-K3W§ = false;
               _loc2_ = new §_-b2K§();
               _loc2_.angle = -MathUtil.PI_HALF + owner.direction * MathUtil.PI_QUARTER;
               _loc2_.§_-R18§ = param1;
               shootWithParams(_loc2_);
            }
         }
         else
         {
            this.§_-9A§();
         }
      }
      
      override protected function shoot() : void
      {
         if(owner.isImmobile())
         {
            this.§_-9A§();
         }
         else
         {
            this.§_-a1L§(shotParams.angle);
         }
      }
      
      protected function §_-a1L§(param1:Number) : void
      {
         this.§_-h2e§(param1);
         this.§_-b18§();
      }
      
      protected function §_-h2e§(param1:Number) : void
      {
         this.rope = new Rope(owner,paramSet,param1,MAX_POWER,this.ropeLength.value);
      }
      
      protected function §_-b18§() : void
      {
         var _loc1_:int = owner.x + Math.cos(shotParams.angle) * 15;
         var _loc2_:int = owner.y + Math.sin(shotParams.angle) * 15;
         this.§_-Cg§ = Pool.getPoint(_loc1_,_loc2_);
         this.§_-43H§ = Pool.getPoint(owner.x,owner.y);
         this.§_-01u§.ax = 0;
         this.rope.addEventListener(§_-L3Q§.§_-o13§,this.§_-y13§);
         this.rope.addEventListener(§_-L3Q§.DISPOSED,this.§_-92D§);
         owner.keepUnstable = true;
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-Et§);
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            ControlSets.UNIT_CONTROL_SET.deactivate();
         }
         shooting = true;
         §_-mG§ = false;
      }
      
      override public function detach() : void
      {
         if(this.rope != null)
         {
            if(this.§_-43H§ == null)
            {
               this.weaponCharge();
            }
            ControlSets.WEAPON_CONTROL_SET.deactivate();
         }
         if(owner != null && !owner.disposed)
         {
            owner.graphics.§_-i§();
            if(!owner.stable)
            {
               owner.stable = false;
               owner.keepUnstable = false;
               owner.§_-F27§(false);
            }
            owner.graphics.unlock();
         }
         super.detach();
      }
      
      private function §_-y13§(param1:starling.events.Event) : void
      {
         if(owner == null)
         {
            this.§_-tg§();
            return;
         }
         ControlSets.WEAPON_CONTROL_SET.activate();
         this.rope.removeEventListener(§_-L3Q§.§_-o13§,this.§_-y13§);
         owner.x = this.§_-43H§.x;
         owner.y = this.§_-43H§.y;
         Pool.putPoint(this.§_-43H§);
         this.§_-43H§ = null;
         owner.§_-41F§(Pool.getPoint(this.rope.x - this.rope.speedX,this.rope.y - this.rope.speedY),this.ropeLength.value);
         if(this.§_-K3W§)
         {
            owner.setSpeedXY(0,0);
         }
         owner.graphics.getContainer().rotation = 0;
         owner.graphics.rope(this.§_-n1x§);
         owner.graphics.lock();
         RopePhysicModel(owner.getPhysicModel()).physParams.addForce(this.§_-01u§);
         §_-K2b§();
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.§_-Gn§,this.§_-4N§);
      }
      
      protected function §_-r2P§() : void
      {
         owner.graphics.getContainer().rotation = owner.angle + (owner.direction == 1 ? Math.PI : 0);
      }
      
      override protected function onFrame(param1:starling.events.Event) : void
      {
         var _loc2_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(owner == null || owner.disposed || _loc2_.§_-Rk§() || _loc2_.§_-U2O§())
         {
            if(this.rope != null && !this.rope.disposed)
            {
               this.weaponCharge();
            }
            §_-G1Z§();
         }
         else
         {
            this.§_-r2P§();
         }
      }
      
      private function §_-92D§(param1:starling.events.Event) : void
      {
         var _loc2_:§_-eX§ = null;
         if(Boolean(this.rope) && Boolean(!this.rope.onPicked) && !this.§_-K3W§)
         {
            _loc2_ = §_-X1j§.§_-12n§.gameMaster;
            if(Boolean(_loc2_) && Boolean(_loc2_.activeUnit) && _loc2_.§_-42C§())
            {
               _loc2_.activeUnit.§_-vp§ = true;
            }
         }
         this.§_-tg§();
         this.§_-9A§();
         if(owner != null && !owner.disposed && Boolean(owner.attributes.jump))
         {
            owner.attributes.jump.§_-ZG§();
         }
      }
      
      private function §_-4N§(param1:starling.events.Event, param2:§_-v2Q§) : void
      {
         var _loc4_:Joint = null;
         if(!owner || owner.disposed)
         {
            return;
         }
         var _loc3_:Array = RopePhysicModel(owner.getPhysicModel()).getJoints();
         for each(_loc4_ in _loc3_)
         {
            if(§_-H3D§.distance(param2.source.x - _loc4_.x,param2.source.y - _loc4_.y) < param2.power)
            {
               this.weaponCharge();
               break;
            }
         }
      }
      
      override public function get needToWaitActionEnd() : Boolean
      {
         return false;
      }
      
      private function §_-tg§() : void
      {
         shooting = false;
         §_-mG§ = true;
         if(owner)
         {
            owner.graphics.unlock();
            owner.attributes.jump.§_-ZG§();
         }
         if(Boolean(§_-X1j§.§_-12n§.scene) && Boolean(§_-X1j§.§_-12n§.scene.gameInterface))
         {
            §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         }
         if(this.rope)
         {
            this.rope.removeEventListener(§_-L3Q§.§_-o13§,this.§_-y13§);
            this.rope.removeEventListener(§_-L3Q§.DISPOSED,this.§_-92D§);
            this.rope.disposed = true;
            this.rope = null;
         }
      }
      
      private function §_-9A§() : void
      {
         var _loc1_:§_-01J§ = owner;
         this.detach();
         if(Boolean(_loc1_) && !_loc1_.disposed)
         {
            _loc1_.reattachWeapon();
         }
         var _loc2_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(_loc2_)
         {
            if(_loc2_.activeUnit)
            {
               _loc2_.activeUnit.§_-vp§ = true;
            }
            if(_loc2_.§_-E3b§())
            {
               ControlSets.UNIT_CONTROL_SET.activate();
            }
         }
      }
      
      public function weaponCharge() : void
      {
         var _loc1_:Number = NaN;
         var _loc2_:Number = NaN;
         var _loc3_:uint = 0;
         if(this.§_-53f§)
         {
            return;
         }
         shooting = false;
         this.§_-53f§ = true;
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            ControlSets.WEAPON_CONTROL_SET.deactivate();
         }
         if(!owner.disposed)
         {
            owner.graphics.unlock();
            owner.physParams.removeForce(this.§_-01u§.name);
            _loc1_ = MathUtil.§_-F1V§(§_-H3D§.sign(owner.speedX) * Math.min(Math.abs(owner.speedX),16));
            _loc2_ = MathUtil.§_-F1V§(§_-H3D§.sign(owner.speedY) * Math.min(Math.abs(owner.speedY),16));
            if(owner.getPhysicModel() is RopePhysicModel)
            {
               RopePhysicModel(owner.getPhysicModel()).clear();
            }
            §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.§_-Gn§,this.§_-4N§);
            owner.§_-dz§();
            owner.setSpeedXY(_loc1_,_loc2_);
            if(_loc1_ > 0 && owner.direction == §_-8K§.LEFT)
            {
               owner.turnRight();
            }
            else if(_loc1_ < 0 && owner.direction == §_-8K§.RIGHT)
            {
               owner.turnLeft();
            }
            owner.graphics.getContainer().rotation = 0;
            owner.rotateUntilStable(false);
            if(Boolean(this.§_-Cg§) && §_-X1j§.§_-12n§.gameMaster.activeSquad.isLocal())
            {
               _loc3_ = §_-H3D§.distance(this.§_-Cg§.x - owner.x,this.§_-Cg§.y - owner.y) / Physics.METER;
               if(_loc3_ > 0)
               {
                  §_-X1j§.§_-12n§.battleStats.increment(§_-r26§.§_-K33§,_loc3_);
                  if(§_-X1j§.§_-12n§.battleStats.§_-B2c§())
                  {
                     Effects.floatingText(_loc3_.toString(),Effects.MISC_COLOR,owner.x,owner.y);
                  }
               }
               Pool.putPoint(this.§_-Cg§);
               this.§_-Cg§ = null;
            }
         }
         this.§_-tg§();
         if(!§_-X1j§.§_-12n§.gameMaster.§_-U2O§() && !owner.disposed)
         {
            this.§_-9A§();
         }
      }
      
      public function weaponLeftPressed() : void
      {
         this.§_-01u§.ax -= this.§_-I15§;
      }
      
      public function weaponLeftReleased() : void
      {
         this.§_-01u§.ax += this.§_-I15§;
      }
      
      public function weaponRightPressed() : void
      {
         this.§_-01u§.ax += this.§_-I15§;
      }
      
      public function weaponRightReleased() : void
      {
         this.§_-01u§.ax -= this.§_-I15§;
      }
      
      public function weaponUpPressed() : void
      {
         if(owner.getPhysicModel() is RopePhysicModel)
         {
            RopePhysicModel(owner.getPhysicModel()).deltaLength = RopePhysicModel(owner.getPhysicModel()).deltaLength - this.§_-Bi§;
         }
      }
      
      public function weaponUpReleased() : void
      {
         if(owner.getPhysicModel() is RopePhysicModel)
         {
            RopePhysicModel(owner.getPhysicModel()).deltaLength = RopePhysicModel(owner.getPhysicModel()).deltaLength + this.§_-Bi§;
         }
      }
      
      public function weaponDownPressed() : void
      {
         if(owner.getPhysicModel() is RopePhysicModel)
         {
            RopePhysicModel(owner.getPhysicModel()).deltaLength = RopePhysicModel(owner.getPhysicModel()).deltaLength + this.§_-Bi§;
         }
      }
      
      public function weaponDownReleased() : void
      {
         if(owner.getPhysicModel() is RopePhysicModel)
         {
            RopePhysicModel(owner.getPhysicModel()).deltaLength = RopePhysicModel(owner.getPhysicModel()).deltaLength - this.§_-Bi§;
         }
      }
      
      private function get §_-H2O§() : Boolean
      {
         return !owner.isImmobile() && this.rope == null;
      }
   }
}

