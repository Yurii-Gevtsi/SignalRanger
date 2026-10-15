package ru.pragmatix.wormix.messages.clans.request
{
   import §_-Cl§.§_-L2z§;
   import §_-OJ§.§_-L3B§;
   import §_-j22§.§_-5g§;
   import §_-p14§.§_-A3Y§;
   import §_-xu§.§_-a1B§;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.messages.client.GetFriendsForBoss;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.textures.Texture;
   import starlingbuilder.engine.UIBuilder;
   
   public class LowerRankInClanRequest
   {
      
      public var profileId:int;
      
      public var targetRank:int;
      
      public function LowerRankInClanRequest()
      {
         super();
      }
   }
}

