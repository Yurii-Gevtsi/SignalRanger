package ru.pragmatix.wormix.weapons
{
   import §_-T1o§.§_-L3M§;
   import §_-b1F§.§_-G4§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Zd§;
   import §_-zI§.*;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import feathers.utils.textures.TextureCache;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.events.ProgressEvent;
   import flash.events.SecurityErrorEvent;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.events.Event;
   import starling.textures.Texture;
   
   public class Bat extends §_-72x§
   {
      
      private const POWER:uint = 28;
      
      private var ch:§_-W2f§;
      
      private var §_-11z§:§_-01J§;
      
      private var §_-tU§:§_-F3o§;
      
      private var §_-n1D§:Texture;
      
      public function Bat()
      {
         super();
         this.ch = new §_-W2f§(MathUtil.PI_ONE_SIX,MathUtil.PI_ONE_SIX,MathUtil.PI_ONE_SIX,§_-W2f§.§_-o18§,true);
         §_-z1W§(this.ch);
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         this.§_-n1D§ = Application.assetManager.getTexture("resurces/_ammo/" + param1["weaponClip"]);
         super.§_-z2Y§(param1);
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         param1.§_-m1X§ = 0;
         super.attach(param1);
      }
      
      override public function detach() : void
      {
         owner.§_-m1X§ = 0;
         super.detach();
      }
      
      override protected function §_-P1y§() : Vector.<§_-01J§>
      {
         var _loc1_:int = owner.x + 40 * Math.cos(this.ch.getAngle());
         var _loc2_:int = owner.y + 40 * Math.sin(this.ch.getAngle());
         if(_loc2_ > owner.y)
         {
            _loc2_ = owner.y;
         }
         return Physics.getUnitsInCircle(_loc1_,_loc2_,paramSet.damageRadius.value);
      }
      
      override protected function shoot() : void
      {
         if(MathUtil.equal(shotParams.angle,0) || MathUtil.equal(shotParams.angle,Math.PI))
         {
            shotParams.angle = §_-H3D§.§_-r2H§(shotParams.angle,-MathUtil.PI_HALF,MathUtil.PI_ONE_18);
         }
         else if(MathUtil.equal(shotParams.angle,MathUtil.PI_ONE_SIX) || MathUtil.equal(shotParams.angle,5 * MathUtil.PI_ONE_SIX))
         {
            shotParams.angle = §_-H3D§.§_-r2H§(shotParams.angle,-MathUtil.PI_HALF,MathUtil.PI_ONE_SIX);
         }
         ControlSets.UNIT_CONTROL_SET.deactivate();
         owner.§_-d2T§(clip);
         §_-Zd§.§_-33o§(20,this.§_-Wh§);
         this.§_-k2d§();
      }
      
      private function §_-Wh§() : void
      {
         if(owner != null)
         {
            §_-h2B§.play(§_-d27§.§_-M2a§);
         }
      }
      
      private function §_-k2d§() : void
      {
         var _loc1_:uint = 0;
         var _loc2_:uint = 0;
         if(owner != null)
         {
            this.§_-11z§ = owner;
            _loc1_ = owner.graphics.bat(this.§_-n1D§,property["offsetX"],property["offsetY"]);
            _loc2_ = 26;
            this.§_-tU§ = §_-X1j§.§_-12n§.scene.makeUnstabilizer();
            §_-Zd§.§_-33o§(_loc2_,this.§_-al§);
            §_-Zd§.§_-33o§(_loc1_,this.§_-y2k§);
            §_-Zd§.§_-33o§(Math.max(_loc2_,_loc1_),this.§_-12i§);
            owner.addEventListener(§_-L3Q§.§_-S2B§,this.§_-a1z§);
         }
      }
      
      private function §_-12i§() : void
      {
         if(this.§_-tU§)
         {
            this.§_-tU§.disposed = true;
         }
      }
      
      private function §_-al§() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:Vector.<§_-01J§> = null;
         var _loc4_:uint = 0;
         var _loc5_:§_-01J§ = null;
         var _loc6_:uint = 0;
         if(owner != null)
         {
            _loc1_ = owner.x + 40 * Math.cos(shotParams.angle);
            _loc2_ = owner.y + 40 * Math.sin(shotParams.angle);
            if(_loc2_ > owner.y)
            {
               _loc2_ = owner.y;
            }
            _loc3_ = Physics.getUnitsInCircle(_loc1_,_loc2_,paramSet.damageRadius.value);
            _loc4_ = 0;
            for each(_loc5_ in _loc3_)
            {
               if(_loc5_ != owner)
               {
                  _loc4_++;
                  _loc6_ = uint(paramSet.damage.value);
                  §_-G4§.§_-Ee§(owner,_loc1_,_loc2_,_loc5_ as §_-F3o§,_loc6_,0,paramSet.explosionRadius.value,false);
                  §_-G4§.§_-H1E§(_loc5_ as §_-F3o§,shotParams.angle,this.POWER,false);
                  this.§_-jr§(_loc5_);
               }
            }
            if(_loc4_ > 0)
            {
               §_-h2B§.play(§_-d27§.§_-K2T§);
            }
         }
      }
      
      protected function §_-jr§(param1:§_-01J§) : void
      {
      }
      
      private function §_-a1z§(param1:starling.events.Event) : void
      {
         if(owner)
         {
            owner.removeEventListener(§_-L3Q§.§_-S2B§,this.§_-a1z§);
         }
         this.§_-y2k§();
      }
      
      private function §_-y2k§() : void
      {
         if(Boolean(§_-X1j§.§_-12n§.gameMaster) && §_-X1j§.§_-12n§.gameMaster.activeUnit == this.§_-11z§)
         {
            if(owner != null)
            {
               owner.§_-l2A§();
               this.detach();
               ControlSets.UNIT_CONTROL_SET.activate();
            }
            nextTurnOnStable();
            this.§_-11z§ = null;
         }
      }
   }
}

