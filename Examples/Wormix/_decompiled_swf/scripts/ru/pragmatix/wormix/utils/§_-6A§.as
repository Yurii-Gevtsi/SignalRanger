package ru.pragmatix.wormix.utils
{
   import §_-23P§.*;
   import §_-2k§.*;
   import §_-31N§.§_-42G§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-73I§.§_-Y1l§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.*;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-MQ§;
   import §_-B1y§.§_-bs§;
   import §_-CF§.§_-p2U§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-E1I§.§_-b2n§;
   import §_-L1H§.§_-C3e§;
   import §_-RE§.§_-qW§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-Y1O§.§_-yA§;
   import §_-Y1Q§.§_-61Z§;
   import §_-Y1b§.§_-22N§;
   import §_-YF§.§_-N3§;
   import §_-Z1K§.§_-q24§;
   import §_-b1R§.§_-C3P§;
   import §_-fo§.§_-r5§;
   import §_-hO§.§_-03k§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-m1g§.RatingPanel;
   import §_-n1w§.*;
   import §_-nE§.SelectCraftWeaponScreen;
   import §_-nE§.§_-p16§;
   import §_-o14§.*;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-H3D§;
   import §_-w2i§.§_-Ia§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-A1s§;
   import §_-zI§.§_-X1q§;
   import §_-zI§.§_-g1a§;
   import com.hurlant.crypto.tls.§_-21v§;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.FeathersControl;
   import feathers.core.IFeathersControl;
   import feathers.core.ITextRenderer;
   import feathers.core.PopUpManager;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.LayoutBoundsResult;
   import feathers.layout.VerticalAlign;
   import feathers.layout.ViewPortBounds;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import flash.display.DisplayObject;
   import flash.display.DisplayObjectContainer;
   import flash.display.MovieClip;
   import flash.display.Sprite;
   import flash.display.StageOrientation;
   import flash.events.StageOrientationEvent;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.social.lang.EnLangController;
   import ru.pragmatix.flox.social.lang.ILangController;
   import ru.pragmatix.flox.social.lang.RuLangController;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.AnimationStatesRenderer;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.clans.main.server.GetJoinClansMainRequest;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanAuditActionTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-i14§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.display.Canvas;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   public class §_-6A§
   {
      
      public function §_-6A§()
      {
         super();
      }
      
      public static function §_-a1w§(param1:UserProfile, param2:ClanTO) : Boolean
      {
         var _loc3_:Boolean = §_-JU§(param1,param2);
         return _loc3_ && param1.clanMember.rank == §_-X1q§.§_-O2V§;
      }
      
      public static function §_-W2J§(param1:UserProfile, param2:ClanTO) : Boolean
      {
         var _loc3_:Boolean = §_-JU§(param1,param2);
         return _loc3_ && param1.clanMember.rank == §_-X1q§.OFFICER;
      }
      
      public static function §_-JU§(param1:UserProfile, param2:ClanTO) : Boolean
      {
         return param1.§_-ia§() && param1.clanMember.clanId == param2.id;
      }
      
      public static function §_-s1U§(param1:UserProfile, param2:UserProfile) : Boolean
      {
         return param1.§_-ia§() && param2.§_-ia§() && param1.clanMember.clanId == param2.clanMember.clanId;
      }
      
      public static function §_-R1A§(param1:uint) : MovieClip
      {
         return §_-r5§.§_-tF§(§_-g1a§.§_-B2w§[param1]);
      }
      
      public static function §_-F1T§(param1:uint, param2:uint) : MovieClip
      {
         var _loc3_:Vector.<String> = §_-g1a§.§_-PZ§[param1];
         return §_-r5§.§_-tF§(_loc3_[param2]);
      }
      
      public static function §_-wQ§(param1:UserProfile, param2:uint) : void
      {
         var _loc3_:ClanTO = §_-A23§.§_-Y1d§(param2);
         var _loc4_:ClanMemberStructure = param1.clanMember;
         _loc4_.clanId = _loc3_.id;
         _loc4_.clanName = _loc3_.name;
         _loc4_.clanEmblem = _loc3_.emblem;
         _loc4_.clanSeasonRating = _loc3_.seasonRating;
         var _loc5_:ClanMemberTO = §_-62r§(param1,_loc3_);
         if(_loc5_)
         {
            _loc4_.rank = _loc5_.rank;
         }
      }
      
      public static function §_-tK§() : void
      {
         var tcm:UserProfile = null;
         var needDisable:Boolean = false;
         var temp:UserProfile = null;
         var myProfile:UserProfile = Application.userProfile;
         var teamClanmates:Vector.<UserProfile> = §_-71v§.§_-11g§(myProfile);
         for each(tcm in teamClanmates)
         {
            if(tcm.§_-H38§())
            {
               if(tcm.getLevel() > myProfile.getLevel())
               {
                  needDisable = true;
                  if(tcm.§_-t2I§() != §_-yA§.§_-n2Y§)
                  {
                     tcm.§_-62T§(§_-yA§.§_-n2Y§);
                  }
               }
               else if(!myProfile.§_-ia§() || tcm.§_-t2I§() == §_-yA§.§_-02F§)
               {
                  needDisable = true;
                  if(!myProfile.§_-ia§() && tcm.§_-t2I§() == §_-yA§.§_-uF§)
                  {
                     tcm.§_-62T§(§_-yA§.§_-02F§);
                  }
               }
               if(needDisable)
               {
                  temp = tcm;
                  §_-X1j§.§_-D3H§.§_-s19§(tcm,function():void
                  {
                     temp.§_-Ox§(false);
                     Application.userProfile.dispatchEventWith(Event.CHANGE);
                  });
               }
            }
            else if(tcm.getLevel() > myProfile.getLevel() && tcm.§_-t2I§() == §_-yA§.§_-uF§)
            {
               tcm.§_-62T§(§_-yA§.§_-n2Y§);
            }
         }
      }
      
      public static function §_-62r§(param1:UserProfile, param2:ClanTO) : ClanMemberTO
      {
         var _loc3_:ClanMemberTO = null;
         for each(_loc3_ in param2.members)
         {
            if(_loc3_.profileId == param1.getId())
            {
               return _loc3_;
            }
         }
         return null;
      }
      
      public static function §_-Gi§(param1:flash.display.DisplayObjectContainer, param2:Vector.<uint>) : void
      {
         §_-J1A§(param1,param2[0],param2[1]);
         §_-H2s§(param1,param2[2],param2[3]);
      }
      
      public static function §_-J1A§(param1:flash.display.DisplayObjectContainer, param2:uint, param3:uint) : void
      {
         var _loc4_:MovieClip = §_-R1A§(param2);
         var _loc5_:flash.display.DisplayObject = param1.getChildByName("icon");
         param1.addChild(_loc4_);
         if(_loc5_)
         {
            param1.removeChild(_loc5_);
            _loc4_.name = _loc5_.name;
            _loc4_.x = _loc5_.x;
            _loc4_.y = _loc5_.y;
         }
         else
         {
            _loc4_.name = "icon";
         }
         var _loc6_:ColorTransform = new ColorTransform();
         _loc6_.color = §_-g1a§.§_-W1Q§[param3];
         _loc4_.transform.colorTransform = _loc6_;
      }
      
      public static function §_-H2s§(param1:flash.display.DisplayObjectContainer, param2:uint, param3:uint) : void
      {
         var _loc5_:flash.display.DisplayObject = null;
         var _loc4_:flash.display.DisplayObject = §_-6A§.§_-F1T§(param2,param3);
         _loc5_ = param1.getChildByName("pattern");
         var _loc6_:int = _loc5_ ? param1.getChildIndex(_loc5_) : int(param1.numChildren - 1);
         if(_loc5_)
         {
            if(param1.contains(_loc5_))
            {
               param1.removeChild(_loc5_);
            }
            _loc4_.name = _loc5_.name;
            _loc4_.x = _loc5_.x;
            _loc4_.y = _loc5_.y;
         }
         else
         {
            _loc4_.name = "pattern";
         }
         param1.addChildAt(_loc4_,_loc6_ > 0 ? _loc6_ : 0);
      }
      
      public static function §_-G17§(param1:flash.display.DisplayObjectContainer) : void
      {
         var _loc2_:flash.display.DisplayObject = param1.getChildByName("pattern");
         var _loc3_:flash.display.DisplayObject = param1.getChildByName("icon");
         _loc3_.x = _loc2_.width * 0.06;
         _loc3_.y = _loc2_.height * 0.13;
      }
      
      public static function §_-nk§(param1:uint) : §_-zi§
      {
         var _loc2_:§_-zi§ = new §_-zi§(§_-r5§.§_-tF§(§_-X1q§.§_-E2R§[param1 - 1]));
         if(param1 == §_-X1q§.§_-O2V§)
         {
            _loc2_.§_-11P§(§_-s2a§.§_-K3t§("RANK_LEADER"));
         }
         else if(param1 == §_-X1q§.OFFICER)
         {
            _loc2_.§_-11P§(§_-s2a§.§_-K3t§("RANK_OFFICER"));
         }
         else
         {
            _loc2_.§_-11P§(§_-s2a§.§_-K3t§("RANK_SOLDIER"));
         }
         return _loc2_;
      }
      
      public static function §_-R1w§(param1:uint) : String
      {
         if(param1 == §_-X1q§.§_-O2V§)
         {
            return §_-s2a§.§_-K3t§(§_-s2a§.RANK_LEADER);
         }
         if(param1 == §_-X1q§.OFFICER)
         {
            return §_-s2a§.§_-K3t§(§_-s2a§.RANK_OFFICER);
         }
         return §_-s2a§.§_-K3t§(§_-s2a§.RANK_SOLDIER);
      }
      
      public static function §_-b2w§(param1:int, param2:uint = 20, param3:ClanTO = null) : String
      {
         var _loc4_:ClanMemberTO = §_-k1a§(param1,param3);
         return _loc4_ ? §_-33r§.§_-X1e§(_loc4_.name,param2) : StringUtil.§_-43t§;
      }
      
      public static function §_-k1a§(param1:int, param2:ClanTO = null) : ClanMemberTO
      {
         var _loc3_:ClanMemberTO = null;
         if(!param2)
         {
            param2 = §_-A23§.userClan;
         }
         if(param2)
         {
            for each(_loc3_ in param2.members)
            {
               if(param1 == _loc3_.profileId)
               {
                  return _loc3_;
               }
            }
         }
         return null;
      }
      
      public static function updateClan(param1:ClanTO) : void
      {
         var _loc2_:ClanMemberTO = null;
         var _loc3_:UserProfile = null;
         for each(_loc2_ in param1.members)
         {
            _loc3_ = §_-N3§.§_-g1D§(_loc2_.profileId);
            if(_loc3_)
            {
               _loc3_.clanMember.clanId = param1.id;
               _loc3_.clanMember.clanName = param1.name;
               _loc3_.clanMember.clanEmblem = param1.emblem;
               _loc3_.clanMember.clanSeasonRating = param1.seasonRating;
               _loc3_.clanMember.rank = _loc2_.rank;
            }
         }
         §_-A23§.§_-o2G§(param1);
         if(Boolean(§_-A23§.userClan) && §_-A23§.userClan.id == param1.id)
         {
            §_-wQ§(Application.userProfile,param1.id);
         }
      }
      
      public static function §_-D21§(param1:String, param2:Function = null) : Boolean
      {
         if(param1.length < §_-8D§.§_-32O§)
         {
            §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_ERROR),§_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.ERROR_MIN_CHAR_CLAN_NAME),{"value":§_-8D§.§_-32O§}),§_-fH§.§_-1B§,param2);
            return false;
         }
         return true;
      }
      
      public static function §_-b1J§(param1:Vector.<ClanMemberTO>) : Vector.<ClanMemberTO>
      {
         var members:Vector.<ClanMemberTO> = param1;
         var compareFunc:Function = function(param1:ClanMemberTO, param2:ClanMemberTO):int
         {
            if(param1.seasonRating > param2.seasonRating)
            {
               return -1;
            }
            if(param1.seasonRating == param2.seasonRating && param1.rank < param2.rank)
            {
               return -1;
            }
            if(param1.seasonRating == param2.seasonRating && param1.rank == param2.rank && param1.lastLoginTime > param2.lastLoginTime)
            {
               return -1;
            }
            return 1;
         };
         return members.sort(compareFunc);
      }
      
      public static function §_-D31§(param1:Vector.<ClanMemberTO>) : Vector.<ClanMemberTO>
      {
         var members:Vector.<ClanMemberTO> = param1;
         var compareFunc:Function = function(param1:ClanMemberTO, param2:ClanMemberTO):int
         {
            return param1.seasonRating > param2.seasonRating ? -1 : 1;
         };
         return members.sort(compareFunc);
      }
      
      public static function §_-Jr§(param1:Vector.<ClanAuditActionTO>) : Vector.<ClanAuditActionTO>
      {
         var actions:Vector.<ClanAuditActionTO> = param1;
         var compareFunc:Function = function(param1:ClanAuditActionTO, param2:ClanAuditActionTO):int
         {
            return param1.date < param2.date ? -1 : 1;
         };
         actions.sort(compareFunc);
         return actions;
      }
      
      public static function §_-P1C§(param1:Vector.<ClanMemberTO>, param2:int) : Vector.<ClanMemberTO>
      {
         var _loc4_:ClanMemberTO = null;
         var _loc3_:Vector.<ClanMemberTO> = new Vector.<ClanMemberTO>();
         for each(_loc4_ in param1)
         {
            if(_loc4_.rank == param2)
            {
               _loc3_[_loc3_.length] = _loc4_;
            }
         }
         return _loc3_;
      }
      
      public static function §_-oy§(param1:int) : §_-A1s§
      {
         var _loc3_:Object = null;
         var _loc2_:int = 0;
         if(param1 > 0)
         {
            for each(_loc3_ in §_-8D§.§_-OM§)
            {
               if(_loc3_.hasOwnProperty("place") && _loc3_.place == param1 || _loc3_.hasOwnProperty("max") && _loc3_.max >= param1 && _loc3_.min <= param1)
               {
                  _loc2_ = int(_loc3_.tier);
                  break;
               }
            }
         }
         return §_-A1s§.valueOf(_loc2_);
      }
      
      public static function §_-u1Z§(param1:uint, param2:Vector.<uint>) : MovieClip
      {
         var _loc3_:MovieClip = new MovieClip();
         var _loc4_:MovieClip = §_-r5§.§_-tF§(§_-8D§.§_-93T§[param1 - 1]);
         _loc3_.addChild(_loc4_);
         var _loc5_:MovieClip = §_-r5§.§_-tF§(§_-8D§.§_-21Q§ + String(param2[3] + 1));
         _loc5_.x = -23;
         _loc5_.y = -85.4;
         _loc3_.addChild(_loc5_);
         var _loc6_:MovieClip = §_-R1A§(param2[0]);
         var _loc7_:ColorTransform = new ColorTransform();
         _loc7_.color = §_-g1a§.§_-W1Q§[param2[1]];
         _loc6_.transform.colorTransform = _loc7_;
         _loc6_.scaleX = _loc6_.scaleY = 0.6;
         _loc6_.x = -22;
         _loc6_.y = -72.5;
         _loc3_.addChild(_loc6_);
         return _loc3_;
      }
      
      public static function §_-X22§(param1:String) : MovieClip
      {
         var _loc2_:MovieClip = new MovieClip();
         var _loc3_:MovieClip = §_-r5§.§_-tF§(§_-8D§.§_-93T§[0]);
         _loc2_.addChild(_loc3_);
         var _loc4_:MovieClip = §_-r5§.§_-tF§(param1);
         _loc4_.x = -23;
         _loc4_.y = -85.4;
         _loc2_.addChild(_loc4_);
         return _loc2_;
      }
      
      public static function §_-d2Q§(param1:UserProfile) : void
      {
         var _loc2_:UserProfile = null;
         for each(_loc2_ in param1.§_-A1U§())
         {
            if(_loc2_.§_-t2I§() == §_-yA§.§_-n2Y§ && param1.getLevel() >= _loc2_.getLevel())
            {
               _loc2_.§_-62T§(§_-yA§.§_-uF§);
            }
         }
      }
      
      public static function §_-Hz§() : void
      {
         var _loc1_:UserProfile = null;
         for each(_loc1_ in Application.userProfile.§_-A1U§())
         {
            if(_loc1_.§_-t2I§() == §_-yA§.§_-uF§)
            {
               _loc1_.§_-62T§(§_-yA§.§_-02F§);
            }
         }
         §_-tK§();
      }
   }
}

