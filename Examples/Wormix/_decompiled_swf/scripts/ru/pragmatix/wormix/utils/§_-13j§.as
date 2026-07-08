package ru.pragmatix.wormix.utils
{
   import §_-62§.§_-gr§;
   import §_-73I§.§_-q15§;
   import §_-Nq§.*;
   import §_-TF§.§_-q1j§;
   import feathers.core.ITextRenderer;
   import feathers.layout.RelativePosition;
   import flash.geom.Rectangle;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import starling.display.DisplayObject;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class §_-13j§ extends §_-e1I§
   {
      
      public function §_-13j§(param1:uint, param2:uint = 0)
      {
         super(Event.ENTER_FRAME,param1,param2);
      }
      
      public function restart(param1:int = -1) : void
      {
         _delay = param1 == -1 ? _delay : uint(param1);
         frames = 0;
         stop();
         start();
      }
   }
}

