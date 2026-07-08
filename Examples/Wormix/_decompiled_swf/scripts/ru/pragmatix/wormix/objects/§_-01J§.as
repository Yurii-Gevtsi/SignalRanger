package ru.pragmatix.wormix.objects
{
   import §_-62§.§_-T1y§;
   import §_-62§.§_-zF§;
   import §_-73I§.§_-Z2e§;
   import §_-A1K§.§_-TA§;
   import §_-P1B§.§_-EB§;
   import §_-P2i§.§_-G3J§;
   import §_-U1i§.§_-I3t§;
   import §_-Y1O§.§_-V23§;
   import §_-b1F§.§_-H3Z§;
   import §_-b1F§.§_-v2Q§;
   import §_-j2R§.§_-v1T§;
   import flash.geom.Point;
   import ru.pragmatix.§_-D3R§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.bonus.§_-z17§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public interface §_-01J§ extends §_-D3R§
   {
      
      function §_-o1z§(param1:Boolean = true) : void;
      
      function §_-gU§(param1:IPhysicModel) : void;
      
      function §_-dz§() : void;
      
      function §_-41F§(param1:Point, param2:Number) : void;
      
      function onUnstable(param1:Event) : void;
      
      function onFalling(param1:Event) : void;
      
      function §_-f2T§() : void;
      
      function §_-K1A§(param1:§_-i14§) : void;
      
      function §_-r1V§(param1:int) : void;
      
      function §_-L§(param1:§_-zF§) : void;
      
      function §_-aV§(param1:§_-T1y§) : void;
      
      function §_-P1t§(param1:int) : void;
      
      function §_-G15§(param1:int, param2:int) : void;
      
      function nextState() : void;
      
      function §_-ab§(param1:Number) : void;
      
      function activate() : void;
      
      function §_-F27§(param1:Boolean) : void;
      
      function deactivate() : void;
      
      function §_-f1r§() : Boolean;
      
      function §_-l2A§(param1:Boolean = false) : void;
      
      function §_-oK§(param1:DisplayObject, param2:int = 0, param3:int = 0, param4:Boolean = true) : void;
      
      function §_-d2T§(param1:DisplayObject) : void;
      
      function §_-U8§(param1:DisplayObject, param2:int = 0, param3:int = 0, param4:Boolean = true, param5:Boolean = false, param6:Boolean = true) : void;
      
      function §_-n28§(param1:DisplayObject) : void;
      
      function get crawling() : int;
      
      function set crawling(param1:int) : void;
      
      function get rolling() : int;
      
      function set rolling(param1:int) : void;
      
      function get keepUnstable() : Boolean;
      
      function set keepUnstable(param1:Boolean) : void;
      
      function defaultOnDamaged(param1:Event, param2:§_-v2Q§) : void;
      
      function §_-f15§() : void;
      
      function §_-n2t§() : void;
      
      function §_-q2T§() : void;
      
      function defaultOnDisposed(param1:Event = null) : void;
      
      function §_-r8§() : void;
      
      function §_-ak§(param1:UserProfile, param2:int, param3:int) : void;
      
      function turnRight() : void;
      
      function turnLeft() : void;
      
      function §_-o1k§(param1:Boolean = false) : void;
      
      function §_-u2K§(param1:Boolean = false) : void;
      
      function turn(param1:int) : void;
      
      function §_-rN§(param1:int, param2:Boolean = false) : void;
      
      function §_-Ck§(param1:Boolean = false) : void;
      
      function §_-01D§() : void;
      
      function rotateUntilStable(param1:Boolean = true) : void;
      
      function §_-H2R§() : void;
      
      function §_-22t§(param1:Event) : void;
      
      function §_-P1W§(param1:uint) : void;
      
      function reattachWeapon() : void;
      
      function §_-W1u§(param1:int) : void;
      
      function §_-U1S§() : void;
      
      function attachTrail() : void;
      
      function §_-B1C§() : void;
      
      function get hp() : int;
      
      function §_-OQ§() : Boolean;
      
      function get bonuses() : §_-z17§;
      
      function get buffs() : §_-v1T§;
      
      function §_-z1Q§() : void;
      
      function §_-M2z§() : void;
      
      function getNameHintInfoPanel() : String;
      
      function getSquadNumber() : uint;
      
      function get height() : int;
      
      function get width() : int;
      
      function isImmobile() : Boolean;
      
      function §_-e14§(param1:Boolean) : void;
      
      function getHat() : §_-zF§;
      
      function §_-32q§() : §_-T1y§;
      
      function get §_-vp§() : Boolean;
      
      function set §_-vp§(param1:Boolean) : void;
      
      function get §_-9u§() : Boolean;
      
      function set §_-9u§(param1:Boolean) : void;
      
      function §_-c1F§() : void;
      
      function §_-K13§() : void;
      
      function §_-s2C§() : Boolean;
      
      function get weapon() : Weapon;
      
      function set weapon(param1:Weapon) : void;
      
      function get §_-81o§() : Weapon;
      
      function set §_-81o§(param1:Weapon) : void;
      
      function get squad() : §_-V23§;
      
      function set squad(param1:§_-V23§) : void;
      
      function get record() : UserProfile;
      
      function set record(param1:UserProfile) : void;
      
      function get §_-m1X§() : Number;
      
      function set §_-m1X§(param1:Number) : void;
      
      function get direction() : int;
      
      function set direction(param1:int) : void;
      
      function get graphics() : §_-Z2e§;
      
      function set graphics(param1:§_-Z2e§) : void;
      
      function get §_-vt§() : Boolean;
      
      function set §_-vt§(param1:Boolean) : void;
      
      function get aiArsenal() : §_-A1l§;
      
      function set aiArsenal(param1:§_-A1l§) : void;
      
      function get aiMissFactor() : int;
      
      function set aiMissFactor(param1:int) : void;
      
      function get aiShotPower() : int;
      
      function set aiShotPower(param1:int) : void;
      
      function get §_-G1e§() : Boolean;
      
      function set §_-G1e§(param1:Boolean) : void;
      
      function get §_-s1l§() : String;
      
      function set §_-s1l§(param1:String) : void;
      
      function get attributes() : §_-EB§;
      
      function get §_-9K§() : Boolean;
      
      function set §_-9K§(param1:Boolean) : void;
      
      function get §_-91i§() : Boolean;
      
      function set §_-91i§(param1:Boolean) : void;
      
      function get lastDamager() : §_-H3Z§;
      
      function §_-Sk§(param1:DamageType) : void;
      
      function §_-Cs§(param1:§_-x2M§) : int;
      
      function getPhysicModel() : IPhysicModel;
      
      function get physParams() : PhysParams;
      
      function set physParams(param1:PhysParams) : void;
      
      function get x() : int;
      
      function set x(param1:int) : void;
      
      function get y() : int;
      
      function set y(param1:int) : void;
      
      function get stable() : Boolean;
      
      function set stable(param1:Boolean) : void;
      
      function addSpeedXY(param1:Number, param2:Number) : void;
      
      function setSpeedVector(param1:Number, param2:Number) : void;
      
      function setSpeedXY(param1:Number, param2:Number) : void;
      
      function get speedX() : Number;
      
      function get speedY() : Number;
      
      function get speed() : Number;
      
      function get angle() : Number;
      
      function get disposed() : Boolean;
      
      function set disposed(param1:Boolean) : void;
      
      function getId() : uint;
      
      function get clip() : DisplayObject;
      
      function get §_-m13§() : DisplayObject;
      
      function get §_-92S§() : Boolean;
      
      function set §_-92S§(param1:Boolean) : void;
      
      function §_-E17§(param1:§_-TA§) : void;
      
      function get §_-F2x§() : §_-I3t§;
      
      function set §_-F2x§(param1:§_-I3t§) : void;
      
      function get controlsMap() : §_-G3J§;
      
      function get §_-m9§() : Boolean;
      
      function set §_-m9§(param1:Boolean) : void;
   }
}

