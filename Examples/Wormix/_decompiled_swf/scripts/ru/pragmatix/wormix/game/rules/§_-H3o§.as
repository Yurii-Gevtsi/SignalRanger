package ru.pragmatix.wormix.game.rules
{
   import §_-62§.§_-G1p§;
   import §_-73I§.*;
   import §_-73d§.§_-v1o§;
   import §_-82l§.§_-A2Y§;
   import §_-91A§.*;
   import §_-9e§.§_-73e§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-Af§.§_-d1B§;
   import §_-Af§.§_-k1I§;
   import §_-B1y§.*;
   import §_-C2t§.§_-92W§;
   import §_-CR§.§_-72p§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.*;
   import §_-G24§.§_-yi§;
   import §_-I1l§.§_-81Z§;
   import §_-K35§.§_-99§;
   import §_-L1H§.§_-C3e§;
   import §_-L2F§.§_-D2p§;
   import §_-RE§.§_-52p§;
   import §_-RE§.§_-qW§;
   import §_-RE§.§_-r2q§;
   import §_-SP§.§_-M4§;
   import §_-TF§.§_-q1j§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-XA§;
   import §_-U1i§.§_-hK§;
   import §_-Vg§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-o2i§;
   import §_-Y1b§.§_-s2Z§;
   import §_-Y1b§.§_-x1g§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-e2r§.EquipTopPanel;
   import §_-e2r§.§_-i1O§;
   import §_-fo§.*;
   import §_-j1w§.§_-L1x§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-k1u§.§_-Vr§;
   import §_-k2s§.§_-b0§;
   import §_-k2s§.§_-e1k§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-nE§.DowngradeCraftWeaponScreen;
   import §_-nE§.InfoCraftWeaponScreen;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.UpgradeCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-o14§.*;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s1Q§.Artifacts;
   import §_-u13§.MAC;
   import §_-y1s§.*;
   import §_-z1g§.§_-522§;
   import §_-z1g§.§_-y1S§;
   import com.greensock.loading.LoaderStatus;
   import com.hurlant.crypto.*;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.controls.ScrollPolicy;
   import feathers.core.ITextRenderer;
   import feathers.data.ArrayCollection;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.ILayoutData;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.display.DisplayObject;
   import flash.events.Event;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.flox.gui.controls.§_-N2r§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.CommandState;
   import ru.pragmatix.wormix.game.§_-3p§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-B1m§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.SetLocale;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F1r§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-H3o§ implements §_-l1m§
   {
      
      private var isBoss:Boolean;
      
      private var data:§_-s2Z§;
      
      private var profiles:Vector.<UserProfile>;
      
      private var §_-717§:uint;
      
      private var missionIds:Array;
      
      private var §_-11T§:§_-P2q§;
      
      private var §_-MF§:Vector.<§_-b0§>;
      
      private var §_-I3w§:Vector.<§_-e1k§>;
      
      private var §_-RZ§:Boolean = false;
      
      public function §_-H3o§(param1:§_-O2i§)
      {
         var _loc3_:§_-D2p§ = null;
         var _loc4_:int = 0;
         var _loc5_:§_-52p§ = null;
         var _loc6_:int = 0;
         super();
         this.data = param1.scenario;
         this.§_-717§ = param1.§_-sP§();
         this.profiles = param1.§_-D1h§();
         this.missionIds = [].concat(param1.missionIds);
         var _loc2_:§_-y1S§ = §_-r2q§.§_-Un§(param1.missionIds[0]);
         this.isBoss = §_-r2q§.isBoss(_loc2_.getId());
         if(!param1.§_-Dy§())
         {
            _loc3_ = _loc2_ is §_-D2p§ ? _loc2_ as §_-D2p§ : null;
            this.§_-RZ§ = this.isBoss && §_-k1i§(_loc2_.getId());
            if(this.§_-RZ§)
            {
               this.§_-11T§ = Boolean(_loc3_) && _loc3_.§_-K1z§() != null ? _loc3_.§_-K1z§() : _loc2_.§_-814§();
            }
            else if(Boolean(_loc3_) && !§_-X1j§.§_-A24§.§_-X2A§())
            {
               this.§_-11T§ = _loc3_.§_-ky§();
            }
            else
            {
               this.§_-11T§ = _loc2_.§_-814§();
            }
         }
         else
         {
            this.isBoss = true;
            _loc4_ = §_-qW§.getInstance().§_-71R§(param1.mapId,param1.missionIds);
            if(_loc4_ != -1)
            {
               _loc5_ = §_-52p§.§_-B2W§(_loc4_);
               this.§_-11T§ = _loc5_.§_-4z§();
            }
            else
            {
               this.§_-11T§ = new §_-P2q§([]);
            }
         }
         this.§_-I3w§ = new Vector.<§_-e1k§>(0);
         if(this.isBoss)
         {
            if(Boolean(param1.reagentsForBattle) && param1.reagentsForBattle.length > 0)
            {
               for each(_loc6_ in param1.reagentsForBattle)
               {
                  this.§_-I3w§.push(new §_-e1k§(_loc6_,1));
               }
            }
         }
         this.§_-MF§ = this.§_-11T§.§_-S1n§();
      }
      
      public static function §_-k1i§(param1:int) : Boolean
      {
         return param1 == §_-r2q§.§_-r9§(param1).§_-I3b§() + 1;
      }
      
      public function dispose() : void
      {
         if(this.data)
         {
            this.data.dispose();
         }
         this.data = null;
         this.profiles = null;
         this.§_-11T§ = null;
         this.§_-MF§ = null;
         this.§_-I3w§ = null;
      }
      
      public function §_-e2d§() : void
      {
         §_-X1j§.§_-B35§.§_-33m§();
      }
      
      public function §_-l26§(param1:§_-hK§) : void
      {
         this.data.clear();
      }
      
      public function §_-p1t§(param1:§_-J1§, param2:§_-XA§) : §_-hK§
      {
         var _loc4_:int = 0;
         var _loc5_:§_-d1B§ = null;
         var _loc3_:§_-hK§ = §_-Q2u§.§_-C1b§(param1,param2);
         if(_loc3_.result == §_-J1§.WINNER)
         {
            _loc3_.exp = this.§_-11T§.§_-p25§(true);
            _loc3_.money = this.§_-11T§.§_-51e§();
            _loc3_.realMoney = this.§_-11T§.§_-o1N§();
            _loc3_.exp = this.§_-11T§.§_-p25§();
            _loc3_.reagents = this.§_-I3w§.concat();
            if(this.isBoss || this.data is §_-o2i§)
            {
               §_-Q2u§.§_-f1x§(_loc3_);
            }
         }
         if(this.isBoss && this.§_-RZ§ && _loc3_.result == §_-J1§.WINNER)
         {
            _loc4_ = §_-r2q§.§_-Un§(this.missionIds[0]).getId();
            if(this.data is §_-d1B§ && _loc4_ == §_-N1V§.§_-I39§.§_-X1P§().§_-rm§ + 1)
            {
               §_-N1V§.§_-I39§.§_-X1P§().§_-rm§ = _loc4_;
            }
            else if(this.data is §_-x1g§ && _loc4_ == §_-N1V§.§_-I39§.§_-N1i§().§_-rm§ + 1)
            {
               §_-N1V§.§_-I39§.§_-N1i§().§_-rm§ = _loc4_;
            }
         }
         if(this.data is §_-d1B§)
         {
            _loc5_ = §_-d1B§(this.data);
            _loc3_.totalDamageToBoss = _loc5_.§_-ve§;
            _loc3_.totalDamageToPlayer = _loc5_.§_-63Z§;
            _loc3_.totalUsedItems = _loc5_.§_-j1i§;
         }
         return _loc3_;
      }
      
      public function §_-73f§() : §_-3p§
      {
         var _loc1_:§_-73e§ = this.data.§_-H1J§();
         if(this.data is §_-o2i§)
         {
            _loc1_.§_-52M§ = false;
         }
         if(this.profiles.length > 1)
         {
            §_-73e§.§_-H2v§(_loc1_);
         }
         var _loc2_:§_-3p§ = new §_-3p§(this.data.§_-Tr§(this.profiles,this.§_-717§),this.§_-717§,_loc1_);
         this.data.activate();
         this.data.§_-e2d§();
         if(this.data is §_-d1B§)
         {
            §_-d1B§(this.data).activateAchievs(this.missionIds[0],this.§_-RZ§);
         }
         return _loc2_;
      }
      
      public function §_-B2C§() : void
      {
         this.data.§_-9T§();
      }
      
      public function §_-Q1M§(param1:§_-O2i§, param2:§_-hK§) : §_-73y§
      {
         var _loc3_:§_-y1S§ = §_-r2q§.§_-Un§(param1.missionIds[0]);
         return §_-r2q§.isBoss(_loc3_.getId()) ? §_-81Z§.create(param1,param2) : §_-yi§.create(param1,param2);
      }
      
      public function §_-024§() : Boolean
      {
         return true;
      }
   }
}

