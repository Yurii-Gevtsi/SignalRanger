package ru.pragmatix.wormix.model.user
{
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-Ly§.§_-81L§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   
   public class §_-WU§
   {
      
      private var level:uint;
      
      private var §_-z1G§:uint;
      
      private var §_-h1R§:uint;
      
      private var §_-71l§:uint;
      
      public function §_-WU§(param1:uint, param2:uint, param3:uint, param4:uint)
      {
         super();
         this.level = param1;
         this.§_-z1G§ = param2;
         this.§_-h1R§ = param3;
         this.§_-71l§ = param4;
      }
      
      public function §_-736§() : uint
      {
         return this.§_-71l§;
      }
      
      public function getLevel() : uint
      {
         return this.level;
      }
      
      public function §_-L1M§() : uint
      {
         return this.§_-z1G§;
      }
      
      public function §_-L2a§() : uint
      {
         return this.§_-h1R§;
      }
   }
}

