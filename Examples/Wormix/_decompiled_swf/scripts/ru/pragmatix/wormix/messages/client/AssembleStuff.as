package ru.pragmatix.wormix.messages.client
{
   import §_-j22§.§_-5g§;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import org.gestouch.core.*;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.model.user.*;
   
   use namespace gestouch_internal;
   
   public class AssembleStuff
   {
      
      public var familyId:int;
      
      public var moneyType:int;
      
      public function AssembleStuff(param1:int, param2:int)
      {
         super();
         this.familyId = param1;
         this.moneyType = param2;
      }
   }
}

