package ru.pragmatix.wormix.messages.client
{
   import §_-62§.§_-O2H§;
   import §_-DJ§.§_-J2l§;
   import §_-Ly§.§_-81L§;
   import §_-TF§.§_-q1j§;
   import §_-YF§.§_-q2v§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.*;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import feathers.controls.Label;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.geom.Point;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.core.Starling;
   import starling.display.Image;
   import starling.events.Event;
   import starling.text.TextField;
   import starling.text.TextFormat;
   import starling.utils.Align;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class SteamTransactionInitRequest
   {
      
      public var productCode:String;
      
      public function SteamTransactionInitRequest()
      {
         super();
      }
      
      public function toString() : String
      {
         return "SteamTransactionInitRequest{productCode=" + String(this.productCode) + "}";
      }
   }
}

