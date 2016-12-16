$(function () {
    //初始化CheckBox
    $(".icheck_box").iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red',
        increaseArea: '20%' // optional
    });
    //ios7 复选框
    var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
    elems.forEach(function (html) {
        var switchery = new Switchery(html, { color: 'rgb(237, 85, 101)' });
    });
    //全选 反选
    $('input[name="checkall"]').on('ifChecked', function (event) {
        $("input[name='checkbox_name']").iCheck('check');
    });
    $('input[name="checkall"]').on('ifUnchecked', function (event) {
        $("input[name='checkbox_name']").iCheck('uncheck');
    });
    //下拉菜单
    $('.select2').select2();
    //工具提示
    $("[data-toggle='tooltip']").tooltip();
    //使用col插件实现表格头宽度拖拽
    $(".table").colResizable();
    //列表选择删除
    $('#delete').click(function () {
        var vals = '';
        $('input[name="checkbox_name"]:checked').each(function () {
            vals += $(this).val() + ',';
        });
        if (vals == '' || vals == ',') {
            dig.error("对不起，请选中您要操作的记录！");
            return;
        }
        var url = window.location.href.split('?')[0].toLowerCase();
        if (url.lastIndexOf('/index') > 0) {
            url = url.substring(0, url.indexOf('/index'));
        }
        url = url + '/Delete';
        var msg = "删除记录后不可恢复，您确定吗？";
        dig.confirm("删除确认", msg, function () {
            $.post(url, { idList: vals }, function (res) {
                if (res.Status == "y") {
                    dig.successcallback('删除成功', function () {
                        window.location.reload();
                    });
                }
                else {
                    dig.error(res.Msg);
                }
            }, "json");
        });
    });
    //刷新按钮
    $(".reload-link").mouseover(function () {
        $(this).find("i").addClass("fa-spin");
    }).mouseout(function () { $(this).find("i").removeClass("fa-spin"); });
    //返回
    $("#btn-dig-close").click(function () {
        dig.close();
    });
    //点击表格，选中/取消 复选框
    $("#dataTable tr").slice(1).each(function (g) {
        var p = $(this);
        $(this).children().slice(1).click(function () {
            if ($($(p.children()[0]).children()[0]).children().eq(0).prop("checked")) {
                $($(p.children()[0]).children()[0]).iCheck("uncheck");
            }
            else {
                $($(p.children()[0]).children()[0]).iCheck("check");
            }
        });
    });
    $("#emailTable tr").slice(0).each(function (g) {
        var p = $(this);
        $(this).children().slice(1).click(function () {
            if ($($(p.children()[0]).children()[0]).children().eq(0).prop("checked")) {
                $($(p.children()[0]).children()[0]).iCheck("uncheck");
            }
            else {
                $($(p.children()[0]).children()[0]).iCheck("check");
            }
        });
    });
    //时间
    laydate.skin('molv');
});