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


//编辑 start
//当前正在编辑的行
var editRow;
//行进入编辑状态，所有单元格都显示为相应的editor
function edit() {
    //如果进入edit函数前有行仍然处于编辑状态则必须完成相应的编辑操作，
    //因此选中该行，函数退出
    if (editRow != undefined) {
        $('#dg').treegrid('select', editRow.id);
        return;
    }
    //得到当前选中的行，并赋予editRow，调用beginEdit，使得行进入编辑状态
    var row = $('#dg').treegrid('getSelected');
    if (row) {
        editRow = row;
        $('#dg').treegrid('beginEdit', editRow.id);
    }
}

//将编辑结果保存至数据库中
//进入编辑状态后调用有效
function save() {
    //如果在没有编辑行(进入编辑状态)的情况下，则不必调用此函数因此退出
    if (editRow != undefined) {
        //获取当前正编辑的行对象
        var row = editRow;
        var rowId = editRow.id;

        //获取每列对应的editor对象
        var textEditor = $('#dg').treegrid('getEditor', { index: rowId, field: 'text' });
        var attributesEditor = $('#dg').treegrid('getEditor', { index: rowId, field: 'attributes' });
        var iconClsEditor = $('#dg').treegrid('getEditor', { index: rowId, field: 'iconCls' });
        //var enumFuncTypeEditor = $('#dg').treegrid('getEditor', { index: rowIndex, field: 'enumFuncType' });

        //通过editor对象获取编辑后的结果
        row.text = textEditor.target.val();
        row.attributes = attributesEditor.target.val();
        row.iconCls = iconClsEditor.target.val();
        //row.enumFuncType = enumFuncTypeEditor.target.combobox('getValue');

        //ajax调用控制器action进行数据更新
        $.post("/SysTree/UpdateAuthor",
            { data: $.toJSON(row) },
            function (data) {
                var info = eval('(' + data + ')');
                if (info.Success) {
                    //成功后调用reload刷新界面数据
                    $('#dg').treegrid('reload');
                    //reload之后行会退出编辑状态，因此当前的编辑行要重新设置为undefined
                    editRow = undefined;
                }
                else {
                    $.messager.alert('提示', info.Message, 'info');
                }
            })
    }
}

//退出行编辑状态
function cancel() {
    if (editRow != undefined) {
        $('#dg').treegrid('cancelEdit', editRow.id);
        editRow = undefined;
    }
}
//编辑 end

//删除
function removeit() {
    //获取当前选中行
    var row = $('#dg').treegrid('getSelected');
    //如果不为空
    if (row) {
        $.messager.confirm('提示', '是否删除?', function (r) {
            if (r) {
                //ajax调用控制器action更新数据库
                $.post("/SysTree/DeleteAuthor",
                    { id: row.id },
                    function (dataStr) {
                        var data = eval('(' + dataStr + ')');
                        if (data.Success) {
                            //成功后刷新界面
                            $("#dg").treegrid('reload');
                        }
                        else {
                            $.messager.alert("提示", data.Message);
                        }
                    })
            }
        })
    }
}

//增加同级
function addEq() {
    var row = $('#dg').treegrid('getSelected');
    if (row) {
        $.post("/SysTree/AddEqAuthor",
            { data: $.toJSON(row) },
            function (dataStr) {
                var data = eval('(' + dataStr + ')');
                if (data.Success) {
                    $("#dg").treegrid('reload');
                }
                else {
                    $.messager.alert("提示", data.Message);
                }
            })
    }
}

//增加子级
function addSub() {
    var row = $('#dg').treegrid('getSelected');
    if (row) {
        $.post("/SysTree/AddSubAuthor",
            { data: $.toJSON(row) },
            function (dataStr) {
                var data = eval('(' + dataStr + ')');
                if (data.Success) {
                    $("#dg").treegrid('reload');
                }
                else {
                    $.messager.alert(data.Message);
                }
            })
    }
}

function move(o) {
    var n = $("#dg").treegrid("getSelected"); if (n == null) { alert("无法移动!"); return; };
    var selectRow = $('#datagrid-row-r1-2-' + n.id);
    if (o == 'up') {
        var pre = selectRow.prev();
        if (typeof (pre.attr("node-id")) == "undefined") {
            pre = pre.prev();
        }

        if (typeof (pre.attr("node-id")) == "undefined" /*|| pre.attr("node-id").indexOf("L") == 0*/) {
            alert("无法移动!");
        } else {
            var preId = parseInt(pre.attr("node-id"));
            $.post("/SysTree/ExChangeOrder",
                { aId: n.id, aOrder: n.order, bId: preId },
                function (data) {
                    if (data) {
                        var n2 = $("#dg").treegrid("pop", n.id);
                        $("#dg").treegrid("insert", { before: preId, data: n2 });
                        $("#dg").treegrid("select", n.id);
                    } else {
                        alert("移动过程出现异常,请稍后再试");
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
            $.post("/SysTree/ExChangeOrder",
                { aId: n.id, aOrder: n.order, bId: nextId },
                function (data) {
                    if (data) {
                        var n2 = $("#dg").treegrid("pop", nextId);
                        $("#dg").treegrid("insert", { before: n.id, data: n2 });
                        $("#dg").treegrid("select", n.id);
                    } else {
                        alert("移动过程出现异常,请稍后再试");
                    }
                });
        }
    }
}

function up() {
    var selectedRow = $("#dg").datagrid('getSelected');
    if (!selectedRow) {
        $.messager.alert('提示', '请选择要升序的行!', 'info');
        return;
    }
    var pre = selectedRow.prev();
    var selectedRowIndex = $("#dg").datagrid('getRowIndex', selectedRow);
    var rows = $("#dg").datagrid('getRows');
    if (selectedRowIndex != 0) {
        var rowToExchange = rows[selectedRowIndex - 1];
        $.post("/SysTree/ExChangeOrder",
            { aId: selectedRow.Id, bId: rowToExchange.Id },
            function (info) {
                var data = eval('(' + info + ')');
                if (data.Success) {
                    $("#dg").datagrid('reload');
                }
            })
    }
}

function down() {
    var selectedRow = $('#dg').datagrid('getSelected');
    if (!selectedRow) {
        $.messager.alert('提示', '请选择要降序的行!', 'info');
        return;
    }

    var selectedRowIndex = $('#dg').datagrid('getRowIndex', selectRow);
    var rows = $('#dg').datagrid('getRows');
    if (selectedRowIndex != rows.length - 1) {
        var rowToExchange = rows[selectedRowIndex + 1];
        $.post("/SysTree/ExChangeOrder",
            { aId: selectedRow.Id, bId: rowToExchange.Id },
            function (info) {
                var data = eval('(' + info + ')');
                if (data.Success) {
                    $('#dg').datagrid('reload');
                }
            })
    }
}