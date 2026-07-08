package ru.pragmatix.wormix.social.common.post
{
   import §_-02q§.§_-4s§;
   import §_-03T§.§_-2p§;
   import §_-12a§.KamikazeBuff;
   import §_-21p§.§_-4w§;
   import §_-21p§.§_-N1H§;
   import §_-2H§.§_-91a§;
   import §_-2H§.§_-A1R§;
   import §_-2H§.§_-N15§;
   import §_-2H§.§_-Y1i§;
   import §_-31N§.§_-42G§;
   import §_-31W§.§_-8§;
   import §_-31W§.§_-Kb§;
   import §_-52L§.§_-u2x§;
   import §_-62§.*;
   import §_-73d§.§_-a2s§;
   import §_-73d§.§_-v1o§;
   import §_-82l§.§_-A2Y§;
   import §_-A1K§.§_-TA§;
   import §_-A2U§.*;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B1y§.§_-o2k§;
   import §_-B1y§.§_-ts§;
   import §_-B2z§.§_-63i§;
   import §_-C3f§.*;
   import §_-CF§.§_-E19§;
   import §_-CR§.§_-72p§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-DJ§.§_-q2F§;
   import §_-F39§.§_-a2W§;
   import §_-H3§.*;
   import §_-H9§.§_-NX§;
   import §_-J3L§.§_-h1D§;
   import §_-L1H§.§_-b2K§;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-RE§.§_-r2q§;
   import §_-SP§.§_-M4§;
   import §_-Ta§.BuyVipDialog;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-V1H§.§_-m1d§;
   import §_-Y1O§.*;
   import §_-Y1b§.*;
   import §_-YF§.§_-q2v§;
   import §_-ZB§.§_-81R§;
   import §_-b1F§.§_-G4§;
   import §_-bI§.*;
   import §_-dS§.§_-D3a§;
   import §_-g2C§.GlobalChatTextMessageItemRenderer;
   import §_-g2C§.§_-62U§;
   import §_-g2C§.§_-I1R§;
   import §_-g2C§.§_-K2A§;
   import §_-g2C§.§_-K2u§;
   import §_-g2C§.§_-p8§;
   import §_-g2C§.§_-w1K§;
   import §_-hO§.§_-BY§;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-j1w§.§_-lD§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-W2f§;
   import §_-l1K§.§_-cZ§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-sQ§.§_-H3D§;
   import §_-u1P§.§_-Po§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-lp§;
   import §_-z1g§.§_-y1S§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-X1q§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.greensock.loading.core.*;
   import config.§_-B1K§;
   import feathers.controls.Button;
   import feathers.controls.NumericStepper;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
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
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.DisplayObject;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.events.ProgressEvent;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.net.Socket;
   import flash.net.URLRequest;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.IUserProfile;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.flox.social.post.AbstractPostController;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.AssembleStuffResult;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.rewards.§_-d2F§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-81K§;
   import ru.pragmatix.wormix.utils.§_-MN§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.VilePumpkin;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import ru.pragmatix.wormix.weapons.render.IRenderer;
   import starling.display.Canvas;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.extensions.ParticleAura;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-Gx§ extends AbstractPostController implements §_-81R§
   {
      
      private static var instance:§_-Gx§;
      
      public static const §_-G3S§:int = 100;
      
      protected var §_-T26§:§_-Y1i§ = new §_-Y1i§();
      
      protected var §_-p1X§:§_-A1R§ = new §_-A1R§();
      
      protected var §_-2R§:§_-N15§;
      
      protected var §_-M1K§:§_-91a§ = new §_-91a§();
      
      protected var §_-u2u§:§_-NX§;
      
      private var §_-T1d§:uint = 0;
      
      public function §_-Gx§(param1:§_-o2k§, param2:ISocialController, param3:§_-NX§, param4:String, param5:§_-Y1i§ = null, param6:§_-91a§ = null, param7:§_-A1R§ = null)
      {
         super(param1,param2,param4 || "data/icons");
         this.§_-u2u§ = param3;
         this.§_-2R§ = new §_-N15§();
         this.§_-2R§.§_-C2l§();
         if(activityPoster)
         {
            this.§_-T26§.§_-C2l§();
         }
         if(wallPoster)
         {
            this.§_-p1X§.§_-C2l§();
         }
         if(notifier)
         {
            this.§_-M1K§.§_-C2l§();
         }
         instance = this;
      }
      
      protected static function postOnWallAfterNetworkChoose(param1:String, param2:Object = null, param3:String = null, param4:Function = null) : void
      {
         var _loc5_:Function = null;
         if(instance.wallPoster)
         {
            _loc5_ = §_-bs§.§_-d1Z§(§_-w5§,null,[param4]);
            §_-J2l§.§_-u2b§.show(instance.postOnWall,param1,param2,param3,_loc5_);
         }
      }
      
      protected static function postOnWallImageAfterNetworkChoose(param1:String, param2:Object = null, param3:BitmapData = null, param4:Function = null) : void
      {
         var _loc5_:String = null;
         var _loc6_:Bitmap = null;
         var _loc7_:Function = null;
         if(Boolean(instance.wallPoster) && Boolean(param3))
         {
            _loc5_ = §_-F1P§.format(param1,param2);
            _loc6_ = new Bitmap(param3);
            _loc7_ = §_-bs§.§_-d1Z§(§_-w5§,null,[param4]);
            §_-J2l§.§_-u2b§.show(instance.wallPoster.wallPostLoadedImg,_loc5_,"",_loc6_,_loc7_);
         }
      }
      
      private static function §_-w5§(param1:Boolean, param2:Function = null) : void
      {
         §_-J2l§.§_-u2b§.hide();
         if(param2 != null)
         {
            param2(param1);
         }
      }
      
      protected static function notifyAfterNetworkChoose(param1:IUserProfile, param2:String, param3:Object = null, param4:String = null, param5:Function = null) : void
      {
         instance.notify(param1,param2,param3,param4,§_-M28§);
      }
      
      private static function §_-M28§(param1:Boolean) : void
      {
         if(!param1)
         {
            §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ERROR),§_-s2a§.§_-K3t§(§_-s2a§.ERROR_SOCIAL));
         }
      }
      
      protected static function postActivityAfterNetworkChoose(param1:String, param2:Object = null, param3:String = null, param4:String = null, param5:String = null) : void
      {
         instance.postActivity(param1,param2,param3,param4,param5);
      }
      
      public function dispose() : void
      {
         activityPoster = null;
         wallPoster = null;
         notifier = null;
         iconsDir = null;
         context = null;
      }
      
      public function §_-n2r§(param1:§_-B2i§) : void
      {
         var _loc2_:String = null;
         if(!this.§_-hh§())
         {
            return;
         }
         switch(param1.configName)
         {
            case "boxer":
               _loc2_ = this.§_-u2u§.boxerIcon;
               break;
            case "demon":
               _loc2_ = this.§_-u2u§.demonIcon;
               break;
            case "rabbit":
               _loc2_ = this.§_-u2u§.rabbitIcon;
               break;
            case "zombie":
               _loc2_ = this.§_-u2u§.zombieIcon;
               break;
            case "cat":
               _loc2_ = this.§_-u2u§.catIcon;
               break;
            case "dragon":
               _loc2_ = this.§_-u2u§.dragonIcon;
               break;
            case "bot":
               _loc2_ = this.§_-u2u§.robotIcon;
         }
         this.§_-3L§(this.§_-p1X§.§_-F1J§,this.§_-p1X§.§_-M1f§,this.§_-p1X§.§_-n2r§,{"race":§_-s2a§.§_-K3t§(param1.getName())},_loc2_);
         if(activityPoster)
         {
            this.§_-wY§(this.§_-T26§.§_-n2r§,{"raceName":§_-s2a§.§_-K3t§(param1.getName())},_loc2_);
         }
      }
      
      private function §_-j1b§(param1:UserProfile, param2:§_-hK§, param3:Boolean = false) : void
      {
         var _loc4_:String = null;
         var _loc5_:Object = null;
         if(activityPoster)
         {
            _loc4_ = §_-33r§.§_-c2x§(param1);
            if(Boolean(param2) && Boolean(param1.§_-Zp§()))
            {
               _loc5_ = {
                  "enemyName":_loc4_,
                  "rating":String(param1.§_-Zp§())
               };
               this.§_-wY§(this.§_-T26§.winInPvp,_loc5_,this.§_-u2u§.winInPvp);
            }
            else
            {
               this.§_-wY§(this.§_-T26§.§_-U2q§,{"enemyName":_loc4_},this.§_-u2u§.winInPvp);
            }
         }
      }
      
      public function §_-lT§(param1:§_-O2i§, param2:§_-hK§, param3:Function) : void
      {
      }
      
      public function rubyFoundInHouse(param1:UserProfile) : void
      {
         var _loc2_:Object = {"name":§_-33r§.§_-J2O§(param1)};
         if(activityPoster)
         {
            this.§_-wY§(this.§_-T26§.rubyFoundInHouse,_loc2_,this.§_-u2u§.rubyFoundInHouse);
         }
         this.§_-3L§(this.§_-p1X§.§_-V2H§,this.§_-p1X§.§_-M1f§,this.§_-p1X§.rubyFoundInHouse,_loc2_,this.§_-u2u§.rubyFoundInHouse);
      }
      
      public function kickTeamMate(param1:UserProfile) : void
      {
         if(this.§_-hh§() && Boolean(activityPoster))
         {
            this.§_-wY§(this.§_-T26§.kickTeamMate,{"name":§_-33r§.§_-J2O§(param1)},this.§_-u2u§.kickTeamMate);
         }
         if(this.§_-Ez§())
         {
            this.§_-r§(param1,this.§_-M1K§.§_-21W§,this.§_-M1K§.§_-Q1b§,this.§_-M1K§.kickTeamMate,null,this.§_-u2u§.kickTeamMate);
         }
      }
      
      private function §_-33K§() : void
      {
         if(activityPoster)
         {
            this.§_-wY§(this.§_-T26§.missionComplete,null,this.§_-u2u§.missionComplete);
         }
      }
      
      public function enter4DaySequence() : void
      {
         if(activityPoster)
         {
            this.§_-wY§(this.§_-T26§.enter4DaySequence,null,this.§_-u2u§.enter4DaySequence);
         }
      }
      
      public function share4DayBonus() : void
      {
         if(!this.§_-hh§())
         {
            return;
         }
         var _loc1_:String = this.§_-p1X§.enter4DayTitle;
         var _loc2_:String = this.§_-p1X§.§_-M1f§;
         var _loc3_:String = this.§_-p1X§.enter4DaySequence;
         var _loc4_:Object = null;
         var _loc5_:String = this.§_-u2u§.enter4DaySequence;
         this.§_-3L§(_loc1_,_loc2_,_loc3_,_loc4_,_loc5_);
      }
      
      protected function §_-J3G§(param1:§_-4l§) : String
      {
         var _loc2_:String = null;
         if(param1.type == §_-522§.§_-a1A§)
         {
            _loc2_ = this.§_-u2u§.enter5DaySequence_2mission;
         }
         else if(param1.type == §_-522§.REACTION)
         {
            _loc2_ = this.§_-u2u§.enter5DaySequence_reaction;
         }
         else if(param1.type == §_-522§.§_-J2y§)
         {
            _loc2_ = this.§_-u2u§.enter5DaySequence_fuzzes;
         }
         else if(param1.type == §_-522§.§_-Mu§)
         {
            _loc2_ = this.§_-u2u§.enter5DaySequence_rubies;
         }
         return _loc2_;
      }
      
      public function enter5DaySequence(param1:§_-4l§) : void
      {
         var _loc2_:String = null;
         var _loc3_:String = null;
         if(activityPoster)
         {
            if(param1.type == §_-522§.§_-a1A§)
            {
               _loc2_ = this.§_-T26§.enter5DaySequence_2mission;
            }
            else if(param1.type == §_-522§.REACTION)
            {
               _loc2_ = this.§_-T26§.enter5DaySequence_reaction;
            }
            else if(param1.type == §_-522§.§_-J2y§)
            {
               _loc2_ = this.§_-T26§.enter5DaySequence_fuzzes;
            }
            else if(param1.type == §_-522§.§_-Mu§)
            {
               _loc2_ = this.§_-T26§.enter5DaySequence_rubies;
            }
            _loc3_ = this.§_-J3G§(param1);
            this.§_-wY§(this.§_-T26§.enter5DaySequence,{"reward":§_-F1P§.format(_loc2_,{"count":param1.count})},_loc3_);
         }
      }
      
      public function share5DaySequence(param1:§_-4l§) : void
      {
         if(!this.§_-hh§())
         {
            return;
         }
         var _loc2_:String = this.§_-p1X§.enter5DayTitle;
         var _loc3_:String = this.§_-p1X§.§_-M1f§;
         var _loc4_:String = this.§_-p1X§.enter5DaySequence;
         var _loc5_:Object = null;
         var _loc6_:String = this.§_-J3G§(param1);
         this.§_-3L§(_loc2_,_loc3_,_loc4_,_loc5_,_loc6_);
      }
      
      public function §_-n2J§(param1:uint) : void
      {
         var _loc2_:String = this.§_-u2u§.§_-G1f§(Application.userProfile.§_-P2e§());
         this.§_-G3V§(§_-s2a§.§_-K3t§(this.§_-p1X§.§_-n2J§),{"level":param1},_loc2_);
         if(activityPoster)
         {
            this.§_-wY§(this.§_-T26§.§_-n2J§,{"level":param1},_loc2_);
         }
      }
      
      protected function §_-jL§(param1:String, param2:§_-zF§) : void
      {
         var _loc3_:String = this.§_-p1X§.§_-92U§;
         var _loc4_:String = this.§_-p1X§.§_-M1f§;
         var _loc5_:String = this.§_-p1X§.§_-jL§;
         var _loc6_:Object = {"hat":param1};
         var _loc7_:String = this.§_-u2u§.§_-b1i§(param2);
         this.§_-3L§(_loc3_,_loc4_,_loc5_,_loc6_,_loc7_);
      }
      
      protected function §_-op§(param1:String, param2:§_-T1y§) : void
      {
         var _loc3_:String = this.§_-p1X§.§_-4X§;
         var _loc4_:String = this.§_-p1X§.§_-M1f§;
         var _loc5_:String = this.§_-p1X§.§_-op§;
         var _loc6_:Object = {"artifact":param1};
         var _loc7_:String = this.§_-u2u§.§_-K14§(param2);
         this.§_-3L§(_loc3_,_loc4_,_loc5_,_loc6_,_loc7_);
      }
      
      protected function §_-e7§(param1:String, param2:§_-O2H§) : void
      {
      }
      
      protected function §_-S2D§(param1:String, param2:§_-O2H§) : void
      {
         this.§_-3L§(this.§_-p1X§.§_-WR§,this.§_-p1X§.§_-M1f§,this.§_-p1X§.§_-e7§,{"weapon":param1},this.§_-u2u§.§_-l1F§(param2));
      }
      
      protected function newItems(param1:Array, param2:Array) : void
      {
         if(activityPoster)
         {
            this.§_-wY§(this.§_-T26§.newItems,{"items":param1.join(", ")},this.§_-u2u§.newItems);
         }
      }
      
      public function §_-A2C§(param1:String) : void
      {
         if(activityPoster)
         {
            this.§_-wY§(this.§_-T26§.§_-A2C§,null,this.§_-u2u§.postStatus,param1);
         }
      }
      
      public function newTeamMate(param1:UserProfile) : void
      {
         if(this.§_-hh§() && Boolean(activityPoster))
         {
            this.§_-wY§(this.§_-T26§.newTeamMate,{"name":§_-33r§.§_-J2O§(param1)},this.§_-u2u§.newTeamMate);
         }
         if(this.§_-Ez§())
         {
            if(!this.§_-X1t§(Application.userProfile,param1))
            {
               return;
            }
            this.§_-r§(param1,§_-s2a§.TITLE_ATTENTION,this.§_-M1K§.§_-U1P§,this.§_-M1K§.newTeamMate,null,this.§_-u2u§.newTeamMate);
            §_-X1j§.§_-2I§.§_-V16§(param1.getId(),Application.userProfile.getCharName(),"YOU_ARE_HIRED");
         }
      }
      
      public function callToPvp(param1:UserProfile, param2:UserProfile) : void
      {
         var _loc3_:String = this.§_-a26§(param1,param2);
         var _loc4_:String = this.§_-u2u§.callToPvp[§_-X1j§.randomSeed.§_-Fh§(0,this.§_-u2u§.callToPvp.length - 1)];
         if(Boolean(notifier) && Boolean(param2))
         {
            this.checkSocialNetworkAndNotify(param2,_loc3_,null,_loc4_);
         }
         else if(activityPoster)
         {
            this.§_-wY§(_loc3_,iconsDir + _loc4_,§_-s2a§.PLAY_CAPTION);
         }
      }
      
      protected function §_-a26§(param1:UserProfile, param2:UserProfile) : String
      {
         var _loc3_:Object = {
            "rating":param1.§_-Zp§(),
            "name":§_-33r§.§_-J2O§(param2)
         };
         return §_-F1P§.format(param1.§_-Zp§() ? §_-s2a§.§_-K3t§(this.§_-2R§.callToPvp) : §_-s2a§.§_-K3t§(this.§_-2R§.§_-623§),_loc3_);
      }
      
      public function activityPumpFriendsReaction(param1:UserProfile, param2:uint) : void
      {
         if(activityPoster)
         {
            this.§_-wY§(Application.userProfile.getSocialData().isFemale() ? §_-s2a§.§_-K3t§(this.§_-T26§.§_-D2O§) : §_-s2a§.§_-K3t§(this.§_-T26§.activityPumpFriendsReaction),{
               "name":§_-33r§.§_-J2O§(param1),
               "count":param2
            },this.§_-u2u§.activityPumpFriendsReaction);
         }
      }
      
      public function requestPumpReaction(param1:UserProfile, param2:Boolean) : void
      {
         if(param1)
         {
            if(param2)
            {
               this.§_-r§(param1,"",this.§_-M1K§.§_-611§,this.§_-M1K§.requestPumpReaction,null,this.§_-u2u§.requestPumpReaction);
            }
            else
            {
               this.checkSocialNetworkAndNotify(param1,§_-s2a§.§_-K3t§(this.§_-M1K§.requestPumpReaction),null,this.§_-u2u§.requestPumpReaction);
            }
         }
         else
         {
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(this.§_-M1K§.§_-t2f§));
         }
      }
      
      public function §_-d2f§(param1:UserProfile, param2:Function) : void
      {
         if(this.§_-M1K§.remindAboutLose)
         {
            this.§_-r§(param1,this.§_-M1K§.§_-vi§,this.§_-M1K§.§_-j2w§,this.§_-M1K§.remindAboutLose,null,this.§_-u2u§.remindAboutLose,param2);
         }
      }
      
      public function §_-91Y§(param1:§_-8§, param2:int, param3:BitmapData = null) : void
      {
         var _loc4_:uint = this.§_-p1X§.§_-on§.length;
         var _loc5_:String = param1.levels[param2].name;
         var _loc6_:String = §_-F1P§.format(§_-s2a§.§_-K3t§(this.§_-p1X§.§_-on§[int(Math.random() * _loc4_)]),{
            "achievementName":§_-s2a§.§_-K3t§(_loc5_),
            "userName":§_-33r§.§_-c2x§(Application.userProfile)
         });
         var _loc7_:§_-Kb§ = param1.levels[param2];
         var _loc8_:String = this.§_-u2u§.§_-g18§(_loc7_);
         if(param3)
         {
            this.§_-H3C§(_loc6_,null,param3);
         }
         else
         {
            this.§_-G3V§(_loc6_,null,_loc8_);
         }
      }
      
      public function §_-q1s§(param1:§_-W1r§) : void
      {
         if(!this.§_-hh§())
         {
            return;
         }
         var _loc2_:String = this.§_-p1X§.§_-827§[int(Math.random() * this.§_-p1X§.§_-827§.length)];
         this.§_-3L§(this.§_-p1X§.§_-71j§,this.§_-p1X§.§_-M1f§,_loc2_,{},this.§_-u2u§.§_-5b§(param1));
      }
      
      public function §_-Kq§(param1:Boolean, param2:§_-O2i§, param3:§_-J1§, param4:Function = null) : void
      {
         var _loc5_:String = null;
         var _loc6_:String = null;
         var _loc7_:UserProfile = §_-33r§.§_-t2t§(param2.§_-b2W§());
         var _loc8_:Object = {
            "userName":§_-33r§.§_-c2x§(Application.userProfile),
            "friendName":(_loc7_ ? §_-33r§.§_-c2x§(_loc7_) : ""),
            "bossName":§_-s2a§.§_-K3t§(§_-r2q§.§_-Un§(param2.missionIds[0]).getName()),
            "bossName2":(param2.§_-Dy§() ? §_-s2a§.§_-K3t§(§_-r2q§.§_-Un§(param2.missionIds[1]).getName()) : "")
         };
         _loc6_ = this.§_-w1x§(param2,param3,_loc7_);
         _loc5_ = §_-F1P§.format(§_-s2a§.§_-K3t§(this.§_-23Y§(param2,param3,_loc7_)),_loc8_);
         if(param1)
         {
            this.§_-G3V§(_loc5_,null,_loc6_,param4);
         }
         if(activityPoster)
         {
            postActivity(_loc5_,null,_loc6_);
         }
      }
      
      private function §_-w1x§(param1:§_-O2i§, param2:§_-J1§, param3:UserProfile) : String
      {
         if(param1.§_-Dy§())
         {
            return this.§_-u2u§.§_-o2z§(param2.equals(§_-J1§.WINNER),param3 != null);
         }
         var _loc4_:§_-y1S§ = §_-r2q§.§_-Un§(param1.missionIds[0]);
         if(param2 == §_-J1§.WINNER)
         {
            return this.§_-u2u§.§_-RX§(_loc4_.getId());
         }
         return this.§_-u2u§.bossIcon;
      }
      
      private function §_-23Y§(param1:§_-O2i§, param2:§_-J1§, param3:UserProfile) : String
      {
         if(param1.§_-Dy§())
         {
            return this.§_-AJ§(param3,param2);
         }
         if(param1.isCoopBoss())
         {
            return this.§_-67§(param3,param2);
         }
         return this.§_-51O§(param1,param2);
      }
      
      private function §_-AJ§(param1:UserProfile, param2:§_-J1§) : String
      {
         if(param2.equals(§_-J1§.WINNER))
         {
            return param1 ? this.§_-T26§.§_-a2t§ : this.§_-T26§.§_-TP§;
         }
         if(param2.equals(§_-J1§.DRAW_GAME))
         {
            return this.§_-p1X§.§_-u2G§;
         }
         return param1 ? this.§_-p1X§.§_-82M§ : this.§_-p1X§.§_-D2u§;
      }
      
      private function §_-67§(param1:UserProfile, param2:§_-J1§) : String
      {
         if(param2.equals(§_-J1§.WINNER))
         {
            return param1 ? this.§_-T26§.§_-22v§ : this.§_-T26§.§_-J3N§;
         }
         if(param2.equals(§_-J1§.DRAW_GAME))
         {
            return this.§_-p1X§.§_-u2G§;
         }
         return param1 ? this.§_-p1X§.§_-1p§ : this.§_-p1X§.§_-d1c§;
      }
      
      private function §_-51O§(param1:§_-O2i§, param2:§_-J1§) : String
      {
         var _loc3_:§_-y1S§ = §_-r2q§.§_-Un§(param1.missionIds[0]);
         if(param2.equals(§_-J1§.WINNER))
         {
            return this.§_-p1X§.§_-S1Q§[_loc3_.getId() - 1];
         }
         if(param2.equals(§_-J1§.§_-23m§))
         {
            return this.§_-p1X§.§_-d1c§;
         }
         return this.§_-p1X§.§_-u2G§;
      }
      
      public function §_-z1j§(param1:String, param2:Function = null) : void
      {
         this.§_-G3V§(§_-s2a§.§_-K3t§(this.§_-p1X§.§_-V2a§),{"name_mission":param1},this.§_-u2u§.tutorialIcon,param2);
      }
      
      public function §_-Y2o§(param1:§_-O2i§, param2:§_-hK§) : void
      {
         var _loc4_:Boolean = false;
         var _loc5_:Boolean = false;
         var _loc6_:UserProfile = null;
         if(!this.§_-hh§())
         {
            return;
         }
         var _loc3_:Boolean = param2.result.equals(§_-J1§.WINNER);
         if(_loc3_)
         {
            _loc4_ = param1.battleType == §_-81M§.§_-e1C§;
            _loc5_ = param1.battleType == §_-81M§.§_-Y1C§ || param1.battleType == §_-81M§.§_-A1W§;
            if(_loc4_)
            {
               if(!this.§_-T1d§)
               {
                  this.§_-33K§();
                  ++this.§_-T1d§;
               }
            }
            else if(_loc5_ && Boolean(activityPoster))
            {
               _loc6_ = param1.§_-83I§()[0];
               this.§_-j1b§(_loc6_,param2);
            }
         }
      }
      
      public function §_-g1w§(param1:§_-W1r§) : void
      {
         var _loc2_:String = null;
         if(!this.§_-hh§())
         {
            return;
         }
         if(param1)
         {
            _loc2_ = §_-ou§.§_-n2f§(param1);
            if(param1 is §_-zF§)
            {
               this.§_-jL§(_loc2_,param1 as §_-zF§);
            }
            else if(param1 is §_-T1y§)
            {
               this.§_-op§(_loc2_,param1 as §_-T1y§);
            }
            else if(param1 is §_-O2H§)
            {
               if(param1.§_-91e§() >= §_-F4§.§_-I1o§ || param1.§_-gc§() >= §_-F4§.§_-w1c§)
               {
                  this.§_-S2D§(_loc2_,param1 as §_-O2H§);
               }
               else
               {
                  this.§_-e7§(_loc2_,param1 as §_-O2H§);
               }
            }
         }
      }
      
      public function §_-3G§(param1:§_-W1r§) : void
      {
         var _loc2_:String = null;
         if(!this.§_-hh§())
         {
            return;
         }
         if(param1)
         {
            _loc2_ = §_-ou§.§_-n2f§(param1);
            if(param1 is §_-zF§)
            {
               this.§_-iy§(_loc2_,param1 as §_-zF§);
            }
            else if(param1 is §_-T1y§)
            {
               this.§_-Op§(_loc2_,param1 as §_-T1y§);
            }
            else if(param1 is §_-O2H§ && param1.getId() > §_-u2x§.§_-H24§)
            {
               this.§_-oh§(_loc2_,param1 as §_-O2H§);
            }
         }
      }
      
      protected function §_-iy§(param1:String, param2:§_-zF§) : void
      {
         var _loc3_:Object = {"item":param1};
         var _loc4_:String = this.§_-u2u§.§_-b1i§(§_-G1p§.§_-12J§(param2.getId(),§_-L1x§.§_-AU§.§_-wz§()));
         this.§_-G3V§(§_-F1P§.format(this.§_-p1X§.§_-wG§,{"item":param1}),_loc3_,_loc4_);
      }
      
      protected function §_-Op§(param1:String, param2:§_-T1y§) : void
      {
         var _loc3_:Object = {"item":param1};
         var _loc4_:String = this.§_-u2u§.§_-K14§(param2);
         this.§_-G3V§(§_-F1P§.format(this.§_-p1X§.§_-wG§,{"item":param1}),_loc3_,_loc4_);
      }
      
      protected function §_-oh§(param1:String, param2:§_-O2H§) : void
      {
         this.§_-3L§(this.§_-p1X§.§_-WR§,this.§_-p1X§.§_-M1f§,this.§_-p1X§.§_-e7§,{"weapon":param1},this.§_-u2u§.§_-l1F§(param2));
      }
      
      public function §_-Z2v§(param1:Array) : void
      {
         var _loc3_:§_-W1r§ = null;
         if(!this.§_-hh§())
         {
            return;
         }
         var _loc2_:Array = [];
         for each(_loc3_ in param1)
         {
            _loc2_.push(§_-ou§.§_-n2f§(_loc3_));
         }
         this.newItems(_loc2_,param1);
      }
      
      public function §_-h2J§(param1:§_-O2i§, param2:§_-hK§, param3:Function) : void
      {
         var start:§_-O2i§ = param1;
         var result:§_-hK§ = param2;
         var callback:Function = param3;
         var text:Array = this.§_-C37§(start,result);
         var d:§_-q2F§ = new §_-q2F§();
         d.setText(text,this.postStatus);
         d.addEventListener(§_-h1D§.WINDOW_CLOSE,function(param1:flash.events.Event):void
         {
            callback();
         });
         d.show();
      }
      
      private function postStatus(param1:String) : void
      {
         this.§_-G3V§(param1,null,this.§_-u2u§.postStatus);
         if(activityPoster)
         {
            activityPoster.activity(param1,iconsDir + this.§_-u2u§.postStatus,§_-s2a§.PLAY_CAPTION);
         }
      }
      
      private function §_-C37§(param1:§_-O2i§, param2:§_-hK§) : Array
      {
         var _loc9_:String = null;
         var _loc3_:Array = [];
         var _loc4_:UserProfile = Application.userProfile;
         var _loc5_:Object = {
            "level":_loc4_.getLevel(),
            "rating":_loc4_.§_-Zp§(),
            "opponent":§_-33r§.§_-J2O§(param1.§_-83I§()[0])
         };
         if(param2.result.equals(§_-J1§.WINNER))
         {
            _loc3_.push(§_-F1P§.format(§_-s2a§.§_-K3t§(this.§_-2R§.§_-21u§),_loc5_));
         }
         if(param1.battleType.§_-72K§())
         {
            _loc3_.push(this.§_-a26§(_loc4_,param1.§_-83I§()[0]));
         }
         var _loc6_:Array = [this.§_-2R§.status0,this.§_-2R§.status1,this.§_-2R§.status2,this.§_-2R§.status3];
         var _loc7_:uint = 3 - _loc3_.length;
         var _loc8_:uint = 0;
         while(_loc8_ < _loc7_)
         {
            _loc9_ = _loc6_[§_-X1j§.randomSeed.§_-Fh§(0,_loc6_.length - 1)];
            _loc3_.push(§_-s2a§.§_-K3t§(_loc9_));
            §_-ts§.remove(_loc6_,_loc9_);
            _loc8_++;
         }
         return _loc3_;
      }
      
      public function §_-41I§() : Boolean
      {
         return false;
      }
      
      public function §_-Ez§() : Boolean
      {
         return notifier != null;
      }
      
      protected function §_-3L§(param1:String, param2:String, param3:String, param4:Object = null, param5:String = null) : void
      {
         var _loc6_:String = §_-s2a§.§_-K3t§(param1);
         var _loc7_:String = §_-s2a§.§_-K3t§(param2);
         var _loc8_:Function = §_-bs§.§_-d1Z§(this.§_-G3V§,null,[§_-s2a§.§_-K3t§(param3),param4,param5]);
         var _loc9_:§_-fH§ = new §_-fH§(_loc6_,_loc7_,§_-fH§.§_-X2g§,_loc8_);
         _loc9_.show();
      }
      
      protected function §_-r§(param1:UserProfile, param2:String, param3:String, param4:String, param5:Object = null, param6:String = null, param7:Function = null) : void
      {
         var msgBox:§_-fH§ = null;
         var profile:UserProfile = param1;
         var questionTitle:String = param2;
         var question:String = param3;
         var msg:String = param4;
         var params:Object = param5;
         var icon:String = param6;
         var callback:Function = param7;
         if(notifier)
         {
            msgBox = §_-fH§.§_-F3p§(§_-s2a§.§_-K3t§(questionTitle),§_-s2a§.§_-K3t§(question),§_-fH§.§_-X2g§,function():void
            {
               checkSocialNetworkAndNotify(profile,§_-s2a§.§_-K3t§(msg),params,icon);
            });
            msgBox.addEventListener(§_-M4§.WINDOW_CLOSE,function(param1:starling.events.Event):void
            {
               if(callback != null)
               {
                  callback();
               }
            });
            msgBox.show();
         }
      }
      
      protected function §_-P2c§(param1:§_-W1r§) : flash.display.DisplayObject
      {
         var _loc2_:flash.display.DisplayObject = §_-81K§.§_-92P§(param1);
         §_-4s§.§_-F21§(_loc2_,§_-G3S§ * 0.9,§_-G3S§ * 0.9);
         var _loc3_:Sprite = new Sprite();
         _loc3_.addChild(_loc2_);
         §_-4s§.§_-k1B§(_loc2_);
         return _loc3_;
      }
      
      private function §_-G3V§(param1:String, param2:Object = null, param3:String = null, param4:Function = null) : void
      {
         var msg:String = param1;
         var params:Object = param2;
         var icon:String = param3;
         var callback:Function = param4;
         §_-N1V§.§_-xJ§(function(param1:Boolean):void
         {
            postOnWallAfterNetworkChoose(msg,params,icon,callback);
         });
      }
      
      private function §_-H3C§(param1:String, param2:Object = null, param3:BitmapData = null, param4:Function = null) : void
      {
         var msg:String = param1;
         var params:Object = param2;
         var bitmapData:BitmapData = param3;
         var callback:Function = param4;
         §_-N1V§.§_-xJ§(function(param1:Boolean):void
         {
            postOnWallImageAfterNetworkChoose(msg,params,bitmapData,callback);
         });
      }
      
      private function checkSocialNetworkAndNotify(param1:IUserProfile, param2:String, param3:Object = null, param4:String = null) : void
      {
         var profile:IUserProfile = param1;
         var msg:String = param2;
         var params:Object = param3;
         var icon:String = param4;
         §_-N1V§.§_-xJ§(function(param1:Boolean):void
         {
            notifyAfterNetworkChoose(profile,msg,params,icon);
         });
      }
      
      private function §_-wY§(param1:String, param2:Object = null, param3:String = null, param4:String = null, param5:String = null) : void
      {
         var msg:String = param1;
         var params:Object = param2;
         var icon:String = param3;
         var linkText:String = param4;
         var link:String = param5;
         §_-N1V§.§_-xJ§(function(param1:Boolean):void
         {
            postActivityAfterNetworkChoose(msg,params,icon,linkText,link);
         });
      }
      
      private function §_-X1t§(param1:UserProfile, param2:UserProfile) : Boolean
      {
         var _loc3_:SocialType = §_-N1V§.socialController.getType();
         var _loc4_:String = param1.getSocialIds()[_loc3_];
         var _loc5_:String = Boolean(param2) && Boolean(param2.getSocialIds()) ? param2.getSocialIds()[_loc3_] : "";
         if(_loc5_ == _loc4_ || _loc5_ == "")
         {
            §_-fH§.§_-h2r§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ATTENTION),§_-s2a§.§_-K3t§(§_-s2a§.ERROR_SOCIAL));
            return false;
         }
         return true;
      }
      
      public function §_-25§(param1:UserProfile, param2:String) : void
      {
         if(!this.§_-hh§())
         {
            return;
         }
      }
      
      public function §_-B38§() : void
      {
      }
      
      public function §_-H3y§() : void
      {
      }
      
      public function §_-WF§() : void
      {
         if(!this.§_-hh§())
         {
            return;
         }
      }
      
      public function §_-xW§() : void
      {
         if(!this.§_-hh§())
         {
            return;
         }
      }
      
      public function §_-x1P§(param1:String, param2:String) : void
      {
      }
      
      public function §_-N2F§(param1:String, param2:String, param3:String, param4:Object, param5:Object) : void
      {
      }
      
      public function §_-eN§(param1:§_-d2F§) : void
      {
      }
      
      public function §_-415§(param1:ClanMemberTO) : void
      {
         if(!this.§_-hh§())
         {
            return;
         }
      }
      
      public function §_-h1P§(param1:int, param2:String) : void
      {
         if(!this.§_-hh§())
         {
            return;
         }
      }
      
      public function §_-f1Y§() : void
      {
         if(!this.§_-hh§())
         {
            return;
         }
      }
      
      public function §_-hh§() : Boolean
      {
         return false;
      }
   }
}

