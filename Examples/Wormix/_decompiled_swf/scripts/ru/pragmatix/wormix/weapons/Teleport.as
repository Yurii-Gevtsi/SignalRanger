package ru.pragmatix.wormix.weapons
{
   import §_-12a§.§_-6p§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-F39§.§_-a2W§;
   import §_-P1B§.§_-b21§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-G4§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class Teleport extends Weapon
   {
      
      protected var sourceLocationDamage:Boolean = false;
      
      protected var allowAntiFreeze:Boolean = false;
      
      protected var healBonus:int = 0;
      
      protected var targetAreaMinRadius:§_-TA§;
      
      private var §_-Z1O§:int;
      
      private var §_-F3g§:int;
      
      private var §_-T1L§:Boolean;
      
      private var §_-f8§:Rectangle;
      
      private var nearestFreePoint:Point;
      
      private var §_-B1X§:Boolean = false;
      
      public function Teleport()
      {
         super();
         §_-z1W§(§_-vO§.§_-735§(this.§_-V1B§));
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         if(param1["sourceLocationDamage"])
         {
            this.sourceLocationDamage = §_-7u§(param1["sourceLocationDamage"]).value;
         }
         if(param1["targetAreaMinRadius"])
         {
            this.targetAreaMinRadius = new §_-TA§(§_-TA§(param1["targetAreaMinRadius"]).value);
         }
         if(param1["allowAntiFreeze"])
         {
            this.allowAntiFreeze = §_-7u§(param1["allowAntiFreeze"]).value;
         }
         if(param1["healBonus"])
         {
            this.healBonus = §_-TA§(param1["healBonus"]).value;
         }
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         var _loc2_:Scene = §_-X1j§.§_-12n§.scene;
         this.§_-f8§ = Pool.getRectangle(0,0,_loc2_.sceneW,_loc2_.sceneH);
         this.nearestFreePoint = Pool.getPoint();
         super.attach(param1);
      }
      
      override public function detach() : void
      {
         super.detach();
         Pool.putPoint(this.nearestFreePoint);
         this.nearestFreePoint = null;
         if(this.§_-f8§)
         {
            Pool.putRectangle(this.§_-f8§);
            this.§_-f8§ = null;
         }
      }
      
      private function §_-V1B§(param1:Number, param2:Number) : Boolean
      {
         var _loc3_:Boolean = this.§_-x1Z§(param1,param2);
         if(!_loc3_)
         {
            §_-h2B§.play(§_-d27§.§_-b2H§);
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.TELEPORT_IMPOSSIBLE),true);
            }
         }
         return _loc3_;
      }
      
      private function §_-x1Z§(param1:int, param2:int) : Boolean
      {
         if(this.§_-Z1O§ == param1 && this.§_-F3g§ == param2)
         {
            return this.§_-T1L§;
         }
         if(this.targetAreaMinRadius)
         {
            this.§_-B1X§ = Physics.nearestFreePoint(param1,param2,HitTest.unit,15,this.nearestFreePoint);
         }
         this.§_-T1L§ = !HitTest.unit(param1,param2,true) && (!this.targetAreaMinRadius || this.§_-B1X§) && this.§_-f8§.containsPoint(this.nearestFreePoint);
         this.§_-Z1O§ = param1;
         this.§_-F3g§ = param2;
         return this.§_-T1L§;
      }
      
      override protected function shoot() : void
      {
         var tmpOwner:§_-01J§ = null;
         var oldX:int = 0;
         var oldY:int = 0;
         tmpOwner = owner;
         oldX = tmpOwner.x;
         oldY = tmpOwner.y;
         var pa:PhaseAmmo = new PhaseAmmo(tmpOwner,tmpOwner.x,tmpOwner.y,true,tmpOwner as §_-F3o§,16777215,16777215,60);
         pa.§_-C2P§ = false;
         if(this.allowAntiFreeze)
         {
            §_-Zd§.once(owner as §_-F3o§,§_-L3Q§.§_-a1V§,this.onStable);
         }
         if(this.§_-B1X§)
         {
            shotParams.point.setTo(this.nearestFreePoint.x,this.nearestFreePoint.y);
         }
         §_-G4§.§_-Ee§(tmpOwner,shotParams.point.x,shotParams.point.y,null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value,false);
         Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMOKE,shotParams.point.x,shotParams.point.y,30,§_-d27§.§_-S14§);
         Physics.teleportUnit(tmpOwner as §_-F3o§,shotParams.point.x,shotParams.point.y,function():void
         {
            if(sourceLocationDamage)
            {
               §_-G4§.§_-Ee§(tmpOwner,oldX,oldY,null,paramSet.damage.value,paramSet.damageRadius.value,paramSet.explosionRadius.value,false,false,null,true,false);
               Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_SMALL,shotParams.point.x,shotParams.point.y,30);
            }
            tmpOwner.stable = false;
            detach();
            nextTurnOnStable();
         });
         §_-h2B§.play(§_-d27§.TELEPORT);
      }
      
      private function onStable(param1:Event) : void
      {
         var canUseAntiFreeze:Boolean;
         var gm:§_-eX§ = null;
         var wb:§_-2m§ = null;
         var antiFreeze:§_-Oi§ = null;
         var restoreValue:int = 0;
         var enhValue:int = 0;
         var tmpOwner:§_-01J§ = null;
         var turnNumber:uint = 0;
         var antiFreezeDelay:uint = 0;
         var e:Event = param1;
         var target:§_-01J§ = §_-01J§(e.currentTarget);
         if(this.healBonus)
         {
            restoreValue = this.healBonus;
            enhValue = target.bonuses.getStatValue(§_-x2M§.HEAL_BONUS);
            if(enhValue > 0)
            {
               restoreValue += MathUtil.§_-na§(enhValue,restoreValue);
            }
            target.attributes.hp.restore(restoreValue,§_-b21§.§_-uy§);
         }
         gm = §_-X1j§.§_-12n§.gameMaster;
         wb = target.squad.§_-B2M§().§_-V2J§();
         antiFreeze = §_-Cq§.§_-J2a§(§_-q24§.ANTIFREEZE,wb);
         canUseAntiFreeze = Boolean(antiFreeze) && !antiFreeze.isEmpty() && antiFreeze.§_-d1q§() == -1;
         if(this.allowAntiFreeze && gm.§_-42C§() && canUseAntiFreeze)
         {
            gm.§_-36§(false);
            gm.§_-c2w§(false);
            tmpOwner = target;
            §_-Q2v§.§_-01z§(antiFreeze.getId());
            tmpOwner.§_-P1W§(antiFreeze.getId());
            tmpOwner.attributes.jump.§_-03m§();
            turnNumber = gm.§_-82g§();
            antiFreezeDelay = 60;
            §_-Zd§.§_-33o§(antiFreezeDelay,function():void
            {
               tmpOwner.attributes.jump.§_-ZG§();
               if(turnNumber == gm.§_-82g§() && !gm.§_-v1V§())
               {
                  gm.§_-c2w§();
                  if(!tmpOwner.disposed && Boolean(tmpOwner.weapon))
                  {
                     tmpOwner.weapon.detach();
                  }
                  nextTurnOnStable();
               }
            });
         }
         else
         {
            nextTurnOnStable();
         }
      }
   }
}

