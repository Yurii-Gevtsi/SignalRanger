package ru.pragmatix.wormix.weapons
{
   import §_-62§.§_-G1p§;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   
   public class SledgehammerDownGradable extends Sledgehammer implements §_-O1c§
   {
      
      public function SledgehammerDownGradable()
      {
         super();
      }
      
      override protected function §_-F8§() : uint
      {
         var _loc1_:Boolean = false;
         _loc1_ = shotParams.§_-R18§ || !property["holeDepth"];
         return !_loc1_ ? uint(property["holeDepth"]) : uint(super.§_-F8§());
      }
   }
}

