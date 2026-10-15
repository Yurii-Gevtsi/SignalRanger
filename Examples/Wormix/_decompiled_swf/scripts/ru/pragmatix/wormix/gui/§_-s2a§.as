package ru.pragmatix.wormix.gui
{
   import §_-5Y§.§_-91F§;
   import §_-9§.§_-52A§;
   import §_-B1y§.Cookie;
   import §_-CF§.§_-p2U§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-J31§.§_-e2J§;
   import §_-l1g§.§_-L3Q§;
   import §_-u1F§.§_-SE§;
   import flash.events.Event;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.Parasite;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class §_-s2a§
   {
      
      public static const ERROR_SOCIAL_OP:String = "ERROR_SOCIAL_OP";
      
      public static const ADS_UNAVAILABLE:String = "ADS_UNAVAILABLE";
      
      public static const ADS_UNAVAILABLE_TEMPORAL:String = "ADS_UNAVAILABLE_TEMPORAL";
      
      public static const FAST_BATTLE:String = "FAST_BATTLE";
      
      public static const FAST_BATTLE_REWARD:String = "FAST_BATTLE_REWARD";
      
      public static const FAST_BATTLE_TEXT:String = "FAST_BATTLE_TEXT";
      
      public static const PHOTO_LOAD_COMPLETE:String = "PHOTO_LOAD_COMPLETE";
      
      public static const AND:String = "AND";
      
      public static const LOADING:String = "LOADING";
      
      public static const UPLOADING:String = "UPLOADING";
      
      public static const SAVING:String = "SAVING";
      
      public static const WAITING_DATA:String = "WAITING_DATA";
      
      public static const SWITCHING_TURN:String = "SWITCHING_TURN";
      
      public static const CONNECTING:String = "CONNECTING";
      
      public static const TITLE_EXIT:String = "TITLE_EXIT";
      
      public static const DESCRIPTION_EXIT:String = "DESCRIPTION_EXIT";
      
      public static const TRY_AGAIN_BUTTON:String = "TRY_AGAIN_BUTTON";
      
      public static const LOGIN_ERROR_TITLE:String = "LOGIN_ERROR_TITLE";
      
      public static const LOGIN_ERROR_MESSAGE:String = "LOGIN_ERROR_MESSAGE";
      
      public static const ALREADY_IN_GAME_LOGIN_TRYING_LIMIT_OUT:String = "ALREADY_IN_GAME_LOGIN_TRYING_LIMIT_OUT";
      
      public static const TITLE_SERVER_MSG:String = "TITLE_SERVER_MSG";
      
      public static const SERVER_UPDATE:String = "SERVER_UPDATE";
      
      public static const TITLE_ATTENTION:String = "TITLE_ATTENTION";
      
      public static const TITLE_ERROR:String = "TITLE_ERROR";
      
      public static const TITLE_BAN:String = "TITLE_BAN";
      
      public static const GAME_NAME:String = "GAME_NAME";
      
      public static const TITLE_UPDATE_IS_AVAILABLE:String = "TITLE_UPDATE_IS_AVAILABLE";
      
      public static const TEXT_UPDATE_IS_AVAILABLE:String = "TEXT_UPDATE_IS_AVAILABLE";
      
      public static const TEXT_NOT_IN_WHITE_LIST:String = "TEXT_NOT_IN_WHITE_LIST";
      
      public static const UPDATE:String = "UPDATE";
      
      public static const PERFORMANCE_WARNING:String = "PERFORMANCE_WARNING";
      
      public static const CHANGE_NAME_ERROR:String = "CHANGE_NAME_ERROR";
      
      public static const LEVEL_UP_DIALOG_TITLE:String = "LEVEL_UP_DIALOG_TITLE";
      
      public static const LEVEL_UP_DIALOG_DESCRIPTION:String = "LEVEL_UP_DIALOG_DESCRIPTION";
      
      public static const LEVEL_UP_DIALOG_BONUS_TEXT:String = "LEVEL_UP_DIALOG_BONUS_TEXT";
      
      public static const LEVEL_UP_DIALOG_THINGS_TEXT:String = "LEVEL_UP_DIALOG_THINGS_TEXT";
      
      public static const LEVEL_UP_DIALOG_BUTTON_SHOP:String = "LEVEL_UP_DIALOG_BUTTON_SHOP";
      
      public static const LEVEL_UP_DIALOG_LIFE_TIPS:String = "LEVEL_UP_DIALOG_LIFE_TIPS";
      
      public static const LEVEL_UP_DIALOG_PARAMS_TIPS:String = "LEVEL_UP_DIALOG_PARAMS_TIPS";
      
      public static const LEVEL_UP_DIALOG_COMMAND_TIPS:String = "LEVEL_UP_DIALOG_COMMAND_TIPS";
      
      public static const ERROR_SERVER:String = "ERROR_SERVER";
      
      public static const ERROR_SAVING:String = "ERROR_SAVING";
      
      public static const ERROR_SOCIAL:String = "ERROR_SOCIAL";
      
      public static const ERROR_SCENE_LOAD:String = "ERROR_SCENE_LOAD";
      
      public static const ERROR_HACKING_ATTEMPT:String = "ERROR_HACKING_ATTEMPT";
      
      public static const ERROR_REQUEST_FRIEND_TO_BATTLE:String = "ERROR_REQUEST_FRIEND_TO_BATTLE";
      
      public static const ERROR_BANK_FAIL:String = "ERROR_BANK_FAIL";
      
      public static const ERROR_WRONG_PRICE:String = "ERROR_WRONG_PRICE";
      
      public static const ERROR_NOT_ENOUGH_MONEY:String = "ERROR_NOT_ENOUGH_MONEY";
      
      public static const ERROR_ADVERTISING_LIMIT_REACHED:String = "ERROR_ADVERTISING_LIMIT_REACHED";
      
      public static const ERROR_AUTH_KEY:String = "ERROR_AUTH_KEY";
      
      public static const UNSYNCH_ERROR:String = "UNSYNCH_ERROR";
      
      public static const INCORRECT_PROTOCOL_VERSION:String = "INCORRECT_PROTOCOL_VERSION";
      
      public static const RECEIPT_VERIFICATION_ERROR_APPLE:String = "RECEIPT_VERIFICATION_ERROR_APPLE";
      
      public static const RECEIPT_VERIFICATION_ERROR_GOOGLE:String = "RECEIPT_VERIFICATION_ERROR_GOOGLE";
      
      public static const RECEIPT_VERIFICATION_ERROR_STEAM:String = "RECEIPT_VERIFICATION_ERROR_STEAM";
      
      public static const TITLE_ERROR_LOAD_RES:String = "TITLE_ERROR_LOAD_RES";
      
      public static const MSG_ERROR_LOAD_ROOM_RES:String = "MSG_ERROR_LOAD_ROOM_RES";
      
      public static const BUY_RUBIES_SUCCESSFULL:String = "BUY_RUBIES_SUCCESSFULL";
      
      public static const OLD_ADS_TEXT:String = "OLD_ADS_TEXT";
      
      public static const ADS_TEXT:String = "ADS_TEXT";
      
      public static const ADS_BUTTON_TEXT:String = "ADS_BUTTON_TEXT";
      
      public static const SHOP:String = "SHOP";
      
      public static const WEAPON_TAB:String = "WEAPON_TAB";
      
      public static const EQUIP_TAB:String = "EQUIP_TAB";
      
      public static const HAT_TAB:String = "HAT_TAB";
      
      public static const LONG_RANGE_WEAPONS:String = "LONG_RANGE_WEAPONS";
      
      public static const CLOSE_RANGE_WEAPONS:String = "CLOSE_RANGE_WEAPONS";
      
      public static const SPECIAL_WEAPONS:String = "SPECIAL_WEAPONS";
      
      public static const GADGETS:String = "GADGETS";
      
      public static const HATS:String = "HATS";
      
      public static const ARTIFACTS:String = "ARTIFACTS";
      
      public static const CATEGORY_PURCHASED:String = "CATEGORY_PURCHASED";
      
      public static const SALE_TEXT:String = "SALE_TEXT";
      
      public static const REMAINING_TEXT:String = "REMAINING_TEXT";
      
      public static const TITLE_BUY:String = "TITLE_BUY";
      
      public static const SHOP_REQUIRED_LEVEL:String = "SHOP_REQUIRED_LEVEL";
      
      public static const SHOP_REQUIRED_FRIENDS:String = "SHOP_REQUIRED_FRIENDS";
      
      public static const SHOP_BUY_FOR_REAL_MONEY:String = "SHOP_BUY_FOR_REAL_MONEY";
      
      public static const SHOP_REQUIRED_FIELD:String = "SHOP_REQUIRED_FIELD";
      
      public static const SHOP_REQUIRED_IMPOSSIBLE:String = "SHOP_REQUIRED_IMPOSSIBLE";
      
      public static const FRIENDS_REQUIREMENT:String = "FRIENDS_REQUIREMENT";
      
      public static const LEVEL_REQUIREMENT:String = "LEVEL_REQUIREMENT";
      
      public static const CRAFT_PRICE_LABEL:String = "CRAFT_PRICE_LABEL";
      
      public static const PRICE_LABEL:String = "PRICE_LABEL";
      
      public static const RATING_REQUIREMENT:String = "RATING_REQUIREMENT";
      
      public static const PRICE_FREE:String = "PRICE_FREE";
      
      public static const PRICE_OR_LABEL:String = "PRICE_OR_LABEL";
      
      public static const ALREADY_CRAFTED_LABEL:String = "ALREADY_CRAFTED_LABEL";
      
      public static const SHOP_REQUIRED_RATING:String = "SHOP_REQUIRED_RATING";
      
      public static const SCENARIO_REQUIREMENT:String = "SCENARIO_REQUIREMENT";
      
      public static const SHOP_SCENARIO_REQUIREMENT:String = "SHOP_SCENARIO_REQUIREMENT";
      
      public static const DAMAGE:String = "DAMAGE";
      
      public static const LEVEL_BRIEF:String = "LEVEL_BRIEF";
      
      public static const COUNT_LABEL:String = "COUNT_LABEL";
      
      public static const QUANTITY_INFINITY:String = "QUANTITY_INFINITY";
      
      public static const QUANTITY_PER_BATTLE:String = "QUANTITY_PER_BATTLE";
      
      public static const BUY_REACTION_RATES:String = "BUY_REACTION_RATES";
      
      public static const FIRST_SHOT_DELAY:String = "FIRST_SHOT_DELAY";
      
      public static const BUY_EXISTING_WEAPON:String = "BUY_EXISTING_WEAPON";
      
      public static const BUY_EXISTING_EQUIP:String = "BUY_EXISTING_EQUIP";
      
      public static const BUY_2_HAT:String = "BUY_2_HAT";
      
      public static const BUY_SUCCESSFUL:String = "BUY_SUCCESSFUL";
      
      public static const COMING_SOON:String = "COMING_SOON";
      
      public static const ARENA:String = "ARENA";
      
      public static const BATTLES_AVAILABLE:String = "BATTLES_AVAILABLE";
      
      public static const MISSION_DESCR:String = "MISSION_DESCR";
      
      public static const BOSSES_DESCR:String = "BOSSES_DESCR";
      
      public static const TEAM_HP_LABEL:String = "TEAM_HP_LABEL";
      
      public static const TEAM_LABEL:String = "TEAM_LABEL";
      
      public static const AWARD_LABEL:String = "AWARD_LABEL";
      
      public static const MISSION_TAB_NAME:String = "MISSION_TAB_NAME";
      
      public static const MISSION_FREE_GAME:String = "MISSION_FREE_GAME";
      
      public static const PVP_TAB_NAME:String = "PVP_TAB_NAME";
      
      public static const BOSS_TAB_NAME:String = "BOSS_TAB_NAME";
      
      public static const BOSS_COOP_MODE_BTN:String = "BOSS_COOP_MODE_BTN";
      
      public static const SUPER_BOSS_TAB_NAME:String = "SUPER_BOSS_TAB_NAME";
      
      public static const BOSS_COOP_TITLE:String = "BOSS_COOP_TITLE";
      
      public static const PVP_MESSAGE:String = "PVP_MESSAGE";
      
      public static const PVP_WAGER_LABEL:String = "PVP_WAGER_LABEL";
      
      public static const PVP_PRIZE_LABEL:String = "PVP_PRIZE_LABEL";
      
      public static const PVP_BONUS_LABEL:String = "PVP_BONUS_LABEL";
      
      public static const PVP_WAGER_BTN:String = "PVP_WAGER_BTN";
      
      public static const BONUS_EXPERIENCE:String = "BONUS_EXPERIENCE";
      
      public static const DUEL_WITH_FRIEND:String = "DUEL_WITH_FRIEND";
      
      public static const MISSION_BTN:String = "MISSION_BTN";
      
      public static const BOSS_BTN:String = "BOSS_BTN";
      
      public static const SEARCH_BTN:String = "SEARCH_BTN";
      
      public static const FRIENDS_TITLE:String = "FRIENDS_TITLE";
      
      public static const RANDOM_PARTNER:String = "RANDOM_PARTNER";
      
      public static const ARENA_BUY_BATTLE:String = "ARENA_BUY_BATTLE";
      
      public static const ARENA_DELAY_MESSAGE:String = "ARENA_DELAY_MESSAGE";
      
      public static const BOSS_RESOURCES_LOAD:String = "BOSS_RESOURCES_LOAD";
      
      public static const ARENA_DELAY_DESC:String = "ARENA_DELAY_DESC";
      
      public static const ARENA_DELAY_DONT_WAIT:String = "ARENA_DELAY_DONT_WAIT";
      
      public static const ARENA_BUY_FIVE_BATTLES:String = "ARENA_BUY_FIVE_BATTLES";
      
      public static const ARENA_BUY_BTN_NAME:String = "ARENA_BUY_BTN_NAME";
      
      public static const ARENA_EXISTS_MONEY:String = "ARENA_EXISTS_MONEY";
      
      public static const ARENA_OR_LABEL:String = "ARENA_OR_LABEL";
      
      public static const CALL_FRIEND_ON_PVP_MENU_TITLE:String = "CALL_FRIEND_ON_PVP_MENU_TITLE";
      
      public static const CHOOSE_FRIEND_FOR_BOSS_TITLE:String = "CHOOSE_FRIEND_FOR_BOSS_TITLE";
      
      public static const FRIEND_EXCEED_BATTLES:String = "FRIEND_EXCEED_BATTLES";
      
      public static const FRIEND_MISSION_LOCKED:String = "FRIEND_MISSION_LOCKED";
      
      public static const NO_FRIENDS_FOR_COOP_MISSION:String = "NO_FRIENDS_FOR_COOP_MISSION";
      
      public static const NO_FRIENDS_FOR_COOP_FIGHT:String = "NO_FRIENDS_FOR_COOP_FIGHT";
      
      public static const NO_FRIENDS_FOR_INVITE:String = "NO_FRIENDS_FOR_INVITE";
      
      public static const BATTLE_REQUEST_PLAYER_EXCEED_BATTLES:String = "BATTLE_REQUEST_PLAYER_EXCEED_BATTLES";
      
      public static const REMOVE_FROM_GROUP:String = "REMOVE_FROM_GROUP";
      
      public static const ADD_IN_GROUP:String = "ADD_IN_GROUP";
      
      public static const USER_PAGE:String = "USER_PAGE";
      
      public static const CALL_TO_BATTLE:String = "CALL_TO_BATTLE";
      
      public static const CALL_TO_LOCAL_BATTLE:String = "CALL_TO_LOCAL_BATTLE";
      
      public static const GROUP_ICON_QTIP:String = "GROUP_ICON_QTIP";
      
      public static const CAN_NOT_ADD_IN_GROUP:String = "CAN_NOT_ADD_IN_GROUP";
      
      public static const REMOVE_FROM_GROUP_CONFIRMATION:String = "REMOVE_FROM_GROUP_CONFIRMATION";
      
      public static const ALREADY_IN_TEAM:String = "ALREADY_IN_TEAM";
      
      public static const REPLACE_IN_GROUP_CONFIRMATION:String = "REPLACE_IN_GROUP_CONFIRMATION";
      
      public static const INACTIVE_IN_GROUP:String = "INACTIVE_IN_GROUP";
      
      public static const ACTIVATE_IN_GROUP:String = "ACTIVATE_IN_GROUP";
      
      public static const DEACTIVATE_IN_GROUP:String = "DEACTIVATE_IN_GROUP";
      
      public static const TITLE_HEROIC_BOSSES_PANEL:String = "TITLE_HEROIC_BOSSES_PANEL";
      
      public static const UNAVAILABLE_PANEL_TITLE:String = "UNAVAILABLE_PANEL_TITLE";
      
      public static const UNAVAILABLE_PANEL_DESCRIPTION_1:String = "UNAVAILABLE_PANEL_DESCRIPTION_1";
      
      public static const UNAVAILABLE_PANEL_DESCRIPTION_2:String = "UNAVAILABLE_PANEL_DESCRIPTION_2";
      
      public static const AVAILABLE_PANEL_DESCRIPTION:String = "AVAILABLE_PANEL_DESCRIPTION";
      
      public static const PARAMS_TITLE:String = "PARAMS_TITLE";
      
      public static const ALLOCATE_POINTS:String = "ALLOCATE_POINTS";
      
      public static const NO_POINTS:String = "NO_POINTS";
      
      public static const POINTS_ICON_QTIP:String = "POINTS_ICON_QTIP";
      
      public static const ARMOR_DESCRIPTION:String = "ARMOR_DESCRIPTION";
      
      public static const ATTACK_DESCRIPTION:String = "ATTACK_DESCRIPTION";
      
      public static const REACTION_DESCRIPTION:String = "REACTION_DESCRIPTION";
      
      public static const RESET_PARAMS_NAME:String = "RESET_PARAMS_NAME";
      
      public static const RESET_PARAMS_DESC:String = "RESET_PARAMS_DESC";
      
      public static const HOUSE_QTIP:String = "HOUSE_QTIP";
      
      public static const NEWS_QTIP:String = "NEWS_QTIP";
      
      public static const ARMOR:String = "ARMOR";
      
      public static const ATTACK:String = "ATTACK";
      
      public static const ATTACK_POISON:String = "ATTACK_POISON";
      
      public static const ATTACK_FIRE:String = "ATTACK_FIRE";
      
      public static const REDUCTION_FIRE:String = "REDUCTION_FIRE";
      
      public static const LEECH:String = "LEECH";
      
      public static const REGENERATION:String = "REGENERATION";
      
      public static const MEDKIT_ENHANCEMENT:String = "MEDKIT_ENHANCEMENT";
      
      public static const DODGE:String = "DODGE";
      
      public static const REACTION:String = "REACTION";
      
      public static const HP:String = "HP";
      
      public static const RACE:String = "RACE";
      
      public static const SELECT_RACE:String = "SELECT_RACE";
      
      public static const SELECTED_RACE:String = "SELECTED_RACE";
      
      public static const TO_HP:String = "TO_HP";
      
      public static const RATING:String = "RATING";
      
      public static const RATING_DESCRIPTION:String = "RATING_DESCRIPTION";
      
      public static const RATING_WARNING:String = "RATING_WARNING";
      
      public static const RATING_AWARDS_TITLE:String = "RATING_AWARDS_TITLE";
      
      public static const RATING_AWARDS_TEXT:String = "RATING_AWARDS_TEXT";
      
      public static const HEAL_BONUS:String = "HEAL_BONUS";
      
      public static const REDUCTION_SHOCK:String = "REDUCTION_SHOCK";
      
      public static const ATTACK_SHOCK:String = "ATTACK_SHOCK";
      
      public static const INCREASE_AIR_DAMAGE:String = "INCREASE_AIR_DAMAGE";
      
      public static const AIR_REDUCTION:String = "AIR_REDUCTION";
      
      public static const CALL_TO_BATTLE_BY_ID:String = "CALL_TO_BATTLE_BY_ID";
      
      public static const REFRESH_FRIENDLIST:String = "REFRESH_FRIENDLIST";
      
      public static const TIPS_ON_OFF:String = "TIPS_ON_OFF";
      
      public static const CHAR_HINT_ON_OFF:String = "CHAR_HINT_ON_OFF";
      
      public static const SHOW_FRIEND_HOUSE:String = "SHOW_FRIEND_HOUSE";
      
      public static const §_-xc§:String = "LEVEL_NET_ENOUGH_FOR_SEARCH";
      
      public static const INVITE_FRIENDS_TEXT:String = "INVITE_FRIENDS_TEXT";
      
      public static const NO_FIVE_DAY_TITLE:String = "NO_FIVE_DAY_TITLE";
      
      public static const NO_FIVE_DAY_DESCR:String = "NO_FIVE_DAY_DESCR";
      
      public static const ALBUM_TITLE:String = "ALBUM_TITLE";
      
      public static const ALBUM_DESCRIPTION:String = "ALBUM_DESCRIPTION";
      
      public static const PHOTO_MENU_TITLE:String = "PHOTO_MENU_TITLE";
      
      public static const VIDEO_TITLE:String = "VIDEO_TITLE";
      
      public static const VIDEO_LIMIT_MSG:String = "VIDEO_LIMIT_MSG";
      
      public static const VIDEO_LIMIT_DESC:String = "VIDEO_LIMIT_DESC";
      
      public static const VIDEO_SAVE_TITLE:String = "VIDEO_SAVE_TITLE";
      
      public static const VIDEO_SAVE_ATTENTION:String = "VIDEO_SAVE_ATTENTION";
      
      public static const VIDEO_SAVE_SIZE:String = "VIDEO_SAVE_SIZE";
      
      public static const VIDEO_SAVE_QUESTION:String = "VIDEO_SAVE_QUESTION";
      
      public static const VIDEO_SAVE_SUCCESS:String = "VIDEO_SAVE_SUCCESS";
      
      public static const VIDEO_FILE_NAME:String = "VIDEO_FILE_NAME";
      
      public static const VIDEO_SAVING_TITLE:String = "VIDEO_SAVING_TITLE";
      
      public static const VIDEO_SAVING_DESC:String = "VIDEO_SAVING_DESC";
      
      public static const VIRAL_QUESTION_CHALLENGE_POST:String = "VIRAL_QUESTION_CHALLENGE_POST";
      
      public static const BANK_TITLE:String = "BANK_TITLE";
      
      public static const BANK_BUY_RUBY:String = "BANK_BUY_RUBY";
      
      public static const BANK_BUY_COIN:String = "BANK_BUY_COIN";
      
      public static const BANK_CHOOSE:String = "BANK_CHOOSE";
      
      public static const GO_AT_BANK:String = "GO_AT_BANK";
      
      public static const BUTTON_GO_AT_BANK:String = "BUTTON_GO_AT_BANK";
      
      public static const RUBY_PRICE:String = "RUBY_PRICE";
      
      public static const FUZZES_PRICE:String = "FUZZES_PRICE";
      
      public static const RUBY_PRICE_ALT_END:String = "RUBY_PRICE_ALT_END";
      
      public static const FUZZES_PRICE_ALT_END:String = "FUZZES_PRICE_ALT_END";
      
      public static const RUBY_PRICE_NO_END:String = "RUBY_PRICE_NO_END";
      
      public static const FUZZES_PRICE_NO_END:String = "FUZZES_PRICE_NO_END";
      
      public static const DR_DESCR:String = "DR_DESCR";
      
      public static const DR_CURRENCY:String = "DR_CURRENCY";
      
      public static const HU_CURRENCY:String = "HU_CURRENCY";
      
      public static const OK_CURRENCY:String = "OK_CURRENCY";
      
      public static const FS_CURRENCY:String = "FS_CURRENCY";
      
      public static const NK_CURRENCY:String = "NK_CURRENCY";
      
      public static const NATE_CURRENCY:String = "NATE_CURRENCY";
      
      public static const BANK_BONUS:String = "BANK_BONUS";
      
      public static const BANK_BONUS_INFO:String = "BANK_BONUS_INFO";
      
      public static const BANK_INCOME_PAYMENT_RUBY:String = "BANK_INCOME_PAYMENT_RUBY";
      
      public static const BANK_INCOME_PAYMENT_COIN:String = "BANK_INCOME_PAYMENT_COIN";
      
      public static const BANK_BUY_TITLE:String = "BANK_BUY_TITLE";
      
      public static const BANK_DEPOSIT_TITLE:String = "BANK_DEPOSIT_TITLE";
      
      public static const BANK_DEPOSIT_SINGLE_TITLE:String = "BANK_DEPOSIT_SINGLE_TITLE";
      
      public static const BANK_VIP_TITLE:String = "BANK_VIP_TITLE";
      
      public static const BANK_BUNDLE_TITLE:String = "BANK_BUNDLE_TITLE";
      
      public static const BANK_BUNDLES_TITLE:String = "BANK_BUNDLES_TITLE";
      
      public static const BANK_BUY_DESCR:String = "BANK_BUY_DESCR";
      
      public static const BANK_DEPOSIT_DESCR:String = "BANK_DEPOSIT_DESCR";
      
      public static const BANK_VIP_DESCR:String = "BANK_VIP_DESCR";
      
      public static const BANK_BUNDLE_DESCR:String = "BANK_BUNDLE_DESCR";
      
      public static const BANK_DEPOSIT_TIME:String = "BANK_DEPOSIT_TIME";
      
      public static const BANK_NO_BONUS:String = "BANK_NO_BONUS";
      
      public static const BANK_VALUE_TITLE:String = "BANK_VALUE_TITLE";
      
      public static const BANK_CLAN_DONATION_TITLE:String = "BANK_CLAN_DONATION_TITLE";
      
      public static const BANK_CLAN_DONATION_DESCRIPTION:String = "BANK_CLAN_DONATION_DESCRIPTION";
      
      public static const VIP_LABEL:String = "VIP_LABEL";
      
      public static const VIP_WEEK:String = "VIP_WEEK";
      
      public static const VIP_WEEK_TIME:String = "VIP_WEEK_TIME";
      
      public static const VIP_MONTH:String = "VIP_MONTH";
      
      public static const VIP_MONTH_TIME:String = "VIP_MONTH_TIME";
      
      public static const VIP_MONTH_TITLE:String = "VIP_MONTH_TITLE";
      
      public static const VIP_WEEK_TITLE:String = "VIP_WEEK_TITLE";
      
      public static const VIP_DESCR:String = "VIP_DESCR";
      
      public static const WEAPON_GET_ALL:String = "WEAPON_GET_ALL";
      
      public static const STICKERS_GET_ALL:String = "STICKERS_GET_ALL";
      
      public static const VIP_BOOSTER:String = "VIP_BOOSTER";
      
      public static const TIME_PERIOD:String = "TIME_PERIOD";
      
      public static const AUTO_RENEWING:String = "AUTO_RENEWING";
      
      public static const VIP_RENAME:String = "VIP_RENAME";
      
      public static const GET_DEPOSIT:String = "GET_DEPOSIT";
      
      public static const DEPOSIT_BUY_SUCCESS:String = "DEPOSIT_BUY_SUCCESS";
      
      public static const DEPOSIT_DAYS_TITLE:String = "DEPOSIT_DAYS_TITLE";
      
      public static const BATTLE_OFFER_TITLE:String = "BATTLE_OFFER_TITLE";
      
      public static const BATTLE_BOSS_OFFER_TITLE:String = "BATTLE_BOSS_OFFER_TITLE";
      
      public static const BATTLE_OFFER_ACCEPT:String = "BATTLE_OFFER_ACCEPT";
      
      public static const TITLE_BATTLE_OFFER_PROFILE_REJECT:String = "TITLE_BATTLE_OFFER_PROFILE_REJECT";
      
      public static const BATTLE_OFFER_PROFILE_REJECT:String = "BATTLE_OFFER_PROFILE_REJECT";
      
      public static const BATTLE_WAITE_LABLE:String = "BATTLE_WAITE_LABLE";
      
      public static const SEC_LABEL:String = "SEC_LABEL";
      
      public static const BATTLE_OP_REQUEST_TITLE:String = "BATTLE_OP_REQUEST_TITLE";
      
      public static const BATTLE_REQUEST_MSG:String = "BATTLE_REQUEST_MSG";
      
      public static const ENEMY_WAITING_MSG:String = "ENEMY_WAITING_MSG";
      
      public static const SEARCHING_RANDOM_PARTNER:String = "SEARCHING_RANDOM_PARTNER";
      
      public static const SEARCHING_RANDOM_PARTNER_TITLE:String = "SEARCHING_RANDOM_PARTNER_TITLE";
      
      public static const CHOOSE_FRIEND_BTN:String = "CHOOSE_FRIEND_BTN";
      
      public static const PVP_SERVER_INCORRECT_BATTLE_ID:String = "PVP_SERVER_INCORRECT_BATTLE_ID";
      
      public static const PVP_SERVER_BATTLE_CREATION_ERROR:String = "PVP_SERVER_BATTLE_CREATION_ERROR";
      
      public static const BATTLE_REQUEST_TIMEOUT:String = "BATTLE_REQUEST_TIMEOUT";
      
      public static const BATTLE_REQUEST_TITLE:String = "BATTLE_REQUEST_TITLE";
      
      public static const BATTLE_CANCEL_TITLE:String = "BATTLE_CANCEL_TITLE";
      
      public static const BATTLE_LOST_TITLE:String = "BATTLE_LOST_TITLE";
      
      public static const BATTLE_REQUEST_BUSY:String = "BATTLE_REQUEST_BUSY";
      
      public static const BATTLE_LOGIN_TIMEOUT:String = "BATTLE_LOGIN_TIMEOUT";
      
      public static const BATTLE_REQUEST_DECLINE:String = "BATTLE_REQUEST_DECLINE";
      
      public static const BATTLE_REQUEST_NOT_ONLINE:String = "BATTLE_REQUEST_NOT_ONLINE";
      
      public static const PVP_SERVER_CONNECTION_ERROR:String = "PVP_SERVER_CONNECTION_ERROR";
      
      public static const PVP_SERVER_LOGIN_ERROR:String = "PVP_SERVER_LOGIN_ERROR";
      
      public static const PVP_SERVER_CONNECTION_CLOSED:String = "PVP_SERVER_CONNECTION_CLOSED";
      
      public static const MAIN_SERVER_CONNECTION_CLOSED:String = "MAIN_SERVER_CONNECTION_CLOSED";
      
      public static const MAIN_SERVER_CONNECTION_CLOSED_LOSE:String = "MAIN_SERVER_CONNECTION_CLOSED_LOSE";
      
      public static const §_-82h§:String = "SERVER_CONNECTION_ERROR";
      
      public static const PVP_SERVER_PROFILACTIC_WORK:String = "PVP_SERVER_PROFILACTIC_WORK";
      
      public static const PVP_SERVER_BANNED:String = "PVP_SERVER_BANNED";
      
      public static const RECONNECT_TIMEOUT:String = "RECONNECT_TIMEOUT";
      
      public static const PVP_BATTLE_STUCK:String = "PVP_BATTLE_STUCK";
      
      public static const PVP_BATTLE_UNSYNC:String = "PVP_BATTLE_UNSYNC";
      
      public static const PVP_WAGER_AVAILABLE_FOR:String = "PVP_WAGER_AVAILABLE_FOR";
      
      public static const PVP_WAGER_AVAILABLE_FOR_TEAM_MIN:String = "PVP_WAGER_AVAILABLE_FOR_TEAM_MIN";
      
      public static const PVP_WAGER_AVAILABLE_FOR_TEAM_MAX:String = "PVP_WAGER_AVAILABLE_FOR_TEAM_MAX";
      
      public static const YOUR_ID:String = "YOUR_ID";
      
      public static const ENEMY_ID:String = "ENEMY_ID";
      
      public static const ENTER_ID_FRIEND:String = "ENTER_ID_FRIEND";
      
      public static const ENTER_INCORRECT_ID_FRIEND:String = "ENTER_INCORRECT_ID_FRIEND";
      
      public static const CALL_TO_BATTLE_YOUSELF:String = "CALL_TO_BATTLE_YOUSELF";
      
      public static const ENTER_ID_FRIEND_IS_NOT_FRIEND:String = "ENTER_ID_FRIEND_IS_NOT_FRIEND";
      
      public static const EXCEEDED_MAX_LENGTH:String = "EXCEEDED_MAX_LENGTH";
      
      public static const COIN_QTIP:String = "COIN_QTIP";
      
      public static const RUBY_QTIP:String = "RUBY_QTIP";
      
      public static const TOKENS_QTIP:String = "TOKENS_QTIP";
      
      public static const COIN_DESCR:String = "COIN_DESCR";
      
      public static const RUBY_DESCR:String = "RUBY_DESCR";
      
      public static const TOKENS_DESCR:String = "TOKENS_DESCR";
      
      public static const OK_PAYMENT_NAME:String = "OK_PAYMENT_NAME";
      
      public static const ENEMY_TURN_MSG:String = "ENEMY_TURN_MSG";
      
      public static const WINNER_TITLE:String = "WINNER_TITLE";
      
      public static const LOOSER_TITLE:String = "LOOSER_TITLE";
      
      public static const DRAW_TITLE:String = "DRAW_TITLE";
      
      public static const OPPONENT_CHEATER:String = "OPPONENT_CHEATER";
      
      public static const OPPONENT_SURRENDER_TITLE:String = "OPPONENT_SURRENDER_TITLE";
      
      public static const PROTOCOL_CHEATER:String = "PROTOCOL_CHEATER";
      
      public static const FPS_CHEATER:String = "FPS_CHEATER";
      
      public static const SPEED_HACK_CHEATER:String = "SPEED_HACK_CHEATER";
      
      public static const SERVER_SAID_I_AM_CHEATER:String = "SERVER_SAID_I_AM_CHEATER";
      
      public static const WINNER_RATING:String = "WINNER_RATING";
      
      public static const WINNER_MONEY:String = "WINNER_MONEY";
      
      public static const LOSED_RATING:String = "LOSED_RATING";
      
      public static const LOSED_MONEY:String = "LOSED_MONEY";
      
      public static const GO_EXP_LABEL:String = "GO_EXP_LABEL";
      
      public static const GO_BONUS_EXP_LABEL:String = "GO_BONUS_EXP_LABEL";
      
      public static const GO_MONEY_LABEL:String = "GO_MONEY_LABEL";
      
      public static const GO_NEW_LEVEL:String = "GO_NEW_LEVEL";
      
      public static const LEVEL_LABEL:String = "LEVEL_LABEL";
      
      public static const BONUS:String = "BONUS";
      
      public static const PROFITABLY:String = "PROFITABLY";
      
      public static const POPULARLY:String = "POPULARLY";
      
      public static const BATTLE_RESULT_SAVING:String = "BATTLE_RESULT_SAVING";
      
      public static const NOTE_TITLE:String = "NOTE_TITLE";
      
      public static const NOTE_TITLE_WINNER:String = "NOTE_TITLE_WINNER";
      
      public static const NOTE_TITLE_LOOSER:String = "NOTE_TITLE_LOOSER";
      
      public static const NOTE_TITLE_DRAW_GAME:String = "NOTE_TITLE_DRAW_GAME";
      
      public static const NOTE_BODY:String = "NOTE_BODY";
      
      public static const NOTE_LEVEL:String = "NOTE_LEVEL";
      
      public static const TITLE_ADD_NOTE:String = "TITLE_ADD_NOTE";
      
      public static const NOTE_ADD_SUCCESS:String = "NOTE_ADD_SUCCESS";
      
      public static const NOTE_ADD_ERROR:String = "NOTE_ADD_ERROR";
      
      public static const CREATE_NOTE_BTN:String = "CREATE_NOTE_BTN";
      
      public static const DISABLE_CHAT:String = "DISABLE_CHAT";
      
      public static const CHAT_DISABLED:String = "CHAT_DISABLED";
      
      public static const ENABLE_CHAT:String = "ENABLE_CHAT";
      
      public static const CHAT_ENABLED:String = "CHAT_ENABLED";
      
      public static const CHAT_CALL_TO_PVP:String = "CHAT_CALL_TO_PVP";
      
      public static const EXIT_BATTLE:String = "EXIT_BATTLE";
      
      public static const EXIT_BATTLE_MSG:String = "EXIT_BATTLE_MSG";
      
      public static const EXP_QTIP:String = "EXP_QTIP";
      
      public static const SHOP_QTIP:String = "SHOP_QTIP";
      
      public static const BANK_QTIP:String = "BANK_QTIP";
      
      public static const SETTINGS_QTIP:String = "SETTINGS_QTIP";
      
      public static const VIP_QTIP:String = "VIP_QTIP";
      
      public static const HOME_QTIP:String = "HOME_QTIP";
      
      public static const DEPOSIT_QTIP:String = "DEPOSIT_QTIP";
      
      public static const LEVEL_QTIP:String = "LEVEL_QTIP";
      
      public static const RATING_QTIP:String = "RATING_QTIP";
      
      public static const MINIMAL_RATING_QTIP:String = "MINIMAL_RATING_QTIP";
      
      public static const BONUS_QTIP:String = "BONUS_QTIP";
      
      public static const WIND_QTIP:String = "WIND_QTIP";
      
      public static const CHAT_QTIP:String = "CHAT_QTIP";
      
      public static const BAG_QTIP:String = "BAG_QTIP";
      
      public static const FLAG_QTIP:String = "FLAG_QTIP";
      
      public static var SOUND_ON:String = "SOUND_ON";
      
      public static var SOUND_OFF:String = "SOUND_OFF";
      
      public static var SOUND_LABEL:String = "SOUND_LABEL";
      
      public static var MUSIC_ON:String = "MUSIC_ON";
      
      public static var MUSIC_OFF:String = "MUSIC_OFF";
      
      public static var MUSIC_LABEL:String = "MUSIC_LABEL";
      
      public static var PVP_IGNORE_MODE_ON:String = "PVP_IGNORE_MODE_ON";
      
      public static var PVP_IGNORE_MODE_OFF:String = "PVP_IGNORE_MODE_OFF";
      
      public static var PVP_IGNORE_LABEL:String = "PVP_IGNORE_LABEL";
      
      public static var STATUS_ON:String = "STATUS_ON";
      
      public static var STATUS_OFF:String = "STATUS_OFF";
      
      public static var FULLSCREEN_ON:String = "FULLSCREEN_ON";
      
      public static var FULLSCREEN_OFF:String = "FULLSCREEN_OFF";
      
      public static var FULLSCREEN_LABEL:String = "FULLSCREEN_LABEL";
      
      public static var PERSONALIZED_ADS_LABEL:String = "PERSONALIZED_ADS_LABEL";
      
      public static var ALLSOUND_QTIP:String = "ALLSOUND_QTIP";
      
      public static var VOLUME_QTIP:String = "VOLUME_QTIP";
      
      public static var QUALITY_HIGH_QTIP:String = "QUALITY_HIGH_QTIP";
      
      public static var QUALITY_MEDIUM_QTIP:String = "QUALITY_MEDIUM_QTIP";
      
      public static var QUALITY_LOW_QTIP:String = "QUALITY_LOW_QTIP";
      
      public static var PHOTO_QTIP:String = "PHOTO_QTIP";
      
      public static var VIDEO_QTIP:String = "VIDEO_QTIP";
      
      public static var SCALE_QTIP:String = "SCALE_QTIP";
      
      public static var REACTION_QTIP:String = "REACTION_QTIP";
      
      public static var ENTER_FROM_CLOUD_LABEL:String = "ENTER_FROM_CLOUD_LABEL";
      
      public static var GOOGLE_SAVE_LABEL:String = "GOOGLE_SAVE_LABEL";
      
      public static var FREE_LABEL:String = "FREE_LABEL";
      
      public static const POST_NEWS_TITLE:String = "POST_NEWS_TITLE";
      
      public static const STATUS_TEXT:String = "STATUS_TEXT";
      
      public static const CHOOOSE_FRIEND_BTN_NAME:String = "CHOOOSE_FRIEND_BTN_NAME";
      
      public static const LEARN_BTN_NAME:String = "LEARN_BTN_NAME";
      
      public static const HOT_SEAT_CARD_TITLE:String = "HOT_SEAT_CARD_TITLE";
      
      public static const HOT_SEAT_CARD_DESC:String = "HOT_SEAT_CARD_DESC";
      
      public static const UNKNOWN_CARD_NAME:String = "UNKNOWN_CARD_NAME";
      
      public static const UNKNOWN_CARD_DESC:String = "UNKNOWN_CARD_DESC";
      
      public static const PLAY_MISSION_BTN_NAME:String = "PLAY_MISSION_BTN_NAME";
      
      public static const PVP_FIND_ENEMY:String = "PVP_FIND_ENEMY";
      
      public static const HARDNESS:String = "HARDNESS";
      
      public static const EASY:String = "EASY";
      
      public static const MEDIUM:String = "MEDIUM";
      
      public static const HARD:String = "HARD";
      
      public static const ENEMIES_COUNT_LABEL:String = "ENEMIES_COUNT_LABEL";
      
      public static const EXIT_QTIP:String = "EXIT_QTIP";
      
      public static const TEAM_TITLE:String = "TEAM_TITLE";
      
      public static const RACE_TITLE:String = "RACE_TITLE";
      
      public static const CAN_HIRE_AT_LEVEL:String = "CAN_HIRE_AT_LEVEL";
      
      public static const ARMORY_QTIP:String = "ARMORY_QTIP";
      
      public static const TEAM_QTIP:String = "TEAM_QTIP";
      
      public static const RACE_QTIP:String = "RACE_QTIP";
      
      public static const CRAFT_QTIP:String = "CRAFT_QTIP";
      
      public static const WARDROBE_QTIP:String = "WARDROBE_QTIP";
      
      public static const WARDROBE_REMOVE_ALL:String = "WARDROBE_REMOVE_ALL";
      
      public static const WARDROBE_HATS_TITLE:String = "WARDROBE_HATS_TITLE";
      
      public static const WARDROBE_ARTIFACTS_TITLE:String = "WARDROBE_ARTIFACTS_TITLE";
      
      public static const TAB_HIRE_FROM_CLAN:String = "TAB_HIRE_FROM_CLAN";
      
      public static const TAB_HIRE_FRIEND:String = "TAB_HIRE_FRIEND";
      
      public static const TAB_HIRE_BOT:String = "TAB_HIRE_BOT";
      
      public static const MERCENARY_TEAM_MEMBER_DESC:String = "MERCENARY_TEAM_MEMBER_DESC";
      
      public static const FRIEND_TEAM_MEMBER_DESC:String = "FRIEND_TEAM_MEMBER_DESC";
      
      public static const CLAN_TEAM_MEMBER_DESC:String = "CLAN_TEAM_MEMBER_DESC";
      
      public static const NO_PROFILES_FOR_ADD_TO_TEAM:String = "NO_PROFILES_FOR_ADD_TO_TEAM";
      
      public static const MEMBER_SOCLAN_ALREADY_EXIST_LIMIT:String = "MEMBER_SOCLAN_ALREADY_EXIST_LIMIT";
      
      public static const BAG_TITLE:String = "BAG_TITLE";
      
      public static const RATING_TITLE:String = "RATING_TITLE";
      
      public static const RATING_POS_LABEL:String = "RATING_POS_LABEL";
      
      public static const RATING_TO_DAY:String = "RATING_TO_DAY";
      
      public static const RATING_YESTERDAY:String = "RATING_YESTERDAY";
      
      public static const RATING_ALL_TIME:String = "RATING_ALL_TIME";
      
      public static const RATING_CLANS:String = "RATING_CLANS";
      
      public static const RATING_BATTLES_ALL:String = "RATING_BATTLES_ALL";
      
      public static const RATING_BATTLES_BET_SMALL:String = "RATING_BATTLES_BET_SMALL";
      
      public static const RATING_BATTLES_BET_MED:String = "RATING_BATTLES_BET_MED";
      
      public static const RATING_BTN_2х2:String = "RATING_BTN_2х2";
      
      public static const RATING_BTN_2х2_FRIEND:String = "RATING_BTN_2х2_FRIEND";
      
      public static const NOT_ENOUGH_RATING_TITLE:String = "NOT_ENOUGH_RATING_TITLE";
      
      public static const NOT_ENOUGH_RATING_DESCR:String = "NOT_ENOUGH_RATING_DESCR";
      
      public static const LOWER_RATING_LEAGUE:String = "LOWER_RATING_LEAGUE";
      
      public static const FIGHTER_RATING_LEAGUE:String = "FIGHTER_RATING_LEAGUE";
      
      public static const VETERAN_RATING_LEAGUE:String = "VETERAN_RATING_LEAGUE";
      
      public static const MASTER_RATING_LEAGUE:String = "MASTER_RATING_LEAGUE";
      
      public static const ACE_RATING_LEAGUE:String = "ACE_RATING_LEAGUE";
      
      public static const RUBY_RATING_LEAGUE:String = "RUBY_RATING_LEAGUE";
      
      public static const MIN_RATING:String = "MIN_RATING";
      
      public static const DESCR_LOWER_RATING_LEAGUE:String = "DESCR_LOWER_RATING_LEAGUE";
      
      public static const DESCR_FIGHTER_RATING_LEAGUE:String = "DESCR_FIGHTER_RATING_LEAGUE";
      
      public static const DESCR_VETERAN_RATING_LEAGUE:String = "DESCR_VETERAN_RATING_LEAGUE";
      
      public static const DESCR_MASTER_RATING_LEAGUE:String = "DESCR_MASTER_RATING_LEAGUE";
      
      public static const DESCR_ACE_RATING_LEAGUE:String = "DESCR_ACE_RATING_LEAGUE";
      
      public static const DESCR_RUBY_RATING_LEAGUE:String = "DESCR_RUBY_RATING_LEAGUE";
      
      public static const HELP_TITLE:String = "HELP_TITLE";
      
      public static const BONUS_HOLIDAYS_TITLE:String = "BONUS_HOLIDAYS_TITLE";
      
      public static const BONUS_HOLIDAYS_DEFAULT_MSG:String = "BONUS_HOLIDAYS_DEFAULT_MSG";
      
      public static const DAILY_BONUS_TITLE:String = "DAILY_BONUS_TITLE";
      
      public static const DEPOSIT_INCOME_TITLE:String = "DEPOSIT_INCOME_TITLE";
      
      public static const DEPOSIT_INCOME_MSG:String = "DEPOSIT_INCOME_MSG";
      
      public static const COLLECT:String = "COLLECT";
      
      public static const DOUBLE_LABEL:String = "DOUBLE_LABEL";
      
      public static const REFUND_WAGER:String = "REFUND_WAGER";
      
      public static const NEED_RUBY_TITLE:String = "NEED_RUBY_TITLE";
      
      public static const BONUS_EXP:String = "BONUS_EXP";
      
      public static const BONUS_FUZY:String = "BONUS_FUZY";
      
      public static const RANDOM_MAP:String = "RANDOM_MAP";
      
      public static const SELECT_RANDOM_MAP:String = "SELECT_RANDOM_MAP";
      
      public static const SELECT_MAP_TITLE:String = "SELECT_MAP_TITLE";
      
      public static const MAP_TITLE:String = "MAP_TITLE";
      
      public static const RACE_BOXER_NAME:String = "RACE_BOXER_NAME";
      
      public static const RACE_DEMON_NAME:String = "RACE_DEMON_NAME";
      
      public static const ABILITIES_LABEL:String = "ABILITIES_LABEL";
      
      public static const NO_ABILITIES:String = "NO_ABILITIES";
      
      public static const RACE_UNAVAILABLE:String = "RACE_UNAVAILABLE";
      
      public static const RACE_NO_WORMS:String = "RACE_NO_WORMS";
      
      public static const ACHIEVEMENTS_TITLE:String = "ACHIEVEMENTS_TITLE";
      
      public static const ACHIEV_QTIP:String = "ACHIEV_QTIP";
      
      public static const ACHIEV_PROGRESS_LABEL:String = "ACHIEV_PROGRESS_LABEL";
      
      public static const ACHIEV_AWARD_POINTS_LABEL:String = "ACHIEV_AWARD_POINTS_LABEL";
      
      public static const ACHIEV_POINTS_LABEL:String = "ACHIEV_POINTS_LABEL";
      
      public static const AVAILABLE_AWARD:String = "AVAILABLE_AWARD";
      
      public static const ACHIEV_BATTLE_BUTTON:String = "ACHIEV_BATTLE_BUTTON";
      
      public static const ACHIEV_GAME_BUTTON:String = "ACHIEV_GAME_BUTTON";
      
      public static const ACHIEV_BOSSES_BUTTON:String = "ACHIEV_BOSSES_BUTTON";
      
      public static const ACHIEV_STATISTIC_BUTTON:String = "ACHIEV_STATISTIC_BUTTON";
      
      public static const ACHIEV_AWARD_BUTTON:String = "ACHIEV_AWARD_BUTTON";
      
      public static const ACHIEV_SELECT_BUTTON:String = "ACHIEV_SELECT_BUTTON";
      
      public static const ACHIEV_GOAL_TITLE:String = "ACHIEV_GOAL_TITLE";
      
      public static const ACHIEV_LEVELUP_TITLE:String = "ACHIEV_LEVELUP_TITLE";
      
      public static const ACHIEV_BURNED_ENEMIES_1LEVEL_TITLE:String = "ACHIEV_BURNED_ENEMIES_1LEVEL_TITLE";
      
      public static const ACHIEV_BURNED_ENEMIES_2LEVEL_TITLE:String = "ACHIEV_BURNED_ENEMIES_2LEVEL_TITLE";
      
      public static const ACHIEV_BURNED_ENEMIES_3LEVEL_TITLE:String = "ACHIEV_BURNED_ENEMIES_3LEVEL_TITLE";
      
      public static const ACHIEV_DESTROYED_SQUARE_1LEVEL_TITLE:String = "ACHIEV_DESTROYED_SQUARE_1LEVEL_TITLE";
      
      public static const ACHIEV_DESTROYED_SQUARE_2LEVEL_TITLE:String = "ACHIEV_DESTROYED_SQUARE_2LEVEL_TITLE";
      
      public static const ACHIEV_DESTROYED_SQUARE_3LEVEL_TITLE:String = "ACHIEV_DESTROYED_SQUARE_3LEVEL_TITLE";
      
      public static const ACHIEV_DROWNED_OPPONENTS_1LEVEL_TITLE:String = "ACHIEV_DROWNED_OPPONENTS_1LEVEL_TITLE";
      
      public static const ACHIEV_DROWNED_OPPONENTS_2LEVEL_TITLE:String = "ACHIEV_DROWNED_OPPONENTS_2LEVEL_TITLE";
      
      public static const ACHIEV_DROWNED_OPPONENTS_3LEVEL_TITLE:String = "ACHIEV_DROWNED_OPPONENTS_3LEVEL_TITLE";
      
      public static const ACHIEV_WAGER_WINNER_1LEVEL_TITLE:String = "ACHIEV_WAGER_WINNER_1LEVEL_TITLE";
      
      public static const ACHIEV_WAGER_WINNER_2LEVEL_TITLE:String = "ACHIEV_WAGER_WINNER_2LEVEL_TITLE";
      
      public static const ACHIEV_WAGER_WINNER_3LEVEL_TITLE:String = "ACHIEV_WAGER_WINNER_3LEVEL_TITLE";
      
      public static const ACHIEV_DIPLOMAT_1LEVEL_TITLE:String = "ACHIEV_DIPLOMAT_1LEVEL_TITLE";
      
      public static const ACHIEV_DIPLOMAT_2LEVEL_TITLE:String = "ACHIEV_DIPLOMAT_2LEVEL_TITLE";
      
      public static const ACHIEV_DIPLOMAT_3LEVEL_TITLE:String = "ACHIEV_DIPLOMAT_3LEVEL_TITLE";
      
      public static const ACHIEV_WIND_KILLS_1LEVEL_TITLE:String = "ACHIEV_WIND_KILLS_1LEVEL_TITLE";
      
      public static const ACHIEV_WIND_KILLS_2LEVEL_TITLE:String = "ACHIEV_WIND_KILLS_2LEVEL_TITLE";
      
      public static const ACHIEV_WIND_KILLS_3LEVEL_TITLE:String = "ACHIEV_WIND_KILLS_3LEVEL_TITLE";
      
      public static const ACHIEV_MASSIVE_DAMAGE_1LEVEL_TITLE:String = "ACHIEV_MASSIVE_DAMAGE_1LEVEL_TITLE";
      
      public static const ACHIEV_MASSIVE_DAMAGE_2LEVEL_TITLE:String = "ACHIEV_MASSIVE_DAMAGE_2LEVEL_TITLE";
      
      public static const ACHIEV_MASSIVE_DAMAGE_3LEVEL_TITLE:String = "ACHIEV_MASSIVE_DAMAGE_3LEVEL_TITLE";
      
      public static const ACHIEV_GRAVES_SANK_1LEVEL_TITLE:String = "ACHIEV_GRAVES_SANK_1LEVEL_TITLE";
      
      public static const ACHIEV_GRAVES_SANK_2LEVEL_TITLE:String = "ACHIEV_GRAVES_SANK_2LEVEL_TITLE";
      
      public static const ACHIEV_GRAVES_SANK_3LEVEL_TITLE:String = "ACHIEV_GRAVES_SANK_3LEVEL_TITLE";
      
      public static const ACHIEV_ONE_HP_WIN_1LEVEL_TITLE:String = "ACHIEV_ONE_HP_WIN_1LEVEL_TITLE";
      
      public static const ACHIEV_ONE_HP_WIN_2LEVEL_TITLE:String = "ACHIEV_ONE_HP_WIN_2LEVEL_TITLE";
      
      public static const ACHIEV_ONE_HP_WIN_3LEVEL_TITLE:String = "ACHIEV_ONE_HP_WIN_3LEVEL_TITLE";
      
      public static const ACHIEV_IMMOBILE_KILLS_1LEVEL_TITLE:String = "ACHIEV_IMMOBILE_KILLS_1LEVEL_TITLE";
      
      public static const ACHIEV_IMMOBILE_KILLS_2LEVEL_TITLE:String = "ACHIEV_IMMOBILE_KILLS_2LEVEL_TITLE";
      
      public static const ACHIEV_IMMOBILE_KILLS_3LEVEL_TITLE:String = "ACHIEV_IMMOBILE_KILLS_3LEVEL_TITLE";
      
      public static const ACHIEV_FLAWLESS_VICTORIES_1LEVEL_TITLE:String = "ACHIEV_FLAWLESS_VICTORIES_1LEVEL_TITLE";
      
      public static const ACHIEV_FLAWLESS_VICTORIES_2LEVEL_TITLE:String = "ACHIEV_FLAWLESS_VICTORIES_2LEVEL_TITLE";
      
      public static const ACHIEV_FLAWLESS_VICTORIES_3LEVEL_TITLE:String = "ACHIEV_FLAWLESS_VICTORIES_3LEVEL_TITLE";
      
      public static const ACHIEV_FLEW_MILES_1LEVEL_TITLE:String = "ACHIEV_FLEW_MILES_1LEVEL_TITLE";
      
      public static const ACHIEV_FLEW_MILES_2LEVEL_TITLE:String = "ACHIEV_FLEW_MILES_2LEVEL_TITLE";
      
      public static const ACHIEV_FLEW_MILES_3LEVEL_TITLE:String = "ACHIEV_FLEW_MILES_3LEVEL_TITLE";
      
      public static const ACHIEV_DOUBLE_KILLLS_1LEVEL_TITLE:String = "ACHIEV_DOUBLE_KILLLS_1LEVEL_TITLE";
      
      public static const ACHIEV_DOUBLE_KILLLS_2LEVEL_TITLE:String = "ACHIEV_DOUBLE_KILLLS_2LEVEL_TITLE";
      
      public static const ACHIEV_DOUBLE_KILLLS_3LEVEL_TITLE:String = "ACHIEV_DOUBLE_KILLLS_3LEVEL_TITLE";
      
      public static const ACHIEV_MEDKIT_KILLS_1LEVEL_TITLE:String = "ACHIEV_MEDKIT_KILLS_1LEVEL_TITLE";
      
      public static const ACHIEV_MEDKIT_KILLS_2LEVEL_TITLE:String = "ACHIEV_MEDKIT_KILLS_2LEVEL_TITLE";
      
      public static const ACHIEV_MEDKIT_KILLS_3LEVEL_TITLE:String = "ACHIEV_MEDKIT_KILLS_3LEVEL_TITLE";
      
      public static const ACHIEV_GATHERED_SUPPLIES_1LEVEL_TITLE:String = "ACHIEV_GATHERED_SUPPLIES_1LEVEL_TITLE";
      
      public static const ACHIEV_GATHERED_SUPPLIES_2LEVEL_TITLE:String = "ACHIEV_GATHERED_SUPPLIES_2LEVEL_TITLE";
      
      public static const ACHIEV_GATHERED_SUPPLIES_3LEVEL_TITLE:String = "ACHIEV_GATHERED_SUPPLIES_3LEVEL_TITLE";
      
      public static const ACHIEV_KAMIKAZE_1LEVEL_TITLE:String = "ACHIEV_KAMIKAZE_1LEVEL_TITLE";
      
      public static const ACHIEV_KAMIKAZE_2LEVEL_TITLE:String = "ACHIEV_KAMIKAZE_2LEVEL_TITLE";
      
      public static const ACHIEV_KAMIKAZE_3LEVEL_TITLE:String = "ACHIEV_KAMIKAZE_3LEVEL_TITLE";
      
      public static const ACHIEV_ROPED_MILES_1LEVEL_TITLE:String = "ACHIEV_ROPED_MILES_1LEVEL_TITLE";
      
      public static const ACHIEV_ROPED_MILES_2LEVEL_TITLE:String = "ACHIEV_ROPED_MILES_2LEVEL_TITLE";
      
      public static const ACHIEV_ROPED_MILES_3LEVEL_TITLE:String = "ACHIEV_ROPED_MILES_3LEVEL_TITLE";
      
      public static const ACHIEV_FUZZES_SPENT_1LEVEL_TITLE:String = "ACHIEV_FUZZES_SPENT_1LEVEL_TITLE";
      
      public static const ACHIEV_FUZZES_SPENT_2LEVEL_TITLE:String = "ACHIEV_FUZZES_SPENT_2LEVEL_TITLE";
      
      public static const ACHIEV_FUZZES_SPENT_3LEVEL_TITLE:String = "ACHIEV_FUZZES_SPENT_3LEVEL_TITLE";
      
      public static const ACHIEV_RUBIES_SPENT_1LEVEL_TITLE:String = "ACHIEV_RUBIES_SPENT_1LEVEL_TITLE";
      
      public static const ACHIEV_RUBIES_SPENT_2LEVEL_TITLE:String = "ACHIEV_RUBIES_SPENT_2LEVEL_TITLE";
      
      public static const ACHIEV_RUBIES_SPENT_3LEVEL_TITLE:String = "ACHIEV_RUBIES_SPENT_3LEVEL_TITLE";
      
      public static const ACHIEV_PUMPED_REACTION_1LEVEL_TITLE:String = "ACHIEV_PUMPED_REACTION_1LEVEL_TITLE";
      
      public static const ACHIEV_PUMPED_REACTION_2LEVEL_TITLE:String = "ACHIEV_PUMPED_REACTION_2LEVEL_TITLE";
      
      public static const ACHIEV_PUMPED_REACTION_3LEVEL_TITLE:String = "ACHIEV_PUMPED_REACTION_3LEVEL_TITLE";
      
      public static const ACHIEV_RUBIES_FOUND_1LEVEL_TITLE:String = "ACHIEV_RUBIES_FOUND_1LEVEL_TITLE";
      
      public static const ACHIEV_RUBIES_FOUND_2LEVEL_TITLE:String = "ACHIEV_RUBIES_FOUND_2LEVEL_TITLE";
      
      public static const ACHIEV_RUBIES_FOUND_3LEVEL_TITLE:String = "ACHIEV_RUBIES_FOUND_3LEVEL_TITLE";
      
      public static const ACHIEV_MADE_PHOTO_1LEVEL_TITLE:String = "ACHIEV_MADE_PHOTO_1LEVEL_TITLE";
      
      public static const ACHIEV_MADE_PHOTO_2LEVEL_TITLE:String = "ACHIEV_MADE_PHOTO_2LEVEL_TITLE";
      
      public static const ACHIEV_MADE_PHOTO_3LEVEL_TITLE:String = "ACHIEV_MADE_PHOTO_3LEVEL_TITLE";
      
      public static const ACHIEV_MADE_VIDEO_1LEVEL_TITLE:String = "ACHIEV_MADE_VIDEO_1LEVEL_TITLE";
      
      public static const ACHIEV_MADE_VIDEO_2LEVEL_TITLE:String = "ACHIEV_MADE_VIDEO_2LEVEL_TITLE";
      
      public static const ACHIEV_MADE_VIDEO_3LEVEL_TITLE:String = "ACHIEV_MADE_VIDEO_3LEVEL_TITLE";
      
      public static const ACHIEV_GAME_VISITS_1LEVEL_TITLE:String = "ACHIEV_GAME_VISITS_1LEVEL_TITLE";
      
      public static const ACHIEV_GAME_VISITS_2LEVEL_TITLE:String = "ACHIEV_GAME_VISITS_2LEVEL_TITLE";
      
      public static const ACHIEV_GAME_VISITS_3LEVEL_TITLE:String = "ACHIEV_GAME_VISITS_3LEVEL_TITLE";
      
      public static const ACHIEV_DROP_WATER_1LEVEL_TITLE:String = "ACHIEV_DROP_WATER_1LEVEL_TITLE";
      
      public static const ACHIEV_DROP_WATER_2LEVEL_TITLE:String = "ACHIEV_DROP_WATER_2LEVEL_TITLE";
      
      public static const ACHIEV_DROP_WATER_3LEVEL_TITLE:String = "ACHIEV_DROP_WATER_3LEVEL_TITLE";
      
      public static const ACHIEV_DROP_WATER_FIRST_TURN_1LEVEL_TITLE:String = "ACHIEV_DROP_WATER_FIRST_TURN_1LEVEL_TITLE";
      
      public static const ACHIEV_DROP_WATER_FIRST_TURN_2LEVEL_TITLE:String = "ACHIEV_DROP_WATER_FIRST_TURN_2LEVEL_TITLE";
      
      public static const ACHIEV_DROP_WATER_FIRST_TURN_3LEVEL_TITLE:String = "ACHIEV_DROP_WATER_FIRST_TURN_3LEVEL_TITLE";
      
      public static const ACHIEV_KILL_WALKING_ZOMBIES_1LEVEL_TITLE:String = "ACHIEV_KILL_WALKING_ZOMBIES_1LEVEL_TITLE";
      
      public static const ACHIEV_KILL_WALKING_ZOMBIES_2LEVEL_TITLE:String = "ACHIEV_KILL_WALKING_ZOMBIES_2LEVEL_TITLE";
      
      public static const ACHIEV_KILL_WALKING_ZOMBIES_3LEVEL_TITLE:String = "ACHIEV_KILL_WALKING_ZOMBIES_3LEVEL_TITLE";
      
      public static const ACHIEV_KILL_WEAR_HATS_1LEVEL_TITLE:String = "ACHIEV_KILL_WEAR_HATS_1LEVEL_TITLE";
      
      public static const ACHIEV_KILL_WEAR_HATS_2LEVEL_TITLE:String = "ACHIEV_KILL_WEAR_HATS_2LEVEL_TITLE";
      
      public static const ACHIEV_KILL_WEAR_HATS_3LEVEL_TITLE:String = "ACHIEV_KILL_WEAR_HATS_3LEVEL_TITLE";
      
      public static const ACHIEV_PARTISAN_1LEVEL_TITLE:String = "ACHIEV_PARTISAN_1LEVEL_TITLE";
      
      public static const ACHIEV_PARTISAN_2LEVEL_TITLE:String = "ACHIEV_PARTISAN_2LEVEL_TITLE";
      
      public static const ACHIEV_PARTISAN_3LEVEL_TITLE:String = "ACHIEV_PARTISAN_3LEVEL_TITLE";
      
      public static const ACHIEV_COLLECTOR_1LEVEL_TITLE:String = "ACHIEV_COLLECTOR_1LEVEL_TITLE";
      
      public static const ACHIEV_COLLECTOR_2LEVEL_TITLE:String = "ACHIEV_COLLECTOR_2LEVEL_TITLE";
      
      public static const ACHIEV_COLLECTOR_3LEVEL_TITLE:String = "ACHIEV_COLLECTOR_3LEVEL_TITLE";
      
      public static const ACHIEV_BURNED_ENEMIES_DESCR:String = "ACHIEV_BURNED_ENEMIES_DESCR";
      
      public static const ACHIEV_DESTROYED_SQUARE_DESCR:String = "ACHIEV_DESTROYED_SQUARE_DESCR";
      
      public static const ACHIEV_DROWNED_OPPONENTS_DESCR:String = "ACHIEV_DROWNED_OPPONENTS_DESCR";
      
      public static const ACHIEV_WAGER_WINNER_DESCR:String = "ACHIEV_WAGER_WINNER_DESCR";
      
      public static const ACHIEV_DIPLOMAT_DESCR:String = "ACHIEV_DIPLOMAT_DESCR";
      
      public static const ACHIEV_WIND_KILLS_DESCR:String = "ACHIEV_WIND_KILLS_DESCR";
      
      public static const ACHIEV_MASSIVE_DAMAGE_DESCR:String = "ACHIEV_MASSIVE_DAMAGE_DESCR";
      
      public static const ACHIEV_GRAVES_SANK_DESCR:String = "ACHIEV_GRAVES_SANK_DESCR";
      
      public static const ACHIEV_ONE_HP_WIN_DESCR:String = "ACHIEV_ONE_HP_WIN_DESCR";
      
      public static const ACHIEV_IMMOBILE_KILLS_DESCR:String = "ACHIEV_IMMOBILE_KILLS_DESCR";
      
      public static const ACHIEV_FLAWLESS_VICTORIES:String = "ACHIEV_FLAWLESS_VICTORIES";
      
      public static const ACHIEV_FLEW_MILES_DESCR:String = "ACHIEV_FLEW_MILES_DESCR";
      
      public static const ACHIEV_DOUBLE_KILLLS_DESCR:String = "ACHIEV_DOUBLE_KILLLS_DESCR";
      
      public static const ACHIEV_MEDKIT_KILLS_DESCR:String = "ACHIEV_MEDKIT_KILLS_DESCR";
      
      public static const ACHIEV_GATHERED_SUPPLIES_DESCR:String = "ACHIEV_GATHERED_SUPPLIES_DESCR";
      
      public static const ACHIEV_KAMIKAZE_DESCR:String = "ACHIEV_KAMIKAZE_DESCR";
      
      public static const ACHIEV_ROPED_MILES_DESCR:String = "ACHIEV_ROPED_MILES_DESCR";
      
      public static const ACHIEV_FUZZES_SPENT_DESCR:String = "ACHIEV_FUZZES_SPENT_DESCR";
      
      public static const ACHIEV_RUBIES_SPENT_DESCR:String = "ACHIEV_RUBIES_SPENT_DESCR";
      
      public static const ACHIEV_PUMPED_REACTION_DESCR:String = "ACHIEV_PUMPED_REACTION_DESCR";
      
      public static const ACHIEV_RUBIES_FOUND_DESCR:String = "ACHIEV_RUBIES_FOUND_DESCR";
      
      public static const ACHIEV_MADE_PHOTO_DESCR:String = "ACHIEV_MADE_PHOTO_DESCR";
      
      public static const ACHIEV_MADE_VIDEO_DESCR:String = "ACHIEV_MADE_VIDEO_DESCR";
      
      public static const ACHIEV_DROP_WATER_DESCR:String = "ACHIEV_DROP_WATER_DESCR";
      
      public static const ACHIEV_KILL_WALKING_ZOMBIES_DESCR:String = "ACHIEV_KILL_WALKING_ZOMBIES_DESCR";
      
      public static const ACHIEV_WEAR_HATS_DESCR:String = "ACHIEV_WEAR_HATS_DESCR";
      
      public static const ACHIEV_GAME_VISITS_DESCR:String = "ACHIEV_GAME_VISITS_DESCR";
      
      public static const ACHIEV_DROP_WATER_FIRST_TURN_DESCR:String = "ACHIEV_DROP_WATER_FIRST_TURN_DESCR";
      
      public static const ACHIEV_PARTISAN_DESCR:String = "ACHIEV_PARTISAN_DESCR";
      
      public static const ACHIEV_COLLECTOR_DESCR:String = "ACHIEV_COLLECTOR_DESCR";
      
      public static const ACHIEV_GUNS_IN_BOX_DESCR:String = "ACHIEV_GUNS_IN_BOX_DESCR";
      
      public static const ACHIEV_GUNS_IN_BOX_1LEVEL_TITLE:String = "ACHIEV_GUNS_IN_BOX_1LEVEL_TITLE";
      
      public static const ACHIEV_GUNS_IN_BOX_2LEVEL_TITLE:String = "ACHIEV_GUNS_IN_BOX_2LEVEL_TITLE";
      
      public static const ACHIEV_GUNS_IN_BOX_3LEVEL_TITLE:String = "ACHIEV_GUNS_IN_BOX_3LEVEL_TITLE";
      
      public static const ACHIEV_WEAPONS_UPGRADE_DESCR:String = "ACHIEV_WEAPONS_UPGRADE_DESCR";
      
      public static const ACHIEV_WEAPONS_UPGRADE_1LEVEL_TITLE:String = "ACHIEV_WEAPONS_UPGRADE_1LEVEL_TITLE";
      
      public static const ACHIEV_WEAPONS_UPGRADE_2LEVEL_TITLE:String = "ACHIEV_WEAPONS_UPGRADE_2LEVEL_TITLE";
      
      public static const ACHIEV_WEAPONS_UPGRADE_3LEVEL_TITLE:String = "ACHIEV_WEAPONS_UPGRADE_3LEVEL_TITLE";
      
      public static const ACHIEV_CRAFT_LEGENDARY_DESCR:String = "ACHIEV_CRAFT_LEGENDARY_DESCR";
      
      public static const ACHIEV_CRAFT_LEGENDARY_1LEVEL_DESCR:String = "ACHIEV_CRAFT_LEGENDARY_1LEVEL_DESCR";
      
      public static const ACHIEV_CRAFT_LEGENDARY_1LEVEL_TITLE:String = "ACHIEV_CRAFT_LEGENDARY_1LEVEL_TITLE";
      
      public static const ACHIEV_CRAFT_LEGENDARY_2LEVEL_TITLE:String = "ACHIEV_CRAFT_LEGENDARY_2LEVEL_TITLE";
      
      public static const ACHIEV_CRAFT_LEGENDARY_3LEVEL_TITLE:String = "ACHIEV_CRAFT_LEGENDARY_3LEVEL_TITLE";
      
      public static const ACHIEV_SUPERBOSS_DEFEATED_DESCR:String = "ACHIEV_SUPERBOSS_DEFEATED_DESCR";
      
      public static const ACHIEV_SUPERBOSS_DEFEATED_1LEVEL_TITLE:String = "ACHIEV_SUPERBOSS_DEFEATED_1LEVEL_TITLE";
      
      public static const ACHIEV_SUPERBOSS_DEFEATED_2LEVEL_TITLE:String = "ACHIEV_SUPERBOSS_DEFEATED_2LEVEL_TITLE";
      
      public static const ACHIEV_SUPERBOSS_DEFEATED_3LEVEL_TITLE:String = "ACHIEV_SUPERBOSS_DEFEATED_3LEVEL_TITLE";
      
      public static const ACHIEV_OPEN_RACE_DESCR:String = "ACHIEV_OPEN_RACE_DESCR";
      
      public static const ACHIEV_OPEN_RACE_1LEVEL_DESCR:String = "ACHIEV_OPEN_RACE_1LEVEL_DESCR";
      
      public static const ACHIEV_OPEN_RACE_1LEVEL_TITLE:String = "ACHIEV_OPEN_RACE_1LEVEL_TITLE";
      
      public static const ACHIEV_OPEN_RACE_2LEVEL_TITLE:String = "ACHIEV_OPEN_RACE_2LEVEL_TITLE";
      
      public static const ACHIEV_OPEN_RACE_3LEVEL_TITLE:String = "ACHIEV_OPEN_RACE_3LEVEL_TITLE";
      
      public static const ACHIEV_RETURN_FRIENDS_DESCR:String = "ACHIEV_RETURN_FRIENDS_DESCR";
      
      public static const ACHIEV_RETURN_FRIENDS_1LEVEL_DESCR:String = "ACHIEV_RETURN_FRIENDS_1LEVEL_DESCR";
      
      public static const ACHIEV_RETURN_FRIENDS_1LEVEL_TITLE:String = "ACHIEV_RETURN_FRIENDS_1LEVEL_TITLE";
      
      public static const ACHIEV_RETURN_FRIENDS_2LEVEL_TITLE:String = "ACHIEV_RETURN_FRIENDS_2LEVEL_TITLE";
      
      public static const ACHIEV_RETURN_FRIENDS_3LEVEL_TITLE:String = "ACHIEV_RETURN_FRIENDS_3LEVEL_TITLE";
      
      public static const ACHIEV_COOP_FRIEND_WINNER_DESCR:String = "ACHIEV_COOP_FRIEND_WINNER_DESCR";
      
      public static const ACHIEV_COOP_FRIEND_WINNER_1LEVEL_TITLE:String = "ACHIEV_COOP_FRIEND_WINNER_1LEVEL_TITLE";
      
      public static const ACHIEV_COOP_FRIEND_WINNER_2LEVEL_TITLE:String = "ACHIEV_COOP_FRIEND_WINNER_2LEVEL_TITLE";
      
      public static const ACHIEV_COOP_FRIEND_WINNER_3LEVEL_TITLE:String = "ACHIEV_COOP_FRIEND_WINNER_3LEVEL_TITLE";
      
      public static const §_-E27§:String = "PVP_PLAYED";
      
      public static const §_-p1Z§:String = "PVP_WON";
      
      public static const §_-429§:String = "PVP_DRAWS";
      
      public static const §_-z1q§:String = "MISSIONS_PLAYED";
      
      public static const §_-j2z§:String = "MISSIONS_WON";
      
      public static const §_-qC§:String = "MISSIONS_EARNED";
      
      public static const §_-ke§:String = "WAGERS_EARNED";
      
      public static const §_-gS§:String = "RUBIES_FOUND";
      
      public static const §_-c1u§:String = "SOLO_DAMAGE";
      
      public static const §_-B3v§:String = "TEAM_DAMAGE";
      
      public static const §_-38§:String = "SOLO_DAMAGE_TAKEN";
      
      public static const §_-Sx§:String = "TEAM_DAMAGE_TAKEN";
      
      public static const §_-C2R§:String = "FRIENDLY_FIRE";
      
      public static const §_-42F§:String = "SNIPER_SHOTS";
      
      public static const §_-B2k§:String = "WIND_SHOTS";
      
      public static const §_-H2z§:String = "KILLED_BOXERS";
      
      public static const §_-D1Q§:String = "KILLED_DEMONS";
      
      public static const §_-j2L§:String = "KILLED_RABBITS";
      
      public static const §_-d1X§:String = "KILLED_ZOMBIES";
      
      public static const §_-422§:String = "KILLED_CATS";
      
      public static const §_-022§:String = "KILLED_REVIVED";
      
      public static const §_-Si§:String = "KILLED_DRAGONS";
      
      public static const §_-Jo§:String = "KILLED_BOTS";
      
      public static const KILLED_TOTAL:String = "KILLED_TOTAL";
      
      public static const §_-s2b§:String = "UNITS_LOST";
      
      public static const §_-935§:String = "COLLECTED_MEDKITS";
      
      public static const §_-91R§:String = "COLLECTED_CRATES";
      
      public static const §_-W1e§:String = "COLLECTED_STARS";
      
      public static const §_-Zz§:String = "DESTROYED_SUPPLIES";
      
      public static const §_-N1U§:String = "GRAVES_SANK";
      
      public static const §_-91x§:String = "METERS_FLOWN";
      
      public static const §_-K2Q§:String = "METERS_WALKED";
      
      public static const BUTTON_YES:String = "BUTTON_YES";
      
      public static const BUTTON_OK:String = "BUTTON_OK";
      
      public static const BUTTON_CANCEL:String = "BUTTON_CANCEL";
      
      public static const BUTTON_CLOSE:String = "BUTTON_CLOSE";
      
      public static const BUTTON_HIRE:String = "BUTTON_HIRE";
      
      public static const BUTTON_BUY:String = "BUTTON_BUY";
      
      public static const BUTTON_RESET:String = "BUTTON_RESET";
      
      public static const BUTTON_RESET_PARAMS:String = "BUTTON_RESET_PARAMS";
      
      public static const BUTTON_ADD:String = "BUTTON_ADD";
      
      public static const BUTTON_SELECT:String = "BUTTON_SELECT";
      
      public static const BUTTON_PUBLISH:String = "BUTTON_PUBLISH";
      
      public static const BUTTON_SEND:String = "BUTTON_SEND";
      
      public static const BUTTON_REQUEST:String = "BUTTON_REQUEST";
      
      public static const BUTTON_CREATE:String = "BUTTON_CREATE";
      
      public static const BUTTON_NEW_PROFILE:String = "BUTTON_NEW_PROFILE";
      
      public static const BUTTON_ASK_FRIEND:String = "BUTTON_ASK_FRIEND";
      
      public static const BUTTON_ASK_FRIEND2:String = "BUTTON_ASK_FRIEND2";
      
      public static const BUTTON_RESTART:String = "BUTTON_RESTART";
      
      public static const BUTTON_SELECT_MAP:String = "BUTTON_SELECT_MAP";
      
      public static const BUTTON_NEXT_FIGHT:String = "BUTTON_NEXT_FIGHT";
      
      public static const BUTTON_TO_MENU:String = "BUTTON_TO_MENU";
      
      public static const BUTTON_LOAD:String = "BUTTON_LOAD";
      
      public static const BUTTON_OVERWRITE:String = "BUTTON_OVERWRITE";
      
      public static const BUTTON_STAY:String = "BUTTON_STAY";
      
      public static const BUTTON_SOLO_PLAY:String = "BUTTON_SOLO_PLAY";
      
      public static const BUTTON_ON:String = "BUTTON_ON";
      
      public static const BUTTON_OFF:String = "BUTTON_OFF";
      
      public static const BUTTON_INFO:String = "BUTTON_INFO";
      
      public static const BUTTON_EDIT:String = "BUTTON_EDIT";
      
      public static const BUTTON_LEAVE:String = "BUTTON_LEAVE";
      
      public static const BUTTON_INCREASE:String = "BUTTON_INCREASE";
      
      public static const BUTTON_IMPROVE:String = "BUTTON_IMPROVE";
      
      public static const BUTTON_SAVE:String = "BUTTON_SAVE";
      
      public static const BATTLE_INIT:String = "BATTLE_INIT";
      
      public static const BATTLE_INIT_POS:String = "BATTLE_INIT_POS";
      
      public static const BATTLE_INIT_MAP:String = "BATTLE_INIT_MAP";
      
      public static const BATTLE_LOAD_MAP_CONFIG:String = "BATTLE_LOAD_MAP_CONFIG";
      
      public static const BATTLE_LOAD_MAP:String = "BATTLE_LOAD_MAP";
      
      public static const BATTLE_ENEMY_DATA_LOAD:String = "BATTLE_ENEMY_DATA_LOAD";
      
      public static const BATTLE_PVP_LOGIN:String = "BATTLE_PVP_LOGIN";
      
      public static const BATTLE_MISSION_LOAD:String = "BATTLE_MISSION_LOAD";
      
      public static const BATTLE_PVP_CONNECT:String = "BATTLE_PVP_CONNECT";
      
      public static const BATTLE_CLEARING:String = "BATTLE_CLEARING";
      
      public static const REACTION_MENU_TITLE:String = "REACTION_MENU_TITLE";
      
      public static const ASK_FRIEND_PUMP_REACTION_TITLE:String = "ASK_FRIEND_PUMP_REACTION_TITLE";
      
      public static const REACTION_MENU_BUTTON:String = "REACTION_MENU_BUTTON";
      
      public static const REACTION_MENU_MSG_NONE_SELECTED:String = "REACTION_MENU_MSG_NONE_SELECTED";
      
      public static const REACTION_MENU_MSG_NONE_PUMPED_3DAYS:String = "REACTION_MENU_MSG_NONE_PUMPED_3DAYS";
      
      public static const PUMP_REACTION_DAY_LIMIT_ADD:String = "PUMP_REACTION_DAY_LIMIT_ADD";
      
      public static const GRENADE_TIMER:String = "GRENADE_TIMER";
      
      public static const CROSSHAIR_TOO_HIGH:String = "CROSSHAIR_TOO_HIGH";
      
      public static const ATOM_MARK_ALREADY_EXIST:String = "ATOM_MARK_ALREADY_EXIST";
      
      public static const GIRDER_IMPOSSIBLE:String = "GIRDER_IMPOSSIBLE";
      
      public static const TELEPORT_IMPOSSIBLE:String = "TELEPORT_IMPOSSIBLE";
      
      public static const ALREADY_BANDAGED:String = "ALREADY_BANDAGED";
      
      public static const ALREADY_ANTITOXIN:String = "ALREADY_ANTITOXIN";
      
      public static const FORCE_FIELD_ALREADY_EXIST:String = "FORCE_FIELD_ALREADY_EXIST";
      
      public static const BANDAGE_USELESS:String = "BANDAGE_USELESS";
      
      public static const KAMIKAZE_ALREADY_ACTIVATED:String = "KAMIKAZE_ALREADY_ACTIVATED";
      
      public static const ALREADY_ACTIVE_UNIT:String = "ALREADY_ACTIVE_UNIT";
      
      public static const DOCTOR_BAG_USELESS:String = "DOCTOR_BAG_USELESS";
      
      public static const DOCTOR_BAG_USELESS_NO_ALLIES:String = "DOCTOR_BAG_USELESS_NO_ALLIES";
      
      public static const DOCTOR_BAG_USELESS_MAX_HP:String = "DOCTOR_BAG_USELESS_MAX_HP";
      
      public static const PRIZE_TWO_MISSION:String = "PRIZE_TWO_MISSION";
      
      public static const CONTAINS_RANDOM_WEAPON:String = "CONTAINS_RANDOM_WEAPON";
      
      public static const TITLE_MSG:String = "TITLE_MSG";
      
      public static const PUMP_REACTION_OK:String = "PUMP_REACTION_OK";
      
      public static const PUMP_REACTION_DAY_LIMIT:String = "PUMP_REACTION_DAY_LIMIT";
      
      public static const PUMP_REACTION_TODAY_ALREADY:String = "PUMP_REACTION_TODAY_ALREADY";
      
      public static const PUMP_REACTION_ERROR:String = "PUMP_REACTION_ERROR";
      
      public static const REACTION2_QTIP:String = "REACTION2_QTIP";
      
      public static const NOTE_BALLS_ATTENTION:String = "NOTE_BALLS_ATTENTION";
      
      public static const BUTTON_BALLS:String = "BUTTON_BALLS";
      
      public static const BUTTON_ARENA:String = "BUTTON_ARENA";
      
      public static const SEARCH_NO_RESULTS:String = "SEARCH_NO_RESULTS";
      
      public static const DAILY_BONUS_CONGRATZ:String = "DAILY_BONUS_CONGRATZ";
      
      public static const DAILY_BONUS_TEXT:String = "DAILY_BONUS_TEXT";
      
      public static const BANK_DEPOSIT_FIRST_DAY:String = "BANK_DEPOSIT_FIRST_DAY";
      
      public static const DAILY_BONUS_DAY:String = "DAILY_BONUS_DAY";
      
      public static const DEPOSIT_OVERALL_TITLE:String = "DEPOSIT_OVERALL_TITLE";
      
      public static const NEED_RUBY_BUTTON1_TEXT:String = "NEED_RUBY_BUTTON1_TEXT";
      
      public static const NEED_RUBY_BUTTON2_TEXT:String = "NEED_RUBY_BUTTON2_TEXT";
      
      public static const NEED_RUBY_BUTTON3_TEXT:String = "NEED_RUBY_BUTTON3_TEXT";
      
      public static const NEED_RUBY_BUTTON4_TEXT:String = "NEED_RUBY_BUTTON4_TEXT";
      
      public static const NEED_RUBY_CHECKBOX_TEXT:String = "NEED_RUBY_CHECKBOX_TEXT";
      
      public static const MASSIVE_DAMAGE:String = "MASSIVE_DAMAGE";
      
      public static const DOUBLE_KILL:String = "DOUBLE_KILL";
      
      public static const TRIPPLE_KILL:String = "TRIPPLE_KILL";
      
      public static const ULTRA_KILL:String = "ULTRA_KILL";
      
      public static const ATTRIBUTES_BONUS:String = "ATTRIBUTES_BONUS";
      
      public static const SYSPVP_OPPONENT_OUT:String = "SYSPVP_OPPONENT_OUT";
      
      public static const SYSPVP_INACTIVITY_DROP:String = "SYSPVP_INACTIVITY_DROP";
      
      public static const SYSPVP_UNCONFIRMED_EOT:String = "SYSPVP_UNCONFIRMED_EOT";
      
      public static const SYSPVP_OPPONENT_SURRENDER:String = "SYSPVP_OPPONENT_SURRENDER";
      
      public static const SYSPVP_OPPONENT_CHEATER:String = "SYSPVP_OPPONENT_CHEATER";
      
      public static const SYSPVP_OPPONENT_LOST_CONNECTION:String = "SYSPVP_OPPONENT_LOST_CONNECTION";
      
      public static const SYSPVP_OPPONENT_RETURNED:String = "SYSPVP_OPPONENT_RETURNED";
      
      public static const SYSPVP_LONG_TURN:String = "SYSPVP_LONG_TURN";
      
      public static const SYSPVP_UNDEFINED:String = "SYSPVP_UNDEFINED";
      
      public static const SYSPVP_BATTLE_ID:String = "SYSPVP_BATTLE_ID";
      
      public static const SYSPVP_YOU_PLAY_FOR:String = "SYSPVP_YOU_PLAY_FOR";
      
      public static const SYSPVP_ENEMY_ID:String = "SYSPVP_ENEMY_ID";
      
      public static const SYSPVP_ENEMIES_ID:String = "SYSPVP_ENEMIES_ID";
      
      public static const SYSPVP_ALLY_ID:String = "SYSPVP_ALLY_ID";
      
      public static const SYSPVP_ALLY_PLAYS_FOR:String = "SYSPVP_ALLY_PLAYS_FOR";
      
      public static const SYSPVP_NEW_TURN:String = "SYSPVP_NEW_TURN";
      
      public static const WAIT_FOR_RESULT:String = "WAIT_FOR_RESULT";
      
      public static const PLAY_CAPTION:String = "PLAY_CAPTION";
      
      public static const COMMON_CHAT_TITLE:String = "COMMON_CHAT_TITLE";
      
      public static const TEAM_CHAT_TITLE:String = "TEAM_CHAT_TITLE";
      
      public static const PAYMENT_MSG_FAILED:String = "PAYMENT_MSG_FAILED";
      
      public static const LITTLE_COUNT_POINTS:String = "LITTLE_COUNT_POINTS";
      
      public static const NEDD_AWARD_MSG:String = "NEDD_AWARD_MSG";
      
      public static const NEDD_AWARD:String = "NEDD_AWARD";
      
      public static const BTN_SHARE_NAME:String = "BTN_SHARE_NAME";
      
      public static const WIN_GAME_SHARE:String = "WIN_GAME_SHARE";
      
      public static const BTN_GREAT:String = "BTN_GREAT";
      
      public static const NO_KEYS_IN_SEARCH_HOUSE:String = "NO_KEYS_IN_SEARCH_HOUSE";
      
      public static const HINT_KEY_COUNT:String = "HINT_KEY_COUNT";
      
      public static const OPENING_SOON:String = "OPENING_SOON";
      
      public static const FORBIDDEN_TRANSITION_PAGE:String = "FORBIDDEN_TRANSITION_PAGE";
      
      public static const SENDER_MESSAGE_FRIEND:String = "SENDER_MESSAGE_FRIEND";
      
      public static const BOSS_GAMEOVER_TITLE_1:String = "BOSS_GAMEOVER_TITLE_1";
      
      public static const BOSS_GAMEOVER_TITLE_2:String = "BOSS_GAMEOVER_TITLE_2";
      
      public static const BOSS_GAMEOVER_BONUS_DESCRIPTION:String = "BOSS_GAMEOVER_BONUS_DESCRIPTION";
      
      public static const BOSS_GAMEOVER_REWARD_DESCRIPTION:String = "BOSS_GAMEOVER_REWARD_DESCRIPTION";
      
      public static const BOSS_GAMEOVER_TROPHY:String = "BOSS_GAMEOVER_TROPHY";
      
      public static const LEARNING_MISSION_1_NAME:String = "LEARNING_MISSION_1_NAME";
      
      public static const LEARNING_MISSION_2_NAME:String = "LEARNING_MISSION_2_NAME";
      
      public static const LEARNING_MISSION_3_NAME:String = "LEARNING_MISSION_3_NAME";
      
      public static const LEARNING_MISSION_4_NAME:String = "LEARNING_MISSION_4_NAME";
      
      public static const LEARNING_MISSION_5_NAME:String = "LEARNING_MISSION_5_NAME";
      
      public static const LEARNING_MISSION_1_DESCR:String = "LEARNING_MISSION_1_DESCR";
      
      public static const LEARNING_MISSION_2_DESCR:String = "LEARNING_MISSION_2_DESCR";
      
      public static const LEARNING_MISSION_3_DESCR:String = "LEARNING_MISSION_3_DESCR";
      
      public static const LEARNING_MISSION_4_DESCR:String = "LEARNING_MISSION_4_DESCR";
      
      public static const LEARNING_MISSION_5_DESCR:String = "LEARNING_MISSION_5_DESCR";
      
      public static const BOSS_PIRATE_NAME:String = "BOSS_PIRATE_NAME";
      
      public static const BOSS_PIRATE_DESCR:String = "BOSS_PIRATE_DESCR";
      
      public static const BOSS_ILLUSIONIST_NAME:String = "BOSS_ILLUSIONIST_NAME";
      
      public static const BOSS_ILLUSIONIST_DESCR:String = "BOSS_ILLUSIONIST_DESCR";
      
      public static const BOSS_SHAMAN_NAME:String = "BOSS_SHAMAN_NAME";
      
      public static const BOSS_SHAMAN_DESCR:String = "BOSS_SHAMAN_DESCR";
      
      public static const BOSS_VIKING_NAME:String = "BOSS_VIKING_NAME";
      
      public static const BOSS_VIKING_DESCR:String = "BOSS_VIKING_DESCR";
      
      public static const BOSS_WIND_MASTER_NAME:String = "BOSS_WIND_MASTER_NAME";
      
      public static const BOSS_WIND_MASTER_DESCR:String = "BOSS_WIND_MASTER_DESCR";
      
      public static const BOSS_SAMURAI_CAT_NAME:String = "BOSS_SAMURAI_CAT_NAME";
      
      public static const BOSS_SAMURAI_CAT_DESCR:String = "BOSS_SAMURAI_CAT_DESCR";
      
      public static const BOSS_MINER_NAME:String = "BOSS_MINER_NAME";
      
      public static const BOSS_MINER_DESCR:String = "BOSS_MINER_DESCR";
      
      public static const BOSS_ELDER_DEMONS_NAME:String = "BOSS_ELDER_DEMONS_NAME";
      
      public static const BOSS_ELDER_DEMONS_DESCR:String = "BOSS_ELDER_DEMONS_DESCR";
      
      public static const BOSS_SOUL_NAME:String = "BOSS_SOUL_NAME";
      
      public static const BOSS_SOUL_DESCR:String = "BOSS_SOUL_DESCR";
      
      public static const BOSS_GHOST_NAME:String = "BOSS_GHOST_NAME";
      
      public static const BOSS_GHOST_DESCR:String = "BOSS_GHOST_DESCR";
      
      public static const BOSS_YAKUZA_NAME:String = "BOSS_YAKUZA_NAME";
      
      public static const BOSS_YAKUZA_DESCR:String = "BOSS_YAKUZA_DESCR";
      
      public static const BOSS_ROMEO_JULIET_NAME:String = "BOSS_ROMEO_JULIET_NAME";
      
      public static const BOSS_ROMEO_JULIET_DESCR:String = "BOSS_ROMEO_JULIET_DESCR";
      
      public static const BOSS_KING_DEAD_NAME:String = "BOSS_KING_DEAD_NAME";
      
      public static const BOSS_KING_DEAD_DESCR:String = "BOSS_KING_DEAD_DESCR";
      
      public static const BOSS_DEAD_PIRATE_NAME:String = "BOSS_DEAD_PIRATE_NAME";
      
      public static const BOSS_DEAD_PIRATE_DESCR:String = "BOSS_DEAD_PIRATE_DESCR";
      
      public static const BOSS_SERGEANT_NAME:String = "BOSS_SERGEANT_NAME";
      
      public static const BOSS_SERGEANT_DESCR:String = "BOSS_SERGEANT_DESCR";
      
      public static const BOSS_SINGLE_MODE_BTN:String = "BOSS_SINGLE_MODE_BTN";
      
      public static const BOSS_PLAY_BTN:String = "BOSS_PLAY_BTN";
      
      public static const BOSS_AWARD:String = "BOSS_AWARD";
      
      public static const §_-3g§:String = "NO_PATH_TEXT";
      
      public static const BOSS_UNAVAILABLE_TITLE:String = "BOSS_UNAVAILABLE_TITLE";
      
      public static const BOSS_UNAVAILABLE_DESC:String = "BOSS_UNAVAILABLE_DESC";
      
      public static const BOSS_UNAVAILABLE_REQUIRED_LEVEL:String = "BOSS_UNAVAILABLE_REQUIRED_LEVEL";
      
      public static const BOSS_UNAVAILABLE_REQUIRED_LEVEL_TEXT:String = "BOSS_UNAVAILABLE_REQUIRED_LEVEL_TEXT";
      
      public static const DESCR_REWARD_BOSS_INFO_FIRST_COMPLETE:String = "DESCR_REWARD_BOSS_INFO_FIRST_COMPLETE";
      
      public static const DESCR_REWARD_BOSS_INFO:String = "DESCR_REWARD_BOSS_INFO";
      
      public static const BATTLE_BOSS_BTN:String = "BATTLE_BOSS_BTN";
      
      public static const ONE_DAY_PANEL_NAME:String = "ONE_DAY_PANEL_NAME";
      
      public static const ONE_DAY_PANEL_DESCR:String = "ONE_DAY_PANEL_DESCR";
      
      public static const ONE_DAY_PANEL_RETURN_TOMORROW:String = "ONE_DAY_PANEL_RETURN_TOMORROW";
      
      public static const HELP_LEARNING_MISSION_ITEM_MENU:String = "HELP_LEARNING_MISSION_ITEM_MENU";
      
      public static const LEVEL_NOT_APPROPRIATE_PANEL_NAME:String = "LEVEL_NOT_APPROPRIATE_PANEL_NAME";
      
      public static const LEVEL_NOT_APPROPRIATE_PANEL_DESCR:String = "LEVEL_NOT_APPROPRIATE_PANEL_DESCR";
      
      public static const BOSS_GAMEOVER_LOSE_TITLE_2:String = "BOSS_GAMEOVER_LOSE_TITLE_2";
      
      public static const BOSS_GAMEOVER_LOSE_DESCR_1:String = "BOSS_GAMEOVER_LOSE_DESCR_1";
      
      public static const BOSS_GAMEOVER_LOSE_DESCR_2:String = "BOSS_GAMEOVER_LOSE_DESCR_2";
      
      public static const BOSS_GAMEOVER_DRAW_TITLE_2:String = "BOSS_GAMEOVER_DRAW_TITLE_2";
      
      public static const BOSS_GAMEOVER_DRAW_DESCR:String = "BOSS_GAMEOVER_DRAW_DESCR";
      
      public static const NO_PATH_TEXT:String = "NO_PATH_TEXT";
      
      public static const BONUS_ATTACK:String = "BONUS_ATTACK";
      
      public static const BONUS_ARMOR:String = "BONUS_ARMOR";
      
      public static const TITLE_CONFIRMATION:String = "TITLE_CONFIRMATION";
      
      public static const CONFIRMATION_DOWNGRADE_TEXT:String = "CONFIRMATION_DOWNGRADE_TEXT";
      
      public static const UNLOCK_BOSS_RESULT_TITLE:String = "UNLOCK_BOSS_RESULT_TITLE";
      
      public static const UNLOCK_BOSS_RESULT_SUCCESS:String = "UNLOCK_BOSS_RESULT_SUCCESS";
      
      public static const UNLOCK_BOSS_RESULT_ERROR:String = "UNLOCK_BOSS_RESULT_ERROR";
      
      public static const UNLOCK_BOSS_TEXT:String = "UNLOCK_BOSS_TEXT";
      
      public static const UNLOCK_BOSS_PANEL_TEXT:String = "UNLOCK_BOSS_PANEL_TEXT";
      
      public static const BACK_BTN:String = "BACK_BTN";
      
      public static const CRAFT_REWARD_TITLE:String = "CRAFT_REWARD_TITLE";
      
      public static const COLLECT_FOR_REAGENT_BTN:String = "COLLECT_FOR_REAGENT_BTN";
      
      public static const CRAFT_EQUIP_NOT_ENOUGH_REGS:String = "CRAFT_EQUIP_NOT_ENOUGH_REGS";
      
      public static const BEST_EQUIP_ALREADY_EXIST:String = "BEST_EQUIP_ALREADY_EXIST";
      
      public static const TRY_YOUR_LUCK:String = "TRY_YOUR_LUCK";
      
      public static const GREAT_LUCK:String = "GREAT_LUCK";
      
      public static const CRAFT_VARIATION_HINT:String = "CRAFT_VARIATION_HINT";
      
      public static const RECRAFT_VARIATION_HINT:String = "RECRAFT_VARIATION_HINT";
      
      public static const COLLECT_FOR_RUBY_BTN:String = "COLLECT_FOR_RUBY_BTN";
      
      public static const DOWNGRADE_TITLE:String = "DOWNGRADE_TITLE";
      
      public static const DOWNGRADE_BTN2:String = "DOWNGRADE_BTN2";
      
      public static const CRAFT_BTN:String = "CRAFT_BTN";
      
      public static const REBUILD_BTN:String = "REBUILD_BTN";
      
      public static const REAGENT_DEMAND:String = "REAGENT_DEMAND";
      
      public static const GRADE_S_DEMAND:String = "GRADE_S_DEMAND";
      
      public static const PRONOUN_I:String = "PRONOUN_I";
      
      public static const SUCCESS_SEND:String = "SUCCESS_SEND";
      
      public static const BOSS_FARMER_NAME:String = "BOSS_FARMER_NAME";
      
      public static const BOSS_FARMER_DESCR:String = "BOSS_FARMER_DESCR";
      
      public static const BOSS_HUNTER_NAME:String = "BOSS_HUNTER_NAME";
      
      public static const BOSS_HUNTER_DESCR:String = "BOSS_HUNTER_DESCR";
      
      public static const BOSS_MANIACS_NAME:String = "BOSS_MANIACS_NAME";
      
      public static const BOSS_MANIACS_DESCR:String = "BOSS_MANIACS_DESCR";
      
      public static const BOSS_ENGINEER_NAME:String = "BOSS_ENGINEER_NAME";
      
      public static const BOSS_ENGINEER_DESCR:String = "BOSS_ENGINEER_DESCR";
      
      public static const BOSS_GUARDIANS_DEPTHS_NAME:String = "BOSS_GUARDIANS_DEPTHS_NAME";
      
      public static const BOSS_GUARDIANS_DEPTHS_DESCR:String = "BOSS_GUARDIANS_DEPTHS_DESCR";
      
      public static const BOSS_DARK_KNIGHT_NAME:String = "BOSS_DARK_KNIGHT_NAME";
      
      public static const BOSS_DARK_KNIGHT_DESCR:String = "BOSS_DARK_KNIGHT_DESCR";
      
      public static const BOSS_DARK_KNIGHT_TEAM_ACOLYTES:String = "BOSS_DARK_KNIGHT_TEAM_ACOLYTES";
      
      public static const BOSS_DARK_KNIGHT_ACOLYTE:String = "BOSS_DARK_KNIGHT_ACOLYTE";
      
      public static const BOSS_ASSASSIN_NAME:String = "BOSS_ASSASSIN_NAME";
      
      public static const BOSS_ASSASSIN_DESCR:String = "BOSS_ASSASSIN_DESCR";
      
      public static const BOSS_ALCHEMIST_NAME:String = "BOSS_ALCHEMIST_NAME";
      
      public static const BOSS_ALCHEMIST_DESCR:String = "BOSS_ALCHEMIST_DESCR";
      
      public static const BOSS_ALCHEMIST_ASISSTANT_TEAM:String = "BOSS_ALCHEMIST_ASISSTANT_TEAM";
      
      public static const BOSS_ALCHEMIST_ASISSTANT:String = "BOSS_ALCHEMIST_ASISSTANT";
      
      public static const BOSS_ARCHBOT_NAME:String = "BOSS_ARCHBOT_NAME";
      
      public static const BOSS_ARCHBOT_DESCR:String = "BOSS_ARCHBOT_DESCR";
      
      public static const BOSS_ARCHBOT_ASISSTANT_TEAM:String = "BOSS_ARCHBOT_ASISSTANT_TEAM";
      
      public static const BOSS_ARCHBOT_GOLEM:String = "BOSS_ARCHBOT_GOLEM";
      
      public static const BOSS_EMPEROR_NAME:String = "BOSS_EMPEROR_NAME";
      
      public static const BOSS_EMPEROR_DESCR:String = "BOSS_EMPEROR_DESCR";
      
      public static const BOSS_EMPEROR_BOT_NAME_1:String = "BOSS_EMPEROR_BOT_NAME_1";
      
      public static const BOSS_EMPEROR_ARCHER:String = "BOSS_EMPEROR_ARCHER";
      
      public static const BOSS_EMPEROR_SPEARMAN:String = "BOSS_EMPEROR_SPEARMAN";
      
      public static const BOSS_EMPEROR_GENERAL:String = "BOSS_EMPEROR_GENERAL";
      
      public static const BOSS_EMPEROR_DEFENDER:String = "BOSS_EMPEROR_DEFENDER";
      
      public static const BOSS_SCIENTIST_NAME:String = "BOSS_SCIENTIST_NAME";
      
      public static const BOSS_SCIENTIST_DESCR:String = "BOSS_SCIENTIST_DESCR";
      
      public static const BOSS_SCIENTIST_BOT_NAME_1:String = "BOSS_SCIENTIST_BOT_NAME_1";
      
      public static const BOSS_SYMBIOTE_NAME:String = "BOSS_SYMBIOTE_NAME";
      
      public static const BOSS_SYMBIOTE_DESCR:String = "BOSS_SYMBIOTE_DESCR";
      
      public static const BOSS_SYMBIOTE_BOT_NAME_1:String = "BOSS_SYMBIOTE_BOT_NAME_1";
      
      public static const BOSS_SYMBIOTE_COCOON:String = "BOSS_SYMBIOTE_COCOON";
      
      public static const BOSS_PALADIN_NAME:String = "BOSS_PALADIN_NAME";
      
      public static const BOSS_PALADIN_DESCR:String = "BOSS_PALADIN_DESCR";
      
      public static const BOSS_HACKER_NAME:String = "BOSS_HACKER_NAME";
      
      public static const BOSS_HACKER_DESCR:String = "BOSS_HACKER_DESCR";
      
      public static const BOSS_HACKER_TEAM_PUPILS:String = "BOSS_HACKER_TEAM_PUPILS";
      
      public static const BOSS_HACKER_PUPILS:String = "BOSS_HACKER_PUPILS";
      
      public static const BOSS_THIEVES_NAME:String = "BOSS_THIEVES_NAME";
      
      public static const BOSS_THIEVES_DESCR:String = "BOSS_THIEVES_DESCR";
      
      public static const BOSS_THIEF_BOT_1_NAME:String = "BOSS_THIEF_BOT_1_NAME";
      
      public static const BOSS_THIEF_BOT_2_NAME:String = "BOSS_THIEF_BOT_2_NAME";
      
      public static const BOSS_PSIONICS_NAME:String = "BOSS_PSIONICS_NAME";
      
      public static const BOSS_PSIONICS_DESCR:String = "BOSS_PSIONICS_DESCR";
      
      public static const BOSS_PSIONICS_BOT_NAME_1:String = "BOSS_PSIONICS_BOT_NAME_1";
      
      public static const BOSS_PSIONICS_TEAM_BOT_2:String = "BOSS_PSIONICS_TEAM_BOT_2";
      
      public static const BOSS_PSIONICS_BOT_NAME_2:String = "BOSS_PSIONICS_BOT_NAME_2";
      
      public static const BOSS_PHANTOMS_NAME:String = "BOSS_PHANTOMS_NAME";
      
      public static const BOSS_PHANTOMS_DESCR:String = "BOSS_PHANTOMS_DESCR";
      
      public static const BOSS_PHANTOMS_BOT_NAME:String = "BOSS_PHANTOMS_BOT_NAME";
      
      public static const BOSS_ARCHDEMON_NAME:String = "BOSS_ARCHDEMON_NAME";
      
      public static const BOSS_ARCHDEMON_DESCR:String = "BOSS_ARCHDEMON_DESCR";
      
      public static const MOVE_TEAMMATE_HINT:String = "MOVE_TEAMMATE_HINT";
      
      public static const CRAFT_CRATE:String = "CRAFT_CRATE";
      
      public static const CRAFT_TITLE:String = "CRAFT_TITLE";
      
      public static const CRAFT_COLLECTED_INNER_TITLE:String = "CRAFT_COLLECTED_INNER_TITLE";
      
      public static const REAGENTS_QTIP:String = "REAGENTS_QTIP";
      
      public static const CURRENT_USER_ALREADY_IN_BATTLE:String = "CURRENT_USER_ALREADY_IN_BATTLE";
      
      public static const WAITING_FOR_OTHER_PLAYERS:String = "WAITING_FOR_OTHER_PLAYERS";
      
      public static const BATTLE_2X2_OFFER_TITLE:String = "BATTLE_2X2_OFFER_TITLE";
      
      public static const PVP_SERVER_DOWN:String = "PVP_SERVER_DOWN";
      
      public static const PVP_DUEL_CARD:String = "PVP_DUEL_CARD";
      
      public static const PVP_SURVIVAL_CARD:String = "PVP_SURVIVAL_CARD";
      
      public static const PVP_DEATH_MATCH_CARD:String = "PVP_DEATH_MATCH_CARD";
      
      public static const PVP_TWO_VS_TWO_CARD:String = "PVP_TWO_VS_TWO_CARD";
      
      public static const PVP_TWO_VS_TWO_WITH_FRIEND_CARD:String = "PVP_TWO_VS_TWO_WITH_FRIEND_CARD";
      
      public static const PVP_TWO_VS_TWO_WITH_FRIEND_CARD_DESC:String = "PVP_TWO_VS_TWO_WITH_FRIEND_CARD_DESC";
      
      public static const PVP_GLADIATOR_CARD:String = "PVP_GLADIATOR_CARD";
      
      public static const RESET_AWARDS_MSG:String = "RESET_AWARDS_MSG";
      
      public static const RESET_FOR:String = "RESET_FOR";
      
      public static const SETTINGS_TITLE:String = "SETTINGS_TITLE";
      
      public static const LANGUAGE_TITLE:String = "LANGUAGE_TITLE";
      
      public static const RUSSIAN_TITLE:String = "RUSSIAN_TITLE";
      
      public static const ENGLISH_TITLE:String = "ENGLISH_TITLE";
      
      public static const CHINESE_TITLE:String = "CHINESE_TITLE";
      
      public static const INFO_AND_SUPPORT:String = "INFO_AND_SUPPORT";
      
      public static const REPORT_A_PROBLEM:String = "REPORT_A_PROBLEM";
      
      public static const CONF_POLICY:String = "CONF_POLICY";
      
      public static const TERMS_OF_USE:String = "TERMS_OF_USE";
      
      public static const SCALE_TEXT:String = "SCALE_TEXT";
      
      public static const GAMEPAD_SETUP_TITLE:String = "GAMEPAD_SETUP_TITLE";
      
      public static const GAMEPAD_SETUP_BEFORE_CONTROL_NAME:String = "GAMEPAD_SETUP_BEFORE_CONTROL_NAME";
      
      public static const GAMEPAD_SETUP_AFTER_CONTROL_NAME:String = "GAMEPAD_SETUP_AFTER_CONTROL_NAME";
      
      public static const CONTROL_LEFT:String = "CONTROL_LEFT";
      
      public static const CONTROL_RIGHT:String = "CONTROL_RIGHT";
      
      public static const CONTROL_UP:String = "CONTROL_UP";
      
      public static const CONTROL_DOWN:String = "CONTROL_DOWN";
      
      public static const CONTROL_BAG:String = "CONTROL_BAG";
      
      public static const CONTROL_SHOOT:String = "CONTROL_SHOOT";
      
      public static const CONTROL_ALT:String = "CONTROL_ALT";
      
      public static const CONTROL_JUMP:String = "CONTROL_JUMP";
      
      public static const CONTROL_BACK_JUMP:String = "CONTROL_BACK_JUMP";
      
      public static const CONTROL_ESCAPE:String = "CONTROL_ESCAPE";
      
      public static const CHANGE_NAME_NOTE:String = "CHANGE_NAME_NOTE";
      
      public static const CHANGE_NAME_TITLE:String = "CHANGE_NAME_TITLE";
      
      public static const DEFAULT_NAME_TEXT:String = "DEFAULT_NAME_TEXT";
      
      public static const REPORT_BODY_TEMPLATE:String = "REPORT_BODY_TEMPLATE";
      
      public static const NEED_TO_SETUP_EMAIL:String = "NEED_TO_SETUP_EMAIL";
      
      public static const DO_YOU_LIKE_THIS_GAME:String = "DO_YOU_LIKE_THIS_GAME";
      
      public static const PLEASE_LEAVE_FEEDBACK:String = "PLEASE_LEAVE_FEEDBACK";
      
      public static const RATE_IT:String = "RATE_IT";
      
      public static const REMIND_ME_LATER:String = "REMIND_ME_LATER";
      
      public static const NO_THANKS:String = "NO_THANKS";
      
      public static const EQUIP_EXPIRED:String = "EQUIP_EXPIRED";
      
      public static const HAT_DISAPPEAR:String = "HAT_DISAPPEAR";
      
      public static const DAY_TEXT:String = "DAY_TEXT";
      
      public static const HOURS_TEXT:String = "HOURS_TEXT";
      
      public static const MINUTE_TEXT:String = "MINUTE_TEXT";
      
      public static const TEAM_UP:String = "TEAM_UP";
      
      public static const §_-Rl§:String = "HAT.SPEED";
      
      public static const §_-110§:String = "HAT.SPEED_ALL";
      
      public static const §_-iY§:String = "HAT.MIGHTY_JUMP";
      
      public static const §_-Q2t§:String = "HAT.DOUBLE_JUMP";
      
      public static const §_-13h§:String = "HAT.WATER_BREATH";
      
      public static const §_-zj§:String = "HAT.PROTECTION_SLOWING";
      
      public static const §_-N1k§:String = "HAT.ANTIPOISON";
      
      public static const §_-Yo§:String = "HAT.FROST_PROTECTION";
      
      public static const §_-Q1m§:String = "HAT.FROST_DAMAGE";
      
      public static const §_-A1T§:String = "HAT.FORTITUDE";
      
      public static const §_-VA§:String = "HAT.EXPLOSION_DEATH";
      
      public static const §_-K38§:String = "HAT.PENETRATION";
      
      public static const §_-J2H§:String = "HAT.WATER_PROTECTION";
      
      public static const §_-Js§:String = "HAT.IMMOBILITY_REDUCTION";
      
      public static const §_-N1G§:String = "HAT.IMMOBILITY_REDUCTION_SELF";
      
      public static const §_-T25§:String = "BONUS.LEGENDARY_ITEM_FIRST_ACT";
      
      public static const §_-w1R§:String = "BONUS.EPIC_ITEM_FIRST_ACT";
      
      public static const ICLOUD_ENABLE_NEW_ACCOUNT:String = "ICLOUD_ENABLE_NEW_ACCOUNT";
      
      public static const ICLOUD_ENABLE_WITH_CREATE_NEW_ACCOUNT:String = "ICLOUD_ENABLE_WITH_CREATE_NEW_ACCOUNT";
      
      public static const ICLOUD_ENABLE_ALREADY_HAS_ACCOUNT:String = "ICLOUD_ENABLE_ALREADY_HAS_ACCOUNT";
      
      public static const ICLOUD_DISABLE_CONTINUE_WITH_SAME_ACCOUNT:String = "ICLOUD_DISABLE_CONTINUE_WITH_SAME_ACCOUNT";
      
      public static const ICLOUD_DISABLE_CONTINUE_WITH_OTHER_ACCOUNT:String = "ICLOUD_DISABLE_CONTINUE_WITH_OTHER_ACCOUNT";
      
      public static const ICLOUD_TRY_ANOTHER_ACCOUNT:String = "ICLOUD_TRY_ANOTHER_ACCOUNT";
      
      public static const GOOGLE_LINK_NEED:String = "GOOGLE_LINK_NEED";
      
      public static const GOOGLE_LINK_LATER_IN_SETTINGS:String = "GOOGLE_LINK_LATER_IN_SETTINGS";
      
      public static const GOOGLE_LINK_NEW_ACCOUNT:String = "GOOGLE_LINK_NEW_ACCOUNT";
      
      public static const GOOGLE_LINK_HAS_OTHER_ACCOUNT:String = "GOOGLE_LINK_HAS_OTHER_ACCOUNT";
      
      public static const GOOGLE_LINK_ALREADY_COMPLETED:String = "GOOGLE_LINK_ALREADY_COMPLETED";
      
      public static const HEROIC_BOSSES_DIFFICULTY_EASY:String = "HEROIC_BOSSES_DIFFICULTY_EASY";
      
      public static const HEROIC_BOSSES_DIFFICULTY_MEDIUM:String = "HEROIC_BOSSES_DIFFICULTY_MEDIUM";
      
      public static const HEROIC_BOSSES_DIFFICULTY_HARD:String = "HEROIC_BOSSES_DIFFICULTY_HARD";
      
      public static const HEROIC_BOSSES_DIFFICULTY_INSANE:String = "HEROIC_BOSSES_DIFFICULTY_INSANE";
      
      public static const SUPER_BOSS_BATTLE_OFFER_TITLE:String = "SUPER_BOSS_BATTLE_OFFER_TITLE";
      
      public static const BOSS_BATTLE_OFFER_TITLE:String = "BOSS_BATTLE_OFFER_TITLE";
      
      public static const NUCLEAR_LAUNCH_DETECTED:String = "NUCLEAR_LAUNCH_DETECTED";
      
      public static const WATER_INCREASING:String = "WATER_INCREASING";
      
      public static const WATER_START_INCREASING:String = "WATER_START_INCREASING";
      
      public static const DEPOSIT_CONSUMPTION_ERROR:String = "DEPOSIT_CONSUMPTION_ERROR";
      
      public static const DEPOSIT_MAX_COUNT_LIMIT:String = "DEPOSIT_MAX_COUNT_LIMIT";
      
      public static const SOCIAL_NONE_STATUS:String = "SOCIAL_NONE_STATUS";
      
      public static const SOCIAL_STEAM_NAME:String = "SOCIAL_STEAM_NAME";
      
      public static const SOCIAL_NONE_ACTION:String = "SOCIAL_NONE_ACTION";
      
      public static const SOCIAL_FB_NAME:String = "SOCIAL_FB_NAME";
      
      public static const SOCIAL_VK_NAME:String = "SOCIAL_VK_NAME";
      
      public static const SOCIAL_FB_SHORT_NAME:String = "SOCIAL_FB_SHORT_NAME";
      
      public static const SOCIAL_VK_SHORT_NAME:String = "SOCIAL_VK_SHORT_NAME";
      
      public static const VK_GAMES:String = "VK_GAMES";
      
      public static const SOCIAL_NETWORKS_NOT_AVAILABLE:String = "SOCIAL_NETWORKS_NOT_AVAILABLE";
      
      public static const SELECT_SOCIAL_NETWORK:String = "SELECT_SOCIAL_NETWORK";
      
      public static const FACEBOOK_CONNECT_WINDOW_TITLE:String = "FACEBOOK_CONNECT_WINDOW_TITLE";
      
      public static const FACEBOOK_CONNECT_WINDOW_DESCRIPTION:String = "FACEBOOK_CONNECT_WINDOW_DESCRIPTION";
      
      public static const FACEBOOK_CONNECTED_BONUS_TEXT:String = "FACEBOOK_CONNECTED_BONUS_TEXT";
      
      public static const FACEBOOK_CONNECTED_BONUS_FAIL_TEXT:String = "FACEBOOK_CONNECTED_BONUS_FAIL_TEXT";
      
      public static const NEED_TO_CONNECT_TEXT_SOCIAL_NETWORK:String = "NEED_TO_CONNECT_TEXT_SOCIAL_NETWORK";
      
      public static const FACEBOOK_PVP_NEED_TO_CONNECT_TEXT:String = "FACEBOOK_PVP_NEED_TO_CONNECT_TEXT";
      
      public static const FACEBOOK_DISCONNECT_WARNING:String = "FACEBOOK_DISCONNECT_WARNING";
      
      public static const SELECT_FRIEND_TO_INVITE:String = "SELECT_FRIEND_TO_INVITE";
      
      public static const INPUT_NAME:String = "INPUT_NAME";
      
      public static const INPUT_MESSAGE:String = "INPUT_MESSAGE";
      
      public static const GLOBAL_CHAT_NOT_ALLOWED:String = "GLOBAL_CHAT_NOT_ALLOWED";
      
      public static const GLOBAL_CHAT_ILLEGAL_STATE:String = "GLOBAL_CHAT_ILLEGAL_STATE";
      
      public static const GLOBAL_CHAT_TOO_FAST:String = "GLOBAL_CHAT_TOO_FAST";
      
      public static const GLOBAL_CHAT_TAB:String = "GLOBAL_CHAT_TAB";
      
      public static const FRIENDS_TAB:String = "FRIENDS_TAB";
      
      public static const CHAT_RESTRICTED:String = "CHAT_RESTRICTED";
      
      public static const RESTRICT_FOR:String = "RESTRICT_FOR";
      
      public static const MEMBER_CRAFT_LEGENDARY_MESSAGE:String = "MEMBER_CRAFT_LEGENDARY_MESSAGE";
      
      public static const CONTEXT_MENU_DUEL:String = "CONTEXT_MENU_DUEL";
      
      public static const CONTEXT_MENU_TEAM_BATTLE:String = "CONTEXT_MENU_TEAM_BATTLE";
      
      public static const CONTEXT_MENU_HEROIC_BOSS:String = "CONTEXT_MENU_HEROIC_BOSS";
      
      public static const CONTEXT_MENU_COOP_BOSS:String = "CONTEXT_MENU_COOP_BOSS";
      
      public static const CONTEXT_MENU_HOTSEAT:String = "CONTEXT_MENU_HOTSEAT";
      
      public static const CONTEXT_MENU_VISIT:String = "CONTEXT_MENU_VISIT";
      
      public static const CONTEXT_MENU_VISIT_PAGE:String = "CONTEXT_MENU_VISIT_PAGE";
      
      public static const CONTEXT_MENU_TOGGLE_STICKERS:String = "CONTEXT_MENU_TOGGLE_STICKERS";
      
      public static const CONTEXT_MENU_SHOW_CLAN_INFO:String = "CONTEXT_MENU_SHOW_CLAN_INFO";
      
      public static const CONTEXT_MENU_SHOW_CLAN_INFO_ANOTHER_PLATFORM:String = "CONTEXT_MENU_SHOW_CLAN_INFO_ANOTHER_PLATFORM";
      
      public static const CONTEXT_MENU_INCREASE_RANK:String = "CONTEXT_MENU_INCREASE_RANK";
      
      public static const CONTEXT_MENU_REDUCE_RANK:String = "CONTEXT_MENU_REDUCE_RANK";
      
      public static const CONTEXT_MENU_EXPEL_PERMISSION_ON:String = "CONTEXT_MENU_EXPEL_PERMISSION_ON";
      
      public static const CONTEXT_MENU_EXPEL_PERMISSION_OFF:String = "CONTEXT_MENU_EXPEL_PERMISSION_OFF";
      
      public static const CONTEXT_MENU_MUTE_MODE_ON:String = "CONTEXT_MENU_MUTE_MODE_ON";
      
      public static const CONTEXT_MENU_MUTE_MODE_OFF:String = "CONTEXT_MENU_MUTE_MODE_OFF";
      
      public static const CONTEXT_MENU_FIRE_FROM_CLAN:String = "CONTEXT_MENU_FIRE_FROM_CLAN";
      
      public static const CONTEXT_MENU_MAKE_LEADER:String = "CONTEXT_MENU_MAKE_LEADER";
      
      public static const CONTEXT_MENU_INVITE_TO_CLAN:String = "CONTEXT_MENU_INVITE_TO_CLAN";
      
      public static const CONTEXT_MENU_INVITE_TO_CLAN_ANOTHER_PLATFORM:String = "CONTEXT_MENU_INVITE_TO_CLAN_ANOTHER_PLATFORM";
      
      public static const PUSHING_MINE_UNIT_SETUP:String = "PUSHING_MINE_UNIT_SETUP";
      
      public static const PUSHING_MINE_GROUND_SETUP:String = "PUSHING_MINE_GROUND_SETUP";
      
      public static const PUSHING_MINE_USELESS:String = "PUSHING_MINE_USELESS";
      
      public static const NEW_STICKERS_PACK_PURCHASED:String = "NEW_STICKERS_PACK_PURCHASED";
      
      public static const CALL_REINFORCEMENTS_USELESS:String = "CALL_REINFORCEMENTS_USELESS";
      
      public static const DAY_PLURALS:String = "DAY_PLURALS";
      
      public static const RUBY_PLURALS:String = "RUBY_PLURALS";
      
      public static const FUZY_PLURALS:String = "FUZY_PLURALS";
      
      public static const MEDAL_PLURALS:String = "MEDAL_PLURALS";
      
      public static const CLAN_CHAT_TAB:String = "CLAN_CHAT_TAB";
      
      public static const TAB_TREASURY_CLAN:String = "TAB_TREASURY_CLAN";
      
      public static const TAB_SEARCH_CLAN:String = "TAB_SEARCH_CLAN";
      
      public static const TAB_CREATE_CLAN:String = "TAB_CREATE_CLAN";
      
      public static const CLAN_EDIT_TAB:String = "CLAN_EDIT_TAB";
      
      public static const CLAN_INFO_TAB:String = "CLAN_INFO_TAB";
      
      public static const TAB_CLAN_ACTIVITY:String = "TAB_CLAN_ACTIVITY";
      
      public static const NOT_IN_CLAN:String = "NOT_IN_CLAN";
      
      public static const CLAN_CHAT_VELCOME:String = "CLAN_CHAT_VELCOME";
      
      public static const CLAN_CHAT_OFFER_CREATE:String = "CLAN_CHAT_OFFER_CREATE";
      
      public static const CREATE_CLAN_BTN:String = "CREATE_CLAN_BTN";
      
      public static const CLAN_NAME:String = "CLAN_NAME";
      
      public static const CLAN_EMBLEM:String = "CLAN_EMBLEM";
      
      public static const CLAN_DESCRIPTION:String = "CLAN_DESCRIPTION";
      
      public static const JOIN_TO_CLAN_LABEL:String = "JOIN_TO_CLAN_LABEL";
      
      public static const OPEN_CLAN:String = "OPEN_CLAN";
      
      public static const CLOSE_CLAN:String = "CLOSE_CLAN";
      
      public static const CLAN_EXIT_OPENED:String = "CLAN_EXIT_OPENED";
      
      public static const CLAN_EXIT_CLOSED:String = "CLAN_EXIT_CLOSED";
      
      public static const JOIN_MINIMAL_RATING:String = "JOIN_MINIMAL_RATING";
      
      public static const LOGO_LABEL_TEXT:String = "LOGO_LABEL_TEXT";
      
      public static const LOGO_COLOR_LABEL_TEXT:String = "LOGO_COLOR_LABEL_TEXT";
      
      public static const PATTERN_LABEL_TEXT:String = "PATTERN_LABEL_TEXT";
      
      public static const PATTERN_COLOR_LABEL_TEXT:String = "PATTERN_COLOR_LABEL_TEXT";
      
      public static const ERROR_MIN_CHAR_CLAN_NAME:String = "ERROR_MIN_CHAR_CLAN_NAME";
      
      public static const CREATE_CLAN_CONFIRMATION:String = "CREATE_CLAN_CONFIRMATION";
      
      public static const CHANGE_CLAN_NAME_CONFIRMATION:String = "CHANGE_CLAN_NAME_CONFIRMATION";
      
      public static const CHANGE_CLAN_EMBLEM_CONFIRMATION:String = "CHANGE_CLAN_EMBLEM_CONFIRMATION";
      
      public static const CHANGE_CLAN_DESCRIPTION_CONFIRMATION:String = "CHANGE_CLAN_DESCRIPTION_CONFIRMATION";
      
      public static const EXPAND_CLAN_CONFIRMATION:String = "EXPAND_CLAN_CONFIRMATION";
      
      public static const CREATE_CLAN_LEVEL_REQ_GREEN:String = "CREATE_CLAN_LEVEL_REQ_GREEN";
      
      public static const CREATE_CLAN_LEVEL_REQ_RED:String = "CREATE_CLAN_LEVEL_REQ_RED";
      
      public static const CREATE_CLAN_RATING_REQ_GREEN:String = "CREATE_CLAN_RATING_REQ_GREEN";
      
      public static const CREATE_CLAN_RATING_REQ_RED:String = "CREATE_CLAN_RATING_REQ_RED";
      
      public static const CANT_CREATE_CLAN_DESC:String = "CANT_CREATE_CLAN_DESC";
      
      public static const ERR_CLAN_NAME_EXISTS:String = "ERR_CLAN_NAME_EXISTS";
      
      public static const ERR_INVALID_ARGUMENT:String = "ERR_INVALID_ARGUMENT";
      
      public static const CLAN_LOCKED_MESSAGE:String = "CLAN_LOCKED_MESSAGE";
      
      public static const ERR_ALREADY_IN_CLAN:String = "ERR_ALREADY_IN_CLAN";
      
      public static const §_-V1p§:String = "ERR_ALREADY_IN_CLAN";
      
      public static const ERR_REQUIREMENTS_FAILURE:String = "ERR_REQUIREMENTS_FAILURE";
      
      public static const PLAYER_INVITE_YOU_IN_CLAN:String = "PLAYER_INVITE_YOU_IN_CLAN";
      
      public static const NO_RESPONSE_FROM_SERVER_CLAN:String = "NO_RESPONSE_FROM_SERVER_CLAN";
      
      public static const ERR_REPEATED_INVITE:String = "ERR_REPEATED_INVITE";
      
      public static const ERR_PROFILE_INVITE_LIMIT:String = "ERR_PROFILE_INVITE_LIMIT";
      
      public static const ERR_CLAN_INVITE_LIMIT:String = "ERR_CLAN_INVITE_LIMIT";
      
      public static const ERR_INTEROP_ON_INVITE:String = "ERR_INTEROP_ON_INVITE";
      
      public static const USER_EXPELLED_COMPENSATION:String = "USER_EXPELLED_COMPENSATION";
      
      public static const CLAN_NOT_FOUND:String = "CLAN_NOT_FOUND";
      
      public static const ERR_ACCESS_DENIED_FOR_CLAN_CHAT:String = "ERR_ACCESS_DENIED_FOR_CLAN_CHAT";
      
      public static const CLAN_RATING_POINTS:String = "CLAN_RATING_POINTS";
      
      public static const CLAN_COMPOSITION:String = "CLAN_COMPOSITION";
      
      public static const PREV_SEASON_LABEL:String = "PREV_SEASON_LABEL";
      
      public static const PREV_SEASON_PLACE:String = "PREV_SEASON_PLACE";
      
      public static const MEDAL_COST_LABEL:String = "MEDAL_COST_LABEL";
      
      public static const CLAN_MEDAL_COST_CONFIRM_WARNING:String = "CLAN_MEDAL_COST_CONFIRM_WARNING";
      
      public static const EXIT_FROM_CLAN_LABEL:String = "EXIT_FROM_CLAN_LABEL";
      
      public static const CHANGE_CLAN_NAME_FAILED:String = "CHANGE_CLAN_NAME_FAILED";
      
      public static const CHANGE_CLAN_DESCRIPTION_FAILED:String = "CHANGE_CLAN_DESCRIPTION_FAILED";
      
      public static const CHANGE_CLAN_EMBLEM_FAILED:String = "CHANGE_CLAN_EMBLEM_FAILED";
      
      public static const CLAN_OP_TITLE_EXPAND_CAPACITY:String = "CLAN_OP_TITLE_EXPAND_CAPACITY";
      
      public static const CLAN_OP_TITLE_CHANGE_DESCRIPTION:String = "CLAN_OP_TITLE_CHANGE_DESCRIPTION";
      
      public static const CLAN_OP_TITLE_CHANGE_EMBLEM:String = "CLAN_OP_TITLE_CHANGE_EMBLEM";
      
      public static const CLAN_OP_TITLE_RENAME:String = "CLAN_OP_TITLE_RENAME";
      
      public static const CLAN_OP_TITLE_DEFAULT:String = "CLAN_OP_TITLE_DEFAULT";
      
      public static const CLAN_OP_WALLET_SELECT:String = "CLAN_OP_WALLET_SELECT";
      
      public static const CLAN_OP_WALLET_SELF:String = "CLAN_OP_WALLET_SELF";
      
      public static const CLAN_OP_WALLET_TREASURY:String = "CLAN_OP_WALLET_TREASURY";
      
      public static const CLAN_MEDAL_COST_CONFIRM_COUNT:String = "CLAN_MEDAL_COST_CONFIRM_COUNT";
      
      public static const JOIN_RATING_NOT_CHANGED:String = "JOIN_RATING_NOT_CHANGED";
      
      public static const CLAN_CAPACITY_LABEL:String = "CLAN_CAPACITY_LABEL";
      
      public static const OFFICERS_POSITIONS:String = "OFFICERS_POSITIONS";
      
      public static const QUIT_CLAN_CONFIRMATION:String = "QUIT_CLAN_CONFIRMATION";
      
      public static const ERR_LEADER_QUIT_CLAN:String = "ERR_LEADER_QUIT_CLAN";
      
      public static const CONFIRMATION_EXIT_FROM_CLAN:String = "CONFIRMATION_EXIT_FROM_CLAN";
      
      public static const CHAT_FLOOD_WARNING:String = "CHAT_FLOOD_WARNING";
      
      public static const ERR_INVITE_ALREADY_PASSED:String = "ERR_INVITE_ALREADY_PASSED";
      
      public static const MAP_CAPACITY_FOR_JOIN_CLAN:String = "MAP_CAPACITY_FOR_JOIN_CLAN";
      
      public static const ERR_SOCIAL_ID:String = "ERR_SOCIAL_ID";
      
      public static const CLAN_CREATED:String = "CLAN_CREATED";
      
      public static const CLAN_DELETED:String = "CLAN_DELETED";
      
      public static const MEMBER_JOINED_CLAN:String = "MEMBER_JOINED_CLAN";
      
      public static const MEMBER_LEFT_CLAN:String = "MEMBER_LEFT_CLAN";
      
      public static const MEMBER_WAS_EXPELLED:String = "MEMBER_WAS_EXPELLED";
      
      public static const LEADER_PROMOTE_RANK_MEMBER:String = "LEADER_PROMOTE_RANK_MEMBER";
      
      public static const LEADER_LOWER_RANK_MEMBER:String = "LEADER_LOWER_RANK_MEMBER";
      
      public static const RANK_OFFICER_ACCUSITIVE:String = "RANK_OFFICER_ACCUSITIVE";
      
      public static const RANK_SOLDIER_ACCUSITIVE:String = "RANK_SOLDIER_ACCUSITIVE";
      
      public static const LEADER_SWITCHED_WITH_OFFICER:String = "LEADER_SWITCHED_WITH_OFFICER";
      
      public static const CLAN_HAS_NEW_NAME:String = "CLAN_HAS_NEW_NAME";
      
      public static const CLAN_HAS_NEW_DESCRIPTION:String = "CLAN_HAS_NEW_DESCRIPTION";
      
      public static const CLAN_HAS_NEW_CAPACITY:String = "CLAN_HAS_NEW_CAPACITY";
      
      public static const CLAN_HAS_NEW_EMBLEM:String = "CLAN_HAS_NEW_EMBLEM";
      
      public static const CLAN_EXIT_HAS_CLOSED:String = "CLAN_EXIT_HAS_CLOSED";
      
      public static const CLAN_EXIT_HAS_OPENED:String = "CLAN_EXIT_HAS_OPENED";
      
      public static const DONATE_CLAN_CONFIRMATION:String = "DONATE_CLAN_CONFIRMATION";
      
      public static const DONATE_CLAN_MEMBER:String = "DONATE_CLAN_MEMBER";
      
      public static const CHANGE_MEDAL_PRICE_CLAN:String = "CHANGE_MEDAL_PRICE_CLAN";
      
      public static const EXCHANGE_MEDALS_CLAN_MEMBER:String = "EXCHANGE_MEDALS_CLAN_MEMBER";
      
      public static const ERR_DONATE_REPEAT:String = "ERR_DONATE_REPEAT";
      
      public static const OFFICER_WAS_EXPEL_PERMISSION_ON:String = "OFFICER_WAS_EXPEL_PERMISSION_ON";
      
      public static const OFFICER_WAS_EXPEL_PERMISSION_OFF:String = "OFFICER_WAS_EXPEL_PERMISSION_OFF";
      
      public static const MEMBER_WAS_MUTED:String = "MEMBER_WAS_MUTED";
      
      public static const MEMBER_WAS_UNMUTED:String = "MEMBER_WAS_UNMUTED";
      
      public static const CLAN_CAPTION_POSITION:String = "CLAN_CAPTION_POSITION";
      
      public static const CLAN_CAPTION_PLAYER:String = "CLAN_CAPTION_PLAYER";
      
      public static const CLAN_CAPTION_PAY:String = "CLAN_CAPTION_PAY";
      
      public static const CLAN_CAPTION_ONLINE:String = "CLAN_CAPTION_ONLINE";
      
      public static const CLAN_CAPTION_RATE:String = "CLAN_CAPTION_RATE";
      
      public static const CLAN_NO_ACTIVITY_THIS_MONTH:String = "CLAN_NO_ACTIVITY_THIS_MONTH";
      
      public static const MEMBER_GLADIATOR_COMPLETE_MESSAGE:String = "MEMBER_GLADIATOR_COMPLETE_MESSAGE";
      
      public static const §_-hC§:String = "MEMBER_GLADIATOR_COMPLETE_MESSAGE";
      
      public static const MESSAGE_TO_CLAN:String = "MESSAGE_TO_CLAN";
      
      public static const JOIN_TO_CLAN_BTN:String = "JOIN_TO_CLAN_BTN";
      
      public static const INVITE_CONFIRMATION_TITLE:String = "INVITE_CONFIRMATION_TITLE";
      
      public static const FRIEND_INVITE_SENT:String = "FRIEND_INVITE_SENT";
      
      public static const ERR_CLAN_EXPEL_LIMIT_EXCEEDED:String = "ERR_CLAN_EXPEL_LIMIT_EXCEEDED";
      
      public static const JOIN_CLAN_CONFIRMATION_TITLE:String = "JOIN_CLAN_CONFIRMATION_TITLE";
      
      public static const INVITE_CONFIRMATION_TEXT:String = "INVITE_CONFIRMATION_TEXT";
      
      public static const JOIN_TO_EXIT_CLOSED_CLAN:String = "JOIN_TO_EXIT_CLOSED_CLAN";
      
      public static const JOIN_CLAN_CONFIRMATION_TEXT:String = "JOIN_CLAN_CONFIRMATION_TEXT";
      
      public static const CLAN_JOIN_DISABLE_BY_LEVEL:String = "CLAN_JOIN_DISABLE_BY_LEVEL";
      
      public static const RANK_LEADER:String = "RANK_LEADER";
      
      public static const RANK_OFFICER:String = "RANK_OFFICER";
      
      public static const RANK_SOLDIER:String = "RANK_SOLDIER";
      
      public static const ONLINE_HOUR:String = "ONLINE_HOUR";
      
      public static const ONLINE_DAYS:String = "ONLINE_DAYS";
      
      public static const ONLINE_MORE_MONTH_AGO:String = "ONLINE_MORE_MONTH_AGO";
      
      public static const ONLINE_INGAME:String = "ONLINE_INGAME";
      
      public static const ONLINE_TODAY:String = "ONLINE_TODAY";
      
      public static const CLAN_TREASURY_LABEL:String = "CLAN_TREASURY_LABEL";
      
      public static const CLAN_TREASURY_PAY_LABEL:String = "CLAN_TREASURY_PAY_LABEL";
      
      public static const USER_EXPELLED_BY_OFFICER_MSG:String = "USER_EXPELLED_BY_OFFICER_MSG";
      
      public static const USER_EXPELLED_FROM_CLAN_MSG:String = "USER_EXPELLED_FROM_CLAN_MSG";
      
      public static const OPEN_FOR_RATING:String = "OPEN_FOR_RATING";
      
      public static const CLAN_SEASON_DATES:String = "CLAN_SEASON_DATES";
      
      public static const CLAN_SEASON_AWARD:String = "CLAN_SEASON_AWARD";
      
      public static const CLAN_SEASON_BONUS_TEXT:String = "CLAN_SEASON_BONUS_TEXT";
      
      public static const CLAN_SEASON_BONUS_WITH_BANNER_TEXT:String = "CLAN_SEASON_BONUS_WITH_BANNER_TEXT";
      
      public static const SEASON_BONUS_TITLE:String = "SEASON_BONUS_TITLE";
      
      public static const SEASON_CLAN_PLACE:String = "SEASON_CLAN_PLACE";
      
      public static const CONTROL_TITLE:String = "CONTROL_TITLE";
      
      public static const CRAFT_BOX_BUY_RESULT:String = "CRAFT_BOX_BUY_RESULT";
      
      public static const RANDOM_RARE_REAGENT_NOTE:String = "RANDOM_RARE_REAGENT_NOTE";
      
      public static const ACTION_FOR_ALT_HINT:String = "ACTION_FOR_ALT_HINT";
      
      public static const WEAPON_IMPROVE_NEED:String = "WEAPON_IMPROVE_NEED";
      
      public static const WEAPON_MAX_IMPROVE_PRICE:String = "WEAPON_MAX_IMPROVE_PRICE";
      
      public static const WEAPON_AFTER_IMPROVE:String = "WEAPON_AFTER_IMPROVE";
      
      public static const WEAPONS_BONUS_TITLE:String = "WEAPONS_BONUS_TITLE";
      
      public static const WEAPONS_BONUS_TEXT:String = "WEAPONS_BONUS_TEXT";
      
      public static const DOWNGRADE_WEAPONS_COMPENSATION_TITLE:String = "DOWNGRADE_WEAPONS_COMPENSATION_TITLE";
      
      public static const DOWNGRADE_WEAPONS_COMPENSATION_TEXT:String = "DOWNGRADE_WEAPONS_COMPENSATION_TEXT";
      
      public static const STEAM_OVERLAY_ACTIVATION_NEEDED:String = "STEAM_OVERLAY_ACTIVATION_NEEDED";
      
      public static const PROMOTION_CONFIRMATION_LEADER:String = "PROMOTION_CONFIRMATION_LEADER";
      
      public static const PROMOTION_CONFIRMATION_OFFICER:String = "PROMOTION_CONFIRMATION_OFFICER";
      
      public static const PROMOTION_LEADER_FAILED:String = "PROMOTION_LEADER_FAILED";
      
      public static const PROMOTION_LEADER_FAILED_2:String = "PROMOTION_LEADER_FAILED_2";
      
      public static const PROMOTION_OFFICER_FAILED_LIMIT:String = "PROMOTION_OFFICER_FAILED_LIMIT";
      
      public static const PROMOTION_OFFICER_FAILED:String = "PROMOTION_OFFICER_FAILED";
      
      public static const CLAN_MATE_FIRE_RETURN_PAY:String = "CLAN_MATE_FIRE_RETURN_PAY";
      
      public static const CLAN_MATE_FIRE_MEDALS_COST:String = "CLAN_MATE_FIRE_MEDALS_COST";
      
      public static const TREASURY_NOT_ENOUGH_TITLE:String = "TREASURY_NOT_ENOUGH_TITLE";
      
      public static const NEED_FOR_COMPENSATION_MEMBER:String = "NEED_FOR_COMPENSATION_MEMBER";
      
      public static const ONLY_LEADER_CAN_EXPELL_MEMBER_WITH_COMPENSATION:String = "ONLY_LEADER_CAN_EXPELL_MEMBER_WITH_COMPENSATION";
      
      public static const CONFIRM_EXPELL_CLANMATE:String = "CONFIRM_EXPELL_CLANMATE";
      
      public static const SEARCH_CLAN_NAME_INPUT:String = "SEARCH_CLAN_NAME_INPUT";
      
      public static const MESSAGE_ON_GET_LEVEL_IN_ROOM:String = "MESSAGE_ON_GET_LEVEL_IN_ROOM";
      
      public static const TREASURY_INCOMING_PAYMENT_SUCCESS:String = "TREASURY_INCOMING_PAYMENT_SUCCESS";
      
      public static const TEAM_MEMBER_TYPE_MERC:String = "TEAM_MEMBER_TYPE_MERC";
      
      public static const TEAM_MEMBER_TYPE_FRIEND:String = "TEAM_MEMBER_TYPE_FRIEND";
      
      public static const TEAM_MEMBER_TYPE_CLANMATE:String = "TEAM_MEMBER_TYPE_CLANMATE";
      
      public static const TEAM_MEMBER_RESTRICTED_TEXT:String = "TEAM_MEMBER_RESTRICTED_TEXT";
      
      public static const PLAYER_NAME_TITLE:String = "PLAYER_NAME_TITLE";
      
      public static const TITLE_WELCOME:String = "TITLE_WELCOME";
      
      public static const ENTER_TUTORIAL_QUESTION:String = "ENTER_TUTORIAL_QUESTION";
      
      public static const NEED_GOOGLE_AUTH:String = "NEED_GOOGLE_AUTH";
      
      public static const STATUS_ALREADY_ON:String = "STATUS_ALREADY_ON";
      
      public static const UNAVAILABLE:String = "UNAVAILABLE";
      
      public static const LABEL_TURN_ON:String = "LABEL_TURN_ON";
      
      public static const LABEL_TURN_OFF:String = "LABEL_TURN_OFF";
      
      public static const LABEL_LINKED:String = "LABEL_LINKED";
      
      public static const GOOGLE_LINKED_TEXT:String = "GOOGLE_LINKED_TEXT";
      
      public static const HELP_ADMINISTRATION:String = "HELP_ADMINISTRATION";
      
      public static const NAVIGATE_SUPPORT_TELEGRAM:String = "NAVIGATE_SUPPORT_TELEGRAM";
      
      public static const NAVIGATE_SUPPORT_VKONTAKTE:String = "NAVIGATE_SUPPORT_VKONTAKTE";
      
      public static const TITLE_VERSION:String = "TITLE_VERSION";
      
      public static const TITLE_ACCOUNT_ID:String = "TITLE_ACCOUNT_ID";
      
      public static const ADMIN_CHAT_MESSAGE:String = "ADMIN_CHAT_MESSAGE";
      
      public static const OTHER_PLATFORM_CALLS_DISABLED:String = "OTHER_PLATFORM_CALLS_DISABLED";
      
      public static const OTHER_PLATFORM_INVITES_UNAVAILABLE:String = "OTHER_PLATFORM_INVITES_UNAVAILABLE";
      
      public static const OTHER_PLATFORM_CLAN_SHOW_UNAVAILABLE:String = "OTHER_PLATFORM_CLAN_SHOW_UNAVAILABLE";
      
      public static const BATTLE_TOKEN:String = "BATTLE_TOKEN";
      
      public static const BATTLE_TOKENS:String = "BATTLE_TOKENS";
      
      public static const BATTLE_TOKENS_TEXT:String = "BATTLE_TOKENS_TEXT";
      
      public static const WAGER_TOKEN:String = "WAGER_TOKEN";
      
      public static const WAGER_TOKENS:String = "WAGER_TOKENS";
      
      public static const WAGER_TOKENS_TEXT:String = "WAGER_TOKENS_TEXT";
      
      public static const BOSS_TOKEN:String = "BOSS_TOKEN";
      
      public static const BOSS_TOKENS:String = "BOSS_TOKENS";
      
      public static const BOSS_TOKENS_TEXT:String = "BOSS_TOKENS_TEXT";
      
      public static const ALL_TOKENS_EXPIRE:String = "ALL_TOKENS_EXPIRE";
      
      public static const ONE_TOKEN_RESTORE:String = "ONE_TOKEN_RESTORE";
      
      public static const FREE_BATTLE_TOKENS:String = "FREE_BATTLE_TOKENS";
      
      public static const WASTED:String = "WASTED";
      
      public static const LIGHT_VERSION:String = "LIGHT_VERSION";
      
      public static const RACE_BUY_CONFIRM_QUESTION:String = "RACE_BUY_CONFIRM_QUESTION";
      
      public function §_-s2a§()
      {
         super();
      }
      
      public static function §_-K3t§(param1:String) : String
      {
         return §_-52A§.§_-K3t§(param1) || param1;
      }
      
      public static function §_-y2o§(param1:String, param2:String = null) : String
      {
         var result:String = null;
         var key:String = param1;
         var defaultValue:String = param2;
         result = defaultValue != null ? defaultValue : key;
         try
         {
            result = §_-52A§.§_-K3t§(key);
         }
         catch(e:Error)
         {
         }
         finally
         {
            return result;
         }
      }
      
      public static function §_-Pj§(param1:String, param2:§_-p2U§, param3:String = null) : String
      {
         var result:String = null;
         var key:String = param1;
         var lang:§_-p2U§ = param2;
         var defaultValue:String = param3;
         result = defaultValue != null ? defaultValue : key;
         try
         {
            result = §_-52A§.§_-Ji§().§_-Pj§(key,lang);
         }
         catch(e:Error)
         {
         }
         finally
         {
            return result;
         }
      }
   }
}

