$(function () {

    //固定南北 panel 分隔条
    $('#mainLayout').layout('panel', 'north').panel('panel').resizable('disable');
    $('#mainLayout').layout('panel', 'south').panel('panel').resizable('disable');
    tabClose();
    tabCloseEven();
    InitLeftTree();
    LoadTopTree();
    LoadFirstAccordTree();
    //addTab('首页', '/Main/Home', 'icon icon-home', false);
});

function LoadTopTree() {
    $.post("/api/Main/GetModules",
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
            });
        });
}

function LoadFirstAccordTree() {
    Clearnav();
    $.post("/Main/GetFirstMenus",
        function (data) {
            if (data === "0")
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
                    url: "/api/Main/GetChildrenFunc?id=" + id,
                    onClick: function (node) {
                        var tabTitle = node.text;
                        var url = node.url;
                        var icon = node.iconCls;
                        addTab(tabTitle, url, icon, true);
                    }
                });
            });
        }, "json");
}

function LoadAccordTree(topId) {
    Clearnav();
    $.post("/api/Main/GetChildrenFunc", { Id: topId },
        function (data) {
            if (data === "0")
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
        }, "json");
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



function logOut() {
    $.messager.confirm('确认', '您确认要退出吗？', function (r) {
        if (r) {
            $.post(
                "/Main/Logout",
                function (data) {
                    var info = eval('(' + data + ')');
                    if (info.Success) {
                        window.location.href = "/Login/Index";
                    }
                    else {
                        $.messager.alert('提示', info.Message, 'info');
                    }
                });
        }
    });
}




