package ru.pragmatix.flox.serialization
{
   import §_-31N§.§_-42G§;
   import §_-31Y§.§_-D3B§;
   import §_-62§.§_-zF§;
   import §_-A1K§.§_-TA§;
   import §_-C2t§.§_-92W§;
   import §_-D3A§.§_-TZ§;
   import §_-F39§.*;
   import §_-L1H§.§_-C3e§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-81l§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-j1w§.§_-L1x§;
   import §_-l1g§.§_-L3Q§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.*;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-M2v§;
   import §_-z1g§.§_-y1S§;
   import com.greensock.*;
   import com.greensock.core.*;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.system.LoaderContext;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.LaserRay;
   import starling.display.DisplayObject;
   
   public class §_-616§ extends §_-q2q§
   {
      
      private static const _log:§_-11S§ = new §_-11S§(§_-616§);
      
      private static const §_-P2s§:Vector.<String> = new <String>["data/serializers.xml"];
      
      private static var §_-i1N§:§_-616§ = null;
      
      public function §_-616§(param1:String, param2:String, param3:LoaderContext)
      {
         super(param1,param2,§_-616§.§_-P2s§,param3);
         if(§_-i1N§ == null)
         {
            §_-i1N§ = this;
            return;
         }
         throw new IllegalOperationError("double instatce for singelton BinarySerializer");
      }
      
      public static function getInstance() : §_-616§
      {
         return §_-616§.§_-i1N§;
      }
      
      public static function §_-fk§(param1:Object) : §_-02j§
      {
         return §_-02j§(§_-i1N§.§_-b9§[getQualifiedClassName(param1)]);
      }
      
      public static function §_-K1E§(param1:Object) : §_-D2L§
      {
         return §_-D2L§(§_-i1N§.§_-926§[getQualifiedClassName(param1)]);
      }
      
      override protected function get log() : §_-11S§
      {
         return _log;
      }
   }
}

