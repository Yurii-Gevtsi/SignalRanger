package ru.pragmatix.wormix.model.user
{
   import §_-51p§.§_-W2e§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-9§.§_-52A§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-L1H§.§_-C3e§;
   import §_-Z1K§.§_-q24§;
   import §_-hO§.§_-v24§;
   import §_-l1g§.§_-L3Q§;
   import §_-sQ§.§_-737§;
   import feathers.controls.List;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.utils.StringUtil;
   import starling.display.Quad;
   import starling.utils.Pool;
   
   public class §_-41p§
   {
      
      public function §_-41p§()
      {
         super();
      }
      
      public static function §_-y2B§() : String
      {
         return "support@pragmatixgames.com";
      }
      
      public static function §_-p1§(param1:uint, param2:uint) : String
      {
         var _loc3_:String = null;
         switch(param1)
         {
            case 31:
            case 36:
               _loc3_ = §_-52A§.§_-K3t§("PARAMS_CHEAT");
               break;
            case 32:
               _loc3_ = §_-52A§.§_-K3t§("PROTOCOL_CHEAT");
               break;
            case 33:
            case 43:
               _loc3_ = §_-52A§.§_-K3t§("BATTLE_CANCEL");
               break;
            case 44:
               _loc3_ = §_-52A§.§_-K3t§("BOT_USE");
               break;
            case 35:
               _loc3_ = §_-52A§.§_-K3t§("SEARCH_CHEAT");
               break;
            case 34:
            case 40:
               _loc3_ = §_-52A§.§_-K3t§("USE_PROGRAMS_FOR_CHEAT");
               break;
            case 39:
               _loc3_ = §_-52A§.§_-K3t§("WARDROBE_CHEAT");
               break;
            case 28:
            case 29:
            case 30:
            case 37:
            case 38:
            case 41:
               _loc3_ = §_-52A§.§_-K3t§("FRIEND_LIST_CHEAT");
               break;
            case 42:
               _loc3_ = §_-52A§.§_-K3t§("SHOP_CHEAT");
               break;
            case 45:
               _loc3_ = §_-52A§.§_-K3t§("BAN_FOR_CHAT");
               break;
            default:
               _loc3_ = §_-52A§.§_-K3t§("GENERAL_BAN");
         }
         if(param2)
         {
            _loc3_ += "\n" + §_-52A§.§_-K3t§("BAN_END_DATE") + " " + §_-737§.§_-x2e§(new Date(param2 * 1000),§_-737§.§_-p23§);
         }
         _loc3_ += "\n" + §_-52A§.§_-K3t§("HELP_FOR_BAN") + " " + StringUtil.§_-CN§(§_-y2B§(),"#5197FF");
         return _loc3_ + ("\n" + §_-52A§.§_-K3t§("USER_ID"));
      }
   }
}

