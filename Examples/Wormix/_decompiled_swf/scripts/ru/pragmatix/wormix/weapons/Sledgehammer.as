package ru.pragmatix.wormix.weapons
{
   import §_-12a§.§_-B16§;
   import §_-5Y§.§_-t2O§;
   import §_-91A§.§_-sH§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-DJ§.§_-J2l§;
   import §_-b1F§.§_-G4§;
   import §_-l1K§.§_-cZ§;
   import §_-r1C§.§_-V1R§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-v2t§.§_-73F§;
   import §_-w2i§.§_-Ia§;
   import com.amanitadesign.steam.MicroTxnAuthorizationResponse;
   import com.amanitadesign.steam.SteamConstants;
   import com.amanitadesign.steam.SteamEvent;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.messages.client.SteamTransactionCommitRequest;
   import ru.pragmatix.wormix.messages.server.SteamTransactionCommitResponse;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.ammo.cloud.BasicCloud;
   import starling.textures.Texture;
   
   public class Sledgehammer extends §_-72x§
   {
      
      private static const §_-bl§:§_-TA§ = new §_-TA§(280);
      
      private static const §_-p2S§:uint = 40;
      
      private static const §_-03G§:uint = 10;
      
      private static const §_-J1c§:uint = 22;
      
      private static const §_-v2L§:Number = 0.35;
      
      private var §_-Wh§:§_-V1R§;
      
      private var §_-ng§:Texture;
      
      public function Sledgehammer()
      {
         super();
         §_-z1W§(new §_-cZ§());
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         this.§_-ng§ = Application.assetManager.getTexture(param1["weaponClip"]);
         super.§_-z2Y§(param1);
      }
      
      override public function deactivate() : void
      {
         if(active)
         {
            super.deactivate();
            if(this.§_-Wh§ != null)
            {
               this.§_-Wh§.stop();
               this.§_-Wh§ = null;
            }
         }
      }
      
      override public function detach() : void
      {
         if(shooting)
         {
            shooting = false;
            nextTurnOnStable();
         }
         super.detach();
      }
      
      protected function §_-y2D§() : Vector.<§_-F3o§>
      {
         var x:int = owner.x + owner.direction * §_-p2S§;
         var y:int = owner.y;
         var targetObjects:Vector.<§_-F3o§> = §_-X1j§.§_-12n§.scene.gameObjectProcessor.cache.§_-MM§([§_-sH§]);
         targetObjects = targetObjects.filter(function(param1:§_-F3o§, param2:int, param3:Vector.<§_-F3o§>):Boolean
         {
            return !(param1 is BasicCloud);
         });
         return Physics.getGameObjectsInCircle(x,y,paramSet.damageRadius.value,targetObjects);
      }
      
      override protected function shoot() : void
      {
         ControlSets.UNIT_CONTROL_SET.deactivate();
         owner.§_-d2T§(clip);
         this.§_-Wh§ = §_-h2B§.play(§_-d27§.§_-B3s§);
         var _loc1_:uint = 4;
         §_-Ia§.§_-33o§(_loc1_,this.§_-k2d§);
      }
      
      private function §_-k2d§() : void
      {
         var _loc1_:uint = 0;
         if(owner != null)
         {
            _loc1_ = Math.max(owner.graphics.hammer(this.§_-ng§),§_-J1c§ + 5);
            shooting = true;
            §_-Ia§.§_-33o§(§_-J1c§,this.§_-815§,this.§_-F8§());
            §_-Ia§.§_-33o§(_loc1_,this.§_-x1i§);
         }
      }
      
      protected function §_-F8§() : uint
      {
         return §_-03G§;
      }
      
      protected function §_-815§(param1:uint) : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc3_:Vector.<§_-01J§> = null;
         var _loc4_:Vector.<§_-F3o§> = null;
         var _loc5_:§_-01J§ = null;
         var _loc6_:§_-F3o§ = null;
         var _loc7_:uint = 0;
         var _loc8_:Number = NaN;
         if(owner != null)
         {
            _loc2_ = owner;
            _loc3_ = §_-P1y§();
            _loc4_ = this.§_-y2D§();
            for each(_loc5_ in _loc3_)
            {
               _loc7_ = Math.min(§_-bl§.value,Math.max(paramSet.damage.value,_loc5_.hp * §_-v2L§));
               §_-G4§.§_-Ee§(_loc2_,_loc5_.x,_loc5_.y,_loc5_ as §_-F3o§,_loc7_,0,paramSet.explosionRadius.value,false,true);
               _loc8_ = 0;
               while(_loc8_ < param1)
               {
                  §_-X1j§.§_-12n§.scene.drawDamage(_loc5_.x,_loc5_.y + _loc8_ * 5,9);
                  _loc8_ += 0.5;
               }
               _loc5_.setSpeedXY(0,20);
               this.§_-6q§(_loc5_);
            }
            for each(_loc6_ in _loc4_)
            {
               if(!(_loc6_ is §_-01J§))
               {
                  _loc6_.disposed = true;
               }
            }
         }
      }
      
      protected function §_-6q§(param1:§_-01J§) : void
      {
         var _loc2_:int = property["statsReduction"] ? int(property["statsReduction"].value) : 0;
         if(_loc2_ > 0)
         {
            param1.buffs.place(new §_-B16§(_loc2_));
         }
      }
      
      private function §_-x1i§() : void
      {
         if(owner != null)
         {
            owner.§_-l2A§();
            this.detach();
            ControlSets.UNIT_CONTROL_SET.activate();
         }
      }
   }
}

