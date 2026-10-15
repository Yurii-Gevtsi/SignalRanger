package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import §_-C3f§.*;
   import §_-Y1O§.*;
   import §_-j1w§.§_-B2i§;
   import §_-l1K§.§_-d2j§;
   import §_-l1K§.§_-vO§;
   import §_-r1C§.§_-h2B§;
   import flash.errors.IllegalOperationError;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.ammo.type.missile.homing.HomingMissile;
   import ru.pragmatix.wormix.weapons.ammo.type.missile.homing.StuffedHomingMissile;
   import starling.display.DisplayObject;
   import starling.utils.Pool;
   
   public class HomingBazooka extends Weapon
   {
      
      protected var mark:DisplayObject;
      
      public function HomingBazooka()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-z1W§(§_-vO§.§_-Bu§());
         §_-z1W§(new §_-d2j§());
      }
      
      override protected function shoot() : void
      {
         var _loc1_:Class = getProperty("ammoClass") as Class;
         var _loc2_:HomingMissile = new _loc1_(owner,Pool.getPoint(owner.x,owner.y),paramSet,shotParams.angle,§_-Yz§(shotParams.power),shotParams.point);
         if(_loc2_ is StuffedHomingMissile)
         {
            StuffedHomingMissile(_loc2_).setDamagePerSecond(§_-TA§(getProperty("damagePerFrame")));
            StuffedHomingMissile(_loc2_).setDamageBonusLimit(§_-TA§(getProperty("damageBonusLimit")));
         }
         this.detach();
         nextTurnOnStable();
      }
      
      override protected function §_-q1b§(param1:int) : void
      {
         if(param1 == 0)
         {
            this.mark = Effects.putSimpleDisappearingCrossMark(shotParams.point.x,shotParams.point.y).display;
            §_-h2B§.play(§_-d27§.§_-p1v§);
         }
      }
      
      override public function attach(param1:§_-01J§) : void
      {
         super.attach(param1);
         this.mark = null;
      }
      
      override public function detach() : void
      {
         super.detach();
         if(this.mark != null)
         {
            §_-X1j§.§_-12n§.scene.removeElement(this.mark);
         }
      }
   }
}

