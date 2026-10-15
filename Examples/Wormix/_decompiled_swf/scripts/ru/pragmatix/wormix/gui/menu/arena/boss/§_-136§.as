package ru.pragmatix.wormix.gui.menu.arena.boss
{
   import §_-RE§.§_-D3v§;
   import §_-S§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-Z1L§.§_-A2V§;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starlingbuilder.engine.tween.WormixTweenBuilder;
   
   public class §_-136§ extends §_-K1P§
   {
      
      protected static var §_-LD§:int = -1;
      
      public function §_-136§()
      {
         super();
      }
      
      override protected function §_-A3E§() : §_-D3v§
      {
         return §_-N1V§.§_-I39§.§_-N1i§();
      }
      
      override protected function §_-I1p§() : int
      {
         return §_-LD§;
      }
      
      override protected function §_-m7§(param1:int) : void
      {
         §_-LD§ = param1;
      }
   }
}

