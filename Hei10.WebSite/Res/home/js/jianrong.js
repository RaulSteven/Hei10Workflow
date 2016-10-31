 /*
  * 全局公共脚本
  */



/* 监听导航菜单宽度 */

$(window).resize(function(e) {
	if($('.header').hasClass('cur')){	
	
	}
	w320();
});

/* 监听滚动 */
   
   if($(window).scrollTop()>=70&&$(window).width()>640){
	   $('.header').addClass("cur");
	   }
   
  $(window).scroll(function(e) {
 var i = $(this).scrollTop();
  if(i>70){
       //NavWidth();	
	  if($(window).width()>640){
	  $('.header').addClass("cur");
	  }
	 
  }else{
	  if($(window).width()>640){
	 	 $('.header').removeClass("cur");
		 $('.search-long-wrap').removeAttr("style"); 
	  }
	  $('.search-long-wrap').removeClass("cur");
	  
  }
});

/* end */

 var w320 = function() {
     if($('body,html').width()<640){
		 $('.header').addClass('cur'); 
		 }
     if($(window).scrollTop()<=70&&$(window).width()>640){
			$('.header').removeClass('cur');  
			 }	
	
	
	}
	w320();
	
	
//菜单
$('.menu').click(function() {
   $('.nav-top').show();
});
$('.nav-top .close').click(function() {
   $('.nav-top').hide();
});

function  navbox() {
	if($(window).width()<=640){
		$('.nav-top li').click(function() {
			$('.nav-top').hide();
		});
	}else{
		$('.nav-top').show();
		}
	
}
navbox();
$(window).resize(function() {
  navbox();  
});

/* end */	




$('.box-staff').each(function() {
			var h = 850;
			var thisbox = $(this);
			var tH = $(thisbox).find('.photos').height();
			var box = $(this).find('.container');
			var more = $(this).find('.look-more');
			
		   //判断是否显示按钮
			if (tH <= h) {
				more.hide();
				box.css({ "height": "auto" });
			} else {
				more.show();
				box.removeAttr('style');
			}
			
			// 点击显示更多  
			$(more).click(function () {
				var boxH = box.height();
				var pH = box.find('.photos').height();
				var dataShow = box.attr('data-show');
				if (!dataShow) {
					box.animate({ "height": pH }, 300,
						function () {
							box.css({ "height": "auto" });
						}).attr('data-show', true);
					more.find('a').addClass('cur').text(more.attr('data-hide'));
					return;
				}
				box.animate({ "height": h }, 300,
					function () {
						box.removeAttr('style');
					}
				).removeAttr('data-show');
				more.find('a').removeClass('cur').text(more.attr('data-show'));
	
			});
			
			
        });
	































	 
	 
	
	 
	 
	 
	 
	 