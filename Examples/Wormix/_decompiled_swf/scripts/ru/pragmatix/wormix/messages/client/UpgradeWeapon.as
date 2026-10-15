package ru.pragmatix.wormix.messages.client
{
   import §_-5P§.§_-H1u§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import flash.events.Event;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.pvp.messages.ex.ReconnectToBattle;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   
   public class UpgradeWeapon
   {
      
      public var recipeId:int;
      
      public function UpgradeWeapon(param1:int = 0)
      {
         super();
         this.recipeId = param1;
      }
   }
}

