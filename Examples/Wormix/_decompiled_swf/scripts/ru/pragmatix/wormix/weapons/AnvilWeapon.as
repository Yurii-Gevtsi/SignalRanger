package ru.pragmatix.wormix.weapons
{
   import §_-F39§.§_-a2W§;
   import §_-G2H§.*;
   import §_-H2g§.§_-di§;
   import §_-L1H§.§_-C3e§;
   import §_-l1K§.§_-vO§;
   import §_-m0§.§_-X1F§;
   import §_-r1C§.§_-h2B§;
   import §_-u1m§.§_-61A§;
   import flash.geom.Point;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.ammo.type.air.Anvil;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class AnvilWeapon extends §_-61A§
   {
      
      private static const §_-C3k§:uint = Physics.UNIT_HALF_HEIGHT;
      
      public function AnvilWeapon()
      {
         super(true,false);
      }
      
      override protected function §_-V1B§(param1:Number, param2:Number) : Boolean
      {
         var _loc3_:Point = null;
         var _loc4_:int = 0;
         _loc3_ = HitTest.castRay(param1,-20,0,§_-X1j§.§_-12n§.scene.sceneH);
         _loc4_ = §_-X1j§.§_-12n§.scene.sceneH - §_-X1j§.§_-12n§.scene.getWaterHeight();
         var _loc5_:Boolean = Boolean(_loc3_) && _loc3_.y > §_-C3k§ && _loc3_.y < _loc4_;
         Pool.putPoint(_loc3_);
         if(_loc5_)
         {
            return true;
         }
         §_-h2B§.play(§_-d27§.§_-b2H§);
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.CALL_REINFORCEMENTS_USELESS),true);
         }
         return false;
      }
      
      override protected function §_-D2a§(param1:Function) : void
      {
         §_-z1W§(§_-vO§.§_-Bu§(param1));
      }
      
      override protected function §_-h2d§(param1:Number, param2:Number, param3:Boolean) : §_-di§
      {
         return Effects.putSimpleCrossMark(param1,param2);
      }
      
      override protected function shoot() : void
      {
         this.§_-B1T§(owner,paramSet,shotParams.point.x,-20,shotParams.§_-B3f§ * 15,0);
         §_-h2B§.play(§_-d27§.§_-D24§);
         detach();
         nextTurnOnStable();
      }
      
      override protected function §_-22V§(param1:int, param2:int, param3:int) : Number
      {
         return param2;
      }
      
      override protected function §_-B1T§(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:Number, param5:Number, param6:Number) : void
      {
         new Anvil(param1,param2,param3,param4);
      }
   }
}

