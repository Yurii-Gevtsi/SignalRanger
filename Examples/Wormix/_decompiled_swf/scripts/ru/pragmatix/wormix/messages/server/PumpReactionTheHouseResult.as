package ru.pragmatix.wormix.messages.server
{
   import §_-fo§.*;
   import §_-q2b§.§_-S2L§;
   import §_-sQ§.§_-H3D§;
   import §_-zI§.§_-X1q§;
   import feathers.data.ArrayCollection;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.flox.social.utils.storage.StorageType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.§_-D2t§;
   import ru.pragmatix.wormix.model.§_-kQ§;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.utils.MathUtil;
   
   public class PumpReactionTheHouseResult
   {
      
      public static const OK:int = 0;
      
      public static const ERROR:int = 2;
      
      public static const TODAY_ALREADY_PUMPED:int = 1;
      
      public static const DAY_LIMIT_PUMP:int = 3;
      
      public var result:int;
      
      public function PumpReactionTheHouseResult()
      {
         super();
      }
   }
}

