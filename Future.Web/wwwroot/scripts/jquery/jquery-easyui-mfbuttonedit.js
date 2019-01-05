(function ($) {
    function buildThis(target) {
        var opts = $.data(target, 'mfbuttonedit').options;
        opts.buttonText = '...';
        $(target).textbox($.extend({}, opts, {
            onChange: function (newValue, oldValue) {
                opts.newValue = newValue;
                opts.oldValue = oldValue;

                if (opts.onChange) {
                    opts.onChange.call(target, data);
                }
            }
        }));

        $(target).next("span").children("a").bind('click',
                function (e) {
                    opts.onButtonClick(opts.newValue);
                });
    }

    $.fn.mfbuttonedit = function (options, param) {
        if (typeof options == 'string') {
            var method = $.fn.mfbuttonedit.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.textbox(options, param);
            }
        }

        options = options || {};
        return this.each(function () {
            var state = $.data(this, 'mfbuttonedit');
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, 'mfbuttonedit', {
                    options: $.extend({}, $.fn.mfbuttonedit.defaults, $.fn.mfbuttonedit.parseOptions(this), options)
                });
            }


            buildThis(this);            
        });
    };

    $.fn.mfbuttonedit.parseOptions = function (target) {
        return $.extend({}, $.fn.textbox.parseOptions(target), {
        });
    };

    $.fn.mfbuttonedit.methods = {
        options: function (jq) {
            var opts = $.data(jq[0], 'mfbuttonedit').options;
            return opts;
        }
    };

    $.fn.mfbuttonedit.defaults = $.extend({}, $.fn.textbox.defaults, {
        newValue: null,
        oldValue: null,
        onButtonClick: function () { alert('Œ¥∂®“Â') }
    });

    ////////////////////////////////
    $.parser.plugins.push('mfbuttonedit');
})(jQuery);