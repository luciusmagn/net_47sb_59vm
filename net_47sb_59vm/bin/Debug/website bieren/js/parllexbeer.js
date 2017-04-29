var teller = 0;

$(function() {
var parllax={
 level:-313,
 init:function(){
  $this=this;
  $(window).scroll(function(){ 
   $this.start();
  });
  $("#reset").click(function(){
   $this.reset();
  });
 },
 start:function(){ 
  $this=this;
  if($this.level >= -2186){
   $('#bearhold').css('background-position',$this.level+'px 0');
   $this.level=this.level-313;
  }
    else{
      $('#reset').show();
    }
 },
 reset:function(){
teller ++;
		document.getElementById("teller").innerHTML = teller;
    $('#reset').hide();
  var i=this.level; 
      while(i <= 0){ 
   $('#bearhold').css({'background-position':i+'px 0'},800);
   i=i+313;
  }
  this.level=-313; 
 }
};
parllax.init();
});