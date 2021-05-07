layui.define(["jquery", 'form', "element"], function (exports) {
    var $ = layui.$,
        layer = layui.layer,
        form = layui.form,
        element = layui.element;

    var dateRange = {
        currentDate: new Date(),
        millisecond: 1000 * 60 * 60 * 24,//一天的毫秒数
        gettoday: function (array) {
            return this.VerifyAppendFormat([nixue.GetDateString("yyyy-MM-dd", dateRange.currentDate), nixue.GetDateString("yyyy-MM-dd", dateRange.currentDate)], array);
        },
        getyesterday: function (array) {
            return this.VerifyAppendFormat([nixue.SetDateString(-1, "yyyy-MM-dd"), nixue.SetDateString(-1, "yyyy-MM-dd")], array);
        },
        /**
         *  获取 本周
         * @param {any} array
         */
        getThisWeek: function (array) {
            //返回date是一周中的某一天
            var week = dateRange.currentDate.getDay();
            //减去的天数
            var minusDay = week != 0 ? week - 1 : 6;
            //本周 周一
            var monday = new Date(dateRange.currentDate.getTime() - (minusDay * dateRange.millisecond));
            //本周 周日
            var sunday = new Date(monday.getTime() + (6 * dateRange.millisecond));
            //添加本周时间
            //返回
            return this.VerifyAppendFormat([dateRange.formatterDate(monday), dateRange.formatterDate(sunday)], array);
        },
        /**
         * 获得上一周的起止日期
         */
        getLastWeek: function (array) {
            //返回date是一周中的某一天
            var week = dateRange.currentDate.getDay();
            //减去的天数
            var minusDay = week != 0 ? week - 1 : 6;
            //获得当前周的第一天
            var currentWeekDayOne = new Date(dateRange.currentDate.getTime() - (dateRange.millisecond * minusDay));
            //上周最后一天即本周开始的前一天
            var priorWeekLastDay = new Date(currentWeekDayOne.getTime() - dateRange.millisecond);
            //上周的第一天
            var priorWeekFirstDay = new Date(priorWeekLastDay.getTime() - (dateRange.millisecond * 6));
            //返回
            return this.VerifyAppendFormat([dateRange.formatterDate(priorWeekFirstDay), dateRange.formatterDate(priorWeekLastDay)], array);
        },
        /**
         * 获得这个月的起止日期
         */
        getThisMonth: function (array) {
            //获得当前月份0-11
            var currentMonth = dateRange.currentDate.getMonth();
            //获得当前年份4位年
            var currentYear = dateRange.currentDate.getFullYear();
            //求出本月第一天
            var firstDay = new Date(currentYear, currentMonth, 1);
            //当为12月的时候年份需要加1
            //月份需要更新为0 也就是下一年的第一个月
            //否则只是月份增加,以便求的下一月的第一天
            if (currentMonth == 11) {
                currentYear++;
                currentMonth = 0;
            } else {
                currentMonth++;
            }
            //下月的第一天
            var nextMonthDayOne = new Date(currentYear, currentMonth, 1);
            //求出上月的最后一天
            var lastDay = new Date(nextMonthDayOne.getTime() - dateRange.millisecond);
            //返回
            return this.VerifyAppendFormat([dateRange.formatterDate(firstDay), dateRange.formatterDate(lastDay)], array);
        },
        /**
         * 获得上个月的起止日期
         */
        getLastMonth: function (array) {
            //获得当前月份0-11
            var currentMonth = dateRange.currentDate.getMonth();
            //获得当前年份4位年
            var currentYear = dateRange.currentDate.getFullYear();
            var currentDay = new Date(currentYear, currentMonth, 1);
            //上个月的第一天
            //年份为0代表,是本年的第一月,所以不能减
            if (currentMonth == 0) {
                currentMonth = 11; //月份为上年的最后月份
                currentYear--; //年份减1
            }
            else {
                currentMonth--;
            }
            var firstDay = new Date(currentYear, currentMonth, 1);
            //求出上月的最后一天
            var lastDay = new Date(currentDay.getTime() - dateRange.millisecond);
            //返回
            return this.VerifyAppendFormat([dateRange.formatterDate(firstDay), dateRange.formatterDate(lastDay)], array);
        },
        /**
         * 格式化日期（不含时间）
         */
        formatterDate: function (date) {
            var datetime = date.getFullYear()
                + "-"// "年"
                + ((date.getMonth() + 1) > 10 ? (date.getMonth() + 1) : "0"
                    + (date.getMonth() + 1))
                + "-"// "月"
                + (date.getDate() < 10 ? "0" + date.getDate() : date
                    .getDate());
            return datetime;
        },
        VerifyAppendFormat: function (dates, array) {
            if (!array) {
                array = ['', ''];
            } else {
                array[0] = " " + array[0];
                array[1] = " " + array[1];
            }
            return { StartTime: dates[0] + array[0], EndTime: dates[1] + array[1] };
        }
    }

    var TimedPolling = {
        list: new Array(),
        TimeRefreshInterval: null,
        add: function (obj) {
            if (typeof (TimedPolling.list) != 'object') {
                TimedPolling.empty();
            }
            TimedPolling.list.push(obj);
        },
        empty: function () {
            TimedPolling.list = new Array();
        },
        start: function (options) {
            if (options.on && options.time) {
                if (typeof (TimedPolling.list) == 'object') {
                    if (options.time > 0) {
                        if (TimedPolling.TimeRefreshInterval) {
                            TimedPolling.stop();
                        }
                        TimedPolling.TimeRefreshInterval = window.setInterval(function () {
                            if (TimedPolling.list.length > 0) {
                                if (options.isfor) {
                                    TimedPolling.list.forEach(function (val, i) {
                                        options.on(val, i);
                                    });
                                } else {
                                    options.on(TimedPolling.list);
                                }
                            }
                        }, options.time);
                    }

                }
            }
        },
        stop: function () {
            window.clearInterval(TimedPolling.TimeRefreshInterval);
            TimedPolling.TimeRefreshInterval = null;
        }
    }

    var nixue =
    {
        /** 判断当前页面是否是主页 */
        IsAdmin: false,
        /** 判断当前页面层级 */
        AdminLayers: 0,
        /** 表示当前层页面信息 */
        Window: window,
        /** 获取上一层的信息 */
        BaseNixue: null,
        /** 获取当前页管理的form */
        form: form,
        /** 获取当前页管理的element */
        element: element,
        /** 在注册完了Vue后可用 */
        Vue: null,
        /**
         * 启动Vue
         * @param {any} options 参数
         */
        AddVue: function (options) {
            $("body").hide();
            console.log("正在加载Vue...");
            if (!options.watch) {
                if (options.data.form) {
                    var filter = options.data.form.filter;
                    console.log("开启自动渲染form！");
                    var test = 'options.watch = {';
                    for (var key in options.data.form) {
                        test += ` 'form.${key}': function (val) {
                            var data = { '${key}': val }
                            formval('${key}', data);
                        } , `
                    }
                    test += `}`
                    eval(test);
                    function formval(key, val) {
                        var source = nixue.form.val(filter)[key];
                        if (source != undefined && source != val[key]) {
                            nixue.form.val(filter, val);
                            console.log("Vue渲染", filter + '->form.' + key, '源：', source, '新：', val[key]);
                        }
                    }
                }
            } else {
                console.log("自动渲染form已取消！");
            }
            $.getScript("https://cdn.jsdelivr.net/npm/vue@2.6.12", function () {  //加载 https://cdn.jsdelivr.net/npm/vue/dist/vue.js
                console.log("Vue加载完成");
                nixue.Vue = new Vue(options);
                $("body").show();
            });
        },
        VueForm: function () {
            if (nixue.Vue) {
                if (nixue.Vue.form) {
                    var filter = nixue.Vue.form.filter;
                    var sources = nixue.form.val(filter);
                    var keys = Object.keys(nixue.Vue.form);
                    for (var i = 0; i < keys.length; i++) {
                        if (keys[i] != 'filter') {
                            if (sources[keys[i]]) {
                                var source = nixue.Vue.form[keys[i]], NewVal = sources[keys[i]];
                                if (isNumber(NewVal)) NewVal = parseFloat(NewVal);
                                if (source != undefined && source != NewVal) {
                                    console.log("form 赋值 Vue", filter + '->form.' + keys[i], '源：', source, '新：', NewVal);
                                    nixue.Vue.form[keys[i]] = NewVal;
                                }
                            }
                        }
                    }
                }
            }
        },
        /**
         * 将对象中可用于int类型，自动转换
         * @param {any} obj
         */
        ToInts: function (obj) {
            var keys = Object.keys(obj);
            for (var i = 0; i < keys.length; i++) {
                var source = obj[keys[i]], NewVal;
                if (typeof source != 'number' && isNumber(source)) {
                    NewVal = parseFloat(source);
                    console.log("ToInts:->", keys[i], '源：', source, '新：', NewVal);
                    obj[keys[i]] = NewVal;
                }
            }
            return obj;
        },
        /**
         * 关闭上层框的页面
         * @param {any} name 页面名称
         */
        LayerClose: function (name) {
            layer.close(layer.getFrameIndex(name));
        },
        /**
         * 关闭当前层框的页面
         */
        ThisClose: function () {
            if (nixue.BaseNixue) {
                nixue.BaseNixue.LayerClose(nixue.Window.name);
            }
        },
        /**
         * 自定义带等待动画请求函数
         * @param {any} url
         * @param {any} data
         * @param {any} callback
         * @param {any} error
         * @param {any} isasync
         */
        Psot: function (callback, url, data, error, isasync = true) {
            if (!url) throw ("url 不能为空！");
            var _loadId = layer.load();
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                contentType: "application/x-www-form-urlencoded;charset=utf-8", //"application/json;charset=utf-8", Form/Payload
                dataType: "json",
                async: isasync,//异步     
                success: function (data) {
                    if (data.code === 2001 || data.code === 500) {
                        nixue.alert(data.msg, "rerun");
                        return;
                    } else if (data.code === 2010) {
                        var href = sessionStorage.layuiminiHomeHref;
                        nixue.alert("您没有操作权限，请联系管理员！", href);
                        return;
                    }
                    callback(data);
                    layer.close(_loadId);//layer.closeAll();
                },
                error: function () {
                    typeof error === 'function' && error();
                    console.log('网络错误【' + url + '】！');
                    layer.close(_loadId);
                }
            });
        },
        /** 写Cookie */
        addCookie: function (objName, objValue, objHours) {
            deleteCookie(objName);
            var str = objName + "=" + escape(objValue);
            if (objHours > 0) {//为0时不设定过期时间，浏览器关闭时cookie自动消失
                var date = new Date();
                var ms = objHours * 3600 * 1000;
                date.setTime(date.getTime() + ms);
                str += "; expires=" + date.toGMTString();
            }
            document.cookie = str;
        },
        /** 读Cookie */
        getCookie: function (objName) {//获取指定名称的cookie的值
            var arrStr = document.cookie.split("; ");
            for (var i = 0; i < arrStr.length; i++) {
                var temp = arrStr[i].split("=");
                if (temp[0] === objName) return unescape(temp[1]);
            }
            return "";
        },
        /** 删除指定名称的Cookie */
        deleteCookie: function (name) {
            var date = new Date();
            date.setTime(date.getTime() - 10000);
            document.cookie = name + "=v; expire=" + date.toGMTString();
        },
        /** 无遮挡提示 */
        msg: function (msg, end, icon = 1) {
            layer.msg(msg, { icon: icon }, function () {
                close(end);
            });
        },
        /** 有遮挡提示 */
        alert: function (msg, end) {
            layer.alert(msg, { yes: _close, end: _close });
            function _close(a) {
                if (a) return layer.close(a);
                close(end);
            }
        },
        /** 选择提示 */
        confirm: function (msg, yes, end) {
            layer.confirm(msg, {
                yes: function (index) {
                    if (typeof yes === "function") yes(index);
                }, cancel: _close, end: _close
            });
            function _close(index, layero) { if (typeof end === "function") end(index, layero); }
        },
        /**
         * 将毫秒转换成时间
         * @param {any} _date
         */
        GetTimeString: function (_date) {
            var date = new Date().getTime() - new Date(_date).getTime();
            var days = Math.floor(date / (24 * 3600 * 1000))
            //计算出小时数
            var leave1 = date % (24 * 3600 * 1000)    //计算天数后剩余的毫秒数
            var hours = Math.floor(leave1 / (3600 * 1000))
            //计算相差分钟数
            var leave2 = leave1 % (3600 * 1000)        //计算小时数后剩余的毫秒数
            var minutes = Math.floor(leave2 / (60 * 1000))
            //计算相差秒数
            var leave3 = leave2 % (60 * 1000)      //计算分钟数后剩余的毫秒数
            var seconds = Math.round(leave3 / 1000)
            return days + "天 " + hours + "小时 " + minutes + " 分钟" + seconds + " 秒";
        },
        /** 解析ip地址结果集 */
        GetIpAddress: async function (Ip) {
            if (window.top.queryips == null || window.top.getips == null) {
                window.top.queryips = [];//console.log("queryips");
                window.top.getips = {};//console.log("getips");
            }
            var address = await GetIpMsg(Ip);
            if (address != null) {
                var _ip = `${ToIpAddress(address.Country)}${ToIpAddress(address.Province)}${ToIpAddress(address.City)}${address.Isp}[${Ip}]`;
                return _ip;
            } else {
                return `保留[${Ip}]`;
            }
        },
        /**
         * 获取url中的参数
         * @param {any} name 要获取的值的参数名
         */
        GetQueryString: function (name, _default) {
            var _location = nixue.Window.location;
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = _location.search.substr(1).match(reg);
            if (r !== null) return unescape(r[2]); return _default ? _default : null;
        },
        /**
         * 获取url中的参数
         * @param {any} name 要获取的值的参数名
         */
        GetQueryInt: function (name, _default) {
            var r = nixue.GetQueryString(name, _default);
            if (r !== null) return parseInt(r); return null;
        },
        /**
         * 货币单位为万
         * @param {any} num int
         * @param {any} unit 转换单位
         */
        GetNumFormatter: function (num, unit) {
            var re = 0;
            if (num != '') {
                switch (unit) {
                    case "w":
                        if (parseFloat(num) >= 10000 || parseFloat(num) <= -10000) {
                            re = (Math.round((num / 10000) * 100) / 100) + "万"; //(num / 10000).toFixed(2) + "万"; Math.floor
                        } else {
                            re = Math.round(num * 100) / 100; //num;
                        }
                        break;
                    default:
                        re = Math.round(num * 100) / 100; //Number(num).toFixed(2);
                }
            }
            return re;
        },
        //每4位字符增加’，‘号
        toThousands: function (num) {
            var result = [], counter = 0;
            num = (num || 0).toString().split('');
            for (var i = num.length - 1; i >= 0; i--) {
                counter++;
                result.unshift(num[i]);
                if (!(counter % 4) && i != 0) { result.unshift(','); }
            }
            return result.join('');
        },
        //去掉字符串里面的数字
        RemoveComma: function (num) {
            if (num === 0) {
                return 0;
            }
            return num.replace(/,/g, '');
        },
        //按照有效数字位数进行四舍五入，默认6位有效数字
        signFigures: function (num, rank = 6) {
            if (!num) return (0);
            const sign = num / Math.abs(num);
            const number = num * sign;
            const temp = rank - 1 - Math.floor(Math.log10(number));
            let ans;
            if (temp > 0) {
                ans = parseFloat(number.toFixed(temp));
            }
            else if (temp < 0) {
                ans = Math.round(number / Math.pow(10, temp)) * temp;
            }
            else {
                ans = Math.round(number);
            }
            return (ans * sign);
        },
        /**
         * 获取复选框选中的Id-特定表格
         * @param {any} data
         * @param {any} key
         */
        GetIds: function (data, key) {
            if (data === null || data === undefined || data.length === 0) {
                return null;
            }
            var idlist = '';
            for (var i = 0; i < data.length; i++) {
                if (data[i][key] != null) {
                    idlist = idlist + data[i][key] + ",";
                }
            }
            if (idlist.length > 1) {
                idlist = idlist.substring(0, idlist.length - 1);
            }
            idlist = idlist == '' ? null : idlist;
            return idlist;
        },
        /**
         * 根据时间增加减少时间
         * @param {any} int
         * @param {any} fmt
         * @param {any} date
         */
        SetDateString: function (int, fmt, date) {
            if (typeof date == 'string') date = new Date(date);
            let val = date ? date : new Date();
            val.setDate(val.getDate() + int);
            return val.format(fmt);
        },
        /**
        * 根据时间转换成自定义格式
        * @param {any} fmt
        * @param {any} date
        */
        GetDateString: function (fmt, date) {
            if (typeof date == 'string') date = new Date(date);
            let val = date ? date : new Date();
            return val.format(fmt);
        },
        TableParseData: function (res) { //将原始数据解析成 table 组件所规定的数据
            if (res.code === 2001 || res.code === 500) {
                nixue.alert(res.msg, "rerun");
                return { code: res.code, msg: res.msg, count: 0, data: [] };
            } else if (res.code === 2010) {
                //ltMenuData.RemoveNavid("", getCookie("MenuNavId"));
                var href = sessionStorage.layuiminiHomeHref
                nixue.alert("您没有操作权限，请联系管理员！", href);
                return { code: res.code, msg: res.msg, count: 0, data: [] };
            }
            if (!res.data.list) res.data.list = [];
            return {
                "code": res.code, //解析接口状态
                "msg": res.msg, //解析提示文本
                "count": res.data.RecordCount, //解析数据长度
                "data": res.data.list //解析数据列表
            };
        },
        TableDone: function (options) {//table, data, callback
            if (options) {
                if (!options.table) {
                    return nixue.msg("没有指定table", null, 2)
                }
                if (!options.data) {
                    return nixue.msg("没有指定data", null, 2)
                }
                if (!options.on) {
                    return nixue.msg("没有指定on", null, 2)
                }
                let _table = {
                    header: null,
                    body: null,
                }
                var that = options.table.siblings('div[lay-id="currentTableId"]');
                _table.header = that.find('div.layui-table-header table.layui-table');
                _table.body = that.find('div.layui-table-body.layui-table-main table.layui-table');
                options.data.forEach(function (item, index) {
                    let _tr = _table.body.find(`tr[data-index=${index}]`);
                    if (options.key) {
                        let _td = _tr.find(`td[data-field=${options.key}] div.layui-table-cell`);
                        options.on.call({ table: _table, index, data: item, key: options.key, tr: _tr, td: _td });
                    } else {
                        let __tds = _tr.find(`td`);
                        let _tds = {};
                        for (var i = 0; i < __tds.length; i++) {
                            let _td = $(__tds[i]);
                            Object.defineProperty(_tds, _td.data("field"), {
                                value: _td.children('div.layui-table-cell'),
                                writable: false
                            });
                        }
                        options.on.call({ table: _table, index, data: item, tr: _tr, tds: _tds });
                    }
                });
            }
        },
        TableSort: function (options, table) {
            if (!options.autoSort) {
                var Id = options.elem.attr("id");
                var filter = options.elem.attr("lay-filter");
                table.on(`sort(${filter})`, function (obj) {
                    var _sort = { SortKey: obj.field, SortType: obj.type };
                    //再次重新渲染Table
                    table.reload(Id, {
                        //page: { curr: 1 },
                        where: _sort
                    }, 'data');
                });

            } else {
                return nixue.msg("autoSort非false。", null, 2);
            }
        },
        SetSelect: function (select, Data, Key, Value) {
            return select.SetSelect(Data, Key, Value);
        },
        KeySetSelect: function (select, Data, Key) {
            return select.KeySetSelect(Data, Key);
        },
        TimeRefresh: function (on) {
            TimeRefreshactive.TimeRefreshOut.on = on;
            TimeRefreshactive.setUpopen();
        },
        dateRange: dateRange,
        TimedPolling: TimedPolling,
        SelectIsShow: function (options) {
            if (options.name) {
                var select = $(options.name).parent();//.next('div.layui-form-select');
                if (select.length > 0) {
                    select.removeClass("layui-show layui-hide");
                    if (options.type) {
                        select.addClass("layui-show");
                    } else {
                        select.addClass("layui-hide");
                    }
                }
            }
        },
        Base64ToBytes: function (base64) {
            // 将base64转为Unicode规则编码
            var bstr = atob(base64),
                n = bstr.length,
                u8arr = new Uint8Array(n);
            while (n--) {
                u8arr[n] = bstr.charCodeAt(n) // 转换编码后才可以使用charCodeAt 找到Unicode编码
            }
            return u8arr;
        },
        BytesToBase64: async function (bytes) {
            // 将base64转为Unicode规则编码
            var u8arr = new Uint8Array(bytes),
                blob = new Blob([u8arr]),
                str = await blob.text();
            return btoa(str);
        },
        open: function (options) {
            var keys = Object.keys(options);
            if (!array_search(keys, 'title')) {
                options.title = false;
            }
            if (!array_search(keys, 'type')) {
                options.type = 2;
            }
            if (!array_search(keys, 'shade')) {
                options.shade = 0.2;
            }
            if (!array_search(keys, 'maxmin')) {
                options.maxmin = true;
            }
            if (!array_search(keys, 'shadeClose')) {
                options.shadeClose = true;
            }
            if (!array_search(keys, 'area')) {
                options.area = ['100%', '100%'];
            }
            if (!array_search(keys, 'id')) {
                options.id = new Date().getTime()
            }
            var index = layer.open(options);
            $(window).on("resize", function () {
                layer.full(index);
            });
            return index;
        }
    };

    var TimeRefreshactive = {
        setPercent: function (size) {
            element.progress('percentTimerefresh', size + '%');
        },
        loading: function (Second) {
            var timeOut = 0;
            var millisecond = (Second * 10)
            if (Second > 0) {
                if (TimeRefreshactive.TimeRefreshInterval) {
                    window.clearInterval(TimeRefreshactive.TimeRefreshInterval);
                    TimeRefreshactive.TimeRefreshInterval = null;
                } else {
                    TimeRefreshactive.TimeRefreshInterval = null;
                }
                TimeRefreshactive.TimeRefreshInterval = window.setInterval(function () {
                    timeOut++;
                    TimeRefreshactive.setPercent(timeOut);
                    if (timeOut == 100) {
                        window.setTimeout(function () {
                            TimeRefreshactive.TimeRefreshOut.on();
                            TimeRefreshactive.percentTimerefresh();
                            timeOut = 0;
                        }, millisecond * 2);
                    }
                }, millisecond);
            }
        },
        setUpopen: function () {
            var index = layer.open({
                title: '设置定时刷新',
                type: 1,
                shade: 0.2,
                shadeClose: true,
                area: ['22%', '20%'],
                content: TimeRefreshactive.SetHtml(),
                success: function () {
                    form.val("exampleTimeRefresh", { 'TimeRefresh': TimeRefreshactive.TimeRefreshOut.TimeRefresh, 'open': TimeRefreshactive.TimeRefreshOut.open });
                    TimeRefreshactive.percentTimerefresh();
                }
            });
            $(window).on("resize", function () {
                layer.full(index);
                //if (TimeRefreshactive.TimeRefreshOut.open) {
                //    TimeRefreshactive.loading(TimeRefreshactive.TimeRefreshOut.TimeRefresh);
                //}
            });
            form.on('switch(switchTimeRefresh)', function (data) {
                layer.msg('自动刷新：' + (this.checked ? '启动' : '关闭'), {
                    offset: '6px'
                });
                if (this.checked) {
                    TimeRefreshactive.TimeRefreshOut.TimeRefresh = form.val("exampleTimeRefresh")["TimeRefresh"];
                    if (TimeRefreshactive.TimeRefreshOut.TimeRefresh < 5) {
                        this.checked = false;
                        form.render("checkbox");
                        return nixue.msg('自动刷新：时间不能小于5秒每次！', null, 2);
                    }
                    TimeRefreshactive.loading(TimeRefreshactive.TimeRefreshOut.TimeRefresh);
                } else {
                    window.clearInterval(TimeRefreshactive.TimeRefreshInterval);
                    TimeRefreshactive.TimeRefreshInterval = null;
                    TimeRefreshactive.setPercent(0);
                }
                TimeRefreshactive.TimeRefreshOut.open = this.checked;
                return false;
            });
        },
        TimeRefreshOut: { TimeRefresh: 20, open: false, on: null },
        SetHtml: function () {
            return `<dav>
                <div style="margin: 10px 10px 10px 10px">
                    <form class="layui-form layui-form-pane" action="" lay-filter="exampleTimeRefresh">
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">刷新时间</label>
                                <div class="layui-input-inline">
                                    <input type="number" name="TimeRefresh" placeholder="秒" value="${TimeRefreshactive.TimeRefreshOut.TimeRefresh}" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                                <div class="layui-form-mid">秒</div>
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <label class="layui-form-label">状态</label>
                            <div class="layui-input-block">
                                <input type="checkbox" name="open" ${TimeRefreshactive.TimeRefreshOut.open ? 'checked="checked"' : ''} lay-skin="switch" lay-filter="switchTimeRefresh" lay-text="ON|OFF">
                            </div>
                        </div>
                    </form>
                </div>
            </dav>` },
        TimeRefreshInterval: null,
        percentTimerefresh: function () {
            var temp = $("div.layui-table-tool-temp");
            if (temp.length > 0) {
                var percentTimerefresh = temp.children("div[lay-filter=percentTimerefresh]");
                if (percentTimerefresh.length == 0) {
                    temp.append(`<div class="layui-progress" lay-showpercent="true" lay-filter="percentTimerefresh" style="margin: 3px;"><div class="layui-progress-bar" lay-percent="0%"></div></div>`);
                }
            }
        }
    }

    //数据绑定
    $.fn.SetSelect = function (Data, Key, Value) {
        if (!Data || Data.length === 0) return false;
        for (var j = 0; j < Data.length; j++) {
            $(this).append("<option value='" + Data[j][Key] + "'>" + Data[j][Value] + "</option>");
        }
        return true;
    };

    $.fn.KeySetSelect = function (Data, Key) {
        if (!Data || Data.length === 0) return false;
        return $(this).find("option").each(function (i) {
            let KindId = parseInt($(this).val());
            if (Data[Key] === KindId) {
                $(this).prop("selected", true);//邀请人奖励类型选项选定
            }
        });
    };

    Date.prototype.format = function (fmt) {
        var o = {
            "M+": this.getMonth() + 1, //月份
            "d+": this.getDate(), //日
            "h+": this.getHours(), //小时
            "m+": this.getMinutes(), //分
            "s+": this.getSeconds(), //秒
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度
            "S": this.getMilliseconds() //毫秒
        };
        fmt = fmt || "yyyy-MM-dd";
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    };

    //解析ip地址结果集
    function ToIpAddress(msg) {
        if (msg == '中国') {
            return '';
        } else if (msg.indexOf('省') >= 1) {
            return msg.replace('省', '');
        } else if (msg.indexOf('市') >= 1) {
            return msg.replace('市', '');
        } else {
            return msg;
        }
    }

    //获取ip地址结果集
    function GetIpMsg(Ip) {
        return new Promise(resolve => {
            if (Ip === '0.0.0.0' || Ip === '127.0.0.1' || Ip.startsWith('192.168.') || Ip.trim() === '' || Ip === null) {
                resolve(null);
            } else {//window
                if (window.top.queryips.length === 0) {
                    console.log("OnIp");
                    window.top.queryips.push({ Ip, resolve });
                    window.top.setTimeout(getqueryip, 200);
                } else {
                    console.log("AddOnIp");
                    window.top.queryips.push({ Ip, resolve });
                }
            }
        });
    }

    function getipNew(Ip, resolve) {
        if (window.top.getips[Ip] != null) {
            console.log("GetIps");
            resolve(window.top.getips[Ip]);
            getqueryip();
        } else {
            $.ajax({//console.log("IpMsg");
                type: "get",
                url: "https://apis.juhe.cn/ip/ipNew",
                dataType: 'jsonp',
                async: true, //true
                data: {
                    ip: Ip, key: 'd72c2d917e8ff499ba161277fde083a8'
                },
                success: function (data) { //【成功回调】
                    console.log(data);
                    if (data.resultcode != "200") {
                        resolve(null);
                        getqueryip();
                    } else {
                        window.top.getips[Ip] = data.result;
                        resolve(data.result);
                        getqueryip();
                    }
                },
                error: function (xhr, type) { //【失败回调】
                    resolve(null);
                    getqueryip();
                }
            });
        }
    }

    function getqueryip() {
        var length = window.top.queryips.length;
        if (length === 0) {
            return;
        }
        var onip = window.top.queryips.splice(length - 1)[0];
        getipNew(onip.Ip, onip.resolve);
    }

    function thisreload() {
        location.reload(true);
    }

    function close(end) {
        if (typeof end == "undefined" || end == null) return;
        if (typeof end === "function") {
            end();
        } else if (end === "back") {
            history.back(-1);
        } else if (end === "reload") {
            thisreload();
        } else if (end === "close") {
            nixue.ThisClose();
        } else if (end === "rerun") {
            window.top.onbeforeunload = null;
            // 登录过期的时候，跳出ifram框架
            if (top.location != self.location) {
                top.location.reload(true);
            } else {
                thisreload();
            }
        } else if (end !== "") {
            window.location.href = end;
        }
    }

    function isNumber(val) {
        var regPos = /^\d+(\.\d+)?$/; //非负浮点数
        var regNeg = /^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$/; //负浮点数
        if (regPos.test(val) || regNeg.test(val)) {
            return true;
        } else {
            return false;
        }
    }

    /**
    * js array_searcy() 函数
    * @param array 必选参数 要查找的数组或对象
    * @param find 必须参数 要查找的内容
    * return 未找到要查找的内容则返回false
       找到一个索引/下标则返回该索引/下标
       找到2个以上索引/下标则以数组形式返回所有索引/下标
    */
    function array_search(array, str) {
        if (typeof array !== 'object') {
            return false;
        } else {
            var found = [];
            for (var i in array) {
                if (array[i] == str) {
                    found.push(i);
                }
            }
            var num = found.length;
            if (num == 0) return false;
            if (num == 1) return found[0];
            return found;
        }
    }

    exports("nixue", nixue);
});