let selectedFolder = "";
let selectedFile = "";
let dictonary = {};
let icoFolder = "";
let formatsList = "";

$(document).ready(function () {
    console.log("ready");
    TakeExtensions();

    $(".formCancel").click(function(event) {
        $(".inputForm").css("visibility", "hidden");
    });
    
    $("#delete").click(function(event) {
        event.preventDefault();
        if (selectedFolder !== "" && selectedFolder !== 1){
            console.log("Удаляем папку: " + selectedFolder);
            const url = "https://localhost:5001/folder/folderdelete?folderId=" + selectedFolder;
            $.ajax({
                url,
                success: function (message) {
                    alert(message);
                    OpenFolder(1)
                },
                error: function (){
                    alert("Ошбика! Проверь подключение.");
                }
            });
        }
        if (selectedFile !== ""){
            console.log("Удаляем файл: " + selectedFile);
            const url = "https://localhost:5001/file/filedelete?fileId=" + selectedFile;
            $.ajax({
                url,
                success: function (message) {
                    alert(message);
                    OpenFolder(1);
                },
                error: function (){
                    alert("Ошбика! Проверь подключение.");
                }
            });
        }
    });
    
    $("#rename").click(function(event) {
        event.preventDefault();
        if (selectedFile !== ""){
            $("#nameAddForm").css("visibility", "visible");
            let oldName = document.getElementById("fileBtn" + selectedFile).innerText;
            $("#newName").val(oldName.substring(0, oldName.indexOf(".")));
        }
        if (selectedFolder !== ""){
            $("#nameAddForm").css("visibility", "visible");
            let oldName = document.getElementById("folderBtn" + selectedFolder).innerText;
            $("#newName").val(oldName);
        }
    });
    
    $("#nameAddBtn").click(function(event) {
        event.preventDefault();
        if (selectedFolder !== ""){
            console.log("Переименовываем папку: " + selectedFolder);
            const url = "https://localhost:5001/folder/folderrename?folderId=" + selectedFolder + "&newName=" + $("#newName").val();
            $.ajax({
                url,
                success: function (message) {
                    alert(message);
                    OpenFolder(1)
                },
                error: function (){
                    alert("Ошбика! Проверь подключение.");
                }
            });
        }
        if (selectedFile !== ""){
            console.log("Переименовываем файл: " + selectedFile);
            const url = "https://localhost:5001/file/filerename?fileId=" + selectedFile + "&newName=" + $("#newName").val();
            $.ajax({
                url,
                success: function (message) {
                    alert(message);
                    OpenFolder(1);
                },
                error: function (){
                    alert("Ошбика! Проверь подключение.");
                }
            });
        }
        $(".inputForm").css("visibility", "hidden");
    });
    
    $("#folderAdd").click(function(event) {
        event.preventDefault();
        if (selectedFolder !== ""){
            $("#folderAddForm").css("visibility", "visible");
            console.log("создаём в: " + selectedFolder);
        }
    });

    $("#folderAddBtn").click(function(event) {
        const url = "https://localhost:5001/folder/folderadd?folderName=" + $("#newFolderName").val() + "&parfolderId=" + selectedFolder;
        $.ajax({
            url,
            success: function (message) {
                alert(message);
            },
            error: function (){
                alert("Ошбика! Проверь подключение.");
            }
        });
        $(".inputForm").css("visibility", "hidden");
    });
    
    $("#extAdd").click(function(event) {
            $("#extAddForm").css("visibility", "visible"); 
    });
    
    $("#extDel").click(function(event) {
        $("#extDelForm").css("visibility", "visible");
    });

    $("#extDelBtn").click(function(event) {
    const url = "https://localhost:5001/extension/extensiondelete?extensionId=" + $("#selectedExt").val();
        $.ajax({
            url,
            success: function (message) {
                alert(message);
                TakeExtensions();
            },
            error: function (){
                alert("Ошбика! Проверь подключение.");
            }
        });
        $(".inputForm").css("visibility", "hidden");
    });

    $("#fileDownload").click(function(event) {
        event.preventDefault();
        if (selectedFile !== "") {
            const url = "https://localhost:5001/file/filedownload?fileId=" + selectedFile;
            const dummy = document.createElement('a');
            dummy.href = url;
            dummy.download = 'my-filename.ext';
            document.body.appendChild(dummy);
            dummy.click();
        }
    });
    
    $("#fileUpload").click(function(event) {
        event.preventDefault();
        if (selectedFolder !== "") {
            document.getElementById("fileSelectedForm").setAttribute("accept", formatsList);
            $("#fileAddForm").css("visibility", "visible");
        }
    });
    
    $("#fileUploadBtn").click(async function(event) {
        event.preventDefault();
        let pathFile = $('#fileSelectedForm').val();
        let input = document.getElementById("fileSelectedForm");
        let text = "";
        let fileContanier ="";
        let reader = new FileReader();
        reader.onload = function(){
            text = reader.result;
            console.log(text);
            fileContanier = {
                FolderId: selectedFolder,
                FileName: pathFile.substring(pathFile.lastIndexOf("\\")+1),
                FileDescription: $("#descriptionForm").val(),
                FileContent: text
            };
            console.log(fileContanier);
            $.ajax({
                url: 'file/fileupload',
                type: 'POST',
                data: JSON.stringify(fileContanier),
                contentType: "application/json;charset=utf-8",
                success: function (message) {
                    alert(message);
                    OpenFolder(1);
                },
                error: function (){
                    alert("Ошбика! Проверь подключение.");
                }
            });
            $(".inputForm").css("visibility", "hidden");
        };
        reader.readAsText(input.files[0]);
    });
});

function TakeExtensions() {
    const url = "https://localhost:5001/extension/ExtensionsList";
    $.ajax({
        url,
        success: function (responsedList) {
            if (responsedList != null){
                let extList = "<select id=\"selectedExt\">";
                for (let r in responsedList) {
                    dictonary[responsedList[r].extensionId] = responsedList[r].extensionIco;
                    if (responsedList[r].extensionName === "folder")
                        icoFolder = responsedList[r].extensionIco;
                    extList += "<option value=\"" + responsedList[r].extensionId + "\">" + responsedList[r].extensionName  + "</option> ";
                    formatsList += "." + responsedList[r].extensionName + ", ";
                }
                extList += "</select>";
                $("#extListBlock").html(extList);
                console.log("formatList: " + formatsList);
            }
            document.getElementsByClassName("foldersImg").src = "data:image/jpg;base64," + icoFolder;
            console.log(dictonary);
        },
        error: function (){
            alert("Ошбика! Проверь подключение.");
        }
    });
}

