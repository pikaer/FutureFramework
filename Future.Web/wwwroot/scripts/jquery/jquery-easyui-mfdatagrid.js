function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}

function guid() {
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}

(function ($) {
    var autoGrids = [];
    function checkAutoGrid() {
        autoGrids = $.grep(autoGrids, function (t) {
            return t.length && t.data('mfdatagrid');
        });
    }
    function saveAutoGrid(omit) {
        //checkAutoGrid();
        //$.map(autoGrids, function (t) {
        //    if (t[0] != $(omit)[0]) {
        //        t.mfdatagrid('saveRow');
        //    }
        //});
        //checkAutoGrid();
    }
    function addAutoGrid(dg) {
        checkAutoGrid();
        for (var i = 0; i < autoGrids.length; i++) {
            if ($(autoGrids[i])[0] == $(dg)[0]) { return; }
        }
        autoGrids.push($(dg));
    }
    function delAutoGrid(dg) {
        checkAutoGrid();
        autoGrids = $.grep(autoGrids, function (t) {
            return $(t)[0] != $(dg)[0];
        });
    }

    $(function () {
        $(document).unbind('.mfdatagrid').bind('mousedown.mfdatagrid', function (e) {
            var p = $(e.target).closest('div.datagrid-view,div.combo-panel,div.window,div.window-mask');
            if (p.length) {
                if (p.hasClass('datagrid-view')) {
                    saveAutoGrid(p.children('table'));
                }
                return;
            }
            saveAutoGrid();
        });
    });

    function buildGrid(target) {
        var opts = $.data(target, 'mfdatagrid').options;        

        $(target).datagrid($.extend({}, opts, {
            //sll add
            onLoadSuccess: function (data) {
                $.each(data.rows, function (index, item) {
                    delete item['state'];
                    if (opts.idField == 'mfid' && !item['mfid'])
                    {
                        item['mfid'] = guid();
                    }
                })
                opts.oldDatas = jQuery.extend(true, [], data.rows);//克隆旧数据

                if (opts.onLoadSuccess) {
                    opts.onLoadSuccess.call(target, data);
                }
            },
            onDblClickCell: function (index, field, value) {
                if (opts.editing) {
                    $(this).mfdatagrid('editRow', index);
                    focusEditor(target, field);
                }
                if (opts.onDblClickCell) {
                    opts.onDblClickCell.call(target, index, field, value);
                }
            },
            onClickCell: function (index, field, value) {
                if (opts.editing && opts.editIndex >= 0) {
                    $(this).mfdatagrid('editRow', index);
                    focusEditor(target, field);
                }
                ////sll add
                //else if (opts.editing && opts.btnField.indexOf(field) != -1)
                //{
                //    $(this).mfdatagrid('editRow', index);
                //    focusEditor(target, field);
                //    return;
                //}

                if (opts.onClickCell) {
                    opts.onClickCell.call(target, index, field, value);
                }
            },
            onAfterEdit: function (index, row) {
                delAutoGrid(this);

                //updatedrows
                if (opts.oldDatas) {
                    var tmpIndex = index - ($(this).datagrid('getRows').length - opts.oldDatas.length);
                    //判断元数据是否被修改
                    if (tmpIndex >= 0) {
                        var oldData = opts.oldDatas[tmpIndex];
                        var isEqual = true;
                        for (var key in row) {
                            if (key == 'state')
                                continue;

                            if (!oldData[key] || row[key] != oldData[key]) {
                                isEqual = false;
                                var inserts = $(this).datagrid('getChanges', "inserted");
                                if (!opts.updateRows.contains(row) && !inserts.contains(row)) {
                                    opts.updateRows.push(row)
                                }
                                break;
                            }
                        }

                        if (isEqual) {
                            opts.updateRows.remove(row);
                        }
                    }
                }

                if (opts.onAfterEdit) opts.onAfterEdit.call(target, index, row);
            },
            onCancelEdit: function (index, row) {
                delAutoGrid(this);
                opts.editIndex = undefined;
                var idValue = row[opts.idField || 'mfid'];
                var sameObjs = $.grep(opts.oldDatas, function (item, index) { return item[opts.idField || 'mfid'] == idValue; });

                if (sameObjs.length == 1) {
                    $(this).datagrid('updateRow', { index: index, row: sameObjs[0] });
                }

                if (opts.onCancelEdit) opts.onCancelEdit.call(target, index, row);
            },
            onBeforeLoad: function (param) {
                if (opts.onBeforeLoad.call(target, param) == false) { return false }
               // $(this).mfdatagrid('rejectAllChange');
                if (opts.tree) {
                    var node = $(opts.tree).tree('getSelected');
                    param[opts.treeParentField] = node ? node.id : undefined;
                }
            }
        }));

        $(window).keyup(function (e) {
            if (e.keyCode == 27) {//此处代表按的是键盘的Esc键
                if (opts.editIndex >= 0) {
                    $(target).datagrid('cancelEdit', opts.editIndex);
                }
            }
            else if (e.keyCode == 46) {//此处代表按的是键盘的Esc键
                $(target).mfdatagrid('removeRow', opts.editIndex);
            }
        });
        var fields = $(target).datagrid('getColumnFields');
        $.each(fields, function (index, item) {
            var colOpt = $(target).datagrid('getColumnOption',item);
            if(colOpt.editor)
            {
                colOpt.styler = _styler;
            }
        })

        function _styler(value, row, index, field) {
            var opts = $(target).mfdatagrid('options')
            var idfield = opts.idfield || 'mfid';
            var olddata = opts.oldDatas;
            for (var i = 0; i < olddata.length; i++)
            {
                if (olddata[i][idfield] && olddata[i][idfield] == row[idfield])
                {
                    if (olddata[i][field] != value)
                    {
                        return "background:url('/UIFrame/dirty.gif') right 0px no-repeat;";
                    }
                }
            }
            return '';
        }

        if (opts.tree) {
            $(opts.tree).tree({
                url: opts.treeUrl,
                onClick: function (node) {
                    $(target).datagrid('load');
                },
                onDrop: function (dest, source, point) {
                    var targetId = $(this).tree('getNode', dest).id;
                    $.ajax({
                        url: opts.treeDndUrl,
                        type: 'post',
                        data: {
                            id: source.id,
                            targetId: targetId,
                            point: point
                        },
                        dataType: 'json',
                        success: function () {
                            $(target).datagrid('load');
                        }
                    });
                }
            });
        }
    }

    function focusEditor(target, field) {
        var opts = $(target).mfdatagrid('options');
        var t;
        var editor = $(target).datagrid('getEditor', { index: opts.editIndex, field: field });
        if (editor) {
            t = editor.target;
        } else {
            var editors = $(target).datagrid('getEditors', opts.editIndex);
            if (editors.length) {
                t = editors[0].target;
            }
        }
        if (t) {
            if ($(t).hasClass('textbox-f')) {
                $(t).textbox('textbox').focus();
            } else {
                $(t).focus();
            }
        }
    }

    $.fn.mfdatagrid = function (options, param) {
        if (typeof options == 'string') {
            var method = $.fn.mfdatagrid.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.datagrid(options, param);
            }
        }       

        options = options || {};
        return this.each(function () {
            var state = $.data(this, 'mfdatagrid');
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, 'mfdatagrid', {
                    options: $.extend({}, $.fn.mfdatagrid.defaults, $.fn.mfdatagrid.parseOptions(this), options)
                });
            }
            buildGrid(this);
        });
    };

    $.fn.mfdatagrid.parseOptions = function (target) {
        return $.extend({}, $.fn.datagrid.parseOptions(target), {
        });
    };

    $.fn.mfdatagrid.methods = {
        options: function (jq) {
            var opts = $.data(jq[0], 'mfdatagrid').options;
            return opts;
        },
        loadData: function (jq, data) {
            return jq.each(function () {
                $(this).mfdatagrid('rejectAllChange');
                $(this).datagrid('loadData', data);
            });
        },
        enableEditing: function (jq) {
            return jq.each(function () {
                var opts = $.data(this, 'mfdatagrid').options;
                opts.editing = true;
            });
        },
        disableEditing: function (jq) {
            return jq.each(function () {
                var opts = $.data(this, 'mfdatagrid').options;
                opts.editing = false;
            });
        },
        isEditing: function (jq, index) {
            var opts = $.data(jq[0], 'mfdatagrid').options;
            var tr = opts.finder.getTr(jq[0], index);
            return tr.length && tr.hasClass('datagrid-row-editing');
        },
        endEditing: function (jq) {
            return jq.each(function () {
                var dg = $(this);
                var opts = $.data(this, 'mfdatagrid').options;

                if (opts.editIndex == undefined) {
                    return true;
                } else if (dg.datagrid("validateRow", opts.editIndex)) {
                    dg.datagrid("endEdit", opts.editIndex);
                    opts.editIndex = undefined;
                    return true;
                } else {
                    return false;
                }
            });
        },
        editRow: function (jq, index) {
            return jq.each(function () {
                var dg = $(this);
                var opts = $.data(this, 'mfdatagrid').options;
                var editIndex = opts.editIndex;
                if (editIndex != index) {
                    if (dg.datagrid('validateRow', editIndex)) {
                        if (editIndex >= 0) {
                            if (opts.onBeforeSave.call(this, editIndex) == false) {
                                setTimeout(function () {
                                    dg.datagrid('selectRow', editIndex);
                                }, 0);
                                return;
                            }
                        }
                        dg.datagrid('endEdit', editIndex);
                        dg.datagrid('beginEdit', index);
                        if (!dg.mfdatagrid('isEditing', index)) {
                            return;
                        }
                        opts.editIndex = index;
                        focusEditor(this);

                        var rows = dg.datagrid('getRows');
                        opts.onEdit.call(this, index, rows[index]);
                    } else {
                        setTimeout(function () {
                            dg.datagrid('selectRow', editIndex);
                        }, 0);
                    }
                }
            });
        },
        addRow: function (jq, index) {
            return jq.each(function () {
                var dg = $(this);
                var opts = $.data(this, 'mfdatagrid').options;
                if (dg.mfdatagrid('endEditing')) {
                    var rows = dg.datagrid('getRows');

                    function _add(index, row) {
                        if (index == undefined) {
                            if (opts.idField == 'mfid' && !row['mfid'])
                                row['mfid'] = guid();

                            dg.datagrid('appendRow', row);
                            opts.editIndex = rows.length - 1;
                        } else {
                            if (opts.idField == 'mfid' && !row['mfid'])
                                row['mfid'] = guid();

                            dg.datagrid('insertRow', { index: index, row: row });
                            opts.editIndex = index;
                        }
                    }
                    if (typeof index == 'object') {
                        _add(index.index, $.extend(index.row, {}))
                    } else {
                        _add(index, {});
                    }

                    dg.datagrid('beginEdit', opts.editIndex);
                    dg.datagrid('selectRow', opts.editIndex);

                    if (opts.tree) {
                        var node = $(opts.tree).tree('getSelected');
                        rows[opts.editIndex][opts.treeParentField] = (node ? node.id : 0);
                    }

                    opts.onAdd.call(this, opts.editIndex, rows[opts.editIndex]);
                }
            });
        },

        saveRows: function (jq) {
            return jq.each(function () {
                var dg = $(this);
                var opts = $.data(this, 'mfdatagrid').options;

                if (dg.mfdatagrid('endEditing')) {
                    var inserted = dg.datagrid('getChanges', "inserted");
                    var deleted = dg.datagrid('getChanges', "deleted");
                    var updated = opts.updateRows;

                    var rows = dg.datagrid('getData').rows;
                    $.each(rows, function (index, item) {
                        if(inserted.contains(item))
                        {
                            item['state'] = 'insert';
                        }
                        else if(updated.contains(item))
                        {
                            item['state'] = 'update';
                        }
                        else if(deleted.contains(item))
                        {
                            item['state'] = 'delete';
                        }
                        delete item['mfid'];
                    });                    

                    var url = opts.saveUrl;
                    if (url) {
                        var changed = (inserted.length != 0 || deleted.length != 0 || updated.length != 0);
                        if (changed) {
                            $.post(url, { rows: JSON.stringify(rows) }, function (data) {
                                if (!data) {
                                    opts.onError.call(this);
                                    return;
                                }

                                if (opts.tree) {
                                    var idValue = row[opts.idField || 'mfid'];
                                    var t = $(opts.tree);
                                    var node = t.tree('find', idValue);
                                    if (node) {
                                        node.text = row[opts.treeTextField];
                                        t.tree('update', node);
                                    } else {
                                        var pnode = t.tree('find', row[opts.treeParentField]);
                                        t.tree('append', {
                                            parent: (pnode ? pnode.target : null),
                                            data: [{ id: idValue, text: row[opts.treeTextField] }]
                                        });
                                    }
                                }
                                                                
                                dg.datagrid('reload');
                                opts.oldDatas = jQuery.extend(true, [], dg.datagrid('getData').rows);//克隆旧数据 要不然修改标记不会被消除

                                opts.onSuccess.call(this);
                                opts.onSave.call(this);
                            }, 'json');
                        } else {
                            opts.onSave.call(this);
                        }
                    } else {
                        opts.onSave.call(this);
                    }

                }
            });
        },
        rejectAllChange: function (jq) {
            return jq.each(function () {
                var dg = $(this);
                var opts = $.data(this, 'mfdatagrid').options;

                dg.datagrid('rejectChanges');
                opts.updateRows = [];
                opts.editIndex = undefined;
            });
        },
        changeRowData:function(jq, data){
            return jq.each(function () {
                var dg = $(this);
                var opts = $.data(this, 'mfdatagrid').options;

                dg.datagrid('updateRow', data);
                var row = dg.datagrid('getRows')[data.index];
                var inserts = dg.datagrid('getChanges', "inserted");
                if (!opts.updateRows.contains(row) && !inserts.contains(row)) {                   
                    opts.updateRows.push(row)
                }
            });
        },
        removeRow: function (jq, index) {
            return jq.each(function () {
                var dg = $(this);
                var opts = $.data(this, 'mfdatagrid').options;

                if (dg.mfdatagrid('endEditing')) {
                    var rows = [];
                    if (index == undefined) {
                        rows = dg.datagrid('getSelections');
                    } else {
                        var rowIndexes = $.isArray(index) ? index : [index];
                        for (var i = 0; i < rowIndexes.length; i++) {
                            var row = opts.finder.getRow(this, rowIndexes[i]);
                            if (row) {
                                rows.push(row);
                            }
                        }
                    }

                    var rowarr = dg.datagrid('getData').rows;
                    for (var i = 0; i < rows.length; i++) {

                        var Index = rowarr.indexOf(rows[0]) - (dg.datagrid('getRows').length - opts.oldDatas.length);
                        //判断元数据是否有数据被删除?
                        if (Index >= 0) {
                            //删除元数据中被删除的数据
                            opts.oldDatas.splice(Index, 1);
                            //删除更新数组中被删除的数据 因为既然数据已经被删除我们就没必要在将该数据拿到后台进行更新操作了 直接删除就可以了
                            opts.updateRows.remove(rows[0])
                        }
                        //删除指定行
                        dg.datagrid('deleteRow', rowarr.indexOf(rows[0]));
                        rowarr = dg.datagrid('getData').rows;
                    }
                    dg.datagrid('clearSelections');
                }
            });
        }
    };

    $.fn.mfdatagrid.defaults = $.extend({}, $.fn.datagrid.defaults, {
        idField: 'mfid',
        singleSelect: true,
        editing: true,
        editIndex: undefined,
        //		destroyConfirmTitle: 'Confirm',
        //		destroyConfirmMsg: 'Are you sure you want to delete?',

        url: null,	// return the datagrid data
        saveUrl: null,	// return the added row

        tree: null,		// the tree selector
        treeUrl: null,	// return tree data
        treeDndUrl: null,	// to process the drag and drop operation, return {success:true}
        treeTextField: 'name',
        treeParentField: 'parentId',

        oldDatas: [],//sll add
        updateRows: [],//sll add

        onAdd: function (index, row) { },
        onEdit: function (index, row) { },
        onBeforeSave: function (index) { },
        onSave: function () { },
        onSuccess: function () { },
        onError: function () { }
    });

    ////////////////////////////////
    $.parser.plugins.push('mfdatagrid');
})(jQuery);