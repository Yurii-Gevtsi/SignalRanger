package ru.pragmatix.wormix.gui
{
   import §_-62§.§_-W1r§;
   import §_-931§.§_-sz§;
   import §_-937§.§_-n1b§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H3§.*;
   import §_-L1H§.§_-C3e§;
   import §_-O2L§.§_-X1T§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-YF§.§_-q2v§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-m1g§.§_-K2W§;
   import §_-nE§.DowngradeCraftWeaponScreen;
   import §_-nE§.InfoCraftWeaponScreen;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.UpgradeCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-s20§.§_-J2g§;
   import §_-u2J§.§_-T1K§;
   import §_-y1s§.*;
   import §_-z1g§.§_-82X§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.events.DeviceOrientationEvent;
   import com.hurlant.crypto.tls.§_-F16§;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.core.ITextRenderer;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import flash.utils.ByteArray;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.CommandState;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.messages.server.EnterAccount;
   import ru.pragmatix.wormix.messages.server.LoginError;
   import ru.pragmatix.wormix.messages.server.UserIsBanned;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class §_-61h§
   {
      
      private var §_-Y2X§:Vector.<§_-K1Q§>;
      
      public function §_-61h§()
      {
         super();
         this.§_-Y2X§ = new Vector.<§_-K1Q§>(0);
      }
      
      public function dispose() : void
      {
         if(this.§_-Y2X§.length > 1)
         {
            ControlSystems.instance.removeEventListener(§_-L3Q§.COMMAND_CONTROL_CHANGED,this.§_-t1a§);
         }
         this.§_-Y2X§ = null;
      }
      
      public function register(param1:§_-K1Q§) : void
      {
         this.§_-Y2X§.push(param1);
         ControlSystems.instance.addEventListener(§_-L3Q§.COMMAND_CONTROL_CHANGED,this.§_-t1a§);
      }
      
      private function §_-t1a§(param1:Event) : void
      {
         var _loc3_:String = null;
         var _loc4_:§_-K1Q§ = null;
         var _loc2_:Command = param1.data as Command;
         if(Boolean(_loc2_) && _loc2_.state == CommandState.BEGIN)
         {
            switch(_loc2_)
            {
               case Command.Left:
                  _loc3_ = "onActionLeft";
                  break;
               case Command.Right:
                  _loc3_ = "onActionRight";
                  break;
               case Command.Down:
                  _loc3_ = "onActionDown";
                  break;
               case Command.Up:
                  _loc3_ = "onActionUp";
                  break;
               case Command.Shoot:
                  _loc3_ = "onActionSelect";
                  break;
               case Command.Escape:
                  _loc3_ = "onActionClose";
                  break;
               case Command.Bag:
                  _loc3_ = "onActionBag";
            }
            if(_loc3_)
            {
               for each(_loc4_ in this.§_-Y2X§)
               {
                  _loc4_[_loc3_]();
               }
            }
         }
      }
      
      public function unregister(param1:§_-K1Q§) : void
      {
         var _loc2_:int = this.§_-Y2X§.indexOf(param1);
         if(_loc2_ > -1)
         {
            this.§_-Y2X§.removeAt(_loc2_);
         }
      }
   }
}

