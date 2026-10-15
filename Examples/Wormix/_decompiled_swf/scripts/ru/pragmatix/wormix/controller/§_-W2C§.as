package ru.pragmatix.wormix.controller
{
   import §_-02q§.§_-4s§;
   import §_-12W§.*;
   import §_-21A§.§_-A2x§;
   import §_-21p§.§_-4w§;
   import §_-23P§.*;
   import §_-2U§.*;
   import §_-31N§.§_-42G§;
   import §_-31W§.§_-r26§;
   import §_-31Y§.§_-D3B§;
   import §_-31u§.*;
   import §_-51p§.§_-W2e§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-zF§;
   import §_-63U§.§_-412§;
   import §_-82a§.*;
   import §_-82l§.§_-A2Y§;
   import §_-91B§.§_-z2l§;
   import §_-937§.§_-81u§;
   import §_-937§.§_-BO§;
   import §_-937§.§_-T15§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-C2M§.DesktopTutorialPanel;
   import §_-C2M§.MobileTutorialPanel;
   import §_-C2M§.§_-o2f§;
   import §_-C2t§.§_-C1i§;
   import §_-C2t§.§_-Ui§;
   import §_-CF§.§_-E19§;
   import §_-CR§.§_-72p§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-G2H§.*;
   import §_-H2g§.§_-di§;
   import §_-J31§.§_-R1h§;
   import §_-L1H§.§_-C3e§;
   import §_-L2F§.§_-D2p§;
   import §_-P1B§.§_-b21§;
   import §_-P2i§.§_-b7§;
   import §_-SP§.§_-M4§;
   import §_-T1t§.§_-u19§;
   import §_-TF§.§_-q1j§;
   import §_-Tm§.§_-RF§;
   import §_-Ty§.§_-K3a§;
   import §_-U1i§.*;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-61Z§;
   import §_-Y1b§.§_-81l§;
   import §_-YF§.§_-L29§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-ZP§.§_-w1J§;
   import §_-aM§.*;
   import §_-d2p§.BuyEquipDialog;
   import §_-d2p§.BuyWeaponDialog;
   import §_-d2p§.BuyWeaponInstantDialog;
   import §_-dS§.§_-D3a§;
   import §_-fo§.§_-r5§;
   import §_-g2C§.GlobalChatTextMessageItemRenderer;
   import §_-g2C§.§_-62U§;
   import §_-g2C§.§_-I1R§;
   import §_-g2C§.§_-K2A§;
   import §_-g2C§.§_-K2u§;
   import §_-g2C§.§_-p8§;
   import §_-g2C§.§_-w1K§;
   import §_-hO§.§_-v24§;
   import §_-j1w§.*;
   import §_-j22§.§_-5g§;
   import §_-k1r§.AlertPanel;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-lh§.§_-z2A§;
   import §_-o14§.*;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-t1R§;
   import §_-s1Q§.Artifacts;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-sQ§.§_-H3D§;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u1P§.ShopResultEvent;
   import §_-u1P§.§_-Po§;
   import §_-u1P§.§_-U1L§;
   import §_-u1m§.*;
   import §_-w2W§.§_-21w§;
   import §_-w2i§.§_-52k§;
   import §_-w2i§.§_-Zd§;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-M2v§;
   import §_-z20§.*;
   import §_-zI§.§_-g1a§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import com.distriqt.extension.inappbilling.events.AvailabilityEvent;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import com.distriqt.extension.pushnotifications.PushNotifications;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.loading.*;
   import com.greensock.plugins.*;
   import com.hurlant.crypto.tls.§_-21v§;
   import config.§_-B1K§;
   import config.§_-p1i§;
   import feathers.controls.Button;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.ToggleButton;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.core.ITextRenderer;
   import feathers.core.PopUpManager;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.utils.textures.TextureCache;
   import flash.display.*;
   import flash.errors.IllegalOperationError;
   import flash.events.*;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.net.SharedObject;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.*;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.SocialApiSettings;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.controller.mobile.*;
   import ru.pragmatix.flox.social.model.Gender;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-230§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-X1Y§;
   import ru.pragmatix.wormix.gui.menu.shop.desktop.§_-w1W§;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.§_-01f§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.client.*;
   import ru.pragmatix.wormix.messages.server.GetFriendListPageResult;
   import ru.pragmatix.wormix.messages.server.Pong;
   import ru.pragmatix.wormix.messages.server.ShopResult;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.PumpReactionRateStructure;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.messages.utils.Result;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpEndBattle;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.pvp.messages.server.PvpStartTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.serialization.*;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.LaserLineAmmoController;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import ru.pragmatix.wormix.weapons.ammo.dataStructures.LaserPair;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import ru.pragmatix.wormix.weapons.interfaces.§_-J3w§;
   import starling.animation.Transitions;
   import starling.core.Starling;
   import starling.display.Button;
   import starling.display.Canvas;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.extensions.ParticleAura;
   import starling.filters.ColorMatrixFilter;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class §_-W2C§ extends starling.events.EventDispatcher
   {
      
      public static const §_-J32§:uint = 100;
      
      private var shopMenu:§_-X1Y§;
      
      private var §_-B3m§:Array;
      
      private var §_-93w§:Array = [];
      
      private var §_-B2g§:int;
      
      private var §_-D1E§:int;
      
      private var §_-D2c§:Function;
      
      public function §_-W2C§()
      {
         super();
         this.§_-C3V§();
      }
      
      private function §_-p2O§() : void
      {
         this.shopMenu = SystemUtil.isDesktop ? new §_-w1W§() : new §_-01f§();
         this.shopMenu.addEventListener(§_-M4§.WINDOW_CLOSE,this.§_-B1Y§);
         this.shopMenu.addEventListener(§_-M4§.WINDOW_CANCEL,this.§_-B1Y§);
         this.shopMenu.addEventListener(§_-Po§.TRY_BUY,this.§_-93F§);
      }
      
      public function get §_-831§() : Boolean
      {
         return Boolean(this.shopMenu) && this.shopMenu.§_-h1C§();
      }
      
      public function get isSubpanelOpened() : Boolean
      {
         return Boolean(this.shopMenu) && this.shopMenu.isSubpanelOpened;
      }
      
      public function §_-O1W§() : void
      {
         if(PopUpManager.isTopLevelPopUp(this.shopMenu as starling.display.DisplayObject))
         {
            if(this.isSubpanelOpened)
            {
               this.shopMenu.§_-52u§();
            }
            else
            {
               this.§_-B1Y§(null);
            }
         }
         else
         {
            §_-YQ§.§_-33S§();
         }
      }
      
      private function §_-93F§(param1:§_-Po§) : void
      {
         var _loc4_:§_-W1r§ = null;
         var _loc5_:§_-W1r§ = null;
         var _loc2_:§_-W1r§ = param1.item as §_-W1r§;
         var _loc3_:§_-Oi§ = Application.userProfile.§_-pd§().§_-d0§(_loc2_.getId());
         if(_loc2_ is §_-O2H§)
         {
            if(§_-G1p§.§_-61d§(_loc2_))
            {
               this.§_-7K§(§_-s2a§.BUY_EXISTING_WEAPON);
               return;
            }
            if(Boolean(_loc3_) && Boolean(_loc3_.isInfinite()) || this.§_-E2m§(_loc2_))
            {
               _loc5_ = this.§_-7l§(_loc2_.getName());
               if(_loc3_ != null)
               {
                  _loc4_ = _loc3_.getRecord();
               }
               if(_loc4_ != null && _loc4_.getInfinite() && §_-O2H§(_loc4_).getMaxShots() > 0)
               {
                  this.§_-728§(_loc2_,param1.§_-j1U§,param1.§_-51w§);
               }
               else if(_loc5_ != null && _loc5_.getName() != "" && §_-O2H§(_loc5_).getMaxShots() > 0)
               {
                  this.§_-728§(_loc2_,param1.§_-j1U§,param1.§_-51w§);
               }
               else
               {
                  this.§_-7K§(§_-s2a§.BUY_EXISTING_WEAPON);
               }
            }
            else
            {
               this.§_-728§(_loc2_,param1.§_-j1U§,param1.§_-51w§);
            }
         }
         else if(_loc2_ is §_-L1r§ && (Boolean(Application.userProfile.§_-XT§().§_-H3E§(_loc2_.getId())) || Boolean(this.§_-PW§(_loc2_.getId()))))
         {
            this.§_-7K§(§_-s2a§.BUY_EXISTING_EQUIP);
         }
         else
         {
            this.§_-728§(_loc2_,param1.§_-j1U§,param1.§_-51w§);
         }
      }
      
      public function §_-728§(param1:§_-W1r§, param2:Array = null, param3:int = 0) : void
      {
         var _loc4_:§_-230§ = this.§_-614§(param1);
         _loc4_.setItem(param1,1);
         _loc4_.show();
         §_-52k§.single(_loc4_ as starling.events.EventDispatcher,§_-U1L§.BUY_ITEM,this.§_-uX§);
      }
      
      private function §_-uX§(param1:Object) : void
      {
         this.§_-dh§(param1.item as §_-W1r§,param1.count,param1.moneyType);
      }
      
      public function §_-dh§(param1:§_-W1r§, param2:int, param3:§_-E19§) : void
      {
         var structure:ShopItemStructure;
         var isComplex:Boolean;
         var isWeapon:Boolean = false;
         var canBePurchased:Boolean = false;
         var item:§_-W1r§ = param1;
         var count:int = param2;
         var moneyType:§_-E19§ = param3;
         try
         {
            canBePurchased = §_-F4§.§_-EY§(item);
            if(!canBePurchased)
            {
               return;
            }
         }
         catch(e:Error)
         {
         }
         this.§_-93w§.push(item);
         structure = this.§_-PW§(item.getId());
         if(!structure)
         {
            this.§_-B3m§.push({
               "id":item.getId(),
               "structure":structure = new ShopItemStructure()
            });
         }
         if(count + structure.count > §_-J32§)
         {
            count = §_-J32§ - structure.count;
         }
         if(moneyType == §_-E19§.§_-51D§)
         {
            structure.moneyType = ShopItemStructure.REAL_MONEY;
            this.§_-D1E§ += item.§_-gc§() * count;
         }
         else
         {
            structure.moneyType = ShopItemStructure.MONEY;
            this.§_-B2g§ += item.§_-91e§() * count;
         }
         structure.id = item.getId();
         isWeapon = item is §_-O2H§;
         isComplex = isWeapon && §_-O2H§(item).isComplex();
         if(item.getInfinite())
         {
            if(isWeapon)
            {
               if(isComplex)
               {
                  structure.count = count;
               }
               else
               {
                  structure.count = -1;
               }
            }
            else
            {
               structure.count = -1;
            }
         }
         else
         {
            structure.count += count;
         }
         §_-N1V§.postController.§_-g1w§(item);
         this.§_-h1S§();
      }
      
      public function §_-56§(param1:Function = null) : void
      {
         this.§_-D2c§ = param1;
         this.§_-122§();
      }
      
      public function §_-ln§(param1:§_-W1r§, param2:Array = null, param3:int = 0) : void
      {
         var _loc4_:§_-Po§ = new §_-Po§(param1,param2,param3);
         (this.shopMenu as starling.events.EventDispatcher).dispatchEvent(_loc4_);
      }
      
      public function §_-p1l§() : void
      {
         this.§_-C3V§();
         if(!this.shopMenu)
         {
            this.§_-p2O§();
         }
      }
      
      public function §_-B1Y§(param1:starling.events.Event) : void
      {
         var e:starling.events.Event = param1;
         this.§_-h1S§(function():void
         {
            Application.§_-E1k§.showScreen(§_-21w§.§_-G31§);
         });
      }
      
      public function §_-S1j§(param1:§_-63i§, param2:Function, param3:Function) : void
      {
         §_-J2l§.§_-u2b§.hide();
         this.§_-kq§(ShopResult(param1.message));
         if(param2 != null)
         {
            param2();
         }
         dispatchEventWith(ShopResultEvent.BUY_SUCCESS);
      }
      
      private function §_-hL§(param1:flash.events.Event, param2:Function, param3:Function) : void
      {
         §_-J2l§.§_-u2b§.hide();
         var _loc4_:ShopResult = new ShopResult();
         _loc4_.result = ShopResult.ERROR;
         this.§_-kq§(_loc4_);
         if(param3 != null)
         {
            param3();
         }
         dispatchEventWith(ShopResultEvent.BUY_FAILED);
      }
      
      private function §_-kq§(param1:ShopResult) : void
      {
         var _loc2_:String = null;
         var _loc3_:int = param1.result;
         if(_loc3_ == ShopResult.SUCCESS)
         {
            if(this.§_-B2g§ > 0 || this.§_-D1E§ > 0)
            {
               this.§_-m22§(this.§_-B2g§,this.§_-D1E§);
               this.§_-q2J§(param1);
               _loc2_ = §_-s2a§.BUY_SUCCESSFUL;
            }
            else
            {
               _loc2_ = §_-s2a§.ERROR_HACKING_ATTEMPT;
            }
            this.§_-C3V§();
         }
         else if(_loc3_ == ShopResult.MIN_REQUIREMENTS_ERROR)
         {
            _loc2_ = §_-s2a§.ERROR_HACKING_ATTEMPT;
         }
         else if(_loc3_ == ShopResult.NOT_ENOUGH_MONEY)
         {
            _loc2_ = §_-s2a§.ERROR_HACKING_ATTEMPT;
         }
         else
         {
            _loc2_ = §_-s2a§.ERROR_SAVING;
         }
         if(Boolean(_loc2_) && _loc2_ != §_-s2a§.BUY_SUCCESSFUL)
         {
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_BUY),§_-s2a§.§_-K3t§(_loc2_));
         }
         if(Boolean(this.shopMenu) && this.shopMenu.§_-h1C§())
         {
            this.shopMenu.§_-oG§(Application.userProfile);
         }
      }
      
      private function §_-m22§(param1:int = 0, param2:int = 0) : void
      {
         if(param1 > 0)
         {
            Application.userProfile.§_-oA§(-this.§_-B2g§);
            §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-T20§],[this.§_-B2g§]);
         }
         if(param2 > 0)
         {
            Application.userProfile.§_-H3i§(-this.§_-D1E§);
            §_-X1j§.§_-k2F§.§_-f1I§([§_-r26§.§_-b22§],[this.§_-D1E§]);
         }
      }
      
      private function §_-q2J§(param1:ShopResult) : void
      {
         var _loc6_:WeaponStructure = null;
         var _loc7_:int = 0;
         var _loc2_:Array = [];
         var _loc3_:Array = param1.weapons;
         var _loc4_:int = int(_loc3_.length);
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            _loc6_ = _loc3_[_loc5_];
            _loc2_.push({
               "id":_loc6_.getId(),
               "count":_loc6_.getCount()
            });
            _loc5_++;
         }
         _loc3_ = param1.stuff;
         _loc4_ = int(_loc3_.length);
         _loc5_ = 0;
         while(_loc5_ < _loc4_)
         {
            _loc7_ = int(_loc3_[_loc5_]);
            _loc2_.push({
               "id":_loc7_,
               "count":-1
            });
            _loc5_++;
         }
         this.§_-G2F§(_loc2_);
      }
      
      private function §_-G2F§(param1:Array) : void
      {
         var _loc5_:Object = null;
         var _loc6_:§_-W1r§ = null;
         var _loc7_:§_-2m§ = null;
         var _loc8_:§_-O2H§ = null;
         var _loc9_:§_-Oi§ = null;
         var _loc10_:uint = 0;
         var _loc11_:uint = 0;
         var _loc2_:Array = [];
         var _loc3_:int = int(param1.length);
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            _loc5_ = param1[_loc4_];
            _loc6_ = §_-G1p§.§_-d0§(_loc5_.id,Application.userProfile.§_-P2e§());
            if(_loc6_.getInfinite())
            {
               _loc2_.push(_loc6_);
            }
            if(_loc6_ is §_-O2H§)
            {
               _loc7_ = Application.userProfile.§_-pd§();
               _loc8_ = _loc6_ as §_-O2H§;
               if(_loc8_.getInfinite())
               {
                  if(_loc8_.isComplex())
                  {
                     _loc9_ = _loc7_.§_-d0§(_loc6_.getId());
                     _loc10_ = (Boolean(_loc9_)) && _loc9_.isInfinite() ? _loc9_.getLevel() : 0;
                     _loc7_.§_-Um§(_loc8_,-1,_loc8_.getMaxShots(),_loc10_ + _loc5_.count);
                  }
                  else
                  {
                     _loc7_.§_-Um§(_loc8_,-1,_loc8_.getMaxShots());
                  }
               }
               else
               {
                  _loc11_ = uint(_loc7_.§_-d3§(_loc8_.getId()));
                  _loc7_.§_-Um§(_loc8_,_loc5_.count + _loc11_,_loc8_.getMaxShots());
               }
            }
            else if(_loc6_ is §_-zF§)
            {
               Application.userProfile.§_-XT§().§_-Um§(_loc6_);
               Application.userProfile.§_-L§(_loc6_ as §_-zF§);
            }
            else if(_loc6_ is §_-T1y§)
            {
               Application.userProfile.§_-XT§().§_-Um§(_loc6_);
               Application.userProfile.§_-aV§(_loc6_ as §_-T1y§);
            }
            _loc4_++;
         }
         §_-N1V§.postController.§_-Z2v§(_loc2_);
         if(Boolean(this.shopMenu) && Boolean(this.shopMenu.§_-h1C§()) && Boolean(_loc6_))
         {
            this.shopMenu.update(_loc6_.getId());
         }
         dispatchEventWith(ShopResultEvent.ITEM_ADDED,false,_loc6_);
      }
      
      private function §_-C3V§() : void
      {
         this.§_-B3m§ = [];
         this.§_-B2g§ = 0;
         this.§_-D1E§ = 0;
      }
      
      private function §_-h1S§(param1:Function = null, param2:Function = null) : void
      {
         var _loc6_:Object = null;
         var _loc3_:BuyShopItems = new BuyShopItems();
         var _loc4_:int = 0;
         var _loc5_:int = int(this.§_-B3m§.length);
         while(_loc4_ < _loc5_)
         {
            _loc6_ = this.§_-B3m§[_loc4_];
            _loc3_.shopItems.push(_loc6_.structure);
            _loc4_++;
         }
         if(_loc3_.shopItems.length > 0)
         {
            §_-J2l§.§_-u2b§.show(§_-d21§.§_-vH§,§_-J2g§.§_-i1N§,_loc3_,ShopResult,this.§_-S1j§,this.§_-hL§,param1,param2);
         }
         else if(param1 != null)
         {
            param1();
         }
      }
      
      public function §_-M1c§(param1:§_-W1r§) : Boolean
      {
         var _loc2_:int = 0;
         var _loc3_:ShopItemStructure = null;
         if(param1 != null)
         {
            _loc2_ = param1.getId();
            _loc3_ = this.§_-PW§(_loc2_);
            return Boolean(_loc3_);
         }
         return false;
      }
      
      public function §_-PW§(param1:int) : ShopItemStructure
      {
         var id:int = param1;
         var result:Array = this.§_-B3m§.filter(function(param1:Object, param2:int, param3:Array):Boolean
         {
            return param1.id == id;
         });
         if(result.length)
         {
            return result[0].structure;
         }
         return null;
      }
      
      public function §_-E2m§(param1:§_-W1r§) : Boolean
      {
         var _loc2_:§_-W1r§ = null;
         for each(_loc2_ in this.§_-93w§)
         {
            if(_loc2_.getName() == param1.getName() && _loc2_.getInfinite())
            {
               return true;
            }
         }
         return false;
      }
      
      public function §_-7l§(param1:String) : §_-W1r§
      {
         var _loc2_:§_-W1r§ = null;
         for each(_loc2_ in this.§_-93w§)
         {
            if(_loc2_.getName() == param1 && _loc2_.getInfinite())
            {
               return _loc2_;
            }
         }
         return new §_-W1r§();
      }
      
      public function §_-G3L§(param1:§_-W1r§) : void
      {
         var _loc2_:§_-230§ = null;
         if(param1 is §_-O2H§)
         {
            _loc2_ = new BuyWeaponInstantDialog();
         }
         else if(param1 is §_-L1r§)
         {
            _loc2_ = new BuyEquipDialog();
         }
         if(_loc2_)
         {
            _loc2_.setItem(param1);
            _loc2_.show();
         }
      }
      
      public function §_-yI§() : void
      {
         if(this.shopMenu)
         {
            this.shopMenu.§_-oG§(Application.userProfile);
         }
      }
      
      public function dispose() : void
      {
         if(this.shopMenu)
         {
            this.§_-o17§();
         }
         this.§_-B3m§ = null;
         this.§_-93w§ = null;
      }
      
      public function §_-o17§() : void
      {
         this.shopMenu.hide();
         this.shopMenu.removeEventListener(§_-M4§.WINDOW_CLOSE,this.§_-B1Y§);
         this.shopMenu.removeEventListener(§_-M4§.WINDOW_CANCEL,this.§_-B1Y§);
         this.shopMenu.removeEventListener(§_-Po§.TRY_BUY,this.§_-93F§);
         this.shopMenu.dispose();
         §_-L29§.§_-gs§(§_-21w§.§_-81H§);
         this.shopMenu = null;
      }
      
      public function §_-t2L§() : int
      {
         return this.§_-D1E§;
      }
      
      public function §_-F20§() : int
      {
         return this.§_-B2g§;
      }
      
      private function §_-7K§(param1:String) : void
      {
         §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_BUY),§_-s2a§.§_-K3t§(param1));
      }
      
      private function §_-122§() : void
      {
         §_-J2l§.§_-u2b§.hide();
         this.§_-p1l§();
         this.shopMenu.addEventListener(§_-M4§.WINDOW_SHOW,this.§_-l2J§);
         this.shopMenu.show();
      }
      
      private function §_-l2J§(param1:starling.events.Event) : void
      {
         if(this.§_-D2c§ != null)
         {
            this.§_-D2c§();
            this.§_-D2c§ = null;
         }
      }
      
      private function §_-614§(param1:§_-W1r§) : §_-230§
      {
         var _loc2_:§_-230§ = null;
         switch(param1.§_-3K§())
         {
            case §_-B1K§.§_-C23§:
            case §_-B1K§.§_-c1c§:
            case §_-B1K§.§_-A2L§:
            case §_-B1K§.§_-x1w§:
               _loc2_ = new BuyWeaponDialog();
               break;
            case §_-B1K§.HAT:
            case §_-B1K§.ARTIFACT:
               _loc2_ = new BuyEquipDialog();
         }
         return _loc2_;
      }
   }
}

