package ru.pragmatix.wormix.messages.client
{
   import §_-21p§.§_-4w§;
   import §_-CF§.§_-p2U§;
   import §_-DJ§.§_-J2l§;
   import §_-J31§.§_-R1h§;
   import §_-P1z§.*;
   import §_-YF§.§_-Zi§;
   import §_-Z1w§.§_-s1x§;
   import §_-j1w§.§_-L1x§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-p18§.§_-h1b§;
   import §_-s20§.§_-J2g§;
   import feathers.controls.Button;
   import flash.events.MouseEvent;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.platform.§_-WK§;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.weapons.ammo.feats.IFrameFeat;
   import starling.display.Image;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class NotifyProfile
   {
      
      public var recipientProfileId:int;
      
      public var socialType:SocialType;
      
      public var locKey:String;
      
      public var locArgs:Vector.<String>;
      
      public var delay:int = 0;
      
      public var timeToLive:int = 0;
      
      public var sessionKey:String;
      
      public function NotifyProfile()
      {
         super();
      }
   }
}

