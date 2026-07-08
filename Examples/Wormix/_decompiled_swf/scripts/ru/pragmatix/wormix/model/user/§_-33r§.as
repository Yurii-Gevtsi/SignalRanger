package ru.pragmatix.wormix.model.user
{
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-79§.§_-ps§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-A2U§.§_-A23§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-fH§;
   import §_-Ly§.§_-81L§;
   import §_-P1z§.*;
   import §_-Vg§.§_-M11§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-yA§;
   import §_-Y1Q§.§_-61Z§;
   import §_-Y1b§.*;
   import §_-Z1K§.§_-q24§;
   import §_-hO§.§_-v24§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import §_-k1u§.§_-Vr§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-o14§.*;
   import §_-qH§.§_-AC§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s1Q§.Artifacts;
   import §_-s1Q§.§_-W2D§;
   import §_-z1g§.§_-F4§;
   import §_-z1g§.§_-lp§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import feathers.controls.Button;
   import feathers.controls.Slider;
   import flash.events.TimerEvent;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.system.ApplicationDomain;
   import flash.system.LoaderContext;
   import flash.system.SecurityDomain;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.Timer;
   import flash.utils.clearTimeout;
   import ru.pragmatix.flox.gui.controls.MessageBox;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.clans.response.treasury.DonateClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.ChooseBonusItemResult;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.SimpleProfileStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.pvp.messages.ex.*;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import ru.pragmatix.wormix.weapons.render.IRenderer;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   
   public class §_-33r§
   {
      
      private static const §_-o1v§:int = 20;
      
      public static const §_-k2R§:String = "N/A";
      
      public static const §_-y1j§:String = "You";
      
      public function §_-33r§()
      {
         super();
      }
      
      public static function §_-c2x§(param1:UserProfile, param2:int = 20) : String
      {
         var _loc3_:String = param1.charName;
         if(_loc3_ != "" && _loc3_ != §_-33r§.§_-y1j§)
         {
            return _loc3_;
         }
         if(param1.getSocialData())
         {
            return §_-X1e§(§_-X1j§.§_-82T§.getDisplayName(param1),param2);
         }
         return §_-k2R§;
      }
      
      public static function §_-X1e§(param1:String, param2:uint = 20) : String
      {
         if(!param1)
         {
            return §_-k2R§;
         }
         return StringUtil.§_-jT§(param1,§_-o1v§);
      }
      
      public static function §_-KQ§(param1:UserProfile, param2:uint = 20) : String
      {
         var _loc3_:String = param1.charName;
         var _loc4_:SocialUserData = param1.getSocialData();
         if((_loc3_ == "" || _loc3_ == "You") && Boolean(_loc4_))
         {
            _loc3_ = _loc4_.getFirstName();
         }
         return §_-X1e§(_loc3_,param2);
      }
      
      public static function §_-fl§(param1:UserProfile) : String
      {
         return param1.charName;
      }
      
      public static function getLastName(param1:UserProfile, param2:uint = 20) : String
      {
         var _loc3_:String = param1.getCharName();
         var _loc4_:SocialUserData = param1.getSocialData();
         if((_loc3_ == "" || _loc3_ == "You") && Boolean(_loc4_))
         {
            _loc3_ = _loc4_.getLastName();
         }
         return §_-X1e§(_loc3_,param2);
      }
      
      public static function §_-J2O§(param1:UserProfile) : String
      {
         return §_-c2x§(param1,-1);
      }
      
      public static function §_-S1K§(param1:UserProfile) : String
      {
         var _loc3_:int = 0;
         var _loc4_:UserProfile = null;
         var _loc2_:String = "";
         while(_loc3_ < param1.§_-A1U§().length)
         {
            _loc4_ = param1.§_-A1U§()[_loc3_];
            if(_loc3_ > 0)
            {
               _loc2_ += ", ";
            }
            _loc2_ += §_-J2O§(_loc4_);
            _loc2_ += "(" + _loc4_.getLevel() + " " + §_-s2a§.NOTE_LEVEL + ", " + _loc4_.§_-P2e§().getName() + ")";
            _loc3_++;
         }
         return _loc2_;
      }
      
      public static function §_-Xj§(param1:§_-yG§) : Boolean
      {
         return §_-As§(param1.moneyType,param1.price);
      }
      
      public static function §_-As§(param1:§_-E19§, param2:§_-x2t§) : Boolean
      {
         return §_-n2V§(Application.userProfile,param1,param2);
      }
      
      public static function §_-n2V§(param1:UserProfile, param2:§_-E19§, param3:§_-x2t§) : Boolean
      {
         var _loc5_:§_-fH§ = null;
         var _loc4_:Boolean = true;
         switch(param2)
         {
            case §_-E19§.§_-51D§:
               if(param1.§_-o1N§() < param3.realPrice)
               {
                  _loc4_ = false;
               }
               break;
            case §_-E19§.§_-i2d§:
               if(param1.§_-51e§() < param3.§_-R1T§)
               {
                  _loc4_ = false;
               }
         }
         if(!_loc4_)
         {
            _loc5_ = new §_-7D§();
            _loc5_.show();
         }
         return _loc4_;
      }
      
      public static function §_-pm§(param1:UserProfile) : void
      {
         if(Boolean(param1) && Boolean(param1.getSocialData()))
         {
            if(param1.getSocialData().getPageUrl())
            {
               navigateToURL(new URLRequest(param1.getSocialData().getPageUrl()),"_blank");
            }
            else
            {
               §_-fH§.§_-q1a§("",§_-s2a§.§_-K3t§(§_-s2a§.FORBIDDEN_TRANSITION_PAGE),§_-fH§.§_-1B§);
            }
         }
      }
      
      public static function §_-r1A§(param1:UserProfile) : int
      {
         var _loc2_:§_-uH§ = new §_-uH§(param1.getLevel());
         _loc2_.§_-q1H§(param1.§_-h2m§());
         _loc2_.§_-eQ§(param1.§_-w1p§());
         return _loc2_.§_-41C§;
      }
      
      public static function §_-i1§(param1:Array) : Array
      {
         var _loc3_:Object = null;
         var _loc4_:UserProfile = null;
         var _loc2_:Array = [];
         for each(_loc3_ in param1)
         {
            _loc4_ = new UserProfile();
            if(_loc3_ is UserProfileStructure)
            {
               _loc4_.build.§_-q2o§(_loc3_ as UserProfileStructure);
            }
            else if(_loc3_ is RatingProfileStructure)
            {
               _loc4_.build.§_-B36§(_loc3_ as RatingProfileStructure);
            }
            else if(_loc3_ is SimpleProfileStructure)
            {
               _loc4_.build.§_-83p§(_loc3_ as SimpleProfileStructure);
            }
            _loc2_.push(_loc4_);
         }
         return _loc2_;
      }
      
      public static function indexOf(param1:Array, param2:UserProfile) : int
      {
         var _loc3_:int = 0;
         while(_loc3_ < param1.length)
         {
            if(param1[_loc3_].equals(param2))
            {
               return _loc3_;
            }
            _loc3_++;
         }
         return -1;
      }
      
      public static function §_-L2C§(param1:§_-F3H§, param2:uint, param3:uint) : Array
      {
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc8_:§_-Oi§ = null;
         var _loc9_:§_-O2H§ = null;
         var _loc13_:uint = 0;
         var _loc14_:uint = 0;
         var _loc7_:Array = [];
         var _loc10_:Array = param1.§_-P2l§();
         _loc4_ = int(_loc10_.length);
         while(--_loc4_ > -1)
         {
            _loc8_ = §_-Oi§(_loc10_[_loc4_]);
            _loc9_ = §_-O2H§(_loc8_.getRecord());
            _loc7_.push({
               "item":_loc8_,
               "bagX":_loc9_.bagX,
               "bagY":_loc9_.bagY
            });
         }
         _loc7_ = _loc7_.sortOn(["bagX","bagY"]);
         _loc4_ = int(_loc7_.length);
         while(--_loc4_ > -1)
         {
            _loc7_[_loc4_] = _loc7_[_loc4_].item;
         }
         var _loc11_:Array = [];
         _loc4_ = int(param2);
         while(--_loc4_ > -1)
         {
            _loc11_.push([]);
         }
         _loc6_ = int(_loc7_.length);
         _loc4_ = 0;
         while(_loc4_ < _loc6_)
         {
            _loc5_ = 0;
            while(_loc5_ < param2)
            {
               if(§_-O2H§((_loc7_[_loc4_] as §_-Oi§).getRecord()).bagX == _loc5_ + 1)
               {
                  _loc11_[_loc5_].push(_loc7_[_loc4_]);
                  if(_loc11_[_loc5_].length > 1)
                  {
                     _loc13_ = 0;
                     while(_loc13_ < _loc11_[_loc5_].length)
                     {
                        if(_loc11_[_loc5_][_loc13_] != null && !(_loc11_[_loc5_][_loc13_] as §_-Oi§).getRecord().getInfinite())
                        {
                           _loc14_ = 0;
                           while(_loc14_ < _loc11_[_loc5_].length)
                           {
                              if(Boolean(_loc13_ != _loc14_ && _loc11_[_loc5_][_loc13_] != null && _loc11_[_loc5_][_loc14_] != null && _loc11_[_loc5_][_loc13_].getId() == _loc11_[_loc5_][_loc14_].getId()) && Boolean(_loc11_[_loc5_][_loc14_].getRecord().getInfinite()) && §_-O2H§((_loc11_[_loc5_][_loc14_] as §_-Oi§).getRecord()).getMaxShots() == 0)
                              {
                                 _loc11_[_loc5_].splice(_loc13_,1);
                              }
                              _loc14_++;
                           }
                        }
                        _loc13_++;
                     }
                  }
                  break;
               }
               _loc5_++;
            }
            _loc4_++;
         }
         var _loc12_:§_-F3H§ = param1 as §_-F3H§;
         _loc11_ = §_-r2v§(_loc11_,param2,param3,_loc12_ ? _loc12_.§_-L30§() : null);
         _loc6_ = int(_loc11_.length);
         _loc4_ = 0;
         while(_loc4_ < _loc6_)
         {
            _loc5_ = 0;
            while(_loc5_ < _loc11_[_loc4_].length)
            {
               _loc7_[_loc5_ * param2 + _loc4_] = _loc11_[_loc4_][_loc5_];
               _loc5_++;
            }
            _loc4_++;
         }
         return _loc7_;
      }
      
      private static function §_-r2v§(param1:Array, param2:uint, param3:uint, param4:Object = null) : Array
      {
         var _loc7_:Array = null;
         var _loc8_:* = 0;
         var _loc10_:§_-Oi§ = null;
         var _loc11_:§_-Oi§ = null;
         var _loc12_:§_-Oi§ = null;
         var _loc13_:uint = 0;
         var _loc14_:uint = 0;
         var _loc15_:Array = null;
         var _loc5_:int = int(param1.length);
         var _loc6_:int = 0;
         for each(_loc7_ in param1)
         {
            if(_loc7_.length > _loc6_)
            {
               _loc6_ = int(_loc7_.length);
            }
         }
         _loc6_ = Math.max(param3,_loc6_);
         _loc8_ = 0;
         _loc8_ = 0;
         while(_loc8_ < _loc5_)
         {
            if(param1[_loc8_].length < _loc6_)
            {
               _loc13_ = _loc6_ - param1[_loc8_].length;
               _loc14_ = 0;
               while(_loc14_ < _loc13_)
               {
                  param1[_loc8_].splice(param1[_loc8_].length,0,null);
                  _loc14_++;
               }
            }
            _loc8_++;
         }
         var _temp_2:* = param1[param1.length - 1];
         var _loc9_:Array = param1[param1.length - 1];
         _loc5_ = int(_loc9_.length);
         _loc8_ = 0;
         while(_loc8_ <= _loc5_)
         {
            _loc10_ = _loc9_[_loc8_];
            if((Boolean(_loc10_)) && param4[_loc10_.getId()] == null)
            {
               _loc15_ = _loc9_.splice(_loc8_);
               break;
            }
            _loc8_++;
         }
         _loc8_ = 0;
         for each(_loc11_ in param4)
         {
            var _loc18_:Number;
            _loc9_[_loc18_ = _loc8_++] = _loc11_;
         }
         while(_loc8_ < §_-F3H§.§_-Bw§)
         {
            _loc9_[_loc8_] = null;
            _loc8_++;
         }
         _loc8_ = int(§_-F3H§.§_-Bw§);
         for each(_loc12_ in _loc15_)
         {
            if(_loc12_)
            {
               _loc9_[_loc18_ = _loc8_++] = _loc12_;
            }
         }
         while(_loc8_ < _loc5_)
         {
            var _loc16_:Number;
            _loc9_[_loc16_ = _loc8_++] = null;
         }
         return param1;
      }
      
      public static function §_-d8§(param1:UserProfile, param2:uint) : Boolean
      {
         var _loc4_:int = 0;
         param1.§_-t5§(param1.§_-p25§() + param2);
         var _loc3_:uint = §_-lp§.§_-L1M§(param1.getLevel());
         if(param1.§_-p25§() >= _loc3_)
         {
            if(§_-lp§.§_-j2h§(param1.getLevel() + 1))
            {
               _loc4_ = int(§_-lp§.§_-L2a§(param1.getLevel() + 1));
               param1.§_-q28§(_loc4_);
               param1.§_-N1n§(param1.getLevel() + 1);
               param1.§_-t5§(param1.§_-p25§() - _loc3_);
               §_-k1x§(param1);
               return true;
            }
            param1.§_-t5§(_loc3_);
         }
         return false;
      }
      
      public static function §_-xV§(param1:UserProfile, param2:§_-W1r§, param3:int = -1) : void
      {
         var _loc7_:§_-2m§ = null;
         var _loc8_:§_-O2H§ = null;
         var _loc9_:int = 0;
         var _loc4_:int = param2 ? param2.getId() : -1;
         if(!_loc4_)
         {
            return;
         }
         var _loc5_:Boolean = _loc4_ > §_-AC§.§_-H24§ && _loc4_ < §_-W2D§.§_-H24§;
         var _loc6_:Boolean = _loc4_ > §_-W2D§.§_-H24§;
         if(_loc5_ || _loc6_)
         {
            §_-2t§.§_-E9§(_loc4_);
         }
         else
         {
            _loc7_ = param1.§_-pd§();
            _loc8_ = §_-G1p§.§_-I4§(_loc4_);
            _loc9_ = param3 == -1 ? -1 : int(param3 + _loc7_.§_-d3§(_loc4_));
            _loc7_.§_-Um§(_loc8_,_loc9_,_loc8_.getMaxShots());
         }
      }
      
      public static function §_-jA§(param1:int, param2:int) : String
      {
         return param1.toString() + (param2 ? " (" + (param2 > 0 ? "+" : "") + param2 + ")" : "");
      }
      
      public static function §_-ym§(param1:UserProfile) : Boolean
      {
         return Boolean(param1) && param1.getId() == Application.userProfile.getId();
      }
      
      public static function §_-r1v§(param1:Array) : Boolean
      {
         var _loc2_:UserProfile = null;
         for each(_loc2_ in param1)
         {
            if(!_loc2_.getSocialData())
            {
               return true;
            }
         }
         return false;
      }
      
      public static function §_-i22§(param1:UserProfile, param2:UserProfile) : int
      {
         var _loc3_:int = param2.§_-N23§() - param1.§_-N23§();
         if(_loc3_ != 0)
         {
            return _loc3_;
         }
         return param2.getId() - param1.getId();
      }
      
      private static function §_-k1x§(param1:UserProfile) : void
      {
         var _loc2_:UserProfile = null;
         for each(_loc2_ in param1.§_-A1U§())
         {
            if(§_-yA§.§_-M1d§(_loc2_) && _loc2_.§_-y1E§() >= param1.getLevel())
            {
               _loc2_.build.§_-o2g§(param1.getLevel());
            }
         }
      }
      
      public static function §_-t2t§(param1:Vector.<UserProfile>) : UserProfile
      {
         var _loc2_:UserProfile = null;
         for each(_loc2_ in param1)
         {
            if(!_loc2_.equals(Application.userProfile))
            {
               return _loc2_;
            }
         }
         return null;
      }
      
      public static function §_-kg§(param1:UserProfile, param2:Vector.<UserProfile>) : Boolean
      {
         var _loc3_:UserProfile = null;
         for each(_loc3_ in param2)
         {
            if(_loc3_.getId() == Application.userProfile.getId())
            {
               return §_-g2a§(param1,_loc3_) && §_-2M§(param1,_loc3_) && §_-A30§(param1,_loc3_) && §_-rY§(param1,_loc3_);
            }
         }
         return false;
      }
      
      private static function §_-g2a§(param1:UserProfile, param2:UserProfile) : Boolean
      {
         return param1.§_-h2m§() == param2.§_-h2m§() && param1.§_-w1p§() == param2.§_-w1p§() && param1.§_-N23§() == param2.§_-N23§() && param1.getLevel() == param2.getLevel() && param1.§_-P2e§() == param2.§_-P2e§() && param1.§_-u2l§() == param2.§_-u2l§();
      }
      
      public static function §_-2M§(param1:UserProfile, param2:UserProfile) : Boolean
      {
         return §_-G1m§(param1.§_-pd§(),param2.§_-pd§());
      }
      
      public static function §_-A30§(param1:UserProfile, param2:UserProfile) : Boolean
      {
         var _loc3_:Boolean = §_-G1m§(param1.§_-XT§(),param2.§_-XT§());
         var _loc4_:int = param1.§_-32q§() ? param1.§_-32q§().getId() : -1;
         var _loc5_:int = param2.§_-32q§() ? param2.§_-32q§().getId() : -1;
         var _loc6_:int = param1.getHat() ? param1.getHat().getId() : -1;
         var _loc7_:int = param2.getHat() ? param2.getHat().getId() : -1;
         return _loc3_ && _loc4_ == _loc5_ && _loc6_ == _loc7_;
      }
      
      private static function §_-G1m§(param1:§_-H2M§, param2:§_-H2M§) : Boolean
      {
         var _loc6_:§_-Oi§ = null;
         var _loc7_:§_-Oi§ = null;
         var _loc3_:Array = param1.§_-P2l§();
         var _loc4_:Array = param2.§_-P2l§();
         if(_loc3_.length != _loc4_.length)
         {
            return false;
         }
         var _loc5_:* = int(_loc3_.length);
         while(_loc5_--)
         {
            _loc6_ = _loc3_[_loc5_];
            _loc7_ = _loc4_[_loc5_];
            if(_loc6_.getId() != _loc7_.getId() || _loc6_.getCount() != _loc7_.getCount())
            {
               return false;
            }
         }
         return true;
      }
      
      private static function §_-rY§(param1:UserProfile, param2:UserProfile) : Boolean
      {
         return param1.§_-n26§().length == param2.§_-n26§().length;
      }
      
      public static function §_-Ga§(param1:WeaponStructure, param2:§_-O2H§) : uint
      {
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc3_:uint = 1;
         var _loc4_:int = param1.getCount();
         if(_loc4_ >= 0)
         {
            return _loc3_;
         }
         if(_loc4_ == -1)
         {
            return param2.getMaxLevel();
         }
         _loc5_ = -10;
         var _temp_2:* = int(_loc5_ - param2.getMaxLevel());
         _loc6_ = _loc5_ - param2.getMaxLevel();
         if(_loc6_ <= _loc4_ && _loc4_ < _loc5_)
         {
            return Math.abs(_loc4_ - _loc5_);
         }
         return _loc3_;
      }
      
      public static function §_-r2A§(param1:§_-H2M§, param2:Array, param3:Boolean = false) : void
      {
         var _loc6_:WeaponStructure = null;
         var _loc7_:int = 0;
         var _loc8_:§_-O2H§ = null;
         var _loc9_:uint = 0;
         var _loc10_:§_-Oi§ = null;
         var _loc4_:int = 0;
         var _loc5_:int = int(param2.length);
         while(_loc4_ < _loc5_)
         {
            _loc6_ = WeaponStructure(param2[_loc4_]);
            _loc7_ = _loc6_.getCount();
            _loc8_ = §_-G1p§.§_-I4§(_loc6_.getId());
            if((Boolean(_loc8_)) && _loc7_ != 0)
            {
               if(!(param3 && _loc7_ >= 0))
               {
                  _loc9_ = §_-Ga§(_loc6_,_loc8_);
                  if(_loc7_ < -1)
                  {
                     _loc7_ = -1;
                  }
                  _loc10_ = param1.§_-Um§(_loc8_,_loc7_,_loc8_.getMaxShots(),_loc9_);
               }
            }
            _loc4_++;
         }
      }
      
      public static function §_-z1J§(param1:UserProfile, param2:Array) : void
      {
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:§_-O2H§ = null;
         var _loc8_:§_-Oi§ = null;
         var _loc3_:int = 0;
         var _loc4_:int = int(param2.length);
         while(_loc3_ < _loc4_)
         {
            _loc5_ = -1;
            _loc6_ = int(param2[_loc3_]);
            _loc7_ = §_-G1p§.§_-I4§(_loc6_);
            if((Boolean(_loc7_)) && Boolean(_loc7_.§_-91e§() > 0) && _loc7_.§_-T1J§() <= param1.getLevel())
            {
               _loc8_ = param1.§_-R1X§.§_-Um§(_loc7_,_loc5_,_loc7_.getMaxShots(),_loc7_.getMaxLevel());
            }
            _loc3_++;
         }
      }
      
      public static function §_-5o§(param1:UserProfile) : §_-F3H§
      {
         var _loc6_:§_-Oi§ = null;
         var _loc7_:§_-Oi§ = null;
         var _loc8_:§_-Oi§ = null;
         var _loc9_:int = 0;
         var _loc2_:§_-2m§ = param1.§_-31d§;
         var _loc3_:§_-Vr§ = param1.§_-R1X§;
         var _loc4_:§_-F3H§ = new §_-F3H§();
         var _loc5_:Array = _loc3_.§_-P2l§();
         var _loc10_:Array = _loc2_.§_-P2l§();
         for each(_loc6_ in _loc10_)
         {
            _loc9_ = _loc6_.getId();
            _loc7_ = _loc3_.§_-O2n§(_loc9_,param1);
            if((Boolean(_loc7_)) && _loc6_.getLevel() < _loc7_.getLevel())
            {
               _loc8_ = _loc7_;
            }
            else
            {
               _loc8_ = _loc6_;
            }
            _loc4_.§_-Um§(_loc8_.getRecord(),_loc8_.getCount(),_loc8_.getMaxShots(),_loc8_.getLevel());
         }
         for each(_loc7_ in _loc5_)
         {
            _loc9_ = _loc7_.getId();
            if(_loc3_.§_-h2S§(_loc7_,param1) && !§_-Cq§.§_-33I§(_loc9_,param1))
            {
               _loc4_.§_-Um§(_loc7_.getRecord(),_loc7_.getCount(),_loc7_.getMaxShots(),_loc7_.getLevel());
            }
         }
         return _loc4_;
      }
      
      public static function §_-M2Z§(param1:uint) : uint
      {
         var _loc3_:Array = null;
         var _loc4_:CraftedEquipFamily = null;
         var _loc2_:Array = [[§_-S25§.AQUALUNG],[Artifacts.§_-O2p§]];
         for each(_loc3_ in _loc2_)
         {
            if(param1 == _loc3_[0])
            {
               return _loc3_[1];
            }
         }
         _loc4_ = §_-2t§.§_-TB§(param1);
         return _loc4_ ? uint(_loc4_.§_-46§()[0].§_-62n§()) : param1;
      }
   }
}

