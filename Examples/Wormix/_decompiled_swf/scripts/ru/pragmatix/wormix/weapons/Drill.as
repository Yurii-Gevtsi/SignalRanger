package ru.pragmatix.wormix.weapons
{
   import §_-51M§.Cat;
   import §_-A2F§.§_-4B§;
   import §_-H2g§.§_-di§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-73L§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Ia§;
   import §_-x2Q§.§_-dU§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-13q§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Drill extends Weapon implements §_-y2Q§
   {
      
      protected var §_-l1w§:int;
      
      protected var §_-s2n§:int;
      
      protected var §_-b1p§:int;
      
      protected var worm:§_-01J§;
      
      private var setPlug:Boolean = false;
      
      private var §_-g2y§:Vector.<Point>;
      
      private var allowTorch:Boolean = false;
      
      private var §_-E11§:Boolean;
      
      protected var §_-H1Z§:Boolean = false;
      
      protected const §_-h8§:int = 20;
      
      protected const DAMAGE:int = 5;
      
      private var armature:§_-di§;
      
      public function Drill()
      {
         super();
         this.§_-l1w§ = §_-z1W§(new §_-cZ§());
         §_-YH§ = true;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         if(param1["allowTorch"])
         {
            this.allowTorch = param1["allowTorch"];
         }
      }
      
      override protected function shoot() : void
      {
         this.§_-b1p§ = §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-O1C§();
         if(property["setPlug"])
         {
            this.setPlug = property["setPlug"];
            this.§_-g2y§ = new Vector.<Point>();
         }
         ControlSets.UNIT_CONTROL_SET.deactivate();
         this.§_-s2n§ = 0;
         this.armature = §_-K1t§.buildArmature("resurces/_ammo/".concat(paramSet.ammoClassName),§_-K1t§.§_-Y1c§,§_-K1t§.§_-t1V§);
         WorldClock.clock.add(this.armature);
         this.armature.display.scaleX = this.armature.display.scaleY = §_-K1t§.§_-31c§;
         this.§_-H1Z§ = true;
         if(owner.direction == §_-8K§.LEFT)
         {
            this.armature.display.rotation = Math.PI;
         }
         owner.§_-U8§(this.armature.display);
         owner.addEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         owner.stable = false;
         owner.keepUnstable = true;
         this.§_-E11§ = true;
         owner.buffs.place(new §_-73L§());
         ControlSets.WEAPON_CONTROL_SET.activate();
         owner.graphics.§_-m14§(false);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-Et§);
      }
      
      private function onCollide(param1:Event) : void
      {
         var _loc2_:Boolean = false;
         if(owner.speedY <= 0)
         {
            return;
         }
         ++this.§_-s2n§;
         _loc2_ = owner.y > §_-X1j§.§_-12n§.scene.sceneH - §_-X1j§.§_-12n§.scene.getWaterHeight() + 15;
         var _loc3_:uint = _loc2_ ? uint(this.§_-h8§ * 2) : uint(this.§_-h8§);
         if(this.§_-s2n§ >= _loc3_)
         {
            this.detach();
            return;
         }
         owner.addSpeedXY(0,-5);
         owner.graphics.§_-m14§(false);
         §_-G4§.§_-Ee§(owner,param1.data.x,param1.data.y + Physics.UNIT_HALF_HEIGHT * 0.5,null,this.DAMAGE,Physics.UNIT_HALF_HEIGHT + 16,19,false,false,DamageType.DRILL);
         §_-G4§.§_-Ee§(owner,param1.data.x,param1.data.y + Physics.UNIT_HALF_HEIGHT,null,this.DAMAGE,Physics.UNIT_HALF_HEIGHT + 16,this.§_-E11§ ? 19 : 15,false,false,DamageType.DRILL);
         if(this.§_-E11§)
         {
            this.§_-E11§ = false;
         }
         if(this.setPlug)
         {
            this.§_-g2y§.push(Pool.getPoint(param1.data.x,param1.data.y + Physics.UNIT_HALF_HEIGHT));
            this.setPlug = !§_-4B§.§_-V1o§(owner,this.§_-g2y§,true);
         }
         param1.stopPropagation();
         if((this.§_-s2n§ - 1) % 4 == 0)
         {
            §_-h2B§.play(§_-d27§.§_-d2A§);
         }
         if(Boolean(owner) && !owner.disposed)
         {
            owner.x = param1.data.x;
            owner.y = param1.data.y;
         }
      }
      
      private function §_-H1C§() : void
      {
         var auger:§_-Oi§;
         var gameMaster:§_-eX§ = null;
         var backpack:§_-2m§ = null;
         var tmpOwner:§_-01J§ = null;
         var turnNumber:uint = 0;
         var augerDelay:uint = 0;
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         if(!this.§_-H1Z§)
         {
            return;
         }
         this.§_-H1Z§ = false;
         this.§_-s2n§ = this.§_-h8§;
         ControlSets.WEAPON_CONTROL_SET.deactivate();
         if(this.armature.display)
         {
            owner.§_-n28§(this.armature.display);
         }
         owner.removeEventListener(§_-L3Q§.§_-o13§,this.onCollide);
         ControlSets.UNIT_CONTROL_SET.activate();
         gameMaster = §_-X1j§.§_-12n§.gameMaster;
         backpack = owner.squad.§_-B2M§().§_-V2J§();
         auger = §_-Cq§.§_-J2a§(§_-q24§.AUGER,backpack);
         §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = false;
         if(this.allowTorch && gameMaster.§_-42C§() && Boolean(auger))
         {
            gameMaster.§_-36§(false);
            gameMaster.§_-c2w§(false);
            tmpOwner = owner;
            §_-Q2v§.§_-01z§(auger.getId());
            tmpOwner.§_-P1W§(auger.getId());
            tmpOwner.attributes.jump.§_-03m§();
            if(tmpOwner is Cat)
            {
               Cat(tmpOwner).disableCatJump();
            }
            turnNumber = gameMaster.§_-82g§();
            augerDelay = 45;
            §_-Ia§.§_-33o§(augerDelay,function():void
            {
               if(turnNumber == gameMaster.§_-82g§())
               {
                  §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = false;
                  tmpOwner.attributes.jump.§_-ZG§();
                  if(tmpOwner is Cat)
                  {
                     Cat(tmpOwner).enableCatJump();
                  }
                  if(!gameMaster.§_-v1V§())
                  {
                     gameMaster.§_-c2w§();
                     if(!tmpOwner.disposed && Boolean(tmpOwner.weapon))
                     {
                        tmpOwner.weapon.detach();
                     }
                     nextTurnOnStable();
                  }
               }
            });
         }
         else
         {
            nextTurnOnStable();
         }
      }
      
      public function weaponCharge() : void
      {
         this.§_-s2n§ = this.§_-h8§;
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-n2O§(Weapon.§_-N1r§);
      }
      
      override public function detach() : void
      {
         §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         this.§_-H1C§();
         if(owner)
         {
            owner.buffs.§_-e1z§(§_-73L§.NAME);
            owner.addSpeedXY(0,-1);
            owner.keepUnstable = false;
         }
         if(this.armature)
         {
            WorldClock.clock.remove(this.armature);
            this.armature.dispose();
            this.armature = null;
         }
         super.detach();
      }
   }
}

