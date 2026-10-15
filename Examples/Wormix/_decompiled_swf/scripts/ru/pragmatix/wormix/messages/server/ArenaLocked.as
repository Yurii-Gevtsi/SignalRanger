package ru.pragmatix.wormix.messages.server
{
   import §_-22z§.*;
   import §_-416§.*;
   import §_-82a§.*;
   import §_-A1K§.§_-TA§;
   import §_-P1B§.§_-b21§;
   import §_-SP§.§_-M4§;
   import §_-k1u§.§_-Oi§;
   import §_-w2i§.§_-Zd§;
   import com.hurlant.math.*;
   import feathers.layout.HorizontalLayout;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class ArenaLocked
   {
      
      public static var CAUSE_NO_BATTLES:int = 1;
      
      public static var CAUSE_MISSION_LOCKED:int = 2;
      
      public static var CAUSE_WRONG_MISSION:int = 3;
      
      public var delay:uint;
      
      public var currentMission:int;
      
      public var errorCode:int = 0;
      
      public function ArenaLocked()
      {
         super();
      }
      
      public function getErrorMessageStringId() : String
      {
         switch(this.errorCode)
         {
            case CAUSE_NO_BATTLES:
               return §_-s2a§.BATTLE_REQUEST_PLAYER_EXCEED_BATTLES;
            case CAUSE_MISSION_LOCKED:
            case CAUSE_WRONG_MISSION:
               return §_-s2a§.PVP_SERVER_INCORRECT_BATTLE_ID;
            default:
               return §_-s2a§.PVP_SERVER_BATTLE_CREATION_ERROR;
         }
      }
      
      public function toString() : String
      {
         return "ArenaLocked{delay=" + String(this.delay) + ",currentMission=" + String(this.currentMission) + ",errorCode=" + String(this.errorCode) + "}";
      }
   }
}

