//把字符串转换为json
function strToJson(strJson) {
    if (strJson == null || strJson == undefined || strJson == "") {
        return {};
    }
    return eval('(' + strJson + ')');
}

var maxSize = 5000 * 1024;   //5MB

//图片压缩
var compress = function (img) {
    var canvas = document.createElement('canvas'),
    ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    canvas.width = img.naturalWidth;
    canvas.height = img.naturalHeight;

    //利用canvas进行绘图
    ctx.drawImage(img, 0, 0, canvas.width, canvas.height);

    //将原来图片的质量压缩到原先的0.2倍。
    var data = canvas.toDataURL("image/jpeg", 0.2); //data url的形式
    return data;
}

//图片上传
function fileUpLoad(data, obj, fallback) {
    var that = obj.target;
    console.log('that === ', that);
    if (that.files.length > 0) {
        loadingIn();
        var reader = new FileReader();
        reader.readAsDataURL(that.files[0]);
        reader.onload = function (e) {
            var img = new Image();
            img.src = this.result;
            var fileContent = this.result;

            img.onload = function () {
                if (fileContent.length > maxSize)
                    fileContent = compress(this);    //图片压缩

                fileContent = fileContent.substring(fileContent.indexOf(",") + 1);

                var params = {
                    fileName: that.files[0].name,
                    fileContent: fileContent,
                    fileSize: fileContent.length
                };

                $.post('/AdminManage/UpLoadImage', params, function (data) {
                    if (data.success) {
                        var backData = data.backData;
                        console.log("imgbackData===", backData);
                        if (fallback && typeof fallback == 'function')
                            fallback(img.src, backData);
                        alert(data.backMsg ? data.backMsg : "上传成功！");
                    } else {
                        alert(data.backMsg ? data.backMsg : "上传失败！");
                    }
                    loadingOut();
                })
            }
        }
    }
}

//多图片上传
function multiFilesUpLoad(itemData, obj, fallback) {
    var that = obj.target;
    var files = [];
    var imgPics = [];
    if (that.files.length > 0) {
        loadingIn();
        for (var i = 0; i < that.files.length; i++) {
            var reader = new FileReader();
            reader.readAsDataURL(that.files[i]);
            (function (i) {
                reader.onload = function (e) {
                    var imgInfo = {},
                        result = this.result;
                    imgInfo.src = result;
                    imgPics.push(imgInfo);

                    var img = new Image();
                    img.src = result;
                    var fileContent = result;

                    img.onload = function () {
                        var singleFile = {};
                        if (fileContent.length > maxSize)
                            fileContent = compress(this); //图片压缩
                        singleFile.fileName = that.files[i].name;
                        singleFile.fileSize = fileContent.length;
                        singleFile.fileContent = fileContent.substring(fileContent.indexOf(",") + 1);

                        files.push(singleFile);
                        singleFile = {};

                        if (files.length == that.files.length) {
                            Compost('/AdminManage/MultiUpLoadImage', JSON.stringify(files), function (data) {
                                var backData = data.backData;
                                console.log("imgbackData===", backData);
                                if (fallback && typeof fallback == 'function')
                                    fallback(backData);
                                alert(data.backMsg ? data.backMsg : "上传成功！");
                            })
                        }
                    }
                }
            })(i);
        }
    }
}

//ajax get 方法封装
function Comget(url, params, successFunc, errorFunc) {
    $.ajax({
        url: url,
        data: params,
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                if (typeof successFunc == 'function')
                    successFunc(data);
            } else {
                alert(data.backMsg);
            }
        },
        error: function () {
            if (typeof errorFunc == 'functoin') {
                errorFunc();
            }
        }
    });
}

//ajax post 方法封装
function Compost(url, params, successFunc, errorFunc) {
    $.ajax({
        url: url,
        data: params,
        type: 'POST',
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                successFunc(data);
            } else {
                alert(data.backMsg);
            }
        },
        error: function () {
            if (typeof errorFunc == 'functoin') {
                errorFunc();
            }
        }
    });
}

//时间戳转换为特定字符连接的字串
function dateFormate(stamp, lineStr) {
    var date = new Date(stamp);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var lineStr = lineStr || '/';
    return year + lineStr + month + lineStr + day;
}

//蒙灰层
function loadingIn(text) {
    text = text || '数据加载中...';
    var append_html = '';
    if ($('.zui-loading').length > 0) {
        return false;
    }

    append_html = '<div class="backdrop fade in"></div>'
                  + '<div class="zui-loading">'
                    + '<span class="icon-spinner icon-spin"></span>'
                  + '</div>';
    $('body').append(append_html);
};

function loadingOut(fast) {
    $('.zui-loading').addClass('fade');
    if (arguments.length == 1) {
        $('.zui-loading, .backdrop').remove();
        return;
    }
    setTimeout(function () {
        $('.zui-loading, .backdrop').remove();
    }, 350);
}