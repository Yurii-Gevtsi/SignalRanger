package ru.pragmatix.wormix.model.items.craft
{
   import §_-62§.§_-O2H§;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.MortarMissile;
   import starling.events.Event;
   
   public class §_-6S§
   {
      
      public var §_-HS§:§_-IE§;
      
      public var weapon:§_-O2H§;
      
      public var next:Vector.<§_-6S§> = new Vector.<§_-6S§>();
      
      public var prev:§_-6S§;
      
      public var level:int;
      
      public var §_-l1u§:§_-13q§;
      
      public function §_-6S§()
      {
         super();
      }
      
      public function §_-327§() : Boolean
      {
         return this.prev == null;
      }
   }
}

