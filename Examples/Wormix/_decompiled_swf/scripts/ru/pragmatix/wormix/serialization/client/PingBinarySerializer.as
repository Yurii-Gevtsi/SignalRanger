package ru.pragmatix.wormix.serialization.client
{
   import §_-02q§.§_-4s§;
   import §_-21p§.§_-4w§;
   import §_-62§.*;
   import §_-91B§.§_-z2l§;
   import §_-A1K§.§_-TA§;
   import §_-E3c§.§_-8W§;
   import §_-F2Q§.GestureEvent;
   import §_-N8§.§_-2C§;
   import §_-N8§.§_-o2w§;
   import §_-T2h§.*;
   import §_-Vg§.ClanEditView;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-d2j§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-Ia§;
   import §_-z20§.§_-Tn§;
   import §_-zI§.§_-F3q§;
   import com.hurlant.math.BigInteger;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import flash.events.Event;
   import flash.net.Socket;
   import flash.text.TextField;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-zi§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.Ping;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.MolotovProjectile;
   import starling.events.Event;
   import starling.extensions.ParticleAura;
   
   public class PingBinarySerializer implements §_-D2L§
   {
      
      public function PingBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 16;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:Ping = Ping(param1);
         var _temp_1:* = new §_-5g§();
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return new Ping();
      }
   }
}

