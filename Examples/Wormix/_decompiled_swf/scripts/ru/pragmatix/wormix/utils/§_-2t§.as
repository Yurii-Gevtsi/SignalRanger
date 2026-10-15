package ru.pragmatix.wormix.utils
{
   import §_-21p§.§_-4w§;
   import §_-21p§.§_-N1H§;
   import §_-23P§.§_-V2T§;
   import §_-23P§.§_-u1z§;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-T1y§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-zF§;
   import §_-73d§.§_-v1o§;
   import §_-91B§.§_-QK§;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-x2t§;
   import §_-H16§.*;
   import §_-Ta§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-yA§;
   import §_-Y1b§.*;
   import §_-YF§.§_-q2v§;
   import §_-b1F§.§_-G4§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-Oi§;
   import §_-k1u§.§_-by§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.§_-Dd§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s1Q§.§_-W2D§;
   import §_-w2i§.§_-Zd§;
   import §_-z1g§.§_-4l§;
   import com.greensock.loading.core.*;
   import com.hurlant.math.BigInteger;
   import config.§_-B1K§;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.themes.FontSize;
   import flash.net.URLRequest;
   import flash.net.URLVariables;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.getDefinitionByName;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.server.EndTurnResponse;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.items.craft.CraftedEquipFamily;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class §_-2t§
   {
      
      public function §_-2t§()
      {
         super();
      }
      
      public static function §_-E9§(param1:int, param2:Boolean = true, param3:Boolean = false, param4:int = 0) : void
      {
         var _loc5_:UserProfile = null;
         var _loc8_:int = 0;
         _loc5_ = Application.userProfile;
         var _loc6_:§_-by§ = _loc5_.§_-XT§();
         var _loc7_:§_-L1r§ = §_-G1p§.§_-d0§(param1,_loc5_.§_-P2e§()) as §_-L1r§;
         if(!(!_loc7_.§_-G1B§() && Boolean(_loc6_.§_-d0§(param1))))
         {
            _loc8_ = _loc7_.§_-G1B§() ? _loc6_.§_-n2k§(_loc7_.getId()) : -1;
            if(_loc7_.§_-G1B§())
            {
               _loc6_.removeItem(param1);
            }
            _loc6_.§_-l2e§(_loc7_,_loc8_,param3,param4);
         }
         if(param2)
         {
            if(param1 >= §_-W2D§.§_-H24§)
            {
               _loc5_.§_-aV§(_loc7_ as §_-T1y§);
            }
            else
            {
               _loc5_.§_-L§(_loc7_ as §_-zF§);
            }
         }
      }
      
      public static function §_-X1r§(param1:§_-N1H§, param2:Object, param3:Boolean = false) : void
      {
         var _loc4_:§_-x2M§ = null;
         var _loc5_:String = null;
         var _loc6_:int = 0;
         var _loc7_:§_-4w§ = null;
         for each(_loc4_ in §_-x2M§.all)
         {
            for(_loc5_ in param2)
            {
               if(_loc5_ == _loc4_.getName())
               {
                  _loc6_ = int(param2[_loc5_]);
                  if(param3)
                  {
                     if(§_-x2M§.§_-31B§(_loc4_))
                     {
                        _loc4_ = §_-x2M§.§_-O2§(_loc4_);
                     }
                     else if(param3 && §_-x2M§.§_-N24§(_loc4_))
                     {
                        _loc6_ = int(_loc6_ * 0.5);
                     }
                  }
                  if(_loc6_ == 0)
                  {
                     break;
                  }
                  var _loc12_:int = 0;
                  var _loc13_:* = param1.§_-b2I§();
                  loop2:
                  while(true)
                  {
                     for each(_loc7_ in _loc13_)
                     {
                        if(_loc7_.getType() == _loc4_)
                        {
                           _loc7_.setValue(_loc7_.getValue() + _loc6_);
                           break loop2;
                        }
                        continue loop2;
                     }
                     param1.addItem(new §_-4w§(_loc4_,_loc6_));
                  }
                  break;
               }
            }
         }
      }
      
      public static function §_-r1j§(param1:uint) : int
      {
         var _loc3_:§_-QK§ = null;
         var _loc4_:§_-Oi§ = null;
         var _loc2_:CraftedEquipFamily = §_-G1p§.§_-713§()[param1];
         for each(_loc3_ in _loc2_.§_-46§())
         {
            for each(_loc4_ in Application.userProfile.§_-XT§().§_-O2Z§())
            {
               if(_loc3_.§_-62n§() == _loc4_.getId())
               {
                  return _loc4_.getId();
               }
            }
         }
         return -1;
      }
      
      public static function §_-TB§(param1:uint) : CraftedEquipFamily
      {
         var _loc2_:CraftedEquipFamily = null;
         var _loc3_:§_-QK§ = null;
         for each(_loc2_ in §_-G1p§.§_-713§())
         {
            for each(_loc3_ in _loc2_.§_-46§())
            {
               if(_loc3_.§_-62n§() == param1)
               {
                  return _loc2_;
               }
            }
         }
         return null;
      }
      
      public static function §_-HJ§(param1:uint) : §_-QK§
      {
         var _loc2_:CraftedEquipFamily = null;
         var _loc3_:§_-QK§ = null;
         for each(_loc2_ in §_-G1p§.§_-713§())
         {
            for each(_loc3_ in _loc2_.§_-46§())
            {
               if(_loc3_.§_-62n§() == param1)
               {
                  return _loc3_;
               }
            }
         }
         return null;
      }
      
      public static function §_-D3Q§() : int
      {
         var _loc1_:§_-by§ = null;
         var _loc4_:CraftedEquipFamily = null;
         var _loc5_:§_-QK§ = null;
         var _loc6_:Boolean = false;
         _loc1_ = Application.userProfile.§_-XT§() as §_-by§;
         var _loc2_:Object = _loc1_.§_-O2Z§();
         var _loc3_:int = 0;
         for each(_loc4_ in §_-G1p§.§_-713§())
         {
            if(!_loc4_.hideInCraft)
            {
               for each(_loc5_ in _loc4_.§_-46§())
               {
                  _loc6_ = Boolean(_loc2_[_loc5_.§_-62n§()]);
                  if((_loc6_) && _loc5_.getType() == §_-z2l§.LEGENDARY)
                  {
                     _loc3_++;
                  }
               }
            }
         }
         return _loc3_;
      }
      
      public static function §_-k1R§(param1:uint) : Boolean
      {
         var _loc2_:§_-QK§ = §_-HJ§(param1);
         return Boolean(_loc2_) && _loc2_.getType() == §_-z2l§.LEGENDARY;
      }
      
      public static function §_-dK§(param1:uint, param2:§_-z2l§) : Boolean
      {
         var _loc3_:§_-QK§ = §_-2t§.§_-HJ§(param1);
         return Boolean(_loc3_) && _loc3_.getType() == param2;
      }
      
      public static function §_-n2Z§(param1:uint) : String
      {
         var _loc2_:§_-W1r§ = §_-G1p§.§_-d0§(param1,§_-L1x§.§_-AU§.§_-wz§());
         var _loc3_:String = _loc2_.getName();
         var _loc4_:§_-QK§ = §_-HJ§(param1);
         var _loc5_:String = _loc4_ ? §_-HJ§(param1).getType().getName() : null;
         if(_loc5_)
         {
            return §_-s2a§.§_-K3t§(_loc5_) + " " + §_-s2a§.§_-K3t§(_loc3_).toLowerCase();
         }
         return §_-s2a§.§_-K3t§(_loc3_);
      }
      
      public static function §_-B34§(param1:§_-L1r§) : String
      {
         var _loc4_:§_-4w§ = null;
         var _loc2_:String = "";
         var _loc3_:Vector.<§_-4w§> = param1.§_-V1v§().§_-b2I§();
         var _loc5_:int = 0;
         for each(_loc4_ in _loc3_)
         {
            if(!_loc4_.§_-I2a§())
            {
               _loc2_ += §_-s2a§.§_-K3t§(_loc4_.getType().§_-23r§()) + ". ";
               _loc5_++;
            }
         }
         return _loc2_;
      }
      
      public static function §_-91M§(param1:§_-L1r§, param2:int) : §_-L1r§
      {
         var _loc4_:§_-L1r§ = null;
         param2 -= §_-72u§.§_-5L§();
         var _loc3_:int = param1.§_-3K§();
         if(_loc3_ == §_-B1K§.HAT)
         {
            _loc4_ = new §_-zF§();
         }
         else if(_loc3_ == §_-B1K§.ARTIFACT)
         {
            _loc4_ = new §_-T1y§();
         }
         param1.copyTo(_loc4_);
         param1.§_-I18§(_loc4_);
         var _loc5_:int = Math.round(param2 / §_-72u§.§_-w2h§);
         _loc4_.§_-o22§(_loc5_);
         _loc4_.§_-4x§(Math.round(param2 / §_-72u§.§_-03d§));
         return _loc4_;
      }
   }
}

