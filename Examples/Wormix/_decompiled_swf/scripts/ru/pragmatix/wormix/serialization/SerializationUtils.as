package ru.pragmatix.wormix.serialization
{
   import §_-03T§.§_-2p§;
   import §_-12a§.§_-G2l§;
   import §_-21p§.§_-4w§;
   import §_-2U§.*;
   import §_-31N§.§_-42G§;
   import §_-31W§.§_-E1E§;
   import §_-43V§.Random;
   import §_-5Y§.§_-Ma§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-73I§.§_-q15§;
   import §_-91B§.§_-QK§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C3f§.§_-1Y§;
   import §_-CF§.§_-x2t§;
   import §_-CR§.§_-02k§;
   import §_-D3A§.*;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-HD§.*;
   import §_-J3L§.§_-h1D§;
   import §_-K35§.§_-S18§;
   import §_-L1H§.§_-C3e§;
   import §_-Ly§.§_-81L§;
   import §_-S§.*;
   import §_-TF§.§_-y2n§;
   import §_-Ta§.*;
   import §_-U1i§.§_-O2i§;
   import §_-Vg§.MainClanWindow;
   import §_-Vg§.§_-K3K§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1b§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-ZB§.§_-A10§;
   import §_-ZP§.§_-H1H§;
   import §_-b1F§.§_-G4§;
   import §_-b1F§.§_-v2Q§;
   import §_-b1R§.§_-Bx§;
   import §_-eW§.§_-S21§;
   import §_-eW§.§_-V2n§;
   import §_-eW§.§_-g2b§;
   import §_-g2r§.§_-03A§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-j1w§.§_-T0§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-q11§.§_-C2n§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-s1Q§.§_-W2D§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-Zd§;
   import §_-z1g§.§_-lp§;
   import §_-zI§.§_-8D§;
   import com.distriqt.extension.cloudstorage.CloudStorage;
   import com.hurlant.math.*;
   import com.hurlant.util.§_-a2P§;
   import config.§_-V2w§;
   import dragonBones.Bone;
   import dragonBones.Slot;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ButtonState;
   import feathers.controls.LayoutGroup;
   import feathers.controls.NumericStepper;
   import feathers.controls.ToggleButton;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.ITextRenderer;
   import feathers.data.IListCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import flash.display.DisplayObject;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.events.MouseEvent;
   import flash.geom.Point;
   import flash.net.Socket;
   import flash.system.ApplicationDomain;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.clearTimeout;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.flox.social.utils.id.IPlatformAccountController;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-s1V§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.clans.request.ClanSummaryRequest;
   import ru.pragmatix.wormix.messages.clans.request.ListClansRequest;
   import ru.pragmatix.wormix.messages.clans.request.edit.RenameClanRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.client.BuyShopItems;
   import ru.pragmatix.wormix.messages.client.GetWhoPumpedReaction;
   import ru.pragmatix.wormix.messages.structures.BossWormStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.pvp.messages.server.BattleLoginError;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.interfaces.§_-y2Q§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   use namespace bi_internal;
   
   public class SerializationUtils
   {
      
      public function SerializationUtils()
      {
         super();
      }
      
      public static function readStructureArray(param1:ByteArray, param2:Class) : Array
      {
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:Array = [];
         var _loc5_:int = 0;
         while(_loc5_ < _loc3_)
         {
            param1.readShort();
            _loc4_[_loc5_] = §_-616§.§_-fk§(param2).deserializeStructure(param1,null);
            _loc5_++;
         }
         return _loc4_;
      }
      
      public static function readStructureVector(param1:ByteArray, param2:Class) : Vector.<Object>
      {
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:Vector.<Object> = new Vector.<Object>(0);
         var _loc5_:int = 0;
         while(_loc5_ < _loc3_)
         {
            param1.readShort();
            _loc4_[_loc5_] = §_-616§.§_-fk§(param2).deserializeStructure(param1,null);
            _loc5_++;
         }
         return _loc4_;
      }
      
      public static function readStructure(param1:ByteArray, param2:Class) : Object
      {
         var _loc3_:int = param1.readShort();
         if(_loc3_ == 0)
         {
            return null;
         }
         return §_-616§.§_-fk§(param2).deserializeStructure(param1,null);
      }
      
      public static function checkSecure(param1:ByteArray, param2:String = "") : void
      {
         var _loc3_:Boolean = param2 != "" ? param2 == §_-J2g§.§_-i1N§.§_-yt§() : true;
         var _loc4_:Boolean = §_-U11§.isSecure(param1,§_-6L§.§_-95§);
         if(!_loc4_ || !_loc3_)
         {
            §_-J2g§.§_-i1N§.dispose();
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.PROTOCOL_CHEATER),§_-fH§.§_-L2I§);
         }
      }
      
      public static function readByteArray(param1:ByteArray, param2:Array = null) : Array
      {
         if(param2 == null)
         {
            param2 = [];
         }
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readByte());
            _loc4_++;
         }
         return param2;
      }
      
      public static function readIntArray(param1:ByteArray, param2:Array = null) : Array
      {
         if(param2 == null)
         {
            param2 = [];
         }
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readInt());
            _loc4_++;
         }
         return param2;
      }
      
      public static function readShortArray(param1:ByteArray, param2:Array = null) : Array
      {
         if(param2 == null)
         {
            param2 = [];
         }
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readShort());
            _loc4_++;
         }
         return param2;
      }
      
      public static function readUshortArray(param1:ByteArray, param2:Array) : void
      {
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readUnsignedShort());
            _loc4_++;
         }
      }
      
      public static function readStringArray(param1:ByteArray, param2:Array) : void
      {
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readUTF());
            _loc4_++;
         }
      }
      
      public static function readUintArray(param1:ByteArray, param2:Array) : void
      {
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readUnsignedInt());
            _loc4_++;
         }
      }
      
      public static function readIntVector(param1:ByteArray, param2:Vector.<int> = null) : Vector.<int>
      {
         if(param2 == null)
         {
            param2 = new Vector.<int>(0);
         }
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readInt());
            _loc4_++;
         }
         return param2;
      }
      
      public static function readUintVector(param1:ByteArray, param2:Vector.<uint> = null) : Vector.<uint>
      {
         if(param2 == null)
         {
            param2 = new Vector.<uint>(0);
         }
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readUnsignedInt());
            _loc4_++;
         }
         return param2;
      }
      
      public static function readShortVector(param1:ByteArray, param2:Vector.<int> = null) : Vector.<int>
      {
         if(param2 == null)
         {
            param2 = new Vector.<int>(0);
         }
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readShort());
            _loc4_++;
         }
         return param2;
      }
      
      public static function readByteVector(param1:ByteArray, param2:Vector.<int> = null) : Vector.<int>
      {
         if(param2 == null)
         {
            param2 = new Vector.<int>(0);
         }
         var _loc3_:uint = param1.readUnsignedShort();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            param2.push(param1.readByte());
            _loc4_++;
         }
         return param2;
      }
      
      public static function writeStructureArray(param1:ByteArray, param2:Class, param3:Array) : void
      {
         var _loc4_:uint = param3.length;
         param1.writeShort(_loc4_);
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            param1.writeShort(1);
            §_-616§.§_-fk§(param2).serializeStructure(param3[_loc5_],param1,null);
            _loc5_++;
         }
      }
      
      public static function writeStructure(param1:ByteArray, param2:Object, param3:Class) : void
      {
         if(param2)
         {
            param1.writeShort(1);
            §_-616§.§_-fk§(param3).serializeStructure(param2,param1,null);
         }
         else
         {
            param1.writeShort(0);
         }
      }
      
      public static function writeByteArray(param1:ByteArray, param2:Array) : void
      {
         var _loc3_:uint = 0;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeByte(_loc3_);
         }
      }
      
      public static function writeIntArray(param1:ByteArray, param2:Array) : void
      {
         var _loc3_:int = 0;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeInt(_loc3_);
         }
      }
      
      public static function writeShortArray(param1:ByteArray, param2:Array) : void
      {
         var _loc3_:int = 0;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeShort(_loc3_);
         }
      }
      
      public static function writeStringArray(param1:ByteArray, param2:Array) : void
      {
         var _loc3_:String = null;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeUTF(_loc3_);
         }
      }
      
      public static function writeUintArray(param1:ByteArray, param2:Array) : void
      {
         var _loc3_:uint = 0;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeUnsignedInt(_loc3_);
         }
      }
      
      public static function writeByteVector(param1:ByteArray, param2:Vector.<int>) : void
      {
         var _loc3_:uint = 0;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeByte(_loc3_);
         }
      }
      
      public static function writeByteUintVector(param1:ByteArray, param2:Vector.<uint>) : void
      {
         var _loc3_:uint = 0;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeByte(_loc3_);
         }
      }
      
      public static function writeIntVector(param1:ByteArray, param2:Vector.<int>) : void
      {
         var _loc3_:int = 0;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeInt(_loc3_);
         }
      }
      
      public static function writeShortVector(param1:ByteArray, param2:Vector.<int>) : void
      {
         var _loc3_:int = 0;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeShort(_loc3_);
         }
      }
      
      public static function writeStringVector(param1:ByteArray, param2:Vector.<String>) : void
      {
         var _loc3_:String = null;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeUTF(_loc3_);
         }
      }
      
      public static function writeUintVector(param1:ByteArray, param2:Vector.<uint>) : void
      {
         var _loc3_:uint = 0;
         param1.writeShort(param2.length);
         for each(_loc3_ in param2)
         {
            param1.writeUnsignedInt(_loc3_);
         }
      }
      
      public static function hexDump(param1:ByteArray) : String
      {
         var _loc5_:int = 0;
         var _loc2_:String = "";
         var _loc3_:String = "";
         param1.position = 0;
         var _loc4_:int = 1;
         while(_loc4_ < param1.length + 1)
         {
            _loc5_ = param1.readByte();
            if(_loc5_ > 20 && _loc5_ < 123)
            {
               _loc3_ += String.fromCharCode(_loc5_);
            }
            else
            {
               _loc3_ += ".";
            }
            _loc2_ += §_-L2U§(_loc5_) + " ";
            if(_loc4_ % 16 == 0)
            {
               _loc2_ += "\t" + _loc3_ + "\n";
               _loc3_ = "";
            }
            _loc4_++;
         }
         if(_loc4_ % 16 != 0)
         {
            while(_loc4_ % 16 != 0)
            {
               _loc2_ += "   ";
               _loc4_++;
            }
            _loc2_ += "   ";
            _loc2_ += "\t" + _loc3_ + "\n";
         }
         return _loc2_;
      }
      
      private static function §_-L2U§(param1:uint) : String
      {
         var _loc2_:String = "";
         var _loc3_:String = "FEDCBA";
         var _loc4_:uint = 0;
         while(_loc4_ < 2)
         {
            if((param1 & 240 >> _loc4_ * 4) >> 4 - _loc4_ * 4 > 9)
            {
               _loc2_ += _loc3_.charAt(15 - ((param1 & 240 >> _loc4_ * 4) >> 4 - _loc4_ * 4));
            }
            else
            {
               _loc2_ += String((param1 & 240 >> _loc4_ * 4) >> 4 - _loc4_ * 4);
            }
            _loc4_++;
         }
         return _loc2_;
      }
   }
}

