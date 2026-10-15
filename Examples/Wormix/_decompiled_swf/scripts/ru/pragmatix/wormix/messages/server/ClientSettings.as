package ru.pragmatix.wormix.messages.server
{
   import §_-134§.*;
   import §_-21p§.§_-4w§;
   import §_-2H§.§_-91a§;
   import §_-2H§.§_-A1R§;
   import §_-2H§.§_-N15§;
   import §_-2H§.§_-Y1i§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-o2k§;
   import §_-CR§.§_-72p§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-T2G§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-H9§.§_-NX§;
   import §_-HD§.*;
   import §_-J31§.§_-R1h§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-SP§.§_-M4§;
   import §_-U1i§.§_-U16§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-81l§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-e2r§.§_-02G§;
   import §_-e2r§.§_-lR§;
   import §_-e2r§.§_-n1§;
   import §_-j1w§.§_-L1x§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.easing.Circ;
   import com.greensock.plugins.*;
   import feathers.controls.Button;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.controls.NumericStepper;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.PopUpManager;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.utils.textures.TextureCache;
   import flash.display.*;
   import flash.events.*;
   import flash.utils.*;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.wormix.controller.*;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-s1V§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.request.edit.RenameClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class ClientSettings
   {
      
      public var list:Object = {};
      
      public function ClientSettings()
      {
         super();
      }
      
      public function setValue(param1:String, param2:String) : void
      {
         this.list[param1] = param2;
      }
      
      public function getValue(param1:String) : String
      {
         return this.list[param1];
      }
      
      public function toString() : String
      {
         var _loc2_:String = null;
         var _loc1_:String = "";
         for(_loc2_ in this.list)
         {
            if(_loc1_.length > 0)
            {
               _loc1_ += ",";
            }
            _loc1_ += _loc2_ + "=" + this.list[_loc2_];
         }
         return "ClientSettings{" + _loc1_ + "}";
      }
   }
}

