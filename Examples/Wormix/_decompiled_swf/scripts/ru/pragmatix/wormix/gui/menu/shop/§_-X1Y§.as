package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-DJ§.§_-T22§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import starling.events.Event;
   
   public interface §_-X1Y§ extends §_-T22§
   {
      
      function update(param1:int) : void;
      
      function get isSubpanelOpened() : Boolean;
      
      function §_-52u§(param1:Event = null) : void;
      
      function show() : void;
      
      function §_-h1C§() : Boolean;
      
      function hide() : void;
      
      function §_-oG§(param1:UserProfile) : void;
      
      function addEventListener(param1:String, param2:Function) : void;
      
      function removeEventListener(param1:String, param2:Function) : void;
      
      function dispose() : void;
   }
}

