//扩展easyui表单的验证  
$.extend($.fn.validatebox.defaults.rules, {
    //验证汉字  
    CHS: {
        validator: function (value) {
            return /^[\u0391-\uFFE5]+$/.test(value);
        },
        message: 'The input Chinese characters only.'
    },
    //移动手机号码验证  
    Mobile: {//value值为文本框中的值  
        validator: function (value) {
            var reg = /^1[3|4|5|8|9]\d{9}$/;
            return reg.test(value);
        },
        message: '清输入正确手机号格式.<br>例如“18788888888”'
    },
    
     //电话号码验证  
    Tel: {//value值为文本框中的值  
        validator: function (value) {
            var reg = /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/;
            return reg.test(value);
        },
        message: '清输入正确电话号码格式.<br>例如“021-11111111”'
    },
    //国内邮编验证  
    ZipCode: {
        validator: function (value) {
            var reg = /^[0-9]\d{5}$/;
            return reg.test(value);
        },
        message: 'The zip code must be 6 digits and 0 began.'
    },
    //数字  
    Number: {
        validator: function (value) {
            var reg = /^[0-9]*$/;
            return reg.test(value);
        },
        message: '请输入数字.'
    },
    //合同编号
    ContractNum: {
        validator: function (value) {
            var reg = /^\d{4}-[0-9A-Z]{1}\d{4}$/;
            return reg.test(value);
        },
        message: '请输入正确格式.<br>例如“1301-01001”或者“1301-A1001”！'
    },
    //档案号
    Num: {
        validator: function (value) {
            var reg1 = /^\d{4}-[0-9A-Z]{1}\d{4}-\d{2}-\d{2}$/;
            var reg2 = /^\d{4}-[0-9A-Z]{1}\d{4}-\d{2}-\d{2}-[0-9A-Z]{1}\d{3}$/;
            return reg1.test(value) || reg2.test(value);
        },
        message: '请输入正确格式.<br>例如“1301-01001-01-11、<br>1301-A1001-01-11、<br>1301-A1001-01-11-A101”！'
    },
    //实数  
    Real: {
        validator: function (value) {
            var reg = /^[-+]?\d+(\.\d+)?$/;
            return reg.test(value);
        },
        message: '请输入实数.<br>例如“1,2,3,4,5....”'
    },
    /*必须和某个字段相等*/
    equalTo: {
        validator: function (value, param) {
            return $(param[0]).val() == value;
        },
        message: '字段不匹配'
    }
})