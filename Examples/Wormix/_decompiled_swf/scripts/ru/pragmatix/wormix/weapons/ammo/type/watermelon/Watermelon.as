package ru.pragmatix.wormix.weapons.ammo.type.watermelon
{
   import §_-L1H§.§_-C3e§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.ScreenShake;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.simple.BouncingGrenade;
   
   public class Watermelon extends BouncingGrenade
   {
      
      private const PIECE_COUNT:uint = 6;
      
      protected var pieceClass:Class = WatermelonPiece;
      
      protected var paramSetEx:§_-C3e§;
      
      public function Watermelon(param1:§_-01J§, param2:§_-C3e§, param3:§_-C3e§, param4:uint, param5:Number, param6:uint, param7:Number)
      {
         super(param1,param2,param4,param5,param6,param7,null);
         this.paramSetEx = param3;
         bounceSounds = [§_-d27§.§_-I1v§];
         stopSound = null;
      }
      
      private function makePiece() : void
      {
         new this.pieceClass(owner,this.x,this.y - 30,§_-X1j§.randomSeed.§_-H3r§(-3 * MathUtil.PI_QUARTER,-MathUtil.PI_QUARTER),§_-X1j§.randomSeed.§_-Fh§(3,7),this.paramSetEx);
      }
      
      override public function explode() : void
      {
         super.explode();
         if(this.PIECE_COUNT > 6)
         {
            §_-X1j§.§_-12n§.scene.dispatchEvent(new §_-H1K§(§_-A2V§.§_-A2e§,"watermelon hack"));
         }
         var _loc1_:int = 0;
         while(_loc1_ < this.PIECE_COUNT)
         {
            this.makePiece();
            _loc1_++;
         }
         ScreenShake.go(paramSet.damage.value);
      }
   }
}

