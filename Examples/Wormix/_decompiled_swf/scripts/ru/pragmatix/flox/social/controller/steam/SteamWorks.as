package ru.pragmatix.flox.social.controller.steam
{
   import §_-03T§.§_-2p§;
   import §_-31W§.*;
   import §_-52V§.§_-R9§;
   import §_-a19§.*;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import com.amanitadesign.steam.FRESteamWorks;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.flox.social.controller.mobile.FbMobileSocialController;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.textures.Texture;
   
   public class SteamWorks
   {
      
      private static var _instance:SteamWorks;
      
      private var _steamWorksExtension:FRESteamWorks;
      
      public function SteamWorks(param1:Object)
      {
         super();
         var _loc2_:Boolean = param1 is SecureObject;
         if(!_loc2_)
         {
            throw new Error("SteamWorks instance must be retrived via SteamWorks.getInstance()");
         }
         this._steamWorksExtension = new FRESteamWorks();
         this._steamWorksExtension.init();
      }
      
      public static function getInstance() : FRESteamWorks
      {
         if(_instance == null)
         {
            _instance = new SteamWorks(new SecureObject());
         }
         return _instance.steamWorksExtension;
      }
      
      public function get steamWorksExtension() : FRESteamWorks
      {
         return this._steamWorksExtension;
      }
   }
}

class SecureObject
{
   
   public function SecureObject()
   {
      super();
   }
}
