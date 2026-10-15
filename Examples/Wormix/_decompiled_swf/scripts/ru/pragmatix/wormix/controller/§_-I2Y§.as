package ru.pragmatix.wormix.controller
{
   import §_-02q§.§_-4s§;
   import §_-12W§.§_-J3J§;
   import §_-12W§.§_-X2w§;
   import §_-12W§.§_-l23§;
   import §_-21p§.§_-4w§;
   import §_-23P§.§_-V2T§;
   import §_-31W§.AwardCell;
   import §_-31W§.§_-Kb§;
   import §_-43V§.Random;
   import §_-533§.§_-c2b§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-zF§;
   import §_-73I§.*;
   import §_-82a§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B1y§.§_-ts§;
   import §_-B2z§.§_-63i§;
   import §_-C2t§.§_-92W§;
   import §_-CF§.§_-E19§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-H16§.*;
   import §_-Ly§.§_-81L§;
   import §_-P1B§.§_-b21§;
   import §_-P2i§.§_-b7§;
   import §_-RE§.*;
   import §_-S§.*;
   import §_-SP§.§_-M4§;
   import §_-Vg§.*;
   import §_-W§.§_-Ap§;
   import §_-Y1O§.§_-r1o§;
   import §_-Y1O§.§_-yA§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.*;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-fo§.§_-hH§;
   import §_-g2r§.§_-x8§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-d2j§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-lh§.CraftRoomPanel;
   import §_-m0§.§_-X1F§;
   import §_-nE§.§_-p16§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-s20§.§_-d21§;
   import §_-sQ§.§_-V1n§;
   import §_-w2W§.AchieveRoomScreenDesktop;
   import §_-w2W§.ArmoryScreenDesktop;
   import §_-w2W§.CraftRoomScreenDesktop;
   import §_-w2W§.DesktopRaceRoomScreen;
   import §_-w2W§.MainMenuScreenDesktop;
   import §_-w2W§.TeamRoomScreenDesktop;
   import §_-w2W§.WardrobeRoomScreen;
   import §_-w2W§.§_-21w§;
   import §_-w2W§.§_-72M§;
   import §_-w2W§.§_-HV§;
   import §_-w2W§.§_-I2R§;
   import §_-w2W§.§_-I3c§;
   import §_-w2W§.§_-h1Z§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import §_-y1s§.*;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import §_-zI§.§_-X1q§;
   import com.distriqt.extension.pushnotifications.PushNotifications;
   import com.greensock.*;
   import com.greensock.loading.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import com.worlize.websocket.*;
   import feathers.controls.BasicButton;
   import feathers.controls.Button;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.core.FeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.layout.VerticalLayoutData;
   import feathers.themes.FontColor;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import flash.events.MouseEvent;
   import flash.events.TimerEvent;
   import flash.geom.ColorTransform;
   import flash.geom.Rectangle;
   import flash.net.URLRequest;
   import flash.net.URLVariables;
   import flash.text.TextField;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getDefinitionByName;
   import flash.utils.getTimer;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.particle.ParticleEffectType;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.menu.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.*;
   import ru.pragmatix.wormix.messages.server.RemoveFromGroupResult;
   import ru.pragmatix.wormix.messages.server.ReorderGroupResult;
   import ru.pragmatix.wormix.messages.server.ToggleTeamMemberResult;
   import ru.pragmatix.wormix.messages.structures.CostStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DynamiteStick;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.IPooledAmmo;
   import ru.pragmatix.wormix.weapons.ammo.phase.HarmPhase;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.text.TextField;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   use namespace bi_internal;
   
   public class §_-I2Y§ extends flash.events.EventDispatcher
   {
      
      public static const EVENT_TEAM_CHANGED:String = "EVENT_TEAM_CHANGED";
      
      private var profile:UserProfile;
      
      public var §_-o1P§:Vector.<uint> = new Vector.<uint>();
      
      public var §_-01I§:Vector.<uint> = new Vector.<uint>();
      
      private var §_-a2e§:Vector.<§_-r1o§> = new Vector.<§_-r1o§>(0);
      
      public function §_-I2Y§()
      {
         super();
         §_-J2g§.§_-o1R§(RemoveFromGroupResult,this.§_-W2d§);
      }
      
      public function §_-u2d§(param1:UserProfile, param2:§_-E19§, param3:uint, param4:int, param5:Function = null) : void
      {
         §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.ADD_IN_GROUP),§_-fH§.§_-X2g§,§_-bs§.§_-d1Z§(this.§_-U2F§,this,[param1,param2,param3,param4,param5]));
      }
      
      public function replace(param1:UserProfile, param2:§_-E19§, param3:uint, param4:int, param5:int, param6:Function) : void
      {
         §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.REPLACE_IN_GROUP_CONFIRMATION),§_-fH§.§_-X2g§,§_-bs§.§_-d1Z§(this.§_-FY§,this,[param1,param2,param3,param4,param5,param6]));
      }
      
      private function §_-FY§(param1:UserProfile, param2:§_-E19§, param3:uint, param4:int, param5:int, param6:Function) : void
      {
         var _loc7_:Function = §_-bs§.§_-d1Z§(this.§_-Q2s§,this,[param1,param4,param6,param5],true);
         §_-J2l§.§_-u2b§.show(§_-X1j§.§_-02s§.§_-v1r§,param1,param2,param3,param4,param5,_loc7_);
      }
      
      public function §_-M2i§(param1:UserProfile) : void
      {
         var _loc2_:UserProfile = null;
         this.§_-a2e§.length = 0;
         for each(_loc2_ in param1.§_-A1U§())
         {
            this.§_-a2e§.push(new §_-r1o§(_loc2_.getId(),_loc2_.§_-H38§()));
         }
      }
      
      private function §_-U2F§(param1:UserProfile, param2:§_-E19§, param3:uint, param4:int, param5:Function) : void
      {
         var _loc6_:Function = §_-bs§.§_-d1Z§(this.§_-Q2s§,this,[param1,param4,param5],true);
         §_-J2l§.§_-u2b§.show(§_-X1j§.§_-02s§.§_-Wi§,param1,param2,param3,param4,_loc6_);
      }
      
      private function §_-Q2s§(param1:Boolean, param2:UserProfile, param3:int, param4:Function, param5:int = 0) : void
      {
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         §_-J2l§.§_-u2b§.hide();
         if(param1)
         {
            if(param3 == §_-yA§.§_-v2X§)
            {
               param2.§_-L§(null);
               param2.§_-aV§(null);
               §_-N1V§.postController.newTeamMate(param2);
            }
            param2.§_-62T§(param3);
            Application.userProfile.build.§_-Fc§(param2,param3 != §_-yA§.§_-v2X§,param5);
            if(param5 == 0)
            {
               this.§_-o1P§.push(param2.getId());
               this.§_-a2e§.push(new §_-r1o§(param2.getId(),param2.§_-H38§()));
            }
            else
            {
               _loc6_ = 0;
               while(_loc6_ < this.§_-o1P§.length)
               {
                  _loc7_ = int(this.§_-o1P§[_loc6_]);
                  if(_loc7_ == param5)
                  {
                     this.§_-o1P§[_loc6_] = param2.getId();
                     this.§_-a2e§[_loc6_].teamMemberId = param5;
                     break;
                  }
                  _loc6_++;
               }
            }
            this.§_-Q29§();
         }
         param4(param1);
      }
      
      public function §_-s19§(param1:UserProfile, param2:Function = null) : void
      {
         this.§_-X17§(param1,false,param2);
      }
      
      public function §_-X17§(param1:UserProfile, param2:Boolean, param3:Function = null) : void
      {
         var _loc4_:Object = new ToggleTeamMember(param1.getId(),param2);
         var _loc5_:Function = §_-bs§.§_-d1Z§(this.§_-t24§,this,[param3],true);
         var _loc6_:Function = §_-bs§.§_-d1Z§(this.§_-e1T§,this,[param3]);
         §_-J2l§.§_-R25§.show(§_-d21§.§_-vH§,§_-J2g§.§_-i1N§,_loc4_,ToggleTeamMemberResult,_loc5_,_loc6_);
      }
      
      private function §_-t24§(param1:§_-63i§, param2:Function) : void
      {
         var _loc3_:ToggleTeamMemberResult = ToggleTeamMemberResult(param1.message);
         this.§_-i15§(_loc3_.result == 0,param2);
      }
      
      private function §_-e1T§(param1:Function) : void
      {
         this.§_-i15§(false,param1);
      }
      
      public function §_-i15§(param1:Boolean, param2:Function) : void
      {
         §_-J2l§.§_-R25§.hide();
         if(param2 != null)
         {
            param2(param1);
         }
      }
      
      public function §_-iF§(param1:UserProfile, param2:int, param3:int, param4:Function) : void
      {
         var message:ReorderGroup;
         var successCallback:Function;
         var failCallback:Function;
         var reorderTeamId:int = 0;
         var newIndexInTeam:int = 0;
         var teamOwner:UserProfile = param1;
         var indexInTeam:int = param2;
         var reorderAdd:int = param3;
         var callback:Function = param4;
         var teamIds:Vector.<int> = §_-71v§.§_-91k§(teamOwner);
         try
         {
            reorderTeamId = teamIds[indexInTeam];
            newIndexInTeam = indexInTeam + reorderAdd;
            teamIds[indexInTeam] = teamIds[newIndexInTeam];
            teamIds[newIndexInTeam] = reorderTeamId;
         }
         catch(e:Error)
         {
            return;
         }
         message = new ReorderGroup();
         message.reorderedWormGroup = teamIds;
         successCallback = §_-bs§.§_-d1Z§(this.§_-J19§,this,[teamOwner,teamIds,callback],true);
         failCallback = §_-bs§.§_-d1Z§(this.§_-NN§,this,[teamOwner,teamIds,callback]);
         §_-J2l§.§_-R25§.show(§_-d21§.§_-vH§,§_-J2g§.§_-i1N§,message,ReorderGroupResult,successCallback,failCallback);
      }
      
      private function §_-J19§(param1:§_-63i§, param2:UserProfile, param3:Vector.<int>, param4:Function) : void
      {
         var _loc5_:ReorderGroupResult = ReorderGroupResult(param1.message);
         this.§_-C2I§(_loc5_.result == ReorderGroupResult.SUCCESS,param2,param3,param4);
      }
      
      private function §_-NN§(param1:UserProfile, param2:Vector.<int>, param3:Function) : void
      {
         this.§_-C2I§(false,param1,param2,param3);
      }
      
      private function §_-C2I§(param1:Boolean, param2:UserProfile, param3:Vector.<int>, param4:Function) : void
      {
         var _loc5_:Array = null;
         var _loc6_:int = 0;
         var _loc7_:UserProfile = null;
         if(param1 == false)
         {
            §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ERROR),§_-s2a§.§_-K3t§(§_-s2a§.ERROR_SAVING));
         }
         else if(param1 == true && param2.equals(Application.userProfile))
         {
            _loc5_ = [];
            _loc6_ = 0;
            while(_loc6_ < param3.length)
            {
               _loc7_ = §_-71v§.§_-U1x§(param2,param3[_loc6_]);
               _loc5_.push(_loc7_);
               _loc6_++;
            }
            Application.userProfile.§_-fR§(_loc5_);
         }
         §_-J2l§.§_-R25§.hide();
      }
      
      private function §_-W2d§(param1:§_-63i§) : void
      {
         var _loc2_:Array = null;
         switch(RemoveFromGroupResult(param1.message).result)
         {
            case RemoveFromGroupResult.SUCCESS:
               _loc2_ = Application.userProfile.§_-A1U§();
               §_-ts§.remove(_loc2_,this.profile);
               §_-N1V§.postController.kickTeamMate(this.profile);
               this.§_-Q29§();
               break;
            case RemoveFromGroupResult.ERROR:
               §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ERROR),§_-s2a§.§_-K3t§(§_-s2a§.ERROR_SAVING));
         }
         §_-J2l§.§_-R25§.hide();
      }
      
      public function §_-t2D§(param1:UserProfile) : void
      {
         var _loc2_:UserProfile = null;
         this.§_-o1P§.length = 0;
         for each(_loc2_ in param1.§_-A1U§())
         {
            this.§_-o1P§[this.§_-o1P§.length] = _loc2_.getId();
         }
      }
      
      public function dispose() : void
      {
         this.profile = null;
         this.§_-o1P§ = null;
         this.§_-01I§ = null;
      }
      
      public function §_-Q29§() : void
      {
         dispatchEvent(new flash.events.Event(EVENT_TEAM_CHANGED));
      }
   }
}

