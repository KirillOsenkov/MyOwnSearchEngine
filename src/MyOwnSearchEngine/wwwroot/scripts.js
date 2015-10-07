function onPageLoad() {
    searchTimerID = -1;
    lastQuery = null;
    lastSearchString = inputBox.value;

    inputBox.focus();

    inputBox.onkeyup = function () {
        if (this.value != lastSearchString || (event && event.keyCode == 13)) {
            lastSearchString = this.value;
            onSearchChange();
        }
    };
}

function onSearchChange() {
    if (inputBox.value.length > 0) {
        if (searchTimerID == -1) {
            searchTimerID = setTimeout(runSearch, 200);
        }
    }
}

function runSearch() {
    searchTimerID = -1;
    if (typeof lastQuery === "object" && lastQuery !== null) {
        lastQuery.abort();
        lastQuery = null;
    }

    search();
}

function search() {
    var query = "api/answers/?query=" + encodeURIComponent(inputBox.value);

    lastQuery = getUrl(query, loadResults);
}

function getUrl(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", url, true);
    xhr.setRequestHeader("Accept", "text/html");
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            var data = xhr.responseText;
            if (typeof data === "string" && data.length > 0) {
                callback(data);
            }
        }
    };
    xhr.send();
    return xhr;
}

function loadResults(data) {
    var container = document.getElementById("outputDiv");
    if (container) {
        container.innerHTML = data;
    }
}