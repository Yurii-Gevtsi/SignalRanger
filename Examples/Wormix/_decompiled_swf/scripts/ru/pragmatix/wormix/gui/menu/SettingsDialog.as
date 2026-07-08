package ru.pragmatix.wormix.gui.menu
{
   import §_-13R§.§_-i17§;
   import §_-2U§.*;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-V1Y§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.§_-D3v§;
   import §_-X14§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-5c§;
   import §_-z1g§.§_-C32§;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.TabBar;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.FeathersControl;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.events.Event;
   import flash.filters.BlurFilter;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.BuyUnlockMission;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.serialization.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-y1V§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.SystemUtil;
   
   public class SettingsDialog extends §_-V1Y§
   {
      
      public static const CHANGE_NAME_EVENT:String = "CHANGE_NAME_EVENT";
      
      public static const SETUP_GAMEPAD_EVENT:String = "SETUP_GAMEPAD_EVENT";
      
      public static const SHOW_INFO_EVENT:String = "SHOW_INFO_EVENT";
      
      private var binder:Binder;
      
      private var §_-t2A§:§_-i17§;
      
      public function SettingsDialog()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.binder = new Binder();
         §_-Z1p§("settings_dialog",this.binder);
         addChild(this.binder._content);
         if(§_-N1V§.settings.§_-03F§)
         {
            this.§_-t2A§ = new §_-i17§(this.binder._socialControllerPanel);
         }
         else
         {
            this.binder._socialControllerPanel.removeFromParent(true);
         }
         §_-X1j§.§_-61v§.§_-G3l§(this.binder._linkButton,this.binder._linkLabel);
         if(!§_-y1V§.isAvailable)
         {
            §_-YQ§.§_-Xv§(this.binder._fullscreenButton,false);
            §_-YQ§.§_-Xv§(this.binder._fullscreenLabel,false);
         }
         §_-J2N§(this.binder._content,starling.events.Event.CLOSE,§_-R1t§);
         §_-J2N§(this.binder._nameButton,starling.events.Event.TRIGGERED,this.§_-y1l§);
         §_-J2N§(this.binder._musicButton,starling.events.Event.TRIGGERED,this.§_-y1l§);
         §_-J2N§(this.binder._soundsButton,starling.events.Event.TRIGGERED,this.§_-y1l§);
         §_-J2N§(this.binder._dndButton,starling.events.Event.TRIGGERED,this.§_-y1l§);
         §_-J2N§(this.binder._infoButton,starling.events.Event.TRIGGERED,this.§_-y1l§);
         §_-J2N§(this.binder._fullscreenButton,starling.events.Event.TRIGGERED,this.§_-y1l§);
         EventDispatcher(§_-X1j§.§_-61v§).addEventListener(starling.events.Event.CHANGE,this.§_-7z§);
      }
      
      override public function dispose() : void
      {
         super.dispose();
         §_-X1j§.§_-61v§.§_-s2j§();
         EventDispatcher(§_-X1j§.§_-61v§).removeEventListener(starling.events.Event.CHANGE,this.§_-7z§);
         §_-jg§(this.binder._content,starling.events.Event.CLOSE);
         §_-jg§(this.binder._nameButton,starling.events.Event.TRIGGERED);
         §_-jg§(this.binder._musicButton,starling.events.Event.TRIGGERED);
         §_-jg§(this.binder._soundsButton,starling.events.Event.TRIGGERED);
         §_-jg§(this.binder._dndButton,starling.events.Event.TRIGGERED);
         §_-jg§(this.binder._infoButton,starling.events.Event.TRIGGERED);
         §_-jg§(this.binder._fullscreenButton,starling.events.Event.TRIGGERED);
      }
      
      private function §_-7z§(param1:starling.events.Event) : void
      {
         invalidate(FeathersControl.INVALIDATION_FLAG_DATA);
      }
      
      override protected function draw() : void
      {
         var _loc3_:String = null;
         var _loc4_:Boolean = false;
         var _loc5_:Boolean = false;
         var _loc6_:String = null;
         var _loc7_:String = null;
         var _loc8_:String = null;
         var _loc9_:String = null;
         var _loc10_:String = null;
         var _loc11_:Array = null;
         var _loc12_:String = null;
         var _loc13_:int = 0;
         var _loc14_:String = null;
         var _loc15_:String = null;
         super.draw();
         var _loc1_:Boolean = §_-vR§.§_-I1d§ == SocialType.MOBILE_IOS.name;
         var _loc2_:Boolean = §_-vR§.§_-I1d§ == SocialType.MOBILE_ANDROID.name;
         if(isInvalid(INVALIDATION_FLAG_DATA))
         {
            _loc3_ = Application.userProfile.getCharName();
            if(_loc3_ == §_-33r§.§_-y1j§ || _loc3_ == "")
            {
               _loc3_ = §_-s2a§.§_-K3t§(§_-s2a§.DEFAULT_NAME_TEXT);
            }
            this.binder._nameValue.text = _loc3_;
            this.binder._soundsButton.label = §_-s2a§.§_-K3t§(§_-h2B§.§_-MY§() ? §_-s2a§.STATUS_ON : §_-s2a§.STATUS_OFF);
            this.binder._musicButton.label = §_-s2a§.§_-K3t§(§_-h2B§.§_-9Y§() ? §_-s2a§.STATUS_ON : §_-s2a§.STATUS_OFF);
            this.binder._dndButton.label = §_-s2a§.§_-K3t§(Application.isUserIgnoring() ? §_-s2a§.STATUS_OFF : §_-s2a§.STATUS_ON);
            this.binder._soundsLabel.styleName = §_-h2B§.§_-MY§() ? AlternateTextStyles.NEW_BRIGHT_NORMAL_LEFT : AlternateTextStyles.NEW_DARK_NORMAL_LEFT;
            this.binder._musicLabel.styleName = §_-h2B§.§_-9Y§() ? AlternateTextStyles.NEW_BRIGHT_NORMAL_LEFT : AlternateTextStyles.NEW_DARK_NORMAL_LEFT;
            this.binder._dndLabel.styleName = Application.isUserIgnoring() ? AlternateTextStyles.NEW_DARK_NORMAL_RIGHT : AlternateTextStyles.NEW_BRIGHT_NORMAL_RIGHT;
            if(§_-y1V§.isAvailable)
            {
               this.binder._fullscreenButton.label = §_-s2a§.§_-K3t§(§_-y1V§.§_-BV§ ? §_-s2a§.STATUS_ON : §_-s2a§.STATUS_OFF);
               this.binder._fullscreenLabel.styleName = §_-y1V§.§_-BV§ ? AlternateTextStyles.NEW_BRIGHT_NORMAL_RIGHT : AlternateTextStyles.NEW_DARK_NORMAL_RIGHT;
            }
            _loc4_ = §_-X1j§.§_-61v§.isServiceAvailable && §_-vR§.§_-42H§ && (!§_-X1j§.§_-61v§.isLinked || _loc1_);
            this.binder._linkButton.isEnabled = _loc4_;
            _loc5_ = §_-X1j§.§_-dr§.numDevices > 0;
            this.binder._gamepadButton.isEnabled = _loc5_;
         }
         if(isInvalid(§_-dU§.§_-h26§))
         {
            _loc6_ = §_-X1j§.§_-It§.getUserId();
            this.binder._content.title = §_-s2a§.§_-K3t§(§_-s2a§.SETTINGS_TITLE) + " :: " + §_-s2a§.§_-K3t§(§_-s2a§.TITLE_ACCOUNT_ID) + " " + Application.userProfile.getId() + (_loc2_ && §_-vR§.§_-w2x§ ? "\n // loc " + _loc6_.substr(0,_loc6_.length * 0.5) + "***" : "") + (_loc1_ && §_-vR§.§_-w2x§ ? "\n // loc " + _loc6_.split("-")[0] + "-***" : "") + (_loc1_ && §_-vR§.§_-w2x§ ? " // kch " + §_-X1j§.§_-D1T§.getUserId().split("-")[0] + "-***" : "") + "\n" + §_-vR§.§_-g1L§();
            this.binder._nameLabel.text = §_-s2a§.§_-K3t§(§_-s2a§.PLAYER_NAME_TITLE);
            this.binder._langLabel.text = §_-s2a§.§_-K3t§(§_-s2a§.LANGUAGE_TITLE);
            this.binder._musicLabel.text = §_-s2a§.§_-K3t§(§_-s2a§.MUSIC_LABEL);
            this.binder._soundsLabel.text = §_-s2a§.§_-K3t§(§_-s2a§.SOUND_LABEL);
            this.binder._dndLabel.text = §_-s2a§.§_-K3t§(§_-s2a§.PVP_IGNORE_LABEL);
            if(§_-y1V§.isAvailable)
            {
               this.binder._fullscreenLabel.text = §_-s2a§.§_-K3t§(§_-s2a§.FULLSCREEN_LABEL);
            }
            this.binder._nameButton.label = §_-s2a§.§_-K3t§(§_-s2a§.CHANGE_NAME_TITLE);
            this.binder._gamepadButton.label = §_-s2a§.§_-K3t§(§_-s2a§.GAMEPAD_SETUP_TITLE);
            this.binder._infoButton.label = §_-s2a§.§_-K3t§(§_-s2a§.INFO_AND_SUPPORT);
         }
         if(isInvalid(INVALIDATION_FLAG_DATA) || isInvalid(§_-dU§.§_-h26§))
         {
            this.binder._linkLabel.styleName = §_-X1j§.§_-61v§.isServiceAvailable && !§_-X1j§.§_-61v§.isLinked ? AlternateTextStyles.NEW_BRIGHT_NORMAL_RIGHT : AlternateTextStyles.NEW_DARK_NORMAL_RIGHT;
            _loc7_ = "";
            switch(§_-vR§.§_-I1d§)
            {
               case SocialType.MOBILE_ANDROID.name:
                  if(§_-X1j§.§_-61v§.isLinked)
                  {
                     _loc10_ = §_-X1j§.§_-p1c§.§_-i1v§;
                     _loc11_ = _loc10_.split("@");
                     _loc12_ = _loc11_[0];
                     _loc13_ = _loc12_.length * 0.3;
                     _loc14_ = _loc12_.substr(0,_loc13_) + StringUtil.§_-u12§(3,"*") + _loc12_.substring(_loc12_.length - _loc13_);
                     _loc15_ = "@" + _loc11_[1];
                     _loc8_ = _loc14_ + _loc15_;
                  }
                  _loc7_ = §_-X1j§.§_-61v§.isLinked ? StringUtil.format(§_-s2a§.GOOGLE_LINKED_TEXT,{"mail":_loc8_}) : §_-s2a§.§_-K3t§(§_-s2a§.GOOGLE_SAVE_LABEL);
                  break;
               case SocialType.MOBILE_IOS.name:
                  _loc7_ = §_-s2a§.§_-K3t§(§_-s2a§.ENTER_FROM_CLOUD_LABEL);
                  _loc9_ = §_-X1j§.§_-61v§.§_-x2Y§();
                  if(_loc9_)
                  {
                     _loc7_ += " (" + _loc9_.split("-")[0] + "-***)";
                  }
            }
            this.binder._linkLabel.text = _loc7_;
            §_-X1j§.§_-61v§.§_-g2O§();
         }
      }
      
      private function §_-y1l§(param1:starling.events.Event) : void
      {
         §_-h2B§.play(§_-81L§.§_-o1Z§);
         switch(param1.currentTarget)
         {
            case this.binder._nameButton:
               dispatchEventWith(CHANGE_NAME_EVENT);
               break;
            case this.binder._soundsButton:
               §_-h2B§.§_-d1I§();
               invalidate(INVALIDATION_FLAG_DATA);
               break;
            case this.binder._musicButton:
               §_-h2B§.§_-n2A§();
               invalidate(INVALIDATION_FLAG_DATA);
               break;
            case this.binder._dndButton:
               Application.§_-dq§(!Application.isUserIgnoring());
               invalidate(INVALIDATION_FLAG_DATA);
               break;
            case this.binder._gamepadButton:
               dispatchEventWith(SETUP_GAMEPAD_EVENT);
               break;
            case this.binder._infoButton:
               dispatchEventWith(SHOW_INFO_EVENT);
               break;
            case this.binder._fullscreenButton:
               if(§_-y1V§.isAvailable)
               {
                  §_-y1V§.§_-B39§();
                  invalidate(INVALIDATION_FLAG_DATA);
               }
         }
      }
   }
}

import feathers.controls.Button;
import feathers.controls.Label;
import feathers.controls.LayoutGroup;
import feathers.controls.Panel;

class Binder
{
   
   public var _content:Panel;
   
   public var _nameLabel:Label;
   
   public var _nameValue:Label;
   
   public var _nameButton:Button;
   
   public var _langLabel:Label;
   
   public var _langPickerList:LanguagePickerList;
   
   public var _socialControllerPanel:LayoutGroup;
   
   public var _linkLabel:Label;
   
   public var _linkButton:Button;
   
   public var _musicLabel:Label;
   
   public var _musicButton:Button;
   
   public var _soundsLabel:Label;
   
   public var _soundsButton:Button;
   
   public var _fullscreenLabel:Label;
   
   public var _fullscreenButton:Button;
   
   public var _dndLabel:Label;
   
   public var _dndButton:Button;
   
   public var _gamepadButton:Button;
   
   public var _infoButton:Button;
   
   public function Binder()
   {
      super();
   }
}
