package ru.pragmatix.wormix.serialization.client
{
   import §_-82l§.§_-A2Y§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-Cl§.§_-L2z§;
   import §_-DJ§.§_-J2l§;
   import §_-OJ§.§_-L3B§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-y1o§;
   import §_-e2r§.§_-636§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u1P§.ShopResultEvent;
   import §_-z1g§.*;
   import feathers.controls.Button;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.client.GetFriendsForBoss;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.SystemUtil;
   
   public class GetFriendsForBossBinarySerializer implements §_-D2L§
   {
      
      public function GetFriendsForBossBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetFriendsForBoss = GetFriendsForBoss(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         SerializationUtils.writeShortVector(_loc5_,_loc3_.bossIds);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
      
      public function getCommandId() : uint
      {
         return 93;
      }
   }
}

