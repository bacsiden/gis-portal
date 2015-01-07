jQuery(document).ready(function() {
	
 
 jQuery("input[name='site_type']").click(function() {
    site_type = this.value;
	if(site_type=="boxed"){
	
		jQuery('#boxed').css('max-width', '1080px');
		jQuery('#boxed').css('margin', '0 auto');
		jQuery('#boxed').css('background', '#FFFFFF');	
		
		jQuery('.navbar-wrapper').css('max-width', '1080px');
		jQuery('.navbar-wrapper').css('margin', '0 auto');		
		jQuery('.navbar-wrapper').css('left', 'auto');
		
	} else{
		jQuery('#boxed').css('max-width', '100%');
		jQuery('.navbar-wrapper').css('max-width', '100%');
		jQuery('.navbar-wrapper').css('left', '0');
		//jQuery('.footerbox').css('width', '100%');
		//jQuery('.footer').css('width', '100%');
	}
});


 jQuery('#primary_color').click(function(){
    jQuery('.iris-picker').toggle();
  });
  
  
  jQuery('.et-color-field').iris({
    palettes: true,
    border: false,
    width: 155,
    color: jQuery('#et_color_field').val(),
    mode: 'hsl',
    palettes: ['#e34461', '#32a0c2', '#82bc1e', '#c01a1a', '#f6bb17', '#54737c'],
    change: function(event, ui) {
              

       /* Hover fix */      
        jQuery('head').append('<style type="text/css">[class*="span"] .box-style:hover span:after, .blog h2 a:hover, .team-data h2:hover span,.team-sr li a:hover span,.s_need ul li:before,.s_need ul li a:hover , .date-list ul li a:hover,.widget ul li:hover a,.fbox ul li a:hover,.color-m span:hover,a:hover, a:focus, a span, span, a,.angle h2 span,.p404 h1, .p404 h1,.color-m span, .ui-tabs .ui-tabs-nav li.ui-tabs-active a,.ui-tabs .ui-tabs-nav li.ui-tabs-active span, .tabs h2, .color, .defult_color, .green_color,.footerbox span.color {color:'+ ui.color.toString() +'} .button:hover,.box-style:hover .btn-white, .likebtn a.l-btn:hover, .button.active2, .home-now, #accordion_op .ui-state-active .ui-accordion-header-icon,a.button,.button,.bg-color,#accordion_op .ui-accordion-header.ui-state-active .ui-accordion-header-icon, #portfolio_hover li .hover,.follower a:hover,.pagenavi li a:hover,.prev2 a:hover, .next2 a:hover,.follower a.fr:hover,.follower a.sr:hover,.small-slider .carousel-caption h1 span, .team-sr:hover,.service2:hover,p-nav ul li:hover:before, .tag a:hover,.probtn:hover,#accordion_os .ui-state-active .ui-accordion-header-icon,.p-nav ul li a.active:before, .pagenavi li a.active, .p-nav ul li:hover:before, .skillset .skill_set_item .progress-bar .progress-bar-content{background-color:'+ ui.color.toString() +'} .prev2 a:hover, .next2 a:hover,.team-sr:hover,.service2:hover, .pagenavi li a:hover,.p-nav ul li:hover:before,.probtn:hover,.tag a:hover,.follower a.fr:hover, .small-slider .carousel-caption h1 span,.portfolio .hover span,.active,.skillset .skill_set_item .progress-bar .progress-bar-content, *::-moz-selection, .bg-color, #portfolio_hover li .hover,.pagenavi li a.active, .portfolio:hover h2,.follower a.sr:hover,.p-nav ul li a.active:before  {background: none repeat scroll 0 0 '+ ui.color.toString() +'}.box-style:hover, .box-style:hover .btn-white,  .team-data h2:hover span,.team-sr:hover,.team-sr li a:hover,.service2:hover, .pagenavi li a:hover, .p-nav ul li:hover:before, .p-nav ul li a.active:before, .probtn:hover,.widget ul li:hover,.tag a:hover,.fbox ul li:hover,.flickr ul li:hover,.color:hover, .color-m span:hover, .follower a:hover, a span, span,.nav-btn a.activeSlide,.active, .v-green,.color,.color-m span, a.button,.button,.follower a.fr:hover,#main-menu ul li ul li a:hover, #menu ul li ul li.current-menu-item a, #menu ul li ul li.current-page-item a,#accordion_op .ui-accordion-header.ui-state-active:before, #accordion_op .ui-accordion-content.ui-accordion-content-active, #accordion_op .ui-accordion-header.ui-state-active {border-color: '+ ui.color.toString() +'} .service_box_arrow:hover .arrow_box:after{  border-color: rgba(227, 229, 231, 0) rgba(227, 229, 231, 0) rgba(227, 229, 231, 0) '+ ui.color.toString() +'} /style>');
		
		
		var can = document.getElementById('canvas4');
		var context = can.getContext('2d');
		context.strokeStyle =  ui.color.toString();
		context.stroke();
		
		var can = document.getElementById('canvas3');
		var context = can.getContext('2d');
		context.strokeStyle = ui.color.toString();
		context.stroke();
		
		var can = document.getElementById('canvas2');
		var context = can.getContext('2d');
		context.strokeStyle = ui.color.toString();
		context.stroke();
		
		var can = document.getElementById('canvas1');
		var context = can.getContext('2d');
		context.strokeStyle = ui.color.toString();
		context.stroke();
		
		var can = document.getElementById('canvas');
		var context = can.getContext('2d');
		context.strokeStyle = ui.color.toString();
		context.stroke();

     }
   });
   
            jQuery('#pattern_img img').click(function(){    
				if(site_type=="boxed"){

					var pattern=(jQuery(this).attr("name"));
              
					jQuery('body').css({ background: 'none ;' });
						jQuery('body').css({ background: 'url(../buzz/front-end-ops/images/patterns/'+pattern+'.png) repeat scroll center center transparent' });
      				
						jQuery('#pattern_img img').removeClass("active");
						jQuery(this).addClass("active");
					}else{
						alert("Background is only for Boxed Model.");
					}	
              		
             });



            jQuery('#bgimage img').click(function(){ 

			if(site_type=="boxed"){			
			 var bgimg=(jQuery(this).attr("name"));
				
				jQuery('body').css({ background: 'none ;' });
				jQuery('body').css({ background: 'url(../buzz/front-end-ops/images/bgimages/'+bgimg+'.jpg) no-repeat  fixed center center / cover  transparent' });
              
				jQuery('#bgimage img').removeClass("active");
				jQuery(this).addClass("active");
              } else{
						alert("Background is only for Boxed Model.");
					}	    
             });




				
	
	jQuery('#front_end_customize').click(function(e) {
		jQuery('.customize-area').toggle();
		jQuery('#front_end_customize').css('display','none');
	});
	
	jQuery('#et-control-close').click(function(e) {
		jQuery('.customize-area').toggle('slow');
		jQuery('#front_end_customize').css('display','block');
	});
	

});		


