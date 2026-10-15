package ru.pragmatix.wormix.messages.clans.request.edit
{
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import flash.display.DisplayObjectContainer;
   import flash.display.SimpleButton;
   import flash.utils.clearTimeout;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.§_-11V§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class ChangeClanJoinRatingRequest
   {
      
      public var joinRating:int;
      
      public function ChangeClanJoinRatingRequest(param1:int)
      {
         super();
         this.joinRating = param1;
      }
   }
}

