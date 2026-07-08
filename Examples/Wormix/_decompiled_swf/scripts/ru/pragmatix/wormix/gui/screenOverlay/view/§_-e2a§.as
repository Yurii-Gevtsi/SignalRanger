package ru.pragmatix.wormix.gui.screenOverlay.view
{
   import §_-C3f§.§_-4I§;
   import §_-DJ§.§_-73§;
   import §_-DJ§.§_-fH§;
   import §_-Ly§.§_-81L§;
   import §_-YF§.§_-q2v§;
   import §_-r1C§.§_-h2B§;
   import §_-w2W§.§_-21w§;
   import feathers.controls.Button;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-O1f§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class §_-e2a§ implements §_-O1f§
   {
      
      private var _buttons:Dictionary;
      
      private var §_-O24§:§_-r1d§;
      
      public function §_-e2a§()
      {
         super();
         this._buttons = new Dictionary();
      }
      
      public function dispose() : void
      {
         var _loc1_:Object = null;
         var _loc2_:EventDispatcher = null;
         for(_loc1_ in this._buttons)
         {
            _loc2_ = _loc1_ as EventDispatcher;
            if(_loc2_)
            {
               _loc2_.removeEventListener(Event.TRIGGERED,this.§_-r6§);
            }
         }
         this._buttons = null;
         this.§_-Y2v§();
      }
      
      public function §_-r1t§(param1:EventDispatcher, param2:int) : void
      {
         this._buttons[param1] = param2;
         param1.addEventListener(Event.TRIGGERED,this.§_-r6§);
      }
      
      private function §_-r6§(param1:Event) : void
      {
         var _loc2_:EventDispatcher = param1.target as EventDispatcher;
         if(param1.target is Button)
         {
            (param1.target as Button).isEnabled = false;
            (param1.target as Button).isEnabled = true;
         }
         var _loc3_:int = int(this._buttons[_loc2_]);
         switch(_loc3_)
         {
            case §_-in§.§_-nu§:
               §_-h2B§.play(§_-81L§.§_-o1Z§);
               §_-fH§.§_-q1a§(§_-s2a§.§_-K3t§(§_-s2a§.TITLE_EXIT),§_-s2a§.§_-K3t§(§_-s2a§.DESCRIPTION_EXIT),§_-fH§.§_-X2g§,Application.§_-i1N§.§_-F3l§);
               break;
            case §_-in§.§_-Z20§:
               §_-h2B§.play(§_-81L§.§_-o1Z§);
               §_-73§.§_-D3u§(§_-q2v§.§_-X2y§);
               break;
            case §_-in§.SHOP:
               §_-h2B§.play(§_-d27§.SHOP);
               Application.§_-E1k§.showScreen(§_-21w§.§_-81H§);
               break;
            case §_-in§.§_-sg§:
               §_-h2B§.play(§_-d27§.§_-sg§);
               §_-N1V§.§_-42k§.§_-B1f§();
         }
      }
      
      public function §_-32X§(param1:§_-r1d§) : void
      {
         this.§_-Y2v§();
         this.§_-O24§ = param1;
         this.§_-O24§.addEventListener(Event.CHANGE,this.§_-K22§);
         this.refresh(this.§_-O24§);
      }
      
      private function §_-Y2v§() : void
      {
         if(this.§_-O24§)
         {
            this.§_-O24§.removeEventListener(Event.CHANGE,this.§_-K22§);
            this.§_-O24§ = null;
         }
      }
      
      private function §_-K22§(param1:Event) : void
      {
         var _loc2_:§_-r1d§ = param1.target as §_-r1d§;
         if(_loc2_)
         {
            this.refresh(_loc2_);
         }
      }
      
      private function refresh(param1:§_-r1d§) : void
      {
      }
   }
}

