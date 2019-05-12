function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}

function guid() {
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}

function guidEmpty() {
    return "00000000-0000-0000-0000-000000000000";
}

function convertDateWithTToyyyyMMdd(src) {
    if (src && src != '') {
        var tmp = src.split('T');
        if (tmp && tmp.length > 1) {
            return tmp[0];
        }
    }
    return src;
}

function convertDateToyyyy_MM_dd(d) {

    return d.substring(0, d.indexOf('T'));
}

function convertDateToyyyy_MM_dd_hh_mm_ss(d) {
    d = d.replace('T', ' ');
    if (d.indexOf('.') >= 0) {
        d = d.substring(0, d.indexOf('.'));
    }
    return d;
}

//编码
function html_encode(str) {
    var s = "";
    if (str.length == 0) return "";
    s = str.replace(/&/g, "&amp;");
    s = s.replace(/</g, "&lt;");
    s = s.replace(/>/g, "&gt;");
    s = s.replace(/ /g, "&nbsp;");
    s = s.replace(/\'/g, "&#39;");
    s = s.replace(/\"/g, "&quot;");
    return s;
}

//解码
function html_decode(str) {
    var s = "";
    if (str.length == 0) return "";
    s = str.replace(/&amp;/g, "&");
    s = s.replace(/&lt;/g, "<");
    s = s.replace(/&gt;/g, ">");
    s = s.replace(/&nbsp;/g, " ");
    s = s.replace(/&#39;/g, "\'");
    s = s.replace(/&quot;/g, "\"");
    return s;
}

function delHtmlTag(description) {
    description = description.replace(/(\n)/g, "");
    description = description.replace(/(\t)/g, "");
    description = description.replace(/(\r)/g, "");
    description = description.replace(/<\/?[^>]*>/g, "");
    description = description.replace(/\s*/g, "");
    return description;
}

//不能用于iframe子页面
function QueryString(paramName) {
    var reg = new RegExp("[\?&]" + paramName + "=([^&]*)[&]?", "i");
    var paramVal = window.location.search.match(reg);
    return paramVal == null ? "" : paramVal[1];
}

//
function dlgFrameQueryString(paramName, frameIdOfSonInParentFrame) {
    var reg = new RegExp("[\?&]" + paramName + "=([^&]*)[&]?", "i");
    var iframe = parent.document.getElementById(frameIdOfSonInParentFrame);
    if (iframe) {
        var win = iframe.contentWindow || ifr.contentDocument;
        var paramVal = win.location.search.match(reg);
        return paramVal == null ? "" : paramVal[1];
    }
    else {
        //alert("iframe在父页面的id必须为" + frameIdOfSonInParentFrame + ",否则无法用该方法传值。")
        return QueryString(paramName);
    }
}

function addLoadEvent(func) {
    //把现有的 window.onload 事件处理函数的值存入变量
    var oldOnload = window.onload;
    if (typeof window.onload != "function") {
        //如果这个处理函数还没有绑定任何函数，就像平时那样添加新函数
        window.onload = func;
    } else {
        //如果处理函数已经绑定了一些函数，就把新函数添加到末尾
        window.onload = function () {
            oldOnload();
            func();
        }
    }
}

//获取浏览器可见区域高度
function getWindowSize() {
    var client = {
        y: 0
    };

    if (typeof document.compatMode != 'undefined' && document.compatMode == 'CSS1Compat') {
        client.y = document.documentElement.clientHeight;
    }
    else if (typeof document.body != 'undefined' && (document.body.scrollLeft || document.body.scrollTop)) {
        client.y = document.body.clientHeight;
    }
    return client;
}

//根据相对路径url返回其绝对路径
function getAbsoluteUrl(url) {
    var a = document.createElement('A');
    a.href = url;  // 设置相对路径给Image, 此时会发送出请求
    url = a.href;  // 此时相对路径已经变成绝对路径
    return url;
}

//判断是否是通过手机访问
function IsFromMobile() {
    var system = {
        win: false,
        mac: false,
        xll: false,
        ipad: false
    };
    //检测平台 
    var p = navigator.platform;
    system.win = p.indexOf("Win") == 0;
    system.mac = p.indexOf("Mac") == 0;
    system.x11 = (p == "X11") || (p.indexOf("Linux") == 0);
    system.ipad = (navigator.userAgent.match(/iPad/i) != null) ? true : false;
    //跳转语句，如果是手机访问就自动跳转到wap.baidu.com页面 
    if (system.win || system.mac || system.xll || system.ipad) {
        return false;
    } else {
        return true;
    }
}



function disableTimer(time, obj) {
    obj.addClass('disabled'); // Disables visually  
    //obj.prop('disabled', true); // Disables visually + functionally
    var hander = setInterval(function () {
        if (time <= 0) {
            clearInterval(hander); //清除倒计时
            obj.removeClass("disabled"); // Disables visually  
            //obj.prop('disabled', false); // Disables visually + functionally
            return false;
        }
        else {
            time = time - 1000;
        }
    }, 1000);
}

//cookie start
function setCookie(c_name, value, expiredays) {
    var exdate = new Date()
    exdate.setDate(exdate.getDate() + expiredays)
    document.cookie = c_name + "=" + escape(value) +
        ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString())
}

function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=")
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return unescape(document.cookie.substring(c_start, c_end))
        }
    }
    return ""
}
//cookie end

/**
 * 打印局部div
 * @param printpage 局部div的ID
 */
function printdom(domId) {
    var headhtml = "<html><head><title></title></head><body>";
    var foothtml = "</body>";
    // 获取div中的html内容
    var newhtml = document.all.item(domId).innerHTML;
    // 获取div中的html内容，jquery写法如下
    // var newhtml= $("#" + printpage).html();

    // 获取原来的窗口界面body的html内容，并保存起来
    var oldhtml = document.body.innerHTML;

    // 给窗口界面重新赋值，赋自己拼接起来的html内容
    document.body.innerHTML = headhtml + newhtml + foothtml;
    // 调用window.print方法打印新窗口
    window.print();

    // 将原来窗口body的html值回填展示
    document.body.innerHTML = oldhtml;
    return false;
}
