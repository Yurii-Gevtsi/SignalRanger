package ru.pragmatix.wormix.pvp.messages.client
{
   import §_-31W§.§_-r26§;
   import §_-z1g§.§_-F4§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   
   public class PvpDropPlayer
   {
      
      public static const SURRENDER:uint = 1;
      
      public static const I_AM_CHEATER:uint = 4;
      
      public static const DESYNC:uint = 5;
      
      public var playerNum:uint;
      
      public var battleId:uint;
      
      public var type:uint;
      
      public function PvpDropPlayer()
      {
         super();
      }
   }
}

