package ru.pragmatix.wormix.social.common.post
{
   import §_-31W§.§_-8§;
   import §_-533§.§_-c2b§;
   import §_-62§.*;
   import §_-63U§.§_-21K§;
   import §_-73d§.§_-h2X§;
   import §_-91B§.§_-QK§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-D3A§.§_-TZ§;
   import §_-F39§.§_-a2W§;
   import §_-G2H§.*;
   import §_-H3§.*;
   import §_-J3L§.§_-h1D§;
   import §_-T1o§.§_-L3M§;
   import §_-TF§.§_-q1j§;
   import §_-TF§.§_-y2n§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-Y1O§.§_-42h§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1b§.§_-OP§;
   import §_-Z1K§.§_-q24§;
   import §_-ZB§.§_-81R§;
   import §_-g2r§.§_-03A§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-lD§;
   import §_-l1g§.§_-L3Q§;
   import §_-v2t§.§_-73F§;
   import §_-w2i§.*;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-4l§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.display.Display;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import flash.display.BitmapData;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.flox.social.utils.storage.StorageType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.NotInClanChatScreen;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.QuitClanResponse;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-L2i§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.AirCatAmmo;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-jw§ implements §_-81R§
   {
      
      public function §_-jw§()
      {
         super();
      }
      
      public function §_-Ez§() : Boolean
      {
         return false;
      }
      
      public function §_-n2r§(param1:§_-B2i§) : void
      {
      }
      
      public function §_-g1w§(param1:§_-W1r§) : void
      {
      }
      
      public function §_-Z2v§(param1:Array) : void
      {
      }
      
      public function §_-h2J§(param1:§_-O2i§, param2:§_-hK§, param3:Function) : void
      {
      }
      
      public function callToPvp(param1:UserProfile, param2:UserProfile) : void
      {
      }
      
      public function newTeamMate(param1:UserProfile) : void
      {
      }
      
      public function §_-41I§() : Boolean
      {
         return false;
      }
      
      public function §_-lT§(param1:§_-O2i§, param2:§_-hK§, param3:Function) : void
      {
      }
      
      public function requestPumpReaction(param1:UserProfile, param2:Boolean) : void
      {
      }
      
      public function activityPumpFriendsReaction(param1:UserProfile, param2:uint) : void
      {
      }
      
      public function §_-Y2o§(param1:§_-O2i§, param2:§_-hK§) : void
      {
      }
      
      public function §_-d2f§(param1:UserProfile, param2:Function) : void
      {
      }
      
      public function rubyFoundInHouse(param1:UserProfile) : void
      {
      }
      
      public function kickTeamMate(param1:UserProfile) : void
      {
      }
      
      public function §_-A2C§(param1:String) : void
      {
      }
      
      public function enter4DaySequence() : void
      {
      }
      
      public function share4DayBonus() : void
      {
      }
      
      public function share5DaySequence(param1:§_-4l§) : void
      {
      }
      
      public function enter5DaySequence(param1:§_-4l§) : void
      {
      }
      
      public function §_-91Y§(param1:§_-8§, param2:int, param3:BitmapData = null) : void
      {
      }
      
      public function §_-q1s§(param1:§_-W1r§) : void
      {
      }
      
      public function §_-n2J§(param1:uint) : void
      {
      }
      
      public function §_-Kq§(param1:Boolean, param2:§_-O2i§, param3:§_-J1§, param4:Function = null) : void
      {
      }
      
      public function §_-z1j§(param1:String, param2:Function = null) : void
      {
      }
      
      public function §_-3G§(param1:§_-W1r§) : void
      {
      }
      
      public function dispose() : void
      {
      }
      
      public function §_-25§(param1:UserProfile, param2:String) : void
      {
      }
      
      public function §_-B38§() : void
      {
      }
      
      public function §_-H3y§() : void
      {
      }
      
      public function §_-WF§() : void
      {
      }
      
      public function §_-xW§() : void
      {
      }
      
      public function §_-x1P§(param1:String, param2:String) : void
      {
      }
      
      public function §_-N2F§(param1:String, param2:String, param3:String, param4:Object, param5:Object) : void
      {
      }
      
      public function §_-eN§(param1:§_-d2F§) : void
      {
      }
      
      public function §_-415§(param1:ClanMemberTO) : void
      {
      }
      
      public function §_-h1P§(param1:int, param2:String) : void
      {
      }
      
      public function §_-f1Y§() : void
      {
      }
      
      public function §_-hh§() : Boolean
      {
         return false;
      }
   }
}

