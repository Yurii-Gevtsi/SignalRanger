package ru.pragmatix.wormix.utils
{
   import §_-51W§.§_-SN§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-L1r§;
   import §_-62§.§_-O2H§;
   import §_-62§.§_-W1r§;
   import §_-62§.§_-gr§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-Es§;
   import §_-DJ§.§_-fH§;
   import §_-J3b§.*;
   import §_-Nq§.§_-G1J§;
   import §_-Nq§.§_-Ow§;
   import §_-Nq§.§_-s1O§;
   import §_-Nq§.§_-uh§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-C3A§;
   import §_-k2s§.§_-fG§;
   import §_-r1C§.§_-h2B§;
   import §_-u2J§.§_-T1K§;
   import §_-z1g§.§_-82X§;
   import com.distriqt.extension.cloudstorage.events.KeyValueStoreEvent;
   import com.distriqt.extension.core.Core;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.data.ArrayCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.TiledRowsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.rewards.§_-qU§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   
   public class §_-o1q§
   {
      
      private static var inited:Boolean;
      
      public function §_-o1q§()
      {
         super();
      }
      
      public static function §_-y1Q§(param1:String = "") : void
      {
         if(inited)
         {
            return;
         }
         inited = true;
         Core.init();
      }
   }
}

