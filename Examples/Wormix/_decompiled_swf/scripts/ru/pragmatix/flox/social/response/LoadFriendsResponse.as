package ru.pragmatix.flox.social.response
{
   import §_-H2g§.§_-di§;
   import §_-ZP§.§_-YG§;
   import §_-ZP§.§_-w1J§;
   import §_-l2r§.§_-K1t§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.social.utils.FacebookTracking;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import starling.display.DisplayObject;
   
   public class LoadFriendsResponse extends SocialResponse
   {
      
      public var uids:Array = [];
      
      public function LoadFriendsResponse()
      {
         super(null,"LOAD.FRIENDS");
      }
   }
}

