package ru.pragmatix.flox.gui.controls
{
   import §_-134§.*;
   import §_-21A§.*;
   import §_-21p§.§_-4w§;
   import §_-43V§.Random;
   import §_-62§.§_-G1p§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-Bv§.§_-G3P§;
   import §_-C3f§.§_-1Y§;
   import §_-CF§.§_-gE§;
   import §_-CR§.§_-72p§;
   import §_-Cl§.§_-L2z§;
   import §_-D3A§.§_-TZ§;
   import §_-J31§.§_-R1h§;
   import §_-Ly§.§_-81L§;
   import §_-OJ§.§_-L3B§;
   import §_-SP§.§_-M4§;
   import §_-T2V§.§_-13F§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-J1§;
   import §_-Vg§.*;
   import §_-Y1O§.*;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-dS§.*;
   import §_-e1e§.§_-CG§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.*;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-u13§.MD2;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u13§.§_-F3§;
   import §_-x2Q§.§_-dU§;
   import com.greensock.loading.core.LoaderCore;
   import com.hurlant.math.BigInteger;
   import com.hurlant.util.*;
   import com.hurlant.util.der.Sequence;
   import com.hurlant.util.der.§_-cz§;
   import com.hurlant.util.der.§_-l1b§;
   import feathers.controls.Button;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.events.FeathersEventType;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalLayout;
   import feathers.themes.FontSize;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.utils.textures.TextureCache;
   import flash.events.*;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.text.TextFormatAlign;
   import flash.utils.*;
   import org.as3commons.zip.*;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.response.edit.CloseExitClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanNewsTO;
   import ru.pragmatix.wormix.messages.client.Login;
   import ru.pragmatix.wormix.messages.server.ProfilesResult;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.Waypoint;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.client.*;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.core.Starling;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   public class §_-N2r§
   {
      
      protected var data:Array;
      
      private var page:int;
      
      private var pageCount:int;
      
      private var pageSize:int;
      
      public function §_-N2r§(param1:Array, param2:int, param3:int = 0)
      {
         super();
         this.data = param1;
         this.pageSize = param2;
         this.pageCount = param3 ? this.§_-1X§(param3) : this.§_-1X§(param1.length);
         this.page = 1;
      }
      
      private function §_-1X§(param1:uint) : int
      {
         if(!param1)
         {
            return 1;
         }
         return Math.ceil(param1 / this.pageSize);
      }
      
      public function §_-aZ§() : int
      {
         return (this.page - 1) * this.pageSize;
      }
      
      public function §_-O2o§(param1:uint, param2:uint, param3:Function) : void
      {
         param3(this.data.slice(param1,param1 + param2));
      }
      
      public function §_-kr§(param1:Function) : void
      {
         var _loc2_:int = this.§_-aZ§();
         this.§_-O2o§(_loc2_,_loc2_ + this.pageSize,param1);
      }
      
      public function §_-a2k§() : int
      {
         return this.data.length;
      }
      
      public function §_-zY§() : int
      {
         return this.page;
      }
      
      public function §_-b1S§(param1:int) : void
      {
         this.page = param1;
      }
      
      public function §_-so§() : Boolean
      {
         return this.page == 1;
      }
      
      public function §_-AM§() : int
      {
         return this.pageSize;
      }
      
      public function §_-f2Z§() : Boolean
      {
         return this.page == this.pageCount;
      }
      
      public function §_-01o§() : int
      {
         return this.pageCount;
      }
      
      public function indexOf(param1:Object) : int
      {
         return this.data.indexOf(param1);
      }
      
      public function §_-91s§() : Array
      {
         return this.data;
      }
      
      public function §_-kb§(param1:uint) : Object
      {
         return this.data[param1];
      }
   }
}

