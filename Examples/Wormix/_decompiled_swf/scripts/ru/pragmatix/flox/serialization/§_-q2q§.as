package ru.pragmatix.flox.serialization
{
   import §_-31W§.*;
   import §_-63U§.§_-NV§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-fH§;
   import §_-J31§.§_-R1h§;
   import §_-Ly§.§_-81L§;
   import §_-QD§.§_-G2z§;
   import §_-U1i§.§_-U16§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1w§.§_-s1x§;
   import §_-Z1w§.§_-w1D§;
   import §_-aM§.*;
   import §_-fo§.*;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-hO§.§_-v24§;
   import §_-l1K§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import com.hurlant.util.§_-a2P§;
   import feathers.core.FeathersControl;
   import flash.display.DisplayObject;
   import flash.geom.Point;
   import flash.net.LocalConnection;
   import flash.system.Capabilities;
   import flash.system.LoaderContext;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getDefinitionByName;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.wormix.controller.§_-S2Y§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-G1F§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.§_-7D§;
   import ru.pragmatix.wormix.messages.server.AssembleStuffResult;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.FreezingMissile;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-q2q§ extends AbstractConfigLoader
   {
      
      protected var §_-y1m§:Object = new Object();
      
      protected var §_-926§:Object = new Object();
      
      protected var §_-b9§:Object = new Object();
      
      public function §_-q2q§(param1:String, param2:String, param3:Vector.<String>, param4:LoaderContext)
      {
         super(param1,param2,param3,§_-v2P§.XML,param4);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:§_-D2L§ = this.§_-y1m§[param2.getCommandId()];
         if(_loc3_ != null)
         {
            return _loc3_.deserializeCommand(param1,param2);
         }
         throw new ArgumentError("Can\'t find serializer for command: " + param2.getCommandId());
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:§_-D2L§ = this.§_-926§[getQualifiedClassName(param1)];
         if(_loc3_ != null)
         {
            _loc3_.serializeCommand(param1,param2);
            return;
         }
         throw new ArgumentError("Can\'t find serializer for class: " + getQualifiedClassName(param1));
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _temp_1:* = this.§_-b9§.get(getQualifiedClassName(param1));
         var _loc4_:§_-02j§ = this.§_-b9§.get(getQualifiedClassName(param1));
         if(_loc4_ != null)
         {
            _loc4_.serializeStructure(param1,param2,param3);
            return;
         }
         throw new ArgumentError("Can\'t find serializer for class: " + getQualifiedClassName(param1));
      }
      
      override protected function §_-v1c§(param1:XML, param2:String) : void
      {
         var _loc3_:uint = 0;
         var _loc4_:XML = null;
         var _loc5_:String = null;
         var _loc6_:§_-D2L§ = null;
         for each(_loc4_ in param1..command)
         {
            _loc5_ = _loc4_.@serializer.toString();
            _loc6_ = new (getDefinitionByName(_loc5_) as Class)();
            _loc3_ = _loc6_.getCommandId();
            this.§_-y1m§[_loc3_] = _loc6_;
            this.§_-926§[_loc4_.@forClass.toString()] = this.§_-y1m§[_loc3_];
            log.debug("init serializer [" + _loc5_ + "] for class [" + _loc4_.@forClass.toString() + "] with commandId =" + _loc3_);
         }
         for each(_loc4_ in param1..structure)
         {
            _loc5_ = _loc4_.@serializer.toString();
            this.§_-b9§[_loc4_.@forClass.toString()] = new (getDefinitionByName(_loc5_) as Class)();
         }
      }
   }
}

