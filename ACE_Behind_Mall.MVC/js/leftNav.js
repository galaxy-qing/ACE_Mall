
//改动人：jack
//改动，把二维的菜单修改为，显示三维的菜单
// 左侧菜单生成
function navBar(strData) {
	var data;
	if(typeof(strData) == "string"){
		var data = JSON.parse(strData); //部分用户解析出来的是字符串，转换一下
	}else{
		data = strData;
	}	
	var ulHtml = '<ul class="layui-nav layui-nav-tree">';
	for(var i=0;i<data.length;i++){
	    for (var z = 0; z < data[i].Children.length; z++) {
	        if (data[i].Item.Name == "头条管理") {
	            //ulHtml += '<li class="layui-nav-item layui-nav-itemed"  >';
	            ulHtml += '<li class="layui-nav-item layui-nav-itemed ' + data[i].Children[z].Item.ParentId.split('-')[0] + '">';
	        } else {
	            ulHtml += '<li class="layui-nav-item ' + data[i].Children[z].Item.ParentId.split('-')[0] + '" style="display :none" >';
	        }
		    if (data[i].Children[z].Children != undefined && data[i].Children[z].Children.length > 0) {
		        ulHtml += '<a href="javascript:;">';
		        if (data[i].Children[z].Item.IconName != undefined && data[i].Children[z].Item.IconName != '') {
		            if (data[i].Children[z].Item.IconName.indexOf("icon-") != -1) {
		                ulHtml += '<i class="iconfont ' + data[i].Children[z].Item.IconName + '" data-icon="' + data[i].Children[z].Item.IconName + '"></i>';
		            } else {
		                ulHtml += '<i class="layui-icon" data-icon="' + data[i].Children[z].Item.IconName + '">' + data[i].Children[z].Item.IconName + '</i>';
		            }
		        }
		        ulHtml += '<cite>' + data[i].Children[z].Item.Name + '</cite>';
		        ulHtml += '<span class="layui-nav-more"></span>';
		        ulHtml += '</a>';
		        ulHtml += '<dl class="layui-nav-child">';
		        for (var j = 0; j < data[i].Children[z].Children.length; j++) {
		            if (data[i].Children[z].Children[j].target == "_blank") {
		                ulHtml += '<dd><a href="javascript:;" data-url="' + data[i].Children[z].Children[j].Item.Url + '" target="' + data[i].Children[z].Children[j].target + '">';
		            } else {
		                ulHtml += '<dd><a href="javascript:;" data-url="' + data[i].Children[z].Children[j].Item.Url + '">';
		            }
		            if (data[i].Children[z].Children[j].Item.IconName != undefined && data[i].Children[z].Children[j].Item.IconName != '') {
		                if (data[i].Children[z].Children[j].Item.IconName.indexOf("icon-") != -1) {
		                    ulHtml += '<i class="iconfont ' + data[i].Children[z].Children[j].Item.IconName + '" data-icon="' + data[i].Children[z].Children[j].Item.IconName + '"></i>';
		                } else {
		                    ulHtml += '<i class="layui-icon" data-icon="' + data[i].Children[z].Children[j].Item.IconName + '">' + data[i].Children[z].Children[j].Item.IconName + '</i>';
		                }
		            }
		            ulHtml += '<cite>' + data[i].Children[z].Children[j].Item.Name + '</cite></a></dd>';
		        }
		        ulHtml += "</dl>";
		    } else {
		        if (data[i].Children[z].target == "_blank") {
		            ulHtml += '<a href="javascript:;" data-url="' + data[i].Children[z].Item.Url + '" target="' + data[i].Children[z].target + '">';
		        } else {
		            ulHtml += '<a href="javascript:;" data-url="' + data[i].Children[z].Item.Url + '">';
		        }
		        if (data[i].Children[z].Item.IconName != undefined && data[i].Children[z].Item.IconName != '') {
		            if (data[i].Children[z].Item.IconName.indexOf("icon-") != -1) {
		                ulHtml += '<i class="iconfont ' + data[i].Children[z].Item.IconName + '" data-icon="' + data[i].Children[z].Item.IconName + '"></i>';
		            } else {
		                ulHtml += '<i class="layui-icon" data-icon="' + data[i].Children[z].Item.IconName + '">' + data[i].Children[z].Item.IconName + '</i>';
		            }
		        }
		        ulHtml += '<cite>' + data[i].Children[z].Item.Name + '</cite></a>';
		    }
		}
		ulHtml += '</li>';
	}
	ulHtml += '</ul>';
	return ulHtml;
}

// 头部菜单生成
function topMenus(strData) {
    var data;
    if (typeof (strData) == "string") {
        var data = JSON.parse(strData); //部分用户解析出来的是字符串，转换一下
    } else {
        data = strData;
    }
    var ulHtml = '';
    for (var i = 0; i < data.length; i++) {
        var x = data[i].Item.Id.split('-')[0];
        if (data[i].Item.Name=="头条管理") {
            ulHtml += ' <li class="layui-nav-item layui-this"> ';
        } else {
            ulHtml += ' <li class="layui-nav-item">';
        }
        ulHtml += '<a href="javascript:;" " onclick="getfath(\''+x+'\')">';
        ulHtml += '<cite>' + data[i].Item.Name + '</cite></a></li>';
    }
    return ulHtml;
    
}
 
//  实现，头部模块点击，左侧菜单显示和隐藏
function getfath(cla) {
    $('.' + cla).siblings().css('display', 'none');
    $('.' + cla ).css('display', 'block');
}


// 注释掉用来备注，之前的功能
//function navBar(strData) {
//    var data;
//    if (typeof (strData) == "string") {
//        var data = JSON.parse(strData); //部分用户解析出来的是字符串，转换一下
//    } else {
//        data = strData;
//    }
//    var ulHtml = '<ul class="layui-nav layui-nav-tree">';
//    for (var i = 0; i < data.length; i++) {
//        if (data[i].spread) {
//            ulHtml += '<li class="layui-nav-item layui-nav-itemed">';
//        } else {
//            ulHtml += '<li class="layui-nav-item">';
//        }
//        if (data[i].Children != undefined && data[i].Children.length > 0) {
//            ulHtml += '<a href="javascript:;">';
//            if (data[i].Item.IconName != undefined && data[i].Item.IconName != '') {
//                if (data[i].Item.IconName.indexOf("icon-") != -1) {
//                    ulHtml += '<i class="iconfont ' + data[i].Item.IconName + '" data-icon="' + data[i].Item.IconName + '"></i>';
//                } else {
//                    ulHtml += '<i class="layui-icon" data-icon="' + data[i].Item.IconName + '">' + data[i].Item.IconName + '</i>';
//                }
//            }
//            ulHtml += '<cite>' + data[i].Item.Name + '</cite>';
//            ulHtml += '<span class="layui-nav-more"></span>';
//            ulHtml += '</a>';
//            ulHtml += '<dl class="layui-nav-child">';
//            for (var j = 0; j < data[i].Children.length; j++) {
//                if (data[i].Children[j].target == "_blank") {
//                    ulHtml += '<dd><a href="javascript:;" data-url="' + data[i].Children[j].Item.Url + '" target="' + data[i].Children[j].target + '">';
//                } else {
//                    ulHtml += '<dd><a href="javascript:;" data-url="' + data[i].Children[j].Item.Url + '">';
//                }
//                if (data[i].Children[j].Item.IconName != undefined && data[i].Children[j].Item.IconName != '') {
//                    if (data[i].Children[j].Item.IconName.indexOf("icon-") != -1) {
//                        ulHtml += '<i class="iconfont ' + data[i].Children[j].Item.IconName + '" data-icon="' + data[i].Children[j].Item.IconName + '"></i>';
//                    } else {
//                        ulHtml += '<i class="layui-icon" data-icon="' + data[i].Children[j].Item.IconName + '">' + data[i].Children[j].Item.IconName + '</i>';
//                    }
//                }
//                ulHtml += '<cite>' + data[i].Children[j].Item.Name + '</cite></a></dd>';
//            }
//            ulHtml += "</dl>";
//        } else {
//            if (data[i].target == "_blank") {
//                ulHtml += '<a href="javascript:;" data-url="' + data[i].Item.Url + '" target="' + data[i].target + '">';
//            } else {
//                ulHtml += '<a href="javascript:;" data-url="' + data[i].Item.Url + '">';
//            }
//            if (data[i].Item.IconName != undefined && data[i].Item.IconName != '') {
//                if (data[i].Item.IconName.indexOf("icon-") != -1) {
//                    ulHtml += '<i class="iconfont ' + data[i].Item.IconName + '" data-icon="' + data[i].Item.IconName + '"></i>';
//                } else {
//                    ulHtml += '<i class="layui-icon" data-icon="' + data[i].Item.IconName + '">' + data[i].Item.IconName + '</i>';
//                }
//            }
//            ulHtml += '<cite>' + data[i].Item.Name + '</cite></a>';
//        }
//        ulHtml += '</li>';
//    }
//    ulHtml += '</ul>';
//    return ulHtml;
//}