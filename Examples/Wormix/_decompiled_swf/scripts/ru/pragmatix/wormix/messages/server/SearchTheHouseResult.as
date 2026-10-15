package ru.pragmatix.wormix.messages.server
{
   import §_-31W§.*;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-p2U§;
   import §_-D3A§.§_-UK§;
   import §_-DJ§.§_-J2l§;
   import §_-L2F§.§_-I1q§;
   import §_-L2F§.§_-t2Z§;
   import §_-P1z§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1b§.*;
   import §_-YF§.§_-Zi§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-j2N§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-s20§.§_-J2g§;
   import §_-x2Q§.§_-dU§;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.weapons.ammo.feats.IFrameFeat;
   import starling.events.Event;
   
   public class SearchTheHouseResult
   {
      
      public static const EMPTY:int = 0;
      
      public static const REAL_MONEY:int = 1;
      
      public static const MONEY:int = 2;
      
      public static const NO_FIVE_DAY:int = 4;
      
      public static const ERROR:int = 3;
      
      public static const KEY_LIMIT_EXEED:int = 5;
      
      public static const REAGENT:int = 7;
      
      public var result:int;
      
      public var value:int;
      
      public var availableSearchKeys:int;
      
      public var friendId:uint;
      
      public var isSecure:Boolean;
      
      public function SearchTheHouseResult()
      {
         super();
      }
   }
}

