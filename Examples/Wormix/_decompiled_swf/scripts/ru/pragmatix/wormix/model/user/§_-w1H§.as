package ru.pragmatix.wormix.model.user
{
   import §_-23P§.§_-V2T§;
   import §_-23P§.§_-u1z§;
   import §_-31u§.§_-l1n§;
   import §_-52L§.§_-u2x§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-zF§;
   import §_-63U§.§_-412§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-4I§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CR§.§_-72p§;
   import §_-DJ§.§_-fH§;
   import §_-K35§.§_-99§;
   import §_-Ly§.§_-81L§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-Y1O§.*;
   import §_-g1P§.§_-z1n§;
   import §_-j1w§.§_-L1x§;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-52D§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-by§;
   import §_-l1K§.§_-E1n§;
   import §_-l1K§.§_-m2§;
   import §_-l1K§.§_-n2T§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import §_-z1g§.§_-lp§;
   import config.§_-V2w§;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.geom.Point;
   import flash.utils.Dictionary;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.messages.server.EnterAccount;
   import ru.pragmatix.wormix.messages.structures.PvpProfileStructure;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.SimpleProfileStructure;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.messages.structures.UnitStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import ru.pragmatix.wormix.weapons.ammo.boss.emperor.EmperorTerracottaWarrior;
   import ru.pragmatix.wormix.weapons.ammo.boss.scientist.ScientistGenerator;
   import starling.display.Quad;
   import starling.events.Event;
   import starling.utils.SystemUtil;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-w1H§
   {
      
      private var profile:UserProfile;
      
      public function §_-w1H§(param1:UserProfile)
      {
         super();
         this.profile = param1;
      }
      
      public function §_-83p§(param1:SimpleProfileStructure) : void
      {
         var _loc3_:UserProfile = null;
         this.profile.ingameMoney = new §_-TA§(param1.money);
         this.profile.realMoney = new §_-TA§(param1.realMoney);
         this.profile.rating = new §_-TA§(param1.rating);
         this.profile.reactionRate = new §_-TA§(param1.reactionRate);
         this.profile.charName = this.profile.§_-i2W§ = param1.name;
         this.profile.§_-U2G§ = param1.vipExpiryTime;
         this.profile.team = [];
         this.profile.§_-53P§.§_-Z2U§.value = param1.armor;
         this.profile.§_-53P§.§_-A3Z§.value = param1.attack;
         this.profile.§_-N1n§(param1.level);
         this.profile.§_-53P§.§_-g2I§.value = §_-lp§.§_-L2a§(param1.level);
         var _loc2_:uint = 0;
         while(_loc2_ < param1.groupCount)
         {
            _loc3_ = new UserProfile();
            _loc3_.§_-53P§.§_-g2I§.value = §_-lp§.§_-L2a§(param1.level);
            this.profile.team.push(_loc3_);
            _loc2_++;
         }
         this.init(param1.id,param1.socialOwnerIds,param1.hatId,param1.artifactId,param1.raceId);
      }
      
      public function §_-B36§(param1:RatingProfileStructure) : void
      {
         var _loc3_:UserProfile = null;
         this.profile.§_-31d§ = new §_-2m§();
         this.profile.ingameMoney = new §_-TA§(param1.money);
         this.profile.realMoney = new §_-TA§(param1.realMoney);
         this.profile.rating = new §_-TA§(param1.rating);
         this.profile.dailyRating = new §_-TA§(param1.dailyRating);
         this.profile.reactionRate = new §_-TA§(param1.reactionRate);
         this.profile.charName = this.profile.§_-i2W§ = param1.name;
         this.profile.§_-U2G§ = param1.vipExpiryTime;
         this.profile.§_-q1H§(param1.armor);
         this.profile.§_-eQ§(param1.attack);
         this.profile.§_-N1n§(param1.level);
         this.profile.§_-q28§(§_-lp§.§_-L2a§(param1.level));
         this.profile.§_-O1Y§(new §_-TA§(param1.oldPlace));
         this.profile.team = [];
         var _loc2_:int = 0;
         while(_loc2_ < param1.groupCount)
         {
            _loc3_ = new UserProfile();
            _loc3_.§_-q28§(§_-lp§.§_-L2a§(param1.level));
            this.profile.team.push(_loc3_);
            _loc2_++;
         }
         this.init(param1.id,param1.socialOwnerIds,param1.hatId,param1.artifactId,param1.raceId);
      }
      
      private function §_-f1P§(param1:UnitStructure, param2:Boolean, param3:int = -1) : void
      {
         var _loc4_:Object = null;
         var _loc5_:§_-u1z§ = null;
         this.profile.experience = new §_-TA§(param1.experience);
         this.profile.teamMemberType = param1.teamMemberType;
         if(param2 && param3 > 0)
         {
            this.§_-y25§(param1.level,param1.armor,param1.attack,param3);
         }
         else
         {
            this.§_-F3Q§(param1.level,param1.armor,param1.attack);
         }
         for(_loc4_ in param1.socialOwnerIds)
         {
            if(!this.profile.socialIds[_loc4_])
            {
               this.profile.socialIds[_loc4_] = param1.socialOwnerIds[_loc4_];
            }
         }
         if(param1.teamMemberType == §_-yA§.§_-e2e§)
         {
            _loc5_ = §_-V2T§.§_-H2p§(param1.ownerId);
            if(_loc5_)
            {
               this.profile.charName = _loc5_.getName();
               this.profile.§_-i2W§ = _loc5_.getName();
            }
         }
         else
         {
            this.profile.charName = this.profile.§_-i2W§ = param1.name;
         }
         this.profile.§_-Ox§(param1.isActive);
         this.init(param1.ownerId,this.profile.socialIds,param1.hatId,param1.artifactId,param1.raceId);
      }
      
      public function §_-q2o§(param1:UserProfileStructure, param2:Boolean = false) : void
      {
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc9_:§_-IE§ = null;
         var _loc10_:Vector.<int> = null;
         var _loc11_:int = 0;
         var _loc12_:TemporalStuffStructure = null;
         var _loc13_:UnitStructure = null;
         var _loc14_:UserProfile = null;
         var _loc15_:§_-zF§ = null;
         var _loc16_:§_-T1y§ = null;
         this.profile.§_-521§(param1.id,param1.socialIds);
         this.profile.ingameMoney = new §_-TA§(param1.money);
         this.profile.realMoney = new §_-TA§(param1.realMoney);
         this.profile.rating = new §_-TA§(param1.rating);
         this.profile.reactionRate = new §_-TA§(param1.reactionRate);
         this.profile.online = false;
         this.profile.§_-U2G§ = param1.rentedItems.activeUntil;
         this.profile.charName = this.profile.§_-i2W§ = param1.name;
         this.profile.§_-23h§ = param1.clanMember;
         if(!this.profile.§_-A2I§)
         {
            this.profile.§_-A2I§ = new §_-C2c§();
         }
         var _loc3_:uint = this.§_-U13§(param1);
         this.profile.team = [];
         var _loc4_:int = int(param1.unitsGroup.length);
         _loc5_ = 0;
         while(_loc5_ < _loc4_)
         {
            _loc13_ = UnitStructure(param1.unitsGroup[_loc5_]);
            if(_loc13_.socialOwnerIds == param1.socialIds)
            {
               this.profile.charName = this.profile.§_-i2W§ = _loc13_.name;
            }
            if(_loc13_.ownerId == this.profile.id)
            {
               this.§_-f1P§(_loc13_,false);
               this.profile.team.push(this.profile);
            }
            else
            {
               _loc14_ = new UserProfile();
               _loc14_.build.§_-f1P§(_loc13_,true,_loc3_);
               _loc14_.§_-620§(_loc13_.socialOwnerIds);
               this.profile.team.push(_loc14_);
            }
            _loc5_++;
         }
         this.§_-A3l§(param1.weaponRecordList,param2);
         this.§_-I28§(param1.rentedItems);
         var _loc7_:§_-H2M§ = this.profile.§_-Q28§ = new §_-H2M§();
         var _loc8_:Array = param1.recipes;
         _loc5_ = 0;
         _loc6_ = int(_loc8_.length);
         while(_loc5_ < _loc6_)
         {
            _loc7_.§_-Um§(§_-Y2u§.§_-v2G§(Math.abs(_loc8_[_loc5_])),§_-H3D§.sign(_loc8_[_loc5_]));
            _loc5_++;
         }
         for each(_loc9_ in §_-Y2u§.§_-uO§())
         {
            if(_loc9_.availableFromStart && _loc9_.§_-T1J§() <= this.profile.getLevel() && !_loc7_.§_-d0§(_loc9_.getId()))
            {
               _loc7_.§_-Um§(_loc9_);
            }
         }
         _loc10_ = new Vector.<int>(0);
         _loc5_ = 0;
         _loc6_ = int(_loc8_.length);
         while(_loc5_ < _loc6_)
         {
            if(§_-H3D§.sign(_loc8_[_loc5_]) > 0)
            {
               _loc10_.push(_loc8_[_loc5_]);
            }
            _loc5_++;
         }
         this.§_-aI§(_loc10_);
         this.profile.§_-h2M§ = new §_-52D§();
         this.profile.§_-h2M§.§_-dQ§(§_-N1V§.settings);
         this.profile.§_-h2M§.§_-V2R§(param1);
         this.profile.wardrobe = new §_-by§();
         for each(_loc11_ in param1.stuff)
         {
            if(§_-V2w§.§_-q1F§(_loc11_))
            {
               this.profile.wardrobe.§_-Um§(§_-G1p§.§_-12J§(_loc11_,this.profile.§_-P2e§()));
            }
            else if(§_-V2w§.§_-Q2a§(_loc11_))
            {
               this.profile.wardrobe.§_-Um§(§_-G1p§.§_-93G§(_loc11_));
            }
         }
         if(this.profile.charName == "" && Boolean(param1 as PvpProfileStructure))
         {
            this.profile.charName = this.profile.§_-i2W§ = (param1 as PvpProfileStructure).socialName;
         }
         this.profile.§_-2N§ = new Vector.<TemporalStuffStructure>();
         for each(_loc12_ in param1.temporalStuff)
         {
            this.profile.§_-2N§.push(_loc12_);
            _loc15_ = §_-G1p§.§_-12J§(_loc12_.stuffId,this.profile.§_-P2e§());
            if(_loc15_)
            {
               this.profile.wardrobe.§_-Um§(_loc15_);
               this.profile.wardrobe.§_-l2e§(_loc15_,_loc12_.expireDate);
            }
            else
            {
               _loc16_ = §_-G1p§.§_-93G§(_loc12_.stuffId);
               if(_loc16_)
               {
                  this.profile.wardrobe.§_-Um§(_loc16_);
                  this.profile.wardrobe.§_-l2e§(_loc16_,_loc12_.expireDate);
               }
            }
         }
      }
      
      private function §_-U13§(param1:UserProfileStructure) : uint
      {
         var _loc3_:UnitStructure = null;
         var _loc2_:uint = §_-lp§.getMaxLevel();
         if(§_-N1V§.settings.§_-1n§())
         {
            for each(_loc3_ in param1.unitsGroup)
            {
               if(_loc3_.ownerId == param1.id)
               {
                  _loc2_ = uint(_loc3_.level);
                  break;
               }
            }
         }
         return _loc2_;
      }
      
      private function init(param1:uint, param2:Dictionary, param3:uint, param4:uint, param5:uint) : void
      {
         this.profile.§_-521§(param1,param2);
         this.profile.§_-s1o§(§_-L1x§.§_-M19§(param5));
         this.profile.§_-L§(§_-G1p§.§_-12J§(param3,this.profile.§_-53P§.raceRecord));
         this.profile.§_-aV§(§_-G1p§.§_-93G§(param4));
      }
      
      public function §_-m2F§(param1:EnterAccount) : void
      {
         var _loc4_:int = 0;
         var _loc5_:§_-W1r§ = null;
         var _loc6_:int = 0;
         var _loc2_:Array = param1.userProfileStructure.stuff;
         var _loc3_:uint = _loc2_.length;
         _loc6_ = 0;
         while(_loc6_ < _loc3_)
         {
            _loc4_ = int(_loc2_[_loc6_]);
            if(§_-V2w§.§_-L2o§(_loc4_))
            {
               _loc5_ = §_-Y2u§.§_-k14§(_loc4_);
               this.profile.§_-91o§.§_-Um§(_loc5_,1);
            }
            _loc6_++;
         }
         var _loc7_:Vector.<int> = param1.reagents;
         _loc3_ = _loc7_.length;
         _loc6_ = 0;
         while(_loc6_ < _loc3_)
         {
            _loc5_ = §_-Y2u§.§_-k14§(_loc6_);
            if((Boolean(_loc5_)) && _loc7_[_loc6_] > 0)
            {
               this.profile.§_-91o§.§_-Um§(_loc5_,_loc7_[_loc6_]);
            }
            _loc6_++;
         }
      }
      
      public function §_-I28§(param1:RentedItemsStructure) : void
      {
         this.profile.§_-U2G§ = param1.activeUntil;
         §_-33r§.§_-z1J§(this.profile,param1.weapons);
         this.profile.§_-p2c§().§_-j2Q§(param1.stuff);
         if(this.profile.§_-33n§())
         {
            this.profile.§_-dP§();
         }
      }
      
      private function §_-A3l§(param1:Array, param2:Boolean = false) : void
      {
         this.profile.§_-31d§ = new §_-2m§();
         §_-33r§.§_-r2A§(this.profile.§_-31d§,param1,param2);
      }
      
      public function §_-cr§(param1:Array) : void
      {
         var _loc3_:WeaponStructure = null;
         var _loc2_:Array = [];
         for each(_loc3_ in param1)
         {
            _loc2_.push(_loc3_.getId());
         }
         this.profile.§_-V2K§ = new §_-F3H§();
         §_-33r§.§_-r2A§(this.profile.§_-V2K§,param1);
      }
      
      private function §_-aI§(param1:Vector.<int>) : void
      {
         var _loc4_:§_-IE§ = null;
         var _loc6_:int = 0;
         var _loc7_:§_-IE§ = null;
         var _loc8_:Boolean = false;
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc2_:Vector.<§_-IE§> = new Vector.<§_-IE§>();
         var _loc3_:Vector.<§_-IE§> = new Vector.<§_-IE§>();
         var _loc5_:* = 0;
         for each(_loc6_ in param1)
         {
            _loc7_ = §_-Y2u§.§_-v2G§(_loc6_);
            if(_loc7_)
            {
               _loc2_.push(_loc7_);
               if(_loc2_[_loc2_.length - 1].level > _loc5_)
               {
                  _loc5_ = _loc2_[_loc2_.length - 1].level;
               }
            }
         }
         while(_loc5_ > 0)
         {
            _loc8_ = true;
            for each(_loc4_ in _loc2_)
            {
               if(_loc4_.level == _loc5_)
               {
                  _loc9_ = _loc4_.prevUpgradeId;
                  _loc3_.push(_loc4_);
                  _loc2_.splice(_loc2_.indexOf(_loc4_),1);
                  do
                  {
                     _loc10_ = 0;
                     _loc11_ = int(_loc2_.length);
                     while(_loc10_ < _loc11_)
                     {
                        if(_loc2_[_loc10_].upgradeId == _loc9_)
                        {
                           _loc9_ = _loc2_[_loc10_].prevUpgradeId;
                           _loc2_.splice(_loc10_,1);
                           break;
                        }
                        _loc10_++;
                     }
                     if(_loc9_ < §_-u2x§.§_-H24§)
                     {
                        this.profile.§_-31d§.removeItem(_loc9_,-1);
                     }
                  }
                  while(_loc9_ >= §_-u2x§.§_-H24§);
                  _loc8_ = false;
                  break;
               }
            }
            if(_loc8_)
            {
               _loc5_--;
            }
         }
         for each(_loc4_ in _loc3_)
         {
            §_-Cq§.§_-G38§(_loc4_.upgradeId,this.profile);
         }
      }
      
      public function §_-DU§(param1:SocialType, param2:SocialUserData) : void
      {
         if(Boolean(param2) && this.profile.getSocialIds()[param1] == param2.getSocialId())
         {
            this.profile.§_-Dw§[param1] = param2;
         }
      }
      
      public function §_-pM§(param1:PvpProfileStructure) : void
      {
         this.§_-q2o§(param1);
         if(this.profile.charName == "")
         {
            this.profile.§_-i2W§ = this.profile.charName = param1.name == "" ? param1.socialName : param1.name;
         }
      }
      
      public function §_-tn§(param1:Object) : void
      {
         var _loc3_:UserProfile = null;
         var _loc4_:String = null;
         var _loc5_:Object = null;
         var _loc6_:SocialType = null;
         var _loc2_:int = 0;
         while(_loc2_ < this.profile.team.length)
         {
            _loc3_ = this.profile.team[_loc2_];
            _loc4_ = _loc3_.getId().toString();
            _loc4_ = "1272";
            for(_loc5_ in param1[_loc4_])
            {
               _loc6_ = _loc5_ as SocialType;
               _loc3_.build.§_-DU§(_loc6_,param1[_loc4_][_loc5_]);
            }
            _loc2_++;
         }
      }
      
      public function §_-H2f§(param1:int) : UserProfile
      {
         var _loc2_:int = 0;
         while(_loc2_ < this.profile.team.length)
         {
            if(this.profile.team[_loc2_].id == param1)
            {
               return this.profile.team[_loc2_];
            }
            _loc2_++;
         }
         return null;
      }
      
      public function §_-Fc§(param1:UserProfile, param2:Boolean, param3:int = 0) : void
      {
         var _loc5_:uint = 0;
         var _loc6_:uint = 0;
         var _loc7_:Array = null;
         var _loc8_:uint = 0;
         var _loc9_:UserProfile = null;
         if(param2)
         {
            if(param1.getHat())
            {
               _loc5_ = §_-33r§.§_-M2Z§(param1.getHat().getId());
               param1.§_-L§(§_-G1p§.§_-12J§(_loc5_,param1.§_-P2e§()));
            }
            if(param1.§_-32q§())
            {
               _loc6_ = §_-33r§.§_-M2Z§(param1.§_-32q§().getId());
               param1.§_-aV§(§_-G1p§.§_-93G§(_loc6_));
            }
         }
         var _loc4_:uint = §_-N1V§.settings.§_-1n§() ? uint(this.profile.getLevel()) : §_-lp§.getMaxLevel();
         param1.build.§_-y25§(param1.getLevel(),param1.§_-53P§.§_-Z2U§.value,param1.§_-53P§.§_-A3Z§.value,_loc4_);
         if(param3 == 0)
         {
            this.profile.§_-A1U§().push(param1);
         }
         else
         {
            _loc7_ = this.profile.§_-A1U§();
            _loc8_ = 0;
            while(_loc8_ < _loc7_.length)
            {
               var _temp_1:* = _loc7_[_loc8_];
               _loc9_ = _loc7_[_loc8_];
               if(_loc9_.getId() == param3)
               {
                  _loc7_[_loc8_] = param1;
                  break;
               }
               _loc8_++;
            }
         }
      }
      
      public function §_-D1X§(param1:uint) : void
      {
         this.§_-y25§(this.profile.getLevel(),this.profile.§_-53P§.§_-Z2U§.value,this.profile.§_-53P§.§_-A3Z§.value,param1);
      }
      
      public function §_-o2g§(param1:uint) : void
      {
         this.§_-y25§(this.profile.§_-v6§.value,this.profile.§_-D3N§,this.profile.§_-7P§,param1);
      }
      
      public function §_-A1y§() : void
      {
         this.§_-F3Q§(this.profile.§_-v6§.value,this.profile.§_-D3N§,this.profile.§_-7P§);
      }
      
      public function §_-F3Q§(param1:uint, param2:int, param3:int) : void
      {
         this.profile.§_-v6§.value = param1;
         this.profile.§_-7P§ = param3;
         this.profile.§_-D3N§ = param2;
         this.profile.§_-N1n§(param1);
         this.profile.§_-q1H§(param2);
         this.profile.§_-eQ§(param3);
         this.profile.§_-q28§(§_-lp§.§_-L2a§(param1));
      }
      
      private function §_-y25§(param1:uint, param2:int, param3:int, param4:uint) : void
      {
         var _loc5_:uint = 0;
         var _loc6_:Number = NaN;
         var _loc7_:uint = 0;
         if(param1 > param4 || param2 + param3 > param1 * 2)
         {
            _loc5_ = Math.min(param1,param4);
            _loc6_ = param3 < 1 ? 1 * param2 : param2 / param3;
            _loc7_ = _loc5_ * 2;
            this.profile.§_-v6§.value = param1;
            this.profile.§_-D3N§ = param2;
            this.profile.§_-7P§ = param3;
            this.profile.§_-N1n§(_loc5_);
            this.profile.§_-53P§.§_-Z2U§.value = int(Math.ceil(_loc6_ * _loc7_ / (_loc6_ + 1)));
            this.profile.§_-53P§.§_-A3Z§.value = _loc7_ - this.profile.§_-53P§.§_-Z2U§.value;
            this.profile.§_-53P§.§_-g2I§.value = §_-lp§.§_-L2a§(_loc5_);
         }
         else
         {
            this.§_-F3Q§(param1,param2,param3);
         }
      }
   }
}

