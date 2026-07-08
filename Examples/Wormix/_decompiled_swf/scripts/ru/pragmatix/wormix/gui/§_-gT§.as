package ru.pragmatix.wormix.gui
{
   import §_-A1K§.§_-TA§;
   import §_-O2L§.§_-C3d§;
   import §_-O2L§.§_-X1T§;
   import §_-RE§.§_-52p§;
   import §_-U1i§.§_-O2i§;
   import §_-b1F§.§_-v2Q§;
   import §_-g2r§.§_-03A§;
   import com.greensock.*;
   import com.greensock.core.*;
   import flash.ui.Keyboard;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.BossTypesTab;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.RejectBattleOffer;
   import starling.events.Event;
   
   public class §_-gT§
   {
      
      private static var §_-X1g§:Dictionary;
      
      public function §_-gT§()
      {
         super();
      }
      
      public static function init() : void
      {
         §_-X1g§ = new Dictionary();
         §_-X1g§[Keyboard.A] = Command.Left;
         §_-X1g§[Keyboard.D] = Command.Right;
         §_-X1g§[Keyboard.W] = Command.Up;
         §_-X1g§[Keyboard.S] = Command.Down;
         §_-X1g§[Keyboard.F] = Command.Jump;
         §_-X1g§[Keyboard.R] = Command.BackJump;
         §_-X1g§[Keyboard.SHIFT] = Command.Alt_Shift;
         §_-X1g§[Keyboard.I] = Command.Bag;
         §_-X1g§[Keyboard.SPACE] = Command.Shoot;
         §_-X1g§[Keyboard.LEFT] = Command.Left;
         §_-X1g§[Keyboard.RIGHT] = Command.Right;
         §_-X1g§[Keyboard.UP] = Command.Up;
         §_-X1g§[Keyboard.DOWN] = Command.Down;
         §_-X1g§[Keyboard.ENTER] = Command.Jump;
         §_-X1g§[Keyboard.BACKSPACE] = Command.BackJump;
         §_-X1g§[Keyboard.Q] = Command.Bag;
         §_-X1g§[§_-Ks§.BAG_OPEN_0] = Command.Bag;
         §_-X1g§[Keyboard.ESCAPE] = Command.Escape;
         §_-X1g§[§_-Ks§.§_-i2a§] = Command.Use;
      }
      
      public static function §_-12B§(param1:uint) : Command
      {
         return §_-X1g§[param1] as Command;
      }
   }
}

