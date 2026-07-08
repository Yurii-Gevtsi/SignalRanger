package ru.pragmatix.wormix.gui.menu
{
   import §_-52L§.§_-u2x§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-ou§;
   import §_-62§.§_-zF§;
   import §_-A2U§.§_-A23§;
   import §_-Af§.*;
   import §_-DJ§.§_-J2l§;
   import §_-Ly§.§_-81L§;
   import §_-Vx§.§_-X1N§;
   import §_-r1C§.§_-h2B§;
   import §_-r1C§.§_-t1R§;
   import §_-t10§.§_-B29§;
   import §_-t10§.§_-S1b§;
   import §_-t10§.§_-W2z§;
   import §_-t10§.§_-xK§;
   import com.greensock.loading.*;
   import feathers.controls.Panel;
   import feathers.controls.ToggleButton;
   import flash.display.SimpleButton;
   import flash.system.ApplicationDomain;
   import flash.system.LoaderContext;
   import flash.system.SecurityDomain;
   import org.swiftsuspenders.§_-T1p§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.PostClanNewsRequest;
   import ru.pragmatix.wormix.messages.clans.response.PostClanNewsResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.platform.§_-WK§;
   import ru.pragmatix.wormix.utils.§_-YD§;
   import starling.display.DisplayObject;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   
   public class §_-738§ extends ToggleButton
   {
      
      protected var §_-F11§:§_-t1R§;
      
      public function §_-738§()
      {
         super();
         this.§_-Y1L§(§_-81L§.§_-o1Z§);
         addEventListener(Event.TRIGGERED,this.playSound);
      }
      
      private function playSound(param1:Event) : void
      {
         if(this.§_-F11§)
         {
            §_-h2B§.play(this.§_-F11§);
         }
      }
      
      public function §_-Y1L§(param1:§_-t1R§) : void
      {
         this.§_-F11§ = param1;
      }
      
      override public function dispose() : void
      {
         removeEventListener(Event.TRIGGERED,this.playSound);
         this.§_-F11§ = null;
      }
   }
}

