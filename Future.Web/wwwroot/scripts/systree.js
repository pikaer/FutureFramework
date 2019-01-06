$(function () {
    InitGrid();
});

function InitGrid() {
    $("#dg").treegrid({
        title: "权限列表",
        rownumbers: true,
        fit: true,
        singleSelect: true,
        nowrap: true,
        remoteSort: false,//前台排序 true则为后台排序 需要传参
        idField: 'funcId',
        animate: false,  //定义在节点展开或折叠的时候是否显示动画效果。
        treeField: 'text',
        striped: true,
        border: false,
        iconCls: 'icon icon-tree',
        url: '/SysTree/GetFuncTreeJson',//获取列表数据
        method: 'post',
        autoRowHeight: false,
        lines: true,
        columns: [[
            { field: 'text', width: 200, title: "名称", halign: 'center' },
            { field: 'url', width: 250, title: "链接", halign: 'center' },
            { field: 'iconCls', width: 170, title: "图标", halign: 'center' },
            { field: 'funcType', width: 180, title: "类型", halign: 'center' }
        ]],
        toolbar: "#tb",
        onLoadSuccess: function () {
           $('#dg').treegrid('collapseAll');
        }
    });
}

//删除
function removeit() {
    //获取当前选中行
    var row = $('#dg').treegrid('getSelected');
    //如果不为空
    if (row) {
        confirmBox('是否删除?', function (r) {
            if (r) {
                addAjaxParam('id', row.Id);
                commitAjax('DeleteFunc', {
                    CallBack: function (data) {
                        if (data) {
                            msgBox(data);
                            //成功后刷新界面
                            $("#dg").treegrid('reload');
                        }
                    }
                });
            }
        });
    }
}

//增加同级
function addEq() {
    var row = $('#dg').treegrid('getSelected');
    if (row) {
        addAjaxParam('data', row);
        commitAjax('AddEqFunc', {
            CallBack: function (data) {
                if (data) {
                    msgBox(data);
                    //成功后刷新界面
                    $("#dg").treegrid('reload');
                }
            }
        });
    }
}

//增加子级
function addSub() {
    var row = $('#dg').treegrid('getSelected');
    if (row) {
        addAjaxParam('data', row);
        commitAjax('AddSubFunc', {
            CallBack: function (data) {
                if (data) {
                    msgBox(data);
                    //成功后刷新界面
                    $("#dg").treegrid('reload');
                }
            }
        });
    }
}

function edit() {
    var row = $('#dg').treegrid('getSelected');
    if (row) {
        openWindow('/SysTree/Item?id=' + row.Id, {
            width: 500, height: 280, onDestroy: function (data) {
                if (data)
                    $("#dg").treegrid('update', { id: row.Id, row: data });
            }
        });
    }
}

function move(o) {
    var n = $("#dg").treegrid("getSelected"); if (n == null) { msgBox("无法移动!"); return; };
    var selectRow = $('#datagrid-row-r1-2-' + n.Id);
    if (o == 'up') {
        var pre = selectRow.prev();
        if (typeof (pre.attr("node-id")) == "undefined") {
            pre = pre.prev();
        }

        if (typeof (pre.attr("node-id")) == "undefined" /*|| pre.attr("node-id").indexOf("L") == 0*/) {
            msgBox("无法移动!");
        } else {
            var preId = parseInt(pre.attr("node-id"));
            addAjaxParam('aId', n.Id);
            addAjaxParam('bId', preId);
            commitAjax('ExChangeOrder', {
                CallBack: function (data) {
                    if (data) {
                        var n2 = $("#dg").treegrid("pop", n.Id);
                        $("#dg").treegrid("insert", { before: preId, data: n2 });
                        $("#dg").treegrid("select", n.Id);
                    }
                }
            });
        }
    }
    if (o == 'down') {
        var next = selectRow.next();
        if (typeof (next.attr("node-id")) == "undefined") {
            next = next.next();
        }
        if (typeof (next.attr("node-id")) == "undefined" /*|| pre.attr("node-id").indexOf("L") == 0*/) {
            alert("无法移动!");
        } else {
            var nextId = parseInt(next.attr("node-id"));
            addAjaxParam('aId', n.Id);
            addAjaxParam('bId', nextId);
            commitAjax('ExChangeOrder', {
                CallBack: function (data) {
                    if (data) {
                        var n2 = $("#dg").treegrid("pop", nextId);
                        $("#dg").treegrid("insert", { before: n.Id, data: n2 });
                        $("#dg").treegrid("select", n.Id);
                    }
                }
            });
        }
    }
}