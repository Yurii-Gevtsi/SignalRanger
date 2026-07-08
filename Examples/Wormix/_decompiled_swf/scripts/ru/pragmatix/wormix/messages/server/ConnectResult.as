package ru.pragmatix.wormix.messages.server
{
   import §_-21p§.§_-4w§;
   import §_-62§.§_-L1r§;
   import §_-DJ§.§_-J2l§;
   import §_-T2h§.*;
   import §_-d26§.§_-c2E§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import com.distriqt.extension.pushnotifications.events.RegistrationEvent;
   import flash.geom.Point;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-qB§;
   import ru.pragmatix.wormix.messages.client.Link;
   import ru.pragmatix.wormix.messages.structures.PreCalculatedPoints;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.PvpCommand;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   
   public class ConnectResult
   {
      
      public var userProfileStructures:Array;
      
      public var onlineFriends:int;
      
      public var friends:int;
      
      public var connectAwardGranted:Boolean;
      
      public var isSecure:Boolean;
      
      public function ConnectResult()
      {
         super();
      }
      
      public function toString() : String
      {
         var _loc1_:String = "";
         var _loc2_:int = 0;
         while(_loc2_ < this.userProfileStructures.length)
         {
            _loc1_ += "struct. " + _loc2_ + ": " + String(this.userProfileStructures[_loc2_]);
            _loc2_++;
         }
         return "ConnectResult{onlineFriends=" + this.onlineFriends + ",friends=" + this.friends + ",connectAwardGranted=" + this.connectAwardGranted + ",isSecure=" + this.isSecure + ",userProfileStructures.length=" + this.userProfileStructures.length + "[" + _loc1_ + "]}";
      }
   }
}

