$(document).ready(function () {
    //int
    $('.maskedLong').keydown(function () {
        if (!$(this).regexMask(/^[-+]?\d{0,19}$/)) {
            return;
        }
    });

    //double
    $('.maskedDouble').keydown(function () {
        if (!$(this).regexMask(/^[-+]?\d{0,11}(\.\d{0,4})?$/)) {
            return;
        }
    });

    //date
    jQuery(function ($) {
        $.datepicker.regional['geo'] = {
            closeText: 'დახურვა',
            prevText: 'წინა თვე',
            nextText: 'შემდეგი თვე',
            currentText: 'დღეს',
            monthNames: ['იანვარი', 'თებერვალი', 'მარტი', 'აპრილი', 'მაისი', 'ივნისი', 'ივლისი', 'აგვისტო', 'სექტემბერი', 'ოქტომბერი', 'ნოემბერი', 'დეკემბერი'],
            monthNamesShort: ['იანვ', 'თებ', 'მარტ', 'აპრ', 'მაი', 'ივნ', 'ივლ', 'აგვ', 'სექ', 'ოქტ', 'ნოემ', 'დეკ'],
            dayNames: ['კვირა', 'ორშაბათი', 'სამშაბათი', 'ოთხშაბათი', 'ხუთშაბათი', 'პარასკევი', 'შაბათი'],
            dayNamesShort: ['კვ', 'ორ', 'სამ', 'ოთ', 'ხუთ', 'პარ', 'შაბ'],
            dayNamesMin: ['კვ', 'ორ', 'სამ', 'ოთ', 'ხუთ', 'პარ', 'შაბ'],
            weekHeader: 'არა',
            dateFormat: 'dd.mm.yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['geo']);
    });

    function initDatepicker() {
        $('.maskedDate').datepicker({ dateFormat: 'dd.mm.yy', inline: true });
        $('.maskedDate').attr('readonly', true);

        $('.maskedMonth').datepicker({ dateFormat: 'mm.yy', inline: true });
        $('.maskedMonth').attr('readonly', true);
    }


    $(document).ready(function () {
        try {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        } catch (e) {
            //initDatepicker();
        } finally {
            initDatepicker();
        }
        
        function EndRequestHandler(sender, args) {
            initDatepicker();
        }
    });
});