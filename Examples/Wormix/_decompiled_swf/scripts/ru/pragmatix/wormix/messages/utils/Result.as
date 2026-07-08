package ru.pragmatix.wormix.messages.utils
{
   import §_-12a§.KamikazeBuff;
   import §_-5Y§.*;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-F1P§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.§_-a2W§;
   import §_-H2g§.*;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-r1C§.§_-h2B§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-X1q§;
   import dragonBones.Armature;
   import dragonBones.Slot;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.model.GenericPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import starling.utils.ScaleMode;
   
   public class Result
   {
      
      public static const SUCCESS:int = 0;
      
      public static const ERROR:int = 1;
      
      public function Result()
      {
         super();
      }
      
      public static function toString(param1:int) : String
      {
         if(param1 == SUCCESS)
         {
            return "SUCCESS";
         }
         return "ERROR";
      }
   }
}

