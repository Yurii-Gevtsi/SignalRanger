package ru.pragmatix.wormix.utils
{
   import §_-12a§.§_-lc§;
   import §_-52L§.§_-u2x§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-zF§;
   import §_-L2F§.§_-D2p§;
   import §_-L2F§.§_-I1q§;
   import §_-L2F§.§_-t2Z§;
   import §_-Ly§.§_-81L§;
   import §_-Ta§.*;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-Y1O§.§_-yA§;
   import §_-Y1b§.*;
   import §_-ZP§.§_-YG§;
   import §_-ZP§.§_-w1J§;
   import §_-hO§.§_-BY§;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-lp§;
   import dragonBones.Slot;
   import feathers.controls.ScrollPolicy;
   import feathers.core.ITextRenderer;
   import feathers.data.VectorCollection;
   import flash.geom.ColorTransform;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.BuyUnlockMissionResult;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.core.Starling;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   public class §_-71v§
   {
      
      public function §_-71v§()
      {
         super();
      }
      
      public static function §_-U1x§(param1:UserProfile, param2:int) : UserProfile
      {
         var _loc3_:Array = param1.§_-A1U§();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_.length)
         {
            if(_loc3_[_loc4_].getId() == param2)
            {
               return _loc3_[_loc4_];
            }
            _loc4_++;
         }
         return null;
      }
      
      public static function §_-Is§(param1:UserProfile) : int
      {
         var _loc3_:UserProfile = null;
         var _loc2_:int = 0;
         for each(_loc3_ in param1.§_-A1U§())
         {
            if(_loc3_.§_-H38§())
            {
               _loc2_++;
            }
         }
         return _loc2_;
      }
      
      public static function §_-F3A§(param1:UserProfile, param2:Boolean) : Vector.<UserProfile>
      {
         var _loc4_:UserProfile = null;
         var _loc3_:Vector.<UserProfile> = new Vector.<UserProfile>(0);
         for each(_loc4_ in param1.§_-A1U§())
         {
            if(param2 == _loc4_.§_-H38§())
            {
               _loc3_.push(_loc4_);
            }
         }
         return _loc3_;
      }
      
      public static function §_-I5§(param1:Array, param2:UserProfile) : Array
      {
         var _loc3_:int = param2.getId();
         var _loc4_:* = int(param1.length - 1);
         while(_loc4_ > -1)
         {
            if(UserProfile(param1[_loc4_]).getId() == _loc3_)
            {
               param1.splice(_loc4_,1);
               break;
            }
            _loc4_--;
         }
         return param1;
      }
      
      public static function §_-91k§(param1:UserProfile) : Vector.<int>
      {
         var _loc3_:UserProfile = null;
         var _loc2_:Vector.<int> = new Vector.<int>(0);
         for each(_loc3_ in param1.§_-A1U§())
         {
            _loc2_.push(_loc3_.getId());
         }
         return _loc2_;
      }
      
      public static function §_-B1c§() : Boolean
      {
         return §_-11g§(Application.userProfile).length < §_-m4§(Application.userProfile);
      }
      
      public static function §_-11g§(param1:UserProfile) : Vector.<UserProfile>
      {
         var _loc3_:UserProfile = null;
         var _loc2_:Vector.<UserProfile> = new Vector.<UserProfile>(0);
         for each(_loc3_ in param1.§_-A1U§())
         {
            if(§_-Q19§(_loc3_.§_-t2I§()))
            {
               _loc2_.push(_loc3_);
            }
         }
         return _loc2_;
      }
      
      public static function §_-m4§(param1:UserProfile) : int
      {
         return 1;
      }
      
      public static function §_-Q19§(param1:int) : Boolean
      {
         return param1 == §_-yA§.§_-uF§ || param1 == §_-yA§.§_-02F§ || param1 == §_-yA§.§_-n2Y§;
      }
      
      public static function §_-Hd§(param1:int) : Boolean
      {
         return param1 == §_-yA§.§_-02F§ || param1 == §_-yA§.§_-n2Y§;
      }
      
      public static function §_-Ay§(param1:int, param2:UserProfile, param3:uint) : void
      {
         if(param1 == §_-yA§.§_-e2e§)
         {
            if(param2.getLevel() != param3)
            {
               param2.§_-N1n§(param3 + 1);
               param2.build.§_-D1X§(param3);
            }
         }
      }
      
      public static function §_-k1k§(param1:UserProfile) : void
      {
         var _loc3_:UserProfile = null;
         var _loc2_:int = param1.getLevel();
         for each(_loc3_ in param1.§_-A1U§())
         {
            §_-Ay§(_loc3_.§_-t2I§(),_loc3_,_loc2_);
         }
      }
   }
}

