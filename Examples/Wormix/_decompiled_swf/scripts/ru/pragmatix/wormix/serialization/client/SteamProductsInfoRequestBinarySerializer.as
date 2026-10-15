package ru.pragmatix.wormix.serialization.client
{
   import §_-82l§.§_-1I§;
   import §_-Af§.*;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-J2l§;
   import §_-G16§.*;
   import §_-H16§.*;
   import §_-H3§.*;
   import §_-J31§.§_-e2J§;
   import §_-SP§.§_-M4§;
   import §_-Tm§.§_-RF§;
   import §_-YF§.§_-q2v§;
   import §_-d2p§.*;
   import §_-hO§.§_-03k§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-Ia§;
   import com.greensock.events.TweenEvent;
   import com.hurlant.math.BigInteger;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.core.PopUpManager;
   import feathers.layout.AnchorLayoutData;
   import feathers.themes.FontSize;
   import flash.events.Event;
   import flash.geom.Rectangle;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.GestureState;
   import org.gestouch.core.Touch;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.client.SteamProductsInfoRequest;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurnResponse;
   import ru.pragmatix.wormix.utils.FrameTimer;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   
   public class SteamProductsInfoRequestBinarySerializer implements §_-D2L§
   {
      
      public function SteamProductsInfoRequestBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 6005;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:SteamProductsInfoRequest = SteamProductsInfoRequest(param1);
         var _temp_1:* = new §_-5g§();
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return new SteamProductsInfoRequest();
      }
   }
}

