package ru.pragmatix.wormix.pvp.messages.client
{
   import §_-B1y§.*;
   import §_-H16§.*;
   import §_-k2s§.§_-Go§;
   import §_-k2s§.§_-fG§;
   import §_-p14§.§_-U1D§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.wormix.messages.client.SteamProductsInfoRequest;
   import ru.pragmatix.wormix.messages.server.SteamProductsInfoResponse;
   
   public class PvpActionEx extends PvpCommand
   {
      
      public var firstFrame:uint;
      
      public var lastFrame:uint;
      
      public var actionsByFrame:Object;
      
      public function PvpActionEx(param1:uint, param2:uint, param3:§_-U1D§)
      {
         super();
         this.firstFrame = param1;
         this.lastFrame = param2;
         this.actionsByFrame = param3.actions;
      }
   }
}

