package ru.pragmatix.wormix.weapons.ammo.boss.emperor
{
   import §_-D3A§.§_-TZ§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.type.shuriken.Shuriken;
   import starling.events.Event;
   
   public class EmperorSpearman extends EmperorTerracottaWarrior
   {
      
      private static const DEFAULT_DAMAGE:uint = 50;
      
      private static const SPEED_X:int = 20;
      
      private static const SPEED_Y:int = -15;
      
      public function EmperorSpearman(param1:§_-01J§, param2:Number, param3:Number)
      {
         armatureName = "TerracottaSpearman";
         name = §_-s2a§.§_-K3t§(§_-s2a§.BOSS_EMPEROR_SPEARMAN);
         super(param1,param2,param3);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.onEndTurn);
      }
      
      override public function attackEnemy() : void
      {
         var _loc1_:Vector.<§_-01J§> = null;
         var _loc3_:§_-01J§ = null;
         _loc1_ = §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner);
         var _loc2_:uint = _loc1_.length;
         if(_loc2_ > 0)
         {
            _loc3_ = _loc1_[§_-X1j§.randomSeed.§_-Fh§(0,_loc2_ - 1)];
            this.x = _loc3_.x + width * clip.scaleX;
            this.y = _loc3_.y;
            turn(_loc3_.x);
            §_-h2B§.play(§_-d27§.§_-k4§(Shuriken.attackSounds));
            §_-G4§.§_-Ee§(owner,x,y,null,0,20,20,false);
            §_-G4§.§_-Ee§(owner,x,y,_loc3_ as §_-F3o§,DEFAULT_DAMAGE,0,0,false);
            _loc3_.setSpeedXY(SPEED_X * direction,SPEED_Y);
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.EOT,this.onEndTurn);
         super.defaultOnDisposed(param1);
      }
      
      public function onEndTurn(param1:Event) : void
      {
         if(!§_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-C6§())
         {
            this.attackEnemy();
         }
      }
   }
}

