document.addEventListener("DOMContentLoaded", async function () {
    const urlParams = new URLSearchParams(window.location.search);
    const documentId = urlParams.get("documentId");

    if (documentId) {
        try {
            const response = await fetch(`/api/documents/content?documentId=${documentId}`, {
                method: "GET",
                credentials: "include"
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const data = await response.json();
            document.getElementById("inputText").value = data.text;

            if (data.isReadOnly) {
                document.getElementById("saveButton").style.display = "none";
            }
        } catch (error) {
            console.error("Ошибка загрузки документа:", error);
            alert("Ошибка загрузки документа.");
        }
    }
});

document.getElementById("convertButton").addEventListener("click", async function () {
    const inputText = document.getElementById("inputText").value;

    try {
        const response = await fetch('/api/convert', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Text: inputText })
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        document.getElementById("outputText").value = data.html;
    } catch (error) {
        console.error('Ошибка:', error);
        alert('Ошибка конвертации.');
    }
});

document.getElementById("saveButton").addEventListener("click", async function () {
    const inputText = document.getElementById("inputText").value;
    const urlParams = new URLSearchParams(window.location.search);
    let documentId = urlParams.get("documentId");
    if (documentId == null)
    {
        documentId = 0;
    }

    let fileName;

    if (documentId) {
        fileName = null;
    } else {
        fileName = prompt("Введите имя файла:", "document.txt");
        if (!fileName) {
            alert("Имя файла обязательно.");
            return;
        }
    }

    try {
        const response = await fetch('/api/documents/upload', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                Text: inputText,
                FileName: fileName,
                DocumentId: documentId
            })
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        alert(`Файл успешно сохранен! URL: ${data.Url}`);
    } catch (error) {
        console.error('Ошибка:', error);
        alert('Ошибка сохранения документа.');
    }
});
document.getElementById("docs").addEventListener("click", function () {
    window.location.href = "/documentlist";
});    
