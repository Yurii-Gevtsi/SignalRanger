package ru.pragmatix.wormix.messages.server
{
   import §_-A1K§.§_-TA§;
   import §_-HD§.§_-63z§;
   import §_-b1F§.§_-G4§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import feathers.controls.ScrollBarDisplayMode;
   import feathers.controls.ScrollInteractionMode;
   import feathers.controls.ScrollPolicy;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.DesktopShopPropertiesListItem;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.DesktopShopnfoRequirementRenderer;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.structures.DepositStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.pvp.messages.ex.RejectBattleOffer;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   
   public class OpenDepositResult
   {
      
      public var deposit:DepositStructure;
      
      public var firstPart:int;
      
      public var sessionKey:String;
      
      public function OpenDepositResult()
      {
         super();
      }
   }
}

