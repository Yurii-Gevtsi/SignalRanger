package ru.pragmatix.wormix.messages.clans
{
   import §_-73d§.§_-V19§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-RE§.§_-r2q§;
   import §_-TF§.§_-F1B§;
   import §_-TF§.§_-q1j§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-n1w§.*;
   import com.hurlant.math.*;
   import feathers.controls.LayoutGroup;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.HorizontalLayoutData;
   import feathers.layout.VerticalAlign;
   import flash.text.TextFormatAlign;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.weapons.*;
   import starling.utils.Color;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   use namespace bi_internal;
   
   public class ClanServiceResult
   {
      
      private static var all:Vector.<ClanServiceResult> = new Vector.<ClanServiceResult>();
      
      public static const OK:ClanServiceResult = new ClanServiceResult(0,"OK");
      
      public static const ERR_INCORRECT_PROTOCOL:ClanServiceResult = new ClanServiceResult(1,"ERR_INCORRECT_PROTOCOL");
      
      public static const ERR_ALREADY_IN_GAME:ClanServiceResult = new ClanServiceResult(2,"ERR_ALREADY_IN_GAME");
      
      public static const ERR_CHEAT:ClanServiceResult = new ClanServiceResult(11,"ERR_CHEAT");
      
      public static const ERR_BAN:ClanServiceResult = new ClanServiceResult(12,"ERR_BAN");
      
      public static const ERR_RUNTIME:ClanServiceResult = new ClanServiceResult(14,"ERR_RUNTIME");
      
      public static const ERR_INDEFINITE:ClanServiceResult = new ClanServiceResult(15,"ERR_INDEFINITE");
      
      public static const ERR_DEADLOCK:ClanServiceResult = new ClanServiceResult(16,"ERR_DEADLOCK");
      
      public static const ERR_ACCESS_DENIED:ClanServiceResult = new ClanServiceResult(102,"ERR_ACCESS_DENIED");
      
      public static const ERR_INVALID_CLAN_NAME:ClanServiceResult = new ClanServiceResult(103,"ERR_INVALID_CLAN_NAME");
      
      public static const ERR_INVALID_ARGUMENT:ClanServiceResult = new ClanServiceResult(104,"ERR_INVALID_ARGUMENT");
      
      public static const ERR_NOT_ENOUGH_MONEY:ClanServiceResult = new ClanServiceResult(105,"ERR_NOT_ENOUGH_MONEY");
      
      public static const ERR_REQUIREMENTS_FAILURE:ClanServiceResult = new ClanServiceResult(106,"ERR_REQUIREMENTS_FAILURE");
      
      public static const ERR_NOT_FOUND:ClanServiceResult = new ClanServiceResult(109,"ERR_NOT_FOUND");
      
      public static const ERR_INTEROP:ClanServiceResult = new ClanServiceResult(110,"ERR_INTEROP");
      
      public static const ERR_INVALID_STATE:ClanServiceResult = new ClanServiceResult(111,"ERR_INVALID_STATE");
      
      public static const ERR_NOT_ENOUGH_TREAS:ClanServiceResult = new ClanServiceResult(112,"ERR_NOT_ENOUGH_TREAS");
      
      public static const ERR_NOT_ENOUGH_RATING:ClanServiceResult = new ClanServiceResult(114,"ERR_NOT_ENOUGH_RATING");
      
      public static const ERR_NOT_ENOUGH_LEVEL:ClanServiceResult = new ClanServiceResult(115,"ERR_NOT_ENOUGH_LEVEL");
      
      public static const ERR_ALREADY_IN_CLAN:ClanServiceResult = new ClanServiceResult(201,"ERR_ALREADY_IN_CLAN");
      
      public static const ERR_NOT_IN_CLAN:ClanServiceResult = new ClanServiceResult(202,"ERR_NOT_IN_CLAN");
      
      public static const ERR_CLAN_SIZE_LIMIT:ClanServiceResult = new ClanServiceResult(203,"ERR_CLAN_SIZE_LIMIT");
      
      public static const ERR_CLAN_INVITE_LIMIT:ClanServiceResult = new ClanServiceResult(204,"ERR_CLAN_INVITE_LIMIT");
      
      public static const ERR_PROFILE_INVITE_LIMIT:ClanServiceResult = new ClanServiceResult(205,"ERR_PROFILE_INVITE_LIMIT");
      
      public static const ERR_REPEATED_INVITE:ClanServiceResult = new ClanServiceResult(206,"ERR_REPEATED_INVITE");
      
      public static const ERR_CLAN_NAME_EXISTS:ClanServiceResult = new ClanServiceResult(207,"ERR_CLAN_NAME_EXISTS");
      
      public static const ERR_CLAN_LOCKED:ClanServiceResult = new ClanServiceResult(208,"ERR_CLAN_LOCKED");
      
      public static const ERR_EMPTY_SOCIAL_ID:ClanServiceResult = new ClanServiceResult(18,"ERR_EMPTY_SOCIAL_ID");
      
      public var code:int;
      
      public var name:String;
      
      public function ClanServiceResult(param1:int, param2:String)
      {
         super();
         this.code = param1;
         this.name = param2;
         all.push(this);
      }
      
      public static function getResultByCode(param1:int) : ClanServiceResult
      {
         var _loc2_:ClanServiceResult = null;
         for each(_loc2_ in all)
         {
            if(_loc2_.code == param1)
            {
               return _loc2_;
            }
         }
         return null;
      }
   }
}

