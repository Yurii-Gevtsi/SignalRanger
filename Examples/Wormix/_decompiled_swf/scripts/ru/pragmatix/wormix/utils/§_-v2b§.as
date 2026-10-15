package ru.pragmatix.wormix.utils
{
   import §_-12a§.§_-E1Q§;
   import §_-12a§.§_-G2l§;
   import §_-62§.§_-W1r§;
   import §_-B1y§.§_-83E§;
   import §_-H2g§.§_-di§;
   import §_-TF§.§_-y2n§;
   import §_-Vu§.MD5;
   import §_-l1g§.§_-T2N§;
   import §_-q2b§.§_-S2L§;
   import §_-zI§.§_-X1q§;
   import feathers.data.ArrayCollection;
   import flash.events.EventDispatcher;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.utils.id.GetAuthKeyEvent;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.flox.social.utils.storage.StorageType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.§_-D2t§;
   import ru.pragmatix.wormix.model.§_-kQ§;
   import ru.pragmatix.wormix.model.items.craft.*;
   import starling.events.Event;
   
   public class §_-v2b§
   {
      
      public function §_-v2b§()
      {
         super();
      }
      
      public static function §_-AO§(param1:IPlatformAccountController, param2:Function) : void
      {
         var ac:IPlatformAccountController = param1;
         var onResponse:Function = param2;
         §_-83E§.single(ac as EventDispatcher,GetAuthKeyEvent.GET_AUTH_KEY,function onGetAuthKey(param1:GetAuthKeyEvent, param2:Function):void
         {
            param2(param1);
         },null,[onResponse],true);
         ac.refreshAuthKey();
      }
      
      public static function §_-j1T§() : void
      {
         var _loc1_:IPlatformAccountController = §_-X1j§.§_-It§;
         var _loc2_:Boolean = _loc1_.getPlatformType().equals(SocialType.MOBILE_IOS);
         if(!_loc2_)
         {
            return;
         }
         var _loc3_:§_-S2L§ = §_-X1j§.§_-D1T§;
         if(!_loc3_.isAvailable || _loc3_.getUserId() == _loc1_.getUserId())
         {
            return;
         }
         if(_loc1_.isCreatedId)
         {
            if(_loc3_.getUserId())
            {
               _loc1_.setUserIdToStore(_loc3_.getUserId(),StorageType.LOCAL_ENCRYPTED);
            }
            else
            {
               _loc3_.setUserId(_loc1_.getUserId());
            }
         }
         else if(_loc1_.getUserId())
         {
            if(!_loc3_.getUserId())
            {
               _loc3_.setUserId(_loc1_.getUserId());
            }
         }
      }
   }
}

