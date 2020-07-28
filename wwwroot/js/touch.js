function SelectFolder(param) {
    $("#fileBtn" + selectedFile).css("background","white");
    $("#folderBtn" + selectedFolder).css("background","white");
    $("#folderBtn" + param).css("background","#bddef6");
    selectedFolder = param;
    selectedFile = "";
    console.log("Selected folder: " + param);
}

function SelectFile(param) {
    $("#folderBtn" + selectedFolder).css("background","white");
    $("#fileBtn" + selectedFile).css("background","white");
    $("#fileBtn" + param).css("background","#bddef6");
    selectedFile = param;
    selectedFolder = "";
    console.log("Selected file: " + param);
}

function CloseFile() {
    $(".txt_redactor").html(" <hr/> <textarea id=\"edit_file\"></textarea>");
}

function SaveFile(fileId) {
    console.log("Saving file: " + fileId);
    let fileContanier = {
        FileId: fileId,
        FileContent: $("#edit_file").val()
    };
    console.log(fileContanier);
    $.ajax({
        url: 'file/filesave',
        type: 'POST',
        data: JSON.stringify(fileContanier),
        contentType: "application/json;charset=utf-8",
        success: function (message) {
            $(".txt_redactor").html("<hr/> <textarea id=\"edit_file\"></textarea>");
            alert(message);
        },
        error: function (){
            alert("Ошбика! Проверь подключение.");
        }
    });
}              

function OpenFile(param) {
    console.log("Open file: " + param);
    $(".txt_redactor").html("<div id=\"openedFileName\"></div>  <hr/> <textarea id=\"edit_file\"></textarea>");
    const url = "https://localhost:5001/file/filecontent?fileId=" + param;
    $.ajax({
        url,
        success: function (responsedFile) {
            $("#edit_file").html(responsedFile.fileContent);
            $("#openedFileName").html(responsedFile.fileName + "<button onclick=CloseFile() class='secondaryBtn'>\u274E</button> <button onclick=SaveFile("+ responsedFile.fileId +") class='secondaryBtn'>\u2705</button>");
        },
        error: function (){
            $("#edit_file").html("Не удалось");
            alert("Ошбика! Проверь подключение.");
        }
    });
}

function OpenFolder(param) {
    let divContent = document.getElementById("folder" + param);
    console.log("Open folder: " + param);
    const url = "https://localhost:5001/folder/foldercontent?folderId=" + param;
    if (divContent.innerText === "")
        $.ajax({
            url,
            success: function (responsedList) {
                let contentString ="";
                for (let r in responsedList.files) {
                    contentString += "<button class =\"filesBtn\" title=\"" + responsedList.files[r].fileDescription + "\" id = \"fileBtn"+ responsedList.files[r].fileId + "\" onclick=SelectFile(" + responsedList.files[r].fileId + ") ondblclick=OpenFile(" + responsedList.files[r].fileId + ")><img style=\"float: left\" src=\"data:image/jpg;base64," + dictonary[responsedList.files[r].extensionId] + "\" width=\"20\" height=\"20\" /> "  + responsedList.files[r].fileName + " </button></br>";
                }
                for (let r in responsedList.folders) {
                    contentString += "<button class =\"foldersBtn\" onclick=SelectFolder(" + responsedList.folders[r].folderId + ") id = \"folderBtn" + responsedList.folders[r].folderId + "\"ondblclick=OpenFolder(" + responsedList.folders[r].folderId + ")><img style=\"float: left\" src=\"data:image/jpg;base64," + icoFolder + "\" width=\"20\" height=\"20\" />"  + responsedList.folders[r].folderName + "</button><div class=\"folder_place\" id=\"folder" + responsedList.folders[r].folderId + "\"></div>";
                }
                $("#folder" + param).html(contentString);
                console.log(responsedList.files);
                console.log(responsedList.folders);
            },
            error: function (){
                alert("Ошбика! Проверь подключение.");
            }
        });
    else {
        $("#folder" + param).html("");
    }
}