package ru.pragmatix.wormix.gui.menu
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-bs§;
   import §_-DJ§.§_-V1Y§;
   import §_-DJ§.§_-fH§;
   import §_-L2F§.§_-I1q§;
   import §_-L2F§.§_-t2Z§;
   import §_-Ly§.§_-81L§;
   import §_-Ly§.§_-n1J§;
   import §_-P1z§.*;
   import §_-Y1b§.*;
   import §_-fo§.§_-Q2z§;
   import §_-fo§.§_-hH§;
   import §_-j22§.§_-5g§;
   import §_-p14§.§_-A3Y§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-x2Q§.§_-dU§;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.gui.controls.*;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.messages.server.BuyUnlockMissionResult;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.utils.*;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.utils.Pool;
   
   use namespace gestouch_internal;
   
   public class InfoAndSupportWindow extends §_-V1Y§
   {
      
      private var binder:Binder;
      
      public function InfoAndSupportWindow()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.binder = new Binder();
         §_-Z1p§("info_panel",this.binder);
         addChild(this.binder._panel);
         this.binder._panel.title = §_-s2a§.§_-K3t§(§_-s2a§.INFO_AND_SUPPORT);
         this.binder._supportBtn.label = §_-s2a§.§_-K3t§(§_-s2a§.REPORT_A_PROBLEM);
         this.binder._termsBtn.label = §_-s2a§.§_-K3t§(§_-s2a§.TERMS_OF_USE);
         this.binder._policyBtn.label = §_-s2a§.§_-K3t§(§_-s2a§.CONF_POLICY);
         §_-J2N§(this.binder._panel,starling.events.Event.CLOSE,§_-R1t§);
         §_-J2N§(this.binder._supportBtn,starling.events.Event.TRIGGERED,this.§_-r6§);
         §_-J2N§(this.binder._termsBtn,starling.events.Event.TRIGGERED,this.§_-r6§);
         §_-J2N§(this.binder._policyBtn,starling.events.Event.TRIGGERED,this.§_-r6§);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         §_-jg§(this.binder._panel,starling.events.Event.CLOSE);
         §_-jg§(this.binder._supportBtn,starling.events.Event.TRIGGERED);
         §_-jg§(this.binder._termsBtn,starling.events.Event.TRIGGERED);
         §_-jg§(this.binder._policyBtn,starling.events.Event.TRIGGERED);
      }
      
      private function §_-r6§(param1:starling.events.Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         switch(param1.target)
         {
            case this.binder._policyBtn:
               navigateToURL(new URLRequest("https://pragmatixgames.com/en/pragmatix-ltd-privacy-policy"));
               break;
            case this.binder._termsBtn:
               navigateToURL(new URLRequest("https://pragmatixgames.com/en/terms-and-conditions"));
               break;
            case this.binder._supportBtn:
               §_-t2B§.§_-y2l§();
         }
      }
   }
}

import feathers.controls.Button;
import feathers.controls.Panel;

class Binder
{
   
   public var _panel:Panel;
   
   public var _supportBtn:Button;
   
   public var _termsBtn:Button;
   
   public var _policyBtn:Button;
   
   public function Binder()
   {
      super();
   }
}
