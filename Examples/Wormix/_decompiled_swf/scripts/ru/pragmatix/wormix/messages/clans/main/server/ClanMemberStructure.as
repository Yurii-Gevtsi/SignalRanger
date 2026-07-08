package ru.pragmatix.wormix.messages.clans.main.server
{
   import §_-21p§.§_-4w§;
   import §_-A1K§.§_-TA§;
   import §_-G1h§.MobileMainOverlayScreen;
   import §_-P1z§.*;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-Vg§.*;
   import §_-l1K§.§_-vO§;
   import §_-m0§.§_-X1F§;
   import §_-r1C§.§_-h2B§;
   import §_-x2Q§.§_-dU§;
   import §_-zI§.§_-X1q§;
   import feathers.controls.Screen;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.events.FeathersEventType;
   import feathers.themes.FontColor;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Color;
   
   public class ClanMemberStructure
   {
      
      public var rank:int;
      
      public var clanId:int;
      
      public var clanName:String;
      
      public var clanEmblem:Vector.<uint> = new Vector.<uint>();
      
      public var clanRating:int;
      
      public var clanSeasonRating:int;
      
      public var reviewState:int;
      
      public var prevSeasonTopPlace:int;
      
      public function ClanMemberStructure()
      {
         super();
      }
      
      public function isBlocked() : Boolean
      {
         return this.reviewState == -1;
      }
      
      public function toString() : String
      {
         return "ClanMemberStructure{" + "clanId=" + this.clanId + "}";
      }
   }
}

