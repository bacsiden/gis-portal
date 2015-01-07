jQuery(document).ready(function () {
    // AutoComplete Chosen select item
    // thêm thẻ <li> cho phân trang    
    addClassForPagging();
    jQuery(".filter-post").change(function () {
        var valuekhuvuc = jQuery(".select-khuvuc").val();
        //alert(valuekhuvuc);
        var valueloai = jQuery(".select-loai").val();
        var valuehinhthuc = jQuery(".select-hinhthuc").val();
        var url = '/Post/ListHinhThuc?' + 'khuvuc=' + valuekhuvuc + '&loaiID=' + valueloai + '&hinhthuc=' + valuehinhthuc;
        location.href = url;

        //        jQuery.ajax({
        //            type: "GET",
        //            url: '/Post/ListHinhThuc',
        //            data: { 'khuvuc': valuekhuvuc, 'loaiID': valueloai, 'hinhthuc': valuehinhthuc },
        //            async: true,
        //            success: function (model) {
        //                jQuery("#wrap-AjaxPaging").html(model);
        //            }
        //        });
    });
    jQuery('#small_slider').carousel({
        interval: 4000
    })
});                         //----------------END DOCUMENT READY FUNCTION -------------------------------  
function AddFile() {
    var vc = jQuery("#viewCount").val();
    vc = parseInt(vc) + 1;
    jQuery("#viewCount").val(vc);
    var txt = "<input type='text' name='imgtitle" + vc + "' style='width: 600px;' placeholder='Tiêu đề ảnh' />";
    jQuery(".ListFiles").append("<div class='span12'><div class='fileinput fileinput-new' data-provides='fileinput'><div class='fileinput-preview fileinput-exists thumbnail' style='max-width: 200px;'></div>" + txt + "<div><span class='btn btn-warning btn-file'><span class='fileinput-new'>Chọn ảnh</span><span class='fileinput-exists'>Thay đổi</span><input type='file' name='files'></span><a href='#' class='btn btn-default fileinput-exists' data-dismiss='fileinput'>Xóa ảnh</a></div></div></div><br />&nbsp;");
}

// ------------------------------------------------------------------
function addClassForPagging() {
    jQuery(".phantrangmvcpager a").wrap("<li></li>");
    jQuery(".phantrangmvcpager li.active").wrapInner("<a href='#' onclick='return false;'></a>");
    jQuery(".phantrangmvcpager a[disabled='disabled']").parent().addClass('disabled');
}

function ajaxPagingByID(wrapID) {

    jQuery(".phantrangmvcpager a[data-ajax='true']").click(function () {
        var url = jQuery(this).attr('href');
        jQuery.ajax({
            type: "GET",
            url: url,
            async: true,
            success: function (model) {
                jQuery("#" + wrapID).html(model);
                addClassForPagging();
                return false;
            }
        });
        return false;
    });
}
function getURLParameter(name) {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null
}
function addParam(url, param, value) {
    var a = document.createElement('a'), regex = /[?&]([^=]+)=([^&]*)/g;
    var match, str = []; a.href = url; value = value || "";
    while (match = regex.exec(a.search))
        if (encodeURIComponent(param) != match[1]) str.push(match[1] + "=" + match[2]);
str.push(encodeURIComponent(param) + "=" + encodeURIComponent(value));
a.search = (a.search.substring(0, 1) == "?" ? "" : "?") + str.join("&");
return a.href;
}