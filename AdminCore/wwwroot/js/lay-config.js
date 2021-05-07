/**
 * date:2021/04/17
 * author:Mr.Chung --> nixue
 * description:此处放layui自定义扩展
 * version:2.2.4
 */

window.rootPath = (function (src) {
    src = document.scripts[document.scripts.length - 1].src;
    return src.substring(0, src.lastIndexOf("/") + 1);
})();

layui.link("../../css/nixue.css");

layui.config({
    base: rootPath + "lay-module/",
    version: true
}).extend({
    miniAdmin: "layuimini/miniAdmin", // layuimini后台扩展
    miniMenu: "layuimini/miniMenu", // layuimini菜单扩展
    miniTab: "layuimini/miniTab", // layuimini tab扩展
    miniTheme: "layuimini/miniTheme", // layuimini 主题扩展
    miniTongji: "layuimini/miniTongji", // layuimini 统计扩展
    step: 'step-lay/step', // 分步表单扩展
    treetable: 'treetable-lay/treetable', //table树形扩展
    tableSelect: 'tableSelect/tableSelect', // table选择扩展
    iconPickerFa: 'iconPicker/iconPickerFa', // fa图标选择扩展
    echarts: 'echarts/echarts', // echarts图表扩展
    echartsTheme: 'echarts/echartsTheme', // echarts图表主题扩展
    wangEditor: 'wangEditor/wangEditor', // wangEditor富文本扩展
    layarea: 'layarea/layarea', //  省市县区三级联动下拉选择器
    nixue: 'nixue', // 自定义协议
});

/**注册支持当前结果的路由规则 */
layui.AddRoute = function () {
    if (parent.layui.BaseNx && parent.layui.BaseNx.IsAdmin) {
        layui.BaseNx = parent.layui.nixue;
        layui.BaseNx.IsAdmin = true;
        layui.BaseNx.AdminLayers += (parent.layui.BaseNx.AdminLayers + 1);
        if (layui.nixue) {
            layui.nixue.BaseNixue = layui.BaseNx;
        }
        return true;
    } else if (parent.layui.nixue && parent.layui.nixue.IsAdmin) {
        layui.BaseNx = parent.layui.nixue;
        return true;
    } else {
        layer.alert("非法访问！", { yes: close, end: close });
        function close() { location = location.origin; }
        return false;
    }
}

