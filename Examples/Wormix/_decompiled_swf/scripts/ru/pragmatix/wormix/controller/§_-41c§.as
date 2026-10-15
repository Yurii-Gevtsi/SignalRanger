package ru.pragmatix.wormix.controller
{
   import §_-23P§.§_-V2T§;
   import §_-23P§.§_-u1z§;
   import §_-2U§.*;
   import §_-52L§.§_-Pk§;
   import §_-52L§.§_-u2x§;
   import §_-5P§.*;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-zF§;
   import §_-A1K§.§_-TA§;
   import §_-B2z§.§_-63i§;
   import §_-CR§.§_-S2o§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-I1l§.*;
   import §_-R2S§.*;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-TF§.*;
   import §_-Ta§.*;
   import §_-Vu§.MD5;
   import §_-Y1O§.§_-42h§;
   import §_-Y1O§.§_-71D§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-yA§;
   import §_-hO§.§_-729§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-p14§.*;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-z1g§.§_-y1S§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.BigInteger;
   import config.§_-vR§;
   import feathers.layout.HorizontalLayout;
   import flash.display.*;
   import flash.events.*;
   import flash.utils.*;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.Disconnect;
   import ru.pragmatix.wormix.messages.client.MobileLogin;
   import ru.pragmatix.wormix.messages.client.SelectStuff;
   import ru.pragmatix.wormix.messages.server.SelectStuffResult;
   import ru.pragmatix.wormix.messages.structures.SelectStuffResultStructure;
   import ru.pragmatix.wormix.messages.structures.SelectStuffStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.social.*;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.ScaleMode;
   
   public class §_-41c§
   {
      
      public function §_-41c§()
      {
         super();
      }
      
      public static function §_-u18§(param1:UserProfile) : Boolean
      {
         return Boolean(param1) && §_-yA§.§_-u18§(param1);
      }
      
      public static function §_-H2p§(param1:UserProfile) : §_-u1z§
      {
         return §_-u18§(param1) ? §_-V2T§.§_-H2p§(param1.getId()) : null;
      }
      
      public function save(param1:Function, param2:Vector.<UserProfile>) : void
      {
         var _loc4_:UserProfile = null;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:§_-u1z§ = null;
         var _loc8_:SelectStuffStructure = null;
         var _loc3_:SelectStuff = new SelectStuff();
         for each(_loc4_ in param2)
         {
            if(Boolean(_loc4_) && !§_-71v§.§_-Q19§(_loc4_.§_-t2I§()))
            {
               _loc5_ = 0;
               _loc6_ = 0;
               _loc7_ = §_-H2p§(_loc4_);
               if(_loc4_.getHat())
               {
                  _loc5_ = _loc4_.getHat().getId();
               }
               if(_loc4_.§_-32q§())
               {
                  _loc6_ = _loc4_.§_-32q§().getId();
               }
               if(Boolean(_loc7_) && _loc7_.§_-PG§() == _loc5_)
               {
                  _loc5_ = 0;
               }
               if(Boolean(_loc7_) && _loc7_.§_-u1E§() == _loc6_)
               {
                  _loc6_ = 0;
               }
               _loc8_ = new SelectStuffStructure(_loc4_.getId(),_loc5_,_loc6_);
               _loc3_.selectStuffs.push(_loc8_);
            }
         }
         §_-d21§.§_-vH§(§_-J2g§.§_-i1N§,_loc3_,SelectStuffResult,this.§_-83Q§,this.§_-w2G§,param1,param2);
      }
      
      private function §_-83Q§(param1:§_-63i§, param2:Function, param3:Vector.<UserProfile>) : void
      {
         var _loc6_:UserProfile = null;
         var _loc7_:SelectStuffResultStructure = null;
         var _loc4_:SelectStuffResult = SelectStuffResult(param1.message);
         var _loc5_:String = "";
         for each(_loc7_ in _loc4_.selectStuffResults)
         {
            _loc6_ = this.§_-s1T§(param3,_loc7_.profileId);
            if(_loc7_.resultHat == SelectStuffResultStructure.SUCCESS)
            {
               _loc6_.§_-L§(§_-G1p§.§_-12J§(_loc7_.hatId,_loc6_.§_-P2e§()));
            }
            else
            {
               _loc5_ += _loc6_.getCharName() + ": шапка не сохранена!\n";
            }
            if(_loc7_.resultArtifact == SelectStuffResultStructure.SUCCESS)
            {
               _loc6_.§_-aV§(§_-G1p§.§_-93G§(_loc7_.artifactId));
            }
            else
            {
               _loc5_ += _loc6_.getCharName() + ": артефакт не сохранен!\n";
            }
         }
         if(_loc5_)
         {
            §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),_loc5_);
         }
         else
         {
            Application.userProfile.dispatchEventWith(flash.events.Event.CHANGE);
         }
         this.executeCallbackWithResult(param2,_loc5_ == "");
      }
      
      private function §_-w2G§(param1:flash.events.Event, param2:Function, param3:Vector.<UserProfile>) : void
      {
         §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.ERROR_SAVING));
         this.executeCallbackWithResult(param2,false);
      }
      
      private function executeCallbackWithResult(param1:Function, param2:Boolean) : void
      {
         §_-J2l§.§_-R25§.hide();
         if(param1 != null)
         {
            param1(param2);
         }
      }
      
      public function §_-F1l§(param1:UserProfile) : void
      {
         var _loc2_:§_-u1z§ = null;
         var _loc3_:§_-zF§ = null;
         if(param1)
         {
            if(§_-yA§.§_-u18§(param1))
            {
               _loc2_ = §_-V2T§.§_-H2p§(param1.getId());
               _loc3_ = §_-G1p§.§_-12J§(_loc2_.§_-PG§(),param1.§_-P2e§());
               param1.§_-L§(_loc3_);
               param1.§_-aV§(§_-G1p§.§_-93G§(_loc2_.§_-u1E§()));
            }
            else if(!§_-71v§.§_-Q19§(param1.§_-t2I§()))
            {
               param1.§_-L§(null);
               param1.§_-aV§(null);
            }
         }
      }
      
      public function §_-32I§(param1:§_-Oi§, param2:Vector.<UserProfile>) : Boolean
      {
         var _loc5_:UserProfile = null;
         var _loc6_:§_-u1z§ = null;
         var _loc7_:Boolean = false;
         var _loc3_:Boolean = false;
         var _loc4_:int = param1.getId();
         for each(_loc5_ in param2)
         {
            if(_loc5_)
            {
               if(Boolean(_loc5_.getHat()) && Boolean(_loc4_ == _loc5_.getHat().getId()) || Boolean(_loc5_.§_-32q§()) && Boolean(_loc4_ == _loc5_.§_-32q§().getId()))
               {
                  _loc6_ = §_-H2p§(_loc5_);
                  _loc7_ = _loc6_ ? _loc6_.§_-PG§() == _loc4_ || _loc6_.§_-u1E§() == _loc4_ : false;
                  if(!_loc7_)
                  {
                     _loc3_ = true;
                     break;
                  }
               }
            }
         }
         return _loc3_;
      }
      
      private function §_-s1T§(param1:Vector.<UserProfile>, param2:int) : UserProfile
      {
         var _loc3_:UserProfile = null;
         for each(_loc3_ in param1)
         {
            if(Boolean(_loc3_) && _loc3_.getId() == param2)
            {
               return _loc3_;
            }
         }
         return null;
      }
   }
}

