package ru.pragmatix.wormix.game.rules
{
   import §_-62§.§_-zF§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-D3A§.*;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-H2g§.§_-di§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-Y1O§.§_-V23§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-w2i§.§_-Zd§;
   import com.greensock.*;
   import com.greensock.core.*;
   import flash.events.*;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   
   public class §_-21V§
   {
      
      public function §_-21V§()
      {
         super();
         throw new Error("Rules can\'t be instantiated!");
      }
      
      public static function §_-t1l§(param1:§_-O2i§) : §_-l1m§
      {
         switch(param1.battleType)
         {
            case §_-81M§.§_-p1U§:
               return new §_-E1q§(param1);
            case §_-81M§.§_-e1C§:
               return new §_-b1M§(param1);
            case §_-81M§.§_-K23§:
               return new §_-H3o§(param1);
            case §_-81M§.§_-Y1C§:
            case §_-81M§.§_-A1W§:
               return new §_-f2§(param1);
            default:
               throw new Error("Can\'t find apropriate rules for battle type " + param1.battleType.value);
         }
      }
   }
}

