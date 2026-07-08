package ru.pragmatix.wormix.weapons.ammo
{
   import §_-31Y§.§_-D3B§;
   import §_-L1H§.§_-C3e§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.model.AnimalPhysicModel;
   import ru.pragmatix.wormix.weapons.ammo.type.animal.RunningObject;
   import starling.events.Event;
   
   public class RunningSwine extends RunningObject
   {
      
      protected var jumping:Boolean = false;
      
      public function RunningSwine(param1:§_-01J§, param2:§_-C3e§, param3:§_-C3e§)
      {
         super(param1,param2,param3);
         tags.push(§_-D3B§.§_-f1U§);
         _width = 30;
         _height = 30;
      }
      
      override protected function update() : void
      {
         if(frames % 23 == 0)
         {
            this.jump();
         }
         if((frames - 1) % 60 == 0)
         {
            §_-h2B§.play(§_-d27§.SWINE);
         }
      }
      
      public function jump() : void
      {
         if(!this.jumping)
         {
            this.jumping = true;
            AnimalPhysicModel(§_-Y2I§).crawling = 0;
            addSpeedXY(direction * SPEED,-12);
         }
      }
      
      override protected function onFalling(param1:Event) : void
      {
         this.jumping = true;
         super.onFalling(param1);
      }
      
      override protected function onStopMoving(param1:Event) : void
      {
         this.jumping = false;
         super.onStopMoving(param1);
      }
   }
}

