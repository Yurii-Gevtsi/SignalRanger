package ru.pragmatix.wormix.controller
{
   import §_-21p§.§_-4w§;
   import §_-31N§.§_-42G§;
   import §_-51W§.§_-SN§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-82l§.§_-1I§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-C1i§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-E29§.§_-32G§;
   import §_-H3§.*;
   import §_-J31§.§_-R1h§;
   import §_-N8§.§_-o2w§;
   import §_-OJ§.§_-L3B§;
   import §_-OJ§.§_-S1v§;
   import §_-P1B§.§_-b21§;
   import §_-P1B§.§_-bb§;
   import §_-SP§.§_-M4§;
   import §_-Tm§.§_-K1I§;
   import §_-Z1w§.§_-s1x§;
   import §_-b1F§.§_-G4§;
   import §_-fo§.§_-hH§;
   import §_-fo§.§_-r5§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-p18§.§_-h1b§;
   import §_-rM§.§_-61j§;
   import §_-x2Q§.§_-dU§;
   import §_-y1s§.§_-P9§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import §_-z1g§.§_-y1S§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import config.§_-vR§;
   import feathers.core.ITextRenderer;
   import flash.display.*;
   import flash.events.*;
   import flash.geom.Rectangle;
   import flash.utils.*;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.response.photo.PhotoSaveResponse;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.analytics.Analytics;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.PostToChat;
   import ru.pragmatix.wormix.messages.client.UpgradeWeapon;
   import ru.pragmatix.wormix.messages.server.OpenDepositResult;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.DepositStructure;
   import ru.pragmatix.wormix.messages.utils.Result;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.platform.§_-WK§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class §_-v2§ extends EventDispatcher
   {
      
      public static const §_-RM§:uint = 4;
      
      public function §_-v2§()
      {
         super();
      }
      
      public static function §_-u2w§() : void
      {
         Application.§_-a1k§(false);
         §_-X1j§.§_-12n§.§_-IT§(§_-N1V§.§_-I39§.§_-X1P§().§_-q29§[0]);
      }
      
      public static function §_-e2K§(param1:Function, param2:Function) : void
      {
         §_-fH§.§_-F3p§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_WELCOME),§_-s2a§.§_-K3t§(§_-s2a§.ENTER_TUTORIAL_QUESTION),§_-fH§.§_-H2d§,param1,param2).show();
      }
      
      public static function §_-72F§() : Boolean
      {
         var _loc1_:Boolean = false;
         switch(§_-vR§.§_-I1d§)
         {
            case SocialType.MOBILE_IOS.name:
            case SocialType.MOBILE_ANDROID.name:
               _loc1_ = Application.userProfile.getLevel() == 1 && Application.userProfile.§_-p25§() == 0 && §_-N1V§.§_-I39§.§_-X1P§().§_-rm§ == 0;
         }
         return _loc1_;
      }
      
      public static function §_-63e§() : Boolean
      {
         return Application.userProfile.getLevel() <= §_-v2§.§_-RM§;
      }
      
      public function §_-G2T§() : Array
      {
         var _loc1_:uint = uint(Application.userProfile.getLevel());
         var _loc2_:§_-32G§ = §_-N1V§.§_-I39§.§_-X1P§();
         var _loc3_:Vector.<§_-y1S§> = _loc2_.§_-q29§;
         var _loc4_:int = _loc2_.§_-rm§;
         if(_loc1_ <= §_-RM§)
         {
            if(_loc1_ == 1 && Application.userProfile.§_-p25§() == 0 && _loc4_ == 0)
            {
               return [_loc3_[0],null,null];
            }
            if(_loc4_ == _loc3_[0].getId() || _loc4_ == 0)
            {
               return [_loc3_[1]];
            }
            if(_loc4_ == _loc3_[1].getId())
            {
               return [_loc3_[2]];
            }
            if(_loc4_ == _loc3_[2].getId())
            {
               return [_loc3_[3]];
            }
            if(_loc4_ == _loc3_[3].getId())
            {
               return [_loc3_[4]];
            }
         }
         return [];
      }
      
      public function §_-63j§() : void
      {
         var libs:Vector.<String> = null;
         if(!§_-r5§.§_-739§("LearningMission1_CentralPart"))
         {
            libs = new <String>["learningMissionsObjects"];
            §_-r5§.§_-GH§(libs,this.§_-G3k§,function(param1:§_-hH§):void
            {
               throw new Error(param1.msg);
            });
         }
         else
         {
            this.§_-G3k§();
         }
      }
      
      private function §_-G3k§() : void
      {
         dispatchEvent(new flash.events.Event(flash.events.Event.COMPLETE,true));
      }
   }
}

