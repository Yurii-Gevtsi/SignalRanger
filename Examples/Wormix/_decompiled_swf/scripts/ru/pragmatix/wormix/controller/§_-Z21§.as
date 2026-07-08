package ru.pragmatix.wormix.controller
{
   import §_-62§.§_-O2H§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-fH§;
   import §_-J31§.§_-e2J§;
   import §_-K35§.§_-99§;
   import §_-N8§.§_-o2w§;
   import §_-R2S§.*;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-fo§.§_-v2P§;
   import §_-j22§.§_-5g§;
   import §_-s20§.§_-J2g§;
   import §_-x2Q§.§_-dU§;
   import dragonBones.animation.WorldClock;
   import flash.events.StatusEvent;
   import flash.net.LocalConnection;
   import flash.system.LoaderContext;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.PostToChat;
   import ru.pragmatix.wormix.messages.server.ProfileRestrictions;
   import ru.pragmatix.wormix.messages.structures.ReconnectToSimpleBattleResultStructure;
   import ru.pragmatix.wormix.messages.structures.RestrictionItemStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.weapons.ammo.LightningBullet;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class §_-Z21§ extends EventDispatcher
   {
      
      public static const CHAT:int = 0;
      
      public static const TEAM_CHAT:int = 1;
      
      public static const GLOBAL_CHAT:int = 2;
      
      private var _list:Array;
      
      public function §_-Z21§()
      {
         super();
         §_-J2g§.§_-o1R§(ProfileRestrictions,this.§_-W2M§);
      }
      
      private function §_-W2M§(param1:§_-63i§) : void
      {
         var _loc2_:ProfileRestrictions = ProfileRestrictions(param1.message);
         this._list = _loc2_.restrictions;
         dispatchEventWith(Event.CHANGE);
      }
      
      public function get list() : Array
      {
         return this._list;
      }
      
      public function set list(param1:Array) : void
      {
         this._list = param1;
      }
      
      public function §_-ga§(param1:int) : Boolean
      {
         var _loc2_:RestrictionItemStructure = null;
         for each(_loc2_ in this._list)
         {
            if(_loc2_.blocks.indexOf(param1) > -1)
            {
               return true;
            }
         }
         return false;
      }
      
      public function §_-X2o§(param1:int) : String
      {
         var _loc2_:RestrictionItemStructure = null;
         var _loc3_:String = null;
         for each(_loc2_ in this._list)
         {
            if(_loc2_.blocks.indexOf(param1) > -1 && _loc2_.endDate > 0)
            {
               _loc3_ = §_-72u§.§_-9Q§(_loc2_.endDate);
               return §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.RESTRICT_FOR),{"date":_loc3_});
            }
         }
         return "";
      }
   }
}

