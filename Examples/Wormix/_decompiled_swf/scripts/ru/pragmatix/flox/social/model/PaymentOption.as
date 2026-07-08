package ru.pragmatix.flox.social.model
{
   import §_-73d§.§_-h2X§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-E19§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-W§.§_-ix§;
   import §_-Z1K§.§_-q24§;
   import flash.geom.Point;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.pvp.messages.server.PvpSystemMessage;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import starling.display.DisplayObject;
   
   public class PaymentOption
   {
      
      public var code:uint;
      
      public var count:uint;
      
      public var bonus:uint;
      
      public var price:Number;
      
      public var name:String;
      
      public var descr:String;
      
      public var moneyType:§_-E19§;
      
      public var icon:String;
      
      public function PaymentOption(param1:uint, param2:uint, param3:Number, param4:§_-E19§, param5:String = null)
      {
         super();
         this.count = param2;
         this.price = param3;
         this.code = param1;
         this.moneyType = param4;
         this.name = param5;
      }
      
      public static function createBonusOption(param1:uint, param2:uint, param3:uint, param4:Number, param5:§_-E19§, param6:String = null, param7:String = null) : PaymentOption
      {
         var _loc8_:PaymentOption = new PaymentOption(param1,param2,param4,param5,param6);
         _loc8_.bonus = param3;
         _loc8_.icon = param7;
         return _loc8_;
      }
      
      public function toString() : String
      {
         return "PaymentOption: {" + " code=" + this.code + " name=" + this.name + " price=" + this.price + " moneyType=" + this.moneyType + " count=" + this.count + " bonus=" + this.bonus + " icon=" + this.icon + " descr=" + this.descr + " }";
      }
   }
}

