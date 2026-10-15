package ru.pragmatix.wormix.controller
{
   import §_-21A§.§_-A2x§;
   import §_-21p§.§_-4w§;
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-zF§;
   import §_-63U§.§_-c1e§;
   import §_-73I§.§_-q15§;
   import §_-931§.§_-02O§;
   import §_-931§.§_-i2r§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-i1V§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-F39§.*;
   import §_-G2H§.*;
   import §_-L1H§.§_-C3e§;
   import §_-L2F§.§_-D2p§;
   import §_-OJ§.§_-32g§;
   import §_-OJ§.§_-L3B§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1b§.*;
   import §_-YF§.§_-N3§;
   import §_-ZP§.§_-w1J§;
   import §_-k2s§.§_-e1U§;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.*;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.*;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.pushnotifications.PushNotifications;
   import config.§_-K32§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Label;
   import feathers.layout.VerticalAlign;
   import flash.display.BitmapData;
   import flash.display.Shape;
   import flash.text.TextFormat;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.*;
   import ru.pragmatix.wormix.gui.menu.clans.chat.NotInClanChatScreen;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.client.SetName;
   import ru.pragmatix.wormix.messages.server.SetNameResult;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.items.craft.§_-6S§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.textures.TextureSmoothing;
   import starling.utils.ScaleMode;
   
   public class §_-F2G§ extends EventDispatcher
   {
      
      public var §_-B3h§:int;
      
      private var §_-Gl§:String = null;
      
      public function §_-F2G§()
      {
         super();
      }
      
      public function §_-E22§() : void
      {
         var _loc1_:UserProfile = Application.userProfile;
         var _loc2_:String = §_-33r§.§_-fl§(_loc1_);
         var _loc3_:String = §_-33r§.§_-KQ§(_loc1_);
         if(_loc2_ == "" && _loc3_ != "")
         {
            this.§_-j2t§(_loc3_);
         }
      }
      
      public function §_-j2t§(param1:String) : void
      {
         var _loc2_:Object = new SetName(param1);
         §_-J2l§.§_-R25§.show(§_-d21§.§_-vH§,§_-J2g§.§_-i1N§,_loc2_,SetNameResult,this.§_-H1a§,this.§_-C2J§);
      }
      
      public function §_-D1b§(param1:String) : void
      {
         this.§_-Gl§ = param1;
         §_-N1V§.§_-42k§.§_-F1I§(§_-w1J§.§_-q1B§);
      }
      
      public function §_-G2f§() : void
      {
         this.§_-j2t§(this.§_-Gl§);
         this.§_-Gl§ = null;
      }
      
      private function §_-H1a§(param1:§_-63i§) : void
      {
         var _loc2_:SetNameResult = SetNameResult(param1.message);
         this.§_-B3h§ = _loc2_.renameAct;
         this.§_-U1f§(Application.userProfile,_loc2_);
         var _loc3_:UserProfile = §_-N3§.§_-g1D§(Application.userProfile.getId());
         if(_loc3_)
         {
            this.§_-U1f§(_loc3_,_loc2_);
         }
         §_-J2l§.§_-R25§.hide();
         dispatchEvent(new Event(Event.COMPLETE));
      }
      
      private function §_-U1f§(param1:UserProfile, param2:SetNameResult) : void
      {
         param1.§_-t0§(param2.name);
         param1.§_-I1N§(param2.name);
      }
      
      private function §_-C2J§(param1:Object) : void
      {
         §_-J2l§.§_-R25§.hide();
         dispatchEvent(new Event(Event.COMPLETE));
      }
   }
}

