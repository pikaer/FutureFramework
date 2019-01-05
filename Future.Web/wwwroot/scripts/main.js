$(function () {

    //固定南北 panel 分隔条
    $('#mainLayout').layout('panel', 'north').panel('panel').resizable('disable');
    $('#mainLayout').layout('panel', 'south').panel('panel').resizable('disable');
    tabClose();
    tabCloseEven();
    // LoadFirsUnReadMessage();
    
    InitLeftTree();
    LoadTopTree();
    LoadFirstAccordTree();
    addTab('首页', '/Main/Home', 'icon icon-home', false);
    //getEmailCount();
    //createRightBotDialog();
    //GetIntervalTime();
    
    // LoadFirsUnReadMessage();
});
function getEmailCount() {   //获取未读消息条数，并显示在红圈中
    $.post("/Sys/LanEmailManage/GettUnReadEmailCount",
       function (data) {
           var info = eval('(' + data + ')');
           if (info == 0) {
               $('#emailCount').html('');
               $('#emailCount').removeClass('unread');
           }
           else {
               if (!$('#emailCount').hasClass('unread'))
               {
                   $('#emailCount').addClass('unread');
               }
               $('#emailCount').html(info);
           }           
       })
}

function LoadTopTree() {
    $.post("/Main/GetModules",
        function (data) {
            var dataObj = data;
            $.each(dataObj, function (i, e) {
                var aHtml = "<a onclick='LoadAccordTree(" + e.id + ")' ";
                aHtml += "class='easyui-linkbutton' ";
                aHtml += "data-options='plain:true,iconCls:\"" + e.iconCls + "\"'>" + e.text + "</a>";
                var tdHtml = "<td>" + aHtml + "</td>";
                var tdSeparator = "<td><div class='datagrid-btn-separator'></div></td>";
                $("#tabTopMenu tr").append(tdHtml + tdSeparator);
                //对于后添加easyui元素需要再次渲染而调用的函数。
                $.parser.parse();
            })
        })
}

function LoadFirstAccordTree() {
    Clearnav();
    $.post("/Main/GetFirstMenus",
         function (data) {
             if (data == "0")
                 window.location = "url";

             $.each(data, function (i, e) {
                 var id = e.id;
                 $("#westAccordion").accordion('add', {
                     title: e.text,
                     content: "<ul id='tree" + id + "'></ul>",
                     selected: true,
                     iconCls: e.iconCls
                 });
                 //$.parser.parse();//再次加载easyUI

                 $("#tree" + id).tree({
                     url: "/Main/GetChildrenFunc?id=" + id,
                     onClick: function (node) {
                         var tabTitle = node.text;
                         var url = node.url;
                         var icon = node.iconCls;
                         addTab(tabTitle, url, icon, true);
                     }
                 });
             });
         }, "json")
}

function LoadAccordTree(topId) {
    Clearnav();
    $.post("/Main/GetChildrenFunc", { id: topId },
         function (data) {
             if (data == "0")
                 window.location = "url";

             $.each(data, function (i, e) {
                 var id = e.id;
                 $("#westAccordion").accordion('add', {
                     title: e.text,
                     content: "<ul id='tree" + id + "'></ul>",
                     selected: true,
                     iconCls: e.iconCls
                 });
                 //$.parser.parse();//再次加载easyUI

                 $("#tree" + id).tree({
                     url: "/Main/GetChildrenFunc?id=" + id,
                     onClick: function (node) {
                         var tabTitle = node.text;
                         var url = node.url;
                         var icon = node.iconCls;
                         addTab(tabTitle, url, icon, true);
                     }
                 });
             });
         }, "json")
}

// 初始化左侧
function InitLeftTree() {

    // 导航菜单绑定初始化
    $("#westAccordion").accordion({
        fillSpace: true,
        fit: true,
        border: false,
        animate: false
    });
    hoverMenuItem();

    //$('#wnav li a').live('click', function () {
    //    var tabTitle = $(this).children('.nav').text();

    //    var url = $(this).attr("rel");
    //    var menuid = $(this).attr("ref");
    //    var icon = getIcon(menuid, icon);

    //    addTab(tabTitle, url, icon);
    //    $('#wnav li div').removeClass("selected");
    //    $(this).parent().addClass("selected");
    //});

}

/**
 * 菜单项鼠标Hover
 */
function hoverMenuItem() {
    $(".easyui-accordion").find('options').hover(function () {
        $(this).parent().addClass("hover");
    }, function () {
        $(this).parent().removeClass("hover");
    });
}

function Clearnav() {
    var pp = $('#westAccordion').accordion('panels');

    $.each(pp, function (i, n) {
        if (n) {
            var t = n.panel('options').title;
            $('#westAccordion').accordion('remove', t);
        }
    });

    pp = $('#westAccordion').accordion('getSelected');
    if (pp) {
        var title = pp.panel('options').title;
        $('#westAccordion').accordion('remove', title);
    }
}

function addTab(subtitle, url, icon, noClose) {
    if (!$('#tabs').tabs('exists', subtitle)) {
        $('#tabs').tabs('add', {
            title: subtitle,
            content: createFrame(url),
            closable: noClose,
            icon: icon || 'icon icon-table'
        });
    } else {
        $('#tabs').tabs('select', subtitle);
        $('#mm-tabupdate').click();
    }
    tabClose();
}

function createFrame(url) {
    //var iframdHeight = parseInt(this.$tabDiv.height());
    //var iframeSrc = opts.src.indexOf('?') >= 0 ? opts.src + "&tabid=" + tabID : opts.src + "?tabid=" + tabID;
    //var $iframe = $('<iframe id="' + tabID + '_iframe" name="' + tabID + '_iframe" frameborder="0" scrolling="auto" style="width:100%;' + (iframdHeight ? "height:" + (iframdHeight - 26).toString() + "px" : "") + '" src="' + iframeSrc + '"></iframe>');

    var s = '<iframe scrolling="auto" frameborder="0" name="tabFrame"  id="tabFrame" src="' + url + '" style="width:100%;height:100%;"></iframe>';
    //s += '@StackExchange.Profiling.MiniProfiler.RenderIncludes()';
    return s;
}

function tabClose() {
    /* 双击关闭TAB选项卡 */
    $(".tabs-inner").dblclick(function () {
        var subtitle = $(this).children(".tabs-closable").text();
        $('#tabs').tabs('close', subtitle);
    });
    /* 为选项卡绑定右键 */
    $(".tabs-inner").bind('contextmenu', function (e) {
        $('#mm').menu('show', {
            left: e.pageX,
            top: e.pageY
        });

        var subtitle = $(this).children(".tabs-closable").text();

        $('#mm').data("currtab", subtitle);
        $('#tabs').tabs('select', subtitle);
        return false;
    });
}
// 绑定右键菜单事件
function tabCloseEven() {
    // 刷新
    $('#mm-tabupdate').click(function () {
        var currTab = $('#tabs').tabs('getSelected');
        var url = $(currTab.panel('options').content).attr('src');
        $('#tabs').tabs('update', {
            tab: currTab,
            options: {
                content: createFrame(url)
            }
        });
    });
    // 关闭当前
    $('#mm-tabclose').click(function () {
        var currtab_title = $('#mm').data("currtab");
        if (currtab_title == '首页') return;
        $('#tabs').tabs('close', currtab_title);
    });
    // 全部关闭
    $('#mm-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            if (t == '首页') return true;
            $('#tabs').tabs('close', t);
        });
    });
    // 关闭除当前之外的TAB
    $('#mm-tabcloseother').click(function () {
        $('#mm-tabcloseright').click();
        $('#mm-tabcloseleft').click();
    });
    // 关闭当前右侧的TAB
    $('#mm-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            // msgShow('系统提示','后边没有啦~~','error');
            //alert('后边没有了');
            return false;
        }
        nextall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            if (t == '首页') return true;
            $('#tabs').tabs('close', t);
        });
        return false;
    });
    // 关闭当前左侧的TAB
    $('#mm-tabcloseleft').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        if (prevall.length == 0) {
            //alert('到头了');
            return false;
        }
        prevall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            if (t == '首页') return true;
            $('#tabs').tabs('close', t);
        });
        return false;
    });

    // 退出
    $("#mm-exit").click(function () {
        $('#mm').menu('hide');
    });
}

function myEmail() {
    parent.ShowDialog(true, '/Sys/LanEmailManage/LanEmailIndex', 410, 775, '我的消息', getEmailCount);
}

function logOut() {
    $.messager.confirm('确认', '您确认要退出吗？', function (r) {
        if (r) {
            $.post(
            "/Main/Logout",
            function (data) {
                var info = eval('(' + data + ')');
                if (info.Success == true) {
                    window.location.href = "/Login/Index";
                }
                else {
                    $.messager.alert('提示', info.Message, 'info');
                }
            })
        }
    });
}

var RefreshCallBack;
var Redirect;
//在home的dialog 可以跳出tab显示
function ShowDialog(urlOrHtml, source, h, w, tit, callBack, redirect) {
    //if (window.frames["ctrl"].location.href != url) {
    if (urlOrHtml) {
        window.frames["dlgFrame"].location.href = source;
        $('#dlgHtml').html('');
    }
    else {
        window.frames["dlgFrame"].document.body.innerText = "";
        $('#dlgHtml').html(source);
    }

    RefreshCallBack = callBack;
    Redirect = redirect;
    $('#openDlg').dialog({ height: h, width: w, title: tit, resizable: true, maximizable: true, minimizable: true });
    $('#openDlg').dialog('open');
    $('#openDlg').window('center');
};
function RefreshAfterDialogConfirm(res) {
    if (RefreshCallBack)
        RefreshCallBack(res);
}

function Redirect(url) {
    if (Redirect)
        Redirect(url);
}
function updatePwd() {
    parent.ShowDialog(true, '/Main/UpdatePwdView', 220, 360, '修改密码');
}

function CloseDialog() {
    $('#openDlg').dialog('close');
}

function Know(id) {
    $.post("/Sys/LanEmailManage/UpdateState", { Id: id },
    function (data) {
        var info = eval('(' + data + ')');
        if (info) {
            getEmailCount();
            window.clearInterval(main_idt);
            $("#rightBotDialog").dialog('close');
        }
        else {
            $.messager.alert('提示', info.Message, 'info');
        }
        LoadFirsUnReadMessage();      //点我知道了后，再去查看是否有未读
    })
}
function Seemore() {
    myEmail();
}
function NoSee() {
    $.post("/HR/StaffManage/SetEmailNoSee",
        function (data) {
            var info = eval('(' + data + ')');
            if (info) {
                $("#rightBotDialog").dialog('close');
                window.clearInterval(main_idt);
            }
            else {
                $.messager.alert('提示', info.Message, 'info');
            }
        });
}

//right bottom dialog
function createRightBotDialog(width, height) {
    var dialog = '<div id="rightBotDialog" class="easyui-dialog"></div>';
    $(document.body).append(dialog);
    var w = width || 250;
    var h = height || 150;
    var topPosition = $(window).height() - h;
    var leftPosition = $(window).width() - w;
    $('#rightBotDialog').dialog({
        width: w,
        height: h,
        top: topPosition,
        left: leftPosition,
        title: '消息提示',
        minimizable: false,
        maximizable: false,
        draggable: false,
        closed: true,
        closable: false,
        onClose: function () {           
            main_rightBotDialogOpenRemainSecond = 30;
        }
    });
}

function main_showRightBotDialog(content, id) {
    var detailA = '';
    detailA += '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" class="easyui-linkbutton" onclick="Know(' + id + ')">知道了</a>';
    detailA += '&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" class="easyui-linkbutton" onclick="Seemore()">更多...</a>';
    detailA += '&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" class="easyui-linkbutton" onclick="NoSee()">不再提醒</a>';

    var cont = content + '<br/><br/>' + detailA;
    $('#rightBotDialog').dialog({ content: cont });
    $("#rightBotDialog").dialog('open');

    main_idt = window.setInterval(function () { main_ls() }, 1000);
}

var main_rightBotDialogOpenRemainSecond = 30;
var main_idt;
function main_ls() {
    if (main_rightBotDialogOpenRemainSecond == 0) {
        $("#rightBotDialog").dialog('close');
        window.clearInterval(main_idt);
        setTimeout("LoadFirsUnReadMessage()", secondsDelayOfShowDialog);
    }
    else {
        $('#rightBotDialog').dialog({ closed: false, title: '消息提示(' + main_rightBotDialogOpenRemainSecond + 's后自动关闭)' });
    }
    main_rightBotDialogOpenRemainSecond--;
}

function LoadFirsUnReadMessage() {
    $.post(
            "/LanEmailManage/GetLatestUnReadEmail",
            function (data) {
                if (data != '') {
                    var info = eval('(' + data + ')');
                    main_showRightBotDialog(info.EmailContent, info.Id);
                }
            })
}

var secondsDelayOfShowDialog;
function GetIntervalTime() {
    $.post("/HR/StaffManage/GetIntervalTime",
    function (data) {
        var info = eval('(' + data + ')');
        if (info.TimeInterval != 0) {   //时间间隔为默认或者为-1的时候不提醒          
            LoadFirsUnReadMessage();
            secondsDelayOfShowDialog = info.TimeInterval;
        }
    })
}

