package ru.pragmatix.wormix.messages.client
{
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-4I§;
   import §_-Cl§.*;
   import §_-DJ§.§_-J2l§;
   import §_-U1i§.*;
   import §_-YF§.§_-q2v§;
   import §_-k2s§.§_-b0§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-e1k§;
   import feathers.controls.Button;
   import feathers.core.ITextRenderer;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanInviteTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   
   public class GetFriendsForBoss
   {
      
      public var bossIds:Vector.<int>;
      
      public function GetFriendsForBoss(param1:Vector.<int>)
      {
         super();
         this.bossIds = param1;
      }
   }
}

