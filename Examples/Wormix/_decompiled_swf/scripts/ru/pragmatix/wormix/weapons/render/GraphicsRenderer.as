package ru.pragmatix.wormix.weapons.render
{
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class GraphicsRenderer implements IRenderer
   {
      
      private var graphics:String;
      
      public function GraphicsRenderer(param1:String)
      {
         super();
         this.graphics = param1;
      }
      
      public function activate(param1:§_-01J§) : void
      {
         param1.§_-s1l§ = this.graphics;
         param1.§_-l2A§();
      }
      
      public function deactivate(param1:§_-01J§) : void
      {
         param1.§_-s1l§ = null;
         param1.§_-l2A§();
      }
      
      public function getHideHandsPattern() : String
      {
         return "none";
      }
   }
}

