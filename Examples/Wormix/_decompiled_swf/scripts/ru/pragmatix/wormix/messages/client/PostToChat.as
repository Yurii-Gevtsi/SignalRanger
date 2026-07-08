package ru.pragmatix.wormix.messages.client
{
   import §_-A1K§.§_-7u§;
   import §_-D3A§.§_-TZ§;
   import §_-b1F§.*;
   import §_-k1u§.§_-Vr§;
   import §_-l1g§.§_-L3Q§;
   import §_-u13§.MAC;
   import com.hurlant.crypto.*;
   import com.hurlant.math.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class PostToChat
   {
      
      public var action:int = 0;
      
      public var profileName:String;
      
      public var message:String;
      
      public function PostToChat(param1:String = "", param2:String = "")
      {
         super();
         this.profileName = param1 || "";
         this.message = param2;
      }
   }
}

