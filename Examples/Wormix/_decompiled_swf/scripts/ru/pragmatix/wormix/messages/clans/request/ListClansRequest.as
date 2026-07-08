package ru.pragmatix.wormix.messages.clans.request
{
   import §_-l1g§.§_-T2N§;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.FeathersControl;
   import feathers.data.ListCollection;
   import feathers.themes.FontSize;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import starling.events.Event;
   
   public class ListClansRequest
   {
      
      public var searchPhrase:String;
      
      public var reviewStates:Vector.<int> = new Vector.<int>();
      
      public var offset:int;
      
      public var limit:int;
      
      public function ListClansRequest(param1:String)
      {
         super();
         this.searchPhrase = param1;
      }
   }
}

