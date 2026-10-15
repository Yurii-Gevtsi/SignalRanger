package ru.pragmatix.wormix.messages.clans
{
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-TF§.§_-q1j§;
   import §_-r1C§.§_-h2B§;
   import §_-zI§.§_-X1q§;
   import com.greensock.*;
   import com.greensock.core.*;
   import flash.text.TextField;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class ClanMessages
   {
      
      private static const §_-b2h§:uint = 7000;
      
      private static const §_-j2v§:uint = 17000;
      
      private static const §_-DK§:uint = 7100;
      
      public static const LOGIN_REQUEST:uint = §_-b2h§ + 1;
      
      public static const LOGIN_CREATE_REQUEST:uint = §_-b2h§ + 2;
      
      public static const LOGIN_JOIN_REQUEST:uint = §_-b2h§ + 3;
      
      public static const LOGIN_RESPONSE:uint = §_-j2v§ + 1;
      
      public static const LOGIN_ERROR_RESPONSE:uint = §_-j2v§ + 3;
      
      public static const LOGOUT_REQUEST:uint = §_-b2h§ + 7;
      
      public static const DELETE_CLAN_REQUEST:uint = §_-b2h§ + 11;
      
      public static const DELETE_CLAN_RESPONSE:uint = §_-j2v§ + 11;
      
      public static const INVITE_TO_CLAN_REQUEST:uint = §_-b2h§ + 102;
      
      public static const INVITE_TO_CLAN_RESPONSE:uint = §_-j2v§ + 102;
      
      public static const EXPEL_FROM_CLAN_REQUEST:uint = §_-b2h§ + 104;
      
      public static const EXPEL_FROM_CLAN_RESPONSE:uint = §_-j2v§ + 104;
      
      public static const QUIT_CLAN_REQUEST:uint = §_-b2h§ + 105;
      
      public static const QUIT_FROM_CLAN_RESPONSE:uint = §_-j2v§ + 105;
      
      public static const PROMOTE_IN_RANK_REQUEST:uint = §_-b2h§ + 106;
      
      public static const PROMOTE_IN_RANK_RESPONSE:uint = §_-j2v§ + 106;
      
      public static const LOWER_IN_RANK_REQUEST:uint = §_-b2h§ + 107;
      
      public static const LOWER_IN_RANK_RESPONSE:uint = §_-j2v§ + 107;
      
      public static const INCREASE_LEVEL_CLAN_REQUEST:uint = §_-b2h§ + 108;
      
      public static const INCREASE_LEVEL_CLAN_RESPONSE:uint = §_-j2v§ + 108;
      
      public static const POST_TO_CHAT_REQUEST:uint = §_-b2h§ + 109;
      
      public static const POST_TO_CHAT_RESPONSE:uint = §_-j2v§ + 109;
      
      public static const CHAT_MESSAGE_EVENT:uint = §_-DK§ + 109;
      
      public static const POST_NEWS_REQUEST:uint = §_-b2h§ + 110;
      
      public static const POST_NEWS_RESPONSE:uint = §_-j2v§ + 110;
      
      public static const CLAN_SUMMARY_REQUEST:uint = §_-b2h§ + 112;
      
      public static const CLAN_SUMMARY_RESPONSE:uint = §_-j2v§ + 112;
      
      public static const LIST_CLANS_REQUEST:uint = §_-b2h§ + 113;
      
      public static const LIST_CLANS_RESPONSE:uint = §_-j2v§ + 113;
      
      public static const RENAME_CLAN_REQUEST:uint = §_-b2h§ + 114;
      
      public static const RENAME_CLAN_RESPONSE:uint = §_-j2v§ + 114;
      
      public static const CHANGE_CLAN_EMBLEM_REQUEST:uint = §_-b2h§ + 115;
      
      public static const CHANGE_CLAN_EMBLEM_RESPONSE:uint = §_-j2v§ + 115;
      
      public static const CHANGE_CLAN_DESCRIPTION_REQUEST:uint = §_-b2h§ + 116;
      
      public static const CHANGE_CLAN_DESCRIPTION_RESPONSE:uint = §_-j2v§ + 116;
      
      public static const TOP_CLANS_REQUEST:uint = §_-b2h§ + 117;
      
      public static const TOP_CLANS_RESPONSE:uint = §_-j2v§ + 117;
      
      public static const CHANGE_CLAN_REVIEW_STATE_REQUEST:uint = §_-b2h§ + 118;
      
      public static const UPDATE_CLAN_REQUEST:uint = §_-b2h§ + 119;
      
      public static const CHANGE_CLAN_JOIN_RATING_REQUEST:uint = §_-b2h§ + 120;
      
      public static const CHANGE_CLAN_JOIN_RATING_RESPONSE:uint = §_-j2v§ + 120;
      
      public static const DONATE_REQUEST:uint = §_-b2h§ + 121;
      
      public static const DONATE_RESPONSE:uint = §_-j2v§ + 121;
      
      public static const CHANGE_CLAN_CLOSE_EXIT_REQUEST:uint = §_-b2h§ + 122;
      
      public static const CHANGE_CLAN_CLOSE_EXIT_RESPONSE:uint = §_-j2v§ + 122;
      
      public static const CHANGE_CLAN_MEDAL_PRICE_REQUEST:uint = §_-b2h§ + 123;
      
      public static const CHANGE_CLAN_MEDAL_PRICE_RESPONSE:uint = §_-j2v§ + 123;
      
      public static const CASH_MEDALS_REQUEST:uint = §_-b2h§ + 124;
      
      public static const CASH_MEDALS_RESPONSE:uint = §_-j2v§ + 124;
      
      public static const AUDIT_ACTIONS_REQUEST:uint = §_-b2h§ + 125;
      
      public static const AUDIT_ACTIONS_RESPONSE:uint = §_-j2v§ + 125;
      
      public static const GLADIATOR_FINISH_EVENT:uint = §_-DK§ + 26;
      
      public static const SET_EXPEL_PERMIT_REQUEST:uint = §_-b2h§ + 127;
      
      public static const SET_EXPEL_PERMIT_RESPONSE:uint = §_-j2v§ + 127;
      
      public static const SET_MUTE_MODE_REQUEST:uint = §_-b2h§ + 128;
      
      public static const SET_MUTE_MODE_RESPONSE:uint = §_-j2v§ + 128;
      
      public static const CRAFT_LEGENDARY_EVENT:uint = §_-DK§ + 29;
      
      public function ClanMessages()
      {
         super();
      }
   }
}

