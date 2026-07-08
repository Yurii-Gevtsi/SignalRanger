package ru.pragmatix.wormix.gui.menu
{
   import §_-5Y§.*;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-a3§;
   import §_-73I§.*;
   import §_-73d§.§_-V19§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-E19§;
   import §_-CR§.§_-02k§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-J2l§;
   import §_-K35§.§_-S18§;
   import §_-Ly§.§_-81L§;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-U1i§.§_-O2i§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-v2Q§;
   import §_-fo§.§_-hH§;
   import §_-g2r§.§_-03A§;
   import §_-hO§.§_-729§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-lh§.§_-z2A§;
   import §_-r1C§.§_-h2B§;
   import §_-u1T§.*;
   import com.greensock.loading.LoaderStatus;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.util.§_-F2L§;
   import feathers.controls.LayoutGroup;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import flash.ui.GameInputDevice;
   import flash.ui.Keyboard;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.social.model.Gender;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controller.§_-Z2p§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.GamepadControlsHandler;
   import ru.pragmatix.wormix.controls.GamepadControlsMap;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.AmmoPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.pvp.messages.ex.RejectBattleOffer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.events.TouchEvent;
   import starling.utils.SystemUtil;
   
   public class GamepadSetupDialog extends §_-73y§
   {
      
      private static const CONTROLS_SENSITIVITY:Number = 0.8;
      
      private static const §_-N1v§:Array = [{
         "command":Command.Left,
         "message":§_-s2a§.CONTROL_LEFT
      },{
         "command":Command.Right,
         "message":§_-s2a§.CONTROL_RIGHT
      },{
         "command":Command.Up,
         "message":§_-s2a§.CONTROL_UP
      },{
         "command":Command.Down,
         "message":§_-s2a§.CONTROL_DOWN
      },{
         "command":Command.Shoot,
         "message":§_-s2a§.CONTROL_SHOOT
      },{
         "command":Command.Alt,
         "message":§_-s2a§.CONTROL_ALT
      },{
         "command":Command.Bag,
         "message":§_-s2a§.CONTROL_BAG
      },{
         "command":Command.Jump,
         "message":§_-s2a§.CONTROL_JUMP
      },{
         "command":Command.BackJump,
         "message":§_-s2a§.CONTROL_BACK_JUMP
      },{
         "command":Command.Escape,
         "message":§_-s2a§.CONTROL_ESCAPE
      }];
      
      private static const §_-wa§:Array = [Keyboard.BACK,Keyboard.BACKSPACE];
      
      private var §_-m1t§:ITextRenderer;
      
      private var §_-j§:ITextRenderer;
      
      private var deviceId:String;
      
      private var device:GameInputDevice;
      
      private var controlsMap:GamepadControlsMap;
      
      private var §_-53J§:int;
      
      private var handler:GamepadControlsHandler;
      
      private var §_-r1J§:String;
      
      public function GamepadSetupDialog(param1:String)
      {
         super(§_-YQ§.§_-qM§(§_-q2v§.§_-C3T§,§_-q2v§.§_-h2y§));
         this.deviceId = param1;
         this.§_-m1t§ = getChildByName("setup") as ITextRenderer;
         this.§_-m1t§.text = §_-s2a§.§_-K3t§(§_-s2a§.GAMEPAD_SETUP_TITLE);
         this.§_-j§ = getChildByName("control") as ITextRenderer;
         destroyOnRemove = true;
         outsideClosable = false;
      }
      
      override protected function §_-Q2j§(param1:Boolean = true) : void
      {
         super.§_-Q2j§(param1);
         this.activateDevice();
         this.§_-H1b§();
      }
      
      private function activateDevice() : void
      {
         this.device = §_-X1j§.§_-dr§.§_-sl§(this.deviceId);
         if(!this.device)
         {
            return;
         }
         this.handler = new GamepadControlsHandler(this.device);
         this.handler.addEventListener(GamepadControlsHandler.EVENT,this.onControlChange);
         this.handler.activate();
         §_-X1j§.§_-dr§.addEventListener(§_-Z2p§.§_-m3§,this.onDeviceRemoved);
      }
      
      private function deactivateDevice() : void
      {
         this.handler.removeEventListener(GamepadControlsHandler.EVENT,this.onControlChange);
         this.handler.dispose();
         this.handler = null;
         this.device = null;
         §_-X1j§.§_-dr§.removeEventListener(§_-Z2p§.§_-m3§,this.onDeviceRemoved);
      }
      
      private function onControlChange(param1:Event) : void
      {
         if(this.§_-53J§ >= §_-N1v§.length)
         {
            return;
         }
         if(§_-wa§.indexOf(param1.data.id) >= 0)
         {
            this.§_-X2O§();
            return;
         }
         if(param1.data.begin)
         {
            if(this.§_-r1J§)
            {
               return;
            }
            this.§_-r1J§ = param1.data.id;
            this.§_-13y§(param1.data.id,param1.data.sign);
         }
         else if(param1.data.id == this.§_-r1J§)
         {
            this.§_-r1J§ = null;
         }
      }
      
      private function §_-13y§(param1:String, param2:int) : void
      {
         var _loc3_:Object = null;
         if(!this.controlsMap.matchByParams(param1,param2))
         {
            _loc3_ = §_-N1v§[this.§_-53J§];
            this.controlsMap.setControlByParams(param1,param2,_loc3_.command);
            this.§_-m26§();
         }
      }
      
      private function §_-H1b§() : void
      {
         this.controlsMap = new GamepadControlsMap(this.device.name);
         this.§_-53J§ = -1;
         this.§_-m26§();
      }
      
      private function §_-m26§() : void
      {
         var _loc1_:Object = null;
         ++this.§_-53J§;
         if(this.§_-53J§ < §_-N1v§.length)
         {
            _loc1_ = §_-N1v§[this.§_-53J§];
            this.§_-j§.text = §_-s2a§.§_-K3t§(§_-s2a§.GAMEPAD_SETUP_BEFORE_CONTROL_NAME) + " \"" + §_-s2a§.§_-K3t§(_loc1_.message) + "\" " + §_-s2a§.§_-K3t§(§_-s2a§.GAMEPAD_SETUP_AFTER_CONTROL_NAME);
         }
         else
         {
            hide();
         }
      }
      
      private function onDeviceRemoved(param1:Event) : void
      {
         hide();
      }
      
      private function §_-c1O§() : void
      {
         §_-X1j§.§_-dr§.§_-L1j§(this.device,this.controlsMap);
      }
      
      override public function dispose() : void
      {
         this.§_-c1O§();
         this.deactivateDevice();
         this.§_-m1t§ = null;
         super.dispose();
      }
      
      override public function §_-X2O§() : void
      {
         this.§_-53J§ = Math.max(-1,this.§_-53J§ - 1);
         this.§_-m26§();
      }
   }
}

