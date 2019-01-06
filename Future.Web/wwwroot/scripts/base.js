//窗口参数配置，继承自基本参数配置和操作单数配置
var windowParamSettings = {
    url: "",
    allowResize: true,
    onDestroy: null,           //销毁时调用
    onLoad: null,              //加载完成时调用页面方法参数为contentWindow
    getDataAction: null,
    title: "无标题",                 //窗口标题
    width: '80%',                //窗口宽度
    height: '80%',               //窗口高度
    addQueryString: true,
    showMaxButton: true,
    funcType: "",              //地址栏FuncType
};

var gridLinkParamSettings = {
    onFilter: null,     //过滤条件函数
    refresh: false,     //关闭后是否刷新grid   
    linkText: "",       //连接文本，默认为当前字段值
    isButton: false,    //是否显示为按钮
    paramField: "Id",   //执行参数字段 
    url: ""             //窗口Url
};

gridLinkParamSettings = jQuery.extend(true, {}, windowParamSettings, gridLinkParamSettings);

//Grid按钮参数配置，继承自执行参数
var gridButtonParamSettings = {
    onFilter: null,           //过滤条件函数
    onButtonClick: null,      //点击按钮执行方法
    linkText: "",             //连接文本，默认为当前字段值
    isButton: false,          //是否显示为按钮
    mustConfirm: true,         //是否需要用户确认
    //confirmBoxTitle
};

function pageInit() {
    //easyui 文本框在table内也自适应
    window.onresize = function () {
        $('.textbox-f').each(function () {
            $(this).next().css({ width: 1, height: 1 });
        }).textbox('resize')
    }

    if (typeof (pageLoad) != "undefined")
        pageLoad();
}

var keyValueArr = {};
function addAjaxParam(key, value) {
    if (typeof (value) == "object" || typeof (value) == "array" || value.constructor == Array || value.constructor == Object)
        value = JSON.stringify(value);

    var newPair = {};
    newPair[key] = value;
    keyValueArr = $.extend(true, {}, keyValueArr, newPair);
}

function commitAjax(url, setting) {
    $.post(url, keyValueArr,
        function (data) {
            if (data) {
                for (var a in data) {
                    //在post后得到数据会将斜杠加引号变为引号，因此要加回去
                    if (Object.prototype.toString.call(data[a]) === '[object String]') {
                        if (data[a].indexOf('"') != -1) {
                            var reg = new RegExp('"', "g");//g,表示全部替换。
                            data[a] = data[a].replace(reg, '\\"')
                        }
                    }
                }
            }

            if (setting.CallBack) {
                setting.CallBack(data);
            }
            keyValueArr = {};
        });
}

function addGridLink(gridId, fieldName, url, gridLinkSettings) {
    var setting = jQuery.extend(true, {}, gridLinkParamSettings, { gridId: gridId }, gridLinkSettings);
    setting["key"] = "th_" + gridId + "." + fieldName;
    setting["gridId"] = gridId;
    setting["url"] = url;
    if (!setting["title"] && setting["linkText"])
        setting["title"] = setting["linkText"];
    pushOrUpdateSettings(setting);

    jQuery("#" + gridId + " th[field='" + fieldName + "']").each(function (index) {
        var jQueryitem = jQuery(this);
        jQueryitem.attr("formatter", "gridLinkFormatter");
        if (setting["linkText"] != "")
            jQueryitem.attr("align", "center");

        jQueryitem.attr("id", "th_" + gridId);//th id 为grid的id加前缀th
    });
}

function gridLinkFormatter(value, row, index) {
    var key = this.id + "." + this.field;
    var settings = getSettings(key);
    if (settings == undefined)
        return value;

    var text = settings.linkText;
    if (text == '') {
        text = value;
    }

    var style = 'color:blue';
    var res = "<a style='" + style + "' href='javascript:void(0)' onclick=gridLinkClick('" + index + "','" + key + "')>" + text + "</a>";
    return res;
}

function gridLinkClick(index, key) {
    var settings = getSettings(key);

    var rows = $('#' + settings["gridId"]).datagrid('getRows');//获得所有行
    var row = rows[index];//根据index获得其中一行。

    var url = settings.url;
    if (settings.paramField && row[settings.paramField]) {
        url += "?" + settings.paramField + "=" + row[settings.paramField];
    }
    openWindow(url, settings);
}

function addGridBtn(gridId, fieldName, gridBtnSettings) {
    var setting = jQuery.extend(true, {}, gridButtonParamSettings, { gridId: gridId }, gridBtnSettings);
    setting["key"] = "th_" + gridId + "." + fieldName;
    setting["gridId"] = gridId;
    if (!setting["title"] && setting["linkText"])
        setting["title"] = setting["linkText"];
    pushOrUpdateSettings(setting);

    jQuery("#" + gridId + " th[field='" + fieldName + "']").each(function (index) {
        var jQueryitem = jQuery(this);
        jQueryitem.attr("formatter", "gridBtnFormatter");
        if (setting["linkText"] != "")
            jQueryitem.attr("align", "center");

        jQueryitem.attr("id", "th_" + gridId);//th id 为grid的id加前缀th        
    });
}

function gridBtnFormatter(value, row, index) {
    var key = this.id + "." + this.field;
    var settings = getSettings(key);
    if (settings == undefined)
        return value;

    var text = settings.linkText;
    if (text == '') {
        text = value;
    }

    var style = 'color:blue';

    var rowJson = JSON.stringify(row);
    var res = "<a style='" + style + "' href='javascript:void(0)' onclick=gridBtnClick('" + index + "','" + key + "')>" + text + "</a>";
    return res;
}

function gridBtnClick(index, key) {
    var settings = getSettings(key);
    var gridId = settings["gridId"];

    var row = $('#' + gridId).mfdatagrid('getRows')[index];
    if ($('#' + gridId).mfdatagrid('endEditing')) {
        if (settings.onButtonClick) {
            settings.onButtonClick(row, index);
        }
    }
}

var gridLinkSettingss = new Array();
//从设置数组中获取设置
function getSettings(key) {

    var settings;
    var settingss = gridLinkSettingss;
    for (var i = 0; i < settingss.length; i++) {
        if (settingss[i]["key"] == key) {
            settings = settingss[i];
            break;
        }
    }

    if (!settings) {
        return;
    }
    return settings;
}

function pushOrUpdateSettings(setting, key) {
    var settingss = gridLinkSettingss;
    if (key)
        setting["key"] = key;

    for (var i = 0; i < settingss.length; i++) {
        if (settingss[i]["key"] == setting["key"]) {
            settingss[i] = setting;
            break;
        }
    }
    if (i == settingss.length || settingss.length == 0)
        settingss.push(setting);
}
//---------------------------id=mf_form------------------------------start
function saveFormData() {
    var selector = $("form[id='mf_form']");
    if (selector.length == 0) {
        alert('页面未找到id未mf_form的form,无法完成saveFormData');
        return;
    }

    var isValid = selector.form('validate');
    if (!isValid) {
        msgBox('请检查所填数据!');
        return;
    }
    var t = selector.serializeArray();
    var formData = {};
    $.each(t, function () {
        formData[this.name] = this.value;
    });
    closeWindow(formData);
}

function setFormData(data, formId) {
    if (!data)
        return;

    formId = formId || 'mf_form';
    if ($("form[id='" + formId + "']").length == 0) {
        alert('页面未找到id未mf_form的form,无法完成setFormData');
        return;
    }
    if (typeof (data) == 'string')
        var data = eval('(' + data + ')');

    for (var key in data) {
        //input
        var ctrl = $('#' + formId + ' input[textboxname=' + key + ']');
        //combobox
        if (ctrl.length == 0)
            ctrl = $('#' + formId + ' select[textboxname=' + key + ']');

        if (ctrl.length != 0) {
            if (ctrl.length > 1) {
                alert(key + '匹配到多个控件');
                ctrl = ctrl[0];
            }
            var classNames = ctrl.attr('class');
            if (classNames) {
                var classNameArr = classNames.split(' ');
                var easyuiClass = $.grep(classNameArr, function (item, index) {
                    return item.indexOf('easyui') != -1;
                })
                if (easyuiClass.length > 0) {
                    easyuiClass = easyuiClass[0];
                    var tmps = easyuiClass.split('-');
                    if (tmps.length == 2) {
                        var easyuiCtrlType = tmps[1];
                        eval("ctrl." + easyuiCtrlType + "('setValue', data[key])");
                    }
                }
                else {
                    ctrl.val(data[key]);
                }
            }
            else if ($('#' + formId + ' input[Id=' + key + ']').length != 0) {
                $('#' + formId + ' input[Id=' + key + ']').val(data[key]);
            }
            else if ($('#' + formId + ' textarea[Id=' + key + ']').length != 0) {
                $('#' + formId + ' input[Id=' + key + ']').val(data[key]);
            }
        }
    }
}
//---------------------------id=mf_form------------------------------end

//---------------------------id=mf_grid------------------------------start
function delRow() {
    var selector = $("table[id='mf_grid']");
    if (selector.length == 0) {
        alert('页面未找到id未mf_grid的table,无法完成delRow');
        return;
    }

    var selections = selector.datagrid('getSelections');
    if (selections.length == 0) {
        return msgBox('请选择要删除的行');
    }

    confirmBox('是否删除?', function (r) {
        if (r) {
            $.each(selections, function (index, item) {
                var index = selector.datagrid('getRowIndex', item);
                selector.datagrid('deleteRow', index);
            })
        }
    })
}

function selectRow() {
    var selector = $("table[id='mf_grid']");
    if (selector.length == 0) {
        alert('页面未找到id未mf_grid的table,无法完成selectRow');
        return;
    }

    var selections = selector.datagrid('getSelections');
    closeWindow(selections);
}
//---------------------------id=mf_grid------------------------------end

//---------------------------layer<-layer------------------------------start
function getlayer() {
    var thisLayer;
    var top = window.top;
    if (top && top.layer) {
        thisLayer = top.layer;
    }
    else if (this.layer) {
        thisLayer = this.layer;
    }
    else {
        msgBox('找不到窗体控件layer对象！');
        return;
    }
    return thisLayer;
}

function msgBox(info, callBack) {
    var thisLayer = getlayer();
    if (callBack) {
        thisLayer.confirm(info, {
            skin: 'layui-layer-lan',
            btn: ['确定'] //按钮
        }, function (index) {
            callBack(true);
            thisLayer.close(index);
        });
    }
    else {
        thisLayer.msg(info);
    }
}

function confirmBox(info, callBack) {
    var thisLayer = getlayer();
    thisLayer.confirm(info, {
        skin: 'layui-layer-lan',
        btn: ['确定', '取消'] //按钮
    }, function (index) {
        callBack(true);
        thisLayer.close(index);
    }, function () {
        callBack(false);
    });
}

function openWindow(url, windowSettings) {
    if (typeof (url) == "undefined") {
        msgBox('当前url不能为空，请检查！');
        return;
    }

    if (windowSettings && typeof (windowSettings) == 'string' && windowSettings.constructor == String) {
        windowSettings = JSON.parse(windowSettings);
    }

    var settings = jQuery.extend(true, {}, windowParamSettings, windowSettings);

    var width = settings.width;
    if (settings.width && settings.width.toString().indexOf('%') != -1) {
        width = settings.width;
    }
    else {
        width = settings.width ? (settings.width.toString() + 'px') : '100px';
    }

    var height = settings.height;
    if (settings.height && settings.height.toString().indexOf('%') != -1) {
        height = settings.height;
    }
    else {
        height = settings.height ? (settings.height.toString() + 'px') : '100px';
    }

    var thisLayer = getlayer();
    thisLayer.open({
        skin: 'layui-layer-lan',
        type: 2,
        title: settings.title,
        maxmin: settings.showMaxButton,
        area: [width, height],
        content: [url],
        scrollbar: true,
        end: function () {
            if (settings.onDestroy) {
                settings.onDestroy(thisLayer.windowResult);
                thisLayer.windowResult = null;//避免重复用
            }
        },
        success: function (layero, index) {
            var obj = $(layero).find('iframe')[0].contentWindow;
            if (obj.setFormData && settings.getDataAction) {
                obj.setFormData(settings.getDataAction());
            }
        }
    });
}

function closeWindow(windowResult) {
    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    parent.layer.windowResult = windowResult;
    parent.layer.close(index); //再执行关闭
}
//---------------------------dialog<-layer------------------------------end

//------------------------extension------
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

//首先我们需要一下工具方法
//为数组添加一些方法
//去重
Array.prototype.unique = function () {
    this.sort();
    var re = [this[0]];
    for (var i = 1; i < this.length; i++) {
        if (this[i] !== re[re.length - 1]) {
            re.push(this[i]);
        }
    }
    return re;
}

//删除
Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
}
//判断元素是否存在
Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
}

//判断元素字段是否存在
Array.prototype.containsField = function (obj, field) {
    var i = this.length;
    while (i--) {
        if (!this[i][field])
            return false;

        if (this[i][field] === obj) {
            return true;
        }
    }
    return false;
}

//对数组或集合的深拷贝
function objDeepCopy(source) {
    var sourceCopy = source instanceof Array ? [] : {};
    for (var item in source) {
        sourceCopy[item] = typeof source[item] === 'object' ? objDeepCopy(source[item])
            : source[item];
    }
    return sourceCopy;
}
//定位
Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) return i;
    }
    return -1;
};
//判断两个对象指定属性的值是否相等(忽略类型)
function isObjectValueEqual(obj1, obj2, arr) {
    for (var i = 0; i < arr.length; i++) {
        var propName = arr[i];
        if (obj1[propName] != obj2[propName]) {
            return false;
        }
    }
    return true;
}