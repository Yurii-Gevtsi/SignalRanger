package ru.pragmatix.wormix.controller
{
   import §_-2U§.*;
   import §_-52L§.§_-u2x§;
   import §_-62§.*;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-C2t§.§_-92W§;
   import §_-C3f§.§_-yG§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H2g§.*;
   import §_-TF§.§_-q1j§;
   import §_-Vu§.MD5;
   import §_-YF§.*;
   import §_-Z1K§.§_-A2n§;
   import §_-Z1K§.§_-M6§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.*;
   import §_-qH§.§_-AC§;
   import §_-s1Q§.§_-W2D§;
   import com.hurlant.util.der.§_-UO§;
   import config.§_-J1r§;
   import dragonBones.Armature;
   import dragonBones.Slot;
   import feathers.core.ToggleGroup;
   import flash.utils.ByteArray;
   import flash.utils.getTimer;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   import starling.textures.Texture;
   
   public class §_-KF§
   {
      
      public function §_-KF§()
      {
         super();
      }
      
      public static function §_-X1X§() : Boolean
      {
         var _loc3_:Object = null;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc1_:int = 0;
         var _loc2_:Array = new §_-A2n§().weapons;
         for each(_loc3_ in _loc2_)
         {
            _loc4_ = Boolean(_loc3_["infinite"]) && Boolean(_loc3_["infinite"] !== true) && Boolean(_loc3_["infinite"]["maxShots"]) ? int(_loc3_["infinite"]["maxShots"]) : 0;
            _loc5_ = Boolean(_loc3_["params"]) && Boolean(_loc3_["params"]["damage"]) ? int(_loc3_["params"]["damage"]) : 0;
            _loc6_ = _loc4_ << 3 + _loc5_ << 2 + (_loc3_.id << 1);
            _loc1_ += (_loc6_ ^ _loc3_.id) << 2;
         }
         return 436156580 == _loc1_;
      }
      
      public static function §_-92K§() : Boolean
      {
         var _loc3_:Object = null;
         var _loc4_:Array = null;
         var _loc5_:String = null;
         var _loc1_:String = "";
         var _loc2_:Array = new §_-J1r§().chars;
         for each(_loc3_ in _loc2_)
         {
            _loc1_ += 3 << _loc3_.id;
            if(_loc3_.bonus)
            {
               _loc4_ = [];
               for(_loc5_ in _loc3_.bonus)
               {
                  _loc4_.push(_loc5_ + _loc3_.bonus[_loc5_]);
               }
               _loc4_.sort();
               _loc1_ += _loc4_.join();
            }
            else
            {
               _loc1_ += 5 << _loc3_.id;
            }
         }
         return "b58b90e07a33a9942eb05ad9cc3b2461" == MD5.§_-bf§(_loc1_);
      }
      
      public static function validate() : Boolean
      {
         var _loc1_:String = §_-lJ§(new §_-A2n§().weapons);
         var _loc2_:String = §_-lJ§(new §_-J1r§().chars);
         var _loc3_:String = §_-lJ§(new §_-AC§().hats);
         var _loc4_:String = §_-lJ§(new §_-u2x§().upgrades);
         var _loc5_:String = §_-lJ§(new §_-M6§().weapons);
         var _loc6_:String = §_-lJ§(new §_-W2D§().artifacts);
         return _loc1_ == "48570ce644a14494b05b74854588652c" && _loc2_ == "7760ef3de5160b6416a2ca8eda80d637" && _loc3_ == "d81c942b069038c961b9b56b118b022b" && _loc4_ == "cbff795cfea9ffbf812f9409a2691965" && _loc5_ == "b736c17a14cf8fb4b170545dc41a82d8" && _loc6_ == "541340ca92a7fc454166241cf76d921f";
      }
      
      public static function §_-lJ§(param1:Array) : String
      {
         var _loc3_:Object = null;
         var _loc2_:String = "01011986";
         for each(_loc3_ in param1)
         {
            _loc2_ += §_-ht§(_loc3_);
         }
         return MD5.§_-bf§(_loc2_);
      }
      
      private static function §_-ht§(param1:Object) : String
      {
         var _loc3_:Array = null;
         var _loc4_:String = null;
         var _loc2_:String = "";
         if(!(param1 is Class))
         {
            if(param1 is §_-7u§ || param1 is §_-TA§)
            {
               _loc2_ += "protected(" + param1.value + ")";
            }
            else if(typeof param1 == "object")
            {
               _loc3_ = [];
               for(_loc4_ in param1)
               {
                  _loc3_.push(_loc4_);
               }
               _loc3_.sort();
               _loc2_ = "{";
               for each(_loc4_ in _loc3_)
               {
                  _loc2_ += _loc4_ + ":" + §_-ht§(param1[_loc4_]) + ",";
               }
               _loc2_ += "}";
            }
            else
            {
               _loc2_ += param1;
            }
         }
         else
         {
            _loc2_ += "Class";
         }
         return _loc2_;
      }
   }
}

