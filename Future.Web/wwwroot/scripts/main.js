$(function () {

    LoadTopTree();
    //tabClose();
    //InitLeftTree();
    //LoadFirstAccordTree();
});

//加载一级菜单
function LoadTopTree() {
    Clearnav();
    $.post("/Main/GetModules",
        function (dataObj) {
            if (dataObj.success) {
                $.each(dataObj.content, function (i, e) {
                    var aHtml = "<a onclick='LoadAccordTree(" + e.id + ")' ";
                    aHtml += "class='easyui-linkbutton' ";
                    aHtml += "data-options='plain:true,iconCls:\"" + e.iconCls + "\"'>" + e.text + "</a>";
                    var tdHtml = "<td>" + aHtml + "</td>";
                    var tdSeparator = "<td><div class='datagrid-btn-separator'></div></td>";
                    $("#tabTopMenu tr").append(tdHtml + tdSeparator);
                    //对于后添加easyui元素需要再次渲染而调用的函数。
                    $.parser.parse();
                });
            }
        });
}

function LoadAccordTree(topId) {
    Clearnav();
    var queryData = {
        Id: topId
    };
    $.post("/Main/GetChildrenFunc", { data: $.toJSON(queryData) },
        function (dataObj) {
            if (dataObj.success) {
                $.each(dataObj.content, function (i, e) {
                    var id = e.id;
                    $("#westAccordion").accordion('add', {
                        title: e.text,
                        content: "<ul id='tree" + id + "'></ul>",
                        selected: true,
                        iconCls: e.iconCls
                    });

                    $("#tree" + id).tree({
                        url: "/Main/ChildrenFunc?data=" + id,
                        onClick: function (node) {
                            var tabTitle = node.text;
                            var url = node.url;
                            var icon = node.iconCls;
                            addTab(tabTitle, url, icon, true);
                        }
                    });
                });
            }
        }, "json");
}

function LoadFirstAccordTree() {
    Clearnav();
    $.post("/Main/GetFirstMenus",
        function (dataObj) {
            if (dataObj.success) {
                $.each(dataObj.content, function (i, e) {
                    var id = e.id;
                    $("#westAccordion").accordion('add', {
                        title: e.text,
                        content: "<ul id='tree" + id + "'></ul>",
                        selected: true,
                        iconCls: e.iconCls
                    });
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
            }
        }, "json");
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
    var s = '<iframe scrolling="auto" frameborder="0" name="tabFrame"  id="tabFrame" src="' + url + '" style="width:100%;height:100%;"></iframe>';
    return s;
}

function tabClose() {
    /* 双击关闭TAB选项卡 */
    $(".tabs-inner").dblclick(function () {
        var subtitle = $(this).children(".tabs-closable").text();
        $('#tabs').tabs('close', subtitle);
    });
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
    
    //删除2次
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

