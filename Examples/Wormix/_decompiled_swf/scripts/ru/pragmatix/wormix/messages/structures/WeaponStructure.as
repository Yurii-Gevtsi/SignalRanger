package ru.pragmatix.wormix.messages.structures
{
   import §_-A1K§.§_-TA§;
   import §_-Af§.§_-k1I§;
   import §_-D3A§.§_-TZ§;
   import §_-TF§.§_-q1j§;
   import §_-Y1b§.§_-81l§;
   import §_-YF§.§_-q2v§;
   import §_-aM§.§_-P1N§;
   import §_-b1F§.§_-G4§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-y1s§.§_-P9§;
   import com.distriqt.extension.cloudstorage.events.KeyValueStoreEvent;
   import com.mesmotronic.ane.AndroidID;
   import flash.events.TimerEvent;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.text.TextField;
   import starling.text.TextFieldAutoSize;
   
   public class WeaponStructure
   {
      
      public var id:uint;
      
      private var count:§_-TA§;
      
      public function WeaponStructure(param1:uint = 0, param2:int = 0)
      {
         super();
         this.id = param1;
         this.count = new §_-TA§(param2);
      }
      
      public function initFromBackpackItem(param1:§_-Oi§) : WeaponStructure
      {
         this.id = param1.getRecord().getId();
         this.count = new §_-TA§(param1.getCount());
         return this;
      }
      
      public function getId() : uint
      {
         return this.id;
      }
      
      public function getCount() : int
      {
         return this.count.value;
      }
      
      public function addCount(param1:int) : void
      {
         this.count.value += param1;
      }
      
      public function countIncrement() : void
      {
         ++this.count.value;
      }
      
      public function toString() : String
      {
         return "WeaponStructure{id=" + String(this.id) + ",count=" + String(this.count.value) + "}";
      }
   }
}

