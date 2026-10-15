package ru.pragmatix.wormix.messages.server
{
   import §_-C2M§.DesktopTutorialPanel;
   import §_-C2M§.MobileTutorialPanel;
   import §_-C2M§.§_-o2f§;
   import §_-F39§.§_-Ey§;
   import §_-Y1O§.*;
   import §_-j22§.§_-5g§;
   import feathers.core.ITextRenderer;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.MobilePlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-fT§;
   import ru.pragmatix.wormix.messages.client.RepeatLevelUpAwardRequest;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class UpgradeWeaponResult
   {
      
      public static const SUCCESS:int = 0;
      
      public var recipeId:int;
      
      public var result:int;
      
      public function UpgradeWeaponResult()
      {
         super();
      }
   }
}

