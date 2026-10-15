package ru.pragmatix.wormix.controller
{
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-Ly§.§_-81L§;
   import §_-SP§.§_-M4§;
   import §_-Y1b§.§_-OP§;
   import §_-Z1K§.§_-q24§;
   import §_-a29§.*;
   import §_-aM§.*;
   import §_-j22§.§_-5g§;
   import §_-r1C§.§_-U2p§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-u1T§.*;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.plugins.*;
   import com.hurlant.crypto.tls.§_-A3§;
   import feathers.controls.ToggleButton;
   import flash.display.*;
   import flash.events.*;
   import flash.geom.*;
   import flash.utils.*;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.AddToGroup;
   import ru.pragmatix.wormix.messages.client.DistributePoints;
   import ru.pragmatix.wormix.messages.server.DistributePointsResult;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.*;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-Y18§
   {
      
      public function §_-Y18§()
      {
         super();
      }
      
      public function §_-Ua§(param1:uint, param2:uint) : void
      {
         var _loc3_:Object = new DistributePoints(param2,param1);
         §_-J2l§.§_-R25§.show(§_-d21§.§_-vH§,§_-J2g§.§_-i1N§,_loc3_,DistributePointsResult,this.§_-n1Q§,this.§_-3W§,param2,param1);
      }
      
      private function §_-n1Q§(param1:§_-63i§, param2:int, param3:int) : void
      {
         var _loc4_:String = null;
         §_-J2g§.§_-fy§(DistributePointsResult,this.§_-n1Q§);
         switch(DistributePointsResult(param1.message).result)
         {
            case DistributePointsResult.SUCCESS:
               Application.userProfile.§_-eQ§(Application.userProfile.§_-w1p§() + param3);
               Application.userProfile.§_-q1H§(Application.userProfile.§_-h2m§() + param2);
               Application.userProfile.dispatchEventWith(starling.events.Event.CHANGE);
               break;
            case DistributePointsResult.NOT_ENOUGH_POINTS:
               _loc4_ = §_-s2a§.§_-K3t§(§_-s2a§.ERROR_HACKING_ATTEMPT);
               break;
            default:
               _loc4_ = §_-s2a§.§_-K3t§(§_-s2a§.ERROR_SAVING);
         }
         §_-J2l§.§_-R25§.hide();
         if(_loc4_)
         {
            §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ERROR),_loc4_);
         }
      }
      
      private function §_-3W§(param1:Object, param2:int, param3:int) : void
      {
         §_-J2l§.§_-R25§.hide();
         §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ERROR),§_-s2a§.§_-K3t§(§_-s2a§.ERROR_SAVING));
      }
   }
}

