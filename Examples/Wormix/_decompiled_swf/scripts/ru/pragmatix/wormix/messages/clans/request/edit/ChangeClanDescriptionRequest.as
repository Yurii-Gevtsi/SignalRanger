package ru.pragmatix.wormix.messages.clans.request.edit
{
   import §_-A2U§.§_-A23§;
   import §_-R2I§.§_-u2V§;
   import §_-ZP§.§_-b23§;
   import §_-l1K§.*;
   import §_-z1g§.§_-82X§;
   import com.hurlant.util.*;
   import feathers.data.ListCollection;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import starling.display.DisplayObject;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ChangeClanDescriptionRequest
   {
      
      public var description:String;
      
      public var fromTreas:Boolean;
      
      public function ChangeClanDescriptionRequest(param1:String, param2:Boolean)
      {
         super();
         this.description = param1;
         this.fromTreas = param2;
      }
   }
}

