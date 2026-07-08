package ru.pragmatix.wormix.gui
{
   import §_-82a§.*;
   import §_-82l§.§_-1I§;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.§_-A23§;
   import §_-Af§.§_-k1I§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-p2U§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-K3O§;
   import §_-E1I§.§_-b2n§;
   import §_-H2g§.§_-di§;
   import §_-SP§.§_-M4§;
   import §_-U1i§.§_-U16§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y1b§.§_-81l§;
   import §_-Y1b§.§_-OP§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-2m§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-q11§.*;
   import §_-qH§.§_-S25§;
   import §_-s1Q§.Artifacts;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-52k§;
   import §_-z1g§.§_-13d§;
   import §_-z1g§.§_-lp§;
   import com.hurlant.math.*;
   import config.§_-x1p§;
   import dragonBones.animation.WorldClock;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controller.§_-m1o§;
   import ru.pragmatix.wormix.messages.clans.main.server.InviteFriendToClan;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.Disconnect;
   import ru.pragmatix.wormix.messages.structures.PumpReactionRateStructure;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-A16§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.social.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.interfaces.§_-J3w§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   use namespace bi_internal;
   
   public class §_-E1B§
   {
      
      private static var _instance:§_-E1B§;
      
      private var queue:Vector.<§_-K3O§> = new Vector.<§_-K3O§>(0);
      
      public function §_-E1B§()
      {
         super();
      }
      
      public static function get instance() : §_-E1B§
      {
         if(!_instance)
         {
            _instance = new §_-E1B§();
         }
         return _instance;
      }
      
      public function §_-w1A§(param1:§_-K3O§) : void
      {
         this.queue.push(param1);
      }
      
      public function start() : void
      {
         this.§_-t1K§();
      }
      
      private function §_-t1K§() : void
      {
         var _loc1_:§_-K3O§ = null;
         if(this.queue.length > 0)
         {
            _loc1_ = this.queue.shift();
            _loc1_.show();
            if(_loc1_ is EventDispatcher)
            {
               §_-52k§.single(_loc1_ as EventDispatcher,§_-M4§.WINDOW_CLOSE,§_-bs§.§_-d1Z§(this.onWindowClose,this,[_loc1_]));
            }
         }
      }
      
      private function onWindowClose(param1:§_-K3O§) : void
      {
         this.§_-t1K§();
      }
      
      public function destroy() : void
      {
         var _loc1_:§_-K3O§ = null;
         for each(_loc1_ in this.queue)
         {
            _loc1_.hide();
            _loc1_.dispose();
         }
         this.queue = new Vector.<§_-K3O§>(0);
      }
   }
}

