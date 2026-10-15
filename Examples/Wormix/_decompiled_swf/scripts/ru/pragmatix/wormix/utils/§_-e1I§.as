package ru.pragmatix.wormix.utils
{
   import §_-A2U§.§_-A23§;
   import §_-Ty§.§_-83N§;
   import §_-Ty§.§_-K3a§;
   import §_-Ty§.§_-U2T§;
   import §_-Ty§.§_-n1U§;
   import §_-Ty§.§_-o1U§;
   import §_-Y1Q§.§_-vx§;
   import §_-YF§.§_-Zi§;
   import §_-d26§.*;
   import §_-hO§.§_-03k§;
   import §_-l1K§.*;
   import §_-nZ§.*;
   import §_-q26§.*;
   import flash.events.Event;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.IncomingClanMessage;
   import ru.pragmatix.wormix.messages.clans.main.server.InviteFriendToClan;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.PostToClanChatResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class §_-e1I§ extends EventDispatcher
   {
      
      protected var _delay:uint;
      
      protected var _repeatCount:uint;
      
      protected var _running:Boolean = false;
      
      protected var frames:uint = 0;
      
      private var §_-W27§:String;
      
      public function §_-e1I§(param1:String, param2:uint, param3:uint = 0)
      {
         super();
         this.§_-W27§ = param1;
         this._delay = param2;
         this._repeatCount = param3;
      }
      
      public function start(param1:int = 0) : void
      {
         if(!this._running)
         {
            this._running = true;
            §_-X1j§.§_-12n§.scene.addEventListener(this.§_-W27§,this.onFrame);
         }
      }
      
      public function stop() : void
      {
         if(this._running)
         {
            this._running = false;
            if(§_-X1j§.§_-12n§.scene)
            {
               §_-X1j§.§_-12n§.scene.removeEventListener(this.§_-W27§,this.onFrame);
            }
         }
      }
      
      private function onFrame(param1:starling.events.Event) : void
      {
         if(§_-X1j§.§_-12n§.scene)
         {
            ++this.frames;
            if(this.frames % this._delay == 0)
            {
               dispatchEvent(new §_-03k§(Math.round(this.frames / this._delay),§_-03k§.§_-836§));
            }
            if(this._delay * this._repeatCount == this.frames)
            {
               this.stop();
               dispatchEvent(new §_-03k§(this._repeatCount,§_-03k§.§_-cE§));
            }
         }
         else
         {
            this.stop();
         }
      }
      
      public function get delay() : uint
      {
         return this._delay;
      }
      
      public function get repeatCount() : uint
      {
         return this._repeatCount;
      }
      
      public function get running() : Boolean
      {
         return this._running;
      }
   }
}

