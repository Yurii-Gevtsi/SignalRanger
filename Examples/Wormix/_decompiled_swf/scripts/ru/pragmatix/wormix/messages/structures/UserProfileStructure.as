package ru.pragmatix.wormix.messages.structures
{
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import flash.utils.Dictionary;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import starling.events.Event;
   
   public class UserProfileStructure
   {
      
      private static const §_-k2T§:String = "#";
      
      public var id:int;
      
      public var money:uint;
      
      public var realMoney:uint;
      
      public var rating:int;
      
      public var unitsGroup:Array;
      
      public var weaponRecordList:Array;
      
      public var stuff:Array;
      
      public var temporalStuff:Array = [];
      
      public var reactionRate:int;
      
      public var socialIds:Dictionary;
      
      public var recipes:Array = [];
      
      public var clanMember:ClanMemberStructure;
      
      public var extraGroupSlotsCount:int;
      
      public var rentedItems:RentedItemsStructure;
      
      public var rankPoints:int;
      
      public var bestRank:int;
      
      public var name:String;
      
      public var units:Array;
      
      public function UserProfileStructure()
      {
         super();
      }
      
      public static function getSocialIdsAsString(param1:Dictionary) : String
      {
         var _loc3_:Object = null;
         var _loc4_:String = null;
         var _loc5_:SocialType = null;
         var _loc2_:String = "";
         for(_loc3_ in param1)
         {
            _loc4_ = param1[_loc3_];
            _loc5_ = _loc3_ as SocialType;
            _loc2_ += _loc5_.servercode + §_-k2T§ + _loc4_ + §_-k2T§;
         }
         return _loc2_.substr(0,_loc2_.length - §_-k2T§.length);
      }
      
      public static function parseSocialIdsString(param1:String) : Dictionary
      {
         var _loc6_:SocialType = null;
         var _loc7_:String = null;
         var _loc2_:Dictionary = new Dictionary();
         var _loc3_:Array = param1.split(§_-k2T§);
         var _loc4_:int = _loc3_.length % 2 == 0 ? int(_loc3_.length) : int(_loc3_.length - 1);
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            _loc6_ = SocialType.valueOfServerCode(parseInt(_loc3_[_loc5_]));
            _loc7_ = _loc3_[_loc5_ + 1];
            _loc2_[_loc6_] = _loc7_;
            _loc5_ += 2;
         }
         return _loc2_;
      }
      
      public static function parseSocialIdsArray(param1:Array) : Dictionary
      {
         var _loc4_:SocialLinkInfoStructure = null;
         var _loc5_:SocialType = null;
         var _loc2_:Dictionary = new Dictionary();
         var _loc3_:int = 0;
         while(_loc3_ < param1.length)
         {
            _loc4_ = param1[_loc3_];
            _loc5_ = SocialType.valueOfServerCode(_loc4_.socialType);
            _loc2_[_loc5_] = _loc4_.socialId;
            _loc3_++;
         }
         return _loc2_;
      }
      
      public function initFromProfile(param1:UserProfile) : void
      {
         var _loc2_:UserProfile = null;
         var _loc3_:§_-Oi§ = null;
         var _loc4_:UnitStructure = null;
         this.id = param1.getId();
         this.money = param1.§_-51e§();
         this.realMoney = param1.§_-o1N§();
         this.socialIds = param1.getSocialIds();
         this.name = param1.§_-K3u§();
         this.unitsGroup = [];
         for each(_loc2_ in param1.§_-A1U§())
         {
            _loc4_ = new UnitStructure();
            _loc4_.initFromProfile(_loc2_);
            this.unitsGroup.push(_loc4_);
         }
         this.weaponRecordList = [];
         for each(_loc3_ in param1.§_-pd§())
         {
            this.weaponRecordList.push(new WeaponStructure().initFromBackpackItem(_loc3_));
         }
      }
      
      public function toString() : String
      {
         return "UserProfileStructure{" + " socialUserId=" + String(this.id) + ", socialIds=" + getSocialIdsAsString(this.socialIds) + ", money=" + String(this.money) + ", realMoney=" + String(this.realMoney) + ", wormsGroup=" + String(this.unitsGroup) + ", weaponRecordList=" + String(this.weaponRecordList) + " }";
      }
      
      public function clone(param1:int = 0, param2:int = 0) : UserProfileStructure
      {
         var _loc5_:Array = null;
         var _loc3_:UserProfileStructure = new UserProfileStructure();
         _loc3_.id = param1 || this.id;
         _loc3_.money = this.money;
         _loc3_.realMoney = this.realMoney;
         _loc3_.rating = this.rating;
         _loc3_.unitsGroup = this.unitsGroup.concat();
         _loc3_.weaponRecordList = this.weaponRecordList.concat();
         _loc3_.stuff = this.stuff.concat();
         _loc3_.temporalStuff = this.temporalStuff.concat();
         _loc3_.reactionRate = this.reactionRate;
         var _loc4_:String = UserProfileStructure.getSocialIdsAsString(this.socialIds);
         if(param2)
         {
            _loc5_ = [SocialType.VK.servercode.toString(),param2];
         }
         _loc3_.socialIds = UserProfileStructure.parseSocialIdsString(param2 ? _loc5_.join("#") : _loc4_);
         _loc3_.recipes = this.recipes.concat();
         _loc3_.clanMember = this.clanMember;
         _loc3_.extraGroupSlotsCount = this.extraGroupSlotsCount;
         _loc3_.rentedItems = new RentedItemsStructure();
         _loc3_.rentedItems.weapons = [];
         _loc3_.rentedItems.stuff = [];
         _loc3_.rankPoints = this.rankPoints;
         _loc3_.bestRank = this.bestRank;
         _loc3_.name = this.name;
         if(this.units)
         {
            _loc3_.units = this.units.concat();
         }
         return _loc3_;
      }
   }
}

