package ru.pragmatix.flox.social.controller
{
   import §_-5Y§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-a3§;
   import §_-Ly§.§_-81L§;
   import §_-Y1b§.§_-22N§;
   import §_-b1F§.§_-G4§;
   import §_-b1R§.§_-Bx§;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-r1C§.§_-h2B§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import config.§_-V2w§;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.geom.ColorTransform;
   import flash.geom.Matrix;
   import flash.geom.Rectangle;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.ex.CancelBattle;
   import ru.pragmatix.wormix.utils.*;
   import starling.core.Starling;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   public class SocialType
   {
      
      public static const NONE:SocialType = new SocialType("none",0);
      
      public static const VK:SocialType = new SocialType("vkontakte",1);
      
      public static const FACEBOOK:SocialType = new SocialType("facebook",9);
      
      public static const MOBILE_ANDROID:SocialType = new SocialType("android",11);
      
      public static const STEAM:SocialType = new SocialType("steam",12);
      
      public static const MOBILE_IOS:SocialType = new SocialType("ios",14);
      
      public var name:String;
      
      public var servercode:uint;
      
      public function SocialType(param1:String, param2:int)
      {
         super();
         this.name = param1;
         this.servercode = param2;
      }
      
      public static function valueOf(param1:String) : SocialType
      {
         switch(param1)
         {
            case VK.name:
               return VK;
            case FACEBOOK.name:
               return FACEBOOK;
            case MOBILE_ANDROID.name:
               return MOBILE_ANDROID;
            case STEAM.name:
               return STEAM;
            case MOBILE_IOS.name:
               return MOBILE_IOS;
            default:
               return NONE;
         }
      }
      
      public static function valueOfServerCode(param1:int) : SocialType
      {
         switch(param1)
         {
            case VK.servercode:
               return VK;
            case FACEBOOK.servercode:
               return FACEBOOK;
            case MOBILE_ANDROID.servercode:
               return MOBILE_ANDROID;
            case STEAM.servercode:
               return STEAM;
            case MOBILE_IOS.servercode:
               return MOBILE_IOS;
            default:
               return NONE;
         }
      }
      
      public function equals(param1:SocialType) : Boolean
      {
         return this.name == param1.name;
      }
      
      public function toString() : String
      {
         return "SocialType{" + "name=" + this.name + ",servercode=" + this.servercode + "}";
      }
   }
}

