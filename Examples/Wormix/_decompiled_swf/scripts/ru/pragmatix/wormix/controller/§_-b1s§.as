package ru.pragmatix.wormix.controller
{
   import §_-21p§.§_-4w§;
   import §_-23P§.§_-V2T§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-zF§;
   import §_-63M§.§_-F3f§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-E19§;
   import §_-DJ§.*;
   import §_-J31§.§_-R1h§;
   import §_-J31§.§_-e2J§;
   import §_-J3L§.§_-K2i§;
   import §_-Ly§.§_-81L§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-P2i§.§_-b7§;
   import §_-S1N§.§_-R1U§;
   import §_-Y1O§.§_-yA§;
   import §_-Z1w§.§_-s1x§;
   import §_-k1u§.§_-by§;
   import §_-l1g§.§_-L3Q§;
   import §_-p18§.§_-vd§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-u2J§.§_-T1K§;
   import §_-w2i§.§_-Ia§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.loading.core.LoaderItem;
   import dragonBones.animation.WorldClock;
   import dragonBones.starling.StarlingFactory;
   import dragonBones.starling.StarlingTextureAtlasData;
   import dragonBones.textures.TextureAtlasData;
   import flash.events.Event;
   import flash.system.Capabilities;
   import flash.system.System;
   import flash.text.Font;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.messages.server.StuffExpired;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   import starling.utils.MathUtil;
   
   public class §_-b1s§
   {
      
      public function §_-b1s§()
      {
         super();
         §_-J2g§.§_-o1R§(StuffExpired,this.§_-137§);
      }
      
      private static function §_-EA§(param1:int) : Boolean
      {
         return param1 >= 1000 && param1 < 2000;
      }
      
      private static function §_-31o§(param1:int) : UserProfile
      {
         var _loc2_:UserProfile = null;
         for each(_loc2_ in Application.userProfile.§_-A1U§())
         {
            if(!§_-71v§.§_-Q19§(_loc2_.§_-t2I§()) && (Boolean(_loc2_.getHat()) && Boolean(_loc2_.getHat().getId() == param1) || Boolean(_loc2_.§_-32q§()) && Boolean(_loc2_.§_-32q§().getId() == param1)))
            {
               return _loc2_;
            }
         }
         return null;
      }
      
      public function §_-137§(param1:§_-63i§) : void
      {
         var _loc2_:StuffExpired = null;
         var _loc7_:int = 0;
         var _loc8_:§_-W1r§ = null;
         var _loc9_:§_-by§ = null;
         var _loc10_:UserProfile = null;
         var _loc11_:§_-zF§ = null;
         var _loc12_:§_-T1y§ = null;
         var _loc13_:String = null;
         _loc2_ = param1.message as StuffExpired;
         var _loc3_:Array = [];
         var _loc4_:uint = _loc2_.stuff.length;
         var _loc5_:uint = 0;
         while(_loc5_ < _loc4_)
         {
            _loc7_ = int(_loc2_.stuff[_loc5_]);
            _loc8_ = §_-G1p§.§_-d0§(_loc7_,Application.userProfile.§_-P2e§());
            _loc9_ = Application.userProfile.§_-XT§() as §_-by§;
            _loc3_.push(§_-s2a§.§_-K3t§(_loc8_.getName()));
            _loc9_.§_-q2X§(_loc7_);
            _loc10_ = §_-31o§(_loc7_);
            if(_loc10_)
            {
               if(_loc10_.§_-t2I§() == §_-yA§.§_-e2e§)
               {
                  §_-V2T§.§_-N1o§(_loc10_);
               }
               else if(_loc10_.getId() == Application.userProfile.getId())
               {
                  if(§_-EA§(_loc7_))
                  {
                     this.§_-G3X§(_loc2_.hat,true);
                     _loc11_ = _loc2_.hat != 0 ? §_-G1p§.§_-12J§(_loc2_.hat,_loc10_.§_-P2e§()) : null;
                     _loc10_.§_-L§(_loc11_);
                  }
                  else
                  {
                     this.§_-G3X§(_loc2_.kit,false);
                     _loc12_ = _loc2_.kit != 0 ? §_-G1p§.§_-93G§(_loc2_.kit) : null;
                     _loc10_.§_-aV§(_loc12_);
                  }
               }
               else if(§_-EA§(_loc7_))
               {
                  _loc10_.§_-L§(null);
               }
               else
               {
                  _loc10_.§_-aV§(null);
               }
            }
            _loc5_++;
         }
         var _loc6_:§_-r1d§ = Application.§_-SH§;
         if(Application.§_-E1k§.§_-do§() && _loc6_.§_-x2k§)
         {
            Application.§_-E1k§.§_-A1J§();
         }
         if(_loc3_.length > 0)
         {
            _loc13_ = §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.EQUIP_EXPIRED),{"equipList":_loc3_.join(", ")});
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),_loc13_,§_-fH§.§_-1B§);
         }
      }
      
      private function §_-G3X§(param1:int, param2:Boolean = true) : void
      {
         var _loc3_:UserProfile = §_-31o§(param1);
         if(_loc3_)
         {
            if(param2)
            {
               _loc3_.§_-L§(null);
            }
            else
            {
               _loc3_.§_-aV§(null);
            }
            if(_loc3_.§_-t2I§() == §_-yA§.§_-e2e§)
            {
               §_-V2T§.§_-N1o§(_loc3_);
            }
         }
      }
   }
}

