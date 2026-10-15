package ru.pragmatix.wormix.weapons
{
   import §_-F39§.§_-a2W§;
   import §_-H2g§.§_-di§;
   import §_-P2i§.§_-b7§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import dragonBones.animation.WorldClock;
   import flash.geom.ColorTransform;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class SwitchTurn extends Weapon
   {
      
      private var §_-L1f§:§_-01J§;
      
      private var §_-43S§:§_-di§;
      
      private var §_-L0§:Weapon;
      
      private var §_-52o§:Boolean = false;
      
      private var §_-ag§:Boolean;
      
      private var §_-b1l§:Array = [null,new ColorTransform(1,1,1,1,61,-87,115)];
      
      private const §_-52f§:int = 85;
      
      public function SwitchTurn(param1:uint = 0)
      {
         super(§_-d2m§);
         §_-a1R§.value = false;
         §_-N1N§ = false;
         §_-z1W§(new §_-cZ§(this.§_-413§));
         §_-YH§ = true;
      }
      
      private function §_-413§() : Boolean
      {
         if(Boolean(this.§_-L1f§) && Boolean(owner) && this.§_-L1f§ == owner)
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.ALREADY_ACTIVE_UNIT),true);
            }
            return false;
         }
         return true;
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         this.§_-E1r§();
         super.attach(param1);
      }
      
      override public function detach() : void
      {
         var _loc1_:§_-01J§ = owner;
         super.detach();
         _loc1_.§_-81o§ = this.§_-L0§;
         _loc1_.§_-e14§(this.§_-ag§);
         _loc1_.§_-z1Q§();
         this.§_-NS§();
         this.§_-L1f§ = null;
         this.§_-L0§ = null;
      }
      
      override public function activate() : void
      {
         if(!active)
         {
            this.§_-ag§ = owner.isImmobile();
            owner.§_-e14§(true);
            super.activate();
            ControlSets.UNIT_CONTROL_SET.deactivate();
            this.§_-013§(owner);
            this.§_-L0§ = owner.§_-81o§ == null || owner.§_-81o§ is SwitchTurn ? §_-73v§.§_-z1F§(owner.record) : owner.§_-81o§;
            §_-b7§.§_-q2s§(owner);
         }
      }
      
      override public function deactivate() : void
      {
         if(active)
         {
            owner.§_-e14§(this.§_-ag§);
            this.§_-633§(this.§_-L1f§,this.§_-L1f§.getId() != owner.getId());
            super.deactivate();
            if(owner == this.§_-L1f§ && §_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               owner.§_-z1Q§();
            }
            ControlSets.UNIT_CONTROL_SET.activate();
         }
      }
      
      override public function §_-im§() : void
      {
         this.§_-G23§(Direction.LEFT);
      }
      
      override public function §_-u1A§() : void
      {
         this.§_-G23§(Direction.RIGHT);
      }
      
      private function §_-G23§(param1:Direction) : void
      {
         if(this.§_-52o§)
         {
            this.§_-013§(this.§_-EQ§(param1));
            §_-h2B§.play(§_-d27§.§_-S29§);
         }
      }
      
      override protected function shoot() : void
      {
         var _loc1_:§_-01J§ = this.§_-L1f§;
         var _loc2_:§_-01J§ = owner;
         this.detach();
         _loc2_.§_-M2z§();
         if(!_loc1_.disposed && _loc1_.stable)
         {
            if(_loc1_.§_-81o§ == null || _loc1_.§_-81o§ is SwitchTurn)
            {
               _loc1_.§_-81o§ = §_-73v§.§_-z1F§(_loc1_.record);
            }
            §_-X1j§.§_-12n§.gameMaster.§_-n14§(_loc1_ as §_-01J§);
            §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = true;
         }
      }
      
      private function §_-EQ§(param1:Direction) : §_-01J§
      {
         var _loc3_:§_-01J§ = null;
         var _loc4_:int = 0;
         var _loc2_:Array = [];
         for each(_loc3_ in §_-X1j§.§_-12n§.gameMaster.activeSquad.getUnits())
         {
            if(!_loc3_.disposed && _loc3_.stable)
            {
               _loc2_.push(_loc3_);
            }
         }
         _loc4_ = 0;
         while(_loc4_ < _loc2_.length)
         {
            if(_loc2_[_loc4_] == this.§_-L1f§)
            {
               break;
            }
            _loc4_++;
         }
         if(param1 == Direction.LEFT)
         {
            return _loc4_ == 0 ? _loc2_[_loc2_.length - 1] : _loc2_[_loc4_ - 1];
         }
         return _loc4_ == _loc2_.length - 1 ? _loc2_[0] : _loc2_[_loc4_ + 1];
      }
      
      private function §_-013§(param1:§_-01J§) : void
      {
         if(param1 != this.§_-L1f§)
         {
            this.§_-633§(this.§_-L1f§);
            this.§_-L1f§ = param1;
            this.§_-XY§(param1);
         }
      }
      
      private function §_-E1r§() : void
      {
         this.§_-43S§ = §_-K1t§.buildArmature("NextTurn",§_-K1t§.§_-Y1c§,§_-K1t§.§_-t1V§);
         WorldClock.clock.add(this.§_-43S§);
         this.§_-43S§.playAnimation(§_-K1t§.§_-Y1c§);
         this.§_-43S§.display.scaleX = this.§_-43S§.display.scaleY = §_-K1t§.§_-31c§;
      }
      
      private function §_-NS§() : void
      {
         WorldClock.clock.remove(this.§_-43S§);
         this.§_-43S§.display.removeFromParent();
         this.§_-43S§.dispose();
         this.§_-43S§ = null;
      }
      
      private function §_-XY§(param1:§_-01J§) : void
      {
         param1.§_-z1Q§();
         if(param1.stable)
         {
            this.§_-W1J§(null);
         }
         param1.addEventListener(§_-L3Q§.§_-a1V§,this.§_-W1J§);
         param1.addEventListener(§_-L3Q§.§_-S2B§,this.§_-u2i§);
      }
      
      private function §_-633§(param1:§_-01J§, param2:Boolean = true) : void
      {
         if(param1)
         {
            if(param2)
            {
               param1.§_-M2z§();
            }
            param1.removeEventListener(§_-L3Q§.§_-a1V§,this.§_-W1J§);
            param1.removeEventListener(§_-L3Q§.§_-S2B§,this.§_-u2i§);
         }
      }
      
      private function §_-W1J§(param1:Event) : void
      {
         this.§_-43S§.display.x = this.§_-L1f§.x;
         this.§_-43S§.display.y = this.§_-L1f§.y - this.§_-52f§;
         §_-MN§.§_-6G§(this.§_-43S§.display);
         var _loc2_:ColorTransform = this.§_-b1l§[this.§_-L1f§.squad.getNumber()];
         if(_loc2_)
         {
            §_-MN§.§_-F3D§(this.§_-43S§.display as DisplayObjectContainer,_loc2_.color);
         }
         §_-X1j§.§_-12n§.scene.sceneCamera.moveToObject(this.§_-L1f§ as §_-F3o§,true);
         this.§_-52o§ = true;
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(this.§_-43S§.display,SortingLayer.POINTERS_LAYER);
      }
      
      private function §_-u2i§(param1:Event) : void
      {
         this.§_-52o§ = false;
         §_-X1j§.§_-12n§.scene.removeElement(this.§_-43S§.display);
      }
   }
}

