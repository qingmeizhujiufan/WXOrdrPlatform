//把字符串转换为json
function strToJson(strJson) {
    if (strJson == null || strJson == undefined || strJson == "") {
        return {};
    }
    return eval('(' + strJson + ')');
}

var maxSize = 200 * 1024;   //200KB

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
    if (that.files.length > 0) {
        var reader = new FileReader();
        reader.readAsDataURL(that.files[0]);
        reader.onload = function (e) {
            var img = new Image();
            img.src = this.result;;
            var fileContent = this.result;;

            img.onload = function () {
                if (fileContent.length > maxSize)
                    fileContent = compress(this);    //图片压缩

                fileContent = fileContent.substring(fileContent.indexOf(",") + 1);

                var params = {
                    fileContent: fileContent
                };

                $.post('/AdminManage/UpLoadImage', params, function (data) {
                    if (data.success) {
                        var backData = data.backData;
                        console.log("imgbackData===", backData);
                        if (fallback && typeof fallback == 'function')
                            fallback(img.src, backData.strFileName);
                        alert(data.backMsg ? data.backMsg : "上传成功！");
                    } else {
                        alert(data.backMsg ? data.backMsg : "上传失败！");
                    }
                })
            }
        }
    }
}