package ru.pragmatix.wormix.model.items.craft
{
   import §_-31W§.§_-Kb§;
   import §_-3D§.§_-E3x§;
   import §_-3D§.§_-a2b§;
   import §_-52L§.§_-u2x§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-D3A§.§_-TZ§;
   import §_-TF§.§_-q1j§;
   import §_-YF§.§_-q2v§;
   import §_-k2s§.§_-Go§;
   import §_-l1g§.§_-L3Q§;
   import §_-y1s§.§_-P9§;
   import §_-zI§.§_-X1q§;
   import feathers.controls.Button;
   import feathers.data.ArrayCollection;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.AbstractAuraEffect;
   import ru.pragmatix.wormix.gui.menu.clans.chat.§_-D2t§;
   import ru.pragmatix.wormix.model.§_-kQ§;
   import ru.pragmatix.wormix.model.rewards.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.events.Event;
   import starling.utils.MathUtil;
   
   public class §_-Y2u§
   {
      
      private static var instance:§_-Y2u§;
      
      private var trees:Object = {};
      
      private var recipes:Object = new Object();
      
      private var reagents:Object = new Object();
      
      private var §_-I2p§:Vector.<§_-kQ§> = new Vector.<§_-kQ§>();
      
      public function §_-Y2u§()
      {
         super();
         instance = this;
      }
      
      public static function getInstance() : §_-Y2u§
      {
         if(!instance)
         {
            instance = new §_-Y2u§();
         }
         return instance;
      }
      
      public static function §_-uO§() : Object
      {
         return §_-Y2u§.getInstance().recipes;
      }
      
      public static function §_-22E§() : Object
      {
         return §_-Y2u§.getInstance().trees;
      }
      
      public static function §_-h1o§() : Object
      {
         return §_-Y2u§.getInstance().reagents;
      }
      
      public static function §_-8Q§(param1:int) : §_-13q§
      {
         return §_-Y2u§.§_-22E§()[param1];
      }
      
      public static function §_-k14§(param1:int) : §_-W1r§
      {
         return §_-Y2u§.§_-h1o§()[param1];
      }
      
      public static function §_-v2G§(param1:int) : §_-IE§
      {
         return §_-Y2u§.§_-uO§()[param1];
      }
      
      public static function §_-c1i§(param1:§_-u2x§) : void
      {
         §_-D1C§();
         §_-48§(param1);
      }
      
      private static function §_-48§(param1:§_-u2x§) : void
      {
         var _loc2_:§_-IE§ = null;
         var _loc3_:§_-13q§ = null;
         for each(_loc2_ in §_-Y2u§.§_-uO§())
         {
            if(_loc2_.prevUpgradeId >= 0 && _loc2_.prevUpgradeId < §_-u2x§.§_-H24§)
            {
               if(!getInstance().trees[_loc2_.prevUpgradeId])
               {
                  getInstance().trees[_loc2_.prevUpgradeId] = new §_-13q§(§_-G1p§.§_-I4§(_loc2_.prevUpgradeId));
                  _loc3_ = getInstance().trees[_loc2_.prevUpgradeId];
                  _loc3_.§_-R2N§ = "1" + §_-L1e§(_loc2_.prevUpgradeId,1,_loc3_.root,_loc3_,param1);
               }
            }
         }
      }
      
      private static function §_-L1e§(param1:int, param2:int, param3:§_-6S§, param4:§_-13q§, param5:§_-u2x§) : String
      {
         var _loc7_:§_-IE§ = null;
         var _loc8_:§_-6S§ = null;
         var _loc9_:String = null;
         var _loc6_:String = "";
         for each(_loc7_ in §_-Y2u§.§_-uO§())
         {
            if(_loc7_.prevUpgradeId == param1)
            {
               _loc8_ = new §_-6S§();
               §_-H20§(param3.weapon,param5.§_-Pu§(_loc7_.upgradeId));
               param3.next.push(_loc8_);
               _loc8_.prev = param3;
               _loc8_.§_-l1u§ = param4;
               _loc8_.level = param2;
               _loc8_.§_-HS§ = _loc7_;
               _loc7_.§_-j0§ = _loc8_;
               _loc8_.weapon = §_-G1p§.§_-I4§(_loc7_.upgradeId);
               _loc9_ = §_-L1e§(_loc7_.upgradeId,param2 + 1,_loc8_,param4,param5);
               if(_loc8_.next.length)
               {
                  _loc6_ += _loc9_;
               }
            }
         }
         return param3.next.length ? param3.next.length.toString() + _loc6_ : "";
      }
      
      private static function §_-D1C§() : void
      {
         var _loc1_:§_-IE§ = null;
         var _loc2_:§_-kQ§ = null;
         for each(_loc1_ in getInstance().recipes)
         {
            _loc2_ = §_-r1O§(_loc1_.prevUpgradeId);
            if(_loc2_ == null)
            {
               _loc2_ = new §_-kQ§(_loc1_.prevUpgradeId);
               getInstance().§_-I2p§.push(_loc2_);
            }
            _loc2_.§_-I2r§.push(_loc1_.upgradeId);
         }
      }
      
      public static function §_-r1O§(param1:int) : §_-kQ§
      {
         var _loc2_:§_-kQ§ = null;
         var _loc3_:int = 0;
         for each(_loc2_ in getInstance().§_-I2p§)
         {
            for each(_loc3_ in _loc2_.§_-I2r§)
            {
               if(_loc3_ == param1)
               {
                  return _loc2_;
               }
            }
         }
         return null;
      }
      
      private static function §_-H20§(param1:§_-O2H§, param2:Object) : void
      {
         var _loc3_:§_-O2H§ = param1.clone();
         _loc3_.initFromConfig(param2);
         §_-G1p§.§_-S1n§()[_loc3_.getId()] = _loc3_;
      }
   }
}

