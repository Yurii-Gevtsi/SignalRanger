package ru.pragmatix.flox.social.controller.mobile
{
   import §_-5Y§.§_-h2c§;
   import §_-K3q§.§_-L3A§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-YF§.§_-q2v§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-b1F§.§_-G4§;
   import §_-kP§.*;
   import §_-q26§.§_-Z25§;
   import §_-zI§.§_-A1s§;
   import com.greensock.TweenMax;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import config.§_-V2w§;
   import feathers.controls.List;
   import flash.geom.Point;
   import flash.net.Socket;
   import flash.utils.IDataOutput;
   import flash.utils.setTimeout;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.structures.PreCalculatedPoints;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.*;
   import starling.display.DisplayObjectContainer;
   
   use namespace bi_internal;
   
   public class EmptyMobileSocialController extends BaseMobileSocialController
   {
      
      private var isInited:Boolean = false;
      
      public function EmptyMobileSocialController(param1:SocialAppContext)
      {
         super(param1);
      }
      
      override public function init(param1:Function) : void
      {
         param1(new SocialResponse("init",this.getType().name,true));
      }
      
      override public function login(param1:Function) : void
      {
         var callback:Function = param1;
         setTimeout(function():void
         {
            callback(new SocialResponse("init",getType().name,false));
         },2000);
      }
      
      override public function logout() : void
      {
      }
      
      override public function isLoggedIn() : Boolean
      {
         return false;
      }
      
      override public function getType() : SocialType
      {
         return SocialType.NONE;
      }
   }
}

