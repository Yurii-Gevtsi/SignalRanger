package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-a2s§;
   import §_-A1K§.§_-TA§;
   import §_-H2g§.§_-di§;
   import §_-l1K§.§_-vO§;
   import §_-r1C§.§_-h2B§;
   import §_-u1m§.§_-61A§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.weapons.ammo.BunkerBusterMissile;
   
   public class BunkerBuster extends §_-61A§
   {
      
      public static const §_-T2g§:uint = 20;
      
      protected var §_-N1F§:Number = -50;
      
      protected var g:Number = Force.GRAVITY.ay;
      
      protected var counter:uint = 20;
      
      protected var armorReduction:uint = 0;
      
      public function BunkerBuster()
      {
         super(true,false);
         §_-Y2U§ = new §_-a2s§();
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         if(param1["counter"])
         {
            this.counter = §_-TA§(param1["counter"]).value;
         }
         if(param1["armorReduction"])
         {
            this.armorReduction = §_-TA§(param1["armorReduction"]).value;
         }
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
         var _loc1_:Number = shotParams.point.x;
         this.§_-RT§(_loc1_,this.§_-N1F§);
         Effects.putSimpleUncontrolledDisappearingCrossMark(shotParams.point.x,shotParams.point.y,20);
         §_-h2B§.play(§_-d27§.§_-D24§);
         detach();
         nextTurnOnStable();
      }
      
      protected function §_-RT§(param1:int, param2:int) : void
      {
         var _loc3_:BunkerBusterMissile = new BunkerBusterMissile(owner,paramSet,paramSetEx,param1,param2,this.counter,this.armorReduction);
         if(getProperty("cripple"))
         {
            _loc3_.setCripple();
         }
      }
   }
}

