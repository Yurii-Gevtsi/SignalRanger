package ru.pragmatix.wormix.objects
{
   import §_-62§.§_-zF§;
   import §_-82l§.§_-1I§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-P1B§.§_-b21§;
   import §_-P2i§.§_-b7§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1Q§.§_-F1g§;
   import §_-YF§.§_-q2v§;
   import §_-b1F§.§_-G4§;
   import §_-j1w§.§_-L1x§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-l2r§.§_-K1t§;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-52k§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.InviteFriendToClan;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.CatPhysicModel;
   import ru.pragmatix.wormix.physics.model.NotStuckingPhysicModel;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.weapons.SwitchTurn;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import ru.pragmatix.wormix.weapons.render.IRenderer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.ScaleMode;
   
   public class ZombieGrave extends §_-A16§
   {
      
      public static const §_-d19§:uint = 13487565;
      
      private const §_-C1C§:uint = 20;
      
      private const §_-C2O§:uint = 35;
      
      private const §_-N2G§:uint = 22;
      
      private const §_-Pg§:Number = 34;
      
      private var name:String;
      
      private var level:uint;
      
      private var squad:§_-V23§;
      
      private var aiArsenal:§_-A1l§;
      
      private var zombie:§_-di§;
      
      private var §_-DB§:UserProfile;
      
      private var isOnGround:Boolean;
      
      private var spawnOnCollide:Boolean;
      
      private var dummy:§_-F3o§;
      
      public function ZombieGrave(param1:String, param2:int, param3:int, param4:§_-01J§, param5:String, param6:uint, param7:§_-V23§, param8:Number = 1, param9:§_-A1l§ = null)
      {
         var clipName:String = param1;
         var x:int = param2;
         var y:int = param3;
         var unit:§_-01J§ = param4;
         var name:String = param5;
         var level:uint = param6;
         var squad:§_-V23§ = param7;
         var scale:Number = param8;
         var aiArsenal:§_-A1l§ = param9;
         super(clipName,x,y,unit,scale);
         this.name = name;
         this.level = level;
         this.squad = squad;
         this.aiArsenal = aiArsenal;
         this.isOnGround = false;
         §_-Zd§.once(this,§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            isOnGround = true;
            if(spawnOnCollide)
            {
               spawnZombie();
            }
         });
         this.spawnOnCollide = false;
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.spawnZombie);
      }
      
      private static function §_-91w§(param1:int) : Boolean
      {
         return param1 < §_-X1j§.§_-12n§.scene.sceneH - §_-X1j§.§_-12n§.scene.getWaterHeight() && §_-X1j§.§_-12n§.gameMaster.§_-83I§().length > 0;
      }
      
      public static function §_-b1G§(param1:String, param2:uint) : UserProfile
      {
         var _loc3_:uint = 150;
         var _loc4_:uint = param2 * 2;
         var _loc5_:uint = 0;
         return UserProfile.custom(param1,param2,§_-L1x§.§_-p21§,_loc3_,_loc5_,_loc4_);
      }
      
      private function spawnZombie(param1:Event = null) : void
      {
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.spawnZombie);
         NotStuckingPhysicModel(§_-Y2I§).radius = 2;
         NotStuckingPhysicModel(§_-Y2I§).moveLimit = 2;
         this.§_-K2q§();
         if(!this.isOnGround)
         {
            this.spawnOnCollide = true;
            return;
         }
         this.zombie.display.y = this.§_-Pg§;
         var _loc2_:Number = y - Physics.UNIT_HALF_HEIGHT;
         if(§_-91w§(_loc2_) && !HitTest.unit(x,_loc2_,true))
         {
            §_-G4§.§_-Ee§(owner,x,_loc2_,null,this.§_-C1C§,this.§_-C2O§,this.§_-N2G§,true,true);
            this.zombie.animation.gotoAndPlay("play",0,-1,1);
            armature.getSlot(§_-32P§).display.visible = true;
            armature.getSlot(§_-32P§).display.addChild(this.zombie.display);
            §_-Ia§.§_-33o§(95,this.§_-Hg§);
            this.makeStable(false);
         }
         else
         {
            disposed = true;
         }
      }
      
      private function §_-K2q§() : void
      {
         this.zombie = §_-K1t§.buildArmature(§_-A16§.§_-hd§ + "/" + §_-q2v§.§_-k2m§,null,§_-K1t§.§_-IP§);
         WorldClock.clock.add(this.zombie);
         §_-MN§.§_-V1k§(this.zombie.display,§_-d19§);
      }
      
      private function makeStable(param1:Boolean) : void
      {
         if(param1)
         {
            this.removeDummy();
         }
         else
         {
            this.§_-R2O§();
         }
         if(§_-Y2I§)
         {
            stable = param1;
            §_-Y2I§.keepUnstable = !param1;
         }
      }
      
      private function removeDummy() : void
      {
         if(Boolean(this.dummy) && !this.dummy.disposed)
         {
            this.dummy.disposed = true;
            this.dummy = null;
         }
      }
      
      private function §_-R2O§() : void
      {
         this.removeDummy();
         this.dummy = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
      }
      
      private function §_-Hg§() : void
      {
         if(disposed)
         {
            return;
         }
         var _loc1_:Number = y - Physics.UNIT_HALF_HEIGHT;
         if(!this.§_-DB§)
         {
            this.§_-DB§ = §_-b1G§(this.name,this.level);
         }
         var _loc2_:§_-01J§ = §_-L1x§.create(this.§_-DB§,x,_loc1_);
         _loc2_.§_-o1z§();
         if(this.aiArsenal)
         {
            _loc2_.aiArsenal = this.aiArsenal;
         }
         this.squad.§_-D3t§(_loc2_);
         §_-X1j§.§_-12n§.scene.gameInterface.§_-9M§();
         disposed = true;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.spawnZombie);
         this.squad = null;
         if(this.zombie)
         {
            WorldClock.clock.remove(this.zombie);
            if(this.zombie.display)
            {
               §_-MN§.§_-PN§(this.zombie.display);
               this.zombie.display.removeFromParent();
            }
            this.zombie.dispose();
            this.zombie = null;
         }
         this.makeStable(true);
         super.defaultOnDisposed(param1);
      }
      
      public function §_-E5§(param1:UserProfile) : void
      {
         this.§_-DB§ = param1;
      }
   }
}

