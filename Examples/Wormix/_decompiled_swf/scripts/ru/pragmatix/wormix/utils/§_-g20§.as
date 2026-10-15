package ru.pragmatix.wormix.utils
{
   import §_-62§.§_-G1p§;
   import §_-62§.§_-a3§;
   import §_-B2z§.§_-63i§;
   import §_-H3§.*;
   import §_-TF§.§_-P19§;
   import §_-Vg§.ClanSearchView;
   import §_-Y1Q§.§_-aG§;
   import §_-r1C§.§_-U2p§;
   import §_-zI§.§_-F3q§;
   import com.distriqt.extension.application.Application;
   import com.distriqt.extension.application.display.Display;
   import com.distriqt.extension.application.display.DisplayCutout;
   import feathers.utils.textures.TextureCache;
   import flash.display.BitmapData;
   import flash.geom.Rectangle;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import starling.display.DisplayObject;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-g20§
   {
      
      private static var §_-a2M§:TextureCache = new TextureCache();
      
      public function §_-g20§()
      {
         super();
      }
      
      public static function §_-W2i§() : TextureCache
      {
         return §_-a2M§;
      }
      
      public static function §_-3i§(param1:int, param2:int) : String
      {
         var _loc3_:§_-a3§ = §_-G1p§.§_-T2l§(param1);
         if(_loc3_)
         {
            return _loc3_.§_-3i§(param2);
         }
         return "";
      }
   }
}

