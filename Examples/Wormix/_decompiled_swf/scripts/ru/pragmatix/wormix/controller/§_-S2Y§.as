package ru.pragmatix.wormix.controller
{
   import §_-11L§.*;
   import §_-21p§.§_-4w§;
   import §_-31W§.§_-g16§;
   import §_-5Y§.§_-t2O§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-63M§.§_-F3f§;
   import §_-63U§.§_-41y§;
   import §_-82l§.§_-A2Y§;
   import §_-937§.§_-n1b§;
   import §_-A1K§.§_-7u§;
   import §_-B1y§.§_-F1P§;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-J31§.§_-e2J§;
   import §_-Ly§.§_-81L§;
   import §_-O2L§.§_-X1T§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-Y1b§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-aM§.*;
   import §_-d2p§.EquipStatCell;
   import §_-e2r§.§_-636§;
   import §_-fo§.§_-r5§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-p18§.§_-vd§;
   import §_-r1C§.*;
   import §_-rM§.§_-61j§;
   import §_-sQ§.§_-V1n§;
   import §_-t10§.*;
   import §_-u1P§.ShopResultEvent;
   import §_-wD§.*;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.events.DeviceOrientationEvent;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.*;
   import com.greensock.loading.core.LoaderCore;
   import dragonBones.starling.StarlingFactory;
   import dragonBones.starling.StarlingTextureAtlasData;
   import dragonBones.textures.TextureAtlasData;
   import feathers.controls.Button;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ListCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.VerticalAlign;
   import flash.display.MovieClip;
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.clearTimeout;
   import flash.utils.getTimer;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.flox.social.utils.storage.EncryptedLocalStorageCookie;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.§_-P1h§;
   import ru.pragmatix.wormix.messages.client.BuyShopItems;
   import ru.pragmatix.wormix.messages.server.GetFriendListPageResult;
   import ru.pragmatix.wormix.messages.server.LinkResult;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   
   public class §_-S2Y§
   {
      
      public var §_-714§:Number;
      
      private var §_-1G§:Array = [];
      
      private var _values:Array = [];
      
      private var §_-RV§:Array = [];
      
      public function §_-S2Y§()
      {
         super();
         this.§_-714§ = new Date().time;
      }
      
      public static function §_-iT§(param1:Array, param2:Array, param3:Array) : §_-S2Y§
      {
         var _loc5_:uint = 0;
         var _loc4_:§_-S2Y§ = new §_-S2Y§();
         if(param1.length == param2.length && param1.length > 0)
         {
            _loc5_ = 0;
            while(_loc5_ < param1.length)
            {
               _loc4_.set(param1[_loc5_],param2[_loc5_]);
               _loc5_++;
            }
         }
         _loc4_.§_-n2y§(param3);
         return _loc4_;
      }
      
      public function §_-Ae§(param1:uint, param2:uint = 1) : void
      {
         if(param1 == §_-g16§.TEAM_DAMAGE)
         {
            if(this.valueOf(§_-g16§.SOLO_DAMAGE) < this.valueOf(§_-g16§.TEAM_DAMAGE) / 10)
            {
               param2 = 0;
            }
         }
         if(param2 > 0)
         {
            this.set(param1,this.valueOf(param1) + param2);
         }
      }
      
      public function set(param1:uint, param2:int) : void
      {
         var _loc3_:int = this.valueOf(param1);
         var _loc4_:int = this.§_-1G§.indexOf(param1);
         if(_loc4_ != -1)
         {
            this._values[_loc4_] = param2;
         }
         else
         {
            this.§_-1G§.push(param1);
            this._values.push(param2);
         }
      }
      
      private function §_-n2y§(param1:Array) : void
      {
         var _loc2_:int = 0;
         var _loc3_:int = int(§_-s1x§.§_-EO§);
         while(_loc2_ < _loc3_)
         {
            this.§_-RV§[_loc2_] = false;
            _loc2_++;
         }
         var _loc4_:int = 0;
         var _loc5_:int = int(param1.length);
         while(_loc4_ < _loc5_)
         {
            this.§_-RV§[int(param1[_loc4_])] = true;
            _loc4_++;
         }
      }
      
      public function §_-Lb§(param1:int) : Boolean
      {
         return this.§_-RV§[param1];
      }
      
      public function §_-j2U§(param1:int) : Array
      {
         this.§_-RV§[param1] = true;
         return this.§_-RV§;
      }
      
      public function valueOf(param1:uint) : int
      {
         var _loc2_:int = this.§_-1G§.indexOf(param1);
         return _loc2_ != -1 ? int(this._values[_loc2_]) : 0;
      }
      
      public function get isEmpty() : Boolean
      {
         return this.§_-1G§.length == 0;
      }
      
      public function get ids() : Array
      {
         return this.§_-1G§.concat();
      }
      
      public function get §_-R1o§() : Array
      {
         return this._values.concat();
      }
      
      public function get count() : uint
      {
         return this.§_-1G§.length;
      }
      
      public function toString() : String
      {
         var _loc1_:String = "";
         var _loc2_:int = 0;
         while(_loc2_ < this.ids.length)
         {
            _loc1_ += (_loc2_ != 0 ? "," : "") + "(" + this.ids[_loc2_] + " -> " + this.§_-R1o§[_loc2_] + ")";
            _loc2_++;
         }
         return _loc1_;
      }
   }
}

