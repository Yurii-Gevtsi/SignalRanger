package ru.pragmatix.wormix.messages.server
{
   import §_-A2F§.§_-4B§;
   import §_-C3f§.*;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-j1w§.§_-B2i§;
   import §_-l1g§.§_-L3Q§;
   import §_-w2i§.§_-Zd§;
   import feathers.data.ListCollection;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.request.ListClansRequest;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import ru.pragmatix.wormix.weapons.ammo.type.shuriken.Shuriken;
   import starling.events.Event;
   
   public class LoginError
   {
      
      public static const PROFILACTIC_WORK:int = 0;
      
      public static const INCORRECT_KEY:int = 1;
      
      public static const ALREADY_IN_GAME:int = 2;
      
      public static const INTERNAL_SERVER_ERROR:int = 3;
      
      public static const BANNED:int = 4;
      
      public static const RECONNECT_TIMEOUT:int = 5;
      
      public static const INCORRECT_PROTOCOL_VERSION:int = 6;
      
      public static const SERVER_IS_STARTING:int = 7;
      
      public static const NOT_IN_WHITE_LIST:int = 8;
      
      public static const INCORRECT_MOBILE_PLATFORM_ID:int = 9;
      
      public static const NEED_STRICT_AUTHORIZATION:int = 10;
      
      public var code:int;
      
      public function LoginError()
      {
         super();
      }
      
      public function toString() : String
      {
         return "LoginError{code=" + String(this.code) + "}";
      }
   }
}

