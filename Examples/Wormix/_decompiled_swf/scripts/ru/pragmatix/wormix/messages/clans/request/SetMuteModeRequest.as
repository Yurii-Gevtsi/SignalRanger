package ru.pragmatix.wormix.messages.clans.request
{
   import §_-A1K§.§_-TA§;
   import §_-Vg§.*;
   import §_-j22§.§_-5g§;
   import §_-m0§.§_-X1F§;
   import §_-r1C§.§_-h2B§;
   import §_-x2Q§.§_-dU§;
   import §_-zI§.§_-X1q§;
   import feathers.themes.FontColor;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import ru.pragmatix.wormix.weapons.interfaces.§_-o2d§;
   import starling.events.Event;
   import starling.utils.Color;
   
   public class SetMuteModeRequest
   {
      
      public var profileId:int;
      
      public var value:Boolean;
      
      public function SetMuteModeRequest()
      {
         super();
      }
   }
}

